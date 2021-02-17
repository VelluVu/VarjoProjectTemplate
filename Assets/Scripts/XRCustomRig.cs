using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Represents the rig and holds static rig related values.
/// </summary>
public class XRCustomRig : MonoBehaviour
{
   
    public Transform hmd;
    public Transform leftController;
    public Transform rightController;
    public Transform body;
    List<InputDevice> devices = new List<InputDevice>();
    public PrimaryButtonWatcher watcher;
    public static bool IsPresent;
    public bool onPrimaryButtonPress = false;

    private void Awake() {
        watcher = GetComponent<PrimaryButtonWatcher>();
    }

    private void Start() 
    {      
        StartCoroutine(CheckDevicePresence());
        watcher.primaryButtonPress.AddListener(onPrimaryButtonEvent);
    }

    private void Update() 
    {
         if(IsPresent)
         {
            AdjustCollider();
         }
    }
    public void onPrimaryButtonEvent(bool pressed) 
    {
        onPrimaryButtonPress
 = pressed;
    }

    public void AdjustCollider()
    {
        float distanceFromHeadToGround = Vector3.Distance(hmd.position, transform.position);

        Collider col = body.GetComponent<Collider>();

        if(col != null)
            if(col is CapsuleCollider)
            {            
                CapsuleCollider thecol = (CapsuleCollider)col;
                thecol.height = distanceFromHeadToGround;
                thecol.center = new Vector3(thecol.center.x, distanceFromHeadToGround/2, thecol.center.z);            
            }
    }


    IEnumerator CheckDevicePresence()
    {
        while(true)
        {
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeadMounted,devices);
            IsPresent = devices.Count > 0;
            /*
            foreach (var item in devices)
            {
                Debug.Log("Device " + item.name + " is Active : " + item.subsystem.running);
            }*/
            
            yield return new WaitForSeconds(5f);
        } 
    }
}