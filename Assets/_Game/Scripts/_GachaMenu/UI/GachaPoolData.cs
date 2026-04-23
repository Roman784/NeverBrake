using System.Linq;
using UnityEngine;
using Utils;

namespace GachaMenu
{
    public class GachaPoolData
    {
        public GachaPoolItemData[] Items;

        public void Shuffle()
        {
            Items = Items
                .OrderBy(x => Random.value)
                .ToArray();
        }
    }

    public class GachaPoolItemData
    {
        public int RewardId;
        public Rarity Rarity;
        public GameObject RewardPreviewPrefab;
    }
}
