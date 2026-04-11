using Gameplay;
using System;
using UnityEngine;

namespace CMS
{
    [CreateAssetMenu(fileName = "CarsCMS",
                     menuName = "CMS/Cars/New Cars",
                     order = 4)]
    public class CarsCMS : ScriptableObject
    {
        public CarInputsCMS InputsCMS;

        [Space]

        public CarControllerFeatures ControllerFeatures;

        [Space]

        public CarBinder Prefab;
    }

    [Serializable]
    public class CarControllerFeatures
    {
        public float MovementSpeed;

        [Space]

        public float MinTurningSpeed;
        public float MaxTurningSpeed;
        public float TurningAcceleration;
        public AnimationCurve TurningAccelerationCurve;

        [Space]

        public float Grip;
    }
}
