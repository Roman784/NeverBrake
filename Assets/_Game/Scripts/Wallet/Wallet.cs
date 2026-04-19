using Cysharp.Threading.Tasks;
using GameRoot;
using GameState;
using R3;
using UnityEngine;

namespace Currency
{
    public class Wallet
    {
        private int _coins;
        private int _lastSavedCoins;

        private Subject<int> _coinsChangedSignalSubj = new();
        private CurrencyRepository Repository => G.Repository.Currency;
        
        public int Coins => _coins;
        public Observable<int> CoinsChangedSignal => _coinsChangedSignalSubj;

        public Wallet()
        {
            _coins = Repository.GetCoins();
        }

        public bool IsEnoughCoins(int value) => _coins >= value;

        public async UniTask Save()
        {
            if (_coins == _lastSavedCoins) return;

            _lastSavedCoins = _coins;
            await Repository.SetCoins(_coins);
        }

        public async UniTask AddCoins(int value, bool save = true)
        {
            _coins += value;
            _coinsChangedSignalSubj.OnNext(_coins);

            if (save) 
                await Save();
        }

        public async UniTask SpendCoins(int value, bool save = true)
        {
            _coins -= value;
            _coinsChangedSignalSubj.OnNext(_coins);

            if (save)
                await Save();
        }

        public async UniTask<bool> TrySpendCoins(int value, bool save = true)
        {
            if (!IsEnoughCoins(value)) return false;

            _coins -= value;
            _coinsChangedSignalSubj.OnNext(_coins);

            if (save) 
                await Save();
            return true;
        }
    }
}