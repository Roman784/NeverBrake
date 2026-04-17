using GameRoot;
using UnityEngine;

namespace CMS
{
    [CreateAssetMenu(fileName = "RootCMS", 
                     menuName = "CMS/New Root", 
                     order = 0)]
    public class RootCMS: ScriptableObject
    {
        public AudioCMS AudioCMS;
        public UICMS UICMS;
        public InitialGameStateCMS InitialGameStateCMS;
        public LevelsCMS LevelsCMS;
        public CarsCMS CarsCMS;
    }
}
