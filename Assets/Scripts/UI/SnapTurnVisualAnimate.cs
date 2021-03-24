using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the moving of arrow sprite
/// </summary>
public class SnapTurnVisualAnimate : MonoBehaviour
{
    [SerializeField]float speed = 1f;
    [SerializeField]float moveDistance = 0.4f;

    [SerializeField]float startZOffsetMultiplier = 0.25f;

    bool changeDir = false;

    Vector3 endPoint;
    Vector3 startPoint;

    public void SetPosAndRot(Transform hmd, Transform rig, bool direction)
    {
        Vector3 lookDir = (hmd.forward - hmd.position).normalized;
        Vector3 instantiationPosition = hmd.position + hmd.forward;

        changeDir = false;
        startPoint = instantiationPosition + transform.forward * startZOffsetMultiplier;
        
        transform.position = startPoint;
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, rig.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        
        if(direction)
            transform.Rotate(new Vector3(0,180,0));
        if(!direction)
            transform.Rotate(new Vector3(0,-180,0));

        endPoint = startPoint + transform.forward * moveDistance;
    }

   

    private void Update() 
    {
        if(!changeDir)
        {
            Vector3 dir = (endPoint - startPoint).normalized;
            transform.position += dir * speed * Time.deltaTime;
            
            if(Vector3.Distance(transform.position, endPoint) < 0.1f)
            {
                changeDir = true;
            }
        }
        else
        {
            Vector3 dir = (startPoint - endPoint).normalized;
            transform.position += dir * speed * Time.deltaTime;
            
            if(Vector3.Distance(transform.position, startPoint) < 0.1f)
            {
                changeDir = false;
            }
        }
    }

}
