using System.Linq;
using UnityEngine;
using Utils;

namespace UI
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
        public int ItemId;
        public Rarity Rarity;
        public Sprite Reward;
    }
}
