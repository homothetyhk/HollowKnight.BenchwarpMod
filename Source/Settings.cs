using Modding;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Benchwarp
{
    [Serializable]
    public class SaveSettings : ModSettings
    {
        public bool benchDeployed;
        public bool atDeployedBench;
        public float benchX;
        public float benchY;
        public string benchScene;
        public Dictionary<string, bool> visitedBenchScenes = Bench.Benches.ToDictionary(bench => bench.sceneName, bench => false);
    }

    [Serializable]
    public class GlobalSettings : ModSettings
    {
        public bool WarpOnly = false;
        public bool UnlockAllBenches = false;
        public bool ShowScene = false;
        public bool SwapNames = false;
        public bool EnableDeploy = true;
        public bool AlwaysToggleAll = false;
        public string benchStyle = "Right";
        public bool DeployCooldown = true;
        public bool Noninteractive = true;
        public bool NoMidAirDeploy = true;
        public bool NoDarkOrDreamRooms = true;
        public bool NoPreload = false;
        public bool ReducePreload = true;
    }
}
