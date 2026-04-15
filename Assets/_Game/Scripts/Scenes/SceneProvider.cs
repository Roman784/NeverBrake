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

        public void OpenObstacleCourseMode()
        {
            var enterParams = new ObstacleCourseModeEnterParams();
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