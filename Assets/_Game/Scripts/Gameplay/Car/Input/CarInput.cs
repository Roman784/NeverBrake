using R3;
using UnityEngine;

namespace Gameplay
{
    public abstract class CarInput : MonoBehaviour
    {
        public abstract int GetHorizontalInput();
        public abstract bool ShouldJump();
        public abstract bool ShouldStartMoving();
    }
}
