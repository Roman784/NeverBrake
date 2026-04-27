using Cysharp.Threading.Tasks;
using GameRoot;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsPopUp : PopUp
    {
        [Space]

        [SerializeField] private Image _musicView;
        [SerializeField] private Sprite _musicOn;
        [SerializeField] private Sprite _musicOff;

        [Space]

        [SerializeField] private Image _soundView;
        [SerializeField] private Sprite _soundOn;
        [SerializeField] private Sprite _soundOff;

        private void Awake()
        {
            DisplayUI();
        }

        public void ChangeMusicVolume()
        {
            var volume = G.AudioProvider.MusicVolume.CurrentValue;
            volume = volume > 0f ? 0f : 1f;
            G.AudioProvider.MusicVolume.OnNext(volume);

            G.Repository.Audio.SetMusicVolume(volume).Forget();

            DisplayUI();
        }

        public void ChangeSoundVolume()
        {
            var volume = G.AudioProvider.SoundVolume.CurrentValue;
            volume = volume > 0f ? 0f : 1f;
            G.AudioProvider.SoundVolume.OnNext(volume);

            G.Repository.Audio.SetSoundVolume(volume).Forget();

            DisplayUI();
        }

        private void DisplayUI()
        {
            var musicVolume = G.AudioProvider.MusicVolume.CurrentValue;
            var soundVolume = G.AudioProvider.SoundVolume.CurrentValue;

            _musicView.sprite = musicVolume > 0f ? _musicOn : _musicOff;
            _soundView.sprite = soundVolume > 0f ? _soundOn : _soundOff;
        }
    }
}
