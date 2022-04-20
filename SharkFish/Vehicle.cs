using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Tyler Garcia
/// Purpose: To manage the force application and movement of each monster
/// Drawbacks: N/A
/// </summary>
public abstract class Vehicle : MonoBehaviour
{
    protected Vector3 position;
    protected Vector3 direction;
    protected Vector3 velocity;
    protected Vector3 acceleration;
    protected bool hasFriction;
    protected float coeff = 0.04f;
    protected GameObject floor;
    protected Manager manager;
    float minX;
    float maxX;
    float minZ;
    float maxZ;

    protected float randomAngle;

    private float radius;

    public float Radius => radius;

    public MeshRenderer mesh;

    private Vector3 forward = Vector3.forward;
    private Vector3 right = Vector3.right;
    public float obstacleViewDistance = 3f;

    // Main camera in use
    protected Camera cam;

    // Height of camera
    protected float camHeight;

    // Width of camera
    protected float camWidth;


    [SerializeField]
    [Min(0.001f)]
    protected float mass = 1f;

    [SerializeField]
    [Tooltip("The radius around the vehicle, where they should seperate from other vehicles")]
    private float personalSpace = 1f;

    public float maxSpeed = 2f;

    public float maxForce = 2f;

    public float Mass
    {
        get { return mass; }
        set { mass = value; }
    }

    public bool HasFriction
    {
        get { return hasFriction; }
        set { hasFriction = value; }
    }

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    // Start is called before the first frame update
    protected void Start()
    {
        cam = Camera.main;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        position = transform.position;
        hasFriction = false;
        floor = GameObject.Find("Plane");
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        minX = (floor.transform.position.x - (floor.transform.localScale.x / 2)) * 10;
        maxX = (floor.transform.position.x + (floor.transform.localScale.x / 2)) * 10;
        minZ = (floor.transform.position.z - (floor.transform.localScale.z / 2)) * 10;
        maxZ = (floor.transform.position.z + (floor.transform.localScale.z / 2)) * 10;
        radius = mesh.bounds.extents.x;
        randomAngle = Random.Range(0, 360);
    }

    // Update is called once per frame
    protected void Update()
    {
        CalculateSteeringForces();

        // This is needed for all vehicles
        
        UpdatePosition();
        SetTransform();
    }

    /// <summary>
    /// Applies a given force to affect the vehicle's acceleration
    /// </summary>
    /// <param name="force">Force to be applied and act on this vehicle</param>
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    /// <summary>
    /// Updates the position of the vehicle using force-based movement
    /// </summary>
    private void UpdatePosition()
    {
        // Step 1: add acceleration to velocity * time
        velocity += acceleration * Time.deltaTime;

        // Don't worry about the y axis, we're just on a plane on the ground
        velocity.y = 0;

        // Step 2: add our velocity to our position
        position += velocity * Time.deltaTime;

        // Don't worry about the y axis, we're just on a plane on the ground
        position.y = 0;

        // Step 3: reset our acceleration and set our direction vector
        if (velocity != Vector3.zero)
        {
            direction = velocity.normalized;

            forward = direction;
            right = Vector3.Cross(forward, Vector3.up);
        }
        acceleration = Vector3.zero;
    }
    
    /// <summary>
    /// Sets the vehicle's transform to the position set in UpdatePosition()
    /// </summary>
    private void SetTransform()
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    /// <summary>
    /// Gets the predicted position when this vehicle should be in x seconds
    /// </summary>
    /// <param name="seconds">How many seconds ahead to look</param>
    /// <returns>The predicted future position</returns>
    public Vector3 GetFuturePosition(float seconds)
    {
        return position + (velocity * seconds);
    }


    /// <summary>
    /// Applies a frictional force in the opposite direction of velocity, based on a coefficient
    /// </summary>
    /// <param name="coeff">The coefficient of friction</param>
    public void ApplyFriction(float coeff)
    {
        Vector3 friction = velocity.normalized * -1;

        friction *= coeff;

        acceleration += friction;
    }

    /// <summary>
    /// Applies a gravitational force in the negative y direction
    /// </summary>
    /// <param name="gravityForce"></param>
    public void ApplyGravity(Vector3 gravityForce)
    {
        acceleration += gravityForce;
    }

    ///// <summary>
    ///// Applies a force from mouse's position to the monster
    ///// </summary>
    //public void ApplyMouseForce()
    //{
    //    // Convert the position of the mouse from screen space (origined at lower left corner) to world space, relative to the world's origin (0, 0, 0)
    //    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    //    mouseWorldPos.z = 0;
    //    Debug.Log(mouseWorldPos.z);

    //    Vector3 force = position - mouseWorldPos;

    //    force = force.normalized;

    //    // Gets the angle that is the monster pointing to the user's cursor

    //    ApplyForce(force * 0.07f * Time.deltaTime);
    //}

    /// <summary>
    /// Bounces monster if it gets out of bounds of the screen
    /// </summary>
    public void Bounce()
    {

        // Gets the min and max of the X and Y coords via the bounds information
        Bounds boundaries = manager.WorldBounds;

        // Adjusts monster position and velocity accordingly if it goes out of bounds of the floor
        if (position.x > boundaries.max.x)
        {
            position.x = boundaries.min.x;
        }
        else if (position.x < boundaries.min.x)
        {
            position.x = boundaries.max.x;
        }

        if (position.z > boundaries.max.z)
        {
            position.z = boundaries.min.z;
        }
        else if (position.z < boundaries.min.z)
        {
            position.z = boundaries.max.z;
        }
        
    }

    /// <summary>
    /// Calculates a force that will turn the vehicle object towards a specific target position
    /// </summary>
    /// <param name="targetPosition">Position of the target that is being seeked</param>
    /// <returns>A force that will turn the vehicle object towards a specific target</returns>
    public Vector3 Seek(Vector3 targetPosition)
    {
        // calculating our desired velocity
        // a vector towards our targetPosition
        Vector3 desiredVelocity = targetPosition - position;

        // Scale desired velocity to equal our max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate the seek steering force
        Vector3 seekingForce = desiredVelocity - velocity;

        //direction = seekingForce.normalized;



        return seekingForce;
    }

    /// <summary>
    /// Calculates a force that will turn the vehicle object towards a specific target GameObject
    /// </summary>
    /// <param name="targetObject">Object to be targeted</param>
    /// <returns>A force that will turn the vehicle object towards a specific target GameObject</returns>
    public Vector3 Seek(GameObject targetObject)
    {
        return Seek(targetObject.transform.position);
    }

    /// <summary>
    /// Calculates a force that will turn the vehicle object towards a specific target Vehicle object
    /// </summary>
    /// <param name="targetVehicle">Vehicle to be targeted</param>
    /// <returns>A force that will turn the vehicle object towards a specific target Vehicle object</returns>
    public Vector3 Seek(Vehicle targetVehicle)
    {
        return Seek(targetVehicle.position);
    }

    /// <summary>
    /// Calculates a force that will turn the vehicle object towards the future position of a target Vehicle
    /// </summary>
    /// <param name="targetVehicle">Vehicle to be targeted</param>
    /// <param name="seconds">How many seconds ahead to look</param>
    /// <returns>Force vector of how to seek based on the targetVehicle's future position</returns>
    public Vector3 Pursue(Vehicle targetVehicle, float seconds = 1)
    {
        Vector3 futurePos = targetVehicle.GetFuturePosition(seconds);
        float futureDist = Vector3.SqrMagnitude(targetVehicle.Position - futurePos);
        float distFromTarget = GetSquaredDistanceBetween(targetVehicle);

        if (distFromTarget < futureDist)
        {
            return Seek(targetVehicle);
        }

        return Seek(futurePos);
    }

    public Vector3 Flee(Vector3 targetPosition)
    {
        // calculating our desired velocity
        // a vector away from our targetPosition
        Vector3 desiredVelocity = position - targetPosition;

        // Scale desired velocity to equal our max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate the flee steering force
        Vector3 fleeingForce = desiredVelocity - velocity;

        return fleeingForce;


    }

    public Vector3 Flee(GameObject fleeObject)
    {
        return Flee(fleeObject.transform.position);
    }

    public Vector3 Flee(Vehicle targetVehicle)
    {
        return Flee(targetVehicle.Position);
    }

    /// <summary>
    /// Calculates a force that will turn the vehicle object away from the future position of a target Vehicle
    /// </summary>
    /// <param name="fleeVehicle">The Vehicle to flee from</param>
    /// <param name="seconds">How many seconds ahead to look</param>
    /// <returns>Force vector of how to flee based on the fleeVehicle's future position</returns>
    public Vector3 Evade(Vehicle fleeVehicle, float seconds = 1)
    {
        Vector3 futurePos = fleeVehicle.GetFuturePosition(seconds);

        float futureDistance = Vector3.SqrMagnitude(fleeVehicle.Position - futurePos);

        float distFromTarget = GetSquaredDistanceBetween(fleeVehicle);

        // If tagrte is closer to me than from it's position in x seconds, flee from target, rather than evade
        if (distFromTarget < futureDistance)
        {
            return Flee(fleeVehicle);
        }

        return Flee(futurePos);
    }

    protected float GetSquaredDistanceBetween(Vehicle entity)
    {
        // Vehicle center, x coord
        float vehicleCenterX = gameObject.GetComponent<BoxCollider>().bounds.center.x;
        // Vehicle center, y coord
        float vehicleCenterZ = gameObject.GetComponent<BoxCollider>().bounds.center.z;
        // Entity center, x coord
        float entityCenterX = entity.GetComponent<BoxCollider>().bounds.center.x;
        // Entity center, y coord
        float entityCenterZ = entity.GetComponent<BoxCollider>().bounds.center.z;

        // Distance between vehicle and entity's center squared
        float distanceSquared = Mathf.Pow((vehicleCenterX - entityCenterX), 2) + Mathf.Pow((vehicleCenterZ - entityCenterZ), 2);

        // Distance between vehicle and entity
        return distanceSquared;
    }

    public Vector3 AvoidObstacle(Obstacle obstacle)
    {
        return AvoidObstacle(obstacle.transform.position, obstacle.Radius);
    }

    public Vector3 AvoidObstacle(Vector3 obstaclePos, float obstacleRadius)
    {
        // Get a vector from the this vhicle to the obstacle
        Vector3 vToObs = obstaclePos - position;

        // Check if the obstacle is behind the vehicle
        float fwdToObsDot = Vector3.Dot(forward, vToObs);
        if (fwdToObsDot < 0)
        {
            return Vector3.zero;
        }

        // Check to see if the obstacle is too far to the left or right
        float rightToObsDot = Vector3.Dot(right, vToObs);
        if (Mathf.Abs(rightToObsDot) > obstacleRadius + radius)
        {
            return Vector3.zero;
        }

        // Check to see if the obstacle is in our view range
        if (fwdToObsDot > obstacleViewDistance)
        {
            return Vector3.zero;
        }

        // Create a weight based on how close we are to the obstacle
        float weight = obstacleViewDistance / Mathf.Max(fwdToObsDot, 0.001f);

        Vector3 desiredVelocity;

        if(rightToObsDot > 0)
        {
            // If the obstacle is on the right, steer left
            desiredVelocity = right * -maxSpeed;
        }
        else
        {
            // If the obstacle is on the left, steer right
            desiredVelocity = right * maxSpeed;
        }

        // Calculate our steering force from our desired velocity
        Vector3 steeringForce = (desiredVelocity - velocity) * weight;

        // return our steering force
        return steeringForce;
    }

    protected Vector3 Seperate<T>(List<T> vehicles) where T : Vehicle
    {
        Vector3 seperationForce = Vector3.zero;

        foreach (Vehicle other in vehicles)
        {
            float sqrDistance = GetSquaredDistanceBetween(other);

            if (sqrDistance < Mathf.Epsilon)
            {
                continue;
            }

            if (sqrDistance < 0.001)
            {
                sqrDistance = 0.001f;
            }

            float personalSpaceRadius = personalSpace * personalSpace;

            if (sqrDistance < personalSpaceRadius)
            {
                seperationForce += Flee(other) * (personalSpaceRadius / sqrDistance);
            }
        }

        return seperationForce;
    }

    protected Vector3 StayInBounds()
    {
        Bounds boundaries = manager.WorldBounds;
        Vector3 futurePos = GetFuturePosition(1);
        Vector3 posToFlee = boundaries.ClosestPoint(position);
        float distBetween = Vector3.Distance(position, posToFlee);
        distBetween = Mathf.Max(distBetween, 0.001f);
        if (position.x > maxX || position.x < minX ||
            position.z > maxZ || position.z < minZ)
        {
            return Seek(posToFlee) * 2000;
        }
        else if (futurePos.x >= maxX || futurePos.x <= minX ||
            futurePos.z >= maxZ || futurePos.z <= minZ)
        {
            return Flee(posToFlee) * (1 / distBetween);
        }
        

        return Vector3.zero;
    }

    public Vector3 AvoidAllObstacles(List<Obstacle> obstacles)
    {
        Vector3 avoidObstacleForce = Vector3.zero;

        foreach (Obstacle obstacle in obstacles)
        {
            avoidObstacleForce += AvoidObstacle(obstacle);
        }

        return avoidObstacleForce;
    }

    public Vector3 Wander()
    {
        Vector3 wanderForce = Vector3.zero;

        // Get a distance in front of Vehicle
        Vector3 distAhead = GetFuturePosition(2);

        // Determine radius of circle and make a circle at that distance
        float radius = 2f;

        // Get circle's center
        Vector3 circleCenter = position + distAhead;

        // Find spot on circle at that angle
        float xSpot = circleCenter.x + Mathf.Cos(randomAngle) * radius;

        float zSpot = circleCenter.z + Mathf.Sin(randomAngle) * radius;

        // Adjust that angle randomly
        float randomNum = Random.Range(1, 2);

        if(randomNum == 1)
        {
            randomAngle -= 10;
        }
        else
        {
            randomAngle += 10;
        }

        // Seek that spot
        wanderForce += Seek(new Vector3(xSpot, position.y, zSpot)) * (1/ 200);

        // Use global position of that spot

        // Constrain angle

        // Return
        return wanderForce;

    }

    public abstract void CalculateSteeringForces();
}

