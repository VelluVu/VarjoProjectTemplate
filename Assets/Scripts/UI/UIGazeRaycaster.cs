using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

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

    private void GazeComplete(float value)
    {
        if(gazeRadialImage != null)
        {
            gazeRadialImage.fillAmount = value;
        }

        gazePressIndicator.SetActive(false);
    }

    private void GazeOnElement(PointerEventData eventData)
    {
        if(gazePressIndicator == null)
        {
            gazePressIndicator = Instantiate(gazePressIndicatorPrefab, hit.point - transform.forward, gazePressIndicatorPrefab.transform.rotation);
            gazePressIndicator.name = "Gaze Press Indicator";
            //gazePressIndicator.transform.rotation = Quaternion.FromToRotation(gazePressIndicator.transform.up,hit.normal)*gazePressIndicator.transform.rotation;
        }
        else
        {
            gazePressIndicator.SetActive(true);
            //gazePressIndicator.transform.rotation = Quaternion.FromToRotation(gazePressIndicator.transform.up,hit.normal)*gazePressIndicator.transform.rotation;
            gazePressIndicator.transform.position = hit.point;
        }
    }

    private void GazeExitElement(PointerEventData eventData)
    {
        gazePressIndicator.SetActive(false);
    }

    public void SettingChange(GameSettings settings)
    {
        preferredHand = settings.CurrentHand;
        Debug.Log(this.gameObject.name + " Hand changed to: " + preferredHand.ToString());
    }

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
