using TMPro;
using UnityEngine;
using R3;
using Utils;

namespace Currency
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsView;

        public void Bind(Wallet wallet)
        {
            SetCoins(wallet.Coins);

            wallet.CoinsChangedSignal.Subscribe(coins =>
             SetCoins(coins));
        }

        private void SetCoins(int coins)
        {
            _coinsView.text = coins.ToCoinsFormat();
        }
    }
}