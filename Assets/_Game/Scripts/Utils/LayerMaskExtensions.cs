using UnityEngine;

namespace Utils
{
    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask origin, LayerMask mask)
        {
            return (origin.value & (1 << mask)) != 0;
        }
    }
}
