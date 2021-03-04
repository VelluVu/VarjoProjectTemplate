using UnityEngine;

public class UIRaycaster : MonoBehaviour
{
    
    LineRenderer lineRenderer;
    RaycastHit hit;

    //[SerializeField]CustomVRInputModule inputModule;
    [SerializeField]private float uiRaycastDistance;
    [SerializeField]LayerMask uiRaycastMask;
    [SerializeField]private float shootPointZ;
    Transform rayShootPoint;
    
    [SerializeField] bool usingControllers;
    bool hits;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        rayShootPoint = new GameObject("rayCastStart").transform;
        rayShootPoint.SetParent(transform);
        rayShootPoint.SetPositionAndRotation(transform.position,transform.rotation);
        rayShootPoint.transform.position = new Vector3(transform.position.x, transform.position.y, transform.forward.z * shootPointZ);
    }

    private void OnEnable() {
        XRSettings.onSettingChange += SettingChange;
    }

    private void OnDisable() {
        XRSettings.onSettingChange -= SettingChange;
        HideRay();
    }

    public void SettingChange(SettingSO settings)
    {
        usingControllers = settings.controllersInUse;
    }

    private void Update() 
    {
        if(!usingControllers)
        {
            return;
        }
        
        RayFromController();
        
    }

    public void RayFromController()
    {
        hits = Physics.Raycast(transform.position, transform.forward, out hit, uiRaycastDistance, uiRaycastMask);

        if(hits)
        {
            ShowLine(hit);
        }
        else
        {
            HideRay();
        }
    }

    private void HideRay()
    {
        if(lineRenderer.positionCount > 0)
            lineRenderer.positionCount = 0;
    }

    private void ShowLine(RaycastHit hits)
    {
        if(lineRenderer.positionCount == 0)
        {
            lineRenderer.positionCount = 2;
        }
        
        lineRenderer.SetPosition(0, rayShootPoint.position);
        lineRenderer.SetPosition(1, hit.point);
    }
}
