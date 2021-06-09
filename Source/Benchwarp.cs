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
    public class Benchwarp : Mod, ITogglableMod,IGlobalSettings<GlobalSettings>,ILocalSettings<SaveSettings>
    {

        internal static Benchwarp instance;

        internal GameObject UIObj;
        internal GlobalSettings globalSettings = new GlobalSettings();
        internal SaveSettings saveSettings = new SaveSettings();
        public void OnLoadGlobal(GlobalSettings s) => globalSettings = s;

        public GlobalSettings OnSaveGlobal() => globalSettings;

        public void OnLoadLocal(SaveSettings s) => saveSettings = s;

        public SaveSettings OnSaveLocal() => saveSettings;
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

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloaded)
        {
            BenchMaker.GetPrefabs(preloaded);
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += BenchMaker.TryToDeploy;

            UIObj = new GameObject();
            UIObj.AddComponent<GUIController>();
            GameObject.DontDestroyOnLoad(UIObj);

            ModHooks.SetPlayerBoolHook += BenchWatcher;
            ModHooks.ApplicationQuitHook += SaveGlobalSettings;

            ModHooks.GetPlayerStringHook += RespawnAtDeployedBench;
            ModHooks.SetPlayerStringHook += RemoveRespawnFromDeployedBench;

            GUIController.Instance.BuildMenus();
            
            if (Benchwarp.instance.saveSettings.benchDeployed && GameManager.instance.sceneName == Benchwarp.instance.saveSettings.benchScene)
            {
                BenchMaker.MakeBench();
            }

            // Imagine if GetPlayerIntHook actually worked
            On.GameManager.OnNextLevelReady += FixRespawnType;

            Hotkeys.ApplyHotkeyOverrides();
        }

        public override string GetVersion()
        {
            return "2.4.1";
        }

        public override int LoadPriority()
        {
            return 100;
        }

        public override List<(string, string)> GetPreloadNames()
        {
            BenchMaker.SetInitialSettings();
            
            if (globalSettings.NoPreload)
            {
                return new List<(string, string)>();
            }

            if (globalSettings.ReducePreload)
            {
                try
                {
                    Bench bench = Bench.GetStyleBench(globalSettings.benchStyle);
                    List<(string, string)> preloads = new List<(string, string)> { (bench.sceneName, bench.respawnMarker) };
                    if (globalSettings.benchStyle == "Camp")
                    {
                        preloads.Add(("Deepnest_East_13", "outskirts__0003_camp"));
                    }
                    if (globalSettings.benchStyle == "Garden")
                    {
                        preloads.Add(("Fungus1_24", "guardian_bench"));
                    }
                    return preloads;
                }
                catch (System.Exception e)
                {
                    LogError(e);
                }
            }

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

        public void Warp()
        {
            if (globalSettings.UnlockAllBenches) 
                RepairBench(PlayerData.instance.GetString(nameof(PlayerData.respawnScene)));
            GameManager.instance.StartCoroutine(Respawn());
        }

        static (string, string)[] sdBenches = new (string, string)[]
        {
            ("Hive_01", "Hive Bench"),
            ("Ruins1_31", "Toll Machine Bench"),
            ("Abyss_18", "Toll Machine Bench"),
            ("Fungus3_50", "Toll Machine Bench")
        };
        private static void RepairBench(string sceneName)
        {
            if (sdBenches.FirstOrDefault(p => p.Item1 == sceneName).Item2 is string id)
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
                case "Crossroads_04":
                    PlayerData.instance.visitedCrossroads = true;
                    break;
                case "Room_Tram":
                    PlayerData.instance.openedTramLower = true;
                    PlayerData.instance.tramOpenedDeepnest = true;
                    break;
                case "Room_Tram_RG":
                    PlayerData.instance.openedTramRestingGrounds = true;
                    PlayerData.instance.tramOpenedCrossroads = true;
                    break;
            }
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
        }

        public void BenchWatcher(string target, bool val)
        {
            if (target == "atBench" && val)
            {
                foreach (Bench bench in Bench.Benches)
                {
                    if (bench.benched) bench.visited = true;
                    else if (GameManager.instance.sceneName == bench.sceneName && Benchwarp.instance.saveSettings.benchScene != bench.sceneName) bench.visited = true;
                    else continue;
                    break;
                }
            }
            PlayerData.instance.SetBoolInternal(target, val);
        }

        private void RemoveRespawnFromDeployedBench(string stringName, string value)
        {
            switch (stringName)
            {
                case nameof(PlayerData.respawnMarkerName):
                    if (value != BenchMaker.DEPLOYED_BENCH_RESPAWN_MARKER_NAME)
                    {
                        Benchwarp.instance.saveSettings.atDeployedBench = false;
                    }
                    break;
                case nameof(PlayerData.respawnScene):
                    if (value != Benchwarp.instance.saveSettings.benchScene)
                    {
                        Benchwarp.instance.saveSettings.atDeployedBench = false;
                    }
                    break;
            }
            PlayerData.instance.SetStringInternal(stringName, value);
        }

        private string RespawnAtDeployedBench(string stringName)
        {
            if (!Benchwarp.instance.saveSettings.atDeployedBench) return PlayerData.instance.GetStringInternal(stringName);
            switch (stringName)
            {
                case nameof(PlayerData.respawnMarkerName):
                    return BenchMaker.DEPLOYED_BENCH_RESPAWN_MARKER_NAME;
                case nameof(PlayerData.respawnScene):
                    return Benchwarp.instance.saveSettings.benchScene;
                default:
                    return PlayerData.instance.GetStringInternal(stringName);
            }
        }

        private void FixRespawnType(On.GameManager.orig_OnNextLevelReady orig, GameManager self)
        {
            if (GameManager.instance.RespawningHero)
            {
                Transform spawnPoint = HeroController.instance.LocateSpawnPoint();
                if (spawnPoint != null && spawnPoint.gameObject != null
                    && spawnPoint.gameObject.GetComponents<PlayMakerFSM>().Any(fsm => fsm.FsmName == "Bench Control"))
                {
                    PlayerData.instance.respawnType = 1;
                }
                else
                {
                    PlayerData.instance.respawnType = 0;
                }
            }

            orig(self);
        }

        internal void ApplyUnlockAllFixes() {
            if (!globalSettings.UnlockAllBenches) return;

            PlayerData pd = PlayerData.instance;

            FieldInfo[] fields = typeof(PlayerData).GetFields();

            // Most of these are unnecessary, but some titlecards can lock you into a bench
            foreach
            (
                FieldInfo fi in fields.Where
                (
                    x => x.Name.StartsWith("visited")
                        || x.Name.StartsWith("tramOpened")
                        || x.Name.StartsWith("openedTram")
                        || x.Name.StartsWith("tramOpened")
                )
            )
            {
                pd.SetBoolInternal(fi.Name, true);
            }

            //This actually fixes the unlockable benches
            SceneData sd = GameManager.instance.sceneData;

            foreach ((string sceneName, string id) in new (string, string)[]
            {
                ("Hive_01", "Hive Bench"),
                ("Ruins1_31", "Toll Machine Bench"),
                ("Abyss_18", "Toll Machine Bench"),
                ("Fungus3_50", "Toll Machine Bench")
            })
            {
                sd.SaveMyState
                (
                    new PersistentBoolData
                    {
                        sceneName = sceneName,
                        id = id,
                        activated = true,
                        semiPersistent = false
                    }
                );
            }
        }

        public void Unload()
        {
            ModHooks.SetPlayerBoolHook -= BenchWatcher;
            ModHooks.ApplicationQuitHook -= SaveGlobalSettings;

            ModHooks.GetPlayerStringHook -= RespawnAtDeployedBench;
            ModHooks.SetPlayerStringHook -= RemoveRespawnFromDeployedBench;

            On.GameManager.OnNextLevelReady -= FixRespawnType;

            BenchMaker.DestroyBench(DontDeleteData: true);
            GameObject.Destroy(TopMenu.canvas);
            GameObject.Destroy(UIObj);
        }

        new public void SaveGlobalSettings()
        {
            base.SaveGlobalSettings();
        }

        
    }
}
