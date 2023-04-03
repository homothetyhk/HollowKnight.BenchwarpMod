using Modding;
using System.Reflection;
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

            Hooks.Hook();

            GUIController.Setup();
            GUIController.Instance.BuildMenus();

            if (LS.benchDeployed && GameManager.instance.sceneName == LS.benchScene)
            {
                BenchMaker.MakeDeployedBench(); // Since the mod could be reenabled in any scene
            }

            if (GS.LegacyHotkeys)
            {
                Hotkeys.ApplyLegacyHotkeys();
            }
            Hotkeys.RefreshHotkeys();
        }

        public void Unload()
        {
            Hooks.Unhook();
            GUIController.Unload();
            BenchMaker.DestroyBench(DontDeleteData: true);
            Hotkeys.RemoveLegacyHotkeys();
        }

        public override string GetVersion()
        {
            return typeof(Benchwarp).Assembly.GetName().Version.ToString();
        }

        public override List<(string, string)> GetPreloadNames()
        {
            return ObjectCache.GetPreloadNames();
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

        private readonly string[] bools = new string[] { Language.Language.Get("MOH_ON", "MainMenu"), Language.Language.Get("MOH_OFF", "MainMenu") };

        public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? toggleButtonEntry)
        {
            IMenuMod.MenuEntry e = toggleButtonEntry.Value;
            IMenuMod.MenuEntry entry = new(e.Name, e.Values, Localize("Toggle all effects of the Benchwarp mod."), e.Saver, e.Loader);

            List<IMenuMod.MenuEntry> menuEntries = new() { entry };

            foreach (FieldInfo fi in typeof(GlobalSettings).GetFields())
            {
                if (fi.GetCustomAttribute<MenuIntAttribute>() is MenuIntAttribute mi)
                {
                    menuEntries.Add(new()
                    {
                        Name = Localize(mi.name),
                        Description = Localize(mi.description),
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
                        Name = Localize(mt.name),
                        Description = Localize(mt.description),
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
