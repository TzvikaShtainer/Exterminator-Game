using Prefabs.Framework;
using UnityEngine;

namespace Prefabs.Enemy.Spawner
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private GameObject[] objectToSpawn;
        [SerializeField] private Transform spawnTransform;

        [Header("Audio")]
        [SerializeField] private AudioClip spawnAudio;
        [SerializeField] private float volume;
        
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public bool StartSpawn()
        {
            if (objectToSpawn.Length == 0) return false;

            if (animator != null)
            {
                animator.SetTrigger("Spawn");
            }
            else
            {
                SpawnImpl();
            }

            Vector3 audioPos = transform.position;
            GamePlayStatics.PlayAudioAtLoc(spawnAudio, audioPos, volume);
            
            return true;
        }

        public void SpawnImpl()
        {
            int randomPick = Random.Range(0, objectToSpawn.Length);
            GameObject newSpawn = Instantiate(objectToSpawn[randomPick], spawnTransform.position, spawnTransform.rotation);

            ISpawnInterface newSpawnInterface = newSpawn.GetComponent<ISpawnInterface>();
            if (newSpawnInterface != null)
            {
                newSpawnInterface.SpawnedBy(gameObject);
            }
        }
    }
}
