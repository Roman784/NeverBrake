using DG.Tweening;
using R3;
using UnityEngine;
using Utils;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _view;

        [Space]

        [SerializeField] private TweenData _showingData;
        [SerializeField] private TweenData _hidingData;

        public Observable<Unit> Show()
        {
            var onCompleted = new Subject<Unit>();

            _view.gameObject.SetActive(true);
            _view.DOFade(1f, _showingData.Duration)
                .SetEase(_showingData.Ease)
                .OnComplete(() => onCompleted.OnNext(Unit.Default));

            return onCompleted;
        }

        public Observable<Unit> Hide()
        {
            var onCompleted = new Subject<Unit>();

            _view.DOFade(0f, _hidingData.Duration)
                .SetEase(_hidingData.Ease)
                .OnComplete(() =>
                {
                    onCompleted.OnNext(Unit.Default);
                    _view.gameObject.SetActive(false);
                });

            return onCompleted;
        }
    }
}