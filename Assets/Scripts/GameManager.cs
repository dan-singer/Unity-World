using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Chief manager of the simulation
/// </summary>
/// <author>Dan Singer</author>
public class GameManager : MonoBehaviour {

    private static GameManager instance;
    /// <summary>
    /// Singleton pattern
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }
    public FollowCam mainCam;
    public Transform camPositionsHolder;
    public Renderer airResistance;

    public List<Vehicle> Foragers { get; private set; }
    public List<FlockManager> FlockManagers { get; private set; }
    public List<Vehicle> Fish { get; private set; }

    private Transform camTemplate;
    private int currentView = 0;
    // Use this for initialization
    void Start () {
        Foragers = GameObject.FindObjectsOfType<PathFollower>().ToList<Vehicle>();
        FlockManagers = GameObject.FindObjectsOfType<FlockManager>().ToList<FlockManager>();
        Fish = GameObject.FindObjectsOfType<FlowFollower>().ToList<Vehicle>();

        SetView(0);
        camTemplate = camPositionsHolder.GetChild(0);

        //Add some dynamic views to the CameraPositions transform:
        MakeNewViewForTarget(Foragers[0].transform);
        MakeNewViewForTarget(FlockManagers[0].Flock[0].transform);

        DebugLineRenderer.Draw = false;
    }

    /// <summary>
    /// Make a new transform under the camPositionsHolder with settings for the camera
    /// </summary>
    private void MakeNewViewForTarget(Transform target)
    {
        Transform targetView = Instantiate<Transform>(camTemplate);
        CamTargetInfo ftarg = targetView.GetComponent<CamTargetInfo>();
        ftarg.target = target;
        ftarg.positionOffset = new Vector3(0, 1, -1);
        targetView.parent = camPositionsHolder;
    }

    /// <summary>
    /// Move to the next camera view
    /// </summary>
    public void NextView()
    {
        currentView++;
        if (currentView >= camPositionsHolder.childCount)
            currentView = 0;
        SetView(currentView);
    }

    /// <summary>
    /// Move to the previous camera view
    /// </summary>
    public void PreviousView()
    {
        currentView--;
        if (currentView < 0)
            currentView = camPositionsHolder.childCount-1;
        SetView(currentView);
    }
    /// <summary>
    /// Set the camera's view by index number
    /// </summary>
    private void SetView(int index)
    {
        CamTargetInfo cti = camPositionsHolder.GetChild(index).GetComponent<CamTargetInfo>();
        if (cti.target)
        {
            mainCam.target = cti.target;
            mainCam.positionOffset = cti.positionOffset;
            mainCam.rotationOffset = cti.rotationOffset;
        }
        else
        {
            mainCam.target = cti.transform;
            mainCam.positionOffset = Vector3.zero;
            mainCam.rotationOffset = Vector3.zero;
        }
    }

    public void SetDebugLineMode(bool val)
    {
        DebugLineRenderer.Draw = val;
        airResistance.enabled = val;
    }
}
