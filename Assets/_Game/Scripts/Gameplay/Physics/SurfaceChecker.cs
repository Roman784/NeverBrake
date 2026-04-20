using UnityEngine;

namespace Physics
{
    public class SurfaceChecker : MonoBehaviour
    {
        [Header("Ground")]
        [SerializeField] private Vector3[] _groundStartingPoints;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private float _groudDistance;

        [Header("Water")]
        [SerializeField] private Vector3[] _waterStartingPoints;
        [SerializeField] private LayerMask _waterMask;
        [SerializeField] private string _waterTag = "Water"; 
        [SerializeField] private float _waterDistance;

        public bool CheckGround(out RaycastHit2D hit)
        {
            return TryGetSurface(
                _groundStartingPoints, Vector3.forward, _groudDistance, _groundMask, out hit);
        }

        public bool CheckWater(out RaycastHit2D hit)
        {
            var combinedMask = _groundMask | _waterMask;
            TryGetSurface(_waterStartingPoints, Vector3.forward, _waterDistance, combinedMask, out hit);
            return hit.collider != null && hit.collider.CompareTag(_waterTag);
        }

        public bool TryGetGround(out RaycastHit2D hit)
        {
            return TryGetSurface(
                _groundStartingPoints, Vector3.forward, float.MaxValue, _groundMask, out hit);
        }

        private bool TryGetSurface(
            Vector3[] startingPoints, 
            Vector3 direction, 
            float distance, 
            LayerMask mask, 
            out RaycastHit2D hit)
        {
            foreach (var startPoint in startingPoints)
            {
                var origin = transform.TransformPoint(startPoint);
                hit = Physics2D.Raycast(origin, direction, distance, mask);
                if (hit.collider != null)
                    return true;
            }

            hit = default;
            return false;
        }

        private void OnDrawGizmos()
        {
            foreach (var startPoint in _groundStartingPoints)
            {
                Gizmos.color = Color.red;
                var origin = transform.TransformPoint(startPoint);
                Gizmos.DrawRay(origin, Vector3.forward * _groudDistance);
            }

            foreach (var startPoint in _waterStartingPoints)
            {
                Gizmos.color = Color.blue;
                var origin = transform.TransformPoint(startPoint);
                Gizmos.DrawRay(origin, Vector3.forward * _waterDistance);
            }
        }
    }
}