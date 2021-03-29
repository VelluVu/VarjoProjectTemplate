using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This Class changes event camera of the canvas.
/// Attach this as an component for the object with canvas,
/// to make UI-interactions possible to interact.
/// </summary>
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

    /// <summary>
    /// This function changes the event camera to new camera.
    /// Called from VR Custom Input Module.
    /// </summary>
    /// <param name="cam">new event camera</param>
    public void SetWorldCamera(Camera cam)
    {
        if(cam != null)
        {
            Debug.Log("Settings camera : " + cam.transform.parent.gameObject.name);

            canvas.worldCamera = cam;
        }
    }

    /// <summary>
    /// This function is called on Setting Change event
    /// to invoke on Canvas Enable event.
    /// </summary>
    /// <param name="settings">new settings</param>
    public void SettingChanged(GameSettings settings)
    {
        if(settings.PreviousHand != settings.CurrentHand)
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
