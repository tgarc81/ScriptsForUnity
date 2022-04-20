using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Tyler Garcia
/// Purpose: Control the movement of the asteroids and the screen-wrapping
/// Drawbacks: N/A
/// </summary>
public class Asteroid : MonoBehaviour
{
    // Asteroid Model
    public GameObject asteroidModel;
    // Asteroid direction
    private Vector3 direction = Vector3.right;
    // Asteroid velocity
    private Vector3 velocity = Vector3.zero;
    // Asteroid position
    private Vector3 position;
    // Main camera in use
    private Camera cam;
    // Height of camera
    private float camHeight;
    // Width of camera
    private float camWidth;
    // Get only for asteroid position
    public Vector3 Position => position;

    // Get and set for asteroid direction
    public Vector3 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

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
        // Constant velocity of asteroid is calculated
        velocity = direction * 5.0f * Time.deltaTime;
        // Gets the temp position variable from its position in the game
        position = transform.position;
        // Updates the position via velocity
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
        // Updates the rotation of the asteroid appropiately
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }
}
