using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class handles the Gaze movement.
/// </summary>
public class XRGazeMovement : IXRMovement
{
    XRMovementSwitch control;

    public delegate void GazeMoveDelegate(Transform hmd);
    public static event GazeMoveDelegate onCorrectMoveAngle;
    public static event GazeMoveDelegate onNonCorrectMoveAngle;

    bool canMove = false;
    
    public void StartState(XRMovementSwitch control)
    {
        this.control = control;
        
        Debug.Log("Chosen MovementType " + this);
    }

    public void ExitState()
    {
        onNonCorrectMoveAngle?.Invoke(control.rig.hmd);
        Debug.Log("Exited MovementType " + this);
    }

    public void UpdateState()
    { 
        GazeMovement();   
    }

    /// <summary>
    /// This function handles the gazing, 
    /// and checking the possibility of movement with transform eulerangles.
    /// When possible to move, 
    /// calls the MoveToGazeDirection on continuous Primary Button press.
    /// Also invokes the events for other classes.
    /// For example on Correct Move Angle MoveEventListener to handle the visual.
    /// </summary>
    private void GazeMovement()
    {
        float xAngle = control.rig.hmd.transform.rotation.eulerAngles.x;
       
        bool moving = xAngle < control.movementVariables.gazeAngleMax && xAngle > control.movementVariables.gazeAngleMin;
        
        if (moving)
        {  
            if(!canMove)
            {
                canMove = true;
                onCorrectMoveAngle?.Invoke(control.rig.hmd);
                control.MoveLock(canMove); 
            }
            if(control.usingControllers)
            {
                if(control.rig.input.GetPrimaryButtonControlPress(control.preferredHand))
                {
                    MoveToGazeDirection();
                }    
            }
            else
            {
                if(control.rig.input.hmdPrimaryButtonPress)
                {
                    MoveToGazeDirection();   
                }
            }
        }
        else
        {
            if(canMove)
            {
                canMove = false;
                onNonCorrectMoveAngle?.Invoke(control.rig.hmd);
                control.MoveLock(canMove);
            }
        }
    }

    /// <summary>
    /// This function moves the player towards the gaze direction.
    /// </summary>
    void MoveToGazeDirection()
    {
        Vector3 moveDir = new Vector3(control.rig.hmd.transform.forward.x, 
                                      control.rig.transform.forward.y, 
                                      control.rig.hmd.transform.forward.z);

        control.rig.transform.position += moveDir * control.movementVariables.moveSpeed * Time.deltaTime;
    }
}