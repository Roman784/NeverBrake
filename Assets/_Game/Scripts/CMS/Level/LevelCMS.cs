using Gameplay;
using Unity.Collections;
using UnityEngine;

namespace CMS
{
    [CreateAssetMenu(fileName = "LevelCMS",
                     menuName = "CMS/Levels/New Level CMS",
                     order = 1)]
    public class LevelCMS : ScriptableObject
    {
        public int Number;
        public Level LevelPrefab;
    }
}
