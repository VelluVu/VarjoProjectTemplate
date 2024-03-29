/// <summary>
/// @Author: Veli-Matti Vuoti
/// This Class triggers the lerp movement, 
/// by invoking the onWayPointMove event.
/// </summary>
public class LerpMovementTrigger
{
    
    public delegate void WaypointMovementDelegate(int wpIndex);
    public static event WaypointMovementDelegate onWaypointMove;
    
    public static void TriggerMovement(int wpIndex)
    {      
        onWaypointMove?.Invoke(wpIndex);
    }

}
