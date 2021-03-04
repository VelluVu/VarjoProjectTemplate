using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class UIGazeRaycaster : MonoBehaviour
{
    //[SerializeField]CustomVRInputModule inputModule;
    [SerializeField] GameObject gazeVisualPrefab;
    [SerializeField]private float uiRaycastDistance;
    [SerializeField]LayerMask uiRaycastMask;
    
    [SerializeField] bool usingControllers;
    GameObject gazeVisual;
    RaycastHit hit;
    bool hits;

    private void OnEnable() {
        XRSettings.onSettingChange += SettingChange;
    }

    private void OnDisable() {
        XRSettings.onSettingChange -= SettingChange;
    }

    public void SettingChange(SettingSO settings)
    {
        usingControllers = settings.controllersInUse;
    }

    private void Update() 
    {
        if(usingControllers)
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
                    gazeVisual.transform.rotation = Quaternion.FromToRotation(gazeVisual.transform.up,hit.normal)*gazeVisual.transform.rotation;
                }
                else
                {
                    if(!gazeVisual.activeSelf)
                        gazeVisual.SetActive(true);
                        
                    gazeVisual.transform.position = hit.point;
                    gazeVisual.transform.rotation = Quaternion.FromToRotation(gazeVisual.transform.up,hit.normal)*gazeVisual.transform.rotation;
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
