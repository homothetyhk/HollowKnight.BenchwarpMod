using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Benchwarp
{
    public static class ObjectCache
    {
        public static bool DidPreload { get; private set; }
        private static GameObject _preloadedBench;
        public static GameObject GetNewBench() => GameObject.Instantiate(_preloadedBench);


        public static List<(string, string)> GetPreloadNames()
        {
            DidPreload = !Benchwarp.GS.NoPreload;

            if (Benchwarp.GS.NoPreload)
            {
                return new List<(string, string)>();
            }

           
            return new List<(string, string)>
            {
                ("Crossroads_47", "RestBench"),
                /*
                ("Crossroads_30", "RestBench"),
                ("Town", "RestBench"),
                ("Crossroads_04", "RestBench"),
                ("Crossroads_ShamanTemple", "BoneBench"),
                ("Fungus1_37", "RestBench"),
                ("Room_Slug_Shrine", "RestBench"),
                ("Fungus3_archive", "RestBench"),
                ("Fungus2_26", "RestBench"),
                ("Fungus2_31", "RestBench"),
                ("Ruins_Bathhouse", "RestBench"),
                ("Waterways_02", "RestBench"),
                ("GG_Atrium", "RestBench"),
                ("Deepnest_Spider_Town", "RestBench Return"),
                ("Deepnest_East_13", "RestBench"),
                //("Deepnest_East_13", "outskirts__0003_camp"),
                ("Room_Colosseum_02", "RestBench"),
                ("Fungus1_24", "RestBench"),
                //("Fungus1_24", "guardian_bench"),
                ("Room_Tram", "RestBench"),
                ("Room_nailmaster", "RestBench"),
                ("Deepnest_East_06", "RestBench"),
                ("Fungus1_15", "RestBench"),
                ("White_Palace_01", "WhiteBench"),
                ("Room_Final_Boss_Atrium", "RestBench"),
                ("GG_Atrium_Roof", "RestBench (1)")
                */
            };
        }

        public static void SavePreloads(Dictionary<string, Dictionary<string, GameObject>> objects)
        {
            if (objects == null || !DidPreload) return; //happens if mod is reloaded
            _preloadedBench = objects.Values.First().Values.First();
            UnityEngine.Object.DontDestroyOnLoad(_preloadedBench);

            /*
            foreach (var kdp in objects)
            {
                string sceneName = kdp.Key;
                foreach (var kvp in kdp.Value)
                {
                    string objectName = kvp.Key;
                    GameObject obj = kvp.Value;
                }
            }

            JsonUtil.Serialize(BenchStyle._styles, "styles.json");
            */
        }
    }
}
