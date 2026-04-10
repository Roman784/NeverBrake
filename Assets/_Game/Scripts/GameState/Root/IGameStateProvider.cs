using Cysharp.Threading.Tasks;
using R3;

namespace GameState
{
    public interface IGameStateProvider
    {
        public GameState GameState { get; }

        public UniTask<bool> LoadGameState();
        public UniTask<bool> SaveGameState();
        public UniTask<bool> ResetGameState();
    }
}
