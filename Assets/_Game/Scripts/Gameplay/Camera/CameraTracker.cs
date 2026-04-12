using UnityEngine;

namespace Gameplay
{
    public class CameraTracker : MonoBehaviour
    {
        private Transform _target;

        public void SetTraget(Transform target)
        {
            _target = target;
        }

        private void LateUpdate()
        {
            var trackPosition = _target.position + Vector3.up * 10;
            trackPosition.y = 10f;
            
            transform.position = trackPosition;
        }
    }
}
