using R3;
using System;
using UnityEngine;

namespace VisualEffects
{
    public class VFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _effect;

        public static VFX Create(
            VFX prefab,
            Transform parent,
            bool instantDestroy = true)
        {
            return Create(prefab, parent.position, parent.rotation, parent, instantDestroy);
        }

        public static VFX Create(
            VFX prefab,
            Vector3 position,
            Quaternion rotation = default,
            Transform parent = null,
            bool instantDestroy = true)
        {
            var vfx = Instantiate(prefab, position, rotation);
            if (parent != null) vfx.transform.SetParent(parent, true);

            return vfx;
        }

        public void Play(bool instantDestroy = false)
        {
            _effect.Play(instantDestroy);
            
            if (instantDestroy)
                Observable.Timer(TimeSpan.FromSeconds(_effect.main.duration))
                    .Subscribe(_ => Destroy())
                    .AddTo(gameObject);
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
