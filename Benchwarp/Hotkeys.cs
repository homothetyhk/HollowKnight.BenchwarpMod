using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Benchwarp.Bench;

namespace Benchwarp
{
    public static class Hotkeys
    {
        /// <summary>
        /// The current list of letter hotkey codes, accounting for hotkey overrides.
        /// </summary>
        public static ReadOnlyDictionary<string, int> CurrentHotkeys { get; } = new(_hotkeys = new());
        private static readonly Dictionary<string, int> _hotkeys;
        private static readonly Dictionary<int, Action> _customHotkeyActions = new();

        public static void RefreshHotkeys()
        {
            _hotkeys.Clear();
            _customHotkeyActions.Clear();

            _hotkeys.AddHotkey("LB", LastBenchID);
            _hotkeys.AddHotkey("SB", StartBenchID);
            _hotkeys.AddHotkey("WD", WarpDeployID);
            _hotkeys.AddHotkey("TM", ToggleMenuID);
            _hotkeys.AddHotkey("DW", DoorWarpID);
            _hotkeys.AddHotkey("DB", DeployBenchID);
            
            foreach ((string code, Action a) in Events.GetHotkeyRequests())
            {
                if (code != null)
                {
                    int id = CustomActionID - _customHotkeyActions.Count;
                    _hotkeys.AddHotkey(code, id);
                    _customHotkeyActions.Add(id, a);
                }
            }
        }

        internal static void AddHotkey(this Dictionary<string, int> dict, string code, int id)
        {
            if (Benchwarp.GS.HotkeyOverrides.TryGetValue(code, out string altCode))
            {
                if (altCode.Length != 2 || !char.IsLetter(altCode[0]) || !char.IsLetter(altCode[1]))
                {
                    Benchwarp.instance.LogError($"Invalid hotkey override {altCode} for {code}: hotkeys must consist of exactly two letters.");
                }
                else
                {
                    code = altCode;
                }
            }
            try
            {
                dict.Add(code, id);
            }
            catch (ArgumentException)
            {
                Benchwarp.instance.LogError($"Duplicate binding for hotkey '{code}'");
            }
        }

        private static List<HotkeyWarper> _legacyHotkeys;
        internal static void ApplyLegacyHotkeys()
        {
            string[] codes = new[]
            {
                "DM", "NM",
                "FS", "FC", "SA", "SM", "BE",
                "GW", "SS", "GT", "GP", "LU", "NS",
                "TA",
                "QS", "LE", "BR", "MV",
                "CQ", "CT", "CS", "WS", "KS", "PH",
                "WW", "GA", "GR", "HG",
                "DS", "FT", "BD",
                "BT", "HS",
                "NO", "EC", "CF", "BB",
                "PD", "CG",
                "RG", "GM",
                "QC", "QT", "QG",
                "PE", "PA", "PB",
                "UT", "LT",
            };

            _legacyHotkeys = Enumerable.Range(0, codes.Length)
                .Select(i => new HotkeyWarper { Code = codes[i], Target = baseBenches[i], }).ToList();
            Events.AddHotkeyRequests(_legacyHotkeys.Select(w => (Func<(string, Action)>)w.GetHotkey));
        }
        internal static void RemoveLegacyHotkeys()
        {
            if (_legacyHotkeys != null)
            {
                Events.RemoveHotkeyRequests(_legacyHotkeys.Select(w => (Func<(string, Action)>)w.GetHotkey));
                _legacyHotkeys = null;
            }
        }

        public static bool TryGetActionID(string code, out int actionID)
        {
            if (code is null || code.Length != 2)
            {
                actionID = default;
                return false;
            }
            if (CurrentHotkeys.TryGetValue(code, out actionID))
            {
                return true;
            }
            if (int.TryParse(code, out actionID) && 0 <= actionID && actionID < Benches.Count)
            {
                return true;
            }
            return false;
        }

        public static void DoHotkeyAction(int actionID)
        {
            if (0 <= actionID && actionID < Benches.Count)
            {
                Benches[actionID].SetBench();
                ChangeScene.WarpToRespawn();
                return;
            }
            else if (_customHotkeyActions.TryGetValue(actionID, out Action a))
            {
                try
                {
                    a?.Invoke();
                }
                catch (Exception e)
                {
                    Benchwarp.instance.LogError($"Error invoking custom hotkey action:\n{e}");
                }
                return;
            }
            else
            {
                switch (actionID)
                {
                    case LastBenchID:
                        ChangeScene.WarpToRespawn();
                        return;
                    case StartBenchID:
                        Events.SetToStart();
                        ChangeScene.WarpToRespawn();
                        return;
                    case WarpDeployID:
                        TopMenu.SetClicked(null);
                        ChangeScene.WarpToRespawn();
                        return;
                    case ToggleMenuID:
                        Benchwarp.GS.ShowMenu = !Benchwarp.GS.ShowMenu;
                        return;
                    case DoorWarpID:
                        TopMenu.DoorWarpClicked(null);
                        return;
                    case DeployBenchID:
                        TopMenu.DeployClicked(null);
                        return;
                    default:
                        Benchwarp.instance.LogError($"Unknown internal hotkey code: {actionID}");
                        return;
                }
            }
        }

        public const int LastBenchID = -1;
        public const int StartBenchID = -2;
        public const int WarpDeployID = -3;
        public const int ToggleMenuID = -4;
        public const int DoorWarpID = -5;
        public const int DeployBenchID = -6;
        /// <summary>
        /// The first id reserved for custom hotkey actions.
        /// </summary>
        public static int CustomActionID { get => DeployBenchID - 1; }

        public class HotkeyWarper
        {
            public string Code { get; init; }
            public Bench Target { get; init; }
            public (string, Action) GetHotkey() => (Code, Warp);
            public void Warp()
            {
                Target.SetBench();
                ChangeScene.WarpToRespawn();
            }
        }

    }
}
