using UnityEngine;
using UnityEngine.InputSystem;


public class MovementStateManager : MonoBehaviour
{
    // Movement
    public float currentMoveSpeed;
    public float walkSpeed = 3, walkBackSpeed = 2;
    public float runSpeed = 7, runBackSpeed = 5;
    public float crouchSpeed = 2, crouchBackSpeed = 1;
    

    [HideInInspector] public Vector3 moveDirection;
    [HideInInspector] public float hzInput, vtInput;
    CharacterController characterController;

    // Ground check
    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundLayer;
    Vector3 groundCheckPosition;

    // Gravity
    [SerializeField] float gravityScale = -9.81f;
    Vector3 gravityVelocity;


    // Movement states
    MovementBaseState currentState;

    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public RunState Run = new RunState();
    public CrouchState Crouch = new CrouchState();

    [HideInInspector] public Animator animator;

    [HideInInspector] public PlayerInput playerInput;


    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        SwitchState(Idle); // Start with Idle state
    }

    void Update()
    {
        GetDirectionAndMove();
        Gravity();

        animator.SetFloat("hzInput", hzInput);
        animator.SetFloat("vtInput", vtInput);

        currentState.UpdateState(this);
    }

    public void SwitchState(MovementBaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    void GetDirectionAndMove()
    {
        playerInput.actions["Move"].ReadValue<Vector2>();

        //hzInput = playerInput.actions["Move"].ReadValue<Vector2>().x;
        //vtInput = playerInput.actions["Move"].ReadValue<Vector2>().y;

        // Get horizontal and vertical input
        hzInput = Input.GetAxis("Horizontal");
        vtInput = Input.GetAxis("Vertical");

        Debug.Log($"hzInput: {hzInput}, vtInput: {vtInput}");

        moveDirection = transform.forward * vtInput + transform.right * hzInput;

        characterController.Move(moveDirection.normalized * currentMoveSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        groundCheckPosition = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(groundCheckPosition, characterController.radius - 0.05f, groundLayer)) return true;
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded()) gravityVelocity.y += gravityScale * Time.deltaTime;
        else if (gravityVelocity.y < 0) gravityVelocity.y = -2;

        characterController.Move(gravityVelocity * Time.deltaTime);
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPosition, characterController.radius - 0.05f);
    }
    */
}
