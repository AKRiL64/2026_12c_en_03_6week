using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CannonBall : Projectile
{

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerController playerController))
        {
            Debug.Log($"Player {collision.gameObject.name} collided with {playerController}");
            playerController.TakeDamage(1);
        }
        base.OnCollisionEnter2D(collision);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            playerController.TakeDamage(1);
        }
        base.OnTriggerEnter2D(other);
    }

    protected override void Stop()
    {
        if(IsLaunched)
            Destroy(gameObject);
    }
}
