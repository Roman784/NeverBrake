using CustomizationMenu;
using Gameplay;
using UnityEngine;
using Utils;

namespace CMS
{
    [CreateAssetMenu(fileName = "CarCMS",
                     menuName = "CMS/Cars/New Car CMS",
                     order = 1)]
    public class CarCMS : ScriptableObject
    {
        public int Id;
        public Car Prefab;
        public SkinPreview PreviewPrefab;
        public Rarity Rarity;
    }
}
