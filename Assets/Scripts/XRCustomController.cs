using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRCustomController : MonoBehaviour
{
    public GameObject modelPrefab;
    GameObject model;

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

    void CheckConnectedDevice(InputDevice device)
    {
        
        if(device.characteristics.HasFlag(hand))
        {
            Debug.Log(transform.name + " detected!");
            InstantiateModel();
        }
        
    }

    void CheckDisconnectedDevice(InputDevice device)
    {
        if(device.characteristics.HasFlag(hand))
        {
            Debug.Log(transform.name + " disconnected!");
        }
    }

    void InstantiateModel()
    {
        if(model == null)
        {
            model = Instantiate(modelPrefab, transform.position, transform.rotation, transform);
        }
    }
}
