using UnityEditor.Tilemaps;
using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "PrefabBrush", menuName = "Brushes/Prefab Brush")]
    [CustomGridBrush(false, true, false, "Perefab Brush")]
    public class PrefabBrush : GameObjectBrush
    {

    }
}
