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

    private void Awake() 
    {
        rig = GetComponent<XRCustomRig>();
        if(rig != null)
            hmd = rig.hmd;
    }

    private void Start() {
        coroutine = CheckTick();
    }

    private void OnEnable() {
        XRCustomRig.onHeadMountedDeviceIsPresent += AlignHMD;
        XRMovementSwitch.onMove += IsMoving;
        XRSettings.onSettingChange += OnSettingChange;
    }

    private void OnDisable() {
        XRCustomRig.onHeadMountedDeviceIsPresent -= AlignHMD;
        XRMovementSwitch.onMove -= IsMoving;
        XRSettings.onSettingChange -= OnSettingChange;
    }

    public void OnSettingChange(SettingSO newSetting)
    {
        isTurningToggled = newSetting.snapTurningOn;
    }

    public void IsMoving(bool isMoving)
    {
        moving = isMoving;
    }

    public void AlignHMD(InputDevice device) 
    {
       hmd.forward = rig.transform.forward;
    }

    private void Update() 
    {
        if(!isTurningToggled)
            return;

        if(moving)
            return;

        SnapTurn();
    }

    IEnumerator CheckTick()
    {
        while(true)
        {          
            yield return new WaitForSeconds(1f);
            recentlyTurned = false;
        }
    }

    private void SnapTurn()
    {
        float dotProduct = Vector3.Dot(hmd.forward, rig.transform.forward);
        
        if (dotProduct < 0 && !recentlyTurned) //if looking opposite from body direction
        {
            StopCoroutine(coroutine);
            Vector3 cross = Vector3.Cross(rig.transform.forward, hmd.forward);

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
        if(dotProduct > 0 && recentlyTurned)
        {  
            StartCoroutine(coroutine);
        }
    }
}
