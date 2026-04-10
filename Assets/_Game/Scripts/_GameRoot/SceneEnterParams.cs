namespace GameRoot
{
    public class SceneEnterParams
    {
        public readonly string SceneName;

        public SceneEnterParams(string sceneName)
        {
            SceneName = sceneName;
        }

        public T As<T>() where T : SceneEnterParams
        {
            return (T)this;
        }

        public bool TryCast<T>(out T enterParams) where T : SceneEnterParams
        {
            if (this is T)
            {
                enterParams = (T)this;
                return true;
            }

            enterParams = null;
            return false;
        }
    }
}
