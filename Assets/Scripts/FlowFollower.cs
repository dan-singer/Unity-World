using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFollower : Vehicle
{
    private FlowField currentFlowField;

    protected override void CalcSteeringForces()
    {
        Vector3 netForce = Vector3.zero;
        //TODO: refactor vehicle class such that parameters don't need to be provided, but are just read from the info objects.
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
            print("looping");
        }



        ApplyForce(netForce);
    }

    private void CollisionStarted(Collider coll)
    {

    }
    private void CollisionEnded(Collider coll)
    {
    }
}
