using R3;
using UnityEngine;
using UnityEngine.UI;

namespace CustomizationMenu
{
    [RequireComponent(typeof(RectTransform))]
    public class CarPreviewItem : MonoBehaviour
    {
        [SerializeField] private Image _iconView;
        [SerializeField] private GameObject _fadeView;

        private RectTransform _rectTransform;
        private Subject<Unit> _pressedSignalSubj = new();

        public RectTransform RectTransform => _rectTransform;
        public Observable<Unit> PressedSignal => _pressedSignalSubj;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Press()
        {
            _pressedSignalSubj.OnNext(Unit.Default);
        }

        public void SetIcon(Sprite sprite)
        {
            _iconView.sprite = sprite;
        }

        public void SetLock(bool isLocked)
        {
            _fadeView.SetActive(isLocked);
        }

        public void SetTransform(float scale, float positionY)
        {
            transform.localScale = Vector3.one * scale;
            _iconView.transform.localPosition = 
                new Vector3(_iconView.transform.localPosition.x, positionY);
        }
    }
}
