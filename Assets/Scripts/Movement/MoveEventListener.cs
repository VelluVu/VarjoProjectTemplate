using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEventListener : MonoBehaviour
{

    public GameObject dirArrowPrefab;
    public GameObject dirArrow;

    MaterialSwapper matSwap;

    private void OnEnable() {
        XRGazeMovement.onUnableToMove += CantMove;
        XRGazeMovement.onAbleToMove += CanMove;
        XRGazeMovement.onCorrectMoveAngle += ShowVisual;
        XRGazeMovement.onNonCorrectMoveAngle += HideVisual;
    } 

    private void OnDisable() {
        XRGazeMovement.onUnableToMove -= CantMove;
        XRGazeMovement.onAbleToMove -= CanMove;
        XRGazeMovement.onCorrectMoveAngle -= ShowVisual;
        XRGazeMovement.onNonCorrectMoveAngle -= HideVisual;
    }

    private void HideVisual(Transform hmd)
    {
        //TODO : hide visual
        dirArrow.SetActive(false);
    }

    private void ShowVisual(Transform hmd)
    {
        Vector3 position = Vector3.ProjectOnPlane(hmd.forward, Vector2.up) + new Vector3(hmd.position.x, 0.1f, hmd.position.z);
        //TODO : show visuals
        if(dirArrow == null)
        {
            dirArrow = Instantiate(dirArrowPrefab, position, Quaternion.Euler(new Vector3(90.01f, hmd.rotation.eulerAngles.y, 0)));
        }
        else
        {
            dirArrow.SetActive(true);
            dirArrow.transform.position = position;
            dirArrow.transform.rotation = Quaternion.Euler(new Vector3(90.01f, hmd.rotation.eulerAngles.y, 0));
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
}
