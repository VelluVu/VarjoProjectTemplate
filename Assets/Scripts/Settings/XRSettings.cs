using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// This class is singleton of settings and holds the scriptable object of settings and functions to change the settings.
/// </summary>
public class XRSettings : Singleton<XRSettings>
{
    public delegate void SettingDelegate(SettingSO newSettings);
    public static event SettingDelegate onSettingChange;
    public SettingSO settings;
    MovementType previousMovementType;

    private void Start() {
        settings.Init();
    }

    private void OnEnable() {
        settings.onChange += ChangedSettingsDirectly;
        XRCustomRig.onControllersNotPresent += ControllersDisconnected;
        XRCustomRig.onControllersArePresent += ControllersConnected;
        XRCustomRig.onLeftControllerIsPresent += LCConnected;
        XRCustomRig.onRightControllerIsPresent += RCConnected;
        XRCustomRig.onLeftControllerDisconnected += LCDisconnected;
        XRCustomRig.onRightControllerDisconnected += RCDisconnected;
    }

    private void OnDisable() {
        settings.onChange -= ChangedSettingsDirectly;
        XRCustomRig.onControllersNotPresent -= ControllersDisconnected;
        XRCustomRig.onControllersArePresent -= ControllersConnected;
        XRCustomRig.onLeftControllerIsPresent -= LCConnected;
        XRCustomRig.onRightControllerIsPresent -= RCConnected;
        XRCustomRig.onLeftControllerDisconnected -= LCDisconnected;
        XRCustomRig.onRightControllerDisconnected -= RCDisconnected;
        settings.currentHand = settings.preferredHand;
    }

    private void RCDisconnected()
    {
        if(settings.preferredHand == PreferredHand.Right && settings.currentHand != PreferredHand.Left)
        {
            settings.currentHand = PreferredHand.Left;
            ChangedSettingsDirectly(settings);
        }
    }

    private void LCDisconnected()
    {
        
        if(settings.preferredHand == PreferredHand.Left && settings.currentHand != PreferredHand.Right)
        {
            settings.currentHand = PreferredHand.Right;
            ChangedSettingsDirectly(settings);
        }
        
    }

    private void RCConnected(InputDevice controller)
    {
        if(settings.preferredHand == PreferredHand.Right && settings.currentHand != PreferredHand.Right)
        {
            settings.currentHand = PreferredHand.Right;
            ChangedSettingsDirectly(settings);
        }
    }

    private void LCConnected(InputDevice controller)
    {
        if(settings.preferredHand == PreferredHand.Left && settings.currentHand != PreferredHand.Left)
        {
            settings.currentHand = PreferredHand.Left;
            ChangedSettingsDirectly(settings);
        }
    }

    private void ControllersConnected(List<InputDevice> controllers)
    {
        if(!settings.controllersInUse)
        {
            settings.controllersInUse = true;
            ChangedSettingsDirectly(settings);
        }
    }

    private void ControllersDisconnected(List<InputDevice> controllers)
    {
        if(settings.controllersInUse)
        {
            settings.controllersInUse = false;
            ChangedSettingsDirectly(settings);
        }
    }

    public void ChangedSettingsDirectly(SettingSO newSettings)
    {
        onSettingChange?.Invoke(newSettings);
    }

    public void ChangeMovementType(MovementType newMovementType)
    {
        if(settings.movementType != newMovementType)
        {        
            settings.movementType = newMovementType;
            onSettingChange?.Invoke(settings);
        }
    }

    public void ChangePreferredHand(PreferredHand newHand)
    {
        if(settings.currentHand != newHand)
        {        
            settings.currentHand = newHand;
            onSettingChange?.Invoke(settings);
        }
    }

    public void ChangeSnapTurning(bool newState)
    {
        if(settings.snapTurningOn != newState)
        {
            settings.snapTurningOn = newState;
            onSettingChange?.Invoke(settings);
        }
    }

}
