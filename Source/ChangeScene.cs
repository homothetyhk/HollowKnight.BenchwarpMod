using GlobalEnums;
using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Benchwarp
{
    public static class ChangeScene
    {
        public static void ChangeToScene(string sceneName, string gateName, float delay = 0f)
        {
            UIManager.instance.TogglePauseGame();
            /*
            UIManager.instance.UIClosePauseMenu();
            
            GameManager.instance.FadeSceneIn();
            GameManager.instance.isPaused = false;
            GameCameras.instance.ResumeCameraShake();
            */
            if (HeroController.instance != null)
            {
                HeroController.instance.UnPause();
            }
            //MenuButtonList.ClearAllLastSelected();
            //TimeController.GenericTimeScale = 1f;
            Time.timeScale = 1f;
            GameManager.instance.actorSnapshotUnpaused.TransitionTo(0f);
            GameManager.instance.ui.AudioGoToGameplay(.2f);
            if (HeroController.instance != null)
            {
                HeroController.instance.UnPause();
            }
            //MenuButtonList.ClearAllLastSelected();
            PlayerData.instance.atBench = false; // kill bench storage

            GameManager.instance.ChangeToScene(sceneName, gateName, delay);
        }
    }
}
