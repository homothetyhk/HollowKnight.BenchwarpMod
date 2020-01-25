using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;
using GlobalEnums;

namespace Benchwarp
{
    public static class RandomizerStartLocation
    {
        public static bool RandomizerActive = false;
        public static bool RandomizerError = false;

        private static object settings;
        private static Func<string, string> GetString;
        private static Func<string, int> GetInt;
        

        public static string StartName => GetString("StartName");
        public static string respawnScene => GetString("StartSceneName");
        public static string respawnMarkerName => GetString("StartRespawnMarkerName");
        public static int respawnType => GetInt("StartRespawnType");
        public static MapZone mapZone => (MapZone)(GetInt("StartMapZone"));


        public static void SetStart()
        {
            if (!RandomizerActive || RandomizerError) return;
            UpdateSettings();
            try
            {
                if (string.IsNullOrEmpty(respawnScene) || string.IsNullOrEmpty(respawnMarkerName))
                {
                    RandomizerActive = false;
                    return;
                }
                PlayerData.instance.respawnScene = respawnScene;
                PlayerData.instance.respawnMarkerName = respawnMarkerName;
                PlayerData.instance.respawnType = respawnType;
                PlayerData.instance.mapZone = mapZone;
            }
            catch
            {
                Benchwarp.instance.Log("Error in setting randomizer start location. Defaulting to Dirtmouth...");
                Bench.Benches.First(bench => bench.sceneName == "Town").SetBench();
                RandomizerError = true;
                RandomizerActive = false;
            }
        }

        public static bool CheckIfAtStart()
        {
            if (!RandomizerActive || RandomizerError) return false;
            try
            {
                if (PlayerData.instance.respawnScene == respawnScene &&
                PlayerData.instance.respawnMarkerName == respawnMarkerName &&
                PlayerData.instance.respawnType == respawnType
                )
                {
                    return true;
                }
                else return false;
            }
            catch
            {
                Benchwarp.instance.Log("Error in checking randomizer start location. Deactivating feature...");
                RandomizerError = true;
                RandomizerActive = false;
                return false;
            }
        }

        public static void CheckForRandomizer()
        {
            if (RandomizerError) return;
            if (Type.GetType("RandomizerMod.RandomizerMod, RandomizerMod3.0") is Type Randomizer)
            {
                UpdateSettings();
                MethodInfo[] methods = settings.GetType().GetMethods();
                MethodInfo s = methods.FirstOrDefault(method => method.Name == "GetString");
                GetString = (string stringName) => (string)s.Invoke(settings, new object[] { null, stringName });
                MethodInfo i = methods.FirstOrDefault(method => method.Name == "GetInt");
                GetInt = (string intName) => (int)i.Invoke(settings, new object[] { null, intName });
            }
        }

        public static void UpdateSettings()
        {
            if (RandomizerError) return;
            if (Type.GetType("RandomizerMod.RandomizerMod, RandomizerMod3.0") is Type Randomizer)
            {
                RandomizerActive = true;
                var inst = Randomizer.GetProperty("Instance").GetValue(null, null);
                settings = inst.GetType().GetProperty("Settings").GetValue(inst, null);
            }
            else RandomizerActive = false;
        }
    }
}
