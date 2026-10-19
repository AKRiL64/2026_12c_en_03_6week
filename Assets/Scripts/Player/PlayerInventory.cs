using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int score = 0;
    private readonly Dictionary<CoinType, int> _collectiblesCount = new Dictionary<CoinType, int>()
    {
        { CoinType.Coin, 0 }, { CoinType.Diamond, 0 }
    };

    [SerializeField] private CoinBar coinBar;
    [SerializeField] private DiamondCounter diamondCounter;

    void Awake()
    {
        coinBar = CoinBar.Instance;
        diamondCounter = DiamondCounter.Instance;
    }
    
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


        switch (type)
        {
            case CoinType.Coin:
                coinBar.UpdateBar(_collectiblesCount[type]);
                break;
            case CoinType.Diamond:
                diamondCounter.SetCounter(_collectiblesCount[type]);
                break;
        }
    }
    
}
