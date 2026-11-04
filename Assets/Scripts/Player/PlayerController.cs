using System;
using FMODUnity;
using UnityEngine;

public class PlayerController: MonoBehaviour 
{
    [SerializeField] GameObject cannon;
    
    [SerializeField] private PlayerInputHandler input;
    [SerializeField] private SpriteRenderer visuals;
    private bool _wasGrounded;
    private bool _isGrounded;
    [SerializeField] private Animator animator;
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
    
    //Audio
    private float _distanceSinceFootstep;
    private bool _wasOnFloor;

    //Hp system
    [SerializeField] private int MaxHp = 3;
    private int _currentHp;
    
    //Spawning player on the last position he was on the ground in case of falling off into the water
    private Vector2 _lastPosition;
    
    private const float CoyoteTimeMax = 0.1f; //100ms
    private float _coyoteTimer = CoyoteTimeMax;

    private const float JumpBufferTimeMax = 0.12f; //120ms
    private float _jumpBufferTimer = JumpBufferTimeMax;
    
    //if both Coyote Time and Jump Input Buffer are sufficient - allows jump
    private bool CanJump =>
        _jumpBufferTimer < JumpBufferTimeMax &&
        _coyoteTimer < CoyoteTimeMax;
    
    
    public bool HasDownInput => input.InputVector.y < 0;

    private void Awake()
    {
        CheckpointManager.SetPlayer(gameObject);
    }

    private void Start()
    {
        _lastPosition = transform.position;
        _currentHp = MaxHp;
        HpBar.Instance.ChangeHp(_currentHp, MaxHp);
        _wasOnFloor = IsOnTheFloor();
    }
    
    private void Update()
    {
        //Applies speed in the move input direction
        Move();
        
        PlayFootstepSound();
        
        PlayLandingSound();
        
        //Updates Coyote Time and Jump Input Buffer
        UpdateBuffers();

        UpdateGroundedState(); 

        //Executes jump if all conditions for it are met
        TryToJump();

        //Switches between different gravity modifiers based on Y movement direction
        ChangeGravityModifier();
        
        //Update last position on the ground
        UpdateLastPosition();
    }

    private void Move()
    {
        float inputX = input.InputVector.x;
        float directionX = 0f;
        if (inputX != 0f)
        {
            directionX = Mathf.Sign(inputX);
            visuals.flipX = directionX != 1;
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
        
        rb.linearVelocityX = directionX * moveSpeed;
    }

    private void UpdateLastPosition()
    {
        if (IsOnTheFloor())
        {
            _lastPosition =  transform.position;
        }
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
    private void UpdateGroundedState()
    {
        _isGrounded = IsOnTheFloor();

        animator.SetBool("grounded", _isGrounded);

        _wasGrounded = _isGrounded;
    }
    private bool IsOnTheFloor() 
    {
        Bounds bounds = playerCollider.bounds;
        
        Vector2 groundCheckPoint = new Vector2(
            bounds.center.x,
            bounds.min.y - groundCheckDistance / 2f
        );

        Vector2 groundCheckSize = new Vector2(
            bounds.size.x * 0.8f,
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
            animator.SetTrigger("jump");
            
            RuntimeManager.PlayOneShot("event:/JUMP", transform.position);
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

    private void PlayFootstepSound()
    {
        float speed = Math.Abs(rb.linearVelocityX);

        if (!IsOnTheFloor() || speed < 0.1f)
        {
            _distanceSinceFootstep = 0f;
            return;
        }
        _distanceSinceFootstep += speed * Time.deltaTime;
        if (_distanceSinceFootstep >= 2f)
        {
            _distanceSinceFootstep = 0f;
            RuntimeManager.PlayOneShot("event:/FOOTSTEPS", transform.position);
        }
    }

    private void PlayLandingSound()
    {
        bool onFloor=  IsOnTheFloor();
        
        if (onFloor && !_wasOnFloor)
        {
            RuntimeManager.PlayOneShot("event:/LAND", transform.position);
        }
        _wasOnFloor = onFloor;
    }

    public void SpawnOnTheFloor()
    {
        rb.linearVelocityX = 0f;
        rb.position = _lastPosition;
    }

    public void TakeDamage(int damage)
    {
        RuntimeManager.PlayOneShot("event:/TAKE_DMG", transform.position);
        
        _currentHp = Math.Clamp(_currentHp - damage, 0, MaxHp);
        HpBar.Instance.ChangeHp(_currentHp, MaxHp);
        if (_currentHp <= 0)
        {
            SaveManager.Load(SaveManager.SaveFilename);
        }
    }

    public void EnableCannon()
    {
        cannon.gameObject.SetActive(true);
    }
}
