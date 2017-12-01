using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component with methods to draw debug lines in the game view.
/// </summary>
/// <author>Dan Singer</author>
public class DebugLineRenderer : MonoBehaviour {

    public Material[] materials;
    public GameObject debugShape;

    private Renderer debugShapeRend;

    private static event Action<bool> DrawChanged;

    private static bool draw = false;
    /// <summary>
    /// Static Draw property, indicating whether ANY debug lines should be drawn.
    /// </summary>
    public static bool Draw
    {
        get
        {
            return draw;
        }
        set
        {
            draw = value;
            if (DrawChanged != null)
                DrawChanged(draw);
        }
    }

    /// <summary>
    /// Nested class used to group info about debug lines.
    /// </summary>
    private class LineInfo
    {
        public Material Material { get; private set; }
        public Vector3 Start { get; private set; }
        public Vector3 End { get; private set; }

        /// <summary>
        /// Construct a new LineInfo object.
        /// </summary>
        /// <param name="mat">Material to use to draw the line</param>
        /// <param name="start">Starting point</param>
        /// <param name="end">Ending point</param>
        public LineInfo(Material mat, Vector3 start, Vector3 end)
        {
            Material = mat;
            Start = start;
            End = end;
        }
    }

    private List<LineInfo> lines;

	/// <summary>
    /// Initialize the DebugLineRenderer component.
    /// </summary>
	void Start () {
        lines = new List<LineInfo>();
        if (debugShape)
            debugShapeRend = debugShape.GetComponent<Renderer>();

        //Only hide the debugShape when Draw state is changed, no need to set this each frame.
        DrawChanged += (draw) => {
            if (debugShapeRend != null)
                debugShapeRend.enabled = draw;
        };
	}

    /// <summary>
    /// Draw a line this frame.
    /// </summary>
    /// <param name="matIndex">Index of the material to use. See materials array.</param>
    /// <param name="start">Starting point of line</param>
    /// <param name="end">Ending point of line</param>
    public void DrawLine(int matIndex, Vector3 start, Vector3 end)
    {
        if (matIndex >= materials.Length)
            return;
        lines.Add(new LineInfo(materials[matIndex], start, end));
    }

    /// <summary>
    /// If a debugShape exists, set its location.
    /// </summary>
    /// <param name="loc">Location to use</param>
    public void SetShapeLocation(Vector3 loc)
    {
        if (debugShape)
            debugShape.transform.position = loc;
    }

    /// <summary>
    /// If a debug shape exists, set it's forward direction
    /// </summary>
    public void SetShapeFwd(Vector3 fwd)
    {
        if (debugShape)
            debugShape.transform.forward = fwd;
    }

    /// <summary>
    /// If Draw is true, render each line in the lines array.
    /// </summary>
    private void OnRenderObject()
    {
        if (!Draw)
            return;
        foreach (LineInfo line in lines)
        {
            line.Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(line.Start);
            GL.Vertex(line.End);
            GL.End();
        }
        lines.Clear();
    }
}
