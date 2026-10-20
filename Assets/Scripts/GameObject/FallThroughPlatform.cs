using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FallThroughPlatform : MonoBehaviour
{
    private Collider2D c;
    private PlayerController player;
    private int playerLayerMask;
    private bool isFallingThrough;

    private void Awake()
    {
        c = GetComponent<Collider2D>();
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        if (!player 
            || !player.HasDownInput 
            || isFallingThrough)
            return;

        StartCoroutine(FallThrough());
    }

    private IEnumerator FallThrough()
    {
        isFallingThrough = true;
        c.excludeLayers |= playerLayerMask;

        yield return new WaitForSeconds(0.5f);

        c.excludeLayers &= ~playerLayerMask;
        isFallingThrough = false;
    }

    private void TrySetPlayerOnPlatform(Collider2D other, bool value)
    {
        PlayerController p = other.GetComponentInParent<PlayerController>();
        if (!p) return;

        if (value)
            player = p;
        else if (player == p)
            player = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TrySetPlayerOnPlatform(collision.collider, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        TrySetPlayerOnPlatform(collision.collider, false);
    }
}
