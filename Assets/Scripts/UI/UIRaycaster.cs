using UnityEngine;

public class UIRaycaster : MonoBehaviour
{
    LineRenderer lineRenderer;
    RaycastHit hit;
    [SerializeField]private float uiRaycastDistance;
    [SerializeField]LayerMask uiRaycastMask;
    [SerializeField]private float shootPointZ;
    Transform rayShootPoint;
    

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        rayShootPoint = new GameObject("rayCastStart").transform;
        rayShootPoint.SetParent(transform);
        rayShootPoint.SetPositionAndRotation(transform.position,transform.rotation);
        rayShootPoint.transform.position = new Vector3(transform.position.x, transform.position.y, transform.forward.z * shootPointZ);
    }

    private void Update() {
       bool hits = Physics.Raycast(transform.position, transform.forward, out hit, uiRaycastDistance, uiRaycastMask);

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
