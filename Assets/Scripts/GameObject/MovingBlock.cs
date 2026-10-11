using System.Collections.Generic;
using UnityEngine;

public class MovingBlock: MonoBehaviour, IPullable
{
    public enum Axis {X, Y}
    
    [SerializeField] private List<Axis> allowedAxises = new List<Axis>();
    [SerializeField] private Rigidbody2D rb;
    
    public void Pull(PlayerController player, float force)
    {
        Vector2  direction = player.transform.position - transform.position;
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
