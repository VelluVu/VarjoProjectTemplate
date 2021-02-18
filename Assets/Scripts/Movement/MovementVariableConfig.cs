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
    public float teleportCooldown = 0.5f;

    [Range(0,1)]public float lerpValue = 0;
    [Range(0,1)]public float lerpSpeed = 0.1f;

    public LayerMask POIMask;
}
