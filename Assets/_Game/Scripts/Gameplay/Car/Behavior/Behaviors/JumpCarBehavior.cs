using R3;
using System;

namespace Gameplay
{
    public class JumpCarBehavior : CarBehavior
    {
        private IDisposable _jumpCompletedSignal;

        public JumpCarBehavior(CarBehaviorHandler handler, Car car) : base(handler, car)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _car.View.SetActiveTireTracks(false);

            _jumpCompletedSignal =_car.Controller.Jump()
                .Subscribe(_ =>
                {
                    _car.View.PlayLandingAnimation();

                    _handler.SetMovementBehavior();
                    return;
                });

            _car.Controller.ApplyBoostImpulse();

            _car.View.PlayBoostAnimation();
            _car.View.PlayBoostVFX();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            var horizontalInput = _car.Input.GetHorizontalInput();
            _car.Controller.ApplyTurning(horizontalInput, deltaTime);
            _car.View.ApplyWheelsTurning(
                horizontalInput, _car.Controller.TurnInputDuration, deltaTime);
        }

        public override void Exit()
        {
            base.Exit();

            _jumpCompletedSignal.Dispose();
        }
    }
}
