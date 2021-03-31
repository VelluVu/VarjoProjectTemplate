using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class makes canvas always face towards hmd.
/// </summary>
public class LookAtHMD : MonoBehaviour
{

    Transform hmd;

    /// <summary>
    /// This function sets the reference of HMD.
    /// </summary>
    /// <param name="newhmd"></param>
    public void SetHMDRef(Transform newhmd)
    {
        hmd = newhmd;
    }

    /// <summary>
    /// This function, is to check if object has the reference for HMD.
    /// </summary>
    /// <returns></returns>
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
