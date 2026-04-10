using UnityEngine;
using State = GameState.GameState;

namespace CMS
{
    [CreateAssetMenu(
        fileName = "InitialGameStateCMS",
        menuName = "CMS/Initial/New Initial Game State",
        order = 1)]
    public class InitialGameStateCMS : ScriptableObject
    {
        public State State;
    }
}
