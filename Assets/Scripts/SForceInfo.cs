using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The following small classes contain information on different types of steering forces.
/// </summary>
/// <author>Dan Singer</author>


[System.Serializable]
///Group information about a force with a weight.
public class SForceInfo  {
    public float weight;
}
[System.Serializable]
///Group information about a force with a radius and weight.
public class SForceRadiusInfo : SForceInfo{ 
    public float radius;
}
[System.Serializable]
///Group information about a force with a weight and secondsAhead field.
public class PursueEvadeInfo : SForceInfo{
    public float secondsAhead;
}
[System.Serializable]
///Group information about a force with a weight, radius, and unitsAhead field.
public class WanderInfo : SForceRadiusInfo{
    public float unitsAhead;
}
[System.Serializable]
public class FlowFieldInfo : PursueEvadeInfo{
    public FlowField flowField;
}

[System.Serializable]
/// Group info about path following force
public class PathFollowInfo : SForceInfo{
    public float radius;
    public float distAhead;
    public float secondsAhead;
    public bool bidirectional;
    public Transform pathParent;
    public Vector3[] Points { get; private set; }
    /// <summary>
    /// Populate the Points array with the positions of the children of the pathParent Transform.
    /// </summary>
    public void CalculatePoints()
    {
        Transform[] temp = pathParent.GetComponentsInChildren<Transform>();
        Points = new Vector3[temp.Length - 1];
        for (int i = 1; i < temp.Length; i++)
            Points[i - 1] = temp[i].position;
    }
}



