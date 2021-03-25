using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// Captures the inputs and invokes events for different states of presses.
/// </summary>
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

    /// <summary>
    /// Returns true or false primary button press, depending on the parameter Preferredhand.
    /// </summary>
    /// <param name="hand">The Hand which press is checked</param>
    /// <returns></returns>
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
        if(hand == PreferredHand.Hmd)
        {
            return hmdPrimaryButtonPress;
        }

        return false;
    }

    private void Update()
    {
        CheckPrimaryButtonInput();
    }

    /// <summary>
    /// Checks the primary press inputs for all main devices.
    /// </summary>
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