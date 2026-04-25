using R3;
using System;
using UnityEngine;
using VisualEffects;

namespace UI
{
    public class GiftToast : Toast
    {
        [Space]

        [SerializeField] private VFX _coinExplosionVFXPrefab;

        private Subject<GiftToast> _giftReceivedSignalSubj = new();
        public Observable<GiftToast> GiftReceivedSignal => _giftReceivedSignalSubj;

        public void GetGift()
        {
            _giftReceivedSignalSubj.OnNext(this);
            _giftReceivedSignalSubj.OnCompleted();

            VFX.Create(_coinExplosionVFXPrefab, transform.position).Play();
            Close();
        }
    }
}
