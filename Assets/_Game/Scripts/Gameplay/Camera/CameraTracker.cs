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

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void Track()
        {
            if (_target == null) return;
            
            var trackPosition = _target.position + Vector3.forward * -6;

            _transform.position = trackPosition;
        }
    }
}
