using UnityEngine.EventSystems;
using UnityEngine;
using R3;

namespace Utils
{
    public class PointerDetector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private bool _isEnabled = true;
        private bool _isPointerOver = false;

        private Subject<PointerEventData> _onPointerClickSignalSubj = new();
        private Subject<PointerEventData> _onPointerEnterSignalSubj = new();
        private Subject<PointerEventData> _onPointerExitSignalSubj = new();
        private Subject<PointerEventData> _onPointerDownSignalSubj = new();
        private Subject<PointerEventData> _onPointerUpSignalSubj = new();

        public bool IsPointerOver => _isPointerOver;

        public Observable<PointerEventData> OnPointerClickSignal => _onPointerClickSignalSubj;
        public Observable<PointerEventData> OnPointerEnterSignal => _onPointerEnterSignalSubj;
        public Observable<PointerEventData> OnPointerExitSignal => _onPointerExitSignalSubj;
        public Observable<PointerEventData> OnPointerDownSignal => _onPointerDownSignalSubj;
        public Observable<PointerEventData> OnPointerUpSignal => _onPointerUpSignalSubj;

        public void Enable()
        {
            if (!_isEnabled && _isPointerOver)
            {
                _onPointerEnterSignalSubj.OnNext(null);
            }
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isEnabled) return;
            _onPointerClickSignalSubj.OnNext(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOver = true;
            if (!_isEnabled) return;
            _onPointerEnterSignalSubj.OnNext(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;
            if (!_isEnabled) return;
            _onPointerExitSignalSubj.OnNext(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isEnabled) return;
            _onPointerDownSignalSubj.OnNext(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isEnabled) return;
            _onPointerUpSignalSubj.OnNext(eventData);
        }
    }
}
