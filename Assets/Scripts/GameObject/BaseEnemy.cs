
using System;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    protected enum EnemyState
    {
        Idle,
        Chasing,
        Attacking,
        Falling
    }

    protected EnemyState state = EnemyState.Idle;
    protected Transform target;
    protected Rigidbody2D rb;

    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected float detectionRange = 15f;
    [SerializeField] protected float forgetRange = 30f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        UpdateState();
    }

    protected virtual void UpdateState()
    {
        switch (state)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;

            case EnemyState.Chasing:
                HandleChasing();
                break;

            case EnemyState.Attacking:
                HandleAttacking();
                break;
            
            case EnemyState.Falling:
                HandleFalling();
                break;
        }
    }

    protected virtual void HandleIdle()
    {
        FindPlayer();
    }

    protected virtual void HandleChasing()
    {
        if (target == null)
        {
            state = EnemyState.Idle;
            return;
        }

        if (!IsTargetInRange(forgetRange))
        {
            target = null;
            state = EnemyState.Idle;
            return;
        }

        ChaseTarget();
    }

    protected virtual void HandleAttacking()
    {
    }

    protected virtual void HandleFalling()
    {
        
    }

    protected virtual void FindPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            detectionRange,
            playerLayer
        );

        if (hit != null && hit.CompareTag("Player"))
        {
            target = hit.transform;
            state = EnemyState.Chasing;
        }
    }

    protected bool IsTargetInRange(float range)
    {
        return target != null &&
               Vector2.Distance(transform.position, target.position) <= range;
    }

    protected abstract void ChaseTarget();

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, forgetRange);
    }

    public virtual void StopMoving()
    {
        state = EnemyState.Falling;
        rb.linearVelocity = Vector2.zero;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Cannon") || collision.gameObject.CompareTag("Player"))
            return;
        state = EnemyState.Idle;
    }
}