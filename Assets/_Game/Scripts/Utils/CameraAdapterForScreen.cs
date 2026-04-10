using System.Collections;
using UnityEngine;

namespace Utils
{
    public class CameraAdapterForScreen : MonoBehaviour
    {
        [SerializeField] private Vector2 _referenceResolution = new Vector2(1080, 1920);
        [SerializeField] private float _referenceSize = 10f;
        [SerializeField, Range(0f, 1f)] private float _match = 0f;

        private Camera _camera;
        private bool _isUpdate;

        private IEnumerator Start()
        {
            _camera = Camera.main;
            _isUpdate = true;

            while (_isUpdate)
            {
                UpdateCameraSize();
                yield return new WaitForSeconds(1);
            }
        }

        private void OnDestroy()
        {
            _isUpdate = false;
        }

        private void UpdateCameraSize()
        {
            var currentAspect = (float)Screen.width / Screen.height;
            if (currentAspect >= 1) return;

            var referenceAspect = _referenceResolution.x / _referenceResolution.y;
            var difference =  currentAspect / referenceAspect;
            var sizeMultiplier = Mathf.Lerp(difference, 1f / difference, _match);
            
            _camera.orthographicSize = _referenceSize * sizeMultiplier;
        }
    }
}
