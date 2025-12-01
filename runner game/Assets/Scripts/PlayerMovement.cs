using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float boundaryOffset = 0.5f;
    
    [Header("Настройки прыжка")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCooldown = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    
    private Vector2 moveInput;
    private bool isGrounded = true;
    private bool canJump = true;
    private float lastJumpTime;
    
    private float leftBound;
    private float rightBound;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        SetupBoundaries();
    }
    
    private void Start()
    {
        rb.freezeRotation = true;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    
    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        Move();
        ClampToRoad();
        if (jumpAction.WasPressedThisFrame() && canJump && isGrounded)
        {
            Jump();
        }
        CheckGround();
    }
    
    private void Move()
    {
        float horizontalSpeed = moveInput.x * moveSpeed;
        Vector3 velocity = rb.linearVelocity;
        velocity.x = horizontalSpeed;
        rb.linearVelocity = velocity;
    }
    
    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        canJump = false;
        lastJumpTime = Time.time;
    }
    
    private void CheckGround()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer);
        if (isGrounded && Time.time - lastJumpTime > jumpCooldown)
        {
            canJump = true;
        }
    }
    
    private void SetupBoundaries()
    {
        GameObject road = GameObject.FindGameObjectWithTag("Road");
        Collider roadCollider = road.GetComponent<Collider>();
        Bounds bounds = roadCollider.bounds;
        leftBound = bounds.min.x + boundaryOffset;
        rightBound = bounds.max.x - boundaryOffset;
    }
    
    private void ClampToRoad()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, leftBound, rightBound);
        transform.position = position;
    }
}