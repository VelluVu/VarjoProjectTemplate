using UnityEngine;

public class XRInputManager : MonoBehaviour
{
    [HideInInspector]public PrimaryButtonWatcher watcher;
  
    public bool onPrimaryButtonPress = false;

    private void Awake() {
        watcher = GetComponent<PrimaryButtonWatcher>();
    }

    private void Start() {
        watcher.primaryButtonPress.AddListener(onPrimaryButtonEvent);
    }

    public void onPrimaryButtonEvent(bool pressed) 
    {    
        onPrimaryButtonPress = pressed;
    }
}
