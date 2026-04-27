using DG.Tweening;
using R3;
using UnityEngine;
using Utils;

namespace UI
{
    public abstract class PopUp : MonoBehaviour
    {
        [SerializeField] protected RectTransform _rootView;

        private Subject<PopUp> _openSignalSubj = new();
        private Subject<PopUp> _closeSignalSubj = new();

        private Tweener _showTween;

        public Observable<PopUp> OpenSignal => _openSignalSubj;
        public Observable<PopUp> CloseSignal => _closeSignalSubj;

        public virtual PopUp SetInitialViewState()
        {
            _rootView.pivot = new Vector2(0.5f, 1f);
            _rootView.anchoredPosition = Vector2.zero;
            return this;
        }

        public void Open()
        {
            Show().OnComplete(() =>
            {
                _openSignalSubj.OnNext(this);
                _openSignalSubj.OnCompleted();
            });
        }

        public void Close()
        {
            Hide().OnComplete(() =>
            {
                _closeSignalSubj.OnNext(this);
                _closeSignalSubj.OnCompleted();
            });
        }

        public virtual Tween Show()
        {
            _showTween = _rootView
                .DOPivotY(0f, 0.4f)
                .SetEase(Ease.OutExpo);
            return _showTween;
        }

        public virtual Tween Hide()
        {
            return _rootView
                .DOPivotY(1f, 0.2f)
                .SetEase(Ease.OutQuad);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _showTween?.Kill();
            _rootView.DOKill();
        }
    }
}