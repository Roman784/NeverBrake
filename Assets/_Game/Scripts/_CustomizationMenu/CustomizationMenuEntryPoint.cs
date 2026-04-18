using Cysharp.Threading.Tasks;
using GameRoot;
using LevelMenu;
using UnityEngine;

namespace CustomizationMenu
{
    public class CustomizationMenuEntryPoint : SceneEntryPoint<CustomizationMenuEnterParams>
    {
        [SerializeField] private CustomizationMenuView _view;

        private CustomizationMenuPresenter _presenter;

        protected override async UniTask Run(CustomizationMenuEnterParams enterParams)
        {
            var model = new CustomizationMenuModel();
            _presenter = new CustomizationMenuPresenter(_view, model);

            _presenter.CreateCarPreviewItems();
            _presenter.ScrollToSelected();

            await UniTask.Yield();
        }

        private void Update()
        {
            Canvas.ForceUpdateCanvases();

        }

        private void OnDestroy()
        {
            _presenter?.Dispose();
        }
    }
}
