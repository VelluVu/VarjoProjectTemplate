using UnityEngine;

public class HidePOIs : MonoBehaviour
{

    Transform [] POIs;

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

    private void ShowThePOIs()
    {
        Debug.Log("Showing Points of Interest");
        for (var i = 0; i < POIs.Length; i++)
        {
            POIs[i].gameObject.SetActive(true);
        }
    }

    private void HideThePOIs()
    {
        Debug.Log("Hiding Points of Interest");
        for (var i = 0; i < POIs.Length; i++)
        {
            POIs[i].gameObject.SetActive(false);
        }
    }
}
