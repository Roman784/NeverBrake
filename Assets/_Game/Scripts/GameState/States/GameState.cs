using System;

namespace GameState
{
    [Serializable]
    public class GameState
    {
        public AudioGameState Audio;
        public LevelsGameState Levels;
        public CarsGameState Cars;
    }
}