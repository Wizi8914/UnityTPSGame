using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class AimStateManager : MonoBehaviour
{
    AimBaseState currentState;
    public HipFireState Hip = new HipFireState();
    public AimState Aim = new AimState();

    [SerializeField] private Transform camFollowPos;
    [SerializeField] private float mouseSensitivity = 1f;

    private float xRotation;
    private float yRotation;
    public PlayerInput playerInput;
    private InputAction lookAction;

    [HideInInspector] public Animator animator;
    [HideInInspector] public CinemachineCamera cam;
    public float adsFov = 40f;
    [HideInInspector] public float hipFov;
    [HideInInspector] public float currentFov;
    public float fovTransitionSpeed = 10;

    private void Start()
    {
        // Get Cinemachine Camera
        cam = GetComponentInChildren<CinemachineCamera>();
        hipFov = cam.Lens.FieldOfView;

        animator = GetComponent<Animator>();
        // Set up input
        playerInput = GetComponentInChildren<PlayerInput>();
        SwitchState(Hip);

        if (playerInput != null)
        {
            lookAction = playerInput.actions["Look"];
            lookAction?.Enable();
        }

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        lookAction?.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (lookAction != null)
        {
            // Get input from new Input System
            Vector2 lookDelta = lookAction.ReadValue<Vector2>();

            Debug.Log($"Look Delta: {lookDelta}");

            // Apply sensitivity
            xRotation += lookDelta.x * mouseSensitivity;
            yRotation -= lookDelta.y * mouseSensitivity;

            // Clamp vertical rotation
            yRotation = Mathf.Clamp(yRotation, -80f, 80f);
        }

        currentState.UpdateState(this);

        cam.Lens.FieldOfView = Mathf.Lerp(cam.Lens.FieldOfView, currentFov, fovTransitionSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (camFollowPos != null)
        {
            // Apply rotations
            camFollowPos.localEulerAngles = new Vector3(yRotation, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, xRotation, transform.eulerAngles.z);
        }
    }
    
    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}