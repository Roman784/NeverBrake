using DG.Tweening;
using R3;
using System;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourcer : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            DontDestroyOnLoad(gameObject);
        }

        public Observable<Unit> ChangeVolume(float volume, float duration = 0f)
        {
            if (duration == 0f)
            {
                _audioSource.volume = volume;
                return Observable.Return(Unit.Default);
            }

            var onCompleted = new Subject<Unit>();
            DOTween.To(
                () => _audioSource.volume,
                newVolume =>
                {
                    _audioSource.volume = newVolume;
                },
                volume,
                duration)
            .OnComplete(() =>
            {
                onCompleted.OnNext(Unit.Default);
                onCompleted.OnCompleted();
            });

            return onCompleted;
        }

        public Observable<Unit> PlayOneShot(AudioClip clip, float addedPitch)
        {
            _audioSource.loop = false;
            _audioSource.pitch = 1f + addedPitch;
            _audioSource.PlayOneShot(clip);

            return Observable.Timer(TimeSpan.FromSeconds(clip.length));
        }

        public void PlayLoop(AudioClip clip)
        {
            _audioSource.loop = true;
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        public bool IsSameClip(AudioClip other)
        {
            return _audioSource.clip == other;
        }
    }
}