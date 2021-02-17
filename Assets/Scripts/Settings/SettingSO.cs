using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is scriptable object which holds the variables for different XR settings.
/// </summary>
[CreateAssetMenu(menuName = "SettingVariables")]
public class SettingSO : ScriptableObject
{
    public MovementType movementType;
}
