using DG.Tweening;
using R3;
using UnityEngine;
using Utils;

namespace UI
{
    public abstract class PopUp : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup _view;

        [Space]

        [SerializeField] private float _initialScale = 0.9f;
        [SerializeField] protected TweenData _openTweenData = 
            new TweenData { Duration = 0.1f, Ease = Ease.OutBack };
        [SerializeField]
        protected TweenData _closeTweenData =
            new TweenData { Duration = 0.1f, Ease = Ease.OutBack };

        private Subject<Unit> _closeSignalSubj = new();
        private Tweener _openTween;

        public Observable<Unit> CloseSignal => _closeSignalSubj;

        public virtual PopUp SetInitialViewState()
        {
            _view.transform.localScale = Vector3.one * _initialScale;
            return this;
        }

        public virtual void Open()
        {
            _openTween = _view.transform
                .DOScale(1, _openTweenData.Duration)
                .SetEase(_openTweenData.Ease);
        }

        public virtual void Close()
        {
            _view.DOFade(0, _closeTweenData.Duration)
                .SetEase(_closeTweenData.Ease)
                .OnComplete(() =>
                {
                    _view.gameObject.SetActive(false);

                    _closeSignalSubj.OnNext(Unit.Default);
                    _closeSignalSubj.OnCompleted();
                });
        }

        public void Destroy()
        {
            _openTween?.Kill();
            Destroy(gameObject);
        }
    }
}