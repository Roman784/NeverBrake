using GameRoot;

namespace UI
{
    public class PausePopUp : PopUp
    {
        public void Continue()
        {
            Close();
        }

        public void RestartLevel()
        {
            G.SceneProvider.RestartScene();
        }

        public void OpenLevelMenu()
        {
            G.SceneProvider.OpenLevelMenu();
        }

        public void OpenSettings()
        {
            // TODO
        }
    }
}
