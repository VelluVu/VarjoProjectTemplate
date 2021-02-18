using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLerpMovement : MonoBehaviour, ICooldown
{

    [SerializeField] int id = IDTable.lerpTestCDID;
    [SerializeField] float cooldownDuration = 1f;

    public int Id => id;

    public float CooldownDuration => cooldownDuration;

    public int waypointToMove = 0;

    XRMovementSwitch control;

    private void Awake() {
        control = GetComponent<XRMovementSwitch>();
        id = IDTable.lerpTestCDID;
    }

    private void Start() {
        Debug.Log(Id);
    }

    private void Update() {

        if(control.GetCurrentMovementType() != MovementType.Lerp)
        {
            return;
        }

        if(control.rig.cooldownSystem.IsOnCooldown(id))
        {
            return;
        }

        if(control.rig.input.onPrimaryButtonPress)
        {         
            LerpMovementTrigger.TriggerMovement(waypointToMove);
            control.rig.cooldownSystem.AddCooldown(this);
        }

    }

}
