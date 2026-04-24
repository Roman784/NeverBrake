using Cysharp.Threading.Tasks;
using Gameplay;
using GameRoot;
using UnityEngine;
using R3;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModeEntryPoint : SceneEntryPoint<ObstacleCourseModeEnterParams>
    {
        [SerializeField] private ObstacleCourseModeView _view;

        private ObstacleCourseModePresenter _presenter;
        private bool _isStarted;

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

            var model = new ObstacleCourseModeModel(
                enterParams: enterParams,
                car: car);
            _presenter = new ObstacleCourseModePresenter(_view, model);

            // ========== Start Lvel ==========

            _presenter.HandleLevelStart();

            await UniTask.Yield();
        }

        private void OnDestroy()
        {
            _presenter?.Dispose();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
                G.ToastsProvider
                    .PrepareGetPrizeToast()
                    .PrepareGetPrizeToast()
                    .PrepareGetPrizeToast()
                    .PrepareGetPrizeToast()
                    .PrepareGetPrizeToast()
                    .Open().Forget();
        }
    }
}
