using System;
using UnityEngine;
using UnityEngine.XR;

public class XRInputManager : MonoBehaviour
{
    InputDevice hmd;
    InputDevice rightController;
    InputDevice leftController;

    public bool hmdPrimaryButtonPress = false;
    public bool leftControllerPrimaryButtonPress = false;
    public bool rightControllerPrimaryButtonPress = false;
    public bool previousHMDPrimaryButtonPress = false;
    public bool previousLeftControllerPrimaryButtonPress = false;
    public bool previousRightControllerPrimaryButtonPress = false;

    public delegate void PrimaryButtonDelegate();
    public static event PrimaryButtonDelegate onHMDPrimaryButtonDown;
    public static event PrimaryButtonDelegate onHMDPrimaryButtonUp;
    public static event PrimaryButtonDelegate onLeftControllerPrimaryButtonDown;
    public static event PrimaryButtonDelegate onLeftControllerPrimaryButtonUp;
    public static event PrimaryButtonDelegate onRightControllerPrimaryButtonDown;
    public static event PrimaryButtonDelegate onRightControllerPrimaryButtonUp;

    private void OnEnable() 
    {
        XRCustomRig.onHeadMountedDeviceIsPresent += InitHMD;
        XRCustomRig.onLeftControllerIsPresent += InitLeftController;
        XRCustomRig.onRightControllerIsPresent += InitRightController;
    }

    private void OnDisable() 
    {
        XRCustomRig.onHeadMountedDeviceIsPresent -= InitHMD;
        XRCustomRig.onLeftControllerIsPresent -= InitLeftController;
        XRCustomRig.onRightControllerIsPresent -= InitRightController;
       
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

    public bool GetPrimaryButtonControlPress(PreferredHand hand)
    {
        if(hand == PreferredHand.Left)
        {
            return leftControllerPrimaryButtonPress;
        }
        if(hand == PreferredHand.Right) 
        {
            return rightControllerPrimaryButtonPress;
        }

        return false;
    }

    private void Update()
    {
        CheckPrimaryButtonInput();
    }

    private void CheckPrimaryButtonInput()
    {
        if (hmd != null)
        {
            bool hmdPrimaryPressed;
            hmd.TryGetFeatureValue(CommonUsages.primaryButton, out hmdPrimaryPressed);
            hmdPrimaryButtonPress = hmdPrimaryPressed;
            if (previousHMDPrimaryButtonPress != hmdPrimaryPressed)
            {
                previousHMDPrimaryButtonPress = hmdPrimaryPressed;

                if (hmdPrimaryPressed)
                {
                    onHMDPrimaryButtonDown?.Invoke();
                }
                else
                {
                    onHMDPrimaryButtonUp?.Invoke();
                }
            }
        }

        if (leftController != null)
        {
            bool leftControllerPrimaryPressed;
            leftController.TryGetFeatureValue(CommonUsages.primaryButton, out leftControllerPrimaryPressed);
            leftControllerPrimaryButtonPress = leftControllerPrimaryPressed;
            if (previousLeftControllerPrimaryButtonPress != leftControllerPrimaryPressed)
            {
                previousLeftControllerPrimaryButtonPress = leftControllerPrimaryPressed;

                if (leftControllerPrimaryPressed)
                {
                    onLeftControllerPrimaryButtonDown?.Invoke();
                }
                else
                {
                    onLeftControllerPrimaryButtonUp?.Invoke();
                }
            }
        }

        if (rightController != null)
        {
            bool rightControllerPrimaryPressed;
            rightController.TryGetFeatureValue(CommonUsages.primaryButton, out rightControllerPrimaryPressed);
            rightControllerPrimaryButtonPress = rightControllerPrimaryPressed;
            if (previousRightControllerPrimaryButtonPress != rightControllerPrimaryPressed)
            {
                previousRightControllerPrimaryButtonPress = rightControllerPrimaryPressed;

                if (rightControllerPrimaryPressed)
                {
                    onRightControllerPrimaryButtonDown?.Invoke();
                }
                else
                {
                    onRightControllerPrimaryButtonUp?.Invoke();
                }
            }
        }
    }
}
