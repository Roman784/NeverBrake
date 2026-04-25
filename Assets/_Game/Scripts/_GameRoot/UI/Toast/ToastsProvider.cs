using Cysharp.Threading.Tasks;
using GameRoot;
using System;
using System.Collections.Generic;
using System.Threading;
using R3;

namespace UI
{
    public class ToastsProvider
    {
        private UICMS UICMS => G.RootCMS.UICMS;

        private List<Toast> _preparedToasts = new();
        private CancellationTokenSource _cts;

        public async UniTask Open()
        {
            _cts?.Cancel();
            _cts?.Dispose();

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                foreach (var toast in _preparedToasts)
                {
                    token.ThrowIfCancellationRequested();

                    toast.Open();

                    await UniTask.Delay(150, cancellationToken: token);
                }
                _preparedToasts.Clear();
            }
            catch (OperationCanceledException) { }
        }

        public void RemoveToast(Toast toast) => _preparedToasts.Remove(toast);

        public TotalCoinsToast PrepareTotalCoinsToast()
        {
            var createdToast = G.ToastFactory.Create(UICMS.TotalCoinsToastPrefab);
            RegisterToast(createdToast);
            return createdToast;
        }

        public CoinsReceivedToast PrepareCoinsReceivedToast(int receivedCoins)
        {
            var createdToast = G.ToastFactory.Create(UICMS.CoinsReceivedToastPrefab);
            createdToast.SetCoins(receivedCoins);
            RegisterToast(createdToast);
            return createdToast;
        }

        public CoinsForAdToast PrepareCoinsForAdToast()
        {
            var createdToast = G.ToastFactory.Create(UICMS.CoinsForAdToastPrefab);
            RegisterToast(createdToast);
            return createdToast;
        }

        public PrizeToast PreparePrizeToast()
        {
            var createdToast = G.ToastFactory.Create(UICMS.PrizeToastPrefab);
            RegisterToast(createdToast);
            return createdToast;
        }

        public GiftToast PrepareGiftToast()
        {
            var createdToast = G.ToastFactory.Create(UICMS.GiftToastPrefab);
            RegisterToast(createdToast);
            return createdToast;
        }

        private void RegisterToast(Toast toast)
        {
            _preparedToasts.Add(toast);
            toast.CloseSignal.Subscribe(t => RemoveToast(t));
        }
    }
}
