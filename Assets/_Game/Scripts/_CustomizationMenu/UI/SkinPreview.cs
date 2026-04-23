using R3;
using UnityEngine;
using UnityEngine.UI;

namespace CustomizationMenu
{
    [RequireComponent(typeof(RectTransform))]
    public class SkinPreview: MonoBehaviour
    {
        [SerializeField] private RectTransform _root;
        [SerializeField] private Image _fadeView;
        [SerializeField] private Color _fadeColor;

        private RectTransform _rectTransform;
        private Subject<Unit> _pressedSignalSubj = new();

        public RectTransform RectTransform => _rectTransform;
        public Observable<Unit> PressedSignal => _pressedSignalSubj;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _fadeView.color = _fadeColor;
        }

        public void Press()
        {
            _pressedSignalSubj.OnNext(Unit.Default);
        }

        public void SetLock(bool isLocked)
        {
            _fadeView.gameObject.SetActive(isLocked);
        }

        public void SetRootYPosition(float positionY)
        {
            _root.transform.localPosition = 
                new Vector3(_root.transform.localPosition.x, positionY);
        }
    }
}
