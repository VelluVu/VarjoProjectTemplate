using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class handles hiding of the Points of Interest,
/// when movement type is changed.
/// </summary>
public class HidePOIs : MonoBehaviour
{

    Transform [] POIs;

    /// <summary>
    /// Initializes the POI array.
    /// </summary>
    private void Awake() {
        POIs = new Transform[transform.childCount];
        for (var i = 0; i < POIs.Length; i++)
        {
            POIs[i] = transform.GetChild(i);
        }
        
        if(XRSettings.Instance.settings.MovementType != MovementType.POIGaze)
            HideThePOIs();
    }

    private void OnEnable() {
        XRSettings.onSettingChange += CheckSetting;
    }

    private void OnDisable() {
        XRSettings.onSettingChange -= CheckSetting;
    }

    /// <summary>
    /// This function is called,
    /// when Settings are changed.
    /// If movement is not POIGaze,
    /// then Hides The Points of interest,
    /// else Shows the Points of interest.
    /// </summary>
    /// <param name="newSetting"></param>
    public void CheckSetting(GameSettings newSetting)
    {
        if(newSetting.MovementType != MovementType.POIGaze)
        {
            HideThePOIs();
        }
        else
        {
            ShowThePOIs();
        }
    }

    /// <summary>
    /// This Function activates the Points of interests.
    /// Loops through POI array and sets each active.
    /// </summary>
    private void ShowThePOIs()
    {
        Debug.Log("Showing Points of Interest");
        for (var i = 0; i < POIs.Length; i++)
        {
            POIs[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// This function hides the Points of interests.
    /// Loops through POI array and sets each de active.
    /// </summary>
    private void HideThePOIs()
    {
        Debug.Log("Hiding Points of Interest");
        for (var i = 0; i < POIs.Length; i++)
        {
            POIs[i].gameObject.SetActive(false);
        }
    }
}
