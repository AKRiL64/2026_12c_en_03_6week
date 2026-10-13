using UnityEngine;

public class Gunpowder : Projectile
{
    [SerializeField] private float explosionDistance = 1.5f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float explosionPower = 1.5f;
    [SerializeField] private float upwardsModifier = 1.1f;
    [SerializeField] private int maxCollidersCount = 32;
    [SerializeField] private LayerMask affectedLayers;
    private ContactFilter2D _contactFilter;

    private LayerMask _raycastMask;
    
    private Vector2 _spawnPosition;
    private Collider2D[] _colliders;

    protected override void Awake()
    {
        base.Awake();
        _spawnPosition = transform.position;
        
        _colliders = new Collider2D[maxCollidersCount];
        _contactFilter = new ContactFilter2D();
        _contactFilter.useLayerMask = true;
        _contactFilter.layerMask = affectedLayers;
        
        _raycastMask = ~LayerMask.GetMask("Player", "Launched Projectile", "Cannon");
    }
    
    public override void Launch(float launchForce)
    {
        gameObject.layer = launchedProjectileLayer;
        Explode();
        transform.position = _spawnPosition;
    }

    private void Explode()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, GetDirection(), explosionDistance, _raycastMask);
        Vector2 calculatedEndpoint = (Vector2)transform.position + GetDirection() * explosionDistance;
        
        Vector2 explosionPosition = hit.collider == null ? calculatedEndpoint : hit.transform.position;
        
        int collidersCount = Physics2D.OverlapCircle(explosionPosition, explosionRadius, _contactFilter, _colliders);
        for (int i = 0; i < collidersCount; i++)
        {
            if (_colliders[i].TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                Debug.DrawLine(explosionPosition, _colliders[i].transform.position, Color.red, 1.5f);
                Vector2 direction = (_colliders[i].transform.position - transform.position).normalized; 
                direction *= explosionPower;
                direction.y *= -upwardsModifier; // it's 04:08 idk why do you need to multiply by negative it shouldn't but it works this way 
                rb.AddForce(direction, ForceMode2D.Impulse);
            }
        }
    }
    
    public override void OnCollisionEnter2D(Collision2D other)
    {
        Stop();
    }
}
