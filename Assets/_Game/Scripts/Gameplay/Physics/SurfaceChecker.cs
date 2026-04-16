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

        public bool CheckGround(out RaycastHit hit)
        {
            return TryGetSurface(
                _groundStartingPoints, Vector3.down, _groudDistance, _groundMask, out hit);
        }

        public bool CheckWater(out RaycastHit hit)
        {
            var combinedMask = _groundMask | _waterMask;
            TryGetSurface(_waterStartingPoints, Vector3.down, _waterDistance, combinedMask, out hit);
            return hit.collider != null && hit.collider.CompareTag(_waterTag);
        }

        public bool TryGetGround(out RaycastHit hit)
        {
            return TryGetSurface(
                _groundStartingPoints, Vector3.down, float.MaxValue, _groundMask, out hit);
        }

        private bool TryGetSurface(
            Vector3[] startingPoints, 
            Vector3 direction, 
            float distance, 
            LayerMask mask, 
            out RaycastHit hit)
        {
            foreach (var startPoint in startingPoints)
            {
                var origin = transform.TransformPoint(startPoint);
                if (UnityEngine.Physics.Raycast(origin, direction, out hit, distance, mask))
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
                Gizmos.DrawRay(origin, Vector3.down * _groudDistance);
            }

            foreach (var startPoint in _waterStartingPoints)
            {
                Gizmos.color = Color.blue;
                var origin = transform.TransformPoint(startPoint);
                Gizmos.DrawRay(origin, Vector3.down * _waterDistance);
            }
        }
    }
}