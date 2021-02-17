using UnityEngine;

/// <summary>
/// Triggers UI activation
/// </summary>
public class UITrigger : MonoBehaviour
{

    IUIPopUp popUpUI;

    private void Awake() {
        popUpUI = GetComponentInParent<IUIPopUp>();
    }

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
