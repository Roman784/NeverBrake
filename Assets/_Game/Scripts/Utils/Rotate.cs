using UnityEngine;

namespace Utils
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private Vector3 _eulers;

        private void Update()
        {
           transform.Rotate(_eulers);
        }
    }
}