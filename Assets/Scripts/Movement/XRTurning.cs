using System.Collections;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// This class handles the turning if toggled on from menu
/// </summary>
public class XRTurning : MonoBehaviour
{
    
    XRCustomRig rig;
    Transform hmd;

    bool recentlyTurned = false;
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
        XRCustomRig.onHeadMountedDeviceIsPresent += AlignHMD;
        XRMovementSwitch.onMove += IsMoving;
        XRSettings.onSettingChange += OnSettingChange;
        XRInputManager.onPrimaryButtonDown += AcceptTurn;
    }

    private void OnDisable() {
        XRCustomRig.onHeadMountedDeviceIsPresent -= AlignHMD;
        XRMovementSwitch.onMove -= IsMoving;
        XRSettings.onSettingChange -= OnSettingChange;
        XRInputManager.onPrimaryButtonDown -= AcceptTurn;
    }

    public void OnSettingChange(GameSettings newSetting)
    {
        isTurningToggled = newSetting.SnapTurningOn;
        Debug.Log(this.gameObject.name + " Snap turning is : " + isTurningToggled);
    }

    public void IsMoving(bool isMoving)
    {
        moving = isMoving;
    }

    public void AlignHMD(InputDevice device) 
    {
       hmd.forward = rig.transform.forward;
    }

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
                recentlyTurned = true;
            }
            if (cross.y < 0) // head is turned 90deg to the left from body
            {
                rig.transform.Rotate(Vector3.up, -90);
                recentlyTurned = true;
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
