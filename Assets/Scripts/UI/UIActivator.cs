using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates UI when hit UItrigger
/// </summary>
public class UIActivator : MonoBehaviour
{
    XRCustomRig rig;
    [SerializeField]float raycastLength = 40f;
    [SerializeField]LayerMask uiActivateMask;

    UITrigger currentUITrigger = null;

    private void Awake() {
        rig = GetComponent<XRCustomRig>();
    }
    private void Update() 
    {
        RaycastHit hit;
        bool hits = Physics.Raycast(rig.hmd.position, rig.hmd.forward, out hit, raycastLength, uiActivateMask);

        if(hits)
        {
            
            UITrigger trigger = hit.transform.GetComponent<UITrigger>();       

            if(trigger != null && currentUITrigger != trigger)
            {
              
                if(currentUITrigger != null)
                {   
                    
                    currentUITrigger.TriggerUI(false, rig.hmd);
                }

                trigger.TriggerUI(true, rig.hmd);
                currentUITrigger = trigger;
            }
        }     
        else
        {
            if(currentUITrigger != null)
            {
                currentUITrigger.TriggerUI(false,rig.hmd);
                currentUITrigger = null;
            }
        }
    }
}
