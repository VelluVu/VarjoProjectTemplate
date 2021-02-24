using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Represents the rig and contains the rig related variables and functions.
/// </summary>
public class XRCustomRig : MonoBehaviour
{
   
    public Transform hmd;
    public XRCustomController leftController;
    public XRCustomController rightController;
    public Transform body;
    
    [Header("Autosearches the references below!")]
    public XRInputManager input;
    public CooldownSystem cooldownSystem;

    InputDevice headMounted;
    List<InputDevice> controllers = new List<InputDevice>();

    [HideInInspector]public bool isPresent;
    [HideInInspector]public bool hasControllers;
    [HideInInspector]public bool hasLeftController;
    [HideInInspector]public bool hasRightController;

    public delegate void DevicesDelegate(List<InputDevice> devices);
    public static event DevicesDelegate onControllersArePresent;
    public static event DevicesDelegate onControllersNotPresent;

    public delegate void DeviceDelegate(InputDevice controller);
    public static event DeviceDelegate onHeadMountedDeviceIsPresent;
    public static event DeviceDelegate onLeftControllerIsPresent;
    public static event DeviceDelegate onRightControllerIsPresent;

    public delegate void DeviceDisconnectedDelegate();
    public static event DeviceDisconnectedDelegate onHeadMountedDeviceDisconnected;
    public static event DeviceDisconnectedDelegate onLeftControllerDisconnected;
    public static event DeviceDisconnectedDelegate onRightControllerDisconnected;

    private void Awake() {
        if(input == null)
            input = GetComponent<XRInputManager>();
        if(cooldownSystem == null)
            cooldownSystem = FindObjectOfType<CooldownSystem>();
    }

    private void OnEnable() {
        InputDevices.deviceDisconnected += DeviceDisconnected;
        InputDevices.deviceConnected += DeviceConnected;
    }

    private void OnDisable() {
        InputDevices.deviceDisconnected -= DeviceDisconnected;
        InputDevices.deviceConnected -= DeviceConnected;
    }

    void DeviceDisconnected(InputDevice device)
    {
        Debug.Log(device.characteristics + " Disconnected!");
        if(device.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
        {
            CheckControllers();
        }
        if(device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
        {
            onLeftControllerDisconnected?.Invoke();
            hasLeftController = false;
        }
        else if(device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
        {
            onRightControllerDisconnected?.Invoke();
            hasRightController = false;         
        }
        else if(device.characteristics.HasFlag(InputDeviceCharacteristics.HeadMounted))
        {
            isPresent = false;
            onHeadMountedDeviceDisconnected?.Invoke();
        }
    }

    void DeviceConnected(InputDevice device)
    {
        Debug.Log(device.characteristics + " Connected!");

        if(device.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
        {
            CheckControllers();
        }
        if(device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
        {
            onLeftControllerIsPresent?.Invoke(device);
            hasLeftController = true;
        }
        else if(device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
        {    
            onRightControllerIsPresent?.Invoke(device);
            hasRightController = true;
        }
        else if(device.characteristics.HasFlag(InputDeviceCharacteristics.HeadMounted))
        {       
            isPresent = true;
            onHeadMountedDeviceIsPresent?.Invoke(device);
            headMounted = device;
        }
    }

    private void Update() 
    {
         if(isPresent)
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
        Transform c = rightController.transform;
        if(hand == PreferredHand.Right)
        {
            c = rightController.transform;
        }
        if(hand == PreferredHand.Left)
        {
            c = leftController.transform;
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

    public void CheckControllers()
    {
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, controllers);
    
        if(controllers.Count == 0 && hasControllers)
        {
            hasControllers = false;
            onControllersNotPresent?.Invoke(controllers);
        }
        if(controllers.Count > 0)
        {
            onControllersArePresent?.Invoke(controllers);
            hasControllers = true;
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