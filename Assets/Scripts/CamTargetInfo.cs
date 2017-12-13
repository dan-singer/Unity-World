using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains information about a camera target
/// </summary>
/// <author>Dan Singer</author>
public class CamTargetInfo : MonoBehaviour {
    public Transform target;
    public Vector3 positionOffset, rotationOffset;
}
