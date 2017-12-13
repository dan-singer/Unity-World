using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Special volume that applies drag to any vehicle that enters it
/// </summary>
/// <author>Dan Singer</author>
public class AirResistance : Volume
{
    public float airDensity = 1.225f;
    public float dragCoefficient = 0.37f;

    /// <summary>
    /// Returns the drag force based on the vehicle's area and direction
    /// </summary>
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
