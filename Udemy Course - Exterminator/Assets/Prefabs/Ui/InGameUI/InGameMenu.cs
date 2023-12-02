using System;
using Prefabs.Framework.LevelManager;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs.Ui.InGameUI
{
    public class InGameMenu : MonoBehaviour
    {
        [SerializeField] private Button resumeBtn;
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button mainMenuBtn;
        
        [SerializeField] private UIManager uiManger;
        [SerializeField] private LevelManager levelManager;

        private void Start()
        {
            resumeBtn.onClick.AddListener(ResumeGame);
            restartBtn.onClick.AddListener(RestartLevel);
            mainMenuBtn.onClick.AddListener(BackToMainMenu);
        }

        private void BackToMainMenu()
        {
            levelManager.GoToMainMenu();
        }

        private void RestartLevel()
        {
            levelManager.RestartCurrentLevel();
        }

        private void ResumeGame()
        {
            uiManger.SwitchToGameplayUI();
        }
    }
}
