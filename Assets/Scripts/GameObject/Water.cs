using FMODUnity;
using UnityEngine;

public class Water: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RuntimeManager.PlayOneShot("event:/WATER", transform.position);
        
        if (! (collision.CompareTag("Player") || collision.CompareTag("Pullable"))) return;

        if (collision.tag == "Player")
        {
            player.TakeDamage(1);
            player.SpawnOnTheFloor();

            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
                player.SpawnOnTheFloor();
            }
        }

        if (collision.tag == "Pullable")
        {
            GameObject pullable = collision.gameObject;
            Destroy(pullable);
        }

    }
}
