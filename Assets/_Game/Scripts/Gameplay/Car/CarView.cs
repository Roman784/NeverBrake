using DG.Tweening;
using R3;
using UnityEngine;
using VisualEffects;

namespace Gameplay
{
    public class CarView : MonoBehaviour
    {
        [SerializeField] private Transform _root;

        [Space]

        [SerializeField] private Transform[] _wheels;
        [SerializeField] private TrailRenderer[] _tireTracks;

        [Space]

        [SerializeField] private Transform _boostVFXPoint;
        [SerializeField] private VFX _boostVFXPrefab;
        [SerializeField] private VFX _collisionVFXPrefab;
        [SerializeField] private VFX _crashVFXPrefab;

        private Sequence _landingSeq;
        private Sequence _boostSeq;

        public void ApplyWheelsTurning(float angle)
        {
            foreach (var wheel in _wheels)
            {
                wheel.localRotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        public void PlayLandingAnimation()
        {
            _landingSeq?.Kill(true);
            _landingSeq = DOTween.Sequence();

            _landingSeq.Append(_root.DOScale(0.8f, 0.05f).SetEase(Ease.OutQuart));
            _landingSeq.AppendInterval(0.05f);
            _landingSeq.Append(_root.DOScale(1f, 0.25f).SetEase(Ease.OutQuad));
        }

        public void SetActiveTireTracks(bool value)
        {
            foreach (var tireTrack in _tireTracks)
            {
                tireTrack.emitting = value;
            }
        }

        public void PlayBoostAnimation()
        {
            _boostSeq?.Kill(true);
            _landingSeq?.Kill(true);

            _boostSeq = DOTween.Sequence();

            _boostSeq.Append(_root.DOScaleY(1.15f, 0.1f).SetEase(Ease.OutQuad));
            _boostSeq.Join(_root.DOScaleX(0.9f, 0.1f).SetEase(Ease.OutQuad));
            _boostSeq.AppendInterval(0.1f);
            _boostSeq.Append(_root.DOScale(1f, 0.4f).SetEase(Ease.InQuad));
        }

        public Tween PlayPortalSuctionAnimation(Vector2 portalCenter)
        {
            _boostSeq?.Kill(true);
            _landingSeq?.Kill(true);
            _root.DOKill(true);

            var seq = DOTween.Sequence();
            seq.Append(transform.DOMove(portalCenter, 0.75f).SetEase(Ease.OutQuad));
            seq.Join(_root.DOScale(0, 2f).SetEase(Ease.InQuad));
            seq.Join(_root.DORotate(Vector3.forward * 360 * 3, 2f, RotateMode.FastBeyond360));

            return seq;
        }

        public Tween PlayFallingIntoWaterAnimation()
        {
            _boostSeq?.Kill(true);
            _landingSeq?.Kill(true);
            _root.DOKill(true);

            var rotation = Vector2.right * 60;
            var position = transform.position + transform.up / 2f;
            position.z += 2f;

            var seq = DOTween.Sequence();
            seq.Append(transform.DOMove(position, 0.75f).SetEase(Ease.InQuad));
            seq.Join(_root.DOLocalRotate(rotation, 0.75f).SetEase(Ease.OutQuad));

            return seq;
        }

        public void PlayBoostVFX()
        {
            VFX.Create(_boostVFXPrefab, _boostVFXPoint).Play();
        }

        public void PlayCollisionVFX(Vector3 position, Vector3 normal)
        {
            var rotation = Quaternion.LookRotation(normal);
            VFX.Create(_collisionVFXPrefab, position, rotation).Play();
        }

        public Observable<Unit> PlayCrashVFX(Vector3 position)
        {
            var effect = VFX.Create(_crashVFXPrefab, position);
            effect.Play();

            return Observable.Timer(System.TimeSpan.FromSeconds(effect.Duration));
        }
    }
}
