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
        public int Number { get; private set; }
        public Level LevelPrefab;

        public void SetNumber(int number) => Number = Mathf.Clamp(number, 1, number);
    }
}
