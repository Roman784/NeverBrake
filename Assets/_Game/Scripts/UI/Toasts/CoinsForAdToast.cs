using R3;

namespace UI
{
    public class CoinsForAdToast : Toast
    {
        private Subject<CoinsForAdToast> _adWatchedSignalSubj = new();
        public Observable<CoinsForAdToast> AdWatchedSignal => _adWatchedSignalSubj;

        public void WatchAdAndGetCoins()
        {
            // TODO: watch ad.

            _adWatchedSignalSubj.OnNext(this);
            _adWatchedSignalSubj.OnCompleted();

            Close();
        }
    }
}
