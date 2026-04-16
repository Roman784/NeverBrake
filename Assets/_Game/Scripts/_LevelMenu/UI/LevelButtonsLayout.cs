using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace LevelMenu
{
    public class LevelButtonsLayout : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private float _containerAdditionalHeight;

        [Space]

        [SerializeField] private Vector2 _offset;
        [SerializeField] private float _spacing;
        [SerializeField, Range(0.01f, 10f)] private float _frequence;
        [SerializeField] private float _amplitude;

        [Space]

        [SerializeField] private TweenData _scrollingTweenData;

        private List<RectTransform> _buttons = new();
        private Tweener _scrollingTween;

        public RectTransform Container => _container;

        public void LayOut(RectTransform button)
        {
            _buttons.Add(button);

            button.SetParent(_container, false);
            button.anchorMin = new Vector2(0.5f, 0f);
            button.anchorMax = new Vector2(0.5f, 0f);
            button.anchoredPosition = GetPosition(_buttons.Count - 1);
        }

        public void ResizeContainer(int buttonsCount)
        {
            var totalHeight = buttonsCount * _spacing + _offset.y + _containerAdditionalHeight;
            _container.sizeDelta = new Vector2(_container.sizeDelta.x, totalHeight);
        }

        public Vector2 GetPosition(int index)
        {
            var xPosition = Mathf.Sin(index * Mathf.PI / _frequence) * _amplitude;
            var yPosition = index * _spacing;
            return new Vector2(xPosition, yPosition) + _offset;
        }

        public void ScrollTo(int buttonIndex, bool instantly = true)
        {
            var minY = -(_container.rect.height - Screen.height);
            var newY = -(--buttonIndex) * _spacing;
            newY = Mathf.Clamp(newY, minY, 0);

            if (instantly)
            {
                _container.anchoredPosition = new Vector2(_container.anchoredPosition.x, newY);
                return;
            }

            _scrollingTween?.Kill(false);
            _scrollingTween = _container
                .DOAnchorPosY(newY, _scrollingTweenData.Duration)
                .SetEase(_scrollingTweenData.Ease);
        }
    }
}