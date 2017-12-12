using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Path follower agent that follows a path, forages for food, and returns to the village
/// </summary>
/// <author>Dan Singer</author>
public class PathFollower : Vehicle
{
    enum State
    {
        FollowingPath,
        Foraging,
        Placing
    }

    private State state;

    public FoodManager foodManager;
    private GameObject foodTarget;

    protected override void Start()
    {
        base.Start();

    }

    protected override void CalcSteeringForces()
    {
        Vector3 netForce = Vector3.zero;
        switch (state)
        {
            case State.FollowingPath:
                netForce += FollowPath() * pathInfo.weight;
                break;
            case State.Foraging:
                netForce += Seek(foodTarget.transform.position) * seekInfo.weight;
                break;
            case State.Placing:
                break;
            default:
                break;
        }
        
        netForce = Vector3.ClampMagnitude(netForce, maxForce);
        ApplyForce(netForce);
    }

    private void CollisionStarted(Collider coll)
    {
        if (coll.GetComponent<FoodManager>() && !foodTarget)
        {
            state = State.Foraging;
            foodTarget = foodManager.GetFoodTarget();
        }
        if (coll.gameObject.GetInstanceID() == foodTarget.GetInstanceID())
        {
            foodTarget.transform.parent = this.transform;
            foodTarget.transform.localPosition = new Vector3(0, 1, 0);
            state = State.FollowingPath;
        }
    }
    private void CollisionEnded(Collider coll)
    {

    }
}
