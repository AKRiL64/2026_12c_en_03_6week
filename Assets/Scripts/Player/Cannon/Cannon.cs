using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private string projectilesTag = "Pullable";
    [SerializeField] private float launchForce = 10;
    [SerializeField] private float pullRadius = 10;
    
    private InputSystem_Actions _actions;
    private Vector2 _mousePosition;
    
    private LayerMask _pullLayerMask;
    private Collider2D _collider;

    void Awake()
    {
        _actions = new InputSystem_Actions();
        _actions.Player.Attack.Disable();
        _actions.Player.Pull.Enable();
        
        _pullLayerMask = ~LayerMask.GetMask("Player");
        if(_collider == null)
            _collider = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        _actions.Player.Pull.started += TryPullObject;
        _actions.Player.Pull.canceled += CancelPulling;
        _actions.Player.Attack.performed += Shoot;
    }

    void OnDisable()
    {
        _actions.Player.Pull.started -= TryPullObject;
        _actions.Player.Pull.canceled -= CancelPulling;
        _actions.Player.Attack.performed -= Shoot;
    }

    void TryPullObject(InputAction.CallbackContext ctx)
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _mousePosition, pullRadius, _pullLayerMask);
        Debug.DrawRay(transform.position, _mousePosition, Color.red, 0.5f);
        
        if (hit.collider != null && hit.collider.gameObject.CompareTag(projectilesTag))
        {
            Debug.DrawRay(transform.position, hit.centroid, Color.green, 0.5f);
            projectile = hit.collider.gameObject;
            MoveObjectToCannon(hit);
            
            _actions.Player.Pull.started -= TryPullObject;
        }
    }

    void CancelPulling(InputAction.CallbackContext ctx)
    {
        if (projectile != null)
        {
            Follower follower = projectile.GetComponent<Follower>();
            follower.enabled = false;
            projectile = null;
            _collider.enabled = false;
            _actions.Player.Pull.started += TryPullObject;
        }
    }

    void Shoot(InputAction.CallbackContext ctx)
    {
        LaunchProjectile();
        _actions.Player.Attack.Disable();
        _actions.Player.Pull.started += TryPullObject;
        _actions.Player.Pull.canceled += CancelPulling;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(projectilesTag) && other.gameObject == projectile)
            CatchProjectile(other);
    }

    private void LaunchProjectile()
    {
        PositionProjectile();
        projectile.SetActive(true);
        projectile.GetComponent<Projectile>().Launch(launchForce);
        projectile = null;
    }

    private void PositionProjectile()
    {
        projectile.transform.position = transform.position;
        
        projectile.transform.rotation *= Quaternion.FromToRotation(
            projectile.transform.up, transform.up);
    }

    private void CatchProjectile(Collider2D other)
    {
        _collider.enabled = false;
        
        GameObject proj = other.gameObject;
        proj.SetActive(false);
        proj.GetComponent<Follower>().enabled = false;
        
        _actions.Player.Pull.canceled -= CancelPulling;
        _actions.Player.Attack.Enable();
    }

    private void MoveObjectToCannon(RaycastHit2D hit)
    {
        _collider.enabled = true;
        
        Follower follower = hit.collider.gameObject.GetComponent<Follower>();
        follower.target = gameObject;
        follower.enabled = true;
    }
}
