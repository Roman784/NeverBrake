using Physics;
using UnityEngine;

namespace Gameplay
{
    public class CarCollisionHandler : MonoBehaviour
    {
        [SerializeField] private SurfaceChecker _surfaceChecker;

        public bool OnGround => _surfaceChecker.CheckGround(out var _);
    }
}
