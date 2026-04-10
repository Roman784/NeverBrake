using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace GameRoot
{
    public abstract class SceneEntryPoint : MonoBehaviour
    {
        public abstract UniTask Run<T>(T enterParams) where T : SceneEnterParams;
    }
}