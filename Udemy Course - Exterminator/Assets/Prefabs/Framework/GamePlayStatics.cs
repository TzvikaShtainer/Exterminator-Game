using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace Prefabs.Framework
{
    public abstract class GamePlayStatics : MonoBehaviour
    {
        class AudioSrcContext : MonoBehaviour
        {
            
        }

        private static ObjectPool<AudioSource> audioPool;

        public static void GameStarted()
        {
            audioPool = new ObjectPool<AudioSource>(CreateAudioSrc, null, null, DestroyAudioSrc, false, 5, 10);
        }
        private static void DestroyAudioSrc(AudioSource audioSrc)
        {
            Destroy(audioSrc.gameObject);
        }

        private static AudioSource CreateAudioSrc()
        {
            GameObject audioSrcGameObject = new GameObject("audioSrcGameObject", typeof(AudioSource), typeof(AudioSrcContext));
            AudioSource audioSrc = audioSrcGameObject.GetComponent<AudioSource>();

            audioSrc.volume = 1;
            audioSrc.spatialBlend = 1;
            audioSrc.rolloffMode = AudioRolloffMode.Linear;
            
            return audioSrc;
        }

        public static void PlayAudioAtLoc(AudioClip audioToPlay, Vector3 playerLoc, float volume)
        {
            AudioSource newSrc = audioPool.Get();
            newSrc.volume = volume;
            newSrc.gameObject.transform.position = playerLoc;
            newSrc.PlayOneShot(audioToPlay);

            newSrc.GetComponent<AudioSrcContext>().StartCoroutine(ReleaseAudioSrc(newSrc, audioToPlay.length));
        }
        
        public static void PlayAudioAtPlayer(AudioClip abilityAudio, float volume)
        {
            PlayAudioAtLoc(abilityAudio, UnityEngine.Camera.main.transform.position, volume);
        }

        private static IEnumerator ReleaseAudioSrc(AudioSource newSrc, float length)
        {
            yield return new WaitForSeconds(length);
            audioPool.Release(newSrc);
        }
        
        public static void SetGamePaused(bool paused)
        {
            Time.timeScale = paused ? 0 : 1;
        }
    }
}
