using UnityEngine;

namespace GameState
{
    public class AudioRepository
    {
        private readonly IGameStateProvider _gameStateProvider;

        private AudioGameState State => _gameStateProvider.GameState.Audio;

        public AudioRepository(IGameStateProvider gameStateProvider)
        {
            _gameStateProvider = gameStateProvider;
        }

        public float GetSoundVolume()
        {
            return State.SoundVolume;
        }

        public float GetMusicVolume()
        {
            return State.MusicVolume;
        }

        public void SetSoundVolume(float volume)
        {
            State.SoundVolume = Mathf.Clamp01(volume);
            _gameStateProvider.SaveGameState();
        }

        public void SetMusicVolume(float volume)
        {
            State.MusicVolume = Mathf.Clamp01(volume);
            _gameStateProvider.SaveGameState();
        }
    }
}
