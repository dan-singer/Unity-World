using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a collider that can be used for collision detection
/// </summary>
/// <author>Dan Singer</author>
public class Collider : MonoBehaviour {

    public enum Method
    {
        AABB,
        BoundingCircles
    }

    public Method collisionMethod;

    public Renderer renderer;

    private HashSet<Collider> collidingWith;

    /// <summary>
    /// These will be broadcast at various stages of the collision.
    /// </summary>
    private static string collisionMessage = "CollisionOccurring";
    private static string collisionStartedMessage = "CollisionStarted";
    private static string collisionEndedMessage = "CollisionEnded";

    /// <summary>
    /// Radius of a sphere encapsulating the mesh.
    /// </summary>
    public float Radius
    {
        get
        {
            return Mathf.Max(renderer.bounds.extents.x, renderer.bounds.extents.y, renderer.bounds.extents.z);
        }
    }
    /// <summary>
    /// Radius of a sphere that is contained in the mesh.
    /// </summary>
    public float InnerRadius
    {
        get
        {
            return Mathf.Min(renderer.bounds.extents.x, renderer.bounds.extents.y, renderer.bounds.extents.z);
        }
    }

    // Use this for initialization
    void Start() {
        if (!renderer)
            renderer = GetComponent<Renderer>();
        collidingWith = new HashSet<Collider>();
    }

    /// <summary>
    /// Loop through each collider, and perform a collision check on it if it hasn't been performed yet this frame.
    /// </summary>
    void Update() {

        foreach (Collider coll in CollisionManager.Instance.AllColliders)
        {
            bool shouldNotCheck = coll == null || coll == this || CollisionManager.Instance.WasCollCheckAlreadyPerformed(this, coll);

            if (shouldNotCheck)
                continue;

            Debug.Assert(coll != null);

            bool collided;
            if (collisionMethod == Method.AABB)
                collided = AABB(coll);
            else
                collided = BoundingCircle(coll);


            CollisionManager.Instance.ReportCollisionCheckThisFrame(this, coll);

            if (collided)
            {
                //Collision began
                if (!collidingWith.Contains(coll))
                {
                    collidingWith.Add(coll);
                    BroadcastCollisionMessage(collisionStartedMessage, coll);
                }
                //Collision occuring
                BroadcastCollisionMessage(collisionMessage,coll);
            }
            else
            {
                //Collision just ended
                if (collidingWith.Contains(coll))
                {
                    collidingWith.Remove(coll);
                    BroadcastCollisionMessage(collisionEndedMessage, coll);
                }
            }
        }
    }

    /// <summary>
    /// Broadcast a message to this MonoBehaviour and to the collider we collided with.
    /// </summary>
    /// <param name="msg">Collision message. See collisionMessage fields.</param>
    /// <param name="other">Other collider we collided with.</param>
    private void BroadcastCollisionMessage(string msg, Collider other)
    {
        if (!other.enabled)
            return;
        BroadcastMessage(msg, other, SendMessageOptions.DontRequireReceiver);
        other.BroadcastMessage(msg, this, SendMessageOptions.DontRequireReceiver);

    }

    /// <summary>
    /// See if there is a collision using Axis Aligned Bounding Box Collision Test
    /// </summary>
    /// <param name="other">Other collider to check against</param>
    /// <returns>True if collision, false otherwise</returns>
    private bool AABB(Collider other)
    {
        bool test = renderer.bounds.max.x > other.renderer.bounds.min.x
            && renderer.bounds.min.x < other.renderer.bounds.max.x
            && renderer.bounds.max.y > other.renderer.bounds.min.y
            && renderer.bounds.min.y < other.renderer.bounds.max.y;

        return test;
    }
    /// <summary>
    /// See if there is a collision using Bounding Circle Collision Test
    /// </summary>
    /// <param name="other">Other collider to check against</param>
    /// <returns>True if collision, false otherwise</returns>
    private bool BoundingCircle(Collider other)
    {
        Vector3 line = other.renderer.bounds.center - renderer.bounds.center;
        float radSum = Radius + other.Radius;
        bool test = line.sqrMagnitude < Mathf.Pow(radSum, 2);
        return test;
    }
}
