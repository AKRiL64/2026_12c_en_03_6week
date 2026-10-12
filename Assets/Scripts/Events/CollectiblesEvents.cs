using System;
using UnityEngine;

public static class CollectiblesEvents
{
    public static event Action<int, int, bool> OnChestOpened; // silver, golden, diamond
    public static event Action<GameObject> OnCoinCollected;

    public static void ChestOpened(int coins, int goldenCoins, bool diamond)
    {
        OnChestOpened?.Invoke(coins, goldenCoins, diamond);
    }

    public static void CoinCollected(GameObject coin)
    {
        OnCoinCollected?.Invoke(coin);
    }
}
