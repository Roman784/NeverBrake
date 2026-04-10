using UI;
using UnityEngine;
using UnityEngine.UI;
using R3;
using GameRoot;

namespace Audio
{
    [RequireComponent(typeof(Button))]
    public class ButtonAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (_clip != null)
                    G.AudioProvider.PlaySound(_clip);
                //else
                   // G.AudioProvider.PlaySound(R.Audio.ButtonClick);
            });
        }
    }
}