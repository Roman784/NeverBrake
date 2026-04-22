using Currency;
using Gameplay;
using GameRoot;
using GameState;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelMenu
{
    public class LevelMenuModel
    {
        public Wallet Wallet { get; private set; }
        public int LevelsCount { get; private set; }
        public int LastPassedLevelNumber { get; private set; }
        public LevelButton SelectedLevelButton { get; set; }

        private Dictionary<LevelButton, int> _numbersMap = new();

        private LevelsRepository Repository => G.Repository.Levels;

        public LevelMenuModel()
        {
            Wallet = G.Wallet;
            LevelsCount = G.RootCMS.LevelsCMS.LevelCount;
            LastPassedLevelNumber = Repository.GetLastPassedLevelNumber();
        }

        public bool IsLevelPassed(int number) => number == LastPassedLevelNumber;
        public bool IsLevelUnlocked(int number) => number <= LastPassedLevelNumber + 1;
        public void AddLevelButtonNumber(LevelButton button, int number) => _numbersMap[button] = number;

        public int GetLevelBestTime(int number) => Repository.GetBestTime(number);
        public int GetLevelDeathCount(int number) => Repository.GetDeathCount(number);

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