using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Tyler Garcia
/// Purpose: Vehicle movement and behavior
/// Drawbacks: N/A
/// </summary>
public class Vehicle : MonoBehaviour
{
    // Position of the vehicle in world space
    private Vector3 vehiclePosition;

    // Lives of vehicle
    private int vehicleLives;

    // Score of player
    private int score = 0;

    // Magnitude of the speed of the vehicle
    public float maxSpeed;

    // Determine the direction of the vehicle
    Vector3 direction = Vector3.right; // = new Vector3(0, 0, 1);
    private Vector3 velocity = Vector3.zero; // = new Vector3(0, 0, 0);

    // Acceleration vector will calculate rate of change in velocity per frame
    private Vector3 acceleration = Vector3.zero;

    // Rate at which the vehicle will acceleration
    private float accelerationRate;

    // Turn speed of vehicle, how fast it turns
    public float turnSpeed;

    // Maps the player's input
    private Vector2 playerInput;

    // Main camera in use
    public Camera cam;

    // Height of camera
    private float camHeight;

    // Width of camera
    private float camWidth;

    // Get only for vehicle position
    public Vector3 VehiclePosition => vehiclePosition;

    // Get only for vehicle direction
    public Vector3 Direction => direction;

    // Get only for velocity of vehicle
    public Vector3 Velocity => velocity;

    // Get and set for player score
    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    // Get and set for player lives
    public int VehicleLives
    {
        get { return vehicleLives; }
        set { vehicleLives = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        vehiclePosition = transform.position;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        vehicleLives = 3;
    }

    // Update is called once per frame
    void Update()
    {
        // If Up arrow is pressed
        if (playerInput.y > 0)
        {
            accelerationRate = 8.0f;
            // Calculate the acceleration vector
            acceleration = direction * accelerationRate;

            // Velocity is velocity plus acceleration
            velocity += acceleration * Time.deltaTime;

            // Clamp the velocity so it never exceeds its max speed
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        }
        else
        {
            // Constant state of deceleration
            accelerationRate = 0.05f;
            // Updates velocity based on deceleration
            velocity = velocity * accelerationRate;
        }

        // If Right arrow is pressed
        if (playerInput.x > 0)
        {
            // Rotate our direction by turn speed each frame
            direction = Quaternion.Euler(0, 0, -turnSpeed * Time.deltaTime) * direction;

            // Calculate the acceleration vector
            acceleration = direction * accelerationRate;
        }

        // If Left arrow is pressed
        else if (playerInput.x < 0)
        {
            // Rotate our direction by turn speed to the left
            direction = Quaternion.Euler(0, 0, turnSpeed * Time.deltaTime) * direction;
        }

        // Add velocity to position
        vehiclePosition += velocity * Time.deltaTime;

        // If the vehicle moves past the left border of the camera
        if(vehiclePosition.x < -(camWidth / 2))
        {
            // Wrap it to the right border of the camera
            vehiclePosition.x = camWidth / 2;
        }

        // If the vehicle moves past the right border of the camera
        if(vehiclePosition.x > camWidth / 2)
        {
            // Wrap it to the left border of the camera
            vehiclePosition.x = -(camWidth / 2);
        }

        // If the vehicle moves past the bottom border of the camera
        if(vehiclePosition.y < -(camHeight / 2))
        {
            // Wrap it to the top border of the camera
            vehiclePosition.y = camHeight / 2;
        }

        // If the vehicle moves past the top border of the camera
        if(vehiclePosition.y > camHeight / 2)
        {
            // Wrap it to the bottom border of the camera
            vehiclePosition.y = -(camHeight / 2);
        }


        // Move the position of the vehicle to the new position
        transform.position = vehiclePosition;

        // Update the vehicle's rotation appropiately
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);


    }

    /// <summary>
    /// Runs whenever the player presses one of the arrow keys
    /// </summary>
    /// <param name="value">The key that the user pressed, an arrow key</param>
    public void OnMove(InputValue value)
    {
        // Gets player's input based on button pressed
        playerInput = value.Get<Vector2>();
    }

    /// <summary>
    /// Affects the ship when it is hit by an asteroid
    /// </summary>
    public void GetHit()
    {
        // If the ship has no more lives
        if(vehicleLives == 0)
        {
            // Loads the game over screen and gives confirmation in log
            Debug.Log("Loading scene: Game over");
            SceneManager.LoadScene("GameOver");
        }
        // Depletes the ship lives by one
        vehicleLives--;
        // Recenters the ship
        vehiclePosition = Vector3.zero;
        transform.position = vehiclePosition;
    }
}
