using CustomizationMenu;
using LevelMenu;
using ObstacleCourseMode;
using System;
using UI;

namespace GameRoot
{
    public class SceneProvider
    {
        private readonly SceneLoader _sceneLoader;

        private SceneEnterParams _currentSceneParams;
        private Action _currentSceneOpenAction;

        public SceneProvider(UIRoot uiRoot)
        {
            _sceneLoader = new SceneLoader(uiRoot);
        }

        public void OpenLevelMenu()
        {
            var enterParams = new LevelMenuEnterParams();
            _currentSceneOpenAction = CreateSceneOpenAction<
                LevelMenuEntryPoint, LevelMenuEnterParams>(enterParams);
            _currentSceneOpenAction.Invoke();
        }

        public void OpenCustomizationMenu()
        {
            var enterParams = new CustomizationMenuEnterParams();
            _currentSceneOpenAction = CreateSceneOpenAction<
                CustomizationMenuEntryPoint, CustomizationMenuEnterParams>(enterParams);
            _currentSceneOpenAction.Invoke();
        }

        public void OpenObstacleCourseMode(int number, int carId)
        {
            var enterParams = new ObstacleCourseModeEnterParams(number, carId);
            _currentSceneOpenAction = CreateSceneOpenAction<
                ObstacleCourseModeEntryPoint, ObstacleCourseModeEnterParams>(enterParams);
            _currentSceneOpenAction.Invoke();
        }

        public void RestartScene()
        {
            _currentSceneOpenAction?.Invoke();
        }

        private Action CreateSceneOpenAction<TEntryPoint, TEnterParams>(TEnterParams enterParams)
            where TEntryPoint : SceneEntryPoint
            where TEnterParams : SceneEnterParams
        {
            return () =>
            {
                _currentSceneParams = enterParams;
                _sceneLoader.LoadAndRunScene
                    <TEntryPoint, TEnterParams>(enterParams);
            };
        }
    }
}