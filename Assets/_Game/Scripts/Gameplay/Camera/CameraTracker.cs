using UnityEngine;

namespace Gameplay
{
    public class CameraTracker
    {
        private Transform _transform;
        private Transform _target;

        public CameraTracker(Transform transform)
        {
            _transform = transform;
        }

        public void SetTraget(Transform target)
        {
            _target = target;
        }

        public void Track()
        {
            if (_target == null) return;
            
            var trackPosition = _target.position + Vector3.up * 10;
            trackPosition.y = _transform.position.y;

            _transform.position = trackPosition;
        }
    }
}
