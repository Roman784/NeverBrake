using DG.Tweening;
using System.Collections;
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
            Coroutines.Start(
                LoadAndRunSceneRoutine<TEntryPoint, TEnterParams>(enterParams));
        }

        // Sequential loading and starting of a new scene.
        private IEnumerator LoadAndRunSceneRoutine<TEntryPoint, TEnterParams>(TEnterParams enterParams)
            where TEntryPoint : SceneEntryPoint
            where TEnterParams : SceneEnterParams
        {
            yield return _uiRoot.ShowLoadingScreen();

            _uiRoot.ClearAllContainers();

            yield return LoadScene(enterParams.SceneName);

            var sceneEntryPoint = Object.FindFirstObjectByType<TEntryPoint>();
            yield return sceneEntryPoint.Run(enterParams);
            
            yield return _uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return null;
        }
    }
}