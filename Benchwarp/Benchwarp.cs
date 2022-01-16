using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Modding;
using UnityEngine;
using UnityEngine.UI;

namespace Benchwarp
{
    public class Benchwarp : Mod, ITogglableMod, IGlobalSettings<GlobalSettings>, ILocalSettings<SaveSettings>, IMenuMod
    {
        internal static Benchwarp instance;
        public Benchwarp()
        {
            instance = this;
        }

        public static GlobalSettings GS { get; private set; } = new();
        public static SaveSettings LS { get; private set; } = new();

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloaded)
        {
            ObjectCache.SavePreloads(preloaded);

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += BenchMaker.TryToDeploy;
            ModHooks.SetPlayerBoolHook += BenchWatcher;
            ModHooks.GetPlayerStringHook += RespawnAtDeployedBench;
            ModHooks.SetPlayerStringHook += RemoveRespawnFromDeployedBench;
            // Imagine if GetPlayerIntHook actually worked
            On.GameManager.OnNextLevelReady += FixRespawnType;
            On.PlayMakerFSM.OnEnable += StyleOverride;

            GUIController.Setup();
            GUIController.Instance.BuildMenus();

            if (LS.benchDeployed && GameManager.instance.sceneName == LS.benchScene)
            {
                BenchMaker.MakeBench(); // Since the mod could be reenabled in any scene
            }

            if (Hotkeys.CurrentHotkeys.Count == 0)
            {
                Hotkeys.ApplyHotkeyOverrides();
            }
        }

        private void StyleOverride(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM fsm)
        {
            orig(fsm);
            if (GS.ModifyVanillaBenchStyles && fsm.FsmName == "Bench Control")
            {
                if (fsm.gameObject == BenchMaker.DeployedBench) return;
                switch (fsm.gameObject.scene.name)
                {
                    // benches that are too much trouble to implement

                    case "Ruins1_02": // mostly works, but the bench sprite is part of Quirrel
                    case "Deepnest_East_13": // camp
                    case "Fungus1_24": // qg cornifer
                    case "Mines_18": // cg2

                    // Tolls work, but only after a scene change
                    case "Fungus3_50":
                    case "Ruins1_31":
                    case "Abyss_18":
                        return;
                }


                GameObject benchGO = fsm.gameObject;
                Bench bench = Bench.Benches.FirstOrDefault(b => b.sceneName == benchGO.scene.name);
                if (bench == null) return;

                if (!BenchStyle.IsValidStyle(bench.style) || !BenchStyle.IsValidStyle(GS.nearStyle) || !BenchStyle.IsValidStyle(GS.farStyle)) return;

                BenchStyle origStyle = BenchStyle.GetStyle(bench.style);
                BenchStyle nearStyle = BenchStyle.GetStyle(GS.nearStyle);
                BenchStyle farStyle = BenchStyle.GetStyle(GS.farStyle);

                Vector3 position = benchGO.transform.position - bench.specificOffset;
                nearStyle.ApplyFsmAndPositionChanges(benchGO, position);
                nearStyle.ApplyLitSprite(benchGO);
                farStyle.ApplyDefaultSprite(benchGO);
                UnityEngine.Object.Destroy(benchGO.GetComponent<RestBenchTilt>());
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
                    else if (GameManager.instance.sceneName == bench.sceneName && LS.benchScene != bench.sceneName) bench.SetVisited(true);
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
                        LS.atDeployedBench = false;
                    }
                    break;
                case nameof(PlayerData.respawnScene):
                    if (value != LS.benchScene)
                    {
                        LS.atDeployedBench = false;
                    }
                    break;
            }
            return value;
        }

        private string RespawnAtDeployedBench(string stringName, string value) => !LS.atDeployedBench
            ? value 
            : stringName switch
            {
                nameof(PlayerData.respawnMarkerName) => BenchMaker.DEPLOYED_BENCH_RESPAWN_MARKER_NAME,
                nameof(PlayerData.respawnScene) => LS.benchScene,
                _ => value,
            };

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
                RefreshMenu();
            }
            catch (Exception e)
            {
                LogError(e);
            }
        }

        public void OnLoadGlobal(GlobalSettings s)
        {
            GS = s ?? GS ?? new();
        }

        public GlobalSettings OnSaveGlobal()
        {
            return GS;
        }

        public void OnLoadLocal(SaveSettings s)
        {
            LS = s ?? new();
        }

        public SaveSettings OnSaveLocal()
        {
            return LS;
        }

        public bool ToggleButtonInsideMenu => true;

        private readonly string[] bools = new string[] { "True", "False" };

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            IMenuMod.MenuEntry e = toggleButtonEntry.Value;
            IMenuMod.MenuEntry entry = new(e.Name, e.Values, "Toggle all effects of the Benchwarp mod.", e.Saver, e.Loader);

            List<IMenuMod.MenuEntry> menuEntries = new() { entry };

            foreach (FieldInfo fi in typeof(GlobalSettings).GetFields())
            {
                if (fi.GetCustomAttribute<MenuIntAttribute>() is MenuIntAttribute mi)
                {
                    menuEntries.Add(new()
                    {
                        Name = mi.name,
                        Description = mi.description,
                        Values = mi.values,
                        Saver = opt => fi.SetValue(GS, int.Parse(mi.values[opt])),
                        Loader = () =>
                        {
                            string needle = ((int) fi.GetValue(GS)).ToString();
                            return Array.FindIndex(mi.values, s => s == needle);
                        }
                    });
                }
                if (fi.GetCustomAttribute<MenuToggleableAttribute>() is MenuToggleableAttribute mt)
                {
                    menuEntries.Add(new()
                    {
                        Name = mt.name,
                        Description = mt.description,
                        Values = bools,
                        Saver = opt => { fi.SetValue(GS, opt == 0); TopMenu.RebuildMenu(); },
                        Loader = () => (bool)fi.GetValue(GS) ? 0 : 1
                    });
                }
            }

            return menuEntries;
        }

        public void RefreshMenu()
        {
            MenuScreen screen = ModHooks.BuiltModMenuScreens[this];
            if (screen != null)
            {
                foreach (MenuOptionHorizontal option in screen.GetComponentsInChildren<MenuOptionHorizontal>())
                {
                    option.menuSetting.RefreshValueFromGameSettings();
                }
            }
        }
    }
}
