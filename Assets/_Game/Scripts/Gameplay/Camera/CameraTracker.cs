using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
    public class CameraTracker : MonoBehaviour
    {
        [SerializeField] private float _smoothTime;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private Vector2 _lookAheadDistance;
        [SerializeField] private float _lookAheadSharpness;

        private bool _doLookAhead = true;
        private Transform _target;
        private Vector3 _currentLookDirection;
        private Vector3 _velocity;

        private void LateUpdate()
        {
            Track(Time.deltaTime);
        }

        public void DisableLookAhead() => _doLookAhead = false;

        public void SetTarget(Transform target)
        {
            _target = target;
            _currentLookDirection = _target.up;
            transform.position = _target.position + _offset;
        }

        public void Track(float deltaTime)
        {
            if (_target == null) return;

            var currentTargetPosition = _target.position;
            var lookAheadOffset = GetLookAhead(deltaTime);
            var targetPosition = currentTargetPosition + _offset + lookAheadOffset;
            var smoothPosition = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref _velocity,
                _smoothTime
            );

            smoothPosition.z = transform.position.z;
            transform.position = smoothPosition;
        }

        private Vector3 GetLookAhead(float deltaTime)
        {
            if (!_doLookAhead) return Vector2.zero;

            _currentLookDirection = Vector3.Lerp(
                _currentLookDirection,
                _target.up,
                deltaTime * _lookAheadSharpness
            );

            return new Vector3(
                _currentLookDirection.x * _lookAheadDistance.x,
                _currentLookDirection.y * _lookAheadDistance.y,
                0
            );
        }
    }
}