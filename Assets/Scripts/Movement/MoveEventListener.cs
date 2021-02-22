using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEventListener : MonoBehaviour
{

    [SerializeField] GameObject dirArrowPrefab;
    GameObject dirArrow;

    [SerializeField] GameObject teleportIndicatorPrefab;
    GameObject teleportIndicator;

    MaterialSwapper matSwap;
    Transform hmd;
    Transform controller;

    private void OnEnable() {
        XRPOIGazeMovement.onUnableToMove += CantMove;
        XRPOIGazeMovement.onAbleToMove += CanMove;
        XRGazeMovement.onCorrectMoveAngle += ShowGazeVisual;
        XRGazeMovement.onNonCorrectMoveAngle += HideGazeVisual;
        XRTeleportMovement.onTeleportPossible += ShowTeleportIndicator;
        XRTeleportMovement.onTeleportNotPossible += HideTeleportIndicator;
    }

    private void OnDisable() {
        XRPOIGazeMovement.onUnableToMove -= CantMove;
        XRPOIGazeMovement.onAbleToMove -= CanMove;
        XRGazeMovement.onCorrectMoveAngle -= ShowGazeVisual;
        XRGazeMovement.onNonCorrectMoveAngle -= HideGazeVisual;
        XRTeleportMovement.onTeleportPossible -= ShowTeleportIndicator;
        XRTeleportMovement.onTeleportNotPossible -= HideTeleportIndicator;
    }

    private void Update() {
        if(dirArrow != null && hmd != null && dirArrow.activeSelf)
        {
            
            Vector3 position = Vector3.ProjectOnPlane(hmd.forward, Vector2.up) + new Vector3(hmd.position.x, 0.1f, hmd.position.z);
            dirArrow.transform.position = position;
            dirArrow.transform.rotation = Quaternion.Euler(new Vector3(90.01f, hmd.rotation.eulerAngles.y, 0));
        }
    }

    private void HideGazeVisual(Transform hmd)
    {
        //TODO : hide visual
        if(dirArrow != null)
            dirArrow.SetActive(false);
    }

    private void ShowGazeVisual(Transform hmd)
    {
        this.hmd = hmd;
        Vector3 position = Vector3.ProjectOnPlane(hmd.forward, Vector2.up) + new Vector3(this.hmd.position.x, 0.1f, this.hmd.position.z);
        //TODO : show visuals
        if(dirArrow == null)
        {
            dirArrow = Instantiate(dirArrowPrefab, position, Quaternion.Euler(new Vector3(90.01f, this.hmd.rotation.eulerAngles.y, 0)));
        }
        else
        {
            dirArrow.SetActive(true);
            dirArrow.transform.position = position;
            dirArrow.transform.rotation = Quaternion.Euler(new Vector3(90.01f, this.hmd.rotation.eulerAngles.y, 0));
        }
    }

    private void CanMove(Transform target)
    {
        //Debug.Log("Can Move");
        //TODO: Show player that now is time to press button and able to move.
        if(target != null)
        {
            matSwap = target.GetComponent<MaterialSwapper>();
            if(matSwap != null)
                matSwap.SetHighLightMaterial();
        }
    }

    private void CantMove(Transform target)
    {
        //Debug.Log("Can't Move");
        //TODO: Remove the signs of possibilities for movement.
        if(matSwap != null)
        {
            matSwap.SetNormalMaterial();
            matSwap = null;
        }

        if(target != null)
        {
            matSwap = target.GetComponent<MaterialSwapper>();
            if(matSwap != null)
                matSwap.SetNormalMaterial();
        }
    }

    private void HideTeleportIndicator(RaycastHit hit)
    {
        
        if(teleportIndicator != null)
        {
            teleportIndicator.SetActive(false);
        }
    }

    private void ShowTeleportIndicator(RaycastHit hit)
    {
        if(teleportIndicator == null)
        {
            teleportIndicator = Instantiate(teleportIndicatorPrefab, hit.point, teleportIndicatorPrefab.transform.rotation);
            teleportIndicator.transform.rotation = Quaternion.FromToRotation(teleportIndicator.transform.up,hit.normal)*teleportIndicator.transform.rotation;
        }
        else
        {
            teleportIndicator.SetActive(true);
            teleportIndicator.transform.position = hit.point;
            teleportIndicator.transform.rotation = Quaternion.FromToRotation(teleportIndicator.transform.up,hit.normal)*teleportIndicator.transform.rotation;
        }
    }
}
