using UnityEngine;

namespace Gameplay
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private GameplayCamera _camera;
        [SerializeField] private Transform _carSpawnPoint;

        public GameplayCamera Camera => _camera;
        public Vector3 CarSpawnPosition => _carSpawnPoint.position;
        public  Quaternion CarSpawnRotation => _carSpawnPoint.rotation;
    }
}
