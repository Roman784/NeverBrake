using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class CarBehaviorHandler : IDisposable
    {
        private readonly Car _car;

        private Dictionary<Type, CarBehavior> _behaviorsMap;
        private CarBehavior _currentBehavior;

        public CarBehaviorHandler(Car car)
        {
            _car = car;
            InitBehaviorsMap();
        }

        private void InitBehaviorsMap()
        {
            _behaviorsMap = new();

            _behaviorsMap[typeof(IdleCarBehavior)] = new IdleCarBehavior(this, _car);
            _behaviorsMap[typeof(MovementCarBehavior)] = new MovementCarBehavior(this, _car);
            _behaviorsMap[typeof(JumpCarBehavior)] = new JumpCarBehavior(this, _car);
            _behaviorsMap[typeof(DrownCarBehavior)] = new DrownCarBehavior(this, _car);
            _behaviorsMap[typeof(CrashCarBehavior)] = new CrashCarBehavior(this, _car);
            _behaviorsMap[typeof(FinishCarBehavior)] = new FinishCarBehavior(this, _car);
        }

        public void Update(float deltaTime)
        {
            _currentBehavior?.Update(deltaTime);
        }

        public void FixedUpdate(float deltaTime)
        {
            _currentBehavior?.FixedUpdate(deltaTime);
        }

        public void Dispose()
        {
            _currentBehavior?.Exit();
        }

        public void SetIdleBehaviour()
        {
            var behavior = GetBehavior<IdleCarBehavior>();
            SetBehavior(behavior);
        }

        public void SetMovementBehavior()
        {
            var behavior = GetBehavior<MovementCarBehavior>();
            SetBehavior(behavior);
        }

        public void SetJumpBehavior()
        {
            var behavior = GetBehavior<JumpCarBehavior>();
            SetBehavior(behavior);
        }

        public void SetDrownBehavior()
        {
            var behavior = GetBehavior<DrownCarBehavior>();
            SetBehavior(behavior);
        }

        public void SetCrashBehavior()
        {
            var behavior = GetBehavior<CrashCarBehavior>();
            SetBehavior(behavior);
        }

        public void SetFinishBehavior(Collider2D collider)
        {
            var behavior = GetBehavior<FinishCarBehavior>();
            behavior.SetParams(collider);
            SetBehavior(behavior);
        }

        private void SetBehavior(CarBehavior behavior)
        {
            _currentBehavior?.Exit();

            _currentBehavior = behavior;
            _currentBehavior.Enter();
        }

        private T GetBehavior<T>() where T : CarBehavior
        {
            return (T)_behaviorsMap[typeof(T)];
        }
    }
}