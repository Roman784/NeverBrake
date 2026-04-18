using System.Collections.Generic;
using System.Linq;

namespace GameState
{
    public class CarsRepository
    {
        private readonly IGameStateProvider _gameStateProvider;

        private CarsGameState State => _gameStateProvider.GameState.Cars;
        private int[] UnlockedCarIds => State.UnlockedCarIds;


        public CarsRepository(IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
        }

        public int GetSelectedCarId() => State.SelectedCarId;
        public IEnumerable<int> GetUnlockedCarIds() => UnlockedCarIds;

        public void SetSelectedCarId(int carId)
        {
            State.SelectedCarId = carId;
            _gameStateProvider.SaveGameState();
        }

        public void AddUnlockedCar(int carId)
        {
            State.UnlockedCarIds = UnlockedCarIds.Append(carId).ToArray();
            _gameStateProvider.SaveGameState();
        }
    }
}
