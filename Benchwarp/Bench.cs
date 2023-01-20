using GlobalEnums;
using Modding.Converters;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Benchwarp
{
    public class Bench
    {
        static Bench()
        {
            baseBenches = JsonUtil.Deserialize<Bench[]>("Benchwarp.Resources.benches.json");

            // force benches to show in original area order unless modified by subsequent BenchComparisons.
            Dictionary<string, int> areaOrder = baseBenches
                .Select(b => b.areaName)
                .Distinct()
                .Select((a, i) => (a, i))
                .ToDictionary(p => p.a, p => p.i);
            Events.BenchComparisons += (b1, b2) =>
            {
                if (b1.areaName is null || !areaOrder.TryGetValue(b1.areaName, out int a1)) return 1;
                if (b2.areaName is null || !areaOrder.TryGetValue(b2.areaName, out int a2)) return -1;
                return a1.CompareTo(a2);
            };

            // RefreshBenchList(); // not needed, since Events.BenchComparisons.add refreshes bench list.
        }

        /// <summary>
        /// The list of benches embedded in Benchwarp.
        /// </summary>
        public static readonly Bench[] baseBenches;
        /// <summary>
        /// The current list of benches. Can be modified by injecting or suppressing through Events, and refreshed by RefreshBenchList.
        /// </summary>
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

            Benches = Events.GetSortedBenchList(benches).AsReadOnly();
        }

        public readonly string name;
        public readonly string areaName;
        public readonly string sceneName;
        public readonly string respawnMarker;
        public readonly int respawnType;
        public readonly MapZone mapZone;
        public readonly string style;
        [JsonConverter(typeof(Vector3Converter))] public readonly Vector3 specificOffset;

        public bool HasVisited()
        {
            return Benchwarp.LS.visitedBenchScenes.Contains(ToBenchKey());
        }

        public void SetVisited(bool value)
        {
            if (value)
            {
                if (Benchwarp.LS.visitedBenchScenes.Add(ToBenchKey()))
                {
                    Events.InvokeOnBenchUnlock(ToBenchKey());
                }
            }
            else Benchwarp.LS.visitedBenchScenes.Remove(ToBenchKey());
        }

        /// <summary>
        /// Checks whether the bench is locked. Locked benches are not unlocked by benching, but can still be unlocked by SetVisited.
        /// </summary>
        public bool IsLocked()
        {
            return Benchwarp.LS.lockedBenches.Contains(ToBenchKey());
        }

        /// <summary>
        /// Locks or unlocks the bench. Locked benches are not unlocked by benching, but can still be unlocked by SetVisited.
        /// </summary>
        public void SetLocked(bool value)
        {
            if (value) Benchwarp.LS.lockedBenches.Add(ToBenchKey());
            else Benchwarp.LS.lockedBenches.Remove(ToBenchKey());
        }

        public bool AtBench() => PlayerData.instance.respawnScene == sceneName &&
            PlayerData.instance.respawnMarkerName == respawnMarker &&
            PlayerData.instance.respawnType == respawnType &&
            !Benchwarp.LS.atDeployedBench;

        [JsonConstructor]
        public Bench(string name, string areaName, string sceneName, string respawnMarker, 
            int respawnType, MapZone mapZone, string style, Vector3 specificOffset)
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

        /// <summary>
        /// Sets this as the current respawn. No effect if WarpOnly is active or the bench is not unlocked, either by being visited or by GS.UnlockAllBenches.
        /// </summary>
        public void SetBench()
        {
            if (Benchwarp.GS.WarpOnly || !Benchwarp.GS.UnlockAllBenches && !HasVisited()) return;
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

        public BenchKey ToBenchKey()
        {
            return new(sceneName, respawnMarker);
        }
    }
}
