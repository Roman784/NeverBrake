using GameRoot;

namespace UI
{
    public class LevelPassingPopUp : PopUp
    {
        public void Continue()
        {
            // TODO: open next level.
            Close();
        }

        public void Restart()
        {
            G.SceneProvider.RestartScene();
        }

        public void OpenLevelMenu()
        {
            G.SceneProvider.OpenLevelMenu();
        }
    }
}
