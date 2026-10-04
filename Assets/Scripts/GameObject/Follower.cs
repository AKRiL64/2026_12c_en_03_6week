using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] public GameObject target;
    [SerializeField] public float speed = 1f;
    private Rigidbody2D _rb;

    [SerializeField] private float rotationSpeed = 1f;
    private float _timeCount = 0;
    
    void Awake()
    {
        if(_rb == null)
            _rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        _timeCount = 0;
    }
    
    void FixedUpdate()
    {
        _rb.linearVelocity = GetDirection() * speed;
        
        RotateTowardsTarget();
        _timeCount += Time.deltaTime;
    }

    private void RotateTowardsTarget()
    {
        Quaternion targetDirection = Quaternion.FromToRotation(transform.up, target.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * targetDirection, _timeCount * rotationSpeed);
        //Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.red);
    }

    private Vector3 GetDirection()
    {
        if (target == null)
            return Vector3.zero;
        
        return (target.transform.position - transform.position).normalized;
    }
}
