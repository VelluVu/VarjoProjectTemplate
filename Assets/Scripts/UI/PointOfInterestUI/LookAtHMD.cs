using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class makes canvas always face towards hmd.
/// </summary>
public class LookAtHMD : MonoBehaviour
{

    Transform hmd;

    public void SetHMDRef(Transform newhmd)
    {
        hmd = newhmd;
    }

    public bool HasHMDReference()
    {
        return hmd;
    }

    private void Update() {
        
        if(hmd != null)   
        {
            transform.LookAt(hmd);
        }   
    }
}
