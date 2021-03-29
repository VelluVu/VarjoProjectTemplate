using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System;

/// <summary>
/// @Author: Veli-Matti Vuoti
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

    /// <summary>
    /// This function is called when settings go dirty.
    /// Invokes the setting change event.
    /// </summary>
    /// <param name="settings">new settings</param>
    public void OnChangeSettings(GameSettings settings)
    {
        onSettingChange?.Invoke(settings);
    }

    /// <summary>
    /// This function is used to change the movement type.
    /// </summary>
    /// <param name="newMovementType">New movement type</param>
    public void ChangeMovementType(MovementType newMovementType)
    {
        if(settings.MovementType != newMovementType)
        {        
            settings.MovementType = newMovementType;
        }
    }

    /// <summary>
    /// This function is used to change the preferred hand "main hand", 
    /// which user wishes to use for input and actions.
    /// </summary>
    /// <param name="newHand">New preferred hand</param>
    public void ChangePreferredHand(PreferredHand newHand)
    {
        if(settings.CurrentHand != newHand)
        {
            settings.PreferredHand = newHand;
        }
    }

    /// <summary>
    /// This function is used to change snap turning on/off.
    /// </summary>
    /// <param name="newState">New state of snap turning</param>
    public void ChangeSnapTurning(bool newState)
    {
        if(settings.SnapTurningOn != newState)
        {
            settings.SnapTurningOn = newState;
        }
    }

    /// <summary>
    /// This function saves the settings in json format, using json utility.
    /// Save location is application persistent data path "different depending on device" + /SettingsData.json
    /// </summary>
    /// <param name="newSettings">settings object to save passed as parameter</param>
    public void SaveSettings(GameSettings newSettings)
    {
        string data = JsonUtility.ToJson(newSettings);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/SettingsData.json", data);
        Debug.Log("Saved settings to " + Application.persistentDataPath + "/SettingsData.json!");
    }

    /// <summary>
    /// This function saves the settings in json format, using json utility.
    /// Save location is application persistent data path "different depending on device" + /SettingsData.json
    /// </summary>
    public void SaveSettings()
    {
        string data = JsonUtility.ToJson(settings);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/SettingsData.json", data);
        Debug.Log("Saved settings to " + Application.persistentDataPath + "/SettingsData.json!");
    }

    /// <summary>
    /// This function loads the settings from json format using json utility.
    /// if no settings found, sets the default settings.
    /// </summary>
    /// <returns>Returns the loaded settings object</returns>
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

    /// <summary>
    /// Sets the default settings
    /// </summary>
    /// <returns>returns the default settings</returns>
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

/// <summary>
/// This class represents the GameSettings object, 
/// contains all setting variables and it's serializable object.
/// Use XRSetting functions to alter these settings.
/// </summary>
[Serializable]
public class GameSettings
{
    public bool isDirty = false;

    /// <summary>
    /// This property is representing the status of controllers.
    /// </summary>
    /// <value>New state for controllers</value>
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

    /// <summary>
    /// Snap turning state on/off.
    /// </summary>
    /// <value>New state of snap turning</value>
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

    /// <summary>
    /// Current movement type.
    /// </summary>
    /// <value>New movement type</value>
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

    /// <summary>
    /// The current hand property which is connected and the user uses for inputs and interactions.
    /// </summary>
    /// <value>New current hand</value>
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

    /// <summary>
    /// The previously selected main hand property.
    /// </summary>
    /// <value>Previous hand</value>
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

    /// <summary>
    /// The preferred hand "main hand" which is the hand user wants to use for inputs and interractions.
    /// On change sets the previous hand as current hand, then changes current hand to match the preferred hand.
    /// </summary>
    /// <value>New preferred hand</value>
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

    /// <summary>
    /// This function changes isDirty bool to false.
    /// </summary>
    public void ClearDirty()
    {
        Debug.Log("Clearing Dirty Settings...");
        isDirty = false;
    }

}