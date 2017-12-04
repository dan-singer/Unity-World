using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFollower : Vehicle
{
    private FlowField currentFlowField;

    protected override void CalcSteeringForces()
    {
        Vector3 netForce = Vector3.zero;
        if (currentFlowField)
        {
            netForce += FollowFlowField(currentFlowField, transform.position + Velocity * flowFieldInfo.secondsAhead) * flowFieldInfo.weight;
        }
        netForce = Vector3.ClampMagnitude(netForce, maxForce);
        ApplyForce(netForce);
    }

    private void CollisionStarted(Collider coll)
    {
        if (coll.GetComponent<FlowField>())
        {
            currentFlowField = coll.GetComponent<FlowField>();
        }
    }
    private void CollisionEnded(Collider coll)
    {
        if (coll.GetComponent<FlowField>())
        {
            currentFlowField = null;
        }
    }
}
