using System.Collections.Generic;
using UnityEngine;

public class MovingBlock: MonoBehaviour, IPullable
{
    public enum Axis {X, Y}
    
    [SerializeField] private List<Axis> allowedAxises = new List<Axis>();
    [SerializeField] private Rigidbody2D rb;
    
    private void Awake()
    {
        ApplyAxisConstraints();
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
        Vector2  direction = cannon.transform.position - transform.position;
        direction.Normalize();
        if (!allowedAxises.Contains(Axis.X))
        {
            direction.x = 0;
        }

        if (!allowedAxises.Contains(Axis.Y))
        {
            direction.y = 0;
        }
        
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
