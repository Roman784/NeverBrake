using GameRoot;
using GameState;
using R3;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Audio
{
    public class AudioProvider
    {
        private ObjectPool<AudioSourcer> _sourcers;
        private AudioSourcer _musicSourcer;

        private Dictionary<int, AudioSourcer> _loopedSourcers;
        private int _loopedSourcerId;


        public ReactiveProperty<float> MusicVolume;
        public ReactiveProperty<float> SoundVolume;

        private AudioRepository Repository => G.Repository.Audio;
        private AudioSourcer SourcerPrefab => G.RootCMS.AudioCMS.SourcerPrefab;

        public void Init()
        {
            _loopedSourcers = new Dictionary<int, AudioSourcer>();
            MusicVolume = new ReactiveProperty<float>(Repository.GetMusicVolume());
            SoundVolume = new ReactiveProperty<float>(Repository.GetSoundVolume());

            _sourcers = new ObjectPool<AudioSourcer>(
                createFunc: () =>
                {
                    var sourcer = Object.Instantiate(SourcerPrefab);
                    SoundVolume.Subscribe(volume => sourcer.ChangeVolume(volume));
                    sourcer.ChangeVolume(SoundVolume.Value);
                    return sourcer;
                },
                actionOnGet: sourcer => sourcer.gameObject.SetActive(true),
                actionOnRelease: sourcer => sourcer.gameObject.SetActive(false),
                defaultCapacity: 10
            );

            _musicSourcer = Object.Instantiate(SourcerPrefab);
            MusicVolume.Subscribe(volume => _musicSourcer.ChangeVolume(volume));
        }

        public void PlaySound(AudioClip audioClip, float addedPitch = 0)
        {
            var sourcer = _sourcers.Get();
            sourcer.PlayOneShot(audioClip, addedPitch).Subscribe(_ => _sourcers.Release(sourcer));
        }

        // Returns the id for stopping the sound.
        public int PlayLoop(AudioClip audioClip)
        {
            var sourcer = _sourcers.Get();
            sourcer.PlayLoop(audioClip);

            _loopedSourcers.Add(_loopedSourcerId, sourcer);
            return _loopedSourcerId++;
        }

        public void StopSound(int id)
        {
            if (_loopedSourcers.TryGetValue(id, out var sourcer))
            {
                sourcer.Stop();
                _sourcers.Release(sourcer);
                _loopedSourcers.Remove(id);
                return;
            }
            Debug.LogWarning($"Failed to find sourcer by id: {id}");
        }

        public void PlayMusic(params AudioClip[] audioClip)
        {
            var clip = audioClip[Random.Range(0, audioClip.Length)];
            if (_musicSourcer.IsSameClip(clip)) return;

            _musicSourcer.ChangeVolume(0f, 0.5f).Subscribe(_ =>
            {
                _musicSourcer.PlayLoop(clip);
                _musicSourcer.ChangeVolume(MusicVolume.Value, 0.5f);
            });
        }
    }
}