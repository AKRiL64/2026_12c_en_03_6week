using UnityEngine;
using UnityEngine.InputSystem;

public class FierceTooth : Projectile
{
    public override void OnCollisionEnter2D(Collision2D other)
    {
        Stop();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        Stop();
    }
}
