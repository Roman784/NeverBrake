using Cysharp.Threading.Tasks;
using R3;

namespace Gameplay
{
    public class MovementCarBehavior : CarBehavior
    {
        private CompositeDisposable _disposables;

        public MovementCarBehavior(CarBehaviorHandler handler, Car car) : base(handler, car)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _disposables = new CompositeDisposable();

            _car.CollisionRegister.CollidedWithTrapSignal
                .Subscribe(collider => _handler.SetCrashBehavior(collider))
                .AddTo(_disposables);

            _car.View.SetActiveTireTracks(true);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            var shouldJump = _car.Input.ShouldJump();
            if (shouldJump && _car.CollisionRegister.OnGround())
            {
                _handler.SetJumpBehavior();
                return;
            }

            if (_car.CollisionRegister.OnWater())
            {
                _handler.SetDrownBehavior();
                return;
            }
        }

        public override void FixedUpdate(float deltaTime)
        {
            base.Update(deltaTime);

            var horizontalInput = _car.Input.GetHorizontalInput();

            _car.Controller.ApplyMovement(deltaTime);
            _car.Controller.ApplyTurning(horizontalInput, deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
            _disposables.Dispose();
        }
    }
}
