﻿using GlobalEnums;
using Modding;
using System.Collections;
using UnityEngine;

namespace Benchwarp
{
    public static class ChangeScene
    {
        public static void WarpToRespawn()
        {
            Events.InvokeOnBenchwarp();
            if (Benchwarp.GS.UnlockAllBenches)
            {
                UnlockBench(PlayerData.instance.GetString(nameof(PlayerData.respawnScene)));
            }
            GameManager.instance.StartCoroutine(Respawn());
        }

        private static IEnumerator Respawn()
        {
            GameManager.instance.SaveGame();
            GameManager.instance.TimePasses();
            UIManager.instance.UIClosePauseMenu();

            // Collection of various redundant attempts to fix the infamous soul orb bug
            HeroController.instance.TakeMPQuick(PlayerData.instance.MPCharge); // actually broadcasts the event
            HeroController.instance.SetMPCharge(0);
            PlayerData.instance.MPReserve = 0;
            HeroController.instance.ClearMP(); // useless
            PlayMakerFSM.BroadcastEvent("MP DRAIN"); // This is the main fsm path for removing soul from the orb
            PlayMakerFSM.BroadcastEvent("MP LOSE"); // This is an alternate path (used for bindings and other things) that actually plays an animation?
            PlayMakerFSM.BroadcastEvent("MP RESERVE DOWN");

            // Set some stuff which would normally be set by LoadSave
            HeroController.instance.AffectedByGravity(false);
            HeroController.instance.transitionState = HeroTransitionState.EXITING_SCENE;
            if (HeroController.SilentInstance != null)
            {
                if (HeroController.instance.cState.onConveyor || HeroController.instance.cState.onConveyorV || HeroController.instance.cState.inConveyorZone)
                {
                    HeroController.instance.GetComponent<ConveyorMovementHero>()?.StopConveyorMove();
                    HeroController.instance.cState.inConveyorZone = false;
                    HeroController.instance.cState.onConveyor = false;
                    HeroController.instance.cState.onConveyorV = false;
                }

                HeroController.instance.cState.nearBench = false;
            }
            GameManager.instance.cameraCtrl.FadeOut(CameraFadeType.LEVEL_TRANSITION);

            yield return new WaitForSecondsRealtime(0.5f);

            // Actually respawn the character
            GameManager.instance.SetPlayerDataBool(nameof(PlayerData.atBench), false);
            // Allow the player to have control if they warp to a non-bench while diving or cdashing
            if (HeroController.SilentInstance != null)
            {
                HeroController.instance.cState.superDashing = false;
                HeroController.instance.cState.spellQuake = false;
            }
            GameManager.instance.ReadyForRespawn(false);

            yield return new WaitWhile(() => GameManager.instance.IsInSceneTransition);

            EventRegister.SendEvent("UPDATE BLUE HEALTH"); // checks if hp is adjusted for Joni's blessing

            // Revert pause menu timescale
            Time.timeScale = 1f;
            GameManager.instance.FadeSceneIn();

            // We have to set the game non-paused because TogglePauseMenu sucks and UIClosePauseMenu doesn't do it for us.
            GameManager.instance.isPaused = false;

            // Restore various things normally handled by exiting the pause menu. None of these are necessary afaik
            GameCameras.instance.ResumeCameraShake();
            if (HeroController.SilentInstance != null)
            {
                HeroController.instance.UnPause();
            }
            MenuButtonList.ClearAllLastSelected();

            //This allows the next pause to stop the game correctly
            TimeController.GenericTimeScale = 1f;

            // Restores audio to normal levels. Unfortunately, some warps pop atm when music changes over
            GameManager.instance.actorSnapshotUnpaused.TransitionTo(0f);
            GameManager.instance.ui.AudioGoToGameplay(.2f);
        }

        public static void OnBenchwarpCleanup()
        {
            // reset some things not cleaned up when exiting dream sequences, etc
            if (HeroController.SilentInstance != null)
            {
                HeroController.SilentInstance.takeNoDamage = false;
                if (!BenchMaker.IsDreamRoom() && HeroController.SilentInstance.proxyFSM?.FsmVariables?.FindFsmBool("No Charms") is HutongGames.PlayMaker.FsmBool noCharms) noCharms.Value = false;
            }
            if (HutongGames.PlayMaker.FsmVariables.GlobalVariables.FindFsmBool("Is HUD Out") is HutongGames.PlayMaker.FsmBool hudOut && hudOut.Value)
            {
                PlayMakerFSM.BroadcastEvent("IN");
            }
        }

        public static void ChangeToScene(string sceneName, string gateName, float delay = 0f)
        {
            Events.InvokeOnDoorwarp(sceneName, gateName);
            UIManager.instance.UIClosePauseMenu();
            Time.timeScale = 1f;
            GameManager.instance.FadeSceneIn();
            GameManager.instance.isPaused = false;
            GameCameras.instance.ResumeCameraShake();
            if (HeroController.SilentInstance != null)
            {
                HeroController.instance.UnPause();
            }
            MenuButtonList.ClearAllLastSelected();
            TimeController.GenericTimeScale = 1f;
            GameManager.instance.actorSnapshotUnpaused.TransitionTo(0f);
            GameManager.instance.ui.AudioGoToGameplay(.2f);
            if (HeroController.SilentInstance != null)
            {
                HeroController.instance.UnPause();
            }
            MenuButtonList.ClearAllLastSelected();
            PlayerData.instance.atBench = false; // kill bench storage
            if (HeroController.SilentInstance != null)
            {
                if (HeroController.instance.cState.onConveyor || HeroController.instance.cState.onConveyorV || HeroController.instance.cState.inConveyorZone)
                {
                    HeroController.instance.GetComponent<ConveyorMovementHero>()?.StopConveyorMove();
                    HeroController.instance.cState.inConveyorZone = false;
                    HeroController.instance.cState.onConveyor = false;
                    HeroController.instance.cState.onConveyorV = false;
                }

                HeroController.instance.cState.nearBench = false;
            }

            SceneLoad load = ReflectionHelper.GetField<GameManager, SceneLoad>(GameManager.instance, "sceneLoad");
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
            ReflectionHelper.SetField<GameManager, SceneLoad>(GameManager.instance, "sceneLoad", null);

            GameManager.instance.BeginSceneTransition(new DoorwarpSceneLoadInfo
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

        // Some mods (ItemChanger) check type to detect vanilla scene loads.
        private class DoorwarpSceneLoadInfo : GameManager.SceneLoadInfo 
        {
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

        static readonly (string, string)[] _lockedBenches = new (string, string)[]
        {
            ("Hive_01", "Hive Bench"),
            ("Ruins1_31", "Toll Machine Bench"),
            ("Abyss_18", "Toll Machine Bench"),
            ("Fungus3_50", "Toll Machine Bench")
        };
        private static void UnlockBench(string sceneName)
        {
            if (_lockedBenches.FirstOrDefault(p => p.Item1 == sceneName).Item2 is string id)
            {
                GameManager.instance.sceneData.SaveMyState(new PersistentBoolData
                {
                    activated = true,
                    sceneName = sceneName,
                    semiPersistent = false,
                    id = id
                });
            }

            switch (sceneName)
            {
                /*
                case "Town":
                    PlayerData.instance.SetBool(nameof(PlayerData.visitedDirtmouth), true);
                    break;
                case "Crossroads_04":
                    PlayerData.instance.SetBool(nameof(PlayerData.visitedCrossroads), true);
                    break;
                case "White_Palace_01":
                    PlayerData.instance.SetBool(nameof(PlayerData.visitedWhitePalace), true);
                    break;
                */
                case "Room_Tram":
                    PlayerData.instance.SetBool(nameof(PlayerData.openedTramLower), true);
                    PlayerData.instance.SetBool(nameof(PlayerData.tramOpenedDeepnest), true);
                    break;
                case "Room_Tram_RG":
                    PlayerData.instance.SetBool(nameof(PlayerData.openedTramRestingGrounds), true);
                    PlayerData.instance.SetBool(nameof(PlayerData.tramOpenedCrossroads), true);
                    break;
            }
        }
    }
}
