using System.Collections.Generic;
using UnityEngine;

public static class CoinValues
{
    public static Dictionary<CoinType, int> Coins = new Dictionary<CoinType, int>()
    {
        {CoinType.Golden, 5}, {CoinType.Silver, 2}, { CoinType.Diamond, 1 }
    };
}
