using UnityEngine;

public class PlayerController: MonoBehaviour {
    [SerializeField] private PlayerInputHandler input;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;
    
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private bool IsOnTheFloor() {
        return Physics2D.OverlapCircle(
            groundCheckPoint.position,
            groundCheckRadius,
            groundLayer
        );
    }
    
    private void FixedUpdate() {
        Vector2 playerInputVector = input.InputVector;
        rb.linearVelocityX = playerInputVector.x * moveSpeed;
        
        if (input.JumpPressed && IsOnTheFloor()) {
            rb.linearVelocityY = jumpForce;
        }
        
        Debug.Log(IsOnTheFloor() + " " + input.JumpPressed);
    }
}
