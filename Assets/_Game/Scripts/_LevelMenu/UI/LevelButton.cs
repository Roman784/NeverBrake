using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;

namespace LevelMenu
{
    [RequireComponent(typeof(RectTransform))]
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _numberView;
        [SerializeField] private GameObject _mainBackgroundView;
        [SerializeField] private GameObject _portalBackgroundView;
        [SerializeField] private GameObject _fadeView;

        private Subject<Unit> _pressedSignalSubj = new();
        private RectTransform _rectTransform;

        public Observable<Unit> PressedSignal => _pressedSignalSubj;
        public RectTransform RectTransform => _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Press()
        {
            _pressedSignalSubj.OnNext(Unit.Default);
        }

        public void SetNumber(int number)
        {
            _numberView.text = number.ToString();
        }

        public void Lock()
        {
            _fadeView.SetActive(true);
        }

        public void Unlock()
        {
            _fadeView.SetActive(false);
        }

        public void Select()
        {
            _mainBackgroundView.SetActive(false);
            _portalBackgroundView.SetActive(true);
        }

        public void Deselect()
        {
            _mainBackgroundView.SetActive(true);
            _portalBackgroundView.SetActive(false);
        }
    }
}
