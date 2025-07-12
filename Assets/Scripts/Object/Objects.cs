using UnityEngine;
using System.Collections.Generic;

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