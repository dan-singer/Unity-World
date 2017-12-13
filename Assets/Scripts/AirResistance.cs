using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirResistance : Volume
{
    public float airDensity = 1.225f;
    public float dragCoefficient = 0.37f;

    public override Vector3 GetForce(Vehicle vehicle)
    {
        Renderer rend = vehicle.GetComponent<Renderer>();
        if (!rend)
            rend = vehicle.GetComponent<Collider>().renderer;
        float area = rend.bounds.extents.x * rend.bounds.extents.y * 4;
        Vector3 force = airDensity * dragCoefficient * area * -0.5f * vehicle.Velocity.sqrMagnitude * vehicle.Direction;
        return force;
    }
}
