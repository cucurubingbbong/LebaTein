using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minSize = 2f;
    [SerializeField] private float maxSize = 10f;

    [SerializeField] private float anchorZ = 15f; 

    [SerializeField] private Camera cam;

    private Plane anchorPlane;

    private void Awake()
    {
        anchorPlane = new Plane(Vector3.forward, new Vector3(0f, 0f, anchorZ));
    }

    private void LateUpdate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Approximately(scroll, 0f)) return;

        // 줌 전: 마우스가 찍는 월드 좌표
        Vector3 before = MouseWorldOnPlane();

        // 줌
        float size = cam.orthographicSize;
        size -= scroll * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(size, minSize, maxSize);

        // 줌 후
        Vector3 after = MouseWorldOnPlane();

        // 카메라 보정
        Vector3 delta = before - after;
        delta.z = 0f;

        cam.transform.position += delta;
    }

    private Vector3 MouseWorldOnPlane()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (anchorPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }
        return cam.transform.position;
    }
}