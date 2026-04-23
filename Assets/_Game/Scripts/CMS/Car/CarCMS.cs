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
        public int Id { get; private set; }
        public Car Prefab;
        public SkinPreview PreviewPrefab;
        public Rarity Rarity;

        public void SetId(int id) => Id = id;
    }
}
