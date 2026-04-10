using Cysharp.Threading.Tasks;
using GameRoot;
using UnityEngine;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModeEntryPoint : SceneEntryPoint<ObstacleCourseModeEnterParams>
    {
        [SerializeField] private ObstacleCourseModeView _view;

        private ObstacleCourseModePresenter _presenter;

        protected override async UniTask Run(ObstacleCourseModeEnterParams enterParams)
        {
            // ========== MVP ==========
            var model = new ObstacleCourseModeModel();
            _presenter = new ObstacleCourseModePresenter(_view, model);

            await UniTask.Yield();
        }

        private void OnDestroy()
        {
            _presenter?.Dispose();
        }
    }
}
