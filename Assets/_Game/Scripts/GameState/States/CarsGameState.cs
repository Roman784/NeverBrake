using System;

namespace GameState
{
    [Serializable]
    public class CarsGameState
    {
        public int SelectedCarId;
        public int[] UnlockedCarIds;
    }
}
