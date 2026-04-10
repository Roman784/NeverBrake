using R3;
using System.Collections;
using UnityEngine;
using Utils;

namespace UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private Transform _fullscreenUIContainer;
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private PopUpsRoot _popUpsRoot;

        public PopUpsRoot PopUpsRoot => _popUpsRoot;

        public IEnumerator ShowLoadingScreen()
        {
            yield return _loadingScreen.Show().ToCoroutine();
        }

        public IEnumerator HideLoadingScreen()
        {
            yield return _loadingScreen.Hide().ToCoroutine();
        }

        public void AttachFullsreenUI(MonoBehaviour ui)
        {
            ui.transform.SetParent(_fullscreenUIContainer, false);
        }

        public void ClearAllContainers()
        {
            ClearContainer(_fullscreenUIContainer);
            _popUpsRoot.ClearContainer();
        }

        private void ClearContainer(Transform container)
        {
            var childCount = container.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }
    }
}