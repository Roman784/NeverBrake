using Cysharp.Threading.Tasks;
using GameRoot;
using System;
using System.Collections.Generic;
using System.Threading;

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
            }
            catch (OperationCanceledException) { }
        }

        public ToastsProvider PrepareGetPrizeToast()
        {
            var createdToast = G.ToastFactory.Create(UICMS.GetPrizeToastPrefab);
            _preparedToasts.Add(createdToast);

            return this;
        }
    }
}
