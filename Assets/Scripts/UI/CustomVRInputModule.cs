using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class handles the input with unity event system with VR device inputs.
/// </summary>
public class CustomVRInputModule : BaseInputModule
{
    [Tooltip("Drag in 0 = hmd camera, 1 = leftcontroller pointer camera, 2 = rightcontroller pointer camera")]
    [SerializeField]Camera [] graphicRaycastCameras;
    [SerializeField]Camera graphicRaycastCamera;
    InputDeviceCharacteristics device;
    
    GameObject currentObject = null;
    PointerEventData pointerEventData = null;
    
    protected override void OnEnable() {
        base.OnEnable();
        XRInputManager.onPrimaryButtonDown += UIButtonDown;
        XRInputManager.onPrimaryButtonUp += UIButtonUp;
        XRSettings.onSettingChange += ChangeUIRaycaster;
        SetCanvasEventCamera.onCanvasEnable += SetCamera;
    }

    protected override void OnDisable() {
        base.OnDisable();
        XRInputManager.onPrimaryButtonDown -= UIButtonDown;
        XRInputManager.onPrimaryButtonUp -= UIButtonUp;
        XRSettings.onSettingChange -= ChangeUIRaycaster;
        SetCanvasEventCamera.onCanvasEnable -= SetCamera;
    }

    protected override void Awake() {
        base.Awake();
        
        pointerEventData = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        if(graphicRaycastCamera == null)
            return;

        pointerEventData.Reset();
        pointerEventData.position = new Vector2(graphicRaycastCamera.pixelWidth /2, graphicRaycastCamera.pixelHeight /2);

        eventSystem.RaycastAll(pointerEventData, m_RaycastResultCache);
        pointerEventData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        currentObject = pointerEventData.pointerCurrentRaycast.gameObject;
        m_RaycastResultCache.Clear();
        HandlePointerExitAndEnter(pointerEventData, currentObject);
    }

    public void ChangeUIRaycaster(GameSettings settings)
    {
        
        if(settings.CurrentHand == PreferredHand.Left)
        {
            //Debug.Log("Setting left camera!");
            graphicRaycastCameras[2].gameObject.SetActive(false);
            graphicRaycastCameras[1].gameObject.SetActive(true);
            graphicRaycastCamera = graphicRaycastCameras[1];
            graphicRaycastCamera.GetComponent<UIRaycaster>().Activate();
            device = InputDeviceCharacteristics.Left;
            
        }
        else if(settings.CurrentHand == PreferredHand.Right)
        {
            //Debug.Log("Setting right camera!");
            graphicRaycastCameras[1].gameObject.SetActive(false);
            graphicRaycastCameras[2].gameObject.SetActive(true);
            graphicRaycastCamera = graphicRaycastCameras[2];
            graphicRaycastCamera.GetComponent<UIRaycaster>().Activate();
            device = InputDeviceCharacteristics.Right;
        }
        else if(settings.CurrentHand == PreferredHand.Hmd)
        {
            graphicRaycastCameras[1].gameObject.SetActive(false);
            graphicRaycastCameras[2].gameObject.SetActive(false);
            graphicRaycastCamera = graphicRaycastCameras[0];
            device = InputDeviceCharacteristics.HeadMounted;
        }
        
        Debug.Log(this + " Changed raycast camera : " + device + ", " + graphicRaycastCamera.gameObject.name);
    }

    public PointerEventData GetData()
    {
        return pointerEventData;
    }
    
    public GameObject GetSelectedObject()
    {
        return currentObject;
    }

    public void SetCamera(SetCanvasEventCamera eventCameraSetter)
    {
        if(graphicRaycastCamera != null)
            eventCameraSetter.SetWorldCamera(graphicRaycastCamera);
    }

    public bool IsCorrectInputDevice(InputDeviceCharacteristics theHand)
    {
        
        if(device.HasFlag(theHand))
        {
            return true;
        }
        return false;
    }
    
    public void UIButtonDown(InputDeviceCharacteristics deviceCharacteristics)
    {
        if(!IsCorrectInputDevice(deviceCharacteristics))
            return;

        pointerEventData.pointerPressRaycast = pointerEventData.pointerCurrentRaycast;
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(currentObject, pointerEventData, ExecuteEvents.pointerDownHandler);
        if(newPointerPress == null)
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);
        pointerEventData.pressPosition = pointerEventData.position;
        pointerEventData.pointerPress = newPointerPress;
        pointerEventData.rawPointerPress = currentObject;
        //Debug.Log("UI Button Down!");
    }

    public void UIButtonUp(InputDeviceCharacteristics deviceCharacteristics)
    {
        if(!IsCorrectInputDevice(deviceCharacteristics))
            return;

        ExecuteEvents.Execute(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerUpHandler);

        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

        if(pointerEventData.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerClickHandler);
        }

        NullSelectedGameObject();
        //Debug.Log("UI Button UP!");
    }

    public void NullSelectedGameObject()
    {
        eventSystem.SetSelectedGameObject(null);

        pointerEventData.pressPosition = Vector2.zero;
        pointerEventData.pointerPress = null;
        pointerEventData.rawPointerPress = null;
    }
}
