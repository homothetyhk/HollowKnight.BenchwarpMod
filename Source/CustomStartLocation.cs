using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modding;
using GlobalEnums;

namespace Benchwarp
{
    public static class CustomStartLocation
    {
        public static bool RandomizerActive = false;
        public static bool ItemChangerActive = false;
        public static bool Inactive = false;

        

        private static object settings;
        private static Func<string, string> GetString;
        private static Func<string, int> GetInt;

        private static FieldInfo ItemChangerStart;


        public static string StartName => GetString("StartName");
        public static string respawnScene => GetString("StartSceneName");
        public static string respawnMarkerName => GetString("StartRespawnMarkerName");
        public static int respawnType => GetInt("StartRespawnType");
        public static MapZone mapZone => (MapZone)(GetInt("StartMapZone"));


        public static void SetStart()
        {
            if (Inactive) return;
            else if (RandomizerActive)
            {
                UpdateRandomizerSettings();
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
                    Inactive = true;
                    RandomizerActive = false;
                }
            }
            else if (ItemChangerActive)
            {
                try
                {
                    object start = ItemChangerStart.GetValue(null);
                    string respawnScene = (string)start.GetType().GetField("startSceneName").GetValue(start);
                    if (string.IsNullOrEmpty(respawnScene))
                    {
                        return;
                    }

                    PlayerData.instance.respawnScene = respawnScene;
                    PlayerData.instance.respawnMarkerName = "ITEMCHANGER_RESPAWN_MARKER";
                    PlayerData.instance.respawnType = 0;
                }

                catch
                {
                    Benchwarp.instance.Log("Error in setting ItemChanger start location. Defaulting to Dirtmouth...");
                    Bench.Benches.First(bench => bench.sceneName == "Town").SetBench();
                    Inactive = true;
                    ItemChangerActive = false;
                }

            }
        }

        public static bool CheckIfAtStart()
        {
            if (Inactive) return false;
            else if (RandomizerActive)
            {
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
                    Inactive = true;
                    RandomizerActive = false;
                    return false;
                }
            }
            else if (ItemChangerActive)
            {
                try
                {
                    object start = ItemChangerStart.GetValue(null);
                    string respawnScene = (string)start.GetType().GetField("startSceneName").GetValue(start);

                    if (PlayerData.instance.respawnScene == respawnScene && PlayerData.instance.respawnMarkerName == "ITEMCHANGER_RESPAWN_MARKER")
                    {
                        return true;
                    }
                    else return false;
                }
                catch (Exception e)
                {
                    Benchwarp.instance.Log("Error in checking ItemChanger start location. Deactivating feature...\n" + e);
                    Inactive = true;
                    return false;
                }
            }
            else
            {
                Inactive = true;
                return false;
            }
        }

        public static void UpdateRandomizerSettings()
        {
            if (RandomizerActive)
            {
                try
                {
                    var inst = Type.GetType("RandomizerMod.RandomizerMod, RandomizerMod3.0").GetProperty("Instance").GetValue(null, null);
                    settings = inst.GetType()
                        .GetProperty("Settings").GetValue(inst, null);
                }
                catch (Exception e)
                {
                    Inactive = true;
                    Benchwarp.instance.LogError("Error loading RandomizerMod settings:\n" + e);
                    return;
                }
            }
        }

        public static void Setup()
        {
            List<string> mods = ModHooks.Instance.LoadedMods;
            RandomizerActive = mods.Contains("RandomizerMod");
            ItemChangerActive = mods.Contains("ItemChanger");
            Inactive = !(RandomizerActive || ItemChangerActive);

            if (RandomizerActive)
            {
                UpdateRandomizerSettings();
                MethodInfo[] methods = settings.GetType().GetMethods();
                MethodInfo s = methods.FirstOrDefault(method => method.Name == "GetString");
                GetString = (string stringName) => (string)s.Invoke(settings, new object[] { null, stringName });
                MethodInfo i = methods.FirstOrDefault(method => method.Name == "GetInt");
                GetInt = (string intName) => (int)i.Invoke(settings, new object[] { null, intName });
            }

            if (ItemChangerActive)
            {
                try
                {
                    ItemChangerStart = Type.GetType(Assembly.CreateQualifiedName("ItemChanger", "ItemChanger.StartLocation")).GetField("start", BindingFlags.NonPublic | BindingFlags.Static);
                }
                catch (Exception e)
                {
                    Inactive = true;
                    Benchwarp.instance.LogError("Error loading ItemChanger settings:\n" + e);
                    return;
                }
            }
        }

    }
}
