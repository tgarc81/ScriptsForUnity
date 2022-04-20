using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Author: Tyler Garcia
/// Purpose: Manages collision between the objects in the game
/// Drawbacks: N/A
/// </summary>
public class CollisionManager : MonoBehaviour
{
    // Player ship
    public GameObject ship;
    // Reference to the CollisionDetection script
    public CollisionDetection collisionDetector;
    // Reference to the BulletManager script
    public BulletManager bulletManager;
    // Reference to AsteroidManager in the game
    public AsteroidManager asteroidManager;
    // Whether the ship and asteroids are colliding or not
    private bool isShipColliding;
    // Whether a bullet and asteroid are colliding or not
    private bool isBulletColliding;
    // The bullet that is being collided with
    private GameObject collidedBullet;
    // The asteroid that is being collided with by the ship
    private GameObject collidedAsteroidByShip;
    // The asteroid that is being collided with by the bullet
    private GameObject collidedAsteroidByBullet;

    // Start is called before the first frame update
    void Start()
    {
        isShipColliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Collision detection for the big asteroids
        for(int i = asteroidManager.BigAsteroids.Count - 1; i >= 0; i--)
        {
            // Sees whether the ship is colliding with a big asteroid
            isShipColliding = collisionDetector.ShipCollision(ship, asteroidManager.BigAsteroids[i].asteroidModel);
            // If there is collision
            if(isShipColliding)
            {
                // Gets reference for the big asteroid that hit the ship
                collidedAsteroidByShip = asteroidManager.BigAsteroids[i].asteroidModel;
                // Removes that asteroid from the game
                Destroy(collidedAsteroidByShip);
                // Removes that asteroid from the list of active big asteroids
                asteroidManager.BigAsteroids.RemoveAt(i);
                // The ship is hit and the appropiate behavior is called
                ship.GetComponent<Vehicle>().GetHit();
                break;
            }
            // For every active bullet in the game
            for(int j = bulletManager.Bullets.Count - 1; j >= 0; j--)
            {
                // Sees whether the current bullet is colliding with a big asteroid
                isBulletColliding = collisionDetector.BulletCollision(bulletManager.Bullets[j].bulletModel, asteroidManager.BigAsteroids[i].asteroidModel);
                // If there is collision
                if(isBulletColliding)
                {
                    // If the player shoots a big asteroid, they gain 20 points
                    ship.GetComponent<Vehicle>().Score += 20;
                    // The bullet that is colliding
                    collidedBullet = bulletManager.Bullets[j].bulletModel;
                    // Destroys the collided bullet in the scene
                    Destroy(collidedBullet);
                    // Removes the collided bullet from the list of active bullets in the game
                    bulletManager.Bullets.RemoveAt(j);
                    // Gets reference to the asteroid hit by the bullet
                    collidedAsteroidByBullet = asteroidManager.BigAsteroids[i].asteroidModel;
                    // Spawn in 2 NEW asteroids of the other prefab/model at that position of the collided asteroid
                    asteroidManager.SpawnSmallAsteroid(collidedAsteroidByBullet.transform.position);
                    // Destroys the collided asteroid in the scene
                    Destroy(collidedAsteroidByBullet);
                    // Removes the collided asteroid from the list of active big asteroids in the game
                    asteroidManager.BigAsteroids.RemoveAt(i);
                    break;
                }
            }
            // Prevents unneccesary loops from checking unneccesary asteroids by exiting out of the method the moment the finds a bullet-asteroid collision
            if (isBulletColliding)
            {
                break;
            }
        }

        // Collision detection for the small asteroids
        for (int i = asteroidManager.SmallAsteroids.Count - 1; i >= 0; i--)
        {
            // Sees whether the ship is colliding with a small asteroid
            isShipColliding = collisionDetector.ShipCollision(ship, asteroidManager.SmallAsteroids[i].asteroidModel);
            // If there is collision
            if (isShipColliding)
            {
                // Gets reference for the small asteroid that hit the ship
                collidedAsteroidByShip = asteroidManager.SmallAsteroids[i].asteroidModel;
                // Removes that asteroid from the game
                Destroy(collidedAsteroidByShip);
                // Removes that asteroid from the list of active small asteroids
                asteroidManager.SmallAsteroids.RemoveAt(i);
                // The ship is hit and the appropiate behavior is called
                ship.GetComponent<Vehicle>().GetHit();
                break;
            }
            // For every active bullet in the game
            for (int j = bulletManager.Bullets.Count - 1; j >= 0; j--)
            {
                isBulletColliding = collisionDetector.BulletCollision(bulletManager.Bullets[j].bulletModel, asteroidManager.SmallAsteroids[i].asteroidModel);
                if (isBulletColliding)
                {
                    // If the player shoots a small asteroid, they gain 50 points
                    ship.GetComponent<Vehicle>().Score += 50;
                    // The bullet that is colliding
                    collidedBullet = bulletManager.Bullets[j].bulletModel;
                    // Destroys the collided bullet in the scene
                    Destroy(collidedBullet);
                    // Removes the collided bullet from the list of active bullets in the game
                    bulletManager.Bullets.RemoveAt(j);
                    // Gets reference to the asteroid hit by the bullet
                    collidedAsteroidByBullet = asteroidManager.SmallAsteroids[i].asteroidModel;
                    // Destroys the collided asteroid in the scene
                    Destroy(collidedAsteroidByBullet);
                    // Removes the collided asteroid from the list of active small asteroids in the game
                    asteroidManager.SmallAsteroids.RemoveAt(i);
                    break;
                }
            }
            // Prevents unneccesary loops from checking unneccesary asteroids by exiting out of the method the moment the finds a bullet-asteroid collision
            if (isBulletColliding)
            {
                break;
            }
        }

        // Colors the ship depending on whether it is colliding with an asteroid or not
        if (isShipColliding)
        {
            ship.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else 
        {
            ship.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    /// <summary>
    /// Draws the circle around the ship
    /// </summary>
    private void OnDrawGizmos()
    {
        // If there is collision between a ship and asteroid
        if(isShipColliding)
        {
            // Circle is red
            Gizmos.color = Color.red;
        }
        // If there's no collision
        else 
        {
            // Circle is green
            Gizmos.color = Color.green;
        }
        // Draws circle around ship
        Gizmos.DrawWireSphere(ship.transform.position, ship.GetComponent<SphereCollider>().radius);
    }
}
