using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Tyler Garcia
/// Purpose: Switch from the GameOver scene to the main game scene
/// Drawbacks: N/A
/// </summary>
public class GameOverManager : MonoBehaviour
{
    /// <summary>
    /// Changes scene back to main game
    /// </summary>
    /// <param name="value">Value for input</param>
    private void OnReplay(InputValue value)
    {
        // Changes scene to main game with confirmation in log
        Debug.Log("Loading scene: Main game");
        SceneManager.LoadScene("CollisionTest");
    }
}
