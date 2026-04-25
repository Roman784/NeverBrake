using R3;
using UnityEngine;
using VisualEffects;

namespace UI
{
    public class CoinsForAdToast : Toast
    {
        [SerializeField] private VFX _coinExplosionVFXPrefab;

        private Subject<CoinsForAdToast> _adWatchedSignalSubj = new();
        public Observable<CoinsForAdToast> AdWatchedSignal => _adWatchedSignalSubj;

        public void WatchAdAndGetCoins()
        {
            // TODO: watch ad.

            _adWatchedSignalSubj.OnNext(this);
            _adWatchedSignalSubj.OnCompleted();

            VFX.Create(_coinExplosionVFXPrefab, transform.position).Play();
            Close();
        }
    }
}
