using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class handles the teleportation movement.
/// </summary>
public class XRTeleportMovement : IXRMovement
{
    XRMovementSwitch control;
    RaycastHit hit;
    public delegate void XRTeleportDelegate(RaycastHit hit);
    public static event XRTeleportDelegate onTeleportPossible;
    public static event XRTeleportDelegate onTeleportNotPossible;

    bool canTele = false;

    public void StartState(XRMovementSwitch control)
    {
        this.control = control;

        XRInputManager.onPrimaryButtonDown += OnButtonDown;
        
        Debug.Log("Chosen MovementType " + this);
    }

    public void ExitState()
    {
        XRInputManager.onPrimaryButtonDown -= OnButtonDown;
    
        onTeleportNotPossible?.Invoke(hit);
        Debug.Log("Exited MovementType " + this);
    }

    /// <summary>
    /// This function is called,
    /// when primarybutton is pressed.
    /// Uses the correct function, 
    /// when Input Device Characteristics match.
    /// </summary>
    /// <param name="deviceCharacteristics">Input Device Characteristics can be used to check the device</param>
    private void OnButtonDown(InputDeviceCharacteristics deviceCharacteristics)
    {
        if(deviceCharacteristics == InputDeviceCharacteristics.HeadMounted)
        {
            OnHMDButtonDown();
        }
        else if(deviceCharacteristics == InputDeviceCharacteristics.Left)
        {
            OnLeftControllerButtonDown();
        }
        else if(deviceCharacteristics == InputDeviceCharacteristics.Right)
        {
            OnRightControllerButtonDown();
        }
    }

    /// <summary>
    /// This function Calls the Teleport function, 
    /// when can teleport and hmd device primary button pressed
    /// </summary>
    private void OnHMDButtonDown()
    {
        if(canTele)
        {
            Teleport();
        }
    }

    /// <summary>
    /// This function Calls the Teleport function, 
    /// when teleporting is possible and XRMovementSwitch movement hand is left.
    /// </summary>
    private void OnLeftControllerButtonDown()
    {
        if(canTele && control.preferredHand == PreferredHand.Left)
        {
            Teleport();
        }
    }

    /// <summary>
    /// This function Calls the Teleport function, 
    /// when teleporting is possible and XRMovementSwitch movement hand is right.
    /// </summary>
    private void OnRightControllerButtonDown()
    {
        if(canTele && control.preferredHand == PreferredHand.Right)
        {
            Teleport();
        }
    }

    public void UpdateState()
    {
        if(control.usingControllers)
        {
            WithControllers();
        }
        else
        {
            WithHMD();
        } 
    }

    /// <summary>
    /// This function checks the possibility of teleport with controllers, 
    /// by raycasting and hitting only teleport layer.
    /// Also calls the events for teleport possibility 
    /// for MoveEventListener to adjust the visual indicator.
    /// </summary>
    void WithControllers()
    {
        Transform c = control.rig.GetControllerTransform(control.preferredHand);
        bool hits = Physics.Raycast(c.position, 
                                    c.forward, 
                                    out hit, 
                                    control.movementVariables.raycastDistance, 
                                    control.movementVariables.raycastLayerMask);
        
        if(hits)
        {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("TeleMask"))
            {
                onTeleportPossible?.Invoke(hit);
                if(!canTele)
                {
                    canTele = true;
                    control.MoveLock(canTele);       
                }       
            }
            else
            {
                if(canTele)
                {
                    canTele = false;
                    onTeleportNotPossible?.Invoke(hit);
                }
            }
        }
        else
        {
            if(canTele)
            {
                canTele = false;
                onTeleportNotPossible?.Invoke(hit);
                control.MoveLock(canTele); 
            }
        }
    }

    /// <summary>
    /// This function checks the possibility for teleport with hmd.
    /// by raycasting and hitting only teleport layer.
    /// Also calls the events for teleport possibility 
    /// for MoveEventListener to adjust the visual indicator.
    /// </summary>
    void WithHMD()
    {
        //TODO : Make teleport movement that works with headset button
        bool hits = Physics.Raycast(control.rig.hmd.position, 
                                    control.rig.hmd.forward, 
                                    out hit, 
                                    control.movementVariables.raycastDistance, 
                                    control.movementVariables.raycastLayerMask);

        if(hits)
        {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("TeleMask"))
            {
                onTeleportPossible?.Invoke(hit);
                if(!canTele)
                {
                    canTele = true;           
                }
            }
            else
            {
                if(canTele)
                {
                    canTele = false;
                    onTeleportNotPossible?.Invoke(hit);
                }
            }
        }
        else
        {
            if(canTele)
            {
                canTele = false;
                onTeleportNotPossible?.Invoke(hit);
            }
        }
    }

    /// <summary>
    /// This function teleports the player, 
    /// by changing the position of the rig into hit point position.
    /// and uses the haptic function from XRCustomRig.
    /// </summary>
    void Teleport()
    {
        if(control.usingControllers)
            XRCustomRig.Haptic(control.rig.GetControllerDevice(control.preferredHand), 0.5f, 0.1f);

        control.rig.transform.position = hit.point;
    }
}
