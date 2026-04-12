using UnityEngine;

namespace Physics
{
    public class SurfaceChecker : MonoBehaviour
    {
        [Header("Ground")]
        [SerializeField] private Vector3[] _groundStartingPoints;
        [SerializeField] private LayerMask _groundMask;

        [Space]

        [SerializeField] private float _checkingDistance;

        public bool CheckGround(out RaycastHit hit)
        {
            return TryGetSurface(
                _groundStartingPoints, Vector3.down, _checkingDistance, _groundMask, out hit);
        }

        public bool TryGetGround(out RaycastHit hit)
        {
            return TryGetSurface(
                _groundStartingPoints, Vector3.down, float.MaxValue, _groundMask, out hit);
        }

        private bool TryGetSurface(
            Vector3[] startingPoints, Vector3 direction, float distance, LayerMask mask, out RaycastHit hit)
        {
            foreach (var startPoint in startingPoints)
            {
                var origin = transform.position + startPoint;
                return UnityEngine.Physics.Raycast(origin, direction, out hit, distance, mask);
            }

            hit = default;
            return false;
        }

        private void OnDrawGizmos()
        {
            if (_groundStartingPoints == null) return;

            Gizmos.color = Color.red;

            foreach (var startPoint in _groundStartingPoints)
            {
                var origin = transform.position + startPoint;
                Gizmos.DrawRay(origin, Vector3.down * _checkingDistance);
            }
        }
    }
}