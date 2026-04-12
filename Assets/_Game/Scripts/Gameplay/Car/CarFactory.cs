using CMS;
using GameRoot;
using UnityEngine;

namespace Gameplay
{
    public static class CarFactory
    {
        public static Car Create(Car prefab, Vector3 position, Quaternion rotation)
        {
            var createdCar = Object.Instantiate(prefab, position, rotation);
            var input = CreateInput();

            createdCar.Initialize(input);

            return createdCar;
        }

        private static CarInput CreateInput()
        {
            var prefab = GetInputPrefab();
            return Object.Instantiate(prefab);
        }

        private static CarInput GetInputPrefab()
        {
            if (Application.isMobilePlatform)
                return G.RootCMS.CarsCMS.InputsCMS.MobileInput;
            return G.RootCMS.CarsCMS.InputsCMS.KeyboarInput;
        }
    }
}
