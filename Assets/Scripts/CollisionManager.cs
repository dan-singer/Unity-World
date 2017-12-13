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
    public Collider[] AllColliders { get; private set; }

    /// <summary>
    /// Graph representing collision checks this frame.
    /// </summary>
    public Dictionary<Collider, HashSet<Collider>> Collisions { get; private set; }


    private bool readyToUpdateColliders = false;


    /// <summary>
    /// These will be broadcast at various stages of the collision.
    /// </summary>
    private static string collisionMessage = "CollisionOccurring";
    private static string collisionStartedMessage = "CollisionStarted";
    private static string collisionEndedMessage = "CollisionEnded";

    /// <summary>
    /// Update the list of all the colliders.
    /// Be sure to call this when instanitating or destroying anything.
    /// </summary>
    public void UpdateAllColliders()
    {
        readyToUpdateColliders = true;
    }


    /// <summary>
    /// Initialize the CollisionManager.
    /// </summary>
    void Start () {
        AllColliders = Object.FindObjectsOfType<Collider>();
        Collisions = new Dictionary<Collider, HashSet<Collider>>();
    }

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
        for (int i=0; i<AllColliders.Length-1; i++)
        {
            Collider A = AllColliders[i];
            for (int j=i+1; j<AllColliders.Length; j++)
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
    /// Clear each CollisionChecks graph, and update AllColliders array if necessary.
    /// </summary>
    void LateUpdate () {

        if (readyToUpdateColliders)
        {
            AllColliders = Object.FindObjectsOfType<Collider>();
            readyToUpdateColliders = false;
        }
    }
}
