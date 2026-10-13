using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int score = 0;
    private readonly Dictionary<CoinType, int> _collectiblesCount = new Dictionary<CoinType, int>()
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

    private void UpdateCollectibles(CoinType type, int delta)
    {
        if (delta == 0)
            return;
        CollectiblesEvents.CollectiblesChanged(type, _collectiblesCount[type], delta);
        _collectiblesCount[type] += delta;
        
        if(type == CoinType.Coin) // delete once the UI is ready
            coinCounter += delta;
        else if(type == CoinType.Diamond)
            diamondCounter += delta;
    }
    
}
