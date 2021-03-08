using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCanvasEventCamera : MonoBehaviour
{
    Canvas canvas;

    public delegate void CanvasDelegate(SetCanvasEventCamera eventCameraSetter);
    public static event CanvasDelegate onCanvasEnable;

    private void Awake() {
        canvas = GetComponent<Canvas>();
    }

    private void OnEnable() {
        XRSettings.onSettingChange += SettingChanged;
    }

    private void OnDisable() {
        XRSettings.onSettingChange -= SettingChanged;
    }

    public void SetWorldCamera(Camera cam)
    {
        if(cam != null)
        {
            Debug.Log("Settings camera : " + cam.gameObject.name);

            canvas.worldCamera = cam;
        }
    }

    public void SettingChanged(SettingSO settings)
    {
        if(settings.previousHand != settings.currentHand)
            onCanvasEnable?.Invoke(this);
    }

    void Start()
    {
        onCanvasEnable?.Invoke(this);
    }

    private void Update() 
    {
        if(canvas.worldCamera == null)
        {   
            onCanvasEnable?.Invoke(this);
        }
    }

}
