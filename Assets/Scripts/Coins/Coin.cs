using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] public CoinType type;
    private Animator anim;
    private Collider2D collider;
    private bool collected = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        collected = true;

        CollectiblesEvents.CoinCollected(type);
        anim.SetTrigger("pickup");
        collider.enabled = false;
        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject);
    }
}