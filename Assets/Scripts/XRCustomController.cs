using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRCustomController : MonoBehaviour
{
    public GameObject modelPrefab;
    GameObject model;

    public InputDeviceCharacteristics hand;
    InputDevice controller;

    private void OnEnable() 
    {
        XRCustomRig.onControllersArePresent += ConnectedControllers;
        XRCustomRig.onControllersNotPresent += DisconnectedControllers;
    }

    private void OnDisable() 
    {
        XRCustomRig.onControllersArePresent -= ConnectedControllers;
        XRCustomRig.onControllersNotPresent -= DisconnectedControllers;
    }

    void ConnectedControllers(List<InputDevice> controllers)
    {
        for (var i = 0; i < controllers.Count; i++)
        {
            if(controllers[i].characteristics.HasFlag(hand))
            {
                
            }
        }
    }

    void DisconnectedControllers(List<InputDevice> controllers)
    {

    }
}
