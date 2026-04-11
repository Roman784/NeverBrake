using CMS;
using UnityEngine;

namespace Gameplay
{
    public class CarViewModel
    {
        private readonly float _minTurningSpeed;
        private readonly float _maxTurningSpeed;
        private readonly float _turningAcceleration;
        private readonly AnimationCurve _turningAccelerationCurve;

        private float _grip;

        private float _turningTime;
        private float _turningAngle;
        private int _previousTurningDirection;

        public float MovementSpeed { get; private set; }
        public Vector3 MovementDirection { get; private set; }
        public Vector3 TurningDirection { get; private set; }
        public bool IsCrashed { get; private set; } = false;

        public float TurningSpeed => CalculateTurningSpeed();

        public CarViewModel(
            CarControllerFeatures features)
        {
            MovementSpeed = features.MovementSpeed;
            _minTurningSpeed = features.MinTurningSpeed;
            _maxTurningSpeed = features.MaxTurningSpeed; 
            _turningAcceleration = features.TurningAcceleration;
            _grip = features.Grip;
            _turningAccelerationCurve = features.TurningAccelerationCurve;

            _turningAngle = 90f;
        }

        public void Move(float deltaTime)
        {
            var direction = TurningDirection.normalized;
            MovementDirection = Vector3.Lerp(
                MovementDirection, direction, _grip * deltaTime);
        }

        public void Turn(int direction, float deltaTime)
        {
            direction = Mathf.Clamp(direction, -1, 1);
            if (direction == 0 || _previousTurningDirection != direction) _turningTime = 0;
            else _turningTime += deltaTime;

            _previousTurningDirection = direction;
            _turningAngle -= direction * TurningSpeed * deltaTime;

            var radAngle = _turningAngle * Mathf.Deg2Rad;
            var x = Mathf.Cos(radAngle);
            var z = Mathf.Sin(radAngle);

            TurningDirection = new Vector3(x, 0f, z);
        }

        public void Crush()
        {
            IsCrashed = true;
        }

        private float CalculateTurningSpeed()
        {
            var t = _turningAccelerationCurve.Evaluate(_turningTime * _turningAcceleration);
            return Mathf.Lerp(_minTurningSpeed, _maxTurningSpeed, t);
        }
    }
}
