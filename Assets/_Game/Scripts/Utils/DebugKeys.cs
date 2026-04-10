#if UNITY_EDITOR
using GameRoot;
using UnityEngine;

namespace Utils
{
    public class DebugKeys : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Create()
        {
            var go = new GameObject("[DEBUG_KEYS]");
            go.AddComponent<DebugKeys>();
            DontDestroyOnLoad(go);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyUp(KeyCode.R))
                    GameAutostarter.AutostartGame();
            }
        }
    }
}
#endif