using UnityEngine;

namespace Utils
{
    public class FaceToCamera : MonoBehaviour
    {
        private void Start()
        {
            Rotate();
        }

        [ContextMenu("Rotate")]
        private void Rotate()
        {
            var direction = Camera.main.transform.position - transform.position;
            transform.LookAt(transform.position - direction);
        }
    }
}
