using System.Collections.Generic;
using UnityEngine;

public class MovingBlock: MonoBehaviour, IPullable
{
    public enum Axis {X, Y}
    
    [SerializeField] private List<Axis> allowedAxises = new List<Axis>();
    [SerializeField] private Rigidbody2D rb;
    
    private Cannon pullingCannon;
    private float pullingForce;
    
    private void Awake()
    {
        //ApplyAxisConstraints();
    }
    
    private void ApplyAxisConstraints()
    {
        RigidbodyConstraints2D constraints = RigidbodyConstraints2D.None;

        if (!allowedAxises.Contains(Axis.X))
        {
            constraints |= RigidbodyConstraints2D.FreezePositionX;
        }

        if (!allowedAxises.Contains(Axis.Y))
        {
            constraints |= RigidbodyConstraints2D.FreezePositionY;
        }

        rb.constraints = constraints;
    }
    
    public void Pull(Cannon cannon, float force)
    {
        // Avoid duplicate subscriptions if Pull is called repeatedly.
        if (pullingCannon != null) pullingCannon.OnPullableRelease -= StopPulling;
        
        pullingCannon = cannon;
        pullingForce = force;

        pullingCannon.OnPullableRelease += StopPulling;
    }

    private void StopPulling()
    {
        if (pullingCannon != null) pullingCannon.OnPullableRelease -= StopPulling;
        pullingCannon = null;
    }

    private void FixedUpdate()
    {
        if (!pullingCannon)
            return;

        Vector2 direction = (Vector2)pullingCannon.transform.position - rb.position;

        if (!allowedAxises.Contains(Axis.X))
            direction.x = 0;

        if (!allowedAxises.Contains(Axis.Y))
            direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            direction.Normalize();
            rb.linearVelocity = direction * pullingForce / rb.mass;
        }
    }
}
