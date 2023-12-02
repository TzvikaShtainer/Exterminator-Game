using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "Audio/UiAudioPlayer")]
    public class UIAudioPlayer : ScriptableObject
    {
        [SerializeField] private AudioClip clickAudioClip;
        [SerializeField] private AudioClip commitAudioClip;
        [SerializeField] private AudioClip selectAudioClip;
        [SerializeField] private AudioClip winAudioClip;
        
        public void PlayClick()
        {
            PlayAudio(clickAudioClip);
        }
        
        public void PlayCommit()
        {
            PlayAudio(commitAudioClip);
        }
        
        public void PlaySelect()
        {
            PlayAudio(selectAudioClip);
        }
        
        public void PlayWin()
        {
            PlayAudio(winAudioClip);
        }
        
        void PlayAudio(AudioClip audioToPlay)
        {
            Camera.main.GetComponent<AudioSource>().PlayOneShot(audioToPlay); //play from camera so it wont sound like far away
        }
    }
}
