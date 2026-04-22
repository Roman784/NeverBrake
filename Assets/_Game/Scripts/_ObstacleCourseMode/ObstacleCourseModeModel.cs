using Cysharp.Threading.Tasks;
using Gameplay;
using GameRoot;
using GameState;

namespace ObstacleCourseMode
{
    public class ObstacleCourseModeModel
    {
        public ObstacleCourseModeEnterParams EnterParams { get; private set; }
        public Car Car { get; private set; }
        public int DeathCount { get; private set; }

        private int LevelNumber => EnterParams.LevelNumber;
        private LevelsRepository Repository => G.Repository.Levels;
        public SceneProvider SceneProvider => G.SceneProvider;

        public ObstacleCourseModeModel(
            ObstacleCourseModeEnterParams enterParams,
            Car car)
        {
            EnterParams = enterParams;
            Car = car;

            DeathCount = Repository.GetDeathCount(LevelNumber);
        }

        public async UniTask IncreaseDeathCount()
        {
            DeathCount += 1;
            await Repository.SetOrAddLevelDeathCount(LevelNumber, DeathCount);
        }
    }
}
