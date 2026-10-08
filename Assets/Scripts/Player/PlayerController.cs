using UnityEngine;

public class PlayerController: MonoBehaviour 
{
    [SerializeField] private PlayerInputHandler input;
    [SerializeField] private BoxCollider2D playerCollider;
    //TODO: change Rigidbody with Character Controller
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 6.5f;


    //For ground check
    [SerializeField] private float groundCheckDistance = 0.05f;
    [SerializeField] private LayerMask groundLayer;
    
    //To change the jump curve
    [SerializeField] private float gravityModifierUp = 1.2f;
    [SerializeField] private float gravityModifierDown = 1.7f;

    private const float CoyoteTimeMax = 0.1f; //100ms
    private float _coyoteTimer = CoyoteTimeMax;

    private const float JumpBufferTimeMax = 0.12f; //120ms
    private float _jumpBufferTimer = JumpBufferTimeMax;
    
    //if both Coyote Time and Jump Input Buffer are sufficient - allows jump
    private bool CanJump =>
        _jumpBufferTimer < JumpBufferTimeMax &&
        _coyoteTimer < CoyoteTimeMax;

    private void Update()
    {
        //Applies speed in the move input direction
        Move();
        
        //Updates Coyote Time and Jump Input Buffer
        UpdateBuffers();
        
        //Executes jump if all conditions for it are met
        TryToJump();

        //Switches between different gravity modifiers based on Y movement direction
        ChangeGravityModifier();
    }

    private void Move()
    {
        float inputX = input.InputVector.x;
        float directionX = 0f;
        if (inputX != 0f)
        {
            directionX = Mathf.Sign(inputX);
        }
        
        rb.linearVelocityX = directionX * moveSpeed;
    }

    private void UpdateBuffers()
    {
        if (IsOnTheFloor())
        {
            _coyoteTimer = 0f;
        }
        else
        {
            _coyoteTimer += Time.deltaTime;
            if (_coyoteTimer > CoyoteTimeMax) _coyoteTimer = CoyoteTimeMax;
        }

        if (input.JumpPressed)
        {
            _jumpBufferTimer = 0;
        }
        else
        {
            _jumpBufferTimer += Time.deltaTime;
            if (_jumpBufferTimer > JumpBufferTimeMax)  _jumpBufferTimer = JumpBufferTimeMax;
        }
        
    }

    private bool IsOnTheFloor() 
    {
        Bounds bounds = playerCollider.bounds;
        
        Vector2 groundCheckPoint = new Vector2(
            bounds.center.x,
            bounds.min.y
        );

        Vector2 groundCheckSize = new Vector2(
            bounds.size.x,
            groundCheckDistance
        );

        return Physics2D.OverlapBox(
            groundCheckPoint,
            groundCheckSize,
            0f,
            groundLayer
        );
    }

    private void TryToJump()
    {
        if (CanJump)
        {
            rb.linearVelocityY = jumpForce;
            _jumpBufferTimer = JumpBufferTimeMax;
            _coyoteTimer = CoyoteTimeMax;
        }
    }
    
    private void ChangeGravityModifier()
    {
        if (rb.linearVelocityY < 0f)
        {
            rb.gravityScale = gravityModifierDown;
        }
        else
        {
            rb.gravityScale = gravityModifierUp;
        }
    }
}
