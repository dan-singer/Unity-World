using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour for agent that travels in a flock
/// </summary>
public class Flocker : Vehicle
{

    public FlockManager Manager { get; set; }

    protected override void CalcSteeringForces()
    {
        Vector3 netForce = Vector3.zero;
        if (Manager)
        {
            netForce += Separate(Manager.Flock, separationInfo.radius) * separationInfo.weight;
            netForce += Align(Manager.AverageDirection) * alignInfo.weight;
            netForce += Cohere(Manager.Center) * cohereInfo.weight;
        }

        netForce = Vector3.ClampMagnitude(netForce, maxForce);
        ApplyForce(netForce);
    }
}
