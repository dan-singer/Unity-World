using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera that will follow and look at its current target
/// </summary>
/// <author>Dan Singer</author>
public class FollowCam : MonoBehaviour {

    public Transform target;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public float speed = 1;
    public bool userAdjustable = true;
    public float scrollSpeed = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	/// <summary>
    /// Smoothly go to and look at the target, incorporating offsets.
    /// </summary>
	void Update () {
        if (!target)
            return;
        //Position
        Vector3 globalPositionOffset = transform.TransformDirection(positionOffset);
        transform.position = Vector3.Lerp(transform.position, target.position + globalPositionOffset, speed * Time.deltaTime);
        //Rotation
        Vector3 forward = Quaternion.Euler(transform.TransformDirection(rotationOffset)) * target.forward;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forward), speed * Time.deltaTime);

        //User controls
        if (userAdjustable)
        {
            if (Input.GetButton("Fire1"))
            {
                Vector3 mouseInput = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
                rotationOffset += mouseInput;
            }
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            positionOffset.z += zoom * scrollSpeed;

            //Clamp rotationOffset
            rotationOffset.x = Mathf.Clamp(rotationOffset.x, -45, 45);
            rotationOffset.y = Mathf.Clamp(rotationOffset.y, -45, 45);


        }
    }
}
