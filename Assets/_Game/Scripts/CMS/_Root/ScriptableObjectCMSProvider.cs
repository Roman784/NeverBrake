using Cysharp.Threading.Tasks;
using R3;
using System;
using UnityEngine;

namespace CMS
{
    public class ScriptableObjectCMSProvider: ICMSProvider
    {
        private RootCMS _rootCMS;

        public RootCMS RootCMS => _rootCMS;

        public async UniTask<bool> LoadRootCMS()
        {
            _rootCMS = Resources.Load<RootCMS>("RootCMS");
            await UniTask.Yield();

            if (_rootCMS == null)
                Debug.LogError("Failed to load RootCMS!");

            return _rootCMS != null;
        }
    }
}
