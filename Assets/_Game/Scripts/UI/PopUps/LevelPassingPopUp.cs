using GameRoot;
using UnityEngine;

namespace UI
{
    public class LevelPassingPopUp : PopUp
    {
        public void Continue()
        {
            // TODO: open next level.
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) ||
                Input.GetKeyUp(KeyCode.UpArrow))
                Continue();

        }
    }
}
