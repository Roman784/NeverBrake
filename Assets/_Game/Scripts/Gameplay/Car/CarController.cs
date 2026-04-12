using R3;
using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        [SerializeField] private float _forwardAcceleration;
        [SerializeField] private float _maxVelocity;
        [SerializeField] private float _lateralFriction;

        [Space]

        [SerializeField] private float _minTurningSpeed;
        [SerializeField] private float _maxTurningSpeed;
        [SerializeField] private float _turningAcceleration;
        [SerializeField] private AnimationCurve _turningAccelerationCurve;

        [Space]

        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _jumpDuration;
        [SerializeField] private AnimationCurve _jumpHeightCurve;

        [Space]

        [SerializeField] private float _boostImpulse;

        private Rigidbody _rigidbody;
        private CarInput _input;

        private float _turnInputDuration;
        private int _lastTurnDirection;

        public float TurningSpeed => CalculateTurningSpeed();

        public void Initialize(CarInput input)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _input = input;
        }

        public void ProcessInput(float deltaTime)
        {
            var horizontalInput = _input.GetHorizontalInput();
            var shouldJump = _input.ShouldJump();

            LimitMaxSpeed();
            ApplyMovementForces(deltaTime);
            ApplyTurning(horizontalInput, deltaTime);

            if (shouldJump)
                Jump();
        }

        private void ApplyMovementForces(float deltaTime)
        {
            ApplyForwardAcceleration(deltaTime);
            SuppressLateralVelocity(deltaTime);
        }

        private void LimitMaxSpeed()
        {
            _rigidbody.maxLinearVelocity = _maxVelocity;
        }

        private void ApplyForwardAcceleration(float deltaTime)
        {
            var force = transform.forward * _forwardAcceleration * deltaTime;
            _rigidbody.AddForce(force, ForceMode.Force);
        }

        private void SuppressLateralVelocity(float deltaTime)
        {
            var lateralVelocity = Vector3.Project(_rigidbody.linearVelocity, transform.right);
            _rigidbody.AddForce(-lateralVelocity * _lateralFriction * deltaTime, ForceMode.Force);
        }

        private void ApplyTurning(int direction, float deltaTime)
        {
            if (direction == 0 || _lastTurnDirection != direction) _turnInputDuration = 0;
            else _turnInputDuration += deltaTime;

            _lastTurnDirection = direction;

            var angle = transform.rotation.eulerAngles.y;
            angle += direction * TurningSpeed * deltaTime;

            _rigidbody.MoveRotation(Quaternion.Euler(0f, angle, 0f));
        }

        private void Jump()
        {
            StartCoroutine(JumpRoutine());
            ApplyBoostImpulse();
        }

        private IEnumerator JumpRoutine()
        {
            var initialY = 0f;
            for (float time = 0f; time < _jumpDuration; time += Time.deltaTime)
            {
                var progress = time / _jumpDuration;
                var y = _jumpHeight * _jumpHeightCurve.Evaluate(progress);
                
                var newPosition = transform.position;
                newPosition.y = initialY + y;

                transform.position = newPosition;

                yield return null;
            }
        }

        private void ApplyBoostImpulse()
        {
            _rigidbody.AddForce(transform.forward * _boostImpulse, ForceMode.Impulse);
        }

        private float CalculateTurningSpeed()
        {
            var t = _turningAccelerationCurve.Evaluate(_turnInputDuration * _turningAcceleration);
            return Mathf.Lerp(_minTurningSpeed, _maxTurningSpeed, t);
        }
    }
}
