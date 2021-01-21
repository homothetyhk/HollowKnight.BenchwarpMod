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
            UIManager.instance.UIClosePauseMenu();
            Time.timeScale = 1f;
            GameManager.instance.FadeSceneIn();
            GameManager.instance.isPaused = false;
            GameCameras.instance.ResumeCameraShake();
            if (HeroController.instance != null)
            {
                HeroController.instance.UnPause();
            }
            MenuButtonList.ClearAllLastSelected();
            TimeController.GenericTimeScale = 1f;
            GameManager.instance.actorSnapshotUnpaused.TransitionTo(0f);
            GameManager.instance.ui.AudioGoToGameplay(.2f);
            if (HeroController.instance != null)
            {
                HeroController.instance.UnPause();
            }
            MenuButtonList.ClearAllLastSelected();
            PlayerData.instance.atBench = false; // kill bench storage

            SceneLoad load = ReflectionHelper.GetAttr<GameManager, SceneLoad>(GameManager.instance, "sceneLoad");
            if (load != null)
            {
                load.Finish += () =>
                {
                    LoadScene(sceneName, gateName, delay);
                };
            }
            else
            {
                LoadScene(sceneName, gateName, delay);
            }
        }

        private static void LoadScene(string sceneName, string gateName, float delay)
        {
            GameManager.instance.StopAllCoroutines();
            ReflectionHelper.SetAttr<GameManager, SceneLoad>(GameManager.instance, "sceneLoad", null);

            GameManager.instance.BeginSceneTransition(new GameManager.SceneLoadInfo
            {
                IsFirstLevelForPlayer = false,
                SceneName = sceneName,
                HeroLeaveDirection = GetGatePosition(gateName),
                EntryGateName = gateName,
                EntryDelay = delay,
                PreventCameraFadeOut = false,
                WaitForSceneTransitionCameraFade = true,
                Visualization = GameManager.SceneLoadVisualizations.Default,
                AlwaysUnloadUnusedAssets = false
            });
        }

        private static GatePosition GetGatePosition(string name)
        {
            if (name.Contains("top"))
            {
                return GatePosition.top;
            }

            if (name.Contains("bot"))
            {
                return GatePosition.bottom;
            }

            if (name.Contains("left"))
            {
                return GatePosition.left;
            }

            if (name.Contains("right"))
            {
                return GatePosition.right;
            }

            if (name.Contains("door"))
            {
                return GatePosition.door;
            }

            return GatePosition.unknown;
        }
    }
}
