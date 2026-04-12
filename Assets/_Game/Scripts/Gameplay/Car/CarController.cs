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

        [SerializeField] private float _wheelsTurningSpeed;
        [SerializeField] private float _maxWheelsTurning;

        [Space]

        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _jumpDuration;
        [SerializeField] private AnimationCurve _jumpHeightCurve;

        [Space]

        [SerializeField] private float _boostImpulse;

        private Rigidbody _rigidbody;

        private CarView _view;
        private CarCollisionHandler _collisionHandler;
        private CarInput _input;

        private float _turnInputDuration;
        private int _lastHorizontalInput;
        private float _wheelsAngle;

        private bool _inJump;

        public float TurningSpeed => CalculateTurningSpeed();

        public void Initialize(
            CarView view, 
            CarCollisionHandler collisionHandler, 
            CarInput input)
        {
            _rigidbody = GetComponent<Rigidbody>();

            _view = view;
            _collisionHandler = collisionHandler;
            _input = input;
        }

        public void ProcessInput(float deltaTime)
        {
            var horizontalInput = _input.GetHorizontalInput();
            var shouldJump = _input.ShouldJump();

            LimitMaxSpeed();
            ApplyMovementForces(deltaTime);
            ApplyTurning(horizontalInput, deltaTime);

            if (shouldJump && !_inJump && _collisionHandler.OnGround)
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

        private void ApplyTurning(int horizontalInput, float deltaTime)
        {
            if (horizontalInput == 0 || _lastHorizontalInput != horizontalInput) _turnInputDuration = 0;
            else _turnInputDuration += deltaTime;

            _lastHorizontalInput = horizontalInput;

            ApplyBodyTurning(horizontalInput, deltaTime);
            ApplyWheelsTurning(horizontalInput, deltaTime);
        }

        private void ApplyBodyTurning(int horizontalInput, float deltaTime)
        {
            var step = horizontalInput * TurningSpeed;
            var angle = transform.rotation.eulerAngles.y;
            angle += step * deltaTime;

            _rigidbody.MoveRotation(Quaternion.Euler(0f, angle, 0f));
        }

        private void ApplyWheelsTurning(int horizontalInput, float deltaTime)
        {
            var angle = -horizontalInput * Mathf.Lerp(0, _maxWheelsTurning, _turnInputDuration * 1.5f);
            _wheelsAngle = Mathf.Lerp(_wheelsAngle, angle, _wheelsTurningSpeed * deltaTime);
            _view.ApplyWheelsTurning(_wheelsAngle);
        }

        private void Jump()
        {
            _inJump = true;
            StartCoroutine(JumpRoutine());
            ApplyBoostImpulse();
        }

        private IEnumerator JumpRoutine()
        {
            _rigidbody.useGravity = false;
            var initialY = transform.position.y;
            for (float time = 0f; time < _jumpDuration; time += Time.deltaTime)
            {
                var progress = time / _jumpDuration;
                var y = _jumpHeight * _jumpHeightCurve.Evaluate(progress);
                
                var newPosition = transform.position;
                newPosition.y = initialY + y;

                transform.position = newPosition;

                yield return null;
            }

            FinishJump();
        }

        private void FinishJump()
        {
            _inJump = false;
            var velocity = _rigidbody.linearVelocity;
            velocity.y = 0f;
            _rigidbody.linearVelocity = velocity;
            _rigidbody.useGravity = true;
        }

        private void ApplyBoostImpulse()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.AddForce(transform.forward * _boostImpulse, ForceMode.Impulse);
        }

        private float CalculateTurningSpeed()
        {
            var t = _turningAccelerationCurve.Evaluate(_turnInputDuration * _turningAcceleration);
            return Mathf.Lerp(_minTurningSpeed, _maxTurningSpeed, t);
        }
    }
}
