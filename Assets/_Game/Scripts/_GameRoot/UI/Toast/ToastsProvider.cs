using Cysharp.Threading.Tasks;
using GameRoot;
using System;
using System.Collections.Generic;
using System.Threading;
using R3;
using Random = UnityEngine.Random;

namespace UI
{
    public class ToastsProvider
    {
        private UICMS CMS => G.RootCMS.UICMS;

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
            var createdToast = G.ToastFactory.Create(CMS.TotalCoinsToastPrefab);
            RegisterToast(createdToast);
            return createdToast;
        }

        public CoinsReceivedToast PrepareCoinsReceivedToast(int receivedCoins)
        {
            var createdToast = G.ToastFactory.Create(CMS.CoinsReceivedToastPrefab);
            createdToast.SetCoins(receivedCoins);
            RegisterToast(createdToast);
            return createdToast;
        }

        public CoinsForAdToast PrepareCoinsForAdToast()
        {
            var createdToast = G.ToastFactory.Create(CMS.CoinsForAdToastPrefab);
            RegisterToast(createdToast);

            createdToast
                .AdWatchedSignal
                .SubscribeAwait(async (toast, _) =>
                {
                    var coinsScatter = G.RootCMS.CurrencyCMS.CoinsScatterForAd;
                    var randomCoins = Random.Range(coinsScatter.x, coinsScatter.y);
                    await G.Wallet.AddCoins(randomCoins);
                    toast.CloseSignal.SubscribeAwait(async (_, _) =>
                    {
                        G.ToastsProvider.PrepareCoinsReceivedToast(randomCoins);
                        await G.ToastsProvider.Open();
                    });
                });

            return createdToast;
        }

        public PrizeToast PreparePrizeToast()
        {
            var createdToast = G.ToastFactory.Create(CMS.PrizeToastPrefab);
            RegisterToast(createdToast);
            return createdToast;
        }

        public GiftToast PrepareGiftToast()
        {
            var createdToast = G.ToastFactory.Create(CMS.GiftToastPrefab);
            RegisterToast(createdToast);

            createdToast
                .GiftReceivedSignal
                .SubscribeAwait(async (toast, _) =>
                {
                    var coinsScatter = G.RootCMS.CurrencyCMS.CoinsScatterForGift;
                    var randomCoins = Random.Range(coinsScatter.x, coinsScatter.y);
                    await G.Wallet.AddCoins(randomCoins);
                    toast.CloseSignal.SubscribeAwait(async (_, _) =>
                    {
                        G.ToastsProvider.PrepareCoinsReceivedToast(randomCoins);
                        await G.ToastsProvider.Open();
                    });
                });

            return createdToast;
        }

        private void RegisterToast(Toast toast)
        {
            _preparedToasts.Add(toast);
            toast.CloseSignal.Subscribe(t => RemoveToast(t));
        }
    }
}
