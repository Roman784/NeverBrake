using R3;
using UnityEngine;

namespace Gameplay
{
    public class CarCollisionRegister : MonoBehaviour
    {
        [Header("Tags")]
        [SerializeField] private string _groundTag = "Ground";
        [SerializeField] private string _waterTag = "Water";
        [SerializeField] private string _wallTag = "Wall";
        [SerializeField] private string _trapTag = "Trap";
        [SerializeField] private string _finishTag = "Finish";

        [Header("Check Settings")]
        [SerializeField] private Vector3[] _checkPoints;
        [SerializeField] private float _checkRadius;

        private bool _isRegistrationEnabled;
        private bool _isOnGround;
        private bool _isOnWater;

        private Subject<Collision2D> _collidedWithWallSignalSubj = new();
        private Subject<Collider2D> _collidedWithTrapSignalSubj = new();
        private Subject<Collider2D> _collidedWithFinishSignalSubj = new();

        public Observable<Collision2D> CollidedWithWallSignal => _collidedWithWallSignalSubj;
        public Observable<Collider2D> CollidedWithTrapSignal => _collidedWithTrapSignalSubj;
        public Observable<Collider2D> CollidedWithFinishSignal => _collidedWithFinishSignalSubj;

        private void Update()
        {
            UpdateSurfaceState();
        }

        public void EnableRegistration() => _isRegistrationEnabled = true;
        public void DisableRegistration() => _isRegistrationEnabled = false;

        public bool OnGround() => _isRegistrationEnabled && _isOnGround;
        public bool OnWater() => _isRegistrationEnabled && _isOnWater && !_isOnGround;

        private void UpdateSurfaceState()
        {
            _isOnGround = false;
            _isOnWater = false;

            foreach (var point in _checkPoints)
            {
                var worldPoint = transform.TransformPoint(point);
                var hits = Physics2D.OverlapCircleAll(worldPoint, _checkRadius);

                foreach (var hit in hits)
                {
                    if (hit.CompareTag(_groundTag))
                        _isOnGround = true;

                    if (hit.CompareTag(_waterTag))
                        _isOnWater = true;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_isRegistrationEnabled) return;

            if (collision.collider.CompareTag(_wallTag))
                _collidedWithWallSignalSubj.OnNext(collision);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isRegistrationEnabled) return;

            if (other.CompareTag(_finishTag))
                _collidedWithFinishSignalSubj.OnNext(other);

            else if (other.CompareTag(_trapTag))
                _collidedWithTrapSignalSubj.OnNext(other);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (_checkPoints == null) return;

            foreach (var point in _checkPoints)
            {
                Gizmos.DrawSphere(transform.TransformPoint(point), _checkRadius);
            }
        }
    }
}
