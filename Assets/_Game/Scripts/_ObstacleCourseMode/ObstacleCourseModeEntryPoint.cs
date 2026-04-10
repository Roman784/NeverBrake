using Cysharp.Threading.Tasks;
using Gameplay;
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
            G.CarInputFactory = new CarInputFactory();

            // ========== Car ==========



            // ========== MVP ==========

            var model = new ObstacleCourseModeModel(_view);
            _presenter = new ObstacleCourseModePresenter(model);
            _view.Init(_presenter);

            await UniTask.Yield();
        }
    }
}
