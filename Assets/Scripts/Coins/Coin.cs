using System;
using FMODUnity;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] public CoinType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CollectiblesEvents.CoinCollected(type);
        Destroy(gameObject);
        
        RuntimeManager.PlayOneShot("event:/COINS", transform.position);
    }
}
