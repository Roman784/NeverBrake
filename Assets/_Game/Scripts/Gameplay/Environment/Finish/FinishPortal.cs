using UnityEngine;

namespace Gameplay
{
    public class FinishPortal : MonoBehaviour
    {
        [SerializeField] private Transform _centerPoint;

        public Vector3 CenterPosition => _centerPoint.position;
    }
}
