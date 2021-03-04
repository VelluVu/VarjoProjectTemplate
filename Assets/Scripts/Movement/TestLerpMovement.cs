using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// For testing lerp movement from primary button input.
/// Suggestion for developer : Make Custom class or game event which triggers the function in LerpMovementTrigger when need to move player to new position. 
/// </summary>
public class TestLerpMovement : MonoBehaviour
{

    public int waypointToMove = 0;

    XRMovementSwitch control;

    private void Awake() {
        control = GetComponent<XRMovementSwitch>();
    }

    private void OnEnable() {
        XRInputManager.onPrimaryButtonDown += OnButtonDown;
    }

    private void OnDisable() {
        XRInputManager.onPrimaryButtonDown -= OnButtonDown;
        
    }
  
    public void OnButtonDown(InputDeviceCharacteristics deviceCharacteristics)
    {
        LerpMovementTrigger.TriggerMovement(waypointToMove);
    }

}