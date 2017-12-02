using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component representing a flow/direction field. Requires a renderer (preferably box renderer)
/// </summary>
/// <author>Dan Singer</author>

[RequireComponent(typeof(Renderer))]
public class FlowField : MonoBehaviour {

    public int unitsPerCell = 15;

    private Renderer rend;
    public Vector3[,,] Grid { get; private set; }

    delegate bool BoolDel(int i);


    // Use this for initialization
    void Start () {
        Vector3 size = rend.bounds.extents * 2;
        Grid = new Vector3[(int)size.x / unitsPerCell, (int)size.y / unitsPerCell, (int)size.z / unitsPerCell];
	}
    
    /// <summary>
    /// Get the flow vector provided a world position
    /// </summary>
    /// <returns>The flow vector if it exists, null otherwise</returns>
    public Vector3? GetFlowVector(Vector3 worldPosition)
    {
        Vector3 origin = transform.position + new Vector3(-rend.bounds.extents.x, -rend.bounds.extents.y, rend.bounds.extents.z);
        Vector3 localPos = worldPosition - origin;

        int i = (int)localPos.z / unitsPerCell;
        int j = (int)localPos.x / unitsPerCell;
        int k = (int)localPos.y / unitsPerCell;

        BoolDel inArr = (int index) => { return index >= 0 && index < Grid.GetLength(0); };

        if (inArr(i) && inArr(j) && inArr(k))
            return Grid[i, j, k];
        else
            return null;
    }
}
