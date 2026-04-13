using Physics;
using R3;
using UnityEngine;

namespace Gameplay
{
    public class CarCollisionHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private SurfaceChecker _surfaceChecker;

        private Subject<Collision> _collidedSignalSubj = new();

        public Observable<Collision> CollidedSignal => _collidedSignalSubj;
        public bool OnGround => _surfaceChecker.CheckGround(out var _);

        private void OnCollisionEnter(Collision collision)
        {
            if ((_obstacleMask.value & (1 << collision.gameObject.layer)) != 0)
            {
                _collidedSignalSubj.OnNext(collision);
            }
        }
    }
}
