using Currency;
using DG.Tweening;
using R3;
using System.Collections.Generic;
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
        [SerializeField] private CarPreviewItem _itemPrefab;

        [Space]

        [SerializeField] private CanvasGroup _selectButtonView;

        private const float SNAP_VELOCITY_THRESHOLD = 200f;
        private const float SNAP_DURATION = 0.25f;

        private const float SCALE_MIN = 0.5f;
        private const float SCALE_MAX = 1f;

        private const float POSITION_Y_MIN = -128f;
        private const float POSITION_Y_MAX = 0f;

        private const float DISTANCE_NORMALIZATION = 32f;

        private readonly List<CarPreviewItem> _items = new();

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
            UpdateItemTransforms();
            TrySnapToNearest();
        }

        public CarPreviewItem CreateItem()
        {
            var item = Instantiate(_itemPrefab, _contentGrid.transform);
            _items.Add(item);

            UpdateItemTransforms();
            return item;
        }

        public void ScrollTo(CarPreviewItem item, bool instant = true)
        {
            var index = _items.IndexOf(item);

            if (instant)
            {
                var position = CalculateSnapPosition(index);
                _scrollRect.content.anchoredPosition = new Vector2(position, 0f);
                return;
            }

            SnapTo(index);
        }

        public CarPreviewItem GetSelectedItem()
        {
            return _items[GetNearestItemIndex()];
        }

        // ==================== Snap ====================

        private void TrySnapToNearest()
        {
            if (_isSnapping) return;
            if (!_pointerDetector.IsPointerUp) return;
            if (Mathf.Abs(_scrollRect.velocity.x) >= SNAP_VELOCITY_THRESHOLD) return;

            int index = GetNearestItemIndex();
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
                _contentGrid.cellSize.x * _items.Count +
                _contentGrid.spacing.x * (_items.Count - 1);

            float itemCenter =
                _contentGrid.padding.left +
                _contentGrid.cellSize.x * (index + 0.5f) +
                _contentGrid.spacing.x * index;

            return (totalWidth / 2f) - itemCenter;
        }

        // ==================== Items ====================

        private void UpdateItemTransforms()
        {
            Canvas.ForceUpdateCanvases();

            foreach (var item in _items)
            {
                float distance = Mathf.Abs(item.transform.position.x);
                float t = Mathf.Clamp01(1f - distance / DISTANCE_NORMALIZATION);

                float scale = Mathf.Lerp(SCALE_MIN, SCALE_MAX, t);
                float y = Mathf.Lerp(POSITION_Y_MIN, POSITION_Y_MAX, t);

                item.SetTransform(scale, y);
            }
        }

        private int GetNearestItemIndex()
        {
            int index = 0;
            float minDistance = float.MaxValue;

            for (int i = 0; i < _items.Count; i++)
            {
                float distance = Mathf.Abs(_items[i].transform.position.x);

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
                if (child.GetComponent<CarPreviewItem>() != null)
                    Destroy(child.gameObject);
            }
        }
    }
}
