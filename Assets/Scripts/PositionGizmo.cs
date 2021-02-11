using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
