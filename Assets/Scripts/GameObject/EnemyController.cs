using UnityEngine;
using UnityEngine.Windows;

public class EnemyController : MonoBehaviour
//TODO rename to Base EnemyController and create a derived class for each enemy type
{
    private enum EnemyState
    {
        Idle,
        Chasing,
        Attacking
    }
    private EnemyState state = EnemyState.Idle;
    private float detectionRange = 5f;
    private float speed = 2f;
    private Transform target;
    private Vector3 direction = Vector3.left;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (state == EnemyState.Idle) { 
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.name);
                if (hit.collider.CompareTag("Player"))
                {
                    target = hit.collider.transform;
                    state = EnemyState.Chasing;
                }
            }
            hit = Physics2D.Raycast(transform.position + direction * speed * Time.deltaTime, Vector2.down, 2f);
            if (hit.collider == null)
            {
                direction = new Vector3(-direction.x, direction.y, 0);
            }
            Debug.Log("Hit: " + hit.collider.name);

        }
        if (state == EnemyState.Chasing)
        {
            direction = (target.position - transform.position).normalized;
        }
        Move();
    }

    void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
