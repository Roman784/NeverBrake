using DG.Tweening;
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
        private CarInput _input;

        private bool _isLaunched;
        private bool _isCrashed;
        private bool _isFinished;

        public void Initialize(CarInput input)
        {
            _controller = GetComponent<CarController>();
            _collisionRegister = GetComponent<CarCollisionRegister>();
            _input = input;

            _controller.Initialize(_view, _collisionRegister, input);

            _collisionRegister.CollidedWithWallSignal
                .Subscribe(collision => HandleCollisionWithWall(collision))
                .AddTo(gameObject);

            _collisionRegister.CollidedWithTrapSignal
                .Subscribe(collision => HandleCollisionWithTrap(collision))
                .AddTo(gameObject);

            _collisionRegister.CollidedWithWaterSignal
                .Subscribe(collision => HandleCollisionWithWater(collision))
                .AddTo(gameObject);

            _collisionRegister.CollidedWithFinishignal
                .Subscribe(collieder => HandleCollisionWithFinish(collieder))
                .AddTo(gameObject);

            _collisionRegister.Enabled();
        }

        private void FixedUpdate()
        {
            if (_isCrashed || _isFinished) return;

            if (!_isLaunched && 
                _input.ShouldStartMoving() &&
                _collisionRegister.IsAnyPartOnGround())
            {
                _isLaunched = true;
                _controller.Launch();
            }

            _controller.ProcessInput(Time.fixedDeltaTime);
        }

        private void HandleCollisionWithWall(Collision collision)
        {
            if (_isCrashed) return;

            var contact = collision.contacts[0];
            _view.PlayCollisionVFX(contact.point, contact.normal);
        }

        private void HandleCollisionWithTrap(Collision collision)
        {
            if (_isCrashed) return;

            Crash();

            _view.PlayCrashVFX(collision.contacts[0].point)
                    .Subscribe(_ => G.SceneProvider.RestartScene())
                    .AddTo(gameObject);

            G.Camera.Shaker.StrongShake();
            G.Camera.Zoom();
        }

        private void HandleCollisionWithWater(Collision collision)
        {
            if (_isCrashed) return;

            Crash();
            _view.PlayFallingIntoWaterAnimation()
                .OnComplete(() => G.SceneProvider.RestartScene());
            G.Camera.Zoom();
        }

        private void HandleCollisionWithFinish(Collider collider)
        {
            if (_isCrashed) return;
            if (collider.TryGetComponent<FinishPortal>(out var portal))
            {
                _collisionRegister.Disable();
                _controller.Stop();
                _view.PlayPortalSuctionAnimation(portal.CenterPosition)
                    .OnComplete(() => G.SceneProvider.RestartScene());
                G.Camera.Zoom();
                return;
            }
        }

        private void Crash()
        {
            _isCrashed = true;
            _controller.Stop();
            _collisionRegister.Disable();
            _view.SetActiveTireTracks(false);
        }
    }
}
