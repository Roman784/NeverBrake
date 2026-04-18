using Cysharp.Threading.Tasks;
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

        public async UniTask SetSelectedCarId(int carId)
        {
            State.SelectedCarId = carId;
            await _gameStateProvider.SaveGameState();
        }

        public void AddUnlockedCar(int carId)
        {
            State.UnlockedCarIds = UnlockedCarIds.Append(carId).ToArray();
            _gameStateProvider.SaveGameState();
        }
    }
}
