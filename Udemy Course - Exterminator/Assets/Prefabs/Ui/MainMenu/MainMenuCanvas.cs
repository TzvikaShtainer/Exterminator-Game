using System;
using Prefabs.Framework.LevelManager;
using Prefabs.Ui.InGameUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Prefabs.Ui.MainMenu
{
    public class MainMenuCanvas : MonoBehaviour
    {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button controlsBtn;
        [SerializeField] private Button backBtn;
        
        [SerializeField] private CanvasGroup frontUI;
        [SerializeField] private CanvasGroup controlsUI;

        [SerializeField] private LevelManager levelManager;

        private void Start()
        {
            startBtn.onClick.AddListener(StartGame);
            controlsBtn.onClick.AddListener(SwitchToControlUI);
            backBtn.onClick.AddListener(SwitchToFrontUI);
        }

        private void SwitchToFrontUI()
        {
            controlsUI.blocksRaycasts = false;
            controlsUI.alpha = 0;
            
            frontUI.blocksRaycasts = true;
            frontUI.alpha = 1;
        }

        private void SwitchToControlUI()
        {
            controlsUI.blocksRaycasts = true;
            controlsUI.alpha = 1;
            
            frontUI.blocksRaycasts = false;
            frontUI.alpha = 0;
        }

        private void StartGame()
        {
            levelManager.LoadFirstLevel();
        }
    }
}
