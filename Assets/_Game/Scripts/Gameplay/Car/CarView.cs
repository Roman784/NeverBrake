using UnityEngine;

namespace Gameplay
{
    public class CarView : MonoBehaviour
    {
        [SerializeField] private Transform[] _wheels;

        public void ApplyWheelsTurning(float angle)
        {
            foreach (var wheel in _wheels)
            {
                wheel.localRotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }
}
