using UnityEngine;

public class Totem : TurretEnemy
{
    [SerializeField] private bool shootLeft = true;
    
    protected override void HandleAttacking()
    {
        if ((transform.position.x - target.position.x < 0 && shootLeft) ||
            (transform.position.x - target.position.x > 0 && !shootLeft))
        {
            state = EnemyState.Chasing;
            transform.localScale = originalScale;
            return;
        }
        base.HandleAttacking();
    }
}
