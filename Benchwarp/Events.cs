using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GlobalEnums;

namespace Benchwarp
{
    public static class Events
    {
        /// <summary>
        /// Event invoked when a request to benchwarp is made, before beginning to warp.
        /// </summary>
        public static event Action OnBenchwarp;
        internal static void InvokeOnBenchwarp()
        {
            try { OnBenchwarp?.Invoke(); }
            catch (Exception e) { Benchwarp.instance.LogError(e); }
        }
        /// <summary>
        /// Event with (scene, gate) parameters, invoked when a request to doorwarp is made, before beginning to warp.
        /// </summary>
        public static event Action<string, string> OnDoorwarp;
        internal static void InvokeOnDoorwarp(string sceneName, string gateName)
        {
            try { OnDoorwarp?.Invoke(sceneName, gateName); }
            catch (Exception e) { Benchwarp.instance.LogError(e); }
        }
        /// <summary>
        /// Event invoked when a bench is unlocked, after its key is added to local settings.
        /// </summary>
        public static event Action<BenchKey> OnBenchUnlock;
        internal static void InvokeOnBenchUnlock(BenchKey key)
        {
            try { OnBenchUnlock?.Invoke(key); }
            catch (Exception e) { Benchwarp.instance.LogError(e); }
        }


        public static readonly SequentialEventHandler<string> OnGetSceneName = new();
        public static string GetSceneName(string sceneName) => OnGetSceneName.Invoke(sceneName);


        public static readonly SequentialEventHandler<string> OnGetBenchName = new();
        public static string GetBenchName(string benchName) => OnGetBenchName.Invoke(benchName);


        public static readonly SequentialEventHandler<(string respawnScene, string respawnMarkerName, int respawnType, int mapZone)>
            OnGetStartDef = new();
        public static (string respawnScene, string respawnMarkerName, int respawnType, int mapZone) GetStartDef() =>
            OnGetStartDef.Invoke(("Tutorial_01", "Death Respawn Marker", 0, 2));
        public static bool AtStart()
        {
            (string respawnScene, string respawnMarkerName, int _, int _) = GetStartDef();
            return !Benchwarp.LS.atDeployedBench
                && respawnScene == PlayerData.instance.respawnScene 
                && respawnMarkerName == PlayerData.instance.respawnMarkerName;
        }
        public static void SetToStart()
        {
            (string respawnScene, string respawnMarkerName, int respawnType, int mapZone) = GetStartDef();
            Benchwarp.LS.atDeployedBench = false;
            PlayerData.instance.respawnScene = respawnScene;
            PlayerData.instance.respawnMarkerName = respawnMarkerName;
            PlayerData.instance.respawnType = respawnType;
            PlayerData.instance.mapZone = (MapZone)mapZone;
        }

        /*
         // Example code for accessing above events without referencing Benchwarp.dll
        public static void SafeAdd(Func<string, string> f)
        {
            try
            {
                FieldInfo field = Type.GetType("Benchwarp.Events, Benchwarp")
                    .GetField("OnGetSceneName", BindingFlags.Public | BindingFlags.Static);

                field.FieldType
                    .GetEvent("Event", BindingFlags.Public | BindingFlags.Instance)
                    .AddEventHandler(field.GetValue(null), f);
            }
            catch
            {
                return;
            }
        }
        */

        /// <summary>
        /// Event invoked for each bench in the menu. Return true to prevent the bench from appearing, and false otherwise.
        /// <br/>The bench list is only updated when subscribers are added or removed, or when Bench.RefreshBenchList is called manually.
        /// </summary>
        public static event Func<Bench, bool> BenchSuppressors
        {
            add
            {
                _benchSuppressors.Add(value);
                Bench.RefreshBenchList();
                TopMenu.RebuildMenu();
            }
            remove
            {
                _benchSuppressors.Remove(value);
                Bench.RefreshBenchList();
                TopMenu.RebuildMenu();
            }
        }
        private static readonly List<Func<Bench, bool>> _benchSuppressors = new();
        internal static bool ShouldSuppressBench(Bench bench)
        {
            bool value = false;
            foreach (var f in _benchSuppressors) value |= f(bench);
            return value;
        }


        /// <summary>
        /// Event which allows adding new benches to the menu. Benches will be organized into the existing area categories.
        /// <br/>The bench list is only updated when subscribers are added or removed, or when Bench.RefreshBenchList is called manually.
        /// </summary>
        public static event Func<IEnumerable<Bench>> BenchInjectors
        {
            add
            {
                _benchInjectors.Add(value);
                Bench.RefreshBenchList();
                TopMenu.RebuildMenu();
            }
            remove
            {
                _benchInjectors.Remove(value);
                Bench.RefreshBenchList();
                TopMenu.RebuildMenu();
            }
        }
        private static readonly List<Func<IEnumerable<Bench>>> _benchInjectors = new();
        internal static IEnumerable<Bench> GetInjectedBenches()
        {
            return _benchInjectors.SelectMany(f => f());
        }
    }

    public class SequentialEventHandler<T>
    {
        private readonly List<Func<T, T>> modifiers = new();

        public event Func<T, T> Event
        {
            add => modifiers.Add(value);
            remove => modifiers.Remove(value);
        }

        public T Invoke(T defaultValue)
        {
            foreach (Func<T, T> f in modifiers)
            {
                defaultValue = f(defaultValue);
            }
            return defaultValue;
        }
    }
}
