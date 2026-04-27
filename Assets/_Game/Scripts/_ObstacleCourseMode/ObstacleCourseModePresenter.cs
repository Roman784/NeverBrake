using Cysharp.Threading.Tasks;
using GameRoot;
using Pause;
using R3;
using System;
using UI;
using UnityEngine;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModePresenter : IDisposable, IPaused
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

            _view.Enable();

            G.PauseProvider.Register(this);
        }

        public void Dispose()
        {
            _disposables.Dispose();
            G.PauseProvider.Remove(this);
        }

        private void SetupSubscriptions()
        {
            _view.PauseButtonPressedSignal
                .Subscribe(_ => HandlePause())
                .AddTo(_disposables);

            _model.Car.FinishReachedSignal
                .SubscribeAwait(async (_, _) => await HandleLevelPassing())
                .AddTo(_disposables);

            _model.Car.FailedSignal
                .SubscribeAwait(async (_, _) => await HandleLevelFailure())
                .AddTo(_disposables);

            _model.Car.CollisionRegister.CollidedWithWallSignal
                .ThrottleFirst(TimeSpan.FromSeconds(0.5f))
                .Subscribe(_ => HandleTimerPenalty())
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
            _view.Disable();
            StopTimer();

            var coinsForLevelPassing = 0;
            if (!_model.IsLevelPassed)
            {
                coinsForLevelPassing = _model.GetCoinsForLevelPassing();
                await _model.Wallet.AddCoins(coinsForLevelPassing);
            }

            if (!_model.IsLevelPassed || _model.GetCurrentTime() < _model.BestTime)
                await _model.SaveNewBestTime(_model.GetCurrentTime());

            await _model.PassLevel();

            DisplayUI();

            await UniTask.Delay(2000);

            G.PopUpsProvider.OpenLevelPassingPopUp();
            await OpenLevelPassingToasts(coinsForLevelPassing);
        }

        private async UniTask HandleLevelFailure()
        {
            _view.Disable();
            StopTimer();

            await _model.IncreaseDeathCount();
            DisplayUI();

            await UniTask.Delay(1000);

            G.PopUpsProvider.OpenLevelFailurePopUp();
        }

        private async UniTask OpenLevelPassingToasts(int coinsForLevelPassing)
        {
            var toastsProvider = G.ToastsProvider;
            toastsProvider.PrepareTotalCoinsToast();
            if (coinsForLevelPassing > 0)
                toastsProvider.PrepareCoinsReceivedToast(coinsForLevelPassing);
            toastsProvider.PrepareCoinsForAdToast();
            toastsProvider.PrepareGiftToast();
            toastsProvider.PreparePrizeToast();

            await toastsProvider.Open();
        }

        // ================ Timer Penalty ================

        private void HandleTimerPenalty()
        {
            var penaltyValue = _model.TimerCollisionPenalty;
            _model.IncreaseTotalTimerPenalty(penaltyValue);
            _view.DisplayTimerPenalty(penaltyValue);
        }

        // ================ Pause ================

        private void HandlePause()
        {
            G.PopUpsProvider.OpenPausePopUp()
                .CloseSignal
                .Subscribe(_ => G.PauseProvider.Unpause())
                .AddTo(_disposables);
            G.PauseProvider.Pause();
        }

        public void Pause()
        {
            _view.Disable();
            _model.PauseTimer();
        }

        public void Unpause()
        {
            _view.Enable();
            _model.UnpauseTimer();
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
