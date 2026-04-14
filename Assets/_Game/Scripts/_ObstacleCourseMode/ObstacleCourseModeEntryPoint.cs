using Cysharp.Threading.Tasks;
using Gameplay;
using GameRoot;
using UnityEngine;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModeEntryPoint : SceneEntryPoint<ObstacleCourseModeEnterParams>
    {
        [SerializeField] private ObstacleCourseModeView _view;
        [SerializeField] private Gameplay.Camera _camera;
        [SerializeField] private Transform _carSpawnPoint;

        private ObstacleCourseModePresenter _presenter;

        protected override async UniTask Run(ObstacleCourseModeEnterParams enterParams)
        {
            G.Camera = _camera;

            // ========== Car ==========

            var prefab = G.RootCMS.CarsCMS.Prefab;
            var car = CarFactory.Create(prefab, _carSpawnPoint.position, _carSpawnPoint.rotation);

            // ========== Camera ==========

            _camera.Tracker.SetTraget(car.transform);

            // ========== MVP ==========

            var model = new ObstacleCourseModeModel(_view);
            _presenter = new ObstacleCourseModePresenter(model);
            _view.Init(_presenter);

            await UniTask.Yield();
        }
    }
}
