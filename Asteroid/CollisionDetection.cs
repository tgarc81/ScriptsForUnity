using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Tyler Garcia
/// Purpose: Detects collsions between objects in the game
/// Drawbacks: N/A
/// </summary>
public class CollisionDetection : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Checks to see if the ship and an asteroid are colliding via Bounding Circles algorithm
    /// </summary>
    /// <param name="ship">The player ship</param>
    /// <param name="asteroid">An asteroid</param>
    /// <returns>True or false depending on if the two objects are colliding or not</returns>
    public bool ShipCollision(GameObject ship, GameObject asteroid)
    {
        // Ship center, x coord
        float shipCenterX = ship.GetComponent<SphereCollider>().bounds.center.x;
        // Ship center, y coord
        float shipCenterY = ship.GetComponent<SphereCollider>().bounds.center.y;
        // Asteroid center, x coord
        float asteroidCenterX = asteroid.GetComponent<SphereCollider>().bounds.center.x;
        // Asteroid center, y coord
        float asteroidCenterY = asteroid.GetComponent<SphereCollider>().bounds.center.y;
        // Ship radius
        float shipRadius = ship.GetComponent<SphereCollider>().radius;
        // Asteroid radius
        float asteroidRadius = asteroid.GetComponent<SphereCollider>().radius;

        // Distance between ship and asteroid's center squared
        float distanceSquared = Mathf.Pow((shipCenterX - asteroidCenterX), 2) + Mathf.Pow((shipCenterY - asteroidCenterY), 2);

        // Collision condition: if the sum of both radii squares is greater than the distance between their centers squared
        if (Mathf.Pow(shipRadius + asteroidRadius, 2) > distanceSquared)
        {
            // There is collision
            return true;
        }
        // If condition isn't met
        else 
        {
            // No collision
            return false;
        }
    }

    /// <summary>
    /// Checks to see if a bullet and an asteroid are colliding via Bounding Circles algorithm
    /// </summary>
    /// <param name="bullet">A bullet</param>
    /// <param name="asteroid">An asteroid</param>
    /// <returns>True or false depending on if the two objects are colliding or not</returns>
    public bool BulletCollision(GameObject bullet, GameObject asteroid)
    {
        // Bullet center, x coord
        float bulletCenterX = bullet.GetComponent<SphereCollider>().bounds.center.x;
        // Bullet center, y coord
        float bulletCenterY = bullet.GetComponent<SphereCollider>().bounds.center.y;
        // Asteroid center, x coord
        float asteroidCenterX = asteroid.GetComponent<SphereCollider>().bounds.center.x;
        // Asteroid center, y coord
        float asteroidCenterY = asteroid.GetComponent<SphereCollider>().bounds.center.y;
        // Asteroid radius
        float asteroidRadius = asteroid.GetComponent<SphereCollider>().radius;

        // Distance between bullet and asteroid's center squared
        float distanceSquared = Mathf.Pow((bulletCenterX - asteroidCenterX), 2) + Mathf.Pow((bulletCenterY - asteroidCenterY), 2);

        // Collision condition: if the radius of the asteroid is greater than the distance between the bullet and asteroid's centers squared
        if (Mathf.Pow(asteroidRadius, 2) > distanceSquared)
        {
            // There is collision
            return true;
        }
        // If condition isn't met
        else
        {
            // No collision
            return false;
        }
    }
}
