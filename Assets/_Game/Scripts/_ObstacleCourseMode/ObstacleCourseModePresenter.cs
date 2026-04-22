using Cysharp.Threading.Tasks;
using Gameplay;
using GameRoot;
using R3;
using System;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModePresenter : IDisposable
    {
        private ObstacleCourseModeView _view;
        private ObstacleCourseModeModel _model;

        private CompositeDisposable _disposables = new();

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
            _model.Car.FailedSignal
                .SubscribeAwait(async (_, _) => await HandleLevelFailed())
                .AddTo(_disposables);
        }

        // ================ Level Outcome ================

        private void HandleLevelPassing()
        {

        }

        private async UniTask HandleLevelFailed()
        {
            await _model.IncreaseDeathCount();
            DisplayUI();

            await UniTask.Delay(2000);

            _model.SceneProvider.RestartScene();
        }

        // ================ UI ================

        private void DisplayUI()
        {
            _view.DisplayDeathCount(_model.DeathCount);

        }
    }
}
