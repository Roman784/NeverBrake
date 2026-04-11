using Cysharp.Threading.Tasks;
using Gameplay;
using GameRoot;
using UnityEngine;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModeEntryPoint : SceneEntryPoint<ObstacleCourseModeEnterParams>
    {
        [SerializeField] private ObstacleCourseModeView _view;
        [SerializeField] private CameraTracker _camera;

        private ObstacleCourseModePresenter _presenter;

        protected override async UniTask Run(ObstacleCourseModeEnterParams enterParams)
        {
            // ========== Car ==========

            var prefab = G.RootCMS.CarsCMS.Prefab;
            var car = CarFactory.Create(prefab, Vector3.zero, Quaternion.identity);

            // ========== Camera ==========

            _camera.SetTraget(car.transform);

            // ========== MVP ==========

            var model = new ObstacleCourseModeModel(_view);
            _presenter = new ObstacleCourseModePresenter(model);
            _view.Init(_presenter);

            await UniTask.Yield();
        }
    }
}
