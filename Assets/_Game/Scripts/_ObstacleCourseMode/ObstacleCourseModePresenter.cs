using Cysharp.Threading.Tasks;
using GameRoot;
using R3;
using System;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModePresenter : IDisposable
    {
        private ObstacleCourseModeView _view;
        private ObstacleCourseModeModel _model;

        private CompositeDisposable _disposables = new();
        private IDisposable _timerDisposable;

        public ObstacleCourseModePresenter(
            ObstacleCourseModeView view,
            ObstacleCourseModeModel model)
        {
            _view = view;
            _model = model;

            SetupSubscriptions();
            DisplayUI();
        }

        public void Dispose() => _disposables.Dispose();

        private void SetupSubscriptions()
        {
            _model.Car.FinishReachedSignal
                .SubscribeAwait(async (_, _) => await HandleLevelPassing())
                .AddTo(_disposables);

            _model.Car.FailedSignal
                .SubscribeAwait(async (_, _) => await HandleLevelFailure())
                .AddTo(_disposables);
        }

        // ================ Level Start ================

        public void HandleLevelStart()
        {
            Observable.EveryUpdate()
                .Where(_ => _model.Car.Input.ShouldStartMoving())
                .Take(1)
                .SubscribeAwait(async (_, _) =>
                {
                    await UniTask.Yield();

                    StartTimer();
                    _model.Car.StartMovement();
                })
                .AddTo(_disposables);
        }

        private void StartTimer()
        {
            _model.StartTimer();
            _timerDisposable = Observable.EveryUpdate()
                .Subscribe(_ => _view.DisplayCurrentTime(_model.GetCurrentTime()))
                .AddTo(_disposables);
        }

        private void StopTimer()
        {
            _timerDisposable?.Dispose();
        }

        // ================ Level Outcome ================

        private async UniTask HandleLevelPassing()
        {
            StopTimer();

            if (!_model.IsLevelPassed || _model.GetCurrentTime() < _model.BestTime)
                await _model.SaveNewBestTime(_model.GetCurrentTime());

            await _model.PassLevel();

            DisplayUI();

            await UniTask.Delay(2000);

            G.PopUpsProvider.OpenLevelPassingPopUp();
            await OpenLevelPassingToasts();
        }

        private async UniTask HandleLevelFailure()
        {
            StopTimer();

            await _model.IncreaseDeathCount();
            DisplayUI();

            await UniTask.Delay(1000);

            G.PopUpsProvider.OpenLevelFailurePopUp();
        }

        private async UniTask OpenLevelPassingToasts()
        {
            var toastsProvider = G.ToastsProvider;
            toastsProvider.PrepareTotalCoinsToast();
            toastsProvider.PrepareCoinsReceivedToast(10);
            toastsProvider.PrepareCoinsForAdToast();
            toastsProvider.PrepareGiftToast();
            toastsProvider.PreparePrizeToast();

            await toastsProvider.Open();
        }

        // ================ UI ================

        private void DisplayUI()
        {
            _view.DisplayDeathCount(_model.DeathCount);
            _view.DisplayBestTime(_model.BestTime);
            _view.DisplayCurrentTime(_model.GetCurrentTime());

        }
    }
}
