using R3;
using System;
using UnityEngine;

namespace Gameplay
{
    public class CarController : IDisposable
    {
        private readonly CarViewModel _viewModel;
        private readonly Transform _transform;
        private readonly CarInput _input;

        private readonly CompositeDisposable _disposables = new();

        public CarController(
            CarViewModel viewModel, 
            Transform transform,
            CarInput input)
        {
            _viewModel = viewModel;
            _transform = transform;
            _input = input;

            _input.JumpSignal
                .Subscribe(_ => Jump())
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Update(float deltaTime)
        {
            if (_viewModel.IsCrashed) return;

            var horizontalInput = _input.GetHorizontalInput();

            _viewModel.Move(deltaTime);
            _viewModel.Turn(horizontalInput, deltaTime);

            _transform.position += _viewModel.MovementDirection * _viewModel.MovementSpeed * deltaTime;
            _transform.rotation = Quaternion.LookRotation(_viewModel.TurningDirection);
        }

        private void Jump()
        {
            if (_viewModel.IsCrashed) return;


        }
    }
}
