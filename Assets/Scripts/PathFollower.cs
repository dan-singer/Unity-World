using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : Vehicle
{
    protected override void CalcSteeringForces()
    {
        Vector3 netForce = FollowPath() * pathInfo.weight;
        ApplyForce(netForce);
    }
}
