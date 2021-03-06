﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a flock
/// </summary>
/// <author>Dan Singer</author>
public class FlockManager : MonoBehaviour {

    public int spawnQuantity = 5;
    public Flocker flockerPrefab;
    public Flocker flockerLeaderPrefab;
    public Renderer spawnContainer;
    public Renderer flyZone;

    public List<Flocker> Flock { get; private set; }

    /// <summary>
    /// Get the center of the flock
    /// </summary>
    public Vector3 Center { get; private set; }

    public Vector3 AverageDirection { get; private set; }

	// Use this for initialization
	void Start () {
        SpawnFlockers();
	}

    /// <summary>
    /// Spawn flockers in the spawnContainer
    /// </summary>
    public void SpawnFlockers()
    {
        Flock = new List<Flocker>();
        Bounds spawnBounds = spawnContainer.bounds;
        for (int i=0; i<spawnQuantity; i++)
        {
            Vector3 spawnLoc = new Vector3(Random.Range(spawnBounds.min.x, spawnBounds.max.x),
                Random.Range(spawnBounds.min.y, spawnBounds.max.y), Random.Range(spawnBounds.min.z, spawnBounds.max.z));
            Flocker prefab;
            if (i == 0)
                prefab = flockerLeaderPrefab;
            else
                prefab = flockerPrefab;
            Flocker flocker = Instantiate<Flocker>(prefab, spawnLoc, Quaternion.LookRotation(spawnContainer.transform.forward));
            flocker.constrainInfo.constrainArea = flyZone;
            flocker.Manager = this;
            Flock.Add(flocker);
        }
    }
	
	/// <summary>
    /// Keep track of the flock's center and average direction here so each flock member doesn't have to do it.
    /// </summary>
	void Update () {

        Vector3 center = Vector3.zero;
        foreach (Vehicle flocker in Flock)
        {
            center += flocker.transform.position;
        }
        Center = (center / Flock.Count);

        Vector3 avgDir = Vector3.zero;
        foreach (Vehicle flocker in Flock)
        {
            avgDir += flocker.transform.forward;
        }
        AverageDirection = avgDir.normalized;

    }
}
