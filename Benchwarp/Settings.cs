using System.Linq;
using System.Collections.Generic;
using System;

namespace Benchwarp
{
    [Serializable]
    public class SaveSettings
    {
        public bool benchDeployed;
        public bool atDeployedBench;
        public float benchX;
        public float benchY;
        public string benchScene;
        public Dictionary<string, bool> visitedBenchScenes = Bench.Benches.ToDictionary(bench => bench.sceneName, bench => false);
    }

    [Serializable]
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
        public Dictionary<string, string> HotkeyOverrides = new();
    }

    public class MenuToggleableAttribute : Attribute
    {
        public string name;
        public string description;

        public MenuToggleableAttribute(string name, string description = "")
        {
            this.name = name;
            this.description = description;
        }
    }

    public class MenuIntAttribute : Attribute
    {
        public string name;
        public string description;
        public string[] values;

        public MenuIntAttribute(string name, int min, int max, int step, string description = "")
        {
            this.name = name;
            this.description = description;
            this.values = Enumerable.Range(0, (max - min + 1) / step).Select(x => (x * step + min).ToString()).ToArray();
        }
    }
}
