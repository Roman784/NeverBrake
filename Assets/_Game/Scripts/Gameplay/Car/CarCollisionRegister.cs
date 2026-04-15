using Physics;
using R3;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Utils;

namespace Gameplay
{
    public class CarCollisionRegister : MonoBehaviour
    {
        [SerializeField] private string _wallTag = "Wall";
        [SerializeField] private string _trapTag = "Trap";
        [SerializeField] private string _waterTag = "Water";
        [SerializeField] private string _finishTag = "Finish";

        [Space]

        [SerializeField] private SurfaceChecker _surfaceChecker;

        private bool _canRegister;

        private Subject<Collision> _collidedWithWallSignalSubj = new();
        private Subject<Collision> _collidedWithTrapSignalSubj = new();
        private Subject<Collision> _collidedWithWaterSignalSubj = new();
        private Subject<Collider> _collidedWithFinishSignalSubj = new();

        public Observable<Collision> CollidedWithWallSignal => _collidedWithWallSignalSubj;
        public Observable<Collision> CollidedWithTrapSignal => _collidedWithTrapSignalSubj;
        public Observable<Collision> CollidedWithWaterSignal => _collidedWithWaterSignalSubj;
        public Observable<Collider> CollidedWithFinishignal => _collidedWithFinishSignalSubj;
        
        public void Enabled() => _canRegister = true;
        public void Disable() => _canRegister = false;

        public bool AreAllPartsOnGround()
        {
            if (!_canRegister) return false;

            if (_surfaceChecker.CheckGround(out var _, out var missedRays))
                return missedRays == 0;
            return false;
        }

        public bool IsAnyPartOnGround()
        {
            if (!_canRegister) return false;

            return _surfaceChecker.CheckGround(out var _, out var _);
        }

        public float GetGroundHeight()
        {
            if (_surfaceChecker.TryGetGround(out var hit, out var _))
                return hit.point.y;
            return transform.position.y;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_canRegister) return;

            if (collision.collider.CompareTag(_wallTag))
                _collidedWithWallSignalSubj.OnNext(collision);

            else if (collision.collider.CompareTag(_trapTag))
                _collidedWithTrapSignalSubj.OnNext(collision);

            else if (collision.collider.CompareTag(_waterTag))
                _collidedWithWaterSignalSubj.OnNext(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_canRegister) return;

            if (other.CompareTag(_finishTag))
                _collidedWithFinishSignalSubj.OnNext(other);
        }
    }
}
