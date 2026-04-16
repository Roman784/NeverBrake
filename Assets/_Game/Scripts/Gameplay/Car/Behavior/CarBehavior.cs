using GameRoot;
using R3;
using UnityEngine;

namespace Gameplay
{
    public abstract class CarBehavior
    {
        protected readonly CarBehaviorHandler _handler;
        protected readonly Car _car;

        private CompositeDisposable _disposables;

        public CarBehavior(CarBehaviorHandler handler, Car car)
        {
            _handler = handler;
            _car = car;
        }

        public virtual void Enter() 
        {
            _disposables = new CompositeDisposable();

            _car.CollisionRegister.CollidedWithWallSignal
                .Subscribe(collision => HandleCollisionWithWall(collision))
                .AddTo(_disposables);

            _car.CollisionRegister.CollidedWithTrapSignal
                .Subscribe(collision => _handler.SetCrashBehavior(collision))
                .AddTo(_disposables);

            //_car.CollisionRegister.CollidedWithWaterSignal
            //    .Subscribe(collision => _handler.SetDrownBehavior())
            //    .AddTo(_disposables);

            _car.CollisionRegister.CollidedWithFinishignal
                .Subscribe(collieder => _handler.SetFinishBehavior(collieder))
                .AddTo(_disposables);
        }

        public virtual void Update(float deltaTime)
        {
        }

        public virtual void FixedUpdate(float deltaTime) 
        {
        }

        public virtual void Exit() 
        {
            _disposables?.Dispose();
        }

        private void HandleCollisionWithWall(Collision collision)
        {
            var contact = collision.contacts[0];
            _car.View.PlayCollisionVFX(contact.point, contact.normal);
        }
    }
}