using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(CarController))]
    public class Car : MonoBehaviour
    {
        private CarController _controller;

        private bool _isCrashed;

        public void Initialize(CarInput input)
        {
            _controller = GetComponent<CarController>();
            _controller.Initialize(input);
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
