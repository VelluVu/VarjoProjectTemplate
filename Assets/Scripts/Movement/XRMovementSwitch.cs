using UnityEngine;

/// <summary>
/// This class is working as a statepattern for movementstyles and switching the movementstyle when settings change.
/// </summary>
public class XRMovementSwitch : MonoBehaviour
{
    public MovementVariableConfig movementVariables;
    [Tooltip("Automatically Searches the WPS by Waypoint parent gameObject name if this array is left empty!")]
    public Transform[] Wps; //Waypoints for Lerp movement.
    [HideInInspector]public XRCustomRig rig;
    [HideInInspector]public PreferredHand preferredHand;
    [HideInInspector]public bool usingControllers;
    public bool moving = false;
    public delegate void MovementDelegate(bool moving);
    public static event MovementDelegate onMove;
    MovementType currentMovementType;
    IXRMovement currentXRMovement;
    XRGazeMovement xRGazeMovement;
    XRPOIGazeMovement xRPOIGazeMovement;
    XRLerpMovement xRLerpMovement;
    XRTeleportMovement xRTeleportMovement;

    bool ready = false;
    
    private void Awake() {
        rig = GetComponent<XRCustomRig>();
        InitMovementStyles();
        InitWaypointArray();
    }

    public void InitMovementStyles()
    {
        xRGazeMovement = new XRGazeMovement();
        xRPOIGazeMovement = new XRPOIGazeMovement();
        xRLerpMovement = new XRLerpMovement();
        xRTeleportMovement = new XRTeleportMovement();
    }

    public void InitWaypointArray()
    {
        if(Wps == null || Wps.Length == 0)
        {
            GameObject waypoints = GameObject.Find("Waypoints");
            if(waypoints != null)
            {
                Wps = new Transform[waypoints.transform.childCount];
                for (var i = 0; i < Wps.Length; i++)
                {
                    Wps[i] = waypoints.transform.GetChild(i);   
                }
            }
            else
            {
                Debug.LogWarning("Could not populate the Wps array for LerpMovement. Can't find the Waypoints object from scene hierarchy!");
            }
        }
        else
        {
            Debug.Log("Wps array for LerpMovement is custom filled.");
        }
    }

    private void OnEnable() {      
        XRSettings.onSettingChange += CheckMovementType;
    }
    private void OnDisable() {    
        XRSettings.onSettingChange -= CheckMovementType;
    }

    private void Update() 
    {
        if(ready)
            currentXRMovement.UpdateState();
    }

    public void MoveLock(bool canMove)
    { 
        onMove?.Invoke(canMove);
    }

    public void CheckMovementType(GameSettings newSettings)
    {
        if(preferredHand != newSettings.CurrentHand)
            preferredHand = newSettings.CurrentHand;
        if(usingControllers != newSettings.ControllersInUse)
            usingControllers = newSettings.ControllersInUse;
        
        if(currentMovementType == newSettings.MovementType && currentXRMovement != null)
        {   
            return;
        }

        if(currentXRMovement != null && currentMovementType != newSettings.MovementType)
        {
            currentXRMovement.ExitState();
        }
        Debug.Log(this + " Changed movement type!");
        SetCurrentXRMovementType(newSettings.MovementType);

        currentMovementType = newSettings.MovementType;
        
        currentXRMovement.StartState(this);
        ready = true;
    }

    void SetCurrentXRMovementType(MovementType newMovementType)
    {
        switch (newMovementType)
        {
            case MovementType.Gaze:
                currentXRMovement = xRGazeMovement;
                break;
            case MovementType.POIGaze:
                currentXRMovement = xRPOIGazeMovement;
                break;
            case MovementType.Lerp:
                currentXRMovement = xRLerpMovement;
                break;
            case MovementType.Teleport:
                currentXRMovement = xRTeleportMovement;
                break;
            default:
                break;
        }
    }

    public MovementType GetCurrentMovementType()
    {
        return currentMovementType;
    }
}