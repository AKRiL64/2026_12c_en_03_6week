using System;
using UnityEngine;

public static class CollectiblesEvents
{
    public static event Action<int, int, bool> OnChestOpened; // silver, golden, diamond
    public static event Action<CoinType> OnCoinCollected;
    public static event Action<CoinType, int, int> OnCollectiblesChanged; 

    public static void ChestOpened(int silverCoins, int goldenCoins, bool diamond)
    {
        OnChestOpened?.Invoke(silverCoins, goldenCoins, diamond);
    }

    public static void CoinCollected(CoinType coin)
    {
        OnCoinCollected?.Invoke(coin);
    }

    public static void CollectiblesChanged(CoinType type, int previous, int current)
    {
        OnCollectiblesChanged?.Invoke(type, previous, current);
    }
}
