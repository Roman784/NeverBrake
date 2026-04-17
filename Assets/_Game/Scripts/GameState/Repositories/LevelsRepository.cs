using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameState
{
    public class LevelsRepository
    {
        private readonly IGameStateProvider _gameStateProvider;
        
        private LevelsGameState State => _gameStateProvider.GameState.Levels;
        private LevelData[] LevelsData => State.LevelsData;

        private Dictionary<int, LevelData> _levelsMap = new();

        public LevelsRepository(IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;

            InitLevelsMap();
        }

        private void InitLevelsMap()
        {
            if (LevelsData == null) return;
            foreach (var levelData in LevelsData)
                _levelsMap[levelData.Number] = levelData;
        }

        public int GetLastPassedLevelNumber()
        {
            var passedLevels = LevelsData.Where(l => l.IsPassed);
            if (passedLevels.Count() > 0)
                return passedLevels.Max(l => l.Number);
            return 0;
        }

        public int GetBestTime(int levelNumber)
        {
            if (_levelsMap.TryGetValue(levelNumber, out var levelData))
                return levelData.BestTime;
            return 0;
        }

        public int GetDeathCount(int levelNumber)
        {
            if (_levelsMap.TryGetValue(levelNumber, out var levelData))
                return levelData.DeathCount;
            return 0;
        }

        public void SetOrAddLevelIsPassed(int levelNumber, bool isPassed)
        {
            AddNewLevelIfItDoesNotExist(levelNumber);

            _levelsMap[levelNumber].IsPassed = isPassed;
            _gameStateProvider.SaveGameState();
        }

        public void SetOrAddLevelBestTime(int levelNumber, int time)
        {
            AddNewLevelIfItDoesNotExist(levelNumber);

            _levelsMap[levelNumber].BestTime = time;
            _gameStateProvider.SaveGameState();
        }

        public void SetOrAddLevelDeathCount(int levelNumber, int count)
        {
            AddNewLevelIfItDoesNotExist(levelNumber);

            _levelsMap[levelNumber].DeathCount = count;
            _gameStateProvider.SaveGameState();
        }

        private void AddNewLevelIfItDoesNotExist(int levelNumber)
        {
            if (_levelsMap.ContainsKey(levelNumber)) return;

            var newLevelData = new LevelData() { Number = levelNumber };
            State.LevelsData = LevelsData.Append(newLevelData).ToArray();
            _levelsMap.Add(levelNumber, newLevelData);
        }
    }
}
