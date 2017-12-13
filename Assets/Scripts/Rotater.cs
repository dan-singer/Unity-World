using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component which rotates the gameobject it is attached to
/// </summary>
/// <author>Dan Singer</author>
public class Rotater : MonoBehaviour {

    public Vector3 rotationDirection;
    public float rotationSpeed;

	// Use this for initialization
	void Start () {
        rotationDirection.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
	}
}
