using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private string projectilesTag = "Pullable";
    [SerializeField] private float launchForce = 10;
    [SerializeField] private int launchedProjectileLayer = 8;
    
    private InputSystem_Actions _actions;
    private Vector2 _mousePosition;
    
    [SerializeField] private float angleOffset = -90;

    void Awake()
    {
        _actions = new InputSystem_Actions();
        _actions.Player.Attack.Disable();
        _actions.Player.Pull.Enable();
    }

    void OnEnable()
    {
        _actions.Player.Pull.performed += TryPullObject;
        _actions.Player.Attack.performed += Shoot;
    }

    void OnDisable()
    {
        _actions.Player.Pull.performed -= TryPullObject;
        _actions.Player.Attack.performed -= Shoot;
    }

    void TryPullObject(InputAction.CallbackContext ctx)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _mousePosition);

        if (hit.collider != null && hit.collider.gameObject.CompareTag(projectilesTag))
        {
            //Debug.Log(hit.collider.gameObject.name);
            MoveObjectToCannon(hit);
            _actions.Player.Pull.Disable();
        }
    }

    void Shoot(InputAction.CallbackContext ctx)
    {
        LaunchProjectile();
        _actions.Player.Attack.Disable();
        _actions.Player.Pull.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(projectilesTag))
            CatchProjectile(other);
    }

    private void LaunchProjectile()
    {
        projectile.transform.position = transform.position;
        
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        
        Vector2 direction = _mousePosition - (Vector2)transform.position;
        projectile.SetActive(true);
        rb.AddForce(direction * launchForce * rb.mass, ForceMode2D.Impulse);
    }

    private void CatchProjectile(Collider2D other)
    {
        GameObject proj = other.gameObject;
        proj.SetActive(false);
        proj.GetComponent<Follower>().enabled = false;
        proj.layer = launchedProjectileLayer;
        projectile = proj;
        
        _actions.Player.Attack.Enable();
    }

    private void MoveObjectToCannon(RaycastHit2D hit)
    {
        Follower follower = hit.collider.gameObject.GetComponent<Follower>();
        follower.target = gameObject;
        follower.enabled = true;
    }
}
