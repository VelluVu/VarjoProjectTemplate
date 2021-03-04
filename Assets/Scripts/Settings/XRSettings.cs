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
        settings.onChange += OnChangeSettings;
        XRCustomRig.onControllersNotPresent += ControllersDisconnected;
        XRCustomRig.onControllersArePresent += ControllersConnected;
        XRCustomRig.onLeftControllerIsPresent += LCConnected;
        XRCustomRig.onRightControllerIsPresent += RCConnected;
        XRCustomRig.onLeftControllerDisconnected += LCDisconnected;
        XRCustomRig.onRightControllerDisconnected += RCDisconnected;
    }

    private void OnDisable() {
        settings.onChange -= OnChangeSettings;
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
            settings.previousHand = settings.currentHand;
            settings.currentHand = PreferredHand.Left;
            OnChangeSettings(settings);
        }
    }

    private void LCDisconnected()
    {
        
        if(settings.preferredHand == PreferredHand.Left && settings.currentHand != PreferredHand.Right)
        {
            settings.previousHand = settings.currentHand;
            settings.currentHand = PreferredHand.Right;
            OnChangeSettings(settings);
        }
        
    }

    private void RCConnected(InputDevice controller)
    {
        if(settings.preferredHand == PreferredHand.Right && settings.currentHand != PreferredHand.Right)
        {
            settings.previousHand = settings.currentHand;
            settings.currentHand = PreferredHand.Right;
            OnChangeSettings(settings);
        }
    }

    private void LCConnected(InputDevice controller)
    {
        if(settings.preferredHand == PreferredHand.Left && settings.currentHand != PreferredHand.Left)
        {
            settings.previousHand = settings.currentHand;
            settings.currentHand = PreferredHand.Left;
            OnChangeSettings(settings);
        }
    }

    private void ControllersConnected(List<InputDevice> controllers)
    {
        if(!settings.controllersInUse)
        {    
            settings.controllersInUse = true;
            OnChangeSettings(settings);
        }
    }

    private void ControllersDisconnected(List<InputDevice> controllers)
    {
        if(settings.controllersInUse)
        {
            settings.previousHand = settings.currentHand;
            settings.currentHand = PreferredHand.Hmd;
            settings.controllersInUse = false;
            OnChangeSettings(settings);
        }
    }

    public void OnChangeSettings(SettingSO settings)
    {
        onSettingChange?.Invoke(settings);
    }

    public void ChangeMovementType(MovementType newMovementType)
    {
        if(settings.movementType != newMovementType)
        {        
            settings.movementType = newMovementType;
            OnChangeSettings(settings);
        }
    }

    public void ChangePreferredHand(PreferredHand newHand)
    {
        if(settings.currentHand != newHand)
        {        
            settings.previousHand = settings.currentHand;
            settings.currentHand = newHand;
            OnChangeSettings(settings);
        }
    }

    public void ChangeSnapTurning(bool newState)
    {
        if(settings.snapTurningOn != newState)
        {
            settings.snapTurningOn = newState;
            OnChangeSettings(settings);
        }
    }

}
