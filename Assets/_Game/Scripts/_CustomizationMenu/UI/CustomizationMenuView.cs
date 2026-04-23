using Currency;
using DG.Tweening;
using NUnit.Framework.Internal.Commands;
using R3;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace CustomizationMenu
{
    public class CustomizationMenuView : MonoBehaviour
    {
        [SerializeField] private WalletView _walletView;

        [Space]

        [SerializeField] private RectTransform _rootCanvas;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GridLayoutGroup _contentGrid;
        [SerializeField] private PointerDetector _pointerDetector;

        [Space]

        [SerializeField] private CanvasGroup _selectButtonView;

        private const float SNAP_VELOCITY_THRESHOLD = 200f;
        private const float SNAP_DURATION = 0.25f;

        private const float SCALE_MIN = 0.5f;
        private const float SCALE_MAX = 1f;

        private const float POSITION_Y_MIN = -128f;
        private const float POSITION_Y_MAX = 0f;

        private const float DISTANCE_NORMALIZATION = 32f;

        private readonly List<SkinPreview> _previews = new();

        private Tween _snapTween;
        private bool _isSnapping;

        private readonly Subject<Unit> _scrollStartedSignalSubj = new();
        private readonly Subject<Unit> _scrollEndedSignalSubj = new();
        private readonly Subject<Unit> _selectButtonPressedSignalSubj = new();
        private readonly Subject<Unit> _gachaButtonPressedSignalSubj = new();
        private readonly Subject<Unit> _settingsButtonPressedSignalSubj = new();

        public Observable<Unit> ScrollStartedSignal => _scrollStartedSignalSubj;
        public Observable<Unit> ScrollEndedSignal => _scrollEndedSignalSubj;
        public Observable<Unit> SelectButtonPressedSignal => _selectButtonPressedSignalSubj;
        public Observable<Unit> GachaButtonPressedSignal => _gachaButtonPressedSignalSubj;
        public Observable<Unit> SettingsButtonPressedSignal => _selectButtonPressedSignalSubj;

        private void Awake()
        {
            ClearContent();
            SetupPadding();

            _pointerDetector.OnPointerDownSignal
                .Subscribe(_ =>
                {
                    StopSnap();
                    _scrollStartedSignalSubj.OnNext(Unit.Default);
                });

            _pointerDetector.OnPointerUpSignal
                .Subscribe(_ => TrySnapToNearest());
        }

        public void PressSelectButton() => _selectButtonPressedSignalSubj.OnNext(Unit.Default);
        public void PressGachaButton() => _gachaButtonPressedSignalSubj.OnNext(Unit.Default);
        public void PressSettingsButton() => _settingsButtonPressedSignalSubj.OnNext(Unit.Default);

        public void BindWallet(Wallet wallet)
        {
            _walletView.Bind(wallet);
        }

        public void OnScrollChanged(Vector2 _)
        {
            UpdatePreviewTransforms();
            TrySnapToNearest();
        }

        public SkinPreview CreateCarPreview(SkinPreview prefab)
        {
            var item = Instantiate(prefab, _contentGrid.transform);
            _previews.Add(item);

            UpdatePreviewTransforms();
            return item;
        }

        public void ScrollTo(SkinPreview preview, bool instant = true)
        {
            var index = _previews.IndexOf(preview);

            if (instant)
            {
                var position = CalculateSnapPosition(index);
                _scrollRect.content.anchoredPosition = new Vector2(position, 0f);
                return;
            }

            SnapTo(index);
        }

        public SkinPreview GetSelectedPreview()
        {
            return _previews[GetNearestPreviewIndex()];
        }

        // ==================== Snap ====================

        private void TrySnapToNearest()
        {
            if (_isSnapping) return;
            if (!_pointerDetector.IsPointerUp) return;
            if (Mathf.Abs(_scrollRect.velocity.x) >= SNAP_VELOCITY_THRESHOLD) return;

            int index = GetNearestPreviewIndex();
            SnapTo(index);
        }

        private void SnapTo(int index)
        {
            _isSnapping = true;
            _scrollRect.velocity = Vector2.zero;

            float targetX = CalculateSnapPosition(index);

            _snapTween?.Kill(false);
            _snapTween = _scrollRect.content
                .DOAnchorPosX(targetX, SNAP_DURATION)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => StopSnap());
        }

        private void StopSnap()
        {
            _isSnapping = false;
            _snapTween?.Kill(false);
            _scrollEndedSignalSubj.OnNext(Unit.Default);
        }

        // ==================== Layout ====================

        private void SetupPadding()
        {
            int halfWidth = Mathf.FloorToInt(_rootCanvas.sizeDelta.x / 2f);

            _contentGrid.padding.left = halfWidth;
            _contentGrid.padding.right = halfWidth;
        }

        private float CalculateSnapPosition(int index)
        {
            float totalWidth =
                _contentGrid.padding.left +
                _contentGrid.padding.right +
                _contentGrid.cellSize.x * _previews.Count +
                _contentGrid.spacing.x * (_previews.Count - 1);

            float itemCenter =
                _contentGrid.padding.left +
                _contentGrid.cellSize.x * (index + 0.5f) +
                _contentGrid.spacing.x * index;

            return (totalWidth / 2f) - itemCenter;
        }

        // ==================== Items ====================

        private void UpdatePreviewTransforms()
        {
            Canvas.ForceUpdateCanvases();

            foreach (var preview in _previews)
            {
                float distance = Mathf.Abs(preview.transform.position.x);
                float t = Mathf.Clamp01(1f - distance / DISTANCE_NORMALIZATION);

                float scale = Mathf.Lerp(SCALE_MIN, SCALE_MAX, t);
                float y = Mathf.Lerp(POSITION_Y_MIN, POSITION_Y_MAX, t);

                preview.transform.localScale = Vector3.one * scale;
                preview.SetRootYPosition(y);
            }
        }

        private int GetNearestPreviewIndex()
        {
            int index = 0;
            float minDistance = float.MaxValue;

            for (int i = 0; i < _previews.Count; i++)
            {
                float distance = Mathf.Abs(_previews[i].transform.position.x);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    index = i;
                }
            }

            return index;
        }

        // ==================== Cleanup ====================

        private void ClearContent()
        {
            foreach (Transform child in _contentGrid.transform)
            {
                if (child.GetComponent<SkinPreview>() != null)
                    Destroy(child.gameObject);
            }
        }
    }
}
