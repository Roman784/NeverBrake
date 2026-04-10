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

        [MenuItem("Scene/Main Menu")]
        public static void OpenMainMenu()
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/MainMenu.unity");
        }

        [MenuItem("Scene/Ability Menu")]
        public static void OPenAbilityMenu()
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/AbilityMenu.unity");
        }

        [MenuItem("Scene/Hero Menu")]
        public static void OpenHeroMenu()
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/HeroMenu.unity");
        }

        [MenuItem("Scene/Level Menu")]
        public static void OpenLevelMenu()
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/LevelMenu.unity");
        }

        [MenuItem("Scene/Combat")]
        public static void OpenCombat()
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/Combat.unity");
        }

        [MenuItem("Scene/Encounters Map")]
        public static void OpenEncountersMap()
        {
            EditorSceneManager.OpenScene("Assets/_Game/Scenes/EncountersMap.unity");
        }
    }
}
#endif