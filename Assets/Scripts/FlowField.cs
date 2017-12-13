using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component representing a flow/direction field. Requires a renderer (preferably box renderer)
/// </summary>
/// <author>Dan Singer</author>

[RequireComponent(typeof(Renderer))]
public abstract class FlowField : MonoBehaviour {

    //Number of units in each cell. The lower this is, the more detailed the flow field is.
    public float unitsPerCell = 0.5f;

    protected Renderer rend;
    protected Vector3 boundsSize;
    protected DebugLineRenderer debugLineRenderer;

    public Vector3[,] Grid { get; protected set; }



    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        debugLineRenderer = GetComponent<DebugLineRenderer>();
        boundsSize = rend.bounds.extents * 2;

        //Divide the size of the bounds in each dimension by unitsPerCell. 
        //In other words, we have a size in units times cells per unit, so units divide out, and we're left with number of cells in each dimension
        Grid = new Vector3[(int)(boundsSize.x / unitsPerCell), (int)(boundsSize.z / unitsPerCell)];
        //Let subclasses set the flow field vectors
        SetFlowVectors();
	}

    /// <summary>
    /// Set the flow field vectors (called at start)
    /// </summary>
    protected abstract void SetFlowVectors();
    
    /// <summary>
    /// Get the flow vector provided a world position
    /// </summary>
    /// <returns>The flow vector if it exists, null otherwise</returns>
    public Vector3? GetFlowVector(Vector3 worldPosition)
    {
        Vector3 origin = transform.position - rend.bounds.extents;
        //Get position relative to bottom left corner of flow field
        Vector3 localPos = worldPosition - origin;

        //Divide by the cell size in each direction to determine the index in the grid array
        int x = (int)(localPos.x / unitsPerCell);
        int z = (int)(localPos.z / unitsPerCell);

        //Confirm that each index is valid in the Grid array, otherwise return null
        if (IndexInGrid(x, 0) && IndexInGrid(z, 1))
            return Grid[x, z];
        else
            return null;
    }

    /// <summary>
    /// Check if the provided index in the specified dimension of the Grid array is valid.
    /// </summary>
    /// <returns>True if valid, false if not</returns>
    private bool IndexInGrid(int index, int dimension)
    {
        return index >= 0 && index < Grid.GetLength(dimension);
    }

    /// <summary>
    /// Draw Debug lines
    /// </summary>
    private void Update()
    {
        DrawDebugLines();
    }

    /// <summary>
    /// Draws a line for each vector in the flow field. NOTE: This is very expensive!
    /// </summary>
    private void DrawDebugLines()
    {
        Vector3 origin = transform.position - rend.bounds.extents;

        for (int x = 0; x < Grid.GetLength(0); x++)
            for (int z = 0; z < Grid.GetLength(1); z++)
            {
                Vector3 direction = Grid[x, z];
                Vector3 localPos = new Vector3(x, 0, z) * unitsPerCell;
                Vector3 worldPos = origin + localPos;
                debugLineRenderer.DrawLine(0, worldPos, worldPos + direction);
            }
    }
}
