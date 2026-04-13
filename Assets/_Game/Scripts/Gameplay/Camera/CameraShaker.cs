using DG.Tweening;
using Gameplay;

namespace Gameplay
{
    public class CameraShaker
    {
        private UnityEngine.Camera _camera;
        private Tween _shakeTween;

        public CameraShaker(UnityEngine.Camera camera)
        {
            _camera = camera;
        }

        public void WeakShake()
        {
            Shake(0.5f, 2);
        }

        public void MidShake()
        {
            Shake(1f, 4);
        }

        public void StrongShake()
        {
            Shake(2f, 6);
        }

        public void Shake(float strengh, int vibrato = 3)
        {
            _shakeTween?.Kill();
            _shakeTween = _camera.DOShakePosition(0.2f, strengh, vibrato, 45)
                .SetEase(Ease.OutQuad);
        }
    }
}