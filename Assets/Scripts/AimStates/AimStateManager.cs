using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class AimStateManager : MonoBehaviour
{
    [HideInInspector] public AimBaseState currentState;
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

    public Transform aimPosition;
    [HideInInspector] public Vector3 actualAimPosition;
    [SerializeField] float aimTransitionSpeed = 20f;
    [SerializeField] LayerMask aimMask;

    float xFollowPosition;
    float yFollowPosition, ogYFollowPosition;
    [SerializeField] float shoulderSwapSpeed = 10;
    [SerializeField] float crouchCamHeight = 0.6f;
    MovementStateManager moving;


    private void Start()
    {
        moving = GetComponent<MovementStateManager>();
        xFollowPosition = camFollowPos.localPosition.x;
        ogYFollowPosition = camFollowPos.localPosition.y;
        yFollowPosition = ogYFollowPosition;

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

            // Apply sensitivity
            xRotation += lookDelta.x * mouseSensitivity;
            yRotation -= lookDelta.y * mouseSensitivity;

            // Clamp vertical rotation
            yRotation = Mathf.Clamp(yRotation, -80f, 80f);
        }


        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
        {
            aimPosition.position = Vector3.Lerp(aimPosition.position, hit.point, aimTransitionSpeed * Time.deltaTime);
        }
        else
        {
            aimPosition.position = ray.GetPoint(100f); // Default far away position if no hit
        }

        MoveCamera();

        currentState.UpdateState(this);

        cam.Lens.FieldOfView = Mathf.Lerp(cam.Lens.FieldOfView, currentFov, aimTransitionSpeed * Time.deltaTime);
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

    void MoveCamera()
    {
        if (playerInput.actions["SwapShoulder"].triggered) xFollowPosition = -xFollowPosition;
        if (moving.currentState == moving.Crouch) yFollowPosition = crouchCamHeight;
        else yFollowPosition = ogYFollowPosition;

        Vector3 newFollowPosition = new Vector3(xFollowPosition, yFollowPosition, camFollowPos.localPosition.z);
        camFollowPos.localPosition = Vector3.Lerp(camFollowPos.localPosition, newFollowPosition, shoulderSwapSpeed * Time.deltaTime);
    }
}