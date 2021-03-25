using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class represents the controller, 
/// instantiates the model for hand and 
/// knows which hand controller this is.
/// </summary>
public class XRCustomController : MonoBehaviour
{
    public GameObject modelPrefab;
    GameObject model;
    public bool usingModel;

    public InputDeviceCharacteristics hand;
    InputDevice controller;

    private void Awake() {
        if(hand == InputDeviceCharacteristics.None)
            CheckHand();
    }

    /// <summary>
    /// If forgot to set InputDeviceCharacteristics inside the editor, 
    /// this function autosets the variable by the child index.
    /// 0 == LeftController
    /// 1 == RightController
    /// </summary>
    void CheckHand()
    {
        if(transform.parent.GetChild(0) == transform)
        {
            hand = InputDeviceCharacteristics.Left;
        }
        if(transform.parent.GetChild(1) == transform)
        {
            hand = InputDeviceCharacteristics.Right;
        }
    }

    private void OnEnable() 
    {
        InputDevices.deviceConnected += CheckConnectedDevice;   
        InputDevices.deviceDisconnected += CheckDisconnectedDevice;
    }

    private void OnDisable() 
    {
        InputDevices.deviceConnected -= CheckConnectedDevice;  
        InputDevices.deviceDisconnected -= CheckDisconnectedDevice;
    }

    /// <summary>
    /// Called when this controller is connected instantiates the model.
    /// </summary>
    /// <param name="device">Connected inputdevice</param>
    void CheckConnectedDevice(InputDevice device)
    {
        
        if(device.characteristics.HasFlag(hand))
        {
            Debug.Log(transform.name + " detected!");
            if(usingModel)
                InstantiateModel();
        }
        
    }

    /// <summary>
    /// Called when disconnected.
    /// </summary>
    /// <param name="device">Disconnected inputdevice</param>
    void CheckDisconnectedDevice(InputDevice device)
    {
        if(device.characteristics.HasFlag(hand))
        {
            Debug.Log(transform.name + " disconnected!");
        }
    }

    /// <summary>
    /// Instantiates model if it's null.
    /// </summary>
    void InstantiateModel()
    {
        if(model == null)
        {
            model = Instantiate(modelPrefab, transform.position, transform.rotation, transform);
            model.gameObject.name = hand.ToString() + " Controller Model";
        }
    }

    /// <summary>
    /// Used to get the preferredhand which this controller is.
    /// </summary>
    /// <returns>the correct preferredhand</returns>
    public PreferredHand GetPreferredHand()
    {
        if(hand.HasFlag(InputDeviceCharacteristics.Left))
        {
            return PreferredHand.Left;
        }
        else if(hand.HasFlag(InputDeviceCharacteristics.Right))
        {
            return PreferredHand.Right;
        }
        else
        {
            return PreferredHand.Hmd;
        }
    }
}
