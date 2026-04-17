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

            // ========== Car ==========

            var prefab = G.RootCMS.CarsCMS.Prefab;
            var car = CarFactory.Create(prefab, level.CarSpawnPosition, level.CarSpawnRotation);

            // ========== Camera ==========

            var camera = level.Camera;
            G.Camera = camera;

            camera.Tracker.SetTraget(car.transform);

            // ========== MVP ==========

            var model = new ObstacleCourseModeModel(_view);
            _presenter = new ObstacleCourseModePresenter(model);
            _view.Init(_presenter);

            await UniTask.Yield();
        }
    }
}
