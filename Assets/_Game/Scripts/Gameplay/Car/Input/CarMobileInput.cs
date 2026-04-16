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

        private bool _jumpRequested;

        private void Start()
        {
            _jumpButton.OnPointerEnterSignal
                .Subscribe(_ => _jumpRequested = true)
                .AddTo(this);
        }

        public override int GetHorizontalInput()
        {
            if (_leftButton.IsPointerOver)
                return -1;
            if (_rightButton.IsPointerOver)
                return 1;
            return 0;
        }

        public override bool ShouldJump()
        {
            if (_jumpRequested)
            {
                _jumpRequested = false;
                return true;
            }
            return false;
        }

        public override bool ShouldStartMoving()
        {
            return ShouldJump() || GetHorizontalInput() != 0;
        }
    }
}
