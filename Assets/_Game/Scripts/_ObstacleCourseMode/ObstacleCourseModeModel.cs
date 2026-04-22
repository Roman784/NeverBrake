using Cysharp.Threading.Tasks;
using Gameplay;
using GameRoot;
using GameState;
using System.Linq;
using UnityEngine;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModeModel
    {
        public ObstacleCourseModeEnterParams EnterParams { get; private set; }
        public Car Car { get; private set; }
        public bool IsLevelPassed { get; private set; }
        public int DeathCount { get; private set; }
        public int BestTime { get; private set; }

        private bool _isTimerStarted;
        private int _startTime;

        private int LevelNumber => EnterParams.LevelNumber;
        private LevelsRepository Repository => G.Repository.Levels;
        public SceneProvider SceneProvider => G.SceneProvider;

        public ObstacleCourseModeModel(
            ObstacleCourseModeEnterParams enterParams,
            Car car)
        {
            EnterParams = enterParams;
            Car = car;

            IsLevelPassed = Repository.GetPassedLevelNumbers().Any(l => l == LevelNumber);
            DeathCount = Repository.GetDeathCount(LevelNumber);
            BestTime = Repository.GetBestTime(LevelNumber);
        }

        public async UniTask SaveNewBestTime(int time)
        {
            BestTime = time;
            await Repository.SetOrAddLevelBestTime(LevelNumber, BestTime);
        }

        public async UniTask IncreaseDeathCount()
        {
            DeathCount += 1;
            await Repository.SetOrAddLevelDeathCount(LevelNumber, DeathCount);
        }

        public void StartTimer()
        {
            _startTime = Mathf.CeilToInt(Time.time * 100);
            _isTimerStarted = true;
        }

        public int GetCurrentTime()
        {
            if (!_isTimerStarted) return 0;
            return Mathf.CeilToInt(Time.time * 100) - _startTime;
        }
    }
}
