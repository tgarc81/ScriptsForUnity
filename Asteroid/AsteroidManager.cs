using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Tyler Garcia
/// Purpose: Manage all the active asteroids in the game, both big and small, detect collision between them and act accordingly
/// Drawbacks: N/A
/// </summary>
public class AsteroidManager : MonoBehaviour
{
    // Prefab 1 is the brown asteroid
    public Asteroid asteroidPrefab1;
    // Prefab 2 is the gray asteroid
    public Asteroid asteroidPrefab2;
    // All the active small asteroids in the game
    private List<Asteroid> smallAsteroids;
    // All the active big asteroids in the game
    private List<Asteroid> bigAsteroids;

    // Main camera in use
    private Camera cam;

    // Height of camera
    private float camHeight;

    // Width of camera
    private float camWidth;

    // Get only for list of active small asteroids
    public List<Asteroid> SmallAsteroids => smallAsteroids;
    // Get only for list of active big asteroids
    public List<Asteroid> BigAsteroids => bigAsteroids;

    // Start is called before the first frame update
    void Start()
    {
        smallAsteroids = new List<Asteroid>();
        bigAsteroids = new List<Asteroid>();
        cam = Camera.main;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        // If there aren't 3 big asteroids in the scene
        if (BigAsteroids.Count < 3)
        {
            // Spawn a big asteroid
            SpawnBigAsteroid();
        }
    }

    /// <summary>
    /// Spawns a big asteroid randomly in the game
    /// </summary>
    public void SpawnBigAsteroid()
    {
        // Spawns in a big asteroid at a random spot in the game
        Asteroid currentAsteroid = Instantiate(asteroidPrefab1, GetRandomSpawnPoint(), Quaternion.identity);
        // Asteroid is given a random direction to make the game unpredictable
        currentAsteroid.Direction = Quaternion.Euler(0, 0, Random.Range(0, 180)) * currentAsteroid.Direction;
        // Adds the newly spawned big asteroid the the list of active big asteroids in the game
        bigAsteroids.Add(currentAsteroid);
    }

    /// <summary>
    /// Spawns a small asteroid in the game
    /// </summary>
    /// <param name="position">The position of the big asteroid that the small asteroids come from</param>
    public void SpawnSmallAsteroid(Vector3 position)
    {
        // Spawns in a small asteroid slightly off the location of the big asteroid it is from
        Asteroid currentAsteroid1 = Instantiate(asteroidPrefab2, new Vector3(position.x, position.y - 0.2f, 0), Quaternion.identity);
        // The small asteroid has a slight angle to its direction
        currentAsteroid1.Direction = Quaternion.Euler(0, 0, 20) * currentAsteroid1.Direction;
        // Spawns in another small asteroid slightly off the location of the big asteroid it is from
        Asteroid currentAsteroid2 = Instantiate(asteroidPrefab2, new Vector3(position.x, position.y + 0.2f, 0), Quaternion.identity);
        // The other small asteroid has a slight angle to its direction, same as the other small asteroid, but the opposite direction
        currentAsteroid2.Direction = Quaternion.Euler(0, 0, -20) * currentAsteroid2.Direction;
        // Adds both of the newly created small asteroids the the list of active small asteroids in the game
        smallAsteroids.Add(currentAsteroid1);
        smallAsteroids.Add(currentAsteroid2);
    }

    /// <summary>
    /// Generates a random position in the game
    /// </summary>
    /// <returns>A random Vector3, representing a position in the game</returns>
    private Vector3 GetRandomSpawnPoint()
    {
        // Random number from 1-4
        float randNum = Random.Range(1, 5);
        // Left border of screen
        float minX = -(camWidth / 2);
        // Right border of screen
        float maxX = (camWidth / 2);
        // Bottom border of screen
        float minY = -(camHeight / 2);
        // Top border of screen
        float maxY = (camHeight / 2);


        // Spawns an asteroid in the top edge of the screen
        if (randNum == 1)
        {
            return new Vector3(Random.Range(minX, maxX), maxY, 0);
        }
        // Spawns an asteroid in the right edge of the screen
        else if (randNum == 2)
        {
            return new Vector3(maxX, Random.Range(minY, maxY), 0);
        }
        // Spawns an asteroid in the bottom edge of the screen
        else if (randNum == 3)
        {
            return new Vector3(Random.Range(minX, maxX), minY, 0);
        }
        // Spawns an asteroid in the left edge of the screen
        else
        {
            return new Vector3(minX, Random.Range(minY, maxY), 0);
        }
    }

}
