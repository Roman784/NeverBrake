using DG.Tweening;
using R3;
using UnityEngine;
using Utils;

namespace UI
{
    public abstract class PopUp : MonoBehaviour
    {
        [SerializeField] protected RectTransform _rootView;

        private Subject<Unit> _closeSignalSubj = new();
        private Tweener _openTween;

        public Observable<Unit> CloseSignal => _closeSignalSubj;

        public virtual PopUp SetInitialViewState()
        {
            _rootView.pivot = new Vector2(0.5f, 1f);
            _rootView.anchoredPosition = Vector2.zero;
            return this;
        }

        public virtual void Open()
        {
            _openTween = _rootView
                .DOPivotY(0f, 0.4f)
                .SetEase(Ease.OutExpo);
        }

        public virtual void Close()
        {
            _rootView
                .DOPivotY(1f, 0.2f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _closeSignalSubj.OnNext(Unit.Default);
                    _closeSignalSubj.OnCompleted();
                });
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _openTween?.Kill();
            _rootView.DOKill();
        }
    }
}