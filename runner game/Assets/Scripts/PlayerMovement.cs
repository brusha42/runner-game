using System;
// using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Настройки движения")]
    [SerializeField] private float baseForwardSpeed = 10f;
    [SerializeField] private float accelerationCoef = 1.0015f;
    [SerializeField] private float maxForwardSpeed = 60f;
    [SerializeField] private float currentSpeed = 10f;
    [SerializeField] private float sideSpeed = 8f;
    [SerializeField] private float boundaryOffset = 0.5f;
    
    [Header("Настройки прыжка")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCooldown = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Слайдинг")]
    [SerializeField] private float slideDuration = 0.6f;
    
    // components
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction crouchAction;
    
    // moving
    private Vector2 moveInput;
    private float leftBound;
    private float rightBound;
    private float playTime = 0f;

    // jumping
    private bool isGrounded = true;
    private bool canJump = true;
    private float lastJumpTime;

    // sliding
    public bool isSliding = false;
    private float slideTimer = 0f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        crouchAction = playerInput.actions["Crouch"];
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
        if (canJump && ((int)transform.position.z % 15 == 0 || Mathf.Abs(rb.linearVelocity.y) > 0.1f))
        {
            Vector3 vel = rb.linearVelocity;
            vel.y = 0;
            rb.linearVelocity = vel;
        }
        moveInput = moveAction.ReadValue<Vector2>();
        playTime += Time.deltaTime;
        UpdateSpeed();
        Move();
        ClampToRoad();
        if (jumpAction.WasPressedThisFrame() && canJump && isGrounded)
        {
            Jump();
        }
        CheckGround();
        HandleSliding();
        UpdateSliding();
    }

    private void HandleSliding()
    {
        if (crouchAction.WasPressedThisFrame() && !isSliding)
        {
            Slide();
        }
    }

    private void Slide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        Vector3 scale = transform.localScale;
        scale.y /= 2;
        transform.localScale = scale;
    }

    private void UpdateSliding()
    {
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            
            if (slideTimer <= 0)
            {
                isSliding = false;
                Vector3 scale = transform.localScale;
                scale.y *= 2;
                transform.localScale = scale;
            }
        }
    }
    
    private void UpdateSpeed()
    {
        if (currentSpeed < maxForwardSpeed)
        { 
            currentSpeed = (float)Math.Clamp(baseForwardSpeed * Math.Pow(accelerationCoef, playTime), baseForwardSpeed, maxForwardSpeed);

        }
    }

    private void Move()
    {
        float horizontalSpeed = moveInput.x * sideSpeed;
        Vector3 velocity = rb.linearVelocity;
        velocity.x = horizontalSpeed;
        velocity.z = currentSpeed;
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