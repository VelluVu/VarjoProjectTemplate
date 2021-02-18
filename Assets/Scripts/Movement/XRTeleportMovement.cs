using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// This class does the teleportation movement.
/// </summary>
public class XRTeleportMovement : IXRMovement, ICooldown
{
    XRMovementSwitch control;

    public delegate void XRTeleportDelegate(Transform hmd, RaycastHit hit);
    public static event XRTeleportDelegate onTeleportPossible;
    public static event XRTeleportDelegate onTeleportNotPossible;

    bool canTele = false;
    bool recentlyTeleported = false;

    int id = IDTable.teleMovementCDID;
    float cooldownDuration = 0.5f;

    public int Id => id;

    public float CooldownDuration => cooldownDuration;

    public void StartState(XRMovementSwitch control)
    {
        this.control = control;
        id = IDTable.teleMovementCDID;
        cooldownDuration = control.movementVariables.teleportCooldown;
        Debug.Log("Chosen MovementType " + this);
    }

    public void ExitState()
    {
        Debug.Log("Exited MovementType " + this);
    }
    
    public void UpdateState()
    {
        //TODO : Make teleport movement that works with headset button.

        RaycastHit hit;
        bool hits = Physics.Raycast(control.rig.hmd.position, control.rig.hmd.forward, out hit, control.movementVariables.rayCastDistance);

        if(!control.rig.cooldownSystem.IsOnCooldown(id))
        {
            recentlyTeleported = false;
        }

        if(hits)
        {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("TeleMask"))
            {
                onTeleportPossible?.Invoke(control.rig.hmd,hit);
                if(!canTele)
                {
                    canTele = true;           
                }
                if(control.rig.input.onPrimaryButtonPress)
                {
                    
                    if(!recentlyTeleported)
                    {   
                        recentlyTeleported = true;
                        control.rig.transform.position = hit.point;
                        control.rig.cooldownSystem.AddCooldown(this);
                    }
                }
            }
            else
            {
                if(canTele)
                {
                    canTele = false;
                    onTeleportNotPossible?.Invoke(control.rig.hmd,hit);
                }
            }
        }
        else
        {
            if(canTele)
            {
                canTele = false;
                onTeleportNotPossible?.Invoke(control.rig.hmd,hit);
            }
        }
        
    }
}
