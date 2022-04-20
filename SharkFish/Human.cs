using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Vehicle
{
    public override void CalculateSteeringForces()
    {
        maxForce = 3f;
        List<Human> humans = manager.humans;

        Vector3 ultimateForce = Vector3.zero;

        ultimateForce += CalcSharkEvadeForce();

        ultimateForce += Seperate(humans);

        ultimateForce += StayInBounds() * 1000;

        ultimateForce += AvoidAllObstacles(manager.obstacles);

        ultimateForce = Vector3.ClampMagnitude(ultimateForce, maxForce);

        ApplyForce(ultimateForce);
    }

    private Vector3 CalcSharkEvadeForce()
    {
        Vector3 sharkEvadeForce = Vector3.zero;
        foreach (Shark shark in manager.sharks)
        {
            float distBetween = Mathf.Sqrt(GetSquaredDistanceBetween(shark));
            if (distBetween < 50) // Every Unity unit is exactly 10 Cartesian units. Therefore 5 Unity units away is 50 Cartesian units away.
            {
                sharkEvadeForce += Evade(shark);
            }
            else
            {
                sharkEvadeForce += Wander();
            }
        }

        return sharkEvadeForce;
    }

}
