using UnityEngine;

namespace Gameplay
{
    public class Camera : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;

        private CameraTracker _tracker;
        private CameraShaker _shaker;

        public CameraTracker Tracker => _tracker;
        public CameraShaker Shaker => _shaker;

        private void Awake()
        {
            _tracker = new CameraTracker(transform);
            _shaker = new CameraShaker(_camera);
        }

        private void LateUpdate()
        {
            _tracker.Track();
        }
    }
}
