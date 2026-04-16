using GameRoot;
using R3;
using System;
using UnityEngine;

namespace Gameplay
{
    public class CrashCarBehavior : CarBehavior
    {
        private Collision _collision;
        private IDisposable _crashVFXCompletedSignal;

        public CrashCarBehavior(CarBehaviorHandler handler, Car car) : base(handler, car)
        {
        }

        public void SetParams(Collision collision)
        {
            _collision = collision;
        }

        public override void Enter()
        {
            _car.Controller.Stop();
            _car.CollisionRegister.Disable();
            _car.View.SetActiveTireTracks(false);

            _crashVFXCompletedSignal = 
                _car.View.PlayCrashVFX(_collision.contacts[0].point)
                    .Subscribe(_ => G.SceneProvider.RestartScene());

            G.Camera.Shaker.StrongShake();
            G.Camera.Zoom();
        }

        public override void Exit()
        {
            base.Exit();

            _crashVFXCompletedSignal?.Dispose();
        }
    }
}
