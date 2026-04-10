using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace GameRoot
{
    public abstract class SceneEntryPoint : MonoBehaviour
    {
        public abstract UniTask Run<T>(T enterParams) where T : SceneEnterParams;
    }

    public abstract class SceneEntryPoint<TEnterParams> : SceneEntryPoint where TEnterParams : SceneEnterParams
    {
        public override async UniTask Run<T>(T enterParams)
        {
            if (enterParams.TryCast<TEnterParams>(out var specificParams))
                await Run(specificParams);
            else
                Debug.LogError($"Failed to convert {typeof(T)} to {typeof(TEnterParams)}!");
        }

        protected abstract UniTask Run(TEnterParams enterParams);
    }
}