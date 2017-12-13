using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains info on each of the collders as well as optimization methods for collision detection.
/// </summary>
/// <author>Dan Singer</author>
public class CollisionManager : MonoBehaviour {

    private static CollisionManager instance;
    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static CollisionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CollisionManager>();
            }
            return instance;
        }
    }

    /// <summary>
    /// An array of all the colliders in the scene.
    /// </summary>
    public List<Collider> AllColliders { get; private set; }

    /// <summary>
    /// Graph representing collision checks this frame.
    /// </summary>
    public Dictionary<Collider, HashSet<Collider>> Collisions { get; private set; }


    private bool readyToUpdateColliders = false;

    private Queue<Collider> collidersToRemove;
    private Queue<Collider> collidersToAdd;


    /// <summary>
    /// These will be broadcast at various stages of the collision.
    /// </summary>
    private static string collisionMessage = "CollisionOccurring";
    private static string collisionStartedMessage = "CollisionStarted";
    private static string collisionEndedMessage = "CollisionEnded";

    /// <summary>
    /// Remove a collider from the list.
    /// </summary>
    public void RemoveCollider(Collider toRemove)
    {
        collidersToRemove.Enqueue(toRemove);
        readyToUpdateColliders = true;
    }

    /// <summary>
    /// Add a collider to the list. This is only necessary if a collider is spawned during gameplay.
    /// </summary>
    public void AddCollider(Collider toAdd)
    {
        AllColliders.Add(toAdd);
        readyToUpdateColliders = true;
    }

    /// <summary>
    /// Initialize the CollisionManager.
    /// </summary>
    void Start () {
        AllColliders = Object.FindObjectsOfType<Collider>().ToList();
        Collisions = new Dictionary<Collider, HashSet<Collider>>();
        collidersToRemove = new Queue<Collider>();
        collidersToAdd = new Queue<Collider>();
    }

    /// <summary>
    /// Notify both colliders of the collision event that just occured
    /// </summary>
    private void InformColliders(string msg, Collider A, Collider B)
    {
        A.BroadcastMessage(msg, B, SendMessageOptions.DontRequireReceiver);
        B.BroadcastMessage(msg, A, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// Loop through each collider, check for collisions, and notify them when they occur
    /// </summary>
    private void Update()
    {
        for (int i=0; i<AllColliders.Count-1; i++)
        {
            Collider A = AllColliders[i];
            for (int j=i+1; j<AllColliders.Count; j++)
            {
                Collider B = AllColliders[j];
                bool colliding;
                if (A.collisionMethod == Collider.Method.AABB)
                    colliding = A.AABB(B);
                else
                    colliding = A.BoundingCircle(B);
                if (colliding)
                {
                    if (!Collisions.ContainsKey(A) || (Collisions.ContainsKey(A) && !Collisions[A].Contains(B)))
                    {
                        InformColliders(collisionStartedMessage, A, B);
                        if (Collisions.ContainsKey(A))
                            Collisions[A].Add(B);
                        else
                            Collisions.Add(A, new HashSet<Collider>());
                    }
                    InformColliders(collisionMessage, A, B);
                }
                else
                {
                    if (Collisions.ContainsKey(A) && Collisions[A].Contains(B))
                    {
                        InformColliders(collisionEndedMessage, A, B);
                        Collisions[A].Remove(B);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handle collider removal and adding in late update so we don't edit the list while iterating over it
    /// </summary>
    void LateUpdate () {

        //Handle collider removal after update loop so we don't 
        if (readyToUpdateColliders)
        {
            while (collidersToRemove.Count > 0)
            {
                Collider toRem = collidersToRemove.Dequeue();
                AllColliders.Remove(toRem);
                Collisions.Remove(toRem);
                foreach (HashSet<Collider> set in Collisions.Values)
                {
                    if (set.Contains(toRem))
                        set.Remove(toRem);
                }
            }
            while (collidersToAdd.Count > 0)
            {
                Collider toAdd = collidersToAdd.Dequeue();
                AllColliders.Add(toAdd);
            }
            readyToUpdateColliders = false;
        }
    }
}
