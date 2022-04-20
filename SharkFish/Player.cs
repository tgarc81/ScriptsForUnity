using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Shark
{
    // Maps the player's input
    private Vector2 playerInput;

    public override void CalculateSteeringForces()
    {
        maxForce = 3f;

        List<Human> food = manager.humans;

        Vector3 ultimateForce = Vector3.zero;

        ultimateForce += PlayerMovement();

        ultimateForce += Seperate(manager.sharks);

        ultimateForce += StayInBounds();

        ultimateForce += AvoidAllObstacles(manager.obstacles);

        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);

        ApplyForce(ultimateForce);
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

    public Vector3 PlayerMovement()
    {
        Vector3 movementForce = Vector3.zero;

        // If Up arrow is pressed
        if (playerInput.y > 0)
        {
            movementForce += Seek(new Vector3(position.x, position.y, position.z + 0.7f));
        }

        // If Down arrow is pressed
        if (playerInput.y < 0)
        {
            movementForce += Seek(new Vector3(position.x, position.y, position.z - 0.7f));
        }

        // If Right arrow is pressed
        if (playerInput.x > 0)
        {
            movementForce += Seek(new Vector3(position.x + 0.7f, position.y, position.z));
        }

        // If Left arrow is pressed
        else if (playerInput.x < 0)
        {
            movementForce += Seek(new Vector3(position.x - 0.7f, position.y, position.z));
        }

        return movementForce;
    }
}
