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
            _view.ScrollEndedSignal
                .Subscribe(_ => Debug.Log(_view.GetSelectedItem()));
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
                createdPreview.SetIcon(_model.GetCarPreview(carId));
            }
        }
    }
}
