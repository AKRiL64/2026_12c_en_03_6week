using UnityEngine;

public class PatrolEnemy : BaseEnemy
{
    [SerializeField] private float speed = 2f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 2f;
    [SerializeField] private float wallCheckDistance = 0.5f;

    private Vector2 direction = Vector2.left;

    protected override void Update()
    {
        base.Update();

        if (state == EnemyState.Idle)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (!HasGroundAhead(false) || HasWallAhead())
        {
            TurnAround();
        }

        Move();
    }

    private bool HasGroundAhead(bool jumping)
    {
        Vector2 origin =
            (Vector2)transform.position +
            direction * 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        Vector2 origin2 =
    (Vector2)transform.position +
    direction * 2f;

        RaycastHit2D hit2 = Physics2D.Raycast(
            origin2,
            Vector2.down,
            groundCheckDistance * 5,
            groundLayer
        );
        if (jumping == false)
            return hit.collider != null;
        else
            return hit.collider != null || hit2.collider != null;
    }

    private bool HasWallAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            wallCheckDistance,
            groundLayer
        );

        return hit.collider != null;
    }

    private void TurnAround()
    {
        direction.x *= -1f;
    }

    protected override void ChaseTarget()
    {
        direction.x = Mathf.Sign(target.position.x - transform.position.x);

        if (HasGroundAhead(true) && !HasWallAhead())
        {
            Move();
        }
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(
            direction.x * speed,
            rb.linearVelocity.y
        );
    }

    private void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.green;

        Vector3 origin =
            transform.position +
            (Vector3)direction * 0.5f;

        Gizmos.DrawRay(
            origin,
            Vector2.down * groundCheckDistance
        );

        Vector3 origin2 =
    transform.position +
    (Vector3)direction * 2f;

        Gizmos.DrawRay(
            origin2,
            Vector2.down * groundCheckDistance * 5
        );

        Gizmos.color = Color.red;

        Gizmos.DrawRay(
            transform.position,
            direction * wallCheckDistance
        );
    }
}