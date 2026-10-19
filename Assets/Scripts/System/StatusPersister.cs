using System.Collections.Generic;
using UnityEngine;

public static class StatusPersister
{
    public static int Score { get; private set; } = 0;
    public static Dictionary<CoinType, int> CollectiblesCount { get; private set; } = new Dictionary<CoinType, int>()
    {
        { CoinType.Coin, 0},
        { CoinType.Diamond, 0}
    };

    public static void SaveScore(int score)
    {
        Score = score;
    }

    public static void SaveCollectibles(Dictionary<CoinType, int> collectibles)
    {
        foreach (var pair in collectibles)
            CollectiblesCount[pair.Key] = pair.Value;
    }
    
    public static void SaveCollectibles(int coins, int diamonds)
    {
        CollectiblesCount[CoinType.Coin] = coins;
        CollectiblesCount[CoinType.Diamond] = diamonds;
    }
}
