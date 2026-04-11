using R3;
using UnityEngine;

namespace Gameplay
{
    public abstract class CarInput : MonoBehaviour
    {
        protected Subject<Unit> _jumpSignalSubj = new();
        public Observable<Unit> JumpSignal => _jumpSignalSubj;
        
        public abstract int GetHorizontalInput();
    }
}
