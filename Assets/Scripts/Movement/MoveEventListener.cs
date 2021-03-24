using System;
using UnityEngine;

/// <summary>
/// This class listens the movement related events and indicates the possibility of movement for player.
/// </summary>
public class MoveEventListener : MonoBehaviour
{

    [SerializeField] GameObject gazeMoveVisualPrefab;
    GameObject gazeMoveVisual;

    [SerializeField] GameObject teleportIndicatorPrefab;
    GameObject teleportIndicator;

    [SerializeField] GameObject snapTurnVisualPrefab;
    GameObject snapTurnVisual;
    SnapTurnVisualAnimate snapAnim;

    MaterialSwapper matSwap;
    Transform hmd;
    Transform controller;
    Camera hmdCam;
    Transform rig;

    private void Awake() {
        hmdCam = GameObject.FindGameObjectWithTag("HMD").GetComponent<Camera>();
        rig = GameObject.FindGameObjectWithTag("Rig").transform;
    }

    private void OnEnable() {
        XRPOIGazeMovement.onUnableToMove += CantMove;
        XRPOIGazeMovement.onAbleToMove += CanMove;
        XRGazeMovement.onCorrectMoveAngle += ShowGazeVisual;
        XRGazeMovement.onNonCorrectMoveAngle += HideGazeVisual;
        XRTeleportMovement.onTeleportPossible += ShowTeleportIndicator;
        XRTeleportMovement.onTeleportNotPossible += HideTeleportIndicator;
        XRTurning.onTurningPossible += ShowSnapTurnVisual;
        XRTurning.onTurningNotPossible += HideSnapTurnVisual;
    }
    
    private void OnDisable() {
        XRPOIGazeMovement.onUnableToMove -= CantMove;
        XRPOIGazeMovement.onAbleToMove -= CanMove;
        XRGazeMovement.onCorrectMoveAngle -= ShowGazeVisual;
        XRGazeMovement.onNonCorrectMoveAngle -= HideGazeVisual;
        XRTeleportMovement.onTeleportPossible -= ShowTeleportIndicator;
        XRTeleportMovement.onTeleportNotPossible -= HideTeleportIndicator;
        XRTurning.onTurningPossible -= ShowSnapTurnVisual;
        XRTurning.onTurningNotPossible -= HideSnapTurnVisual;
    }

    private void Update() {
        if(gazeMoveVisual != null && hmd != null && gazeMoveVisual.activeSelf)
        {
            
            Vector3 position = Vector3.ProjectOnPlane(hmd.forward, Vector2.up) + new Vector3(hmd.position.x, 0.1f, hmd.position.z);
            gazeMoveVisual.transform.position = position;
            gazeMoveVisual.transform.rotation = Quaternion.Euler(new Vector3(90.01f, hmd.rotation.eulerAngles.y, 0));
        }
    }

    private void HideGazeVisual(Transform hmd)
    {
        if(gazeMoveVisual != null)
            gazeMoveVisual.SetActive(false);
    }

    private void ShowGazeVisual(Transform hmd)
    {
        this.hmd = hmd;
        Vector3 position = Vector3.ProjectOnPlane(hmd.forward, Vector2.up) + new Vector3(this.hmd.position.x, 0.1f, this.hmd.position.z);
        
        if(gazeMoveVisual == null)
        {
            gazeMoveVisual = Instantiate(gazeMoveVisualPrefab, position, Quaternion.Euler(new Vector3(90.01f, this.hmd.rotation.eulerAngles.y, 0)));
        }
        else
        {
            gazeMoveVisual.SetActive(true);
            gazeMoveVisual.transform.position = position;
            gazeMoveVisual.transform.rotation = Quaternion.Euler(new Vector3(90.01f, this.hmd.rotation.eulerAngles.y, 0));
        }
    }

    private void CanMove(Transform target)
    {
        if(target != null)
        {
            matSwap = target.GetComponent<MaterialSwapper>();
            if(matSwap != null)
                matSwap.SetHighLightMaterial();
        }
    }

    private void CantMove(Transform target)
    {
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

    private void ShowSnapTurnVisual(bool right)
    {
        if(snapTurnVisual == null)
        {
            snapTurnVisual = Instantiate(snapTurnVisualPrefab, hmdCam.transform.position + hmdCam.transform.forward, Quaternion.identity);
            snapAnim = snapTurnVisual.GetComponent<SnapTurnVisualAnimate>();
            snapAnim.SetPosAndRot(hmdCam.transform, rig, right);  
        }

        if(!snapTurnVisual.activeSelf)
        {
            snapTurnVisual.SetActive(true);
        }

        if(snapTurnVisual != null)
        { 
            snapAnim.SetPosAndRot(hmdCam.transform, rig, right);  
        }
        
    }
    
    private void HideSnapTurnVisual(bool right)
    {
        if(snapTurnVisual != null)
            snapTurnVisual.SetActive(false);
    }
    
}
