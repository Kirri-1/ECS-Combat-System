using UnityEngine;
using UnityEngine.InputSystem;

public class OrbitCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset = new Vector3(0, 1.5f, 0);

    [Header("Orbit Settings")]
    public float distance = 4f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Input")]
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private float mouseSensitivity = 100f;

    private float yaw = 0f;
    private float pitch = 20f;

    private void OnEnable()
    {
        lookAction.action.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void OnDisable()
    {
        lookAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        EscapeWindow();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;
        OrbitCameraFunc();
    }

    void OrbitCameraFunc()
    {
        Vector2 input = lookAction.action.ReadValue<Vector2>();

        yaw += input.x * mouseSensitivity * Time.deltaTime;
        pitch -= input.y * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion orbitRotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 orbitCenter = target.position + targetOffset;

        transform.position = orbitCenter + orbitRotation * new Vector3(0, 0, -distance);

        transform.LookAt(orbitCenter);
    }

    public Vector3 GetCameraForward()
    {
        return Quaternion.Euler(0, yaw, 0) * Vector3.forward;
    }

    public Vector3 GetCameraRight()
    {
        return Quaternion.Euler(0, yaw, 0) * Vector3.right;
    }


    private void EscapeWindow()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}