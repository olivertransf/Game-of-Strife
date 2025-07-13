using UnityEngine;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;

public class Objects : MonoBehaviour
{
    public List<ActionCard> actionCards;
    public List<Career> careers;
    public List<Baby> babies;
    public List<House> houses;
    public List<Spouse> spouses;
}


[CreateAssetMenu]
public class ActionCard : ScriptableObject {
    public string title;
    public string description;
    public Sprite image;
    public void Execute() {
        // TODO: Implement the function
        Debug.Log("ActionCard executed");
    }
}
[CreateAssetMenu]
public class Career : ScriptableObject {
    public string title;
    public string description;
    public int salary;
    public int rollNumber;  
    public Sprite image;
    public void Execute() {
        // TODO: Implement the function
        Debug.Log("Career executed");
    }
}

[CreateAssetMenu]
public class Baby : ScriptableObject {
    public string title;
    public string description;
    public int age;
    public Sprite image;
    public void Execute() {
        // TODO: Implement the function
        Debug.Log("Baby executed");
    }
}

[CreateAssetMenu]
public class House : ScriptableObject {
    public string title;
    public string description;
    public int price;
    public int redCost;
    public int blackCost;
    public Sprite image;
    public void Execute() {
        // TODO: Implement the function
        Debug.Log("House executed");
    }
}

[CreateAssetMenu]
public class Spouse : ScriptableObject {
    public string title;
    public string description;
    public Sprite image;
    public void Execute() {
        // TODO: Implement the function
        Debug.Log("Spouse executed");
    }
}

[CreateAssetMenu(fileName = "NewPlayer", menuName = "PlayerObject")]
public class PlayerObject : ScriptableObject
{
    // Identity
    public string title;
    public int order;
    public string description;
    public Sprite image;

    // Personal Info
    public int age;
    public bool isMale;

    // Employment & Career
    public bool isEmployed;
    public int salary;
    public List<Career> careers;
    public int careerCount => careers?.Count ?? 0;

    // Family
    public List<Spouse> spouses;
    public List<Baby> babies;
    public int babyCount => babies?.Count ?? 0;

    // Housing & Transport
    public List<House> houses;
    public int houseCount => houses?.Count ?? 0;
    public int carCount;
    public bool hasCar => carCount > 0;
    public bool hasHouse => houseCount > 0;

    // Cards & Actions
    public List<ActionCard> actionCards;
    public int actionCardCount => actionCards?.Count ?? 0;

    // Financials
    public int money;
    public int debt;
    public List<Investment> investments;

    // Game Mechanics
    public int[] diceRolls;
    public int rollNumber;
    public int speedModifier;

    // Status Flags
    public bool isMarried;
    public bool hasBaby => babyCount > 0;
    public bool hasCareer => careerCount > 0;
    public bool isRetired;
    public bool isDead;
    public bool isJailed;
    public bool isQuarantined;

    // Special Roles
    public bool isSnake;
    public bool isBee;
    public bool isBacteria;
}

public class Investment : ScriptableObject {
    public int value;
    public int price;
    public Vector3Int location;
    public Sprite image;
}