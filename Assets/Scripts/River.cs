using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// River implementation of a FlowField
/// </summary>
public class River : FlowField
{
    /// <summary>
    /// Number of complete waves to generate across the flow field
    /// </summary>
    public float waves = 3;

    public int paddingCells = 2;

    /// <summary>
    /// Generate river vectors based on a sine wave.
    /// </summary>
    protected override void SetFlowVectors()
    {
        float maxAngle = Mathf.PI * 2 * waves;
        float increment = maxAngle / Grid.GetLength(1); //Divide by length of z-axis, as this is the direction the wave should flow.

        float curAngle = 0;
        for (int x=0; x<Grid.GetLength(0); x++)
        {
            bool isPadding = x < paddingCells || x >= Grid.GetLength(0) - paddingCells;

            for (int z=0; z<Grid.GetLength(1); z++)
            {
                if (!isPadding)
                {
                    //This is based on a sine wave, and the instantaneous velocity of a sin wave is given by cos
                    float derivative = Mathf.Cos(curAngle);
                    float angle = Mathf.Atan(derivative) * Mathf.Rad2Deg;
                    Vector3 flow = Quaternion.Euler(0, -angle, 0) * new Vector3(0, 0, 1);
                    Grid[x, z] = flow;
                    curAngle += increment;
                }
                else
                {
                    //Just push the vehicle back to the center if it's a padding cell so they don't leave the flow fields
                    if (x < paddingCells)
                        Grid[x, z] = Vector3.right;
                    else
                        Grid[x, z] = -Vector3.right;
                }
            }
            curAngle = 0;
        }
    }


}
