using GlobalEnums;
using System;
using System.Linq;
using UnityEngine;

namespace Benchwarp
{
    public class Bench
    {
        static Bench()
        {
            Benches = JsonUtil.Deserialize<Bench[]>("Benchwarp.Resources.benches.json");
        }


        public static readonly Bench[] Benches;

        public readonly string name;
        public readonly string areaName;
        public readonly string sceneName;
        public readonly string respawnMarker;
        public readonly int respawnType;
        public readonly MapZone mapZone;
        public readonly string style;
        public readonly Vector3 specificOffset;

        public bool HasVisited()
        {
            return Benchwarp.LS.visitedBenchScenes.ContainsKey(sceneName)
                && Benchwarp.LS.visitedBenchScenes[sceneName];
        }

        public void SetVisited(bool value)
        {
            Benchwarp.LS.visitedBenchScenes[sceneName] = value;
        }

        public bool AtBench() => PlayerData.instance.respawnScene == sceneName &&
            PlayerData.instance.respawnMarkerName == respawnMarker &&
            PlayerData.instance.respawnType == respawnType &&
            !Benchwarp.LS.atDeployedBench;

        [Newtonsoft.Json.JsonConstructor]
        public Bench(string name, string areaName, string sceneName, string respawnMarker, int respawnType, MapZone mapZone, string style, Vector3 specificOffset)
        {
            this.name = name; // may not be unique
            this.areaName = areaName; // may be abbreviated, see below
            this.sceneName = sceneName;
            this.respawnMarker = respawnMarker;
            this.respawnType = respawnType;
            this.mapZone = mapZone;
            this.style = style;
            this.specificOffset = specificOffset;
        }

        public void SetBench()
        {
            if (!Benchwarp.GS.UnlockAllBenches && !HasVisited()) return;
            Benchwarp.LS.atDeployedBench = false;
            PlayerData.instance.respawnScene = sceneName;
            PlayerData.instance.respawnMarkerName = respawnMarker;
            PlayerData.instance.respawnType = respawnType;
            PlayerData.instance.mapZone = mapZone;
        }
        public static Bench GetStyleBench(string style)
        {
            return Benches.First(bench => bench.style == style);
        }
    }
}
