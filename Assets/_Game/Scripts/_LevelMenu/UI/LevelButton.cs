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

        [SerializeField] private GameObject _statsView;
        [SerializeField] private TMP_Text _bestScoreView;
        [SerializeField] private TMP_Text _deathCountView;

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

        public void SetStats(bool active, string bestScore = "", string deathCount = "")
        {
            _bestScoreView.text = bestScore;
            _deathCountView.text = deathCount;

            var bestScoreTransform = _bestScoreView.GetComponent<RectTransform>();
            var deathCountTransform = _deathCountView.GetComponent<RectTransform>();

            bestScoreTransform.sizeDelta = new Vector2(_bestScoreView.preferredWidth, bestScoreTransform.sizeDelta.y);
            deathCountTransform.sizeDelta = new Vector2(_deathCountView.preferredWidth, deathCountTransform.sizeDelta.y);

            _statsView.SetActive(active);
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
