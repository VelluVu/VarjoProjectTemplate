using UnityEngine;

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
