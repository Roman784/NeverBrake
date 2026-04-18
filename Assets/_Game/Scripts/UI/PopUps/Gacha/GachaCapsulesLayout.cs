using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using Utils;

namespace UI
{
    public class GachaCapsulesLayout : MonoBehaviour
    {
        [SerializeField] private RectTransform _capsulesContainer;

        [SerializeField] private float _axixRatio;
        [SerializeField] private float _initialYOffset;

        [SerializeField] private float _narrowing;

        [SerializeField] private float _minCapsuleScale;
        [SerializeField] private float _maxCapsuleScale;

        private Dictionary<GachaCapsule, float> _capsuleHeightsMap = new();

        private float _semiMajorCapsulesAxis;
        private float _semiMinorCapsulesAxis;
        private float _yOffset;

        public void LayOutCapsules(IReadOnlyList<GachaCapsule> capsules, float angleOffset = 0)
        {
            if (capsules.Count == 0) return;

            InitAxisSize(capsules.Count());
            InitYOffset();

            for (int i = 0; i < capsules.Count; i++)
            {
                LayOutCapsule(capsules[i], capsules.Count(), i, angleOffset);
            }

            SortCapsulesRendering(capsules);
        }

        public void ClearContainer()
        {
            var childCount = _capsulesContainer.childCount;
            for (int i = 0; i < childCount; i++)
                Destroy(_capsulesContainer.GetChild(i).gameObject);
        }

        private void InitAxisSize(int capsulesCount)
        {
            _semiMajorCapsulesAxis = Mathf.Sqrt(capsulesCount) * 156;
            _semiMinorCapsulesAxis = _semiMajorCapsulesAxis / _axixRatio;
        }

        private void InitYOffset()
        {
            float bottomAngle = -90 * Mathf.Deg2Rad;
            float y = GetRawCapsulePosition(bottomAngle).y;
            _yOffset = _initialYOffset * 2f - y;
        }

        private void SortCapsulesRendering(IEnumerable<GachaCapsule> capsules)
        {
            _capsuleHeightsMap.Clear();

            foreach (var capsule in capsules)
                _capsuleHeightsMap[capsule] = capsule.transform.position.y;

            _capsuleHeightsMap = _capsuleHeightsMap
                .OrderByDescending(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);

            int i = 0;
            foreach (var capsule in _capsuleHeightsMap.Keys)
            {
                capsule.transform.SetParent(_capsulesContainer, false);
                capsule.transform.SetSiblingIndex(i);
                i++;
            }
        }

        private void LayOutCapsule(GachaCapsule capsule, int totalCount, int idx, float angleOffset = 0)
        {
            var angle = (((360f / totalCount) * idx) - 90f + angleOffset) * Mathf.Deg2Rad;
            angle += (Mathf.Cos(angle) * _narrowing) * Mathf.Deg2Rad; // Decreasing the distance between objects to illusion of 3d depth.

            var normalizedHeight = Mathf.Sin(angle) / 2f + 0.5f;

            capsule.RectTransform.anchoredPosition = GetCapsulePosition(angle);
            capsule.RectTransform.localScale = GetCapsuleScale(normalizedHeight);
            capsule.SetFade(normalizedHeight);
        }

        private Vector2 GetCapsulePosition(float angle)
        {
            return GetRawCapsulePosition(angle) + Vector2.up * _yOffset;
        }

        private Vector2 GetRawCapsulePosition(float angle)
        {
            float yPos = Mathf.Sin(angle) * _semiMinorCapsulesAxis;
            float xPos = Mathf.Cos(angle) * _semiMajorCapsulesAxis;
            return new Vector2(xPos, yPos);
        }

        private Vector2 GetCapsuleScale(float height)
        {
            float scale = Mathf.Lerp(_maxCapsuleScale, _minCapsuleScale, height);
            return Vector2.one * scale;
        }
    }
}