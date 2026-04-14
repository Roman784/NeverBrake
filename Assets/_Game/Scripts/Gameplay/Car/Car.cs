using GameRoot;
using R3;
using UnityEngine;
using Utils;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

namespace Gameplay
{
    [RequireComponent(typeof(CarController))]
    [RequireComponent(typeof(CarCollisionRegister))]
    public class Car : MonoBehaviour
    {
        [SerializeField] private CarView _view;

        [Space]

        [SerializeField] private LayerMask _crashObstacleMask;
        
        private CarController _controller;
        private CarCollisionRegister _collisionRegister;

        private bool _isCrashed;

        public void Initialize(CarInput input)
        {
            _controller = GetComponent<CarController>();
            _collisionRegister = GetComponent<CarCollisionRegister>();

            _controller.Initialize(_view, _collisionRegister, input);

            _collisionRegister.CollidedSignal
                .Subscribe(collision => HandleCollision(collision))
                .AddTo(gameObject);
        }

        private void FixedUpdate()
        {
            if (_isCrashed) return;

            _controller.ProcessInput(Time.fixedDeltaTime);
        }

        private void HandleCollision(Collision collision)
        {
            if (_isCrashed) return;

            var contact = collision.contacts[0];

            if (_crashObstacleMask.Contains(collision.collider.gameObject.layer))
            {
                _view.PlayCrashVFX(contact.point);
                G.Camera.Shaker.StrongShake();
                Crash();
                return;
            }
            else if (collision.collider.TryGetComponent<FinishPortal>(out var portal))
            {
                portal.GetComponent<Collider>().enabled = false;
                _controller.Stop();
                _view.PlayPortalSuctionAnimation(portal.CenterPosition);
                return;
            }

            _view.PlayCollisionVFX(contact.point, contact.normal);
        }

        private void Crash()
        {
            _controller.Stop();
            _isCrashed = true;
        }
    }
}
