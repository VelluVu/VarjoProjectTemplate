using System;
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

    /// <summary>
    /// Current main input device, which is preferred as main hand or device to use for activating in game actions.
    /// </summary>
    InputDevice currentMainInputDevice;

#region Primary Button variables
    public bool mainPrimaryButtonPressed = false;
    public bool previousMainPrimaryButtonPressed = false;

    public bool hmdPrimaryButtonPress = false;
    public bool leftControllerPrimaryButtonPress = false;
    public bool rightControllerPrimaryButtonPress = false;
    public bool previousHMDPrimaryButtonPress = false;
    public bool previousLeftControllerPrimaryButtonPress = false;
    public bool previousRightControllerPrimaryButtonPress = false;

    public delegate void PrimaryButtonDelegate(InputDeviceCharacteristics deviceCharacteristics);
    public static event PrimaryButtonDelegate onPrimaryButtonDown;
    public static event PrimaryButtonDelegate onPrimaryButtonUp;

    public delegate void MainInputPrimaryButtonPressedDelegate();
    public static event MainInputPrimaryButtonPressedDelegate onMainPrimaryButtonPressedDown;
    public static event MainInputPrimaryButtonPressedDelegate onMainPrimaryButtonPressedUp;
#endregion

#region Secondary Button variables
    public bool mainSecondaryButtonPressed = false;
    public bool previousMainSecondaryButtonPressed = false;

    public bool hmdSecondaryButtonPress = false;
    public bool leftControllerSecondaryButtonPress = false;
    public bool rightControllerSecondaryButtonPress = false;
    public bool previousHMDSecondaryButtonPress = false;
    public bool previousLeftControllerSecondaryButtonPress = false;
    public bool previousRightControllerSecondaryButtonPress = false;

    public delegate void SecondaryButtonDelegate(InputDeviceCharacteristics deviceCharacteristics);
    public static event SecondaryButtonDelegate onSecondaryButtonDown;
    public static event SecondaryButtonDelegate onSecondaryButtonUp;

    public delegate void MainInputSecondaryButtonPressedDelegate();
    public static event MainInputSecondaryButtonPressedDelegate onMainSecondaryButtonPressedDown;
    public static event MainInputSecondaryButtonPressedDelegate onMainSecondaryButtonPressedUp;
#endregion

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

    /// <summary>
    /// This function is called on Right Controller Is Present event
    /// to initialize the right controller input device,
    /// for left controller inputchecking.
    /// </summary>
    /// <param name="controller"></param>
    private void InitRightController(InputDevice controller)
    {
        rightController = controller;
    }

    /// <summary>
    /// This function is called on Left Controller Is Present event
    /// to initialize the right controller input device,
    /// for right controller input checking.
    /// </summary>
    /// <param name="controller"></param>
    private void InitLeftController(InputDevice controller)
    {
        leftController = controller;
    }

    /// <summary>
    /// This function is called on Head Mounted Device Is Present event
    /// to initialize the hmd input device,
    /// for hmd input checking.
    /// </summary>
    /// <param name="controller"></param>
    private void InitHMD(InputDevice controller)
    {
        hmd = controller;
    }

    /// <summary>
    /// This function Returns raw true or false primary button press, depending on the parameter Preferredhand.
    /// </summary>
    /// <param name="hand">The Hand which press is checked</param>
    /// <returns>returns bool of button being pressed or not</returns>
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

    /// <summary>
    /// This function is called by XRRig, when settings change.
    /// </summary>
    /// <param name="mainInputDevice"></param>
    public void UpdateMainInputDevice(InputDevice mainInputDevice)
    {
        currentMainInputDevice = mainInputDevice;
    }

    private void Update()
    {
        CheckPrimaryButtonInput();
        CheckSecondaryButtonInput();
    }

  
    /// <summary>
    /// Checks the primary press inputs for all main devices.
    /// </summary>
    private void CheckPrimaryButtonInput()
    {
        if(currentMainInputDevice != null)
        {
            bool mainInputPressed;
            currentMainInputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out mainInputPressed);
            mainPrimaryButtonPressed = mainInputPressed;
            if(previousMainPrimaryButtonPressed != mainInputPressed)
            {
                previousMainPrimaryButtonPressed = mainInputPressed;

                if(mainInputPressed)
                {
                    onMainPrimaryButtonPressedDown?.Invoke();
                }
                else
                {
                    onMainPrimaryButtonPressedUp?.Invoke();
                }
            }
        }

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

    private void CheckSecondaryButtonInput()
    {
        if(currentMainInputDevice != null)
        {
            bool mainSecondaryInputPressed;
            currentMainInputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out mainSecondaryInputPressed);
            mainSecondaryButtonPressed = mainSecondaryInputPressed;
            if(previousMainSecondaryButtonPressed != mainSecondaryInputPressed)
            {
                previousMainSecondaryButtonPressed = mainSecondaryInputPressed;

                if(mainSecondaryInputPressed)
                {
                    onMainSecondaryButtonPressedDown?.Invoke();
                }
                else
                {
                    onMainSecondaryButtonPressedUp?.Invoke();
                }
            }
        }

        if (hmd != null)
        {
            bool hmdSecondaryPressed;
            hmd.TryGetFeatureValue(CommonUsages.secondaryButton, out hmdSecondaryPressed);
            hmdSecondaryButtonPress = hmdSecondaryPressed;
            if (previousHMDSecondaryButtonPress != hmdSecondaryPressed)
            {
                previousHMDSecondaryButtonPress = hmdSecondaryPressed;

                if (hmdSecondaryPressed)
                {
                    onSecondaryButtonDown?.Invoke(InputDeviceCharacteristics.HeadMounted);
                }
                else
                {
                    onSecondaryButtonUp?.Invoke(InputDeviceCharacteristics.HeadMounted);
                }
            }
        }

        if (leftController != null)
        {
            bool leftControllerSecondaryPressed;
            leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out leftControllerSecondaryPressed);
            leftControllerSecondaryButtonPress = leftControllerSecondaryPressed;
            if (previousLeftControllerSecondaryButtonPress != leftControllerSecondaryPressed)
            {
                previousLeftControllerSecondaryButtonPress = leftControllerSecondaryPressed;

                if (leftControllerSecondaryPressed)
                {
                    onSecondaryButtonDown?.Invoke(InputDeviceCharacteristics.Left);
                }
                else
                {
                    onSecondaryButtonUp?.Invoke(InputDeviceCharacteristics.Left);
                }
            }
        }

        if (rightController != null)
        {
            bool rightControllerSecondaryPressed;
            rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out rightControllerSecondaryPressed);
            rightControllerSecondaryButtonPress = rightControllerSecondaryPressed;
            if (previousRightControllerSecondaryButtonPress != rightControllerSecondaryPressed)
            {
                previousRightControllerSecondaryButtonPress = rightControllerSecondaryPressed;

                if (rightControllerSecondaryPressed)
                {
                    onSecondaryButtonDown?.Invoke(InputDeviceCharacteristics.Right);
                }
                else
                {
                    onSecondaryButtonUp?.Invoke(InputDeviceCharacteristics.Right);
                }
            }
        }
    }
}