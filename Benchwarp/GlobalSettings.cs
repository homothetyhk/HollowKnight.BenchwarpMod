namespace Benchwarp
{
    public class GlobalSettings
    {
        [MenuToggleable(name: "Show Menu", description: "Toggle only the Benchwarp Menu UI")]
        public bool ShowMenu = true;
        [MenuToggleable(name: "Warp Only")]
        public bool WarpOnly = false;
        [MenuToggleable(name: "Unlock All")]
        public bool UnlockAllBenches = false;
        public bool ShowScene = false;
        [MenuInt("Max Displayed Room Names", 1, 20, 1)]
        public int MaxSceneNames = 1;
        public bool SwapNames = false;
        [MenuToggleable(name: "Enable Deploy")]
        public bool EnableDeploy = true;
        [MenuToggleable(name: "Always Toggle All")]
        public bool AlwaysToggleAll = false;
        public bool ModifyVanillaBenchStyles = false;
        public string nearStyle = "Right";
        public string farStyle = "Right";
        public bool DeployCooldown = true;
        public bool Noninteractive = true;
        public bool NoMidAirDeploy = true;
        public bool NoDarkOrDreamRooms = true;
        public bool NoPreload = false;
        [MenuToggleable(name: "Door Warp")]
        public bool DoorWarp = false;
        [MenuToggleable(name: "Enable Hotkeys")]
        public bool EnableHotkeys = false;
        [MenuToggleable(name: "Legacy Hotkeys", description: "Toggle Benchwarp or restart to apply changes")]
        public bool LegacyHotkeys = false;
        [MenuToggleable(name: "Override Localization", description: "Keeps Benchwarp in English regardless of game language")]
        public bool OverrideLocalization = false;

        public Dictionary<string, string> HotkeyOverrides = new();
    }
}
