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
    }

    private void OnEnable() {
        XRSettings.onSettingChange += CheckSetting;
    }

    private void OnDisable() {
        XRSettings.onSettingChange -= CheckSetting;
    }

    public void CheckSetting(SettingSO newSetting)
    {
        if(newSetting.movementType != MovementType.POIGaze)
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
        for (var i = 0; i < POIs.Length; i++)
        {
            POIs[i].gameObject.SetActive(true);
        }
    }

    private void HideThePOIs()
    {
         for (var i = 0; i < POIs.Length; i++)
        {
            POIs[i].gameObject.SetActive(false);
        }
    }
}
