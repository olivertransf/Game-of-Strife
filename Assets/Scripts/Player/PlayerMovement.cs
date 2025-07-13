using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private SpinManager spinManager;
    [SerializeField] private int currentPathIndex = 0;
    [SerializeField] private float moveSpeed = 2f;
    
    private bool isMoving = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find BoardManager if not assigned
        if (boardManager == null)
        {
            boardManager = FindFirstObjectByType<BoardManager>();
        }
        
        // Find Tilemap if not assigned
        if (tilemap == null)
        {
            tilemap = FindFirstObjectByType<Tilemap>();
        }
        
        // Find SpinManager if not assigned
        if (spinManager == null)
        {
            spinManager = FindFirstObjectByType<SpinManager>();
        }
        
        // Position player at starting position
        if (boardManager != null && boardManager.GetPathPositions().Count > 0)
        {
            Vector3 startPos = boardManager.GetPathPositions()[currentPathIndex];
            // Convert to world position if using cell positions
            if (tilemap != null)
            {
                Vector3Int cellPos = new Vector3Int(Mathf.RoundToInt(startPos.x), Mathf.RoundToInt(startPos.y), Mathf.RoundToInt(startPos.z));
                transform.position = tilemap.GetCellCenterWorld(cellPos);
            }
            else
            {
                transform.position = startPos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// Public method to move one space forward - can be called from buttons
    /// </summary>
    public void MoveOneSpace()
    {
        if (isMoving || boardManager == null) return;
        
        var pathPositions = boardManager.GetPathPositions();
        if (pathPositions.Count == 0) return;
        
        // Check if we're at the last index
        if (currentPathIndex >= pathPositions.Count - 1)
        {
            Debug.Log("Player has reached the end of the path!");
            return;
        }
        
        // Move to next position
        currentPathIndex++;
        Vector3 targetCellPos = pathPositions[currentPathIndex];
        StartCoroutine(MoveToCellPosition(targetCellPos));
    }
    
    /// <summary>
    /// Spin and move based on the spin result
    /// </summary>
    public void SpinAndMove()
    {
        if (isMoving || boardManager == null || spinManager == null) return;
        
        // Start the spin
        spinManager.Spin();
        
        // Wait for spin to complete, then move
        StartCoroutine(WaitForSpinAndMove());
    }
    
    /// <summary>
    /// Waits for spin to complete, then moves the player
    /// </summary>
    private IEnumerator WaitForSpinAndMove()
    {
        // Wait for spin duration plus a small buffer
        yield return new WaitForSeconds(spinManager.spinDuration + 0.1f);
        
        // Get the final spin number (1-10)
        // int spinResult = GetSpinResult();
        
        // Move the player based on spin result
        MoveSpaces(1);
    }
    
    /// <summary>
    /// Get the current spin result from the SpinManager
    /// </summary>
    private int GetSpinResult()
    {
        if (spinManager != null)
        {
            return spinManager.GetFinalNumber();
        }
        
        // Fallback to random number 1-10 if we can't get the actual result
        return Random.Range(1, 11);
    }
    
    /// <summary>
    /// Smoothly moves the player to the target cell position
    /// </summary>
    private IEnumerator MoveToCellPosition(Vector3 targetCellPos)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 targetWorldPos;
        
        // Convert cell position to world position
        if (tilemap != null)
        {
            Vector3Int cellPos = new Vector3Int(Mathf.RoundToInt(targetCellPos.x), Mathf.RoundToInt(targetCellPos.y), Mathf.RoundToInt(targetCellPos.z));
            targetWorldPos = tilemap.GetCellCenterWorld(cellPos);
        }
        else
        {
            targetWorldPos = targetCellPos;
        }
        
        float journey = 0f;
        
        while (journey <= 1f)
        {
            journey += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPosition, targetWorldPos, journey);
            yield return null;
        }
        
        transform.position = targetWorldPos;
        isMoving = false;
        
        // Log the color of the tile we landed on
        LogLandedTileColor();
    }
    
    /// <summary>
    /// Optional: Method to move multiple spaces (useful for dice rolls)
    /// </summary>
    public void MoveSpaces(int spaces)
    {
        if (isMoving || boardManager == null || spaces <= 0) return;
        
        StartCoroutine(MoveMultipleSpaces(spaces));
    }
    
    /// <summary>
    /// Moves the player multiple spaces with consistent timing
    /// </summary>
    private IEnumerator MoveMultipleSpaces(int spaces)
    {
        var pathPositions = boardManager.GetPathPositions();
        
        // Calculate how many spaces we can actually move
        int maxSpaces = Mathf.Min(spaces, pathPositions.Count - 1 - currentPathIndex);
        
        if (maxSpaces <= 0)
        {
            Debug.Log("Player has reached the end of the path!");
            yield break;
        }
        
        // Create a smooth path through all target positions
        Vector3 startPosition = transform.position;
        Vector3[] targetPositions = new Vector3[maxSpaces];
        
        for (int i = 0; i < maxSpaces; i++)
        {
            Vector3 targetCellPos = pathPositions[currentPathIndex + i + 1];
            
            // Convert cell position to world position
            if (tilemap != null)
            {
                Vector3Int cellPos = new Vector3Int(Mathf.RoundToInt(targetCellPos.x), Mathf.RoundToInt(targetCellPos.y), Mathf.RoundToInt(targetCellPos.z));
                targetPositions[i] = tilemap.GetCellCenterWorld(cellPos);
            }
            else
            {
                targetPositions[i] = targetCellPos;
            }
        }
        
        // Move through all positions smoothly
        float totalDistance = 0f;
        for (int i = 0; i < targetPositions.Length; i++)
        {
            Vector3 currentPos = (i == 0) ? startPosition : targetPositions[i - 1];
            totalDistance += Vector3.Distance(currentPos, targetPositions[i]);
        }
        
        float journey = 0f;
        float speed = totalDistance / (maxSpaces * (1f / moveSpeed)); 
        
        while (journey <= 1f)
        {
            journey += Time.deltaTime * speed;
            float clampedJourney = Mathf.Clamp01(journey);
            
            // Calculate position along the path
            Vector3 newPosition = CalculatePositionAlongPath(startPosition, targetPositions, clampedJourney);
            transform.position = newPosition;
            
            yield return null;
        }
        
        // Update current path index
        currentPathIndex += maxSpaces;
        transform.position = targetPositions[targetPositions.Length - 1];
        
        Debug.Log($"Moved {maxSpaces} spaces out of {spaces} requested.");
        
        // Log the color of the tile we landed on
        LogLandedTileColor();
        
        // Check if we reached the end
        if (currentPathIndex >= pathPositions.Count - 1)
        {
            Debug.Log("Player has reached the end of the path!");
        }
    }
    
    /// <summary>
    /// Calculate position along a path of waypoints
    /// </summary>
    private Vector3 CalculatePositionAlongPath(Vector3 start, Vector3[] waypoints, float t)
    {
        if (waypoints.Length == 0) return start;
        
        float totalLength = Vector3.Distance(start, waypoints[0]);
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            totalLength += Vector3.Distance(waypoints[i], waypoints[i + 1]);
        }
        
        float targetDistance = totalLength * t;
        float currentDistance = 0f;
        
        // Check if we're between start and first waypoint
        float segmentLength = Vector3.Distance(start, waypoints[0]);
        if (targetDistance <= segmentLength)
        {
            float segmentT = targetDistance / segmentLength;
            return Vector3.Lerp(start, waypoints[0], segmentT);
        }
        currentDistance += segmentLength;
        
        // Check intermediate waypoints
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            segmentLength = Vector3.Distance(waypoints[i], waypoints[i + 1]);
            if (targetDistance <= currentDistance + segmentLength)
            {
                float segmentT = (targetDistance - currentDistance) / segmentLength;
                return Vector3.Lerp(waypoints[i], waypoints[i + 1], segmentT);
            }
            currentDistance += segmentLength;
        }
        
        // If we get here, we're at the end
        return waypoints[waypoints.Length - 1];
    }
    
    /// <summary>
    /// Get the current position index on the path
    /// </summary>
    public int GetCurrentPathIndex()
    {
        return currentPathIndex;
    }
    
    /// <summary>
    /// Check if player has reached the end of the path
    /// </summary>
    public bool HasReachedEnd()
    {
        if (boardManager == null) return false;
        var pathPositions = boardManager.GetPathPositions();
        return currentPathIndex >= pathPositions.Count - 1;
    }
    
    /// <summary>
    /// Set the current position on the path (useful for loading game state)
    /// </summary>
    public void SetCurrentPathIndex(int index)
    {
        if (boardManager == null) return;
        
        var pathPositions = boardManager.GetPathPositions();
        if (index >= 0 && index < pathPositions.Count)
        {
            currentPathIndex = index;
            Vector3 targetCellPos = pathPositions[currentPathIndex];
            
            // Convert to world position if using cell positions
            if (tilemap != null)
            {
                Vector3Int cellPos = new Vector3Int(Mathf.RoundToInt(targetCellPos.x), Mathf.RoundToInt(targetCellPos.y), Mathf.RoundToInt(targetCellPos.z));
                transform.position = tilemap.GetCellCenterWorld(cellPos);
            }
            else
            {
                transform.position = targetCellPos;
            }
        }
    }
    
    /// <summary>
    /// Get the current cell position the player is on
    /// </summary>
    public Vector3Int GetCurrentCellPosition()
    {
        if (tilemap != null)
        {
            return tilemap.WorldToCell(transform.position);
        }
        return Vector3Int.zero;
    }
    
    /// <summary>
    /// Log the color of the tile the player landed on
    /// </summary>
    private void LogLandedTileColor()
    {
        if (tilemap == null) return;
        
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        TileBase tile = tilemap.GetTile(cellPosition);
        
        if (tile != null)
        {
            string tileName = tile.name;
            Debug.Log($"Player landed on tile: {tileName}");
        }
        else
        {
            Debug.Log("Player landed on empty space");
        }
    }
    
}
