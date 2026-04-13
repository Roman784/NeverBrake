using UnityEngine;

namespace Gameplay
{
    public class CircularSaw : MonoBehaviour
    {
        [SerializeField] private Transform _saw;
        [SerializeField] private float _rotationSpeed;

        private void Update()
        {
            _saw.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        }
    }
}
