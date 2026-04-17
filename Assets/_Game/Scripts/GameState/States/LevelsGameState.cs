using System;

namespace GameState
{
    [Serializable]
    public class LevelsGameState
    {
        public LevelData[] LevelsData;
    }

    [Serializable]
    public class LevelData
    {
        public int Number;
        public bool IsPassed;
        public int BestTime;
        public int DeathCount;
    }
}
