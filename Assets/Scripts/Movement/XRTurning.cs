using System.Collections;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class handles the turning if toggled on from menu
/// </summary>
public class XRTurning : MonoBehaviour
{
    
    XRCustomRig rig;
    Transform hmd;
    IEnumerator coroutine;

    bool moving = false;
    bool isTurningToggled = false;
    Vector3 cross;
    float dotProduct;

    bool invokedVisual = false;

    public delegate void TurningDelegate(bool right);
    public static event TurningDelegate onTurningPossible;
    public static event TurningDelegate onTurningNotPossible;

    private void Awake() 
    {
        rig = GetComponent<XRCustomRig>();
        if(rig != null)
            hmd = rig.hmd;
    }

    private void OnEnable() {
        XRMovementSwitch.onMove += IsMoving;
        XRSettings.onSettingChange += OnSettingChange;
        XRInputManager.onPrimaryButtonDown += AcceptTurn;
    }

    private void OnDisable() {
        XRMovementSwitch.onMove -= IsMoving;
        XRSettings.onSettingChange -= OnSettingChange;
        XRInputManager.onPrimaryButtonDown -= AcceptTurn;
    }

    /// <summary>
    /// This function is called,
    /// when settings are changed.
    /// Changes is turning toggled boolean to match setting value.
    /// </summary>
    /// <param name="newSetting">new settings object</param>
    public void OnSettingChange(GameSettings newSetting)
    {
        isTurningToggled = newSetting.SnapTurningOn;
        Debug.Log(this.gameObject.name + " Snap turning is : " + isTurningToggled);
    }

    /// <summary>
    /// This function is called,
    /// when XRMovementSwitch onMove is called.
    /// Used to toggle the moving boolean.
    /// </summary>
    /// <param name="isMoving">new state for moving</param>
    public void IsMoving(bool isMoving)
    {
        moving = isMoving;
    }

    /// <summary>
    /// This function is called,
    /// when primarybutton is down.
    /// turns the player 90 degrees left or right, 
    /// depending on the hmd rotation.
    /// </summary>
    /// <param name="device">device which is used for press</param>
    public void AcceptTurn(InputDeviceCharacteristics device)
    {
        if(!isTurningToggled)
            return;
            
        if(moving)
            return;

        if (dotProduct < 0) //if looking opposite from body direction
        {       
            //cross = Vector3.Cross(rig.transform.forward, hmd.forward);

            if (cross.y > 0) // head is turned 90deg to the right from body
            {
                //Debug.Log("Dot hmd.forward and rig.forward: " + dotProduct); //Use this value to know if user is looking "backwards"
                //Debug.Log("Cross product : " + Vector3.Cross(rig.transform.forward, hmd.forward)); this used to know which way turning -y left y right
                //Snap turn body 90 degrees right
                rig.transform.Rotate(Vector3.up, 90);   
            }
            if (cross.y < 0) // head is turned 90deg to the left from body
            {
                rig.transform.Rotate(Vector3.up, -90);
            }
        }
    }

    private void Update() 
    {
        if(!isTurningToggled)
            return;

        if(moving)
            return;

        SnapTurn();
    }

    /// <summary>
    /// This function tracks the possibility for snapturning and invokes the events.
    /// </summary>
    private void SnapTurn()
    {
        dotProduct = Vector3.Dot(hmd.forward, rig.transform.forward);
        
        if (dotProduct < 0) //if looking opposite from body direction
        {
           
            cross = Vector3.Cross(rig.transform.forward, hmd.forward);

            if (cross.y > 0) // head is turned 90deg to the right from body
            {
                if(!invokedVisual)
                {
                    invokedVisual = true;
                    onTurningPossible?.Invoke(true);
                }
            }
            if (cross.y < 0) // head is turned 90deg to the left from body
            {
                if(!invokedVisual)
                {
                    invokedVisual = true;
                    onTurningPossible?.Invoke(false);
                }
            }
        }

        if(dotProduct > 0)
        {    
            if(invokedVisual)
            {
                invokedVisual = false;
                onTurningNotPossible?.Invoke(false);
            }
        }
    }
}
