using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an object that will follow a flow field. 
/// </summary>
/// <author>Dan Singer</author>
public class FlowFollower : Vehicle
{
    private FlowField currentFlowField;

    /// <summary>
    /// Follow the flow field and loop around it if the end of it is reached.
    /// </summary>
    protected override void CalcSteeringForces()
    {
        Vector3 netForce = Vector3.zero;
        netForce += FollowFlowField(flowFieldInfo.flowField, transform.position + Velocity * flowFieldInfo.secondsAhead) * flowFieldInfo.weight;
        netForce = Vector3.ClampMagnitude(netForce, maxForce);


        Bounds bounds = flowFieldInfo.flowField.GetComponent<Renderer>().bounds;
        if (transform.position.z > bounds.max.z)
        {
            transform.position = new Vector3
            {
                x = transform.position.x,
                y = transform.position.y,
                z = bounds.min.z
            };
        }
        ApplyForce(netForce);
    }
}
