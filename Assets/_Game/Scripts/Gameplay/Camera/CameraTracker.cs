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

        private bool _doTrack;
        private Transform _target;
        private Vector3 _currentLookDirection;
        private Vector3 _velocity;

        private void LateUpdate()
        {
            Track(Time.deltaTime);
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            _doTrack = true;
            _currentLookDirection = _target.up;
            transform.position = _target.position + _offset;
        }

        public void Track(float deltaTime)
        {
            if (_target == null || !_doTrack) return;

            var currentTargetPosition = _target.position;

            _currentLookDirection = Vector3.Lerp(
                _currentLookDirection,
                _target.up,
                deltaTime * _lookAheadSharpness
            );

            var lookAheadOffset = new Vector3(
                _currentLookDirection.x * _lookAheadDistance.x,
                _currentLookDirection.y * _lookAheadDistance.y,
                0
            );

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

        public void Break()
        {
            _doTrack = false;
            transform.DOMove(_target.position + _offset, 0.5f).SetEase(Ease.OutQuad);
        }
    }
}