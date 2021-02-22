using System;
using UnityEngine;
using UnityEngine.XR;

public class XRInputManager : MonoBehaviour
{
    [HideInInspector]public PrimaryButtonWatcher watcher;
    InputDevice hmd;
    InputDevice rightController;
    InputDevice leftController;

    public bool onPrimaryButtonPress = false;
    private bool previousHMDPrimaryButtonPress = false;
    private bool previousLeftControllerPrimaryButtonPress = false;
    private bool previousRightControllerPrimaryButtonPress = false;

    public delegate void PrimaryButtonDelegate();
    public static event PrimaryButtonDelegate onHMDPrimaryButtonDown;
    public static event PrimaryButtonDelegate onHMDPrimaryButtonUp;
    public static event PrimaryButtonDelegate onLeftControllerPrimaryButtonDown;
    public static event PrimaryButtonDelegate onLeftControllerPrimaryButtonUp;
    public static event PrimaryButtonDelegate onRightControllerPrimaryButtonDown;
    public static event PrimaryButtonDelegate onRightControllerPrimaryButtonUp;

    private void Awake() {
        watcher = GetComponent<PrimaryButtonWatcher>();
    }

    private void Start() {
        watcher.primaryButtonPress.AddListener(onPrimaryButtonEvent);
    }

    private void OnEnable() 
    {
        XRCustomRig.headMountedDeviceIsPresent += InitHMD;
        XRCustomRig.leftControllerIsPresent += InitLeftController;
        XRCustomRig.rightControllerIsPresent += InitRightController;

    }

    private void OnDisable() 
    {
        XRCustomRig.headMountedDeviceIsPresent -= InitHMD;
        XRCustomRig.leftControllerIsPresent -= InitLeftController;
        XRCustomRig.rightControllerIsPresent -= InitRightController;
       
    }

    public void onPrimaryButtonEvent(bool pressed) 
    {    
        onPrimaryButtonPress = pressed;
    }

    private void InitRightController(InputDevice controller)
    {
        rightController = controller;
    }

    private void InitLeftController(InputDevice controller)
    {
        leftController = controller;
    }

    private void InitHMD(InputDevice controller)
    {
        hmd = controller;
    }

    private void Update() 
    {
        if(hmd != null)
        {
            bool hmdPrimaryPressed;
            hmd.TryGetFeatureValue(CommonUsages.primaryButton, out hmdPrimaryPressed);
            if(previousHMDPrimaryButtonPress != hmdPrimaryPressed)
            {
                previousHMDPrimaryButtonPress = hmdPrimaryPressed;
                
                if(hmdPrimaryPressed)
                {
                    onHMDPrimaryButtonDown?.Invoke();
                }
                else
                {
                    onHMDPrimaryButtonUp?.Invoke();
                }
        
                //Debug.Log("hmdPrimaryPressed " + hmdPrimaryPressed);
            }
        }

        if(leftController != null)
        {
            bool leftControllerPrimaryPressed;
            leftController.TryGetFeatureValue(CommonUsages.primaryButton, out leftControllerPrimaryPressed);
            if(previousLeftControllerPrimaryButtonPress != leftControllerPrimaryPressed)
            {  
                previousLeftControllerPrimaryButtonPress = leftControllerPrimaryPressed;
            
                if(leftControllerPrimaryPressed)
                {
                    onLeftControllerPrimaryButtonDown?.Invoke();
                }
                else
                {
                    onLeftControllerPrimaryButtonUp?.Invoke();
                }
            
                //Debug.Log("leftControllerPrimaryPressed " + leftControllerPrimaryPressed);
            }
        }

        if(rightController != null)
        {
            bool rightControllerPrimaryPressed;
            rightController.TryGetFeatureValue(CommonUsages.primaryButton, out rightControllerPrimaryPressed);
            if(previousRightControllerPrimaryButtonPress != rightControllerPrimaryPressed)
            {
                previousRightControllerPrimaryButtonPress = rightControllerPrimaryPressed;
               
                if(rightControllerPrimaryPressed)
                {
                    onRightControllerPrimaryButtonDown?.Invoke();
                }
                else
                {
                    onRightControllerPrimaryButtonUp?.Invoke();
                }
                
                //Debug.Log("rightControllerPrimaryPressed" + rightControllerPrimaryPressed);
            }
        }
    }


}
