using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalEnums;
using System.Reflection;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using TMPro;

namespace Benchwarp
{
    public class Benchwarp : Mod<SaveSettings, GlobalSettings>, ITogglableMod
    {

        internal static Benchwarp instance;

        internal GameObject UIObj;

        public Benchwarp()
        {
            instance = this;
            try
            {
                DoorWarp.Load();
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }

        public override void Initialize()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ResetSaveSettings;
            UIObj = new GameObject();
            UIObj.AddComponent<GUIController>();
            GameObject.DontDestroyOnLoad(UIObj);

            //ModHooks.Instance.SetPlayerBoolHook += BenchWatcher;
            //UnityEngine.SceneManagement.SceneManager.activeSceneChanged += CheckForKingsPassUnlock;
            ModHooks.Instance.ApplicationQuitHook += SaveGlobalSettings;

            GUIController.Instance.BuildMenus();
        }

        public override string GetVersion()
        {
            return "1221.1.0";
        }

        public override int LoadPriority()
        {
            return 100;
        }

        public IEnumerator Respawn()
        {
            GameManager.instance.SaveGame();
            GameManager.instance.ResetSemiPersistentItems();

            UIManager.instance.TogglePauseGame();
            //yield return new WaitForSecondsRealtime(0.5f);
            
            //Time.timeScale = 1f;

            // Collection of various redundant attempts to fix the infamous soul orb bug
            HeroController.instance.SetMPCharge(0);
            PlayerData.instance.MPReserve = 0;
            HeroController.instance.ClearMP(); // useless
            PlayMakerFSM.BroadcastEvent("MP DRAIN"); // This is the main fsm path for removing soul from the orb
            PlayMakerFSM.BroadcastEvent("MP LOSE"); // This is an alternate path (used for bindings and other things) that actually plays an animation?
            PlayMakerFSM.BroadcastEvent("MP RESERVE DOWN");

            // Actually respawn the character
            GameManager.instance.SetPlayerDataBool(nameof(PlayerData.atBench), false);
            GameManager.instance.ReadyForRespawn();

            yield return new WaitUntil(() => GameManager.instance.gameState == GameState.ENTERING_LEVEL);

            // Revert pause menu timescale
            Time.timeScale = 1f;
            GameManager.instance.FadeSceneIn();

            // We have to set the game non-paused because TogglePauseMenu sucks and UIClosePauseMenu doesn't do it for us.
            GameManager.instance.isPaused = false;

            // Restore various things normally handled by exiting the pause menu. None of these are necessary afaik
            GameCameras.instance.ResumeCameraShake();
            if (HeroController.instance != null)
            {
                HeroController.instance.UnPause();
            }

            // Restores audio to normal levels. Unfortunately, some warps pop atm when music changes over
            GameManager.instance.actorSnapshotUnpaused.TransitionTo(0f);
            GameManager.instance.ui.AudioGoToGameplay(.2f);
        }

        /*
        public void BenchWatcher(string target, bool val)
        {
            if (target == "atBench" && val)
            {
                foreach (Bench bench in Bench.Benches)
                {
                    if (bench.benched) bench.visited = true;
                    else continue;
                    break;
                }
            }
            PlayerData.instance.SetBoolInternal(target, val);
        }

        private void CheckForKingsPassUnlock(Scene arg0, Scene arg1)
        {
            switch (arg1.name)
            {
                case "Tutorial_01":
                    try
                    {
                        Bench kp = Bench.Benches.First(b => b.sceneName == "Tutorial_01");
                        kp.visited = true;
                    }
                    catch
                    {
                        LogError("Error occurred while attempting to set King's Pass bench as visited.");
                    }
                    break;
            }
        }
        */

        public void Unload()
        {
            //ModHooks.Instance.SetPlayerBoolHook -= BenchWatcher;
            //UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= CheckForKingsPassUnlock;
            ModHooks.Instance.ApplicationQuitHook -= SaveGlobalSettings;

            GameObject.Destroy(TopMenu.canvas);
            GameObject.Destroy(UIObj);
        }

        public void ResetSaveSettings(Scene from, Scene to)
        {
            if (to.name == "Menu_Title") Benchwarp.instance.Settings = new SaveSettings();
        }

        new public void SaveGlobalSettings()
        {
            base.SaveGlobalSettings();
        }
    }
}
