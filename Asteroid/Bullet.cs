using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Tyler Garcia
/// Purpose: Controls the bullet's direction and screen-wrapping
/// Drawbacks: N/A
/// </summary>
public class Bullet : MonoBehaviour
{
    // Model of bullet
    public GameObject bulletModel;
    // Direction of bullet
    private Vector3 direction = Vector3.right;
    // Velocity of bullet
    private Vector3 velocity = Vector3.zero;
    // Position of bullet
    private Vector3 position;

    // The time the bullet has been active for
    private float timeActive = 2.0f;

    // Main camera in use
    private Camera cam;

    // Height of camera
    private float camHeight;

    // Width of camera
    private float camWidth;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
    }

    // Update is called once per frame
    void Update()
    {

        // Velocity is velocity plus acceleration
        velocity = direction * 10.0f * Time.deltaTime;

        position = transform.position;
        // Add velocity to position
        position += velocity;

        // If the vehicle moves past the left border of the camera
        if (position.x < -(camWidth / 2))
        {
            // Wrap it to the right border of the camera
            position.x = camWidth / 2;
        }

        // If the vehicle moves past the right border of the camera
        if (position.x > camWidth / 2)
        {
            // Wrap it to the left border of the camera
            position.x = -(camWidth / 2);
        }

        // If the vehicle moves past the bottom border of the camera
        if (position.y < -(camHeight / 2))
        {
            // Wrap it to the top border of the camera
            position.y = camHeight / 2;
        }

        // If the vehicle moves past the top border of the camera
        if (position.y > camHeight / 2)
        {
            // Wrap it to the bottom border of the camera
            position.y = -(camHeight / 2);
        }

        // Move the position of the vehicle to the new position
        transform.position = position;
        // Updates the rotation accordingly
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }

    /// <summary>
    /// Sets the appropiate rotation and position of the new bullet based on the ship
    /// </summary>
    /// <param name="direction">The ship's direction</param>
    /// <param name="position">The ship's position</param>
    public void FireBullet(Vector3 direction, Vector3 position)
    {
        this.direction = direction;
        this.position = position;
    }

    /// <summary>
    /// Sees whether enough time has passed for the bullet to despawn
    /// </summary>
    /// <param name="time">The time passed inbetween frames</param>
    /// <returns>True or false whether it is okay to despawn the bullet yet or not</returns>
    public bool Despawn(float time)
    {
        // Updates the timer
        timeActive -= time;
        // If enough time has passed
        if(timeActive < 0.5f)
        {
            // Return that it is okay to despawn the bullet
            return true;
        }
        else
        {
            // Return that it is NOT okay to despawn the bullet
            return false;
        }
    }
}
