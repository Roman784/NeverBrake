using DG.Tweening;
using R3;
using UnityEngine;

namespace UI
{
    public abstract class Toast : MonoBehaviour
    {
        [SerializeField] private RectTransform _rootView;
        [SerializeField] private CanvasGroup _rootCanvasGroup;

        private Subject<Toast> _closeSignalSubj = new();
        private Sequence _openingSeq;

        public Observable<Toast> CloseSignal => _closeSignalSubj;

        public void SetInitialViewState()
        {
            var halfScreenWidth = Screen.width / 2;
            _rootView.anchoredPosition = new Vector2(-halfScreenWidth, 0);
            _rootCanvasGroup.alpha = 0f;
        }

        public virtual void Open()
        {
            _openingSeq = DOTween.Sequence();
            _openingSeq.Append(_rootView.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutExpo));
            _openingSeq.Join(_rootCanvasGroup.DOFade(1, 0.5f).SetEase(Ease.OutCubic));
        }

        public virtual void Close()
        {
            _rootCanvasGroup.interactable = false;

            _openingSeq?.Kill(true);
            _rootCanvasGroup.DOFade(0f, 0.25f).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _closeSignalSubj.OnNext(this);
                    _closeSignalSubj.OnCompleted();
                });
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _openingSeq?.Kill(true);
            _rootCanvasGroup.DOKill();
        }
    }
}
