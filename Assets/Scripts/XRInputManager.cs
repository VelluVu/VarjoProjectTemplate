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

    public delegate void PrimaryButtonDelegate(InputDeviceCharacteristics deviceCharacteristics);
    public static event PrimaryButtonDelegate onPrimaryButtonDown;
    public static event PrimaryButtonDelegate onPrimaryButtonUp;

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
                    onPrimaryButtonDown?.Invoke(InputDeviceCharacteristics.HeadMounted);
                }
                else
                {
                    onPrimaryButtonUp?.Invoke(InputDeviceCharacteristics.HeadMounted);
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
                    onPrimaryButtonDown?.Invoke(InputDeviceCharacteristics.Left);
                }
                else
                {
                    onPrimaryButtonUp?.Invoke(InputDeviceCharacteristics.Left);
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
                    onPrimaryButtonDown?.Invoke(InputDeviceCharacteristics.Right);
                }
                else
                {
                    onPrimaryButtonUp?.Invoke(InputDeviceCharacteristics.Right);
                }
            }
        }
    }
}