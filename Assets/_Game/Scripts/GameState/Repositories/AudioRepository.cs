using Cysharp.Threading.Tasks;
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

        public async UniTask SetSoundVolume(float volume)
        {
            State.SoundVolume = Mathf.Clamp01(volume);
            await _gameStateProvider.SaveGameState();
        }

        public async UniTask SetMusicVolume(float volume)
        {
            State.MusicVolume = Mathf.Clamp01(volume);
            await _gameStateProvider.SaveGameState();
        }
    }
}
