using GameRoot;
using UnityEngine;

namespace Gameplay
{
    public class CarInputFactory
    {
        public CarInput Create()
        {
            var prefab = GetPrefab();
            var cteatedInput = Object.Instantiate(prefab);
            return cteatedInput;
        }

        private CarInput GetPrefab()
        {
            if (Application.isMobilePlatform)
                return G.RootCMS.CarsCMS.InputsCMS.MobileInput;
            return G.RootCMS.CarsCMS.InputsCMS.KeyboarInput;
        }
    }
}
