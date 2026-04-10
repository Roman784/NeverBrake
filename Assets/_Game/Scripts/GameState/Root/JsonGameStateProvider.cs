using CMS;
using Cysharp.Threading.Tasks;
using GameRoot;
using R3;
using System;
using System.IO;
using UnityEngine;

namespace GameState
{
    public class JsonGameStateProvider : IGameStateProvider
    {
        private const string GAME_STATE_KEY = "GAME_STATE";

        private string _savePath;

        public GameState GameState { get; private set; }
        private GameState InitialGameState => G.RootCMS.InitialGameStateCMS.State;

        public JsonGameStateProvider()
        {
            _savePath = GetPath();
        }

        public async UniTask<bool> LoadGameState()
        {
            if (!File.Exists(_savePath))
            {
                GameState = CreateInitalGameState();
                await SaveGameState();
            }
            else
            {
                var json = await File.ReadAllTextAsync(_savePath);
                GameState = JsonUtility.FromJson<GameState>(json);
            }

            if (GameState == null)
                Debug.LogError("Failed to deserialize GameState!");

            return GameState != null;
        }

        public async UniTask<bool> SaveGameState()
        {
            try
            {
                var json = JsonUtility.ToJson(GameState, true);
                await File.WriteAllTextAsync(_savePath, json);

                return true;
            }
            catch 
            { 
                return false;
            }
        }

        public async UniTask<bool> ResetGameState()
        {
            GameState = CreateInitalGameState();
            return await SaveGameState();
        }

        private GameState CreateInitalGameState()
        {
            return JsonUtility.FromJson<GameState>(JsonUtility.ToJson(InitialGameState));
        }

        private string GetPath()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return Path.Combine (Application.persistentDataPath, $"{GAME_STATE_KEY}.json");
#else
            return Path.Combine(Application.dataPath, $"{GAME_STATE_KEY}.json");
#endif
        }
    }
}
