using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Chief manager of the simulation
/// </summary>
/// <author>Dan Singer</author>
public class GameManager : MonoBehaviour {

    private static GameManager instance;
    /// <summary>
    /// Singleton pattern
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    public List<Vehicle> Foragers { get; private set; }
    public List<FlockManager> FlockManagers { get; private set; }
    public List<Vehicle> Fish { get; private set; }

    // Use this for initialization
    void Start () {
        Foragers = GameObject.FindObjectsOfType<PathFollower>().ToList<Vehicle>();
        FlockManagers = GameObject.FindObjectsOfType<FlockManager>().ToList<FlockManager>();
        Fish = GameObject.FindObjectsOfType<FlowFollower>().ToList<Vehicle>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
