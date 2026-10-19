using UnityEngine;

public class GetCannon: MonoBehaviour
{
    [SerializeField] private GameObject cannon;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        GameObject newCannon = Instantiate(cannon, collision.gameObject.transform);
        newCannon.transform.parent = collision.transform;
        newCannon.transform.localPosition = Vector3.zero;
        Destroy(this.gameObject);
    }
}
