using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyWaypointsPresence : MonoBehaviour
{
    
    public delegate void WPDelegate(Transform t);
    public static event WPDelegate onWPSDetected;

    void Start()
    {
        onWPSDetected?.Invoke(transform);
    }

    
}
