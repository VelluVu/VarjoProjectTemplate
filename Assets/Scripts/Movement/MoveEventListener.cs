using System;
using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
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

    /// <summary>
    /// This Function is called, 
    /// when on Non Correct Move Angle event is invoked from XRGazeMovement.
    /// Deactivates the gaze move visual.
    /// </summary>
    /// <param name="hmd">head mounted device transform</param>
    private void HideGazeVisual(Transform hmd)
    {
        if(gazeMoveVisual != null)
            gazeMoveVisual.SetActive(false);
    }

    /// <summary>
    /// This function is called,
    /// when on Correct Move Angle event is invoked from XRGazeMovement.
    /// Instantiates the gaze move visual and projects it on to the ground plane.
    /// if already the gaze move visual exists activates it and projects it on to the ground plane.
    /// TODO: Change ProjectOnPlane second parameter Vector2.up into ground normal and get normal from raycast hit data if necessary.
    /// </summary>
    /// <param name="hmd">head mounted device transform</param>
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

    /// <summary>
    /// This function is called when on Able to Move is invoked from XRPOIGazeMovement.
    /// Gets material swapper component from gazed target, 
    /// and uses it to set highlight material on target.
    /// </summary>
    /// <param name="target"></param>
    private void CanMove(Transform target)
    {
        if(target != null)
        {
            matSwap = target.GetComponent<MaterialSwapper>();
            if(matSwap != null)
                matSwap.SetHighLightMaterial();
        }
    }

    /// <summary>
    /// This function is called,
    /// when on Not Able to Move is invoked from XRPOIGazeMovement.
    /// if material swapper is not null, 
    /// sets the normal material of that material swapper, 
    /// and nulls the material swapper.
    /// if target is not null, 
    /// sets material swapper to match target material swapper,
    /// and sets it material to normal material.
    /// </summary>
    /// <param name="target"></param>
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

    /// <summary>
    /// This function is called,
    /// when on Teleport not Possible is invoked from XRTeleportMovement.
    /// Deactivates the teleport indicator.
    /// </summary>
    /// <param name="hit"></param>
    private void HideTeleportIndicator(RaycastHit hit)
    {
        
        if(teleportIndicator != null)
        {
            teleportIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// This function is called,
    /// when on Teleport Possible is invoked from XRTeleportMovement.
    /// Instantiates teleport indicator and sets it position and rotation.
    /// if already instantiated teleport indicator,
    /// then activates the current one and sets the position and rotation.
    /// Position is set to match the raycast hit info hit point.
    /// Rotation is set to align the teleport indicator on the ground.
    /// </summary>
    /// <param name="hit">raycast hit info</param>
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

    /// <summary>
    /// This function is called,
    /// when on Turning Possible is invoked from SnapTurning.
    /// Instantiates the snap turning visual.
    /// if already instantiated, 
    /// then activates the snap turning visual.
    /// Uses SnapTurnVisualAnimate class to activate the visual animation, 
    /// and SetPosAndRot sets it initial values.
    /// </summary>
    /// <param name="right"></param>
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
    
    /// <summary>
    /// This function is called,
    /// when on Turning not Possible is invoked from SnapTurning.
    /// Deactivates the snapTurnVisual.
    /// </summary>
    /// <param name="right"></param>
    private void HideSnapTurnVisual(bool right)
    {
        if(snapTurnVisual != null)
            snapTurnVisual.SetActive(false);
    }
    
}
