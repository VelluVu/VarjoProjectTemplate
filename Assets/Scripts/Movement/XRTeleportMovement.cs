using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// This class does the teleportation movement.
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

    private void OnHMDButtonDown()
    {
        if(canTele)
        {
            Teleport();
        }
    }
    private void OnLeftControllerButtonDown()
    {
        if(canTele && control.preferredHand == PreferredHand.Left)
        {
            Teleport();
        }
    }
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

    void Teleport()
    {
        if(control.usingControllers)
            XRCustomRig.Haptic(control.rig.GetControllerDevice(control.preferredHand), 0.5f, 0.1f);

        control.rig.transform.position = hit.point;
    }
}
