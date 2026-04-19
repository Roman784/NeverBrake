using Cysharp.Threading.Tasks;
using GameRoot;
using UnityEngine;

namespace GameState
{
    public class CurrencyRepository
    {
        private readonly IGameStateProvider _gameStateProvider;

        private CurrencyGameState State => _gameStateProvider.GameState.Currency;

        public CurrencyRepository(IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
        }

        public int GetCoins()
        {
            return State.Coins;
        }

        public async UniTask SetCoins(int value)
        {
            State.Coins = value;
            await _gameStateProvider.SaveGameState();
        }

        public void AddCoins(int value)
        {
            State.Coins += value;
            _gameStateProvider.SaveGameState();
        }
    }
}
