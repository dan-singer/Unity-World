using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component which causes this gameobject to snap to the terrain
/// </summary>
public class SnapToTerrain : MonoBehaviour {

    public Terrain terrain;
    public bool centerPivot = true;
    public Renderer rend;

	// Use this for initialization
	void Start () {
        if (!terrain)
            terrain = Terrain.activeTerrain;
        if (!rend)
            rend = GetComponent<Renderer>();

	}
	
	// Update is called once per frame
	void Update () {
        float yOffset = centerPivot ? rend.bounds.extents.y : 0;
        transform.position = new Vector3(transform.position.x, terrain.SampleHeight(transform.position) + yOffset, transform.position.z);
	}
}
