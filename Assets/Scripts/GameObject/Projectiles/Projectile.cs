using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected bool gravity = true;
    protected Rigidbody2D Rb;

    [SerializeField] protected int defaultLayer = 0;
    [SerializeField] protected int launchedProjectileLayer = 8;

    protected void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(float launchForce)
    {
        SetupMetadata();
        Rb.AddForce(GetDirection() * launchForce * Rb.mass, ForceMode2D.Impulse);
    }

    private Vector2 GetDirection()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = mousePosition - (Vector2)transform.position;
        return direction.normalized;
    }

    private void SetupMetadata()
    {
        gameObject.layer = launchedProjectileLayer;
        Rb.gravityScale = gravity ? 1 : 0;
    }
    
    public abstract void OnCollisionEnter2D(Collision2D other);
    public abstract void OnTriggerEnter2D(Collider2D other);

    protected void Stop()
    {
        Rb.linearVelocityX = 0;
        gameObject.layer = defaultLayer;
    }
}
