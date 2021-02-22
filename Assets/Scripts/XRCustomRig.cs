using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    InputDevice headMounted;
    List<InputDevice> controllers = new List<InputDevice>();

    [HideInInspector]public bool IsPresent;
    [HideInInspector]public bool HasControllers;
    [HideInInspector]public bool HasLeftController;
    [HideInInspector]public bool HasRightController;

    public delegate void DeviceDelegate(InputDevice controller);

    public static event DeviceDelegate headMountedDeviceIsPresent;
    public static event DeviceDelegate controllerIsPresent;
    public static event DeviceDelegate leftControllerIsPresent;
    public static event DeviceDelegate rightControllerIsPresent;

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
    
    public InputDevice GetControllerDevice(PreferredHand hand)
    {
        XRNode node = XRNode.RightHand;
        if(hand == PreferredHand.Right)
        {
            node = XRNode.RightHand;
        }
        if(hand == PreferredHand.Left)
        {
            node = XRNode.LeftHand;
        }
        
        return InputDevices.GetDeviceAtXRNode(node);
    }

    public Transform GetControllerTransform(PreferredHand hand)
    {
        Transform c = rightController;
        if(hand == PreferredHand.Right)
        {
            c = rightController;
        }
        if(hand == PreferredHand.Left)
        {
            c = leftController;
        }

        return c;
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

    /// <summary>
    /// Activates Device Haptic.
    /// //Haptic(item, 0.5f, 0.2f); for short pulse.
    /// </summary>
    /// <param name="device"></param>
    /// <param name="amplitude"></param>
    /// <param name="duration"></param>
    public static void Haptic(InputDevice device, float amplitude, float duration)
    {    
        HapticCapabilities capabilities;
        bool canHaptic = device.TryGetHapticCapabilities(out capabilities);
        if(canHaptic)
        {
            Debug.Log("HapticData for "+ device.name);
            if(capabilities.supportsBuffer)
            {
                Debug.Log("haptic buffer frequency: " + capabilities.bufferFrequencyHz);
                Debug.Log("haptic buffer max size: " + capabilities.bufferMaxSize);
                Debug.Log("haptic buffer optimal size: " + capabilities.bufferOptimalSize);
            }
            if(capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(capabilities.numChannels, amplitude, duration);
            }
        }
    }

    /// <summary>
    /// Finds the input devices on runtime.
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckDevicePresence()
    {
        while(true)
        {
            
            headMounted = InputDevices.GetDeviceAtXRNode(XRNode.Head);
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, controllers);
            Debug.Log("Amount of controllers: " + controllers.Count);

            if(controllers.Count == 0)
            {
                HasControllers = false;
                HasLeftController = false;
                HasRightController = false;
            }
            
            if(!headMounted.isValid)
            {
                IsPresent = false;
            }

            //Debug.Log("Characteristics of headmounted device <HMD>: " + headMounted.characteristics);
            if(headMounted.isValid && !IsPresent)
            {
                IsPresent = true;      
                headMountedDeviceIsPresent?.Invoke(headMounted);      
            }

            Debug.Log("Presence of hmd: " + IsPresent);
            
            int val = 1;

            foreach (var item in controllers)
            {
                //Debug.Log(val + " Controller characteristics: " + item.characteristics + " IsValid: " + item.isValid );
                if(item.isValid && !HasControllers)
                {
                    controllerIsPresent?.Invoke(item);
                    HasControllers = true;
                }

                if(item.characteristics.HasFlag(InputDeviceCharacteristics.Left) && item.isValid && !HasLeftController)
                {
                    Debug.Log("Left Controller found! " + val);
                    leftControllerIsPresent?.Invoke(item);
                    HasLeftController = item.isValid;
                }

                if(item.characteristics.HasFlag(InputDeviceCharacteristics.Right) && item.isValid && !HasRightController)
                {
                    Debug.Log("Right Controller Found " + val);               
                    rightControllerIsPresent?.Invoke(item);
                    HasRightController = item.isValid;
                }

                //Haptic(item, 0.5f, 0.2f);
                
                val += 1;
            }
           
            
            yield return new WaitForSeconds(10f);
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