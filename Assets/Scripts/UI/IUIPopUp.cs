using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This interface provides two functions PopUp and Hide, 
/// which will be used by some way, example in UITrigger class.
/// </summary>
public interface IUIPopUp
{
    /// <summary>
    /// This function is used to popup the ui-element,
    /// Example use activate ui element face it towards hmd.
    /// </summary>
    /// <param name="hmd"></param>
    public void PopUp(Transform hmd);

    /// <summary>
    /// This function hides the ui-element.
    /// </summary>
    public void Hide();
}
