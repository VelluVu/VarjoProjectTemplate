using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// Class to draw wire sphere gizmo in object position
/// </summary>
public class PositionGizmo : MonoBehaviour
{
    public Color gizmoColor = Color.blue;
    public float gizmoRadius = 0.25f;

    private void OnDrawGizmos() 
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
    }
}
