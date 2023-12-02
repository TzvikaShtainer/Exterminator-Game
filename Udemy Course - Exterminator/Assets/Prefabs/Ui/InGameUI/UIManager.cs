using System.Collections.Generic;
using Audio;
using Prefabs.Framework;
using Prefabs.Framework.LevelManager;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Prefabs.Ui.InGameUI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup gameplayControl;
        [SerializeField] private CanvasGroup pausedMenu;
        [SerializeField] private CanvasGroup shop;
        [SerializeField] private CanvasGroup deathMenu;
        [SerializeField] private CanvasGroup winMenu;
        [SerializeField] private UIAudioPlayer UIAudioPlayer;
        
        [SerializeField] private Button pausedBtn;

        private List<CanvasGroup> allChildern = new List<CanvasGroup>();

        private CanvasGroup currActiveGroup;

        private void Start()
        {
            pausedBtn.onClick.AddListener(SwitchToPausedMenu);
            
            List<CanvasGroup> children = new List<CanvasGroup>();
            GetComponentsInChildren(true, children);
            foreach (CanvasGroup child in children)
            {
                if (child.transform.parent == transform)
                {
                    allChildern.Add(child);
                    SetGroupActive(child, false, false);
                }
            }

            if (allChildern.Count != 0)
            {
                SetCurrentActiveGroup(allChildern[0]);
            }

            LevelManager.onLevelFinished += LevelFinished;
        }

        private void LevelFinished()
        {
           SetCurrentActiveGroup(winMenu);
           GamePlayStatics.SetGamePaused(true);
           UIAudioPlayer.PlayWin();
        }

        private void SetCurrentActiveGroup(CanvasGroup canvasGroup)
        {
            if (currActiveGroup != null)
            {
                SetGroupActive(currActiveGroup, false, false);
            }

            currActiveGroup = canvasGroup;
            SetGroupActive(currActiveGroup, true, true);
        }

        private void SetGroupActive(CanvasGroup child, bool interactable, bool visible)
        {
            child.interactable = interactable;
            child.blocksRaycasts = interactable;
            child.alpha = visible ? 1 : 0;
        }

        public void SetGameplayControlEnabled(bool enabled)
        {
            SetCanvasGroupEnabled(gameplayControl, enabled);
        }
    
        public void SwitchToPausedMenu()
        {
            SetCurrentActiveGroup(pausedMenu);
            GamePlayStatics.SetGamePaused(true);
        }
    
        private void SetCanvasGroupEnabled(CanvasGroup grp, bool enabled)
        {
            grp.interactable = enabled;
            grp.blocksRaycasts = enabled;
        }

        public void SwitchToShop()
        {
            SetCurrentActiveGroup(shop);
            GamePlayStatics.SetGamePaused(true);
        }

        public void SwitchToGameplayUI()
        {
            SetCurrentActiveGroup(gameplayControl);
            GamePlayStatics.SetGamePaused(false);
        }

        public void SwitchToDeathMenu()
        {
            SetCurrentActiveGroup(deathMenu);
            GamePlayStatics.SetGamePaused(true);
        }
    }
}
