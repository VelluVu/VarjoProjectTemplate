using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System;

/// <summary>
/// This class is singleton of settings and holds the scriptable object of settings and functions to change the settings.
/// </summary>
public class XRSettings : Singleton<XRSettings>
{
    public GameSettings settings;
    MovementType previousMovementType;

    public delegate void SettingDelegate(GameSettings newSettings);
    public static event SettingDelegate onSettingChange;
    
    private void Awake() 
    {
        settings = LoadSettings();
    }

    private void Start() 
    {
        
        onSettingChange?.Invoke(settings);
    }

    private void Update() 
    {
        if(settings.isDirty)
        {
            settings.ClearDirty();
            onSettingChange?.Invoke(settings);
        }

    }

    private void OnEnable() {
        
        XRCustomRig.onControllersNotPresent += ControllersDisconnected;
        XRCustomRig.onControllersArePresent += ControllersConnected;
        XRCustomRig.onLeftControllerIsPresent += LCConnected;
        XRCustomRig.onRightControllerIsPresent += RCConnected;
        XRCustomRig.onLeftControllerDisconnected += LCDisconnected;
        XRCustomRig.onRightControllerDisconnected += RCDisconnected;
    }

    private void OnDisable() {
        
        XRCustomRig.onControllersNotPresent -= ControllersDisconnected;
        XRCustomRig.onControllersArePresent -= ControllersConnected;
        XRCustomRig.onLeftControllerIsPresent -= LCConnected;
        XRCustomRig.onRightControllerIsPresent -= RCConnected;
        XRCustomRig.onLeftControllerDisconnected -= LCDisconnected;
        XRCustomRig.onRightControllerDisconnected -= RCDisconnected;
    }

    private void RCDisconnected()
    {
        if(settings.PreferredHand == PreferredHand.Right && settings.CurrentHand != PreferredHand.Left)
        {
            settings.PreviousHand = settings.CurrentHand;
            settings.CurrentHand = PreferredHand.Left;
            settings.RightControllerConnected = false;
        }
    }

    private void LCDisconnected()
    {
        
        if(settings.PreferredHand == PreferredHand.Left && settings.CurrentHand != PreferredHand.Right)
        {       
            settings.CurrentHand = PreferredHand.Right;
            settings.LeftControllerConnected = false;
        }
        
    }

    private void RCConnected(InputDevice controller)
    {
        if(settings.PreferredHand == PreferredHand.Right && settings.CurrentHand != PreferredHand.Right)
        { 
            settings.CurrentHand = PreferredHand.Right;
            settings.RightControllerConnected = true;
        }
    }

    private void LCConnected(InputDevice controller)
    {
        if(settings.PreferredHand == PreferredHand.Left && settings.CurrentHand != PreferredHand.Left)
        { 
            settings.CurrentHand = PreferredHand.Left;
            settings.LeftControllerConnected = true;
        }
    }

    private void ControllersConnected(List<InputDevice> controllers)
    {
      
        if(!settings.ControllersInUse)
        {    
            settings.ControllersInUse = true;
            settings.CurrentHand = settings.PreferredHand;

            if(!settings.RightControllerConnected)
                settings.RightControllerConnected = true;
            if(!settings.LeftControllerConnected)
                settings.LeftControllerConnected = true;
        }
    }

    private void ControllersDisconnected(List<InputDevice> controllers)
    {
        if(settings.ControllersInUse)
        {
            settings.CurrentHand = PreferredHand.Hmd;
            settings.ControllersInUse = false;

            if(settings.LeftControllerConnected)
                settings.LeftControllerConnected = false;
            if(settings.RightControllerConnected)
                settings.RightControllerConnected = false;
        }
    }
    public void OnChangeSettings(GameSettings settings)
    {
        onSettingChange?.Invoke(settings);
    }

    public void ChangeMovementType(MovementType newMovementType)
    {
        if(settings.MovementType != newMovementType)
        {        
            settings.MovementType = newMovementType;
        }
    }

    public void ChangePreferredHand(PreferredHand newHand)
    {
        if(settings.CurrentHand != newHand)
        {
            settings.PreferredHand = newHand;
        }
    }

    public void ChangeSnapTurning(bool newState)
    {
        if(settings.SnapTurningOn != newState)
        {
            settings.SnapTurningOn = newState;
        }
    }

    public void SaveSettings(GameSettings newSettings)
    {
        string data = JsonUtility.ToJson(newSettings);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/SettingsData.json", data);
        Debug.Log("Saved settings to " + Application.persistentDataPath + "/SettingsData.json!");
    }

    public void SaveSettings()
    {
        string data = JsonUtility.ToJson(settings);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/SettingsData.json", data);
        Debug.Log("Saved settings to " + Application.persistentDataPath + "/SettingsData.json!");
    }

    public GameSettings LoadSettings()
    {
        GameSettings settings;

        if(System.IO.File.Exists(Application.persistentDataPath + "/SettingsData.json"))
        {
            string data = System.IO.File.ReadAllText(Application.persistentDataPath + "/SettingsData.json");
            settings = JsonUtility.FromJson<GameSettings>(data);
            Debug.Log("Loaded settings from " + Application.persistentDataPath + "/SettingsData.json!");
        }
        else
        {
            settings = DefaultSettings();
            Debug.Log("Could not locate saved GameSettings, opening default settings!");
        }

        return settings;
    }

    public GameSettings DefaultSettings()
    {
        GameSettings settings = new GameSettings();
        settings.ControllersInUse = false;
        settings.SnapTurningOn = true;
        settings.MovementType = MovementType.Gaze;
        settings.CurrentHand = PreferredHand.Hmd;
        settings.PreferredHand = PreferredHand.Hmd;
        settings.PreviousHand = PreferredHand.Hmd;
        settings.ClearDirty();
        return settings;
    }
    
}

[Serializable]
public class GameSettings
{
    public bool isDirty = false;

    [SerializeField]bool controllersInUse;
    public bool ControllersInUse
    {
        get
        {
            return controllersInUse;
        }
        set
        {
            isDirty = true;
            controllersInUse = value;
        }
    }

    [SerializeField]bool leftControllerConnected;
    public bool LeftControllerConnected
    {
        get
        {
            return leftControllerConnected;
        }
        set
        {
            isDirty = true;
            leftControllerConnected = value;
        }
    }
    [SerializeField]bool rightControllerConnected;
    public bool RightControllerConnected
    {
        get
        {
            return rightControllerConnected;
        }
        set
        {
            isDirty = true;
            rightControllerConnected = value;
        }
    }
    [SerializeField]bool snapTurningOn;
    public bool SnapTurningOn
    {
        get
        {
            return snapTurningOn;
        }
        set
        {
            isDirty = true;
            snapTurningOn = value;
        }
    }

    [SerializeField]MovementType movementType;
    public MovementType MovementType
    {
        get
        {
            return movementType;
        }
        set
        {
            isDirty = true;
            movementType = value;
        }
    }

    [SerializeField]PreferredHand currentHand;
    public PreferredHand CurrentHand
    {
        get
        {
            return currentHand;
        }
        set
        {
            isDirty = true;
            currentHand = value;
        }
    }

    [SerializeField]PreferredHand previousHand;
    public PreferredHand PreviousHand
    {
        get
        {
            return previousHand;
        }
        set
        {
            isDirty = true;
            previousHand = value;
        }
    }

    [SerializeField]PreferredHand preferredHand;
    public PreferredHand PreferredHand
    {
        get
        {
            return preferredHand;
        }
        set
        {
            isDirty = true;
            previousHand = currentHand;
            preferredHand = value;
            currentHand = preferredHand;
        }
    }

    public void ClearDirty()
    {
        Debug.Log("Clearing Dirty Settings...");
        isDirty = false;
    }

}