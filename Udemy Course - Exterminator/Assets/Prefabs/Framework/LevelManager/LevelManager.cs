using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prefabs.Framework.LevelManager
{
    [CreateAssetMenu(menuName = "LevelManager")]
    public class LevelManager : ScriptableObject
    {
        [SerializeField] private int mainMenuBuildIndex = 0;
        [SerializeField] private int firstLevelBuildIndex = 1;

        public delegate void OnLevelFinished();
        public static event OnLevelFinished onLevelFinished;

        public void GoToMainMenu()
        {
            LoadSceneByIndex(mainMenuBuildIndex);
        }

        public void LoadFirstLevel()
        {
            LoadSceneByIndex(firstLevelBuildIndex);
        }
        
        public void RestartCurrentLevel()
        {
            LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex);
        }

        void LoadSceneByIndex(int index)
        {
            SceneManager.LoadScene(index);
            GamePlayStatics.SetGamePaused(false);
        }

        public static void LevelFinished()
        {
            onLevelFinished?.Invoke();
        }
    }
}
