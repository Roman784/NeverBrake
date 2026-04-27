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
        public int TimerCollisionPenalty { get; private set; }

        private bool _isTimerStarted;
        private int _startTime;
        private int _totalTimerPenalty;
        private int _pauseStartTime;
        private int _totalPausedDuration;
        private bool _isPaused;

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
            TimerCollisionPenalty = G.RootCMS.LevelsCMS.TimerCollisionPenalty;
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

        public void IncreaseTotalTimerPenalty(int value)
        {
            _totalTimerPenalty += value;
        }

        public void PauseTimer()
        {
            _isPaused = true;
            _pauseStartTime = Mathf.CeilToInt(Time.time * 100);
        }

        public void UnpauseTimer()
        {
            _isPaused = false;
            int now = Mathf.CeilToInt(Time.time * 100);
            _totalPausedDuration += now - _pauseStartTime;
        }

        public int GetCurrentTime()
        {
            if (!_isTimerStarted) return 0;

            var now = Mathf.CeilToInt(Time.time * 100);
            var pausedTime = _totalPausedDuration;
            if (_isPaused)
                pausedTime += now - _pauseStartTime;

            return now - _startTime - pausedTime + _totalTimerPenalty;
        }

        public async UniTask PassLevel()
        {
            await Repository.SetOrAddLevelIsPassed(LevelNumber, true);
        }
    }
}
