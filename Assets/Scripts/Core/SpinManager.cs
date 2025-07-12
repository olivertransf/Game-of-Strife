using UnityEngine;
using TMPro;

public class SpinManager : MonoBehaviour
{
    public TMP_Text text;
    
    // Public getter for the final spin result
    public int GetFinalNumber()
    {
        return finalNumber;
    }
    public float spinSpeed = 10f;
    public float spinDuration = 1f; // How long to spin before landing
    
    private bool isSpinning = false;
    private float spinTimer = 0f;
    private int finalNumber;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            spinTimer += Time.deltaTime;
            
            // Calculate progress (0 to 1)
            float progress = spinTimer / spinDuration;
            
            // Slow down the cycling as we approach the end
            float currentSpeed = spinSpeed * (1f - progress * 0.8f);
            
            // Cycle through numbers 1-10
            int currentNumber = Mathf.FloorToInt((Time.time * currentSpeed) % 10) + 1;
            text.text = currentNumber.ToString();
            
            // Check if spinning should stop
            if (spinTimer >= spinDuration)
            {
                StopSpin();
            }
        }
    }

    void StartSpin()
    {
        isSpinning = true;
        spinTimer = 0f;
        finalNumber = Random.Range(1, 11); // Random number 1-10
    }
    
    void StopSpin()
    {
        isSpinning = false;
        text.text = finalNumber.ToString();
    }
    
    // Public method to trigger a new spin
    public void Spin()
    {
        StartSpin();
    }
}
