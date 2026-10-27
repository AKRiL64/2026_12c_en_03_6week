
using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] public CoinType type;
    [SerializeField] private GameObject pickupEffect;

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
        Instantiate(pickupEffect, transform.position, Quaternion.identity);
        RuntimeManager.PlayOneShot("event:/COINS", transform.position);
        Destroy(gameObject);
    }
}