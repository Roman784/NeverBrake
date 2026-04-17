using Gameplay;
using UnityEngine;

namespace CMS
{
    [CreateAssetMenu(fileName = "CarInputsCMS",
                     menuName = "CMS/Cars/New Car Inputs CMS",
                     order = 2)]
    public class CarInputsCMS : ScriptableObject
    {
        public CarKeyboarInput KeyboarInput;
        public CarMobileInput MobileInput;
    }
}
