using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Vehicle
{ 
    // How many fish the shark has eaten
    private float score = 0;

    public float Score
    {
        get { return score; }
        set { score = value; }
    }

    public override void CalculateSteeringForces()
    {
        Vector3 ultimateForce = Vector3.zero;

        Human closestHuman = FindClosestHuman();
        if (closestHuman != null)
        {
            ultimateForce += Pursue(closestHuman);
        }

        ultimateForce += Seperate(manager.sharks);

        ultimateForce += StayInBounds();

        ultimateForce += AvoidAllObstacles(manager.obstacles);

        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);
        
        ApplyForce(ultimateForce);
    }

    private Human FindClosestHuman()
    {
        List<Human> food = manager.humans;

        Human closestHuman = null;

        float smallestDistance = Mathf.Infinity;

        foreach (Human human in food)
        {
            Vector3 distFromHuman = position - human.Position;
            if (distFromHuman.magnitude < smallestDistance)
            {
                closestHuman = human;
                smallestDistance = distFromHuman.magnitude;
            }
        }

        return closestHuman;
    }
}
