using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CMS
{
    [CreateAssetMenu(fileName = "LevelsCMS",
                     menuName = "CMS/Levels/New Levels CMS",
                     order = 0)]
    public class LevelsCMS : ScriptableObject
    {
        public int TimerCollisionPenalty;

        [Space]

        public LevelCMS DebugLevelCMS;
        public LevelCMS[] AllLevelsCMS;

        public int LevelCount => AllLevelsCMS.Length;
        public bool IsLevelExist(int number) => GetLevelCMS(number) != null;

        public LevelCMS GetLevelCMS(int number)
        {
            if (number <= 0) return DebugLevelCMS;
            return AllLevelsCMS.FirstOrDefault(l => l.Number == number);
        }

        private void OnValidate()
        {
            SetLevelNumbers();
        }

        private void SetLevelNumbers()
        {
            if (AllLevelsCMS == null) return;
            for (int i = 0; i < AllLevelsCMS.Length; i++)
            {
                var number = i + 1;
                AllLevelsCMS[i].Number = number;
#if UNITY_EDITOR
                EditorUtility.SetDirty(AllLevelsCMS[i]);
#endif
            }
        }
    }
}
