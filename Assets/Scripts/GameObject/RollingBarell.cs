
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RollingBarrel : MonoBehaviour
{
    [SerializeField] private float rollSpeed = 4f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (rollSpeed != 0)
        {
            rb.linearVelocity = new Vector2(
                rollSpeed,
                rb.linearVelocity.y
            );
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            rollSpeed = 0f;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}