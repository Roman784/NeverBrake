using R3;
using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
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

        private Rigidbody2D _rigidbody;

        private float _turnInputDuration;
        private int _lastHorizontalInput;

        private Subject<Unit> _jumpCompletedSignalSubj = new();

        public float TurnInputDuration => _turnInputDuration;
        private float TurningSpeed => CalculateTurningSpeed();

        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Stop()
        {
            StopAllCoroutines();
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;
            _rigidbody.bodyType = RigidbodyType2D.Static;
        }

        public void ApplyMovement(float deltaTime)
        {
            LimitMaxSpeed();
            ApplyForwardAcceleration(deltaTime);
            SuppressLateralVelocity(deltaTime);
        }

        public void ApplyTurning(int horizontalInput, float deltaTime)
        {
            if (horizontalInput == 0 || _lastHorizontalInput != horizontalInput) _turnInputDuration = 0;
            else _turnInputDuration += deltaTime;

            _lastHorizontalInput = horizontalInput;

            ApplyBodyTurning(horizontalInput, deltaTime);
        }

        public Observable<Unit> Jump()
        {
            StartCoroutine(JumpRoutine());

            _jumpCompletedSignalSubj = new Subject<Unit>();
            return _jumpCompletedSignalSubj;
        }

        public void ApplyBoostImpulse()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.AddForce(transform.up * _boostImpulse, ForceMode2D.Impulse);
        }

        //public float GetWheelsTurning(int horizontalInput, float deltaTime)
        //{
        //    var angle = -horizontalInput * Mathf.Lerp(0, 1, _turnInputDuration * 1.5f);
        //    return Mathf.Lerp(_wheelsAngle, angle, _wheelsTurningSpeed * deltaTime);
        //}

        private void LimitMaxSpeed()
        {
            var velocity = _rigidbody.linearVelocity;
            velocity.x = Mathf.Clamp(velocity.x, -_maxVelocity, _maxVelocity);
            velocity.y = Mathf.Clamp(velocity.y, -_maxVelocity, _maxVelocity);
            _rigidbody.linearVelocity = velocity;
        }

        private void ApplyForwardAcceleration(float deltaTime)
        {
            var force = transform.up * _forwardAcceleration * deltaTime;
            _rigidbody.AddForce(force, ForceMode2D.Force);
        }

        private void SuppressLateralVelocity(float deltaTime)
        {
            var right = transform.right;
            var lateralVelocity = Vector2.Dot(_rigidbody.linearVelocity, right) * right;
            _rigidbody.AddForce(-lateralVelocity * _lateralFriction * deltaTime, ForceMode2D.Force);
        }


        private void ApplyBodyTurning(int horizontalInput, float deltaTime)
        {
            var step = horizontalInput * TurningSpeed;
            var angle = transform.rotation.eulerAngles.z;
            angle -= step * deltaTime;

            _rigidbody.MoveRotation(Quaternion.Euler(0f, 0f, angle));
        }

        private IEnumerator JumpRoutine()
        {
            var initialZ = transform.position.z;
            for (float time = 0f; time < _jumpDuration; time += Time.deltaTime)
            {
                var progress = time / _jumpDuration;
                var z = _jumpHeight * _jumpHeightCurve.Evaluate(progress);

                var newPosition = transform.position;
                newPosition.z = initialZ - z;

                transform.position = newPosition;
                transform.localScale = Vector3.one + Vector3.one * -newPosition.z / 2f;

                yield return null;
            }

            CompleteJump();
        }

        private void CompleteJump()
        {
            var position = transform.position; position.z = 0f;
            transform.position = position;
            transform.localScale = Vector3.one;

            _jumpCompletedSignalSubj.OnNext(Unit.Default);
            _jumpCompletedSignalSubj.OnCompleted();
        }

        private float CalculateTurningSpeed()
        {
            var t = _turningAccelerationCurve.Evaluate(_turnInputDuration * _turningAcceleration);
            return Mathf.Lerp(_minTurningSpeed, _maxTurningSpeed, t);
        }
    }
}
