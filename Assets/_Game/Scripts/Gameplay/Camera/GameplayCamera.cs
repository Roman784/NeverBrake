using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(CameraShaker))]
    [RequireComponent(typeof(CameraTracker))]
    public class GameplayCamera : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float _zoomStrenght;

        private CameraTracker _tracker;
        private CameraShaker _shaker;
        private Camera _camera;

        public CameraTracker Tracker => _tracker;
        public CameraShaker Shaker => _shaker;

        private void Awake()
        {
            _tracker = GetComponent<CameraTracker>();
            _shaker = GetComponent<CameraShaker>();

            _camera = Camera.main;
        }

        public void Zoom()
        {
            _tracker.Break();

            var end = _camera.orthographicSize - _camera.orthographicSize * _zoomStrenght;
            DOTween.To
            (
                () => _camera.orthographicSize,
                x =>
                {
                    _camera.orthographicSize = x;
                },
                end,
                0.75f
            ).SetEase(Ease.OutElastic, amplitude: 0f, period: 0.75f);
        }
    }
}
