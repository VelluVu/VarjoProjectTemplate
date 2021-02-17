using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is singleton of settings and holds the scriptable object of settings and functions to change the settings.
/// </summary>
public class XRSettings : Singleton<XRSettings>
{
    public delegate void SettingDelegate(SettingSO newSettings);
    public static event SettingDelegate onSettingLoads;
    public static event SettingDelegate onSettingChange;
    public SettingSO settings;

    private void Start() {
        onSettingLoads?.Invoke(settings);
    }

    public void ChangeMovementType(MovementType type)
    {
        if(settings.movementType != type)
        {
            settings.movementType = type;
            onSettingChange?.Invoke(settings);
        }
    }
}
