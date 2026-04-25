using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    public class CoinsReceivedToast : Toast
    {
        [SerializeField] private TMP_Text _messageView;

        public void SetCoins(int coins)
        {
            _messageView.text = $"{TextIcons.Coin}\tYou got <color=#FFE849>{coins}</color> coins!\t{TextIcons.Coin}"; // Loc
        }
    }
}
