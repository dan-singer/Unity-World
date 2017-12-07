using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a flock
/// </summary>
/// <author>Dan Singer</author>
public class FlockManager : MonoBehaviour {

    public int spawnQuantity = 5;
    public Flocker flockerPrefab;
    public Renderer spawnContainer;

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

    public void SpawnFlockers()
    {
        Flock = new List<Flocker>();
        Bounds spawnBounds = spawnContainer.bounds;
        for (int i=0; i<spawnQuantity; i++)
        {
            Vector3 spawnLoc = new Vector3(Random.Range(spawnBounds.min.x, spawnBounds.max.x),
                Random.Range(spawnBounds.min.y, spawnBounds.max.y), Random.Range(spawnBounds.min.z, spawnBounds.max.z));
            Flocker flocker = Instantiate<Flocker>(flockerPrefab, spawnLoc, Quaternion.LookRotation(spawnContainer.transform.forward));
            flocker.Manager = this;
            Flock.Add(flocker);
        }
    }
	
	// Update is called once per frame
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
