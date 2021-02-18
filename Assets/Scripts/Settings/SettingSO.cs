using UnityEngine;

/// <summary>
/// This class is scriptable object which holds the variables for different XR settings.
/// </summary>
[CreateAssetMenu(menuName = "SettingVariables")]
public class SettingSO : ScriptableObject
{
    public MovementType movementType;

    public delegate void SettingSODelegate(SettingSO settings);
    public event SettingSODelegate onChange;

    public void Init() {       
        onChange?.Invoke(this);
    }

    private void OnValidate() {
        onChange?.Invoke(this);
    }

}
