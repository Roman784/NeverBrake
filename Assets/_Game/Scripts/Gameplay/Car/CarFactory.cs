using CMS;
using GameRoot;
using UnityEngine;

namespace Gameplay
{
    public static class CarFactory
    {
        private static CarsCMS CarsCMS => G.RootCMS.CarsCMS;

        public static CarBinder Create(CarBinder prefab, Vector3 position, Quaternion rotation)
        {
            var binder = Object.Instantiate(prefab, position, rotation);
            var viewModel = new CarViewModel(
                features: CarsCMS.ControllerFeatures);
            var input = CreateInput();

            binder.Init(viewModel, input);

            return binder;
        }

        private static CarInput CreateInput()
        {
            var prefab = GetInputPrefab();
            var cteatedInput = Object.Instantiate(prefab);
            return cteatedInput;
        }

        private static CarInput GetInputPrefab()
        {
            if (Application.isMobilePlatform)
                return G.RootCMS.CarsCMS.InputsCMS.MobileInput;
            return G.RootCMS.CarsCMS.InputsCMS.KeyboarInput;
        }
    }
}
