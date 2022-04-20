using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Author: Tyler Garcia
/// Purpose: Manage all the active bullets in the game, both big and small, detect collision between them and act accordingly
/// Drawbacks: N/A
/// </summary>
public class BulletManager : MonoBehaviour
{
    // Reference to the player ship in the game
    public Vehicle ship;
    // Bullet prefab
    public Bullet bulletPrefab;

    // List of all the bullets in the game
    private List<Bullet> bullets;

    // Get only property for list of bullets
    public List<Bullet> Bullets => bullets;

    /// <summary>
    /// Start is called before the first update
    /// </summary>
    void Start()
    {
        bullets = new List<Bullet>();
    }

    // Update is called once per frame
    void Update()
    {
        // Gets the time between each Update
        float time = Time.deltaTime;
        // For every active bullet
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            // If the bullet should despawn
            if(bullets[i].Despawn(time))
            {
                // Erase bullet GameObject to be despawned
                Destroy(bullets[i].gameObject);
                // Remove despawned bullet from the active bullets list
                bullets.RemoveAt(i); 
            }
        }
    }

    /// <summary>
    /// Creates a bullet when the ship shoots a bullet
    /// </summary>
    /// <param name="value">Value for player input</param>
    private void OnShoot(InputValue value)
    {
        // Use a local variable to keep track of the bullet added to the list
        // The current bullet being added to the list
        // Instantiates in scene
        Bullet currentBullet = Instantiate(bulletPrefab, ship.transform.position, ship.transform.rotation);
        // Sets position and direction to same as ship   
        currentBullet.FireBullet(ship.Direction, ship.transform.position);
        // Adds bullets to the list
        bullets.Add(currentBullet);
    }
}
