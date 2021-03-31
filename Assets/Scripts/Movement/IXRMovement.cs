/// <summary>
/// @Author: Veli-Matti Vuoti
/// This is an interface used to split 
/// the different movement styles into own runnable classes, 
/// that inherit from this interface.
/// </summary>
public interface IXRMovement
{
    /// <summary>
    /// This function is called continuously in XRMovementSwitch Update function.
    /// </summary>
    void UpdateState();

    /// <summary>
    /// This function is called once, 
    /// when enter to the state.
    /// </summary>
    /// <param name="control">XRMovement switch passed as parameter for references</param>
    void StartState(XRMovementSwitch control);

    /// <summary>
    /// This function is called once,
    /// when state is exited.
    /// </summary>
    void ExitState();
}