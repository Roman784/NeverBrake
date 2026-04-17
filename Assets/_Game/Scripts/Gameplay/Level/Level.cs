using UnityEngine;

namespace Gameplay
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _carSpawnPoint;

        public Camera Camera => _camera;
        public Vector3 CarSpawnPosition => _carSpawnPoint.position;
        public  Quaternion CarSpawnRotation => _carSpawnPoint.rotation;
    }
}
