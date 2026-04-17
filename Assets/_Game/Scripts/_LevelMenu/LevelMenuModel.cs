using Gameplay;
using GameRoot;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelMenu
{
    public class LevelMenuModel
    {
        public int LevelsCount { get; private set; }
        public int LastPassedLevelNumber { get; private set; }

        public LevelButton SelectedLevelButton { get; set; }

        private Dictionary<LevelButton, int> _numbersMap = new();

        public LevelMenuModel(
            int levelsCount,
            int lastPassedLevelNumber)
        {
            LevelsCount = levelsCount;
            LastPassedLevelNumber = lastPassedLevelNumber;
        }

        public bool IsLevelUnlocked(int number) => number <= LastPassedLevelNumber + 1;
        public void AddLevelButtonNumber(LevelButton button, int number) => _numbersMap[button] = number;

        public int GetSelectedLevelNumber()
        {
            if (_numbersMap.TryGetValue(SelectedLevelButton, out var number))
                return number;
            return 1;
        }

        public IEnumerable<Vector3> GetLevelButtonPositions()
        {
            return _numbersMap.Keys.Select(b => b.transform.position);
        }
    }
}