using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is switching the movement type.
/// </summary>
public class XRMovementSwitch : MonoBehaviour
{
    public MovementVariableConfig movementVariables;   
    public Transform[] Wps; //Waypoints for Lerp movement.
    [HideInInspector]public XRCustomRig rig;
    MovementType currentMovementType;
    IXRMovement currentXRMovement;
    XRGazeMovement xRGazeMovement;
    XRPOIGazeMovement xRPOIGazeMovement;
    XRLerpMovement xRLerpMovement;
    XRTeleportMovement xRTeleportMovement;

    bool ready = false;
    bool isLoaded => XRCustomRig.IsPresent && ready;

    private void Awake() {
        rig = GetComponent<XRCustomRig>();
        xRGazeMovement = new XRGazeMovement();
        xRPOIGazeMovement = new XRPOIGazeMovement();
        xRLerpMovement = new XRLerpMovement();
        xRTeleportMovement = new XRTeleportMovement();
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

public enum MovementType
{
    Gaze,
    POIGaze,
    Lerp,
    Teleport,
}