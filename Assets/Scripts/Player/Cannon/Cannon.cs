using System;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private ParticleSystem suckParticles;
    private Coroutine suckShakeCoroutine;
    [SerializeField] private string projectilesTag = "Pullable";
    [SerializeField] private float launchForce = 10;
    [SerializeField] private float pullRadius = 10;
    
    [SerializeField] private float pullForce = 10;

    private Collider2D currentlyPulledCollider;
    private Coroutine pullDetectionCoroutine;

    [SerializeField] private float detectionInterval = 0.1f;

    private bool isPullHeld;
    
    private InputSystem_Actions _actions;
    private Vector2 _mousePosition;
    
    private LayerMask _pullLayerMask;
    private Collider2D _collider;

    private DynamicCamera dynamicCamera;
    public event Action OnPullableRelease;
    
    void Awake()
    {
        _actions = new InputSystem_Actions();
        _actions.Player.Attack.Disable();
        _actions.Player.Pull.Enable();
        
        _pullLayerMask = ~LayerMask.GetMask("Player");
        if(_collider == null)
            _collider = GetComponent<Collider2D>();
        dynamicCamera = Camera.main.GetComponent<DynamicCamera>();
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

        isPullHeld = false;

        if (pullDetectionCoroutine != null)
        {
            StopCoroutine(pullDetectionCoroutine);
            pullDetectionCoroutine = null;
        }

        StopSuckEffects();

        _actions.Player.Disable();
    }

    void TryPullObject(InputAction.CallbackContext ctx)
    {
        isPullHeld = true;

        suckParticles.Play();

        if (suckShakeCoroutine == null)
            suckShakeCoroutine = StartCoroutine(SuckShake());

        dynamicCamera.ShakeCamera(0.3f, 0.05f);

        if (pullDetectionCoroutine == null)
            pullDetectionCoroutine = StartCoroutine(DetectPullableObject());
    }

    private IEnumerator DetectPullableObject()
    {
        while (isPullHeld)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            );

            Vector2 mouseDirection = _mousePosition - (Vector2)transform.position;

            RaycastHit2D hit = Physics2D.Raycast(
                transform.position,
                mouseDirection,
                pullRadius,
                _pullLayerMask
            );

            // Currently pulling an IPullable object
            if (currentlyPulledCollider != null)
            {
                if (hit.collider != currentlyPulledCollider)
                {
                    // No longer looking at the object
                    OnPullableRelease?.Invoke();

                    currentlyPulledCollider = null;
                }
            }
            else if (hit.collider != null)
            {
                IPullable pullable = hit.collider.GetComponent<IPullable>();

                if (pullable != null)
                {
                    currentlyPulledCollider = hit.collider;

                    pullable.Pull(this, pullForce);
                }
                else if (hit.collider.gameObject.CompareTag(projectilesTag))
                {
                    projectile = hit.collider.gameObject;

                    MoveObjectToCannon(hit);

                    pullDetectionCoroutine = null;
                    yield break;
                }
            }

            yield return new WaitForSeconds(detectionInterval);
        }

        pullDetectionCoroutine = null;
    }
    private System.Collections.IEnumerator SuckShake()
    {
        while (true)
        {
            dynamicCamera.ShakeCamera(0.3f, 0.02f);
            yield return new WaitForSeconds(0.25f);
        }
    }
    void CancelPulling(InputAction.CallbackContext ctx)
    {
        isPullHeld = false;
        currentlyPulledCollider = null;
        if (pullDetectionCoroutine != null)
        {
            StopCoroutine(pullDetectionCoroutine);
            pullDetectionCoroutine = null;
        }

        StopSuckEffects();

        OnPullableRelease?.Invoke();

        if (projectile != null)
        {
            Follower follower = projectile.GetComponent<Follower>();

            if (follower != null)
                follower.enabled = false;

            projectile = null;

            _collider.enabled = false;
        }
    }

    void Shoot(InputAction.CallbackContext ctx)
    {
        LaunchProjectile();
        _actions.Player.Attack.Disable();
        _actions.Player.Pull.started += TryPullObject;
        _actions.Player.Pull.canceled += CancelPulling;
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag(projectilesTag) && other.gameObject == projectile)
    //         CatchProjectile(other);
    // }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag(projectilesTag))
            CatchProjectile(other);
    }

    private void LaunchProjectile()
    {

        _collider.enabled = false;
        isPullHeld = false;
        currentlyPulledCollider = null;
        if (pullDetectionCoroutine != null)
        {
            StopCoroutine(pullDetectionCoroutine);
            pullDetectionCoroutine = null;
        }

        Follower follower = projectile.GetComponent<Follower>();
        
        PositionProjectile();
        projectile.SetActive(true);
        projectile.GetComponent<Projectile>().Launch(launchForce);
        projectile = null;
        
        SwallowedObjectCell.Instance.ChangeSprite(null);
        
        RuntimeManager.PlayOneShot("event:/CANNON_SPIT", transform.position);
    }

    private void PositionProjectile()
    {
        projectile.transform.position = transform.position;
        
        projectile.transform.rotation *= Quaternion.FromToRotation(
            projectile.transform.up, transform.up);
    }
    
    private void StopSuckEffects()
    {
        suckParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        if (suckShakeCoroutine != null)
        {
            StopCoroutine(suckShakeCoroutine);
            suckShakeCoroutine = null;
        }
    }

    private void CatchProjectile(Collider2D other)
    {
        _collider.enabled = false;
        isPullHeld = false;
        currentlyPulledCollider = null;
        if (pullDetectionCoroutine != null)
        {
            StopCoroutine(pullDetectionCoroutine);
            pullDetectionCoroutine = null;
        }

        StopSuckEffects();
        
        GameObject proj = other.gameObject;
        proj.SetActive(false);
        proj.GetComponent<Follower>().enabled = false;
        
        _actions.Player.Pull.canceled -= CancelPulling;
        _actions.Player.Attack.Enable();

        if (proj.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            SwallowedObjectCell.Instance.ChangeSprite(spriteRenderer.sprite);
        }
        else
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            SwallowedObjectCell.Instance.ChangeSprite(spriteRenderer.sprite);
        }

        RuntimeManager.PlayOneShot("event:/CANNON_SWALLOW",  transform.position);
    }

    private void MoveObjectToCannon(RaycastHit2D hit)
    {
        _collider.enabled = true;
        
        if (hit.collider.gameObject.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
            enemy.StopMoving();
        
        Projectile proj = hit.collider.GetComponent<Projectile>();
        if (proj != null)
            proj.StartSwallowing();
        
        Follower follower = hit.collider.gameObject.GetComponent<Follower>();
        follower.target = gameObject;
        follower.enabled = true;
    }
}
