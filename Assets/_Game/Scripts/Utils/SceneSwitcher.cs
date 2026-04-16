#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;

namespace Utils
{
    public class SceneSwitcher
    {
        [MenuItem("Scene/Boot")]
        public static void OpenBoot()
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/Boot.unity");
        }

        [MenuItem("Scene/Level Menu")]
        public static void OpenMainMenu()
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/LevelMenu.unity");
        }

        [MenuItem("Scene/Obstacle Course Mode")]
        public static void OpenObstacleCourseMode()
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/ObstacleCourseMode.unity");
        }
    }
}
#endif