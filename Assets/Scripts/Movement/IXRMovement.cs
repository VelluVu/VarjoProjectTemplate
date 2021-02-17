
/// <summary>
/// Interface to split the different movement styles into own scripts.
/// </summary>
public interface IXRMovement
{
    void UpdateState();
    void StartState(XRMovementSwitch control);
    void ExitState();
}
