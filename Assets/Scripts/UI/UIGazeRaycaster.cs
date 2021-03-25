using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class handles the gaze ui visuals and raycasting.
/// </summary>
public class UIGazeRaycaster : MonoBehaviour
{
    //[SerializeField]CustomVRInputModule inputModule;
    [SerializeField] GameObject gazeVisualPrefab;
    [SerializeField]private float uiRaycastDistance;
    [SerializeField]LayerMask uiRaycastMask;

    [SerializeField]GameObject gazePressIndicatorPrefab;
    [SerializeField]GameObject gazePressIndicator;
    Image gazeRadialImage;
    
    [SerializeField] PreferredHand preferredHand;
    GameObject gazeVisual;
    RaycastHit hit;
    bool hits;

    private void OnEnable() {
        XRSettings.onSettingChange += SettingChange;
        GazePressOvertime.onEnterElement += GazeOnElement;
        GazePressOvertime.onExitElement += GazeExitElement;
        GazePressOvertime.onSendGazeValue += GazeValueUpdate;
        GazePressOvertime.onGazeComplete += GazeComplete;
    }

    private void OnDisable() {
        XRSettings.onSettingChange -= SettingChange;
        GazePressOvertime.onEnterElement -= GazeOnElement;
        GazePressOvertime.onExitElement -= GazeExitElement;
        GazePressOvertime.onSendGazeValue -= GazeValueUpdate;
        GazePressOvertime.onGazeComplete -= GazeComplete;
    }

    /// <summary>
    /// This function is called, 
    /// when ui element is gazed long enough.
    /// </summary>
    /// <param name="value">amount to fill the gaze press visual</param>
    private void GazeComplete(float value)
    {
        if(gazeRadialImage != null)
        {
            gazeRadialImage.fillAmount = value;
        }

        gazePressIndicator.SetActive(false);
    }

    /// <summary>
    /// This function is called, 
    /// when moving gaze on UI-element
    /// Instantiates the gaze press indicator if not instantiated yet
    /// if instantiated sets active, 
    /// and moves the gaze press indicater position in to the hitpoint.
    /// </summary>
    /// <param name="eventData">pointer event data is parameter of the delegate not used in this function</param>
    private void GazeOnElement(PointerEventData eventData)
    {
        if(gazePressIndicator == null)
        {
            gazePressIndicator = Instantiate(gazePressIndicatorPrefab, hit.point - transform.forward, gazePressIndicatorPrefab.transform.rotation);
            gazePressIndicator.name = "Gaze Press Indicator";
        }
        else
        {
            gazePressIndicator.SetActive(true);
            gazePressIndicator.transform.position = hit.point;
        }
    }

    /// <summary>
    /// This function is called,
    /// when gaze exits the UI-Element.
    /// Disables the gaze press indicator.
    /// </summary>
    /// <param name="eventData"></param>
    private void GazeExitElement(PointerEventData eventData)
    {
        gazePressIndicator.SetActive(false);
    }

    /// <summary>
    /// This function is called,
    /// when settings are changed,
    /// Changes the prederred hand to new settings hand.
    /// </summary>
    /// <param name="settings"></param>
    public void SettingChange(GameSettings settings)
    {
        preferredHand = settings.CurrentHand;
        Debug.Log(this.gameObject.name + " Hand changed to: " + preferredHand.ToString());
    }

    /// <summary>
    /// This function is called, 
    /// when gazing the UI-Element.
    /// Gets the reference for gaze radial image if not already set.
    /// Scales the parameter to match 0.0-1.0 value.
    /// Changes the gaze radial image fillamount.
    /// </summary>
    /// <param name="value">Time gazed on UI-element</param>
    public void GazeValueUpdate(float value)
    {
        if(gazePressIndicator != null)
        {
            if(gazeRadialImage == null)
            {
                gazeRadialImage = gazePressIndicator.GetComponentInChildren<Image>();
            }
            float scaleValue = value / 1;
            gazeRadialImage.fillAmount = scaleValue * value;
           
            gazePressIndicator.transform.position = hit.point - transform.forward;
            gazePressIndicator.transform.LookAt(transform.position);

        }
    }

    private void Update() 
    {
        if(preferredHand != PreferredHand.Hmd)
        {
            if(gazeVisual)
                if(gazeVisual.gameObject.activeSelf)
                    gazeVisual.SetActive(false);
                    
            return;
        }
        RayFromHMD();
    }

    /// <summary>
    /// This function shoots the ray from hmd
    /// </summary>
    public void RayFromHMD()
    {
        hits = Physics.Raycast(transform.position, transform.forward, out hit, uiRaycastDistance, uiRaycastMask);
        if(hits)
        {
            if(gazeVisualPrefab != null)
            {
                if(gazeVisual == null)
                {
                    gazeVisual = Instantiate(gazeVisualPrefab, hit.point, gazeVisualPrefab.transform.rotation);
                    gazeVisual.gameObject.name = "UI Gaze Visual";
                    
                }
                else
                {
                    if(!gazeVisual.activeSelf)
                        gazeVisual.SetActive(true);
                        
                    gazeVisual.transform.position = hit.point;
                   
                }
            }
        }
        else
        {
            if(gazeVisual != null)
                gazeVisual.SetActive(false);
        }
    }

}
