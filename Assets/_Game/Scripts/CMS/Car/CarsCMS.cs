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

        public Car Prefab;
    }
}
