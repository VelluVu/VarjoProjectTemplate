using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class sets SettingsUI Button listeners and initializes setting panels.
/// </summary>
public class SettingsMenuUI : MonoBehaviour
{
    public GameObject settingPanelPrefab;
    public List<SettingPanel> settingPanels = new List<SettingPanel>();

    private void Start() 
    {
        GameObject preferredHandPanel = Instantiate(settingPanelPrefab, transform);
        preferredHandPanel.name = "PreferredHand_Panel";
        settingPanels.Add(preferredHandPanel.GetComponent<SettingPanel>());

        GameObject movementTypePanel = Instantiate(settingPanelPrefab, transform);
        movementTypePanel.name = "MovementType_Panel";
        settingPanels.Add(movementTypePanel.GetComponent<SettingPanel>());

        GameObject snapTurningPanel = Instantiate(settingPanelPrefab, transform);
        snapTurningPanel.name = "SnapTurning_Panel";
        settingPanels.Add(snapTurningPanel.GetComponent<SettingPanel>());

        settingPanels[0].changeLeftButton.onClick.AddListener(()=>ChangePreferredHandLeft(settingPanels[0]));
        settingPanels[0].changeRightButton.onClick.AddListener(()=>ChangePreferredHandRight(settingPanels[0]));
        ChangeText("Preferred Hand: " + XRSettings.Instance.settings.PreferredHand, settingPanels[0]);

        settingPanels[1].changeLeftButton.onClick.AddListener(()=>ChangeMovementTypeLeft(settingPanels[1]));
        settingPanels[1].changeRightButton.onClick.AddListener(()=>ChangeMovementTypeRight(settingPanels[1]));
        ChangeText("Movement Type: " + XRSettings.Instance.settings.MovementType.ToString(), settingPanels[1]);

        settingPanels[2].changeLeftButton.onClick.AddListener(()=>ChangeSnapTurning(settingPanels[2]));
        settingPanels[2].changeRightButton.onClick.AddListener(()=>ChangeSnapTurning(settingPanels[2]));
        ChangeText(GetSnapStatusText(XRSettings.Instance.settings.SnapTurningOn), settingPanels[2]);

    }

    public void ChangePreferredHandRight(SettingPanel settingPanel)
    {
        PreferredHand pref = XRSettings.Instance.settings.PreferredHand;
        PreferredHand newPref = pref;
        
        //Debug.Log(newPref + " " + (int)newPref + " " + (PreferredHand)newPref);

        if(pref == PreferredHand.Hmd && XRSettings.Instance.settings.LeftControllerConnected)
        {
            newPref = PreferredHand.Left;
        }
        else if(pref == PreferredHand.Left && XRSettings.Instance.settings.RightControllerConnected)
        {
            newPref = PreferredHand.Right;
        }
        else if(pref == PreferredHand.Right)
        {
            newPref = PreferredHand.Hmd;
        }

        //Debug.Log(newPref + " " + (int)newPref + " " + (PreferredHand)newPref);

        XRSettings.Instance.ChangePreferredHand(newPref);
        settingPanel.SetText("Main Device: " + newPref.ToString());
    }

    public void ChangePreferredHandLeft(SettingPanel settingPanel)
    {
        PreferredHand pref = XRSettings.Instance.settings.PreferredHand;
        PreferredHand newPref = pref;

        if(!XRSettings.Instance.settings.ControllersInUse)
        {
            XRSettings.Instance.ChangePreferredHand(newPref);
            settingPanel.SetText("Main Device: " + newPref.ToString());
            return;
        }

        //Can't change to controllers if no controllers are connected!
        if(pref == PreferredHand.Hmd && XRSettings.Instance.settings.RightControllerConnected)
        {
            newPref = PreferredHand.Right;
        }
        else if(pref == PreferredHand.Right && XRSettings.Instance.settings.LeftControllerConnected)
        {
            newPref = PreferredHand.Left;
        }
        else if(pref == PreferredHand.Left)
        {
            newPref = PreferredHand.Hmd;
        }

        XRSettings.Instance.ChangePreferredHand(newPref);
        settingPanel.SetText("Main Device: " + newPref.ToString());
    }

    public void ChangeMovementTypeRight(SettingPanel settingPanel)
    {
        MovementType currentMovementType = XRSettings.Instance.settings.MovementType;
        int enumVal = (int)currentMovementType;
        int lastVal = (int)Enum.GetValues(typeof(MovementType)).Cast<MovementType>().Last();
        
        enumVal++;

        if(enumVal > lastVal)
            enumVal = 0;
        
        XRSettings.Instance.ChangeMovementType((MovementType)enumVal);
        ChangeText("Movement Type: " + ((MovementType)enumVal).ToString(), settingPanel);
    }

    public void ChangeMovementTypeLeft(SettingPanel settingPanel)
    {
        MovementType currentMovementType = XRSettings.Instance.settings.MovementType;
        int enumVal = (int)currentMovementType;
        int lastVal = (int)Enum.GetValues(typeof(MovementType)).Cast<MovementType>().Last();
        
        enumVal--;

        if(enumVal < 0)
            enumVal = lastVal;
        
        XRSettings.Instance.ChangeMovementType((MovementType)enumVal);
        ChangeText("Movement Type: " + ((MovementType)enumVal).ToString(), settingPanel);
    }

    public void ChangeText(string text, SettingPanel settingPanel)
    {
        settingPanel.SetText(text);
    }

    public void ChangeSnapTurning(SettingPanel settingPanel)
    {
        bool state = !XRSettings.Instance.settings.SnapTurningOn;
        XRSettings.Instance.ChangeSnapTurning(state);
        settingPanel.SetText(GetSnapStatusText(state));
    }

    /// <summary>
    /// Just a helper function to convert false true into off on.
    /// </summary>
    /// <param name="state">the boolean value</param>
    /// <returns>ON||OFF</returns>
    public string GetSnapStatusText(bool state)
    {
        string text = "Snap Turning: ";
        if(state == false)
            text += "OFF";
        else
            text += "ON";

        return text;
    }  
}
