using R3;
using UnityEngine;
using Utils;

namespace Gameplay
{
    public class CarMobileInput : CarInput
    {
        [SerializeField] private PointerDetector _leftButton;
        [SerializeField] private PointerDetector _rightButton;
        [SerializeField] private PointerDetector _jumpButton;

        private void Start()
        {
            _jumpButton.OnPointerClickSignal.Subscribe(_ => 
                _jumpSignalSubj.OnNext(Unit.Default))
                .AddTo(this);
        }

        public override float GetHorizontalInput()
        {
            if (_leftButton.IsPointerOver)
                return -1f;
            if (_rightButton.IsPointerOver)
                return 1f;
            return 0f;
        }
    }
}
