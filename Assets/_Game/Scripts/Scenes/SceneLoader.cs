using Cysharp.Threading.Tasks;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace GameRoot
{
    public class SceneLoader
    {
        private readonly UIRoot _uiRoot;

        public SceneLoader(UIRoot uiRoot)
        {
            _uiRoot = uiRoot;
        }

        public void LoadAndRunScene<TEntryPoint, TEnterParams>(TEnterParams enterParams)
            where TEntryPoint : SceneEntryPoint
            where TEnterParams : SceneEnterParams
        {
            DOTween.KillAll(true);
            Coroutines.StopAll();

            LoadAndRunSceneAsync<TEntryPoint, TEnterParams>(enterParams).Forget();
        }

        // Sequential loading and starting of a new scene.
        private async UniTask LoadAndRunSceneAsync<TEntryPoint, TEnterParams>(TEnterParams enterParams)
            where TEntryPoint : SceneEntryPoint
            where TEnterParams : SceneEnterParams
        {
            await _uiRoot.ShowLoadingScreen();

            _uiRoot.ClearAllContainers();

            await LoadSceneAsync(enterParams.SceneName);

            var sceneEntryPoint = Object.FindAnyObjectByType<TEntryPoint>();

            if (sceneEntryPoint == null)
            {
                Debug.LogError($"EntryPoint of type {typeof(TEntryPoint)} not found");
                return;
            }

            await sceneEntryPoint.Run(enterParams);

            await _uiRoot.HideLoadingScreen();
        }

        private async UniTask LoadSceneAsync(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName);
            await UniTask.Yield();
        }
    }
}
