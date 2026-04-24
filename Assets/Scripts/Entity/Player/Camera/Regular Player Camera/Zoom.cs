using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(OrbitCamera))]
public class ZoomCamera : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private InputActionReference zoomAction;
    [SerializeField] private float minZoomDistance = 2f;
    [SerializeField] private float maxZoomDistance = 10f;
    [SerializeField] private float zoomSensitivity = 0.5f;
    [SerializeField] private float zoomSmoothness = 10f;

    private float targetDistance;
    private float currentDistance;

    OrbitCamera orbitCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        orbitCamera = GetComponent<OrbitCamera>();
        targetDistance = orbitCamera.distance;
        currentDistance = orbitCamera.distance;
    }

    private void OnEnable()
    {
        zoomAction.action.Enable();
    }
    private void OnDisable()
    {
        zoomAction.action.Disable();
    }

    private void LateUpdate()
    {
        DoZoom();
    }
    void DoZoom()
    {
        float scrollInput = zoomAction.action.ReadValue<float>();

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            targetDistance -= scrollInput * zoomSensitivity;
            targetDistance = Mathf.Clamp(targetDistance, minZoomDistance, maxZoomDistance);
        }

        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * zoomSmoothness);

        orbitCamera.distance = currentDistance;
    }
}
