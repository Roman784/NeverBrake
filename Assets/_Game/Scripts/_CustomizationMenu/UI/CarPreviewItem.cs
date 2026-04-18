using UnityEngine;
using UnityEngine.UI;

namespace CustomizationMenu
{
    [RequireComponent(typeof(RectTransform))]
    public class CarPreviewItem : MonoBehaviour
    {
        [SerializeField] private Image _iconView;

        private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetIcon(Sprite sprite)
        {
            _iconView.sprite = sprite;
        }

        public void SetTransform(float scale, float positionY)
        {
            transform.localScale = Vector3.one * scale;
            _iconView.transform.localPosition = 
                new Vector3(_iconView.transform.localPosition.x, positionY);
        }
    }
}
