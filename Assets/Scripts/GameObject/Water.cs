using FMODUnity;
using UnityEngine;

public class Water: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(1);
            player.SpawnOnTheFloor();
            RuntimeManager.PlayOneShot("event:/WATER", transform.position);
        }
    }
}
