using GameRoot;
using TMPro;
using UnityEngine;
using Utils;
using R3;

namespace UI
{
    public class TotalCoinsToast : Toast
    {
        [SerializeField] private TMP_Text _messageView;

        public override void Open()
        {
            var coins = G.Wallet.Coins;
            SetCoins(coins);

            G.Wallet.CoinsChangedSignal
                .Subscribe(c => SetCoins(c))
                .AddTo(this);

            base.Open();
        }

        private void SetCoins(int coins)
        {
            _messageView.text = 
                $"{TextIcons.Coin}{TextIcons.Coin}{TextIcons.Coin}  " +
                $"Total <color=#FFE849>{coins}</color> coins " +
                $"{TextIcons.Coin}{TextIcons.Coin}{TextIcons.Coin}"; // Loc
        }
    }
}
