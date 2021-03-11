using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GazePressOvertime : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    CustomVRInputModule inputModule;
    PointerEventData pointerEventData;
    bool gazing = false;
    bool pressed = false;
    float time = 0;

    public delegate void GazePressDelegate(PointerEventData eventData);
    public static event GazePressDelegate onEnterElement;
    public static event GazePressDelegate onExitElement;
    
    public delegate void GazePressValueDelegate(float value);
    public static event GazePressValueDelegate onSendGazeValue;
    public static event GazePressValueDelegate onGazeComplete;

    private void Start() {
        inputModule = FindObjectOfType<CustomVRInputModule>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(XRSettings.Instance.settings.CurrentHand == PreferredHand.Hmd)
        {      
            time = 0;
            gazing = true;
            onEnterElement?.Invoke(eventData);
            pointerEventData = eventData;
        }
    }

    private void Update() 
    {
        if(gazing && !pressed)
        {
            time += Time.deltaTime;
            onSendGazeValue?.Invoke(time);
            
            if(time > 1.5f)
            {
                time = 0;
                onGazeComplete?.Invoke(time);
                PressUIButton(pointerEventData);
                pressed = true;
            }
        }
    }


    public void PressUIButton(PointerEventData pointerEventData)
    {
        pointerEventData.pointerPressRaycast = pointerEventData.pointerCurrentRaycast;
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(inputModule.GetSelectedObject(), pointerEventData, ExecuteEvents.pointerDownHandler);
        
        if(newPointerPress == null)
        {   
            Debug.Log(newPointerPress.gameObject.name);
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(inputModule.GetSelectedObject());
        }
        pointerEventData.pressPosition = pointerEventData.position;
        pointerEventData.pointerPress = newPointerPress;
        pointerEventData.rawPointerPress = inputModule.GetSelectedObject();

        pointerEventData.pointerPress.GetComponent<Button>().OnPointerClick(pointerEventData);
        
        //Debug.Log("UI Button Down!");

        ExecuteEvents.Execute(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerUpHandler);
        /*
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(inputModule.GetSelectedObject());

        if(pointerEventData.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerClickHandler);
        }*/

        inputModule.NullSelectedGameObject();

        //Debug.Log("UI Button UP!");
        pressed = false;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(XRSettings.Instance.settings.CurrentHand == PreferredHand.Hmd)
        {
            gazing = false;
            onExitElement?.Invoke(eventData);
            NotGazingUIButton(eventData);       
        }
    }

    public void NotGazingUIButton(PointerEventData pointerEventData)
    {
        ExecuteEvents.Execute(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerUpHandler);

        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(inputModule.GetSelectedObject());

        if(pointerEventData.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerClickHandler);
        }

        inputModule.NullSelectedGameObject();

        //Debug.Log("UI Button UP!");
        pressed = false;
    }
}
