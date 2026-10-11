using UnityEngine;

public class CannonBall : Projectile
{
    protected override void Stop()
    {
        if(IsLaunched)
            Destroy(gameObject);
    }
}
