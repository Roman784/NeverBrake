using GameRoot;

namespace UI
{
    public class LevelFailurePopUp : PopUp
    {
        public void RestartLevel()
        {
            G.SceneProvider.RestartScene();
        }

        public void OpenLevelMenu()
        {
            G.SceneProvider.OpenLevelMenu();
        }
    }
}
