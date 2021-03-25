using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class Triggers UI activation
/// </summary>
public class UITrigger : MonoBehaviour
{

    IUIPopUp popUpUI;

    private void Awake() {
        popUpUI = GetComponentInParent<IUIPopUp>();
    }

    /// <summary>
    /// Uses the state matching functions of pop up ui
    /// </summary>
    /// <param name="state">boolean for Show or hide</param>
    /// <param name="hmd">hmd to pass for pop up ui for direction calculations</param>
    public void TriggerUI(bool state, Transform hmd)
    {
        if(state)
        {         
            popUpUI.PopUp(hmd);
        }
        else
        {         
            popUpUI.Hide();
        }
    }
}
