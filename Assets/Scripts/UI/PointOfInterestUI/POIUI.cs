using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class represents a popup ui-element, using IUIPopUp interface.
/// </summary>
public class POIUI : MonoBehaviour, IUIPopUp
{
    [SerializeField]Canvas canvas;
    [SerializeField]LookAtHMD lookAtHMD;

    public void Hide()
    {      
        canvas.gameObject.SetActive(false);
    }
    public void PopUp(Transform hmd)
    {     
        canvas.gameObject.SetActive(true);
        if(!lookAtHMD.HasHMDReference())
        {
            lookAtHMD.SetHMDRef(hmd);
        }
    }
}
