using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is working as a statepattern for movementstyles and switching the movementstyle when settings change.
/// </summary>
public class XRMovementSwitch : MonoBehaviour
{
    public MovementVariableConfig movementVariables;
    [Tooltip("Automatically Searches the WPS by Waypoint parent gameObject name if this array is left empty!")]
    public Transform[] Wps; //Waypoints for Lerp movement.
    [HideInInspector]public XRCustomRig rig;
    [HideInInspector]public PreferredHand preferredHand;
    [HideInInspector]public bool usingControllers;
    MovementType currentMovementType;
    IXRMovement currentXRMovement;
    XRGazeMovement xRGazeMovement;
    XRPOIGazeMovement xRPOIGazeMovement;
    XRLerpMovement xRLerpMovement;
    XRTeleportMovement xRTeleportMovement;

    bool ready = false;

    private void Awake() {
        rig = GetComponent<XRCustomRig>();
        InitMovementStyles();
        InitWaypointArray();
    }

    public void InitMovementStyles()
    {
        xRGazeMovement = new XRGazeMovement();
        xRPOIGazeMovement = new XRPOIGazeMovement();
        xRLerpMovement = new XRLerpMovement();
        xRTeleportMovement = new XRTeleportMovement();
    }

    public void InitWaypointArray()
    {
        if(Wps == null || Wps.Length == 0)
        {
            GameObject waypoints = GameObject.Find("Waypoints");
            if(waypoints != null)
            {
                Wps = new Transform[waypoints.transform.childCount];
                for (var i = 0; i < Wps.Length; i++)
                {
                    Wps[i] = waypoints.transform.GetChild(i);   
                }
            }
            else
            {
                Debug.LogWarning("Could not populate the Wps array for LerpMovement. Can't find the Waypoints object from scene hierarchy!");
            }
        }
        else
        {
            Debug.Log("Wps array for LerpMovement is custom filled.");
        }
    }

    private void OnEnable() {      
        XRSettings.onSettingChange += CheckMovementType;
    }
    private void OnDisable() {    
        XRSettings.onSettingChange -= CheckMovementType;
    }

    private void Update() 
    {
        if(ready)
        {  
            currentXRMovement.UpdateState();
        }
    }

    public void CheckMovementType(SettingSO newSettings)
    {    
        if(currentXRMovement != null)
            currentXRMovement.ExitState();

        ready = false;
        preferredHand = newSettings.currentHand;
        usingControllers = newSettings.controllersInUse;
        
        switch (newSettings.movementType)
        {
            case MovementType.Gaze:
                currentXRMovement = xRGazeMovement;
                break;
            case MovementType.POIGaze:
                currentXRMovement = xRPOIGazeMovement;
                break;
            case MovementType.Lerp:
                currentXRMovement = xRLerpMovement;
                break;
            case MovementType.Teleport:
                currentXRMovement = xRTeleportMovement;
                break;
            default:
                break;
        }
        currentMovementType = newSettings.movementType;

        ready = true;
        currentXRMovement.StartState(this);
    }

    public MovementType GetCurrentMovementType()
    {
        return currentMovementType;
    }
}