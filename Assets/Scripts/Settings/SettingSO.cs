using UnityEngine;

/// <summary>
/// This class is scriptable object which holds the variables for different XR settings.
/// </summary>
[CreateAssetMenu(menuName = "SettingVariables")]
public class SettingSO : ScriptableObject
{
    public MovementType movementType;
    public bool controllersInUse;
    public bool snapTurningOn;
    public PreferredHand currentHand;
    public PreferredHand previousHand;
    public PreferredHand preferredHand;

    public delegate void SettingSODelegate(SettingSO settings);
    public event SettingSODelegate onChange;
    bool isInit = false;

    public void Init() {
        currentHand = preferredHand;
        previousHand = currentHand; 
        //onChange?.Invoke(this);
        isInit = true;
    }

    private void OnValidate() {
        if(!isInit)
            return;

        onChange?.Invoke(this);
    }

}
