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
        private CarBehaviorHandler _behaviorHandler;

        private Subject<Unit> _failedSignalSubj = new();
        private Subject<Unit> _finishReachedSignalSubj = new();

        public CarView View => _view;
        public CarController Controller => _controller;
        public CarCollisionRegister CollisionRegister => _collisionRegister;
        public CarInput Input => _input;

        public Observable<Unit> FailedSignal => _failedSignalSubj;
        public Observable<Unit> FinishReachedSignal => _finishReachedSignalSubj;

        public void Initialize(CarInput input)
        {
            _controller = GetComponent<CarController>();
            _collisionRegister = GetComponent<CarCollisionRegister>();
            _input = input;

            _behaviorHandler = new CarBehaviorHandler(this);

            _collisionRegister.EnableRegistration();
            _behaviorHandler.SetIdleBehaviour();
        }

        public void StartMovement()
        {
            _behaviorHandler.SetMovementBehavior();
        }

        public void OnFailed()
        {
            _failedSignalSubj.OnNext(Unit.Default);
            _failedSignalSubj.OnCompleted();
        }

        public void OnFinishReached()
        {
            _finishReachedSignalSubj.OnNext(Unit.Default);
            _finishReachedSignalSubj.OnCompleted();
        }

        private void Update()
        {
            _behaviorHandler?.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _behaviorHandler?.FixedUpdate(Time.fixedDeltaTime);
        }

        private void OnDestroy()
        {
            _behaviorHandler?.Dispose();
        }
    }
}
