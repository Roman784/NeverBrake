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
            return _jumpButton.IsPointerOver;
        }
    }
}
