using UnityEngine;
using VisualEffects;

namespace Gameplay
{
    public class CarView : MonoBehaviour
    {
        [SerializeField] private Transform[] _wheels;
        [SerializeField] private TrailRenderer[] _tireTracks;

        [Space]

        [SerializeField] private Transform _boostVFXPoint;
        [SerializeField] private VFX _boostVFXPrefab;
        [SerializeField] private VFX _collisionVFXPrefab;

        public void ApplyWheelsTurning(float angle)
        {
            foreach (var wheel in _wheels)
            {
                wheel.localRotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        public void SetActiveTireTracks(bool value)
        {
            foreach (var tireTrack in _tireTracks)
            {
                tireTrack.emitting = value;
            }
        }

        public void PlayBoostVFX()
        {
            VFX.Create(_boostVFXPrefab, _boostVFXPoint).Play();
        }

        public void PlayCollisionVFX(Vector3 position, Vector3 normal)
        {
            var rotation = Quaternion.LookRotation(normal);
            VFX.Create(_collisionVFXPrefab, position, rotation).Play();
        }
    }
}
