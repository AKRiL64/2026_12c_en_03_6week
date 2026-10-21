
using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] public CoinType type;

    private Animator anim;
    private Collider2D coinCollider;
    private bool collected = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coinCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        collected = true;
        coinCollider.enabled = false;

        CollectiblesEvents.CoinCollected(type);
        anim.SetTrigger("pickup");

        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return null;

        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("pickup"))
        {
            yield return null;
        }

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        Destroy(gameObject);
        
        RuntimeManager.PlayOneShot("event:/COINS", transform.position);
    }
}