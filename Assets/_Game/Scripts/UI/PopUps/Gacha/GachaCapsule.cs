using DG.Tweening;
using R3;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class GachaCapsule : MonoBehaviour
    {
        [Serializable]
        private class MaterialByRarity
        {
            public Rarity Rarity;
            public Material Material;
        }

        [SerializeField] private RawImage _bodyView;
        [SerializeField] private RawImage _coverView;
        [SerializeField] private Image _shadowView;
        [SerializeField] private Image _lightRaysView;

        [Space]

        [SerializeField] private RawImage[] _fadeViews;
        [SerializeField] private float _maxFade;

        [Space]

        [SerializeField] private MaterialByRarity[] _materialsMap;

        [Space]

        [SerializeField] private Image _rewardView;

        private RectTransform _rectTransform;
        private Subject<Unit> _pressedSignalSubj = new();
        private Subject<Unit> _openedSignalSubj = new();

        public RectTransform RectTransform => _rectTransform;
        public Observable<Unit> PressedSignal => _pressedSignalSubj;
        public Observable<Unit> OpenedSignal => _openedSignalSubj;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rewardView.gameObject.SetActive(false);
        }

        public void Press() => _pressedSignalSubj.OnNext(Unit.Default);

        public void SetRarity(Rarity rarity)
        {
            foreach (var matByRarity in _materialsMap)
            {
                if (matByRarity.Rarity != rarity) continue;

                _bodyView.material = matByRarity.Material;
                _coverView.material = matByRarity.Material;

                break;
            }
        }

        public void SetReward(Sprite sprite)
        {
            _rewardView.sprite = sprite;
            _rewardView.SetNativeSize();
        }

        public void SetFade(float t)
        {
            foreach (var view in _fadeViews)
            {
                var color = view.color;
                color.a = Mathf.Lerp(0, _maxFade, t);
                view.color = color;
            }
        }

        public void Open()
        {
            transform.DOKill();
            Sequence seq = DOTween.Sequence();

            seq.Append(transform.DOScale(1.2f, 2f));
            seq.Join(transform.DOShakePosition(2f, new Vector3(1, 1, 0) * 10f, 10));

            seq.Append(transform.DOScale(1.5f, 0.25f).SetEase(Ease.OutBack));
            seq.Join(_coverView.rectTransform.DOAnchorPos(Vector2.up * 200f, 0.1f).SetEase(Ease.OutQuad));
            seq.Join(_bodyView.rectTransform.DOAnchorPos(Vector2.down * 200f, 0.1f).SetEase(Ease.OutQuad));
            seq.Join(_shadowView.rectTransform.DOAnchorPos(Vector2.down * 300f, 0.1f).SetEase(Ease.OutQuad));
            seq.Join(_lightRaysView.DOFade(1, 0.1f).SetEase(Ease.OutQuad));

            seq.JoinCallback(() =>
            {
                _rewardView.gameObject.SetActive(true);

                _openedSignalSubj.OnNext(Unit.Default);
                _openedSignalSubj.OnCompleted();
            });
        }
    }
}