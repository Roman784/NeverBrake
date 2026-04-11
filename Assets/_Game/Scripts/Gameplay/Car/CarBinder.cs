using UnityEngine;
using R3;

namespace Gameplay
{
    public class CarBinder : MonoBehaviour
    {
        private CarViewModel _viewModel;
        private CarInput _input;
        private CarController _controller;

        public void Init(CarViewModel viewModel, CarInput input)
        {
            _viewModel = viewModel;
            _input = input;

            _controller = new CarController(
                viewModel: _viewModel,
                transform: transform,
                input: _input);   
        }

        private void Update()
        {
            if (_viewModel == null || _viewModel.IsCrashed) return;

            _controller.Update(Time.deltaTime);
        }

        private void OnCrush()
        {
            _viewModel.Crush();
        }

        private void OnDestroy()
        {
            _controller?.Dispose();
        }
    }
}
