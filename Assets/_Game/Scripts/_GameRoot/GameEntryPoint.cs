using Audio;
using CMS;
using Cysharp.Threading.Tasks;
using GameState;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameRoot
{
    public sealed class GameEntryPoint : SceneEntryPoint
    {
        private async void Start()
        {
            var enterParams = new SceneEnterParams(Scenes.BOOT);
            await Run(enterParams);
        }

        // Application initializing: loading data and setting up.
        public override async UniTask Run<T>(T _)
        {
            SetAppSettings();

            G.CMSProvider = new ScriptableObjectCMSProvider();
            var gameStateProvider = new JsonGameStateProvider();

            await G.CMSProvider.LoadRootCMS();
            await gameStateProvider.LoadGameState();

            G.Repository = new Repository(gameStateProvider);
            G.UIRoot = CreateUIRoot();
            G.SceneProvider = new SceneProvider(G.UIRoot);
            G.AudioProvider = new AudioProvider();
            G.PopUpsProvider = new PopUpsProvider();
            G.PopUpFactory = new PopUpFactory();

            StartGame();
        }

        private void SetAppSettings()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private UIRoot CreateUIRoot()
        {
            var createdUIRoot = Instantiate(G.RootCMS.UICMS.Root);
            DontDestroyOnLoad(createdUIRoot.gameObject);
            return createdUIRoot;
        }

        // Starts the first scene the player will see.
        private void StartGame()
        {
#if UNITY_EDITOR
            var initialEditorScene = GameAutostarter.InitialEditorScene;

            if (initialEditorScene == Scenes.LEVEL_MENU)
            {
                G.SceneProvider.OpenLevelMenu();
                return;
            }
            else if (initialEditorScene == Scenes.CUSTOMIZATION_MENU)
            {
                G.SceneProvider.OpenCustomizationMenu();
                return;
            }
            else if (initialEditorScene == Scenes.OBSTACLE_COURSE_MODE)
            {
                G.SceneProvider.OpenObstacleCourseMode(0, 0);
                return;
            }

            // For an unregistered scene. For example, from assets.
            else if (initialEditorScene != Scenes.BOOT)
            {
                SceneManager.LoadScene(initialEditorScene);
                return;
            }
#endif

            G.SceneProvider.OpenObstacleCourseMode(0, 0);
        }
    }
}