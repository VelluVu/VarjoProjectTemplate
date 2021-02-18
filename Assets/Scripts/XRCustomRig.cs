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
    
    [Header("Autosearches the references below!")]
    public XRInputManager input;
    public CooldownSystem cooldownSystem;
    List<InputDevice> devices = new List<InputDevice>();
    [HideInInspector]public static bool IsPresent;

    private void Awake() {
        if(input == null)
            input = GetComponent<XRInputManager>();
        if(cooldownSystem == null)
            cooldownSystem = FindObjectOfType<CooldownSystem>();
    }

    private void Start() 
    {      
        StartCoroutine(CheckDevicePresence());      
    }

    private void Update() 
    {
         if(IsPresent)
         {
            AdjustCollider();
         }
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

    IEnumerator Cooldown(float time, bool state)
    {
        yield return new WaitForSeconds(time);
        if(state)
        {
            state = false;
        }
        else
        {
            state = true;
        }
    }
}