using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Scriptable object holds movement related variable values.
/// </summary>
[CreateAssetMenu(menuName = "MovementVariables")]
public class MovementVariableConfig : ScriptableObject
{
    public float gazeAngleMax = 90;
    public float gazeAngleMin = 50;
    public float moveSpeed = 1f;
    public float rayCastDistance = 5f;

    public bool moveToPOI = false;
    public LayerMask POIMask;
    public LayerMask teleMask;
}
