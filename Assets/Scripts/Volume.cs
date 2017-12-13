using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents and area that will apply a force to a vehicle when one enters it.
/// </summary>
/// <author>Dan Singer</author>
[RequireComponent(typeof(Collider))]
public abstract class Volume : MonoBehaviour {

    public abstract Vector3 GetForce(Vehicle vehicle);

}
