using Gameplay;
using UnityEngine;

namespace CMS
{
    [CreateAssetMenu(fileName = "CarInputsCMS",
                     menuName = "CMS/Cars/New Car Inputs",
                     order = 1)]
    public class CarInputsCMS : ScriptableObject
    {
        public CarKeyboarInput KeyboarInput;
        public CarMobileInput MobileInput;
    }
}
