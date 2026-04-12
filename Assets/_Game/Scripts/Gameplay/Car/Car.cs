using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(CarController))]
    [RequireComponent(typeof(CarCollisionHandler))]
    public class Car : MonoBehaviour
    {
        [SerializeField] private CarView _view;
        
        private CarController _controller;
        private CarCollisionHandler _collisionHandler;

        private bool _isCrashed;

        public void Initialize(CarInput input)
        {
            _controller = GetComponent<CarController>();
            _collisionHandler = GetComponent<CarCollisionHandler>();

            _controller.Initialize(_view, _collisionHandler, input);
        }

        private void FixedUpdate()
        {
            if (_isCrashed) return;

            _controller.ProcessInput(Time.fixedDeltaTime);
        }

        public void Crash()
        {
            _isCrashed = true;
        }
    }
}
