using CustomizationMenu;
using GachaMenu;
using LevelMenu;
using ObstacleCourseMode;
using System;
using UI;
using UnityEngine;

namespace GameRoot
{
    public class SceneProvider
    {
        private readonly SceneLoader _sceneLoader;

        private Action _currentSceneOpenAction;

        private string _lastSceneName;
        private Action _lastSceneOpenAction;

        public SceneProvider(UIRoot uiRoot)
        {
            _sceneLoader = new SceneLoader(uiRoot);
        }

        public void OpenPreviousScene()
        {
            _lastSceneOpenAction?.Invoke();
        }

        public void OpenLevelMenu()
        {
            var enterParams = new LevelMenuEnterParams();
            OpenScene<LevelMenuEntryPoint, LevelMenuEnterParams>(enterParams);
        }

        public void OpenCustomizationMenu()
        {
            var enterParams = new CustomizationMenuEnterParams();
            OpenScene<CustomizationMenuEntryPoint, CustomizationMenuEnterParams>(enterParams);
        }

        public void OpenGachaMenu()
        {
            var enterParams = new GachaMenuEnterParams();
            OpenScene<GachaMenuEntryPoint, GachaMenuEnterParams>(enterParams);
        }

        public void OpenObstacleCourseMode(int number, int carId)
        {
            var enterParams = new ObstacleCourseModeEnterParams(number, carId);
            OpenScene<ObstacleCourseModeEntryPoint, ObstacleCourseModeEnterParams>(enterParams);
        }

        public void RestartScene()
        {
            _currentSceneOpenAction?.Invoke();
        }

        private void OpenScene<TEntryPoint, TEnterParams>(TEnterParams enterParams)
            where TEntryPoint : SceneEntryPoint
            where TEnterParams : SceneEnterParams
        {
            if (enterParams.SceneName != _lastSceneName)
            {
                _lastSceneName = enterParams.SceneName;
                _lastSceneOpenAction = _currentSceneOpenAction;
            }

            _currentSceneOpenAction = CreateSceneOpenAction<TEntryPoint, TEnterParams>(enterParams);
            _currentSceneOpenAction.Invoke();
        }

        private Action CreateSceneOpenAction<TEntryPoint, TEnterParams>(TEnterParams enterParams)
            where TEntryPoint : SceneEntryPoint
            where TEnterParams : SceneEnterParams
        {
            return () =>
            {
                _sceneLoader.LoadAndRunScene
                    <TEntryPoint, TEnterParams>(enterParams);
            };
        }
    }
}