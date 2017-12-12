using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages food in the scene and spawning of food
/// </summary>
/// <author>Dan Singer</author>
public class FoodManager : MonoBehaviour {

    public GameObject foodPrefab;
    //x is min, y is max
    public Vector2 quantityBounds;
    public float padding = 1;

    private Renderer rend;
    private Terrain terrain;

    private Queue<GameObject> foodQueue;



	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        terrain = Terrain.activeTerrain;
        foodQueue = new Queue<GameObject>();
        SpawnFood();
	}

    /// <summary>
    /// Get a random quantity of food to spawn
    /// </summary>
    private int GetRandomQuantity()
    {
        return (int)Random.Range(quantityBounds.x, quantityBounds.y);
    }

    /// <summary>
    /// Get a food target from the food queue
    /// </summary>
    public GameObject GetFoodTarget()
    {
        GameObject food = foodQueue.Dequeue();
        if (foodQueue.Count == 0)
            SpawnFood();
        return food;
    }

    /// <summary>
    /// Spawn food in this renderer's bounds on the terrain
    /// </summary>
    public void SpawnFood()
    {
        int quantity = GetRandomQuantity();
        for (int i=0; i<quantity; i++)
        {
            Vector3 min = rend.bounds.min; Vector3 max = rend.bounds.max;
            Vector3 spawnLoc = new Vector3
            {
                x = Random.Range(min.x + padding, max.x - padding),
                y = 0,
                z = Random.Range(min.z + padding, max.z - padding)
            };
            spawnLoc.y = terrain.SampleHeight(spawnLoc);
            GameObject food = Instantiate<GameObject>(foodPrefab, spawnLoc, Quaternion.identity);
            foodQueue.Enqueue(food);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
