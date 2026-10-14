using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int Score { get; private set; } = 0;
    public readonly Dictionary<CoinType, int> CollectiblesCount = new Dictionary<CoinType, int>()
    {
        { CoinType.Coin, 0 }, { CoinType.Diamond, 0 }
    };

    [SerializeField] private int coinCounter = 0; // delete once the UI is ready
    [SerializeField] private int diamondCounter = 0;

    void OnEnable()
    {
        CollectiblesEvents.OnCoinCollected += CollectCoin;
        CollectiblesEvents.OnChestOpened += CollectChest;
    }

    void OnDisable()
    {
        CollectiblesEvents.OnCoinCollected -= CollectCoin;
        CollectiblesEvents.OnChestOpened -= CollectChest;
    }

    private void CollectCoin(CoinType type)
    {
        int delta = CoinValues.Coins.GetValueOrDefault(type, 0);
        type = type == CoinType.Diamond ? CoinType.Diamond : CoinType.Coin;
        UpdateCollectibles(type, delta);
    }

    private void CollectChest(int silverCoins, int goldenCoins, bool diamond)
    {
        int delta = silverCoins * CoinValues.Coins[CoinType.Silver] + goldenCoins * CoinValues.Coins[CoinType.Golden];
        UpdateCollectibles(CoinType.Coin, delta);
        UpdateCollectibles(CoinType.Diamond, diamond ? 1 : 0);
    }

    public void UpdateCollectibles(CoinType type, int delta)
    {
        if (delta == 0)
            return;
        CollectiblesEvents.CollectiblesChanged(type, CollectiblesCount[type], delta);
        CollectiblesCount[type] += delta;
        
        if(type == CoinType.Coin) // delete once the UI is ready
            coinCounter += delta;
        else if(type == CoinType.Diamond)
            diamondCounter += delta;
    }

    public void UpdateScore(int newScore)
    {
        Score = newScore;
    }
}
