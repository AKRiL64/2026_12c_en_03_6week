using UnityEngine;

public class GetCannon: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
       PlayerController player = collision.gameObject.GetComponent<PlayerController>();
       player.EnableCannon();
       Destroy(this.gameObject);
    }
}
