using System;
using System.Collections.Generic;
using System.Linq;
using Modding;
using UnityEngine;

namespace Benchwarp
{
    public class Benchwarp : Mod, ITogglableMod, IGlobalSettings<GlobalSettings>, ILocalSettings<SaveSettings>, IMenuMod
    {
        internal static Benchwarp instance;
        public Benchwarp()
        {
            instance = this;
        }

        public static GlobalSettings GS { get; private set; } = new GlobalSettings();
        public static SaveSettings LS { get; private set; } = new SaveSettings();

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloaded)
        {
            ObjectCache.SavePreloads(preloaded);

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += BenchMaker.TryToDeploy;
            ModHooks.SetPlayerBoolHook += BenchWatcher;
            ModHooks.GetPlayerStringHook += RespawnAtDeployedBench;
            ModHooks.SetPlayerStringHook += RemoveRespawnFromDeployedBench;
            // Imagine if GetPlayerIntHook actually worked
            On.GameManager.OnNextLevelReady += FixRespawnType;

            GUIController.Setup();
            GUIController.Instance.BuildMenus();

            if (Benchwarp.LS.benchDeployed && GameManager.instance.sceneName == Benchwarp.LS.benchScene)
            {
                BenchMaker.MakeBench(); // Since the mod could be reenabled in any scene
            }

            if (Hotkeys.CurrentHotkeys.Count == 0)
            {
                Hotkeys.ApplyHotkeyOverrides();
            }
        }

        public override string GetVersion()
        {
            return typeof(Benchwarp).Assembly.GetName().Version.ToString();
        }

        public override List<(string, string)> GetPreloadNames()
        {
            return ObjectCache.GetPreloadNames();
        }
        

        public bool BenchWatcher(string target, bool val)
        {
            if (target == "atBench" && val)
            {
                foreach (Bench bench in Bench.Benches)
                {
                    if (bench.AtBench()) bench.SetVisited(true);
                    else if (GameManager.instance.sceneName == bench.sceneName && Benchwarp.LS.benchScene != bench.sceneName) bench.SetVisited(true);
                    else continue;
                    break;
                }
            }
            return val;
        }

        private string RemoveRespawnFromDeployedBench(string stringName, string value)
        {
            switch (stringName)
            {
                case nameof(PlayerData.respawnMarkerName):
                    if (value != BenchMaker.DEPLOYED_BENCH_RESPAWN_MARKER_NAME)
                    {
                        Benchwarp.LS.atDeployedBench = false;
                    }
                    break;
                case nameof(PlayerData.respawnScene):
                    if (value != Benchwarp.LS.benchScene)
                    {
                        Benchwarp.LS.atDeployedBench = false;
                    }
                    break;
            }
            return value;
        }

        private string RespawnAtDeployedBench(string stringName, string value)
        {
            if (!Benchwarp.LS.atDeployedBench) return value;
            switch (stringName)
            {
                case nameof(PlayerData.respawnMarkerName):
                    return BenchMaker.DEPLOYED_BENCH_RESPAWN_MARKER_NAME;
                case nameof(PlayerData.respawnScene):
                    return Benchwarp.LS.benchScene;
                default:
                    return value;
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

        public void Unload()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= BenchMaker.TryToDeploy;
            ModHooks.SetPlayerBoolHook -= BenchWatcher;
            ModHooks.GetPlayerStringHook -= RespawnAtDeployedBench;
            ModHooks.SetPlayerStringHook -= RemoveRespawnFromDeployedBench;
            On.GameManager.OnNextLevelReady -= FixRespawnType;

            BenchMaker.DestroyBench(DontDeleteData: true);
            GUIController.Unload();
        }

        new public void SaveGlobalSettings()
        {
            try
            {
                base.SaveGlobalSettings();
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }

        public void OnLoadGlobal(GlobalSettings s)
        {
            GS = s;
        }

        public GlobalSettings OnSaveGlobal()
        {
            return GS;
        }

        public void OnLoadLocal(SaveSettings s)
        {
            LS = s;
        }

        public SaveSettings OnSaveLocal()
        {
            return LS;
        }

        public bool ToggleButtonInsideMenu => false;

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            return new List<IMenuMod.MenuEntry>
            {
                new IMenuMod.MenuEntry("Show Menu", new string[] { "True", "False" }, "Toggle the Benchwarp Menu UI", (i) => GS.ShowMenu = i == 0, () => GS.ShowMenu ? 0 : 1),
            };
        }
    }
}
