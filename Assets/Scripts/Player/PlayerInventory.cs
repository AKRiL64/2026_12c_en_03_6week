using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int Score { get; private set; } = 0;
    public readonly Dictionary<CoinType, int> CollectiblesCount = new Dictionary<CoinType, int>()
    {
        { CoinType.Coin, 0 }, { CoinType.Diamond, 0 }
    };

    [SerializeField] private CoinBar coinBar;
    [SerializeField] private DiamondCounter diamondCounter;

    void Start()
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

    void OnDestroy()
    {
        StatusPersister.SaveScore(Score);
        StatusPersister.SaveCollectibles(CollectiblesCount);
    }

    private void CollectCoin(CoinType type)
    {
        int delta = CoinValues.Coins.GetValueOrDefault(type, 0);
        Score += CoinValues.ScoreValue[type];
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

        switch (type)
        {
            case CoinType.Coin:
                coinBar.UpdateBar(CollectiblesCount[type]);
                break;
            case CoinType.Diamond:
                diamondCounter.SetCounter(CollectiblesCount[type]);
                break;
        }
    }

    public void UpdateCoinbar()
    {
        coinBar = CoinBar.Instance;
    }

    private void GetCollectibles()
    {
        Score = StatusPersister.Score;
        CollectiblesCount[CoinType.Coin] = StatusPersister.CollectiblesCount[CoinType.Coin];
        CollectiblesCount[CoinType.Diamond] = StatusPersister.CollectiblesCount[CoinType.Diamond];
        
        Debug.Log($"{Score} - {CollectiblesCount[CoinType.Coin]} -  {CollectiblesCount[CoinType.Diamond]}");
    }
}
