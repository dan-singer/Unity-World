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

    /// <summary>
    /// Generate river vectors based on a sine wave.
    /// </summary>
    protected override void SetFlowVectors()
    {
        //TODO: Switch direction of river flow (waver from side to side instead)

        float maxAngle = Mathf.PI * 2 * waves;
        float increment = maxAngle / Grid.GetLength(2); //Divide by length of z-axis, as this is the direction the wave should flow.

        float curAngle = 0;
        for (int x=0; x<Grid.GetLength(0); x++)
        {
            for (int y=0; y<Grid.GetLength(1); y++)
            {
                for (int z=0; z<Grid.GetLength(2); z++)
                {
                    //This is based on a sine wave, and the instantaneous velocity of a sin wave is given by cos
                    float derivative = Mathf.Cos(curAngle);
                    float angle = Mathf.Atan(derivative) * Mathf.Rad2Deg;
                    Vector3 flow = Quaternion.Euler(-angle, 0, 0) * new Vector3(0, 0, 1);
                    Grid[x, y, z] = flow;
                    curAngle += increment;
                }
                curAngle = 0;
            }
        }
    }
}
