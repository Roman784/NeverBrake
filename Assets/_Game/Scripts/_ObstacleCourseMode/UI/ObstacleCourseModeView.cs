using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;
using Utils;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _deathCountView;
        [SerializeField] private TMP_Text _bestTimeView;
        [SerializeField] private TMP_Text _currentTimeView;
        [SerializeField] private TMP_Text _penaltyTimeView;

        private RectTransform _penaltyViewRectTransform;
        private Sequence _penaltySeq;

        private Subject<Unit> _pauseButtonPressedSignalSubj = new();
        public Observable<Unit> PauseButtonPressedSignal => _pauseButtonPressedSignalSubj;

        private void Awake()
        {
            _penaltyViewRectTransform = _penaltyTimeView.GetComponent<RectTransform>();
            _penaltyViewRectTransform.localScale = Vector2.zero;
        }

        public void PressPauseButton() => _pauseButtonPressedSignalSubj.OnNext(Unit.Default);

        public void DisplayDeathCount(int count) => _deathCountView.text = count.ToString();
        public void DisplayBestTime(int time) => _bestTimeView.text = time.ToTimeFormat();
        public void DisplayCurrentTime(int time) => _currentTimeView.text = time.ToTimeFormat();

        public void DisplayTimerPenalty(int value)
        {
            _penaltySeq?.Kill();

            _penaltyTimeView.text = "+" + value.ToTimeFormat();
            _penaltyViewRectTransform.anchoredPosition = new Vector2(0, -80);
            _penaltyViewRectTransform.localScale = Vector2.zero;

            _penaltySeq = DOTween.Sequence();
            _penaltySeq.Append(_penaltyTimeView.DOFade(1, 0));
            _penaltySeq.Append(_penaltyViewRectTransform.DOScale(1, 0.15f).SetEase(Ease.OutBack));
            //_penaltySeq.AppendInterval(0.15f);
            _penaltySeq.Append(_penaltyViewRectTransform.DOAnchorPosY(-32, 0.25f).SetEase(Ease.InBack));
            _penaltySeq.Join(_penaltyTimeView.DOFade(0, 0.35f).SetEase(Ease.InQuad));
        }
    }
}
