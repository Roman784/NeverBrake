using Physics;
using R3;
using UnityEngine;
using Utils;

namespace Gameplay
{
    public class CarCollisionRegister : MonoBehaviour
    {
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private SurfaceChecker _surfaceChecker;

        private Subject<Collision> _collidedSignalSubj = new();

        public Observable<Collision> CollidedSignal => _collidedSignalSubj;
        public bool OnGround => _surfaceChecker.CheckGround(out var _);

        private void OnCollisionEnter(Collision collision)
        {
            if (_obstacleMask.Contains(collision.gameObject.layer))
            {
                _collidedSignalSubj.OnNext(collision);
            }
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (_obstacleMask.Contains(other.gameObject.layer))
        //    {
        //        _collidedSignalSubj.OnNext(other);
        //    }
        //}
    }
}
