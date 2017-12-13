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

    /// <summary>
    /// The renderer to use for collisions. If there is not one specified, it will use the one attached to the GameObject, if there is one.
    /// </summary>
    public Renderer renderer;

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

    void Start() {
        if (!renderer)
            renderer = GetComponent<Renderer>();
    }


    /// <summary>
    /// See if there is a collision using Axis Aligned Bounding Box Collision Test
    /// </summary>
    /// <param name="other">Other collider to check against</param>
    /// <returns>True if collision, false otherwise</returns>
    public bool AABB(Collider other)
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
    public bool BoundingCircle(Collider other)
    {
        Vector3 line = other.renderer.bounds.center - renderer.bounds.center;
        float radSum = Radius + other.Radius;
        bool test = line.sqrMagnitude < Mathf.Pow(radSum, 2);
        return test;
    }
}
