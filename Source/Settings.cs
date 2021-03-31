using Modding;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Benchwarp
{
    [Serializable]
    public class SaveSettings : IModSettings
    {
        // public SerializableBoolDictionary visitedBenchScenes = new SerializableBoolDictionary();
        //public Dictionary<string, bool> visitedBenchScenes = Bench.Benches.ToDictionary(bench => bench.sceneName, bench => false);
    }

    [Serializable]
    public class GlobalSettings : IModSettings
    {
        public bool WarpOnly = false;
        //public bool UnlockAllBenches = false;
        public bool ShowScene = false;
        public bool SwapNames = false;
        public bool AlwaysToggleAll = false;
        public bool DoorWarp = false;
    }
}
