using R3;
using System;
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

            SetupSubscriptions();
        }

        private void SetupSubscriptions()
        {
            _view.SelectButtonPressedSignal
                .Subscribe(_ => HandleSelectButtonPressed())
                .AddTo(_disposables);

            _view.ScrollStartedSignal
                .Subscribe(_ => _view.SetLockSelectButton(true))
                .AddTo(_disposables);

            _view.ScrollEndedSignal
                .Subscribe(_ =>
                {
                    SelectCarId(_view.GetSelectedItem());
                    HandleSelectButtonLock();
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

        private void HandleSelectButtonLock()
        {
            var isSelectedCarLocked = !_model.IsSelectedCarUnlocked();
            _view.SetLockSelectButton(isSelectedCarLocked);
        }

        private void HandleSelectButtonPressed()
        {
            if (!_model.IsSelectedCarUnlocked()) return;

            _model.SaveSelectedCarId();
        }
    }
}
