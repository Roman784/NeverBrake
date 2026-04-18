using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class RarityWeightMapper
    {
        private static List<Rarity> _rarities;

        static RarityWeightMapper()
        {
            _rarities =
                Enum.GetValues(typeof(Rarity))
                .Cast<Rarity>()
                .ToList();
        }

        public static int GetWeight(Rarity rarity)
        {
            return _rarities.IndexOf(rarity) + 1;
        }
    }
}
