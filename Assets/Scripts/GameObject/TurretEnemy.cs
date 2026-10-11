using UnityEngine;

public class TurretEnemy : BaseEnemy
{
    [SerializeField] private Transform weapon;

    [SerializeField] private Transform shootPoint;

    [SerializeField] private GameObject projectile;
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float chargeTime = 0.5f;
    [SerializeField] private float squishAmount = 0.7f;

    private float attackTimer;
    private float chargeTimer;
    private bool isCharging;
    private Vector3 originalScale;
    protected override void Awake()
    {
        base.Awake();

        originalScale = transform.localScale;
        attackTimer = attackCooldown;
    }
    protected override void HandleIdle()
    {
        FindPlayer();
    }

    protected override void HandleChasing()
    {
        if (target == null)
        {
            state = EnemyState.Idle;
            return;
        }

        if (Vector2.Distance(transform.position, target.position) <= attackRange)
        {
            state = EnemyState.Attacking;
            return;
        }

        AimAtTarget();
    }

    protected override void HandleAttacking()
    {
        if (target == null)
        {
            state = EnemyState.Idle;
            transform.localScale = originalScale;
            return;
        }

        if (Vector2.Distance(transform.position, target.position) > attackRange)
        {
            state = EnemyState.Chasing;
            transform.localScale = originalScale;
            return;
        }

        AimAtTarget();

        if (!isCharging)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                isCharging = true;
                chargeTimer = chargeTime;
            }
        }
        else
        {
            chargeTimer -= Time.deltaTime;

            float chargeProgress = 1f - (chargeTimer / chargeTime);

            chargeProgress = Mathf.SmoothStep(0f, 1f, chargeProgress);

            float currentScale = Mathf.Lerp(
                1f,
                squishAmount,
                chargeProgress
            );

            transform.localScale = originalScale * currentScale;

            if (chargeTimer <= 0f)
            {
                Attack();

                isCharging = false;
                attackTimer = attackCooldown;

                transform.localScale = originalScale;
            }
        }
    }

    protected override void ChaseTarget()
    {
        AimAtTarget();
    }

    private void AimAtTarget()
    {
        if (target == null || weapon == null)
            return;

        Vector2 direction = target.position - weapon.position;

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation =
            Quaternion.Euler(0f, 0f, angle);

        weapon.rotation = Quaternion.RotateTowards(
            weapon.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void Attack()
    {
        Instantiate(projectile, shootPoint.position, weapon.rotation);
    }
}