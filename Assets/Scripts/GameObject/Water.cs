using FMODUnity;
using UnityEngine;

public class Water: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (! (collision.CompareTag("Player") || collision.CompareTag("Pullable"))) return;

        if (collision.tag == "Player")
        {
            RuntimeManager.PlayOneShot("event:/WATER", collision.transform.position);
            
            PlayerController player = collision.GetComponent<PlayerController>();
            
            if (player != null)
            {
                player.TakeDamage(1);
                player.SpawnOnTheFloor();

                if (player)
                {
                    
                }
            }
        }

        if (collision.tag == "Pullable")
        {
            RuntimeManager.PlayOneShot("event:/WATER", collision.transform.position);
            
            GameObject pullable = collision.gameObject;
            Destroy(pullable);
        }

    }
}
