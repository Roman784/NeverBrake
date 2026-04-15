using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
    public class Camera : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;

        [Space]

        [SerializeField] private int _zoomStrenght;

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

        public void Zoom()
        {
            var end = _camera.fieldOfView - _zoomStrenght;
            DOTween.To
            (
                () => _camera.fieldOfView,
                x =>
                {
                    _camera.fieldOfView = x;
                },
                end,
                0.75f
            ).SetEase(Ease.OutElastic, amplitude: 0f, period: 0.75f);
        }
    }
}
