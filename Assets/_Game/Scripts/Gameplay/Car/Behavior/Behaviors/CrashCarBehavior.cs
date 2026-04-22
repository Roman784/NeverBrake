using GameRoot;
using R3;
using System;
using UnityEngine;

namespace Gameplay
{
    public class CrashCarBehavior : CarBehavior
    {
        private Collider2D _collider;
        private IDisposable _crashVFXCompletedSignal;

        public CrashCarBehavior(CarBehaviorHandler handler, Car car) : base(handler, car)
        {
        }

        public void SetParams(Collider2D collider)
        {
            _collider = collider;
        }

        public override void Enter()
        {
            _car.Controller.Stop();
            _car.CollisionRegister.DisableRegistration();
            _car.View.SetActiveTireTracks(false);
            _car.View.PlayCrashVFX(_car.transform.position);

            G.Camera.Shaker.StrongShake();
            G.Camera.Zoom();

            _car.OnFailed();
        }

        public override void Exit()
        {
            base.Exit();

            _crashVFXCompletedSignal?.Dispose();
        }
    }
}
