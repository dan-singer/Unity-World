using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component which causes this gameobject to snap to the ground, rather than just the terrain.
/// </summary>
public class SnapToGround : MonoBehaviour
{

    public bool centerPivot = true;
    public Renderer rend;

    // Use this for initialization
    void Start()
    {
        if (!rend)
            rend = GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
        float yOffset = centerPivot ? rend.bounds.extents.y : 0;

        RaycastHit info;
        if (Physics.Raycast(new Ray(transform.position + Vector3.up, -transform.up), out info, 10))
        {
            transform.position = new Vector3(transform.position.x, info.point.y + yOffset, transform.position.z);
        }
    }
}
