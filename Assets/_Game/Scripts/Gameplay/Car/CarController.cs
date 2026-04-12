using R3;
using System;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;

        [Space]

        [SerializeField] private float _minTurningSpeed;
        [SerializeField] private float _maxTurningSpeed;
        [SerializeField] private float _turningAcceleration;
        [SerializeField] private AnimationCurve _turningAccelerationCurve;

        [Space]

        [SerializeField] private float _grip;

        private Rigidbody _rigidbody;
        private CarInput _input;

        private float _turningTime;
        private int _previousTurningDirection;

        public float TurningSpeed => CalculateTurningSpeed();

        public void Initialize(CarInput input)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _input = input;
        }

        public void Handle(float deltaTime)
        {
            var horizontalInput = _input.GetHorizontalInput();

            Move(deltaTime);
            Turn(horizontalInput, deltaTime);
        }

        private void Move(float deltaTime)
        {
            var force = transform.forward * _movementSpeed * deltaTime;

            if (_rigidbody.linearVelocity.magnitude < _movementSpeed * 2f)
                _rigidbody.AddForce(force, ForceMode.Force);

            var lateralVelocity = Vector3.Project(_rigidbody.linearVelocity, transform.right);
            _rigidbody.AddForce(-lateralVelocity * _grip * deltaTime, ForceMode.Force);
        }

        private void Turn(int direction, float deltaTime)
        {
            direction = Mathf.Clamp(direction, -1, 1);
            if (direction == 0 || _previousTurningDirection != direction) _turningTime = 0;
            else _turningTime += deltaTime;

            _previousTurningDirection = direction;

            var angle = transform.rotation.eulerAngles.y;
            angle += direction * TurningSpeed * deltaTime;

            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        private float CalculateTurningSpeed()
        {
            var t = _turningAccelerationCurve.Evaluate(_turningTime * _turningAcceleration);
            return Mathf.Lerp(_minTurningSpeed, _maxTurningSpeed, t);
        }
    }
}
