using UnityEngine;

namespace Physics
{
    public class SurfaceChecker : MonoBehaviour
    {
        [SerializeField] private Vector3[] _groundStartingPoints;
        [SerializeField] private LayerMask _groundMask;

        [Space]

        [SerializeField] private float _checkingDistance;

        public bool CheckGround(out RaycastHit hit, out int missedRays)
        {
            return TryGetSurface(
                _groundStartingPoints, Vector3.down, _checkingDistance, _groundMask, out hit, out missedRays);
        }

        public bool TryGetGround(out RaycastHit hit, out int missedRays)
        {
            return TryGetSurface(
                _groundStartingPoints, Vector3.down, float.MaxValue, _groundMask, out hit, out missedRays);
        }

        private bool TryGetSurface(
            Vector3[] startingPoints,
            Vector3 direction,
            float distance,
            LayerMask mask,
            out RaycastHit hit,
            out int missedRays)
        {
            hit = default;
            missedRays = 0;

            bool hasHit = false;
            float closestDistance = float.MaxValue;

            foreach (var startPoint in startingPoints)
            {
                var origin = transform.position + startPoint;

                if (UnityEngine.Physics.Raycast(origin, direction, out var currentHit, distance, mask))
                {
                    hasHit = true;

                    if (currentHit.distance < closestDistance)
                    {
                        closestDistance = currentHit.distance;
                        hit = currentHit;
                    }
                }
                else
                {
                    missedRays++;
                }
            }

            return hasHit;
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