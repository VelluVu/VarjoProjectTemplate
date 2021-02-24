using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Class for device connection testing
/// </summary>
public class DeviceStatusListener : MonoBehaviour
{
    private void OnEnable() 
    {
        XRCustomRig.onHeadMountedDeviceIsPresent += HMDDeviceConnected;
        XRCustomRig.onLeftControllerIsPresent += LControllerDeviceConnected;
        XRCustomRig.onRightControllerIsPresent += RControllerDeviceConnected;
        XRCustomRig.onHeadMountedDeviceDisconnected += HMDDeviceDisconnected;
        XRCustomRig.onLeftControllerDisconnected += LControllerDeviceDisconnected;
        XRCustomRig.onRightControllerDisconnected += RControllerDeviceDisconnected;
        XRCustomRig.onControllersArePresent += ControllersArePresent;
        XRCustomRig.onControllersNotPresent += ControllersNotPresent;
    }

    private void OnDisable() {
        XRCustomRig.onHeadMountedDeviceIsPresent -= HMDDeviceConnected;
        XRCustomRig.onLeftControllerIsPresent -= LControllerDeviceConnected;
        XRCustomRig.onRightControllerIsPresent -= RControllerDeviceConnected;
        XRCustomRig.onHeadMountedDeviceDisconnected -= HMDDeviceDisconnected;
        XRCustomRig.onLeftControllerDisconnected -= LControllerDeviceDisconnected;
        XRCustomRig.onRightControllerDisconnected -= RControllerDeviceDisconnected;
        XRCustomRig.onControllersArePresent -= ControllersArePresent;
        XRCustomRig.onControllersNotPresent -= ControllersNotPresent;
    }

    private void ControllersArePresent(List<InputDevice> controllers)
    {
        Debug.Log(this + ": " + controllers.Count + " Controllers are Present!");
    }
    
    private void ControllersNotPresent(List<InputDevice> controllers)
    {
        Debug.Log(this + ": " + controllers.Count + " Controllers Present...");
    }

    private void RControllerDeviceDisconnected()
    {
        Debug.Log(this + ": Right Controller Disconnected!");
    }

    private void LControllerDeviceDisconnected()
    {
        Debug.Log(this + ": Left Controller Disconnected!");
    }

    private void HMDDeviceDisconnected()
    {
        Debug.Log(this + ": HMD Device Disconnected!");
    }

    private void RControllerDeviceConnected(InputDevice controller)
    {
        Debug.Log(this + ": Right Controller Connected!");
    }

    private void LControllerDeviceConnected(InputDevice controller)
    {
         Debug.Log(this + ": Left Controller Connected!");
    }

    private void HMDDeviceConnected(InputDevice controller)
    {
         Debug.Log(this + ": HMD Device Connected!");
    }
}
