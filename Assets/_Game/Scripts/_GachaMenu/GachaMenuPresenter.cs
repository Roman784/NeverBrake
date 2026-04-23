using Cysharp.Threading.Tasks;
using GameRoot;
using R3;
using System;
using System.Linq;

namespace GachaMenu
{
    public class GachaMenuPresenter : IDisposable
    {
        private GachaMenuView _view;
        private GachaMenuModel _model;

        private CompositeDisposable _disposables = new();

        private Subject<int> _rewardReceivedSignalSubj = new();
        private Subject<int> _equipReceivedRewardSignalSubj = new();
        public Observable<int> RewardReceivedSignal => _rewardReceivedSignalSubj;
        public Observable<int> EquipReceivedRewardSignal => _equipReceivedRewardSignalSubj;

        public GachaMenuPresenter(
            GachaMenuView view,
            GachaMenuModel model)
        {
            _view = view;
            _model = model;

            _view.BindWallet(_model.Wallet);
            _view.SetSpinCost(_model.SpinCost);

            if (_model.Pool.Items.Length == 0)
                _view.ActiveNoMoreCapsulesLeftMode();
            else
                CreateCapsules();

            SetupSubscriptions();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void SetupSubscriptions()
        {
            _view.SpinButtonPressedSignal
                .Subscribe(_ => TrySpin().Forget())
                .AddTo(_disposables);

            _view.EquipButtonPressedSignal
                .Subscribe(_ => EquipReceivedReward())
                .AddTo(_disposables);

            _view.CustomizationButtonPressedSignal
                .Subscribe(_ => OpenCustomizationMenu())
                .AddTo(_disposables);

            _view.ExitButtonPressedSignal
                .Subscribe(_ => ExitFromMenu())
                .AddTo(_disposables);
        }

        private void CreateCapsules()
        {
            foreach (var item in _model.Pool.Items)
            {
                var createdCapsule = _view.CreateCapsule(item.Rarity, item.RewardPreviewPrefab);
                _model.AddCapsule(createdCapsule, item.RewardId);
            }
            _view.ClearContainerAndLayOutCapsules(_model.GetCapsules());
        }

        private async UniTask TrySpin()
        {
            var randomRwardId = _model.GetRandomRewardId();
            var targetCapsule = _model.GetCapsule(randomRwardId);

            if (targetCapsule == null) return;

            var trySpendCoinsResult = await _model.Wallet.TrySpendCoins(_model.SpinCost, false);
            if (!trySpendCoinsResult) return;

            _view.Spin(targetCapsule, _model.GetCapsules())
                .Subscribe(_ =>
                {
                    _view.HideCapsulesWithout(targetCapsule, _model.GetCapsules());
                    _view.HighlightCapsule(targetCapsule);

                    targetCapsule.PressedSignal
                        .Subscribe(_ => targetCapsule.Open())
                        .AddTo(_disposables);

                    targetCapsule.OpenedSignal
                        .Subscribe(_ => ReceiveReward(randomRwardId))
                        .AddTo(_disposables);
                })
                .AddTo(_disposables);
        }

        private void ReceiveReward(int rewardId)
        {
            _model.ReceivedRewardId = rewardId;

            _rewardReceivedSignalSubj.OnNext(rewardId);
            _rewardReceivedSignalSubj.OnCompleted();
        }

        private void EquipReceivedReward()
        {
            var recievedReward = _model.ReceivedRewardId;

            _equipReceivedRewardSignalSubj.OnNext(recievedReward);
            _equipReceivedRewardSignalSubj.OnCompleted();
        }

        private void OpenCustomizationMenu()
        {
            G.SceneProvider.OpenCustomizationMenu();
        }

        private void ExitFromMenu()
        {
            G.SceneProvider.OpenPreviousScene();
        }
    }
}
