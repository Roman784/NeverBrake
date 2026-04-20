using Cysharp.Threading.Tasks;
using GameRoot;
using R3;
using System;
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
                    SelectCarId(_view.GetSelectedItem());
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
                var createdPreview = _view.CreateItem();
                _model.AddCarPreviewItem(createdPreview, carId);

                createdPreview.SetIcon(_model.GetCarPreview(carId));
                createdPreview.SetLock(!_model.IsUnlocked(carId));

                createdPreview.PressedSignal
                    .Subscribe(_ =>
                    {
                        SelectCarId(createdPreview);
                        ScrollToSelected(false);
                    });
            }
        }

        public void ScrollToSelected(bool instant = true)
        {
            var previewItem = _model.GetCarPreviewItem(_model.SelectedCarId);
            if (previewItem != null)
                _view.ScrollTo(previewItem, instant);
        }

        private void SelectCarId(CarPreviewItem previewItem)
        {
            var id = _model.GetCarIdByPreviewItem(previewItem);
            _model.SelectedCarId = id;
        }

        private async UniTask SaveSelectionAndLeave()
        {
            if (_model.IsSelectedCarUnlocked())
                await _model.SaveSelectedCarId();
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
