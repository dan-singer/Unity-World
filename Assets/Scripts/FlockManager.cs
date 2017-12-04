using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a flock
/// </summary>
/// <author>Dan Singer</author>
public class FlockManager : MonoBehaviour {

    public int spawnQuantity = 5;
    public Renderer spawnContainer;

    //TODO: Spawn flockers

    public List<Vehicle> Flock { get; private set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
