using UnityEngine;
using UnityEngine.InputSystem;


public class MovementStateManager : MonoBehaviour
{
    // Movement
    public float currentMoveSpeed;
    public float walkSpeed = 3, walkBackSpeed = 2;
    public float runSpeed = 7, runBackSpeed = 5;
    public float crouchSpeed = 2, crouchBackSpeed = 1;
    public float airSpeed = 1.5f;


    [HideInInspector] public Vector3 moveDirection;
    [HideInInspector] public float hzInput, vtInput;
    CharacterController characterController;

    // Ground check
    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundLayer;
    Vector3 groundCheckPosition;

    // Gravity
    [SerializeField] float gravityScale = -9.81f;
    [SerializeField] float jumpForce = 10f;
    [HideInInspector] public bool jumped;
    Vector3 gravityVelocity;


    // Movement states
    public MovementBaseState previousState;
    public MovementBaseState currentState;

    public IdleState Idle = new IdleState();
    public WalkState Walk = new WalkState();
    public RunState Run = new RunState();
    public CrouchState Crouch = new CrouchState();
    public JumpState Jump = new JumpState();

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
        Falling();

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
        //playerInput.actions["Move"].ReadValue<Vector2>();

        //hzInput = playerInput.actions["Move"].ReadValue<Vector2>().x;
        //vtInput = playerInput.actions["Move"].ReadValue<Vector2>().y;

        // Get horizontal and vertical input
        hzInput = Input.GetAxis("Horizontal");
        vtInput = Input.GetAxis("Vertical");
        Vector3 airDirection = Vector3.zero;

        if (!IsGrounded()) airDirection = transform.forward * vtInput + transform.right * hzInput;
        else moveDirection = transform.forward * vtInput + transform.right * hzInput;

        characterController.Move((moveDirection.normalized * currentMoveSpeed + airDirection.normalized * airSpeed) * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        //groundCheckPosition = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        groundCheckPosition = new Vector3(transform.position.x, transform.position.y + characterController.radius - 0.08f, transform.position.z);
        if (Physics.CheckSphere(groundCheckPosition, characterController.radius - 0.05f, groundLayer)) return true;
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded()) gravityVelocity.y += gravityScale * Time.deltaTime;
        else if (gravityVelocity.y < 0) gravityVelocity.y = -2;

        characterController.Move(gravityVelocity * Time.deltaTime);
    }

    void Falling() => animator.SetBool("Falling", !IsGrounded());

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckPosition, characterController.radius - 0.05f);
    }
    */

    public void JumpForce() => gravityVelocity.y += jumpForce;
    public void Jumped() => jumped = true;
}
