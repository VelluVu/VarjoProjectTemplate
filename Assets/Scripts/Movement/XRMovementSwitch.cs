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
    IXRMovement currentXRMovement;
    XRGazeMovement xRGazeMovement;
    XRLerpMovement xRLerpMovement;
    XRTeleportMovement xRTeleportMovement;

    bool ready = false;
    bool isLoaded => XRCustomRig.IsPresent && ready;

    private void Awake() {
        rig = GetComponent<XRCustomRig>();
        xRGazeMovement = new XRGazeMovement();
        xRLerpMovement = new XRLerpMovement();
        xRTeleportMovement = new XRTeleportMovement();
    }

    private void OnEnable() {
        XRSettings.onSettingLoads += CheckMovementType;
        XRSettings.onSettingChange += CheckMovementType;
    }
    private void OnDisable() {
        XRSettings.onSettingLoads -= CheckMovementType;
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
            case MovementType.Lerp:
                currentXRMovement = xRLerpMovement;
                break;
            case MovementType.Teleport:
                currentXRMovement = xRTeleportMovement;
                break;
            default:
                break;
        }

        ready = true;
        currentXRMovement.StartState(this);
    }

    public IXRMovement GetCurrentMovementType()
    {
        return currentXRMovement;
    }
}

public enum MovementType
{
    Gaze,
    Lerp,
    Teleport,
}