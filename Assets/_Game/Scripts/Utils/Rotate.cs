using UnityEngine;

namespace Utils
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private Vector3 _eulers;

        private void Update()
        {
            var distortion = Mathf.Lerp(0.75f, 1f, Mathf.Sin(Time.time / 2f));
            transform.Rotate(_eulers * distortion * Time.deltaTime);
        }
    }
}