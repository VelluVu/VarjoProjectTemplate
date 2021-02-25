using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRTurning : MonoBehaviour
{
    
    XRCustomRig rig;
    Transform hmd;

    private void Awake() 
    {
        rig = GetComponent<XRCustomRig>();
        if(rig != null)
            hmd = rig.hmd;
    }

    private void Start() 
    {
        StartCoroutine(CheckTick());
    }

    IEnumerator CheckTick()
    {
        while(true)
        {
            if(hmd == null)
            {
                Debug.Log("HMD NOT FOUND! ");              
            }
            else
            {
                Debug.Log("Dot hmd.forward o rig.forward: " + Vector3.Dot(hmd.forward, rig.transform.forward));
                Debug.Log("Cross product of : " + Vector3.Cross(hmd.forward, rig.transform.forward));
            }
            yield return new WaitForSeconds(2f);
        }
    }

}
