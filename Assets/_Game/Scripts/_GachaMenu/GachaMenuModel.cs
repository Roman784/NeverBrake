using CMS;
using Currency;
using GameRoot;
using GameState;
using System.Collections.Generic;
using System.Linq;
using UI;
using Utils;

namespace GachaMenu
{
    public class GachaMenuModel
    {
        public Wallet Wallet { get; private set; }
        public GachaPoolData Pool { get; private set; }
        public int SpinCost { get; private set; }
        public int ReceivedRewardId { get; set; }

        private Dictionary<int, GachaCapsule> _capsulesByRewardIdMap = new();

        public GachaMenuModel(GachaPoolData pool)
        {
            Wallet = G.Wallet;
            SpinCost = G.RootCMS.GachaCMS.SpinCost;

            Pool = pool;
            ReceivedRewardId = -1;
        }

        public IEnumerable<GachaCapsule> GetCapsules() => _capsulesByRewardIdMap.Values;
        public void AddCapsule(GachaCapsule capsule, int rewardId) => _capsulesByRewardIdMap[rewardId] = capsule;

        public int GetRandomRewardId()
        {
            var itemIdByWeights = Pool.Items
                    .Select(i => (i.RewardId, RarityWeightMapper.GetWeight(i.Rarity)))
                    .ToArray();
            return WeightedRandom.Get(itemIdByWeights);
        }

        public GachaCapsule GetCapsule(int rewardId)
        {
            if (_capsulesByRewardIdMap.TryGetValue(rewardId, out var capsule)) 
                return capsule;
            return null;
        }
    }
}
