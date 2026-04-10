using Audio;
using UnityEngine;

namespace CMS
{
    [CreateAssetMenu(fileName = "AudioCMS",
                     menuName = "CMS/Audio/New Audio",
                     order = 3)]
    public class AudioCMS: ScriptableObject
    {
        public AudioSourcer SourcerPrefab;
    }
}