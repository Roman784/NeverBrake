using ObstacleCourseMode;
using UI;

namespace GameRoot
{
    public class SceneProvider
    {
        private readonly SceneLoader _sceneLoader;

        private SceneEnterParams _currentSceneParams;
        private SceneEnterParams _previousSceneParams;

        public string CurrentSceneName => _currentSceneParams?.SceneName ?? "";

        public SceneProvider(UIRoot uiRoot)
        {
            _sceneLoader = new SceneLoader(uiRoot);
        }

        public void OpenObstacleCourseMode()
        {
            var enterParams = new ObstacleCourseModeEnterParams();
            OpenScene<ObstacleCourseModeEntryPoint, ObstacleCourseModeEnterParams>(enterParams);
        }

        private void OpenScene<TEntryPoint, TEnterParams>(TEnterParams enterParams)
            where TEntryPoint : SceneEntryPoint
            where TEnterParams : SceneEnterParams
        {
            _previousSceneParams = _currentSceneParams;
            _currentSceneParams = enterParams;

            _sceneLoader.LoadAndRunScene
                <TEntryPoint, TEnterParams>(enterParams);
        }
    }
}