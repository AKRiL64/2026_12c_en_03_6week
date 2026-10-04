using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected bool gravity = true;
    protected Rigidbody2D Rb;

    [SerializeField] protected int defaultLayer = 0;
    [SerializeField] protected int launchedProjectileLayer = 8;
    [SerializeField] protected float flyDistance = 100f;
    protected Vector2 InitialPosition;
    protected bool IsLaunched = false;
    
    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        InitialPosition = transform.position;
    }

    protected virtual void FixedUpdate()
    {
        if(IsLaunched && Vector2.Distance(transform.position, InitialPosition) > flyDistance)
            Destroy(gameObject);
    }

    public virtual void Launch(float launchForce)
    {
        SetupMetadata();
        Rb.AddForce(GetDirection() * launchForce * Rb.mass, ForceMode2D.Impulse);
    }

    protected virtual Vector2 GetDirection()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mousePosition - (Vector2)transform.position;
        return direction.normalized;
    }

    protected void SetupMetadata()
    {
        gameObject.layer = launchedProjectileLayer;
        Rb.bodyType = RigidbodyType2D.Dynamic;
        Rb.gravityScale = gravity ? 1 : 0;
        
        InitialPosition = transform.position;
        IsLaunched = true;
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        Stop();
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        Stop();
    }

    protected virtual void Stop()
    {
        Rb.linearVelocityX = 0;
        gameObject.layer = defaultLayer;
        IsLaunched = false;
    }
}
