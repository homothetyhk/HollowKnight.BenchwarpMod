using GlobalEnums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Benchwarp
{
    public class Bench
    {
        static Bench()
        {
            baseBenches = JsonUtil.Deserialize<Bench[]>("Benchwarp.Resources.benches.json");
            RefreshBenchList();
        }

        public static readonly Bench[] baseBenches;
        public static ReadOnlyCollection<Bench> Benches { get; private set; }

        public static void RefreshBenchList()
        {
            List<Bench> benches = new();
            foreach (Bench bench in baseBenches)
            {
                if (Events.ShouldSuppressBench(bench)) continue;
                benches.Add(bench);
            }
            foreach (Bench bench in Events.GetInjectedBenches())
            {
                if (Events.ShouldSuppressBench(bench)) continue;
                benches.Add(bench);
            }
            Benches = benches.AsReadOnly();
        }


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
            return Benchwarp.LS.visitedBenchScenes.Contains(new(sceneName, respawnMarker));
        }

        public void SetVisited(bool value)
        {
            if (value) Benchwarp.LS.visitedBenchScenes.Add(new(sceneName, respawnMarker));
            else Benchwarp.LS.visitedBenchScenes.Remove(new(sceneName, respawnMarker));
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
