using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    [Header("Speed Settings")]
    [SerializeField]
    private float sprintSpeed = 5f;
    [SerializeField]
    private float speedMultiplier = 1f;
    [SerializeField]
    private float baseSpeedMultiplier = 1f;
    [SerializeField]
    private float minSpeedMultiplier = 0.3f;

    private float playerSpeed = 5f;


    Rigidbody playerRb;

    [Header("Input Settings")]
    [HideInInspector]
    Vector2 moveInput;

    [SerializeField]
    InputActionReference moveAction;
    [SerializeField]
    InputActionReference sprintAction;

    [Header("Rotation Settings")]
    [SerializeField]
    private OrbitCamera orbitCamera;
    [SerializeField]
    private float rotationSpeed = 10f;

    private Entity _playerEntity;
    private EntityManager _entityManager;
    private bool _entityFound = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var query = _entityManager.CreateEntityQuery(typeof(PlayerTag), typeof(SpeedComponent));

        if (query.TryGetSingletonEntity<PlayerTag>(out _playerEntity))
            _entityFound = true;
        else
            Debug.LogError($"{gameObject.name}: Could not find player entity.");

        query.Dispose();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        sprintAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        sprintAction.action.Disable();
    }

    private void Update()
    {
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            IncreaseSpeed(2f);
        }
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            DecreaseMultiplierSpeed(2f);
        }

        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            ResetMultiplierSpeed();
        }
    }

    private void FixedUpdate()
    {
        DoSpeed();
    }

    void DoSpeed()
    {
        if (_entityFound)
            playerSpeed = _entityManager.GetComponentData<SpeedComponent>(_playerEntity).Current;
        float currentSpeed = ((playerSpeed) + Sprint()) * speedMultiplier;

        moveInput = moveAction.action.ReadValue<Vector2>();

        Vector3 moveDirection = (orbitCamera.GetCameraForward() * moveInput.y + orbitCamera.GetCameraRight() * moveInput.x).normalized;

        Vector3 finalVelocity = new Vector3(moveDirection.x * currentSpeed, playerRb.linearVelocity.y, moveDirection.z * currentSpeed);
        playerRb.linearVelocity = finalVelocity;

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            playerRb.MoveRotation(Quaternion.Slerp(playerRb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
        }

    }

    float Sprint()
    {
        if (sprintAction.action.IsPressed() && moveInput.y > 0.1f)
        {
            return sprintSpeed;
        }
        return 0f;
    }

    public void ResetMultiplierSpeed()
    {
        speedMultiplier = baseSpeedMultiplier;
    }
    public void DecreaseMultiplierSpeed(float value)
    {
        if (value <= 0f)
        {
            Debug.LogWarning("Speed multiplier decrease value should be greater than 0 to have a decreased effect.");
            return;
        }
        if (speedMultiplier <= minSpeedMultiplier)
        {
            Debug.LogWarning("Speed multiplier is already at or below the minimum threshold. Cannot decrease further.");
            return;
        }

        speedMultiplier -= value;

        speedMultiplier = Mathf.Max(speedMultiplier, minSpeedMultiplier);
    }
    public void IncreaseSpeed(float value)
    {
        if (value <= 0f)
        {
            Debug.LogWarning("Speed multiplier should be greater than 0 to have an increased effect.");
            return;
        }
        speedMultiplier += value;
    }
}
