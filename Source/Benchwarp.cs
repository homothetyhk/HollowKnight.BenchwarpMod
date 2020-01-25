using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalEnums;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using TMPro;

namespace Benchwarp
{
    public class Benchwarp : Mod<SaveSettings, GlobalSettings>, ITogglableMod
    {

        internal static Benchwarp instance;

        internal GameObject UIObj;

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloaded)
        {
            instance = this;

            instance.Log("Initializing");

            RandomizerStartLocation.CheckForRandomizer();

            Bench.GenerateBenchData();
            BenchMaker.GetPrefabs(preloaded);
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += BenchMaker.TryToDeploy;

            UIObj = new GameObject();
            UIObj.AddComponent<GUIController>();
            GameObject.DontDestroyOnLoad(UIObj);

            ModHooks.Instance.SetPlayerBoolHook += BenchWatcher;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ClearSettings;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += CheckForKingsPassUnlock;
            ModHooks.Instance.ApplicationQuitHook += SaveGlobalSettings;

            ModHooks.Instance.GetPlayerStringHook += RescueSave;

            ModHooks.Instance.GetPlayerStringHook += RespawnAtDeployedBench;
            ModHooks.Instance.GetPlayerIntHook += RespawnAtDeployedBench2;
            ModHooks.Instance.SetPlayerStringHook += RemoveRespawnFromDeployedBench;

            GUIController.Instance.BuildMenus();
            
            if (Benchwarp.instance.Settings.benchDeployed && GameManager.instance.sceneName == Benchwarp.instance.Settings.benchScene)
            {
                BenchMaker.MakeBench();
            }

        }

        public override string GetVersion()
        {
            return "2.0";
        }

        public override int LoadPriority()
        {
            return 100;
        }

        public override List<(string, string)> GetPreloadNames()
        {

            return new List<(string, string)>
                {
                ("Crossroads_30", "RestBench"),
                ("Town", "RestBench"),
                ("Crossroads_04", "RestBench"),
                ("Crossroads_ShamanTemple", "BoneBench"),
                ("Fungus1_37", "RestBench"),
                ("Room_Slug_Shrine", "RestBench"),
                ("Fungus3_archive", "RestBench"),
                ("Fungus2_26", "RestBench"),
                ("Fungus2_31", "RestBench"),
                ("Ruins_Bathhouse", "RestBench"),
                ("Waterways_02", "RestBench"),
                ("GG_Atrium", "RestBench"),
                ("Deepnest_Spider_Town", "RestBench Return"),
                ("Deepnest_East_13", "RestBench"),
                ("Deepnest_East_13", "outskirts__0003_camp"),
                ("Room_Colosseum_02", "RestBench"),
                ("Fungus1_24", "RestBench"),
                ("Fungus1_24", "guardian_bench"),
                ("Room_Tram", "RestBench"),
                ("Room_nailmaster", "RestBench"),
                ("Deepnest_East_06", "RestBench"),
                ("Fungus1_15", "RestBench"),
                ("White_Palace_01", "WhiteBench"),
                ("Room_Final_Boss_Atrium", "RestBench")
                };
        }

        public IEnumerator Respawn()
        {
            GameManager.instance.SaveGame();
            GameManager.instance.ResetSemiPersistentItems();
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
            GameManager.instance.cameraCtrl.FadeOut(CameraFadeType.LEVEL_TRANSITION);

            yield return new WaitForSecondsRealtime(0.5f);

            // Actually respawn the character
            GameManager.instance.SetPlayerDataBool(nameof(PlayerData.atBench), false);
            GameManager.instance.ReadyForRespawn(false);

            yield return new WaitWhile(() => GameManager.instance.IsInSceneTransition);

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
            MenuButtonList.ClearAllLastSelected();

            //This allows the next pause to stop the game correctly
            TimeController.GenericTimeScale = 1f;

            // Restores audio to normal levels. Unfortunately, some warps pop atm when music changes over
            GameManager.instance.actorSnapshotUnpaused.TransitionTo(0f);
            GameManager.instance.ui.AudioGoToGameplay(.2f);

            /*
             * Removed flower break
             * For one, it did absolutely nothing, since you could just select a bench and savequit instead of warping
             * Also, writing room rando in such a way that it actually checks for a path from Grey Mourner to QG would be unspeakably awful
             * So no
             * Cheaters rejoice
             */
        }

        public void BenchWatcher(string target, bool val)
        {
            if (target == "atBench" && val)
            {
                foreach (Bench bench in Bench.Benches)
                {
                    if (bench.benched) bench.visited = true;
                    else if (GameManager.instance.sceneName == bench.sceneName && Benchwarp.instance.Settings.benchScene != bench.sceneName) bench.visited = true;
                    else continue;
                    break;
                }
            }
            PlayerData.instance.SetBoolInternal(target, val);
        }


        private string RescueSave(string stringName)
        {
            if (!Benchwarp.instance.GlobalSettings.CheckForBrokenSaveFile) return PlayerData.instance.GetStringInternal(stringName);

            if (stringName == "respawnScene" && !Benchwarp.instance.Settings.atDeployedBench)
            {
                foreach (Bench bench in Bench.Benches)
                {
                    if (bench.benched) return PlayerData.instance.GetStringInternal(stringName);
                }
                if (PlayerData.instance.respawnType == 1)
                {
                    LogError("Attempted to load into unrecognized bench. Relocating to Dirtmouth.\n" +
                        "If you would like to disable this check in the future, please visit the Benchwarp GlobalSettings " +
                        "and change key \"CheckForBrokenSaveFile\" to false.");

                    PlayerData.instance.respawnScene = "Town";
                    PlayerData.instance.respawnMarkerName = "RestBench";
                    PlayerData.instance.mapZone = MapZone.TOWN;
                    GameManager.instance.SaveGame();
                    return "Town";
                }
            }
            return PlayerData.instance.GetStringInternal(stringName);
        }

        private void RemoveRespawnFromDeployedBench(string stringName, string value)
        {
            switch (stringName)
            {
                case nameof(PlayerData.respawnMarkerName):
                    if (value != BenchMaker.DEPLOYED_BENCH_RESPAWN_MARKER_NAME)
                    {
                        Benchwarp.instance.Settings.atDeployedBench = false;
                    }
                    break;
                case nameof(PlayerData.respawnScene):
                    if (value != Benchwarp.instance.Settings.benchScene)
                    {
                        Benchwarp.instance.Settings.atDeployedBench = false;
                    }
                    break;
            }
            PlayerData.instance.SetStringInternal(stringName, value);
        }

        private string RespawnAtDeployedBench(string stringName)
        {
            if (!Benchwarp.instance.Settings.atDeployedBench) return PlayerData.instance.GetStringInternal(stringName);
            switch (stringName)
            {
                case nameof(PlayerData.respawnMarkerName):
                    return BenchMaker.DEPLOYED_BENCH_RESPAWN_MARKER_NAME;
                case nameof(PlayerData.respawnScene):
                    return Benchwarp.instance.Settings.benchScene;
                default:
                    return PlayerData.instance.GetStringInternal(stringName);
            }
        }

        private int RespawnAtDeployedBench2(string intName)
        {
            if (!Benchwarp.instance.Settings.atDeployedBench || intName != nameof(PlayerData.respawnType))
            {
                return PlayerData.instance.GetIntInternal(intName);
            }
            else return 1;
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

        public void ClearSettings(Scene arg0, Scene arg1)
        {
            if (arg1.name == "Menu_Title")
            {
                foreach (Bench bench in Bench.Benches)
                {
                    bench.visited = false;
                }

                Benchwarp.instance.Settings.benchDeployed = false;
                Benchwarp.instance.Settings.atDeployedBench = false;
                Benchwarp.instance.Settings.benchScene = null;
                Benchwarp.instance.Settings.benchX = 0f;
                Benchwarp.instance.Settings.benchY = 0f;
            }
        }

        public void Unload()
        {
            ModHooks.Instance.SetPlayerBoolHook -= BenchWatcher;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= CheckForKingsPassUnlock;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= ClearSettings;
            ModHooks.Instance.ApplicationQuitHook -= SaveGlobalSettings;

            ModHooks.Instance.GetPlayerStringHook -= RescueSave;

            ModHooks.Instance.GetPlayerStringHook -= RespawnAtDeployedBench;
            ModHooks.Instance.GetPlayerIntHook -= RespawnAtDeployedBench2;
            ModHooks.Instance.SetPlayerStringHook -= RemoveRespawnFromDeployedBench;

            BenchMaker.DestroyBench(DontDeleteData: true);
            Object.Destroy(TopMenu.canvas);
            Object.Destroy(UIObj);
        }
    }
}
