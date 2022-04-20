using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Tyler Garcia
/// Purpose: Manages text in main game UI
/// Drawbacks: N/A
/// </summary>
public class TextManager : MonoBehaviour
{
    // Reference to the text on screen
    public Text text;
    // Reference to the ship
    public Vehicle ship;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Updates the text in UI based on amount of lives remaining and the player's score
        text.text = $"Lives Left: {ship.VehicleLives}" +
            $"\nScore: {ship.Score}";
    }
}
