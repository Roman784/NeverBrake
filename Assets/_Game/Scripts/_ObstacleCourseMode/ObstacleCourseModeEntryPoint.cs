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
            await UniTask.Yield();

            // ========== Level ==========

            var levelNumber = enterParams.LevelNumber;
            if (!G.RootCMS.LevelsCMS.IsLevelExist(levelNumber))
            {
                G.SceneProvider.OpenLevelMenu();
                return;
            }

            var levelCMS = G.RootCMS.LevelsCMS.GetLevelCMS(levelNumber);
            var levelPrefab = levelCMS.LevelPrefab;
            var level = Instantiate(levelPrefab);

            await UniTask.Yield();

            // ========== Car ==========

            var carId = enterParams.CarId;
            if (!G.RootCMS.CarsCMS.IsCarExist(carId))
            {
                G.SceneProvider.OpenLevelMenu();
                return;
            }

            var prefab = G.RootCMS.CarsCMS.GetCarCMS(carId).Prefab;
            var car = CarFactory.Create(prefab, level.CarSpawnPosition, level.CarSpawnRotation);

            // ========== Camera ==========

            var camera = level.Camera;
            G.Camera = camera;

            camera.Tracker.SetTarget(car.transform);

            // ========== MVP ==========

            var model = new ObstacleCourseModeModel(_view);
            _presenter = new ObstacleCourseModePresenter(model);
            _view.Init(_presenter);

            await UniTask.Yield();
        }
    }
}
