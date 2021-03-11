using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        settingPanels[0].SetText("Preferred Hand: " + XRSettings.Instance.settings.PreferredHand);
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
        settingPanel.SetText("Preferred Hand: " + newPref.ToString().ToUpper());
    }

    public void ChangePreferredHandLeft(SettingPanel settingPanel)
    {
        PreferredHand pref = XRSettings.Instance.settings.PreferredHand;
        PreferredHand newPref = pref;

        if(!XRSettings.Instance.settings.ControllersInUse)
        {
            XRSettings.Instance.ChangePreferredHand(newPref);
            settingPanel.SetText("Preferred Hand: " + newPref.ToString().ToUpper());
            return;
        }

        //Debug.Log(newPref + " " + (int)newPref + " " + (PreferredHand)newPref);

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

        //Debug.Log(newPref + " " + (int)newPref + " " + (PreferredHand)newPref);

        XRSettings.Instance.ChangePreferredHand(newPref);
        settingPanel.SetText("Preferred Hand: " + newPref.ToString().ToUpper());
    }

   
}
