using System.Collections;
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
/// Group info about path following force
public class PathFollowInfo : SForceInfo{
    public float radius;
    public float secondsAhead;
    public Vector3 start, end;
}




