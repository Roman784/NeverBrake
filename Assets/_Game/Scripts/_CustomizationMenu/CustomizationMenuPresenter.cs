using Cysharp.Threading.Tasks;
using GameRoot;
using R3;
using System;
using System.Collections;
using System.Linq;
using UI;
using UnityEngine;

namespace CustomizationMenu
{
    public class CustomizationMenuPresenter : IDisposable
    {
        private CustomizationMenuView _view;
        private CustomizationMenuModel _model;

        private CompositeDisposable _disposables = new();

        public CustomizationMenuPresenter(
            CustomizationMenuView view,
            CustomizationMenuModel model)
        {
            _view = view;
            _model = model;

            _view.BindWallet(_model.Wallet);
            SetupSubscriptions();
        }

        private void SetupSubscriptions()
        {
            _view.SelectButtonPressedSignal
                .SubscribeAwait(async (_, _) => await SaveSelectionAndLeave())
                .AddTo(_disposables);

            _view.GachaButtonPressedSignal
                .Subscribe(_ => OpenGachaPopUp())
                .AddTo(_disposables);

            _view.SettingsButtonPressedSignal
                .Subscribe(_ => OpenSettingsPopUp())
                .AddTo(_disposables);

            _view.ScrollEndedSignal
                .Subscribe(_ =>
                {
                    SelectCarId(_view.GetSelectedPreview());
                })
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void CreateCarPreviewItems()
        {
            foreach (var carId in _model.GetCarIds())
            {
                var previewPrefab = _model.GetCarPreviewPrefab(carId);
                var createdPreview = _view.CreateCarPreview(previewPrefab);
                createdPreview.SetLock(!_model.IsUnlocked(carId));

                createdPreview.PressedSignal
                    .Subscribe(_ =>
                    {
                        SelectCarId(createdPreview);
                        ScrollToSelected(false);
                    });

                _model.AddCarPreview(createdPreview, carId);
            }
        }

        public void ScrollToSelected(bool instant = true)
        {
            var preview = _model.GetCarPreview(_model.SelectedCarId);
            if (preview != null)
                _view.ScrollTo(preview, instant);
        }

        private void SelectCarId(SkinPreview preview)
        {
            var id = _model.GetCarIdByPreview(preview);
            _model.SelectedCarId = id;
        }

        private async UniTask SaveSelectionAndLeave()
        {
            if (!_model.IsSelectedCarUnlocked()) return;

            await _model.SaveSelectedCarId();
            G.SceneProvider.OpenLevelMenu();
        }

        private void OpenGachaPopUp()
        {
            G.SceneProvider.OpenGachaMenu();
        }

        private void OpenSettingsPopUp()
        {

        }
    }
}
