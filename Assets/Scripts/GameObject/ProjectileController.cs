using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);
    }
}
