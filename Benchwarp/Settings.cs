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
        public bool WarpOnly = false;
        public bool UnlockAllBenches = false;
        public bool ShowScene = false;
        public bool SwapNames = false;
        public bool EnableDeploy = true;
        public bool AlwaysToggleAll = false;
        public bool ModifyVanillaBenchStyles = false;
        public string nearStyle = "Right";
        public string farStyle = "Right";
        public bool DeployCooldown = true;
        public bool Noninteractive = true;
        public bool NoMidAirDeploy = true;
        public bool NoDarkOrDreamRooms = true;
        public bool NoPreload = false;
        public bool DoorWarp = false;
        public bool EnableHotkeys = false;
        public bool ShowMenu = true;
        public Dictionary<string, string> HotkeyOverrides = new Dictionary<string, string>();
    }
}
