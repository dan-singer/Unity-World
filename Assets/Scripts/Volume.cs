using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents and area that, when a vehicle enters it, will apply a force.
/// </summary>
/// <author>Dan Singer</author>
[RequireComponent(typeof(Collider))]
public abstract class Volume : MonoBehaviour {

    public abstract Vector3 GetForce(Vehicle vehicle);

}
