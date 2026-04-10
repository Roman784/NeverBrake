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
    }
}