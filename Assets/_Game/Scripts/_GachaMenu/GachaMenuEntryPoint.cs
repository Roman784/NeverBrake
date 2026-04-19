using CMS;
using Cysharp.Threading.Tasks;
using GameRoot;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using R3;

namespace GachaMenu
{
    public class GachaMenuEntryPoint : SceneEntryPoint<GachaMenuEnterParams>
    {
        [SerializeField] private GachaMenuView _view;

        private GachaMenuPresenter _presenter;

        protected override async UniTask Run(GachaMenuEnterParams enterParams)
        {
            var pool = new GachaPoolData()
            {
                Items =
                    GetLockedCarsCMS()
                    .Select(carCMS => new GachaPoolItemData()
                    {
                        RewardId = carCMS.Id,
                        Rarity = carCMS.Rarity,
                        RewardPreview = carCMS.Preview
                    })
                    .ToArray()
            };
            pool.Shuffle();

            var model = new GachaMenuModel(pool);
            _presenter = new GachaMenuPresenter(_view, model);

            _presenter.RewardReceivedSignal
                .SubscribeAwait(async (id, ct) => await SaveNewUnlockedCar(id));
            _presenter.EquipReceivedRewardSignal
                .SubscribeAwait(async (id, ct) => await EquipCarAndExit(id));

            await UniTask.Yield();
        }

        private void OnDestroy()
        {
            _presenter?.Dispose();
        }

        private IEnumerable<CarCMS> GetLockedCarsCMS()
        {
            var allCarCMS = G.RootCMS.CarsCMS.AllCarsCMS;
            var unlockedIds = G.Repository.Cars.GetUnlockedCarIds().ToList();

            return allCarCMS
                .Where(c => !unlockedIds.Contains(c.Id));
        }

        private async UniTask SaveNewUnlockedCar(int id)
        {
            G.Repository.Cars.AddUnlockedCar(id);
            await G.Wallet.Save();
        }

        private async UniTask EquipCarAndExit(int id)
        {
            await G.Repository.Cars.SetSelectedCarId(id);
            G.SceneProvider.OpenPreviousScene();
        }
    }
}
