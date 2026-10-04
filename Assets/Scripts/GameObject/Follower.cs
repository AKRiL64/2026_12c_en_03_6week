using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] public GameObject target;
    [SerializeField] public float speed = 1f;
    private Rigidbody2D _rb;
    
    void Awake()
    {
        if(_rb == null)
            _rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        _rb.linearVelocity = GetDirection() * speed;
        //_rb.AddForce(GetDirection() * pullingForce);
    }

    private Vector3 GetDirection()
    {
        if (target == null)
            return Vector3.zero;
        
        return (target.transform.position - transform.position).normalized;
    }
}
