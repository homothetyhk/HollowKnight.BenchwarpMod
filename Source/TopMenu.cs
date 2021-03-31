using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Benchwarp
{
    public static class DoorWarpSelection
    {
        public static string area;
        public static string room;
        public static string door;

        public static void Clear()
        {
            area = room = door = null;
        }
    }


    public static class TopMenu
    {
        private static CanvasPanel rootPanel;
        private static CanvasPanel sceneNamePanel;
        public static GameObject canvas;
        private static List<string> benchPanels;
        private static int fontSize;

        private static readonly Type t = typeof(GlobalSettings);

        private static readonly Dictionary<string, (string, UnityAction<string>, FieldInfo)[]> Panels =
            new Dictionary<string, (string, UnityAction<string>, FieldInfo)[]>
            {
                ["Settings"] = new (string, UnityAction<string>, FieldInfo)[]
                {
                    ("Warp Only", WarpOnlyClicked, t.GetField(nameof(GlobalSettings.WarpOnly))),
                    //("Unlock All", UnlockAllClicked, t.GetField(nameof(GlobalSettings.UnlockAllBenches))),
                    ("Show Room Name", ShowSceneClicked, t.GetField(nameof(GlobalSettings.ShowScene))),
                    ("Use Room Names", SwapNamesClicked, t.GetField(nameof(GlobalSettings.SwapNames))),
                    ("Always Toggle All", AlwaysToggleAllClicked, t.GetField(nameof(GlobalSettings.AlwaysToggleAll))),
                    ("Door Warp", DoorWarpClicked, t.GetField(nameof(GlobalSettings.DoorWarp)))
                }
            };

        public static Vector2 GridPosition(int count, int rowSize, float hSep, float vSep, Vector2 topCenter)
        {
            float width = hSep * (rowSize - 1);

            Vector2 pos = topCenter;
            pos.x -= width / 2;

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < rowSize - 1; j++)
                {
                    pos.x += hSep;
                    i++;
                    if (i == count) return pos;
                }
                pos.x -= width;
                pos.y += vSep;
            }

            return pos;
        }

        public static void RebuildMenu()
        {
            rootPanel.Destroy();
            sceneNamePanel.Destroy();
            BuildMenu(canvas);
            rootPanel.SetActive(false, true); // collapse all subpanels
            rootPanel.SetActive(true, false);
        }

        public static void BuildMenu(GameObject _canvas)
        {
            canvas = _canvas;

            sceneNamePanel = new CanvasPanel
                (_canvas, GUIController.Instance.images["ButtonsMenuBG"], new Vector2(0f, 0f), new Vector2(1346f, 0f), new Rect(0f, 0f, 0f, 0f));
            sceneNamePanel.AddText("SceneName", "Tutorial_01", new Vector2(5f, 1060f), Vector2.zero, GUIController.Instance.TrajanNormal, 18);

            rootPanel = new CanvasPanel
                (_canvas, GUIController.Instance.images["ButtonsMenuBG"], new Vector2(342f, 15f), new Vector2(1346f, 0f), new Rect(0f, 0f, 0f, 0f));

            Rect buttonRect = new Rect(0, 0, GUIController.Instance.images["ButtonRect"].width, GUIController.Instance.images["ButtonRect"].height);

            fontSize = 12;

            void AddButton(CanvasPanel panel, string name, UnityAction<string> action, Vector2 pos, string displayName = null, Font f = null)
            {
                panel.AddButton
                (
                    name,
                    GUIController.Instance.images["ButtonRectEmpty"],
                    pos,
                    Vector2.zero,
                    action,
                    new Rect(0f, 0f, 80f, 40f),
                    f != null ? f : GUIController.Instance.TrajanNormal,
                    displayName ?? name,
                    fontSize
                );
            }

            CanvasPanel MakePanel(string name, Vector2 position)
            {
                CanvasPanel newPanel = rootPanel.AddPanel
                (
                    name,
                    GUIController.Instance.images["ButtonRectEmpty"],
                    position,
                    Vector2.zero,
                    new Rect(0f, 0f, GUIController.Instance.images["DropdownBG"].width, 270f)
                );
                rootPanel.AddButton
                (
                    name,
                    GUIController.Instance.images["ButtonRect"],
                    position + new Vector2(1f, -20f),
                    Vector2.zero,
                    s => rootPanel.TogglePanel(name),
                    buttonRect,
                    GUIController.Instance.TrajanBold,
                    name
                );

                return newPanel;
            }

            //Main buttons
            rootPanel.AddButton
            (
                "Warp",
                GUIController.Instance.images["ButtonRect"],
                new Vector2(-154f, 40f),
                Vector2.zero,
                WarpClicked,
                buttonRect,
                GUIController.Instance.TrajanBold,
                "Warp"
            );

            CanvasPanel settings = MakePanel("Settings", new Vector2(1445f, 20f));

            for (int i = 0; i < Panels["Settings"].Length; i++)
            {
                (string name, UnityAction<string> action, FieldInfo _) = Panels["Settings"][i];

                AddButton
                (
                    settings,
                    name,
                    action,
                    new Vector2(5f, 25 + i * 40)
                );
            }
            settings.SetActive(false, true);

            if (Benchwarp.instance.GlobalSettings.WarpOnly) return;

            DoorWarpSelection.Clear();
            if (Benchwarp.instance.GlobalSettings.DoorWarp)
            {
                CanvasPanel door3 = MakePanel("Doors", new Vector2(-5f, 20f));
                CanvasPanel door2 = MakePanel("Rooms", new Vector2(395f, 20f));
                CanvasPanel door1 = MakePanel("Areas", new Vector2(1045f, 20f));
                List<string> doorAreas = DoorWarp.Doors.Select(d => d.area).Distinct().ToList();

                for (int i = 0; i < doorAreas.Count; i++)
                {
                    string name = doorAreas[i];
                    UnityAction<string> action = (areaSelected) =>
                    {
                        DoorWarpSelection.Clear();
                        DoorWarpSelection.area = areaSelected;
                        door2.ClearButtons();
                        door3.ClearButtons();
                        door2.SetActive(true, false);
                        door3.SetActive(false, false);
                        List<string> rooms = DoorWarp.Doors
                        .Where(d => d.area == areaSelected)
                        .Select(d => d.room)
                        .Distinct().ToList();
                        for (int j = 0; j < rooms.Count; j++)
                        {
                            AddButton
                            (
                                door2,
                                rooms[j],
                                (roomSelected) =>
                                {
                                    DoorWarpSelection.door = null;
                                    DoorWarpSelection.room = roomSelected;
                                    door3.ClearButtons();
                                    door3.SetActive(true, false);
                                    List<string> doors = DoorWarp.Doors.Where(d => d.room == roomSelected).Select(d => d.door).ToList();
                                    for (int k = 0; k < doors.Count; k++)
                                    {
                                        AddButton
                                        (
                                            door3,
                                            doors[k],
                                            (doorSelected) => DoorWarpSelection.door = doorSelected,
                                            GridPosition(k, 2, 100f, 40f, new Vector2(5f, 25f))
                                        );
                                    }
                                    if (!door3.active) door3.ToggleActive();
                                },
                                GridPosition(j, 6, 100f, 40f, new Vector2(5f, 25f))
                            );
                        }
                    };

                    AddButton
                    (
                        door1,
                        name,
                        action,
                        GridPosition(i, 7, 100f, 40f, new Vector2(5f, 25f))
                    );
                }

                rootPanel.AddButton
                (
                    "Flip",
                    GUIController.Instance.images["ButtonRect"],
                    new Vector2(-154f, 0f),
                    Vector2.zero,
                    FlipClicked,
                    buttonRect,
                    GUIController.Instance.TrajanBold,
                    "Flip"
                );

                rootPanel.FixRenderOrder();
                return;
            }

            Vector2 panelDistance = new Vector2(-155f, 20f);

            Dictionary<string, Vector2> panelButtonHeight = new Dictionary<string, Vector2>();
            benchPanels = new List<string>();

            foreach (Bench bench in Bench.Benches)
            {
                if (!panelButtonHeight.ContainsKey(bench.areaName))
                {
                    benchPanels.Add(bench.areaName);
                    panelDistance += new Vector2(100f, 0f);
                    panelButtonHeight[bench.areaName] = new Vector2(5f, 25f);
                    MakePanel(bench.areaName, panelDistance);
                }
                else
                {
                    panelButtonHeight[bench.areaName] += new Vector2(0f, 40f);
                }

                rootPanel.GetPanel(bench.areaName)
                         .AddButton
                         (
                             bench.name,
                             GUIController.Instance.images["ButtonRectEmpty"],
                             panelButtonHeight[bench.areaName],
                             Vector2.zero,
                             (string s) => bench.SetBench(),
                             new Rect(0f, 0f, 80f, 40f),
                             GUIController.Instance.TrajanNormal,
                             !Benchwarp.instance.GlobalSettings.SwapNames ? bench.name : bench.sceneName,
                             fontSize
                         );
            }

            rootPanel.AddButton
            (
                "All",
                GUIController.Instance.images["ButtonRect"],
                new Vector2(-154f, 0f),
                Vector2.zero,
                AllClicked,
                buttonRect,
                GUIController.Instance.TrajanBold,
                "All"
            );

            rootPanel.FixRenderOrder();
        }

        public static void Update()
        {
            if (rootPanel == null || sceneNamePanel == null) return;
            if (GameManager.instance == null || !GameManager.instance.IsGameplayScene() || HeroController.instance == null)
            {
                rootPanel.SetActive(false, true);
                return;
            }

            Benchwarp bw = Benchwarp.instance;
            GlobalSettings gs = bw.GlobalSettings;

            if (gs.ShowScene)
            {
                sceneNamePanel.SetActive(true, false);
                sceneNamePanel.GetText("SceneName").UpdateText(GameManager.instance.sceneName);
            }
            else sceneNamePanel.SetActive(false, true);

            if (GameManager.instance.IsGamePaused())
            {
                if (!rootPanel.active)
                {
                    RebuildMenu();
                }
            }
            else
            {
                rootPanel.SetActive(false, true);
            }

            if (gs.AlwaysToggleAll && !gs.DoorWarp && !gs.WarpOnly)
            {
                foreach (string s in benchPanels)
                    if (!rootPanel.GetPanel(s).active)
                        rootPanel.TogglePanel(s);
            }

            CanvasPanel settings = rootPanel.GetPanel("Settings");

            if (settings.active)
            {
                foreach ((string name, FieldInfo fi) in Panels["Settings"].Select(x => (x.Item1, x.Item3)))
                {
                    settings.GetButton(name).SetTextColor((bool) fi.GetValue(gs) ? Color.yellow : Color.white);
                }
            }

            if (gs.DoorWarp)
            {
                if (rootPanel.GetPanel("Areas")?.active ?? false)
                {
                    foreach (string area in DoorWarp.Areas)
                    {
                        if (area == DoorWarpSelection.area)
                        {
                            rootPanel.GetButton(area, "Areas")?.SetTextColor(Color.yellow);
                        }
                        else
                        {
                            rootPanel.GetButton(area, "Areas")?.SetTextColor(Color.white);
                        }
                    }
                }

                if ((rootPanel.GetPanel("Rooms")?.active ?? false) && !string.IsNullOrEmpty(DoorWarpSelection.area))
                {
                    foreach (string room in DoorWarp.RoomsByArea[DoorWarpSelection.area])
                    {
                        if (room == DoorWarpSelection.room)
                        {
                            rootPanel.GetButton(room, "Rooms")?.SetTextColor(Color.yellow);
                        }
                        else
                        {
                            rootPanel.GetButton(room, "Rooms")?.SetTextColor(Color.white);
                        }
                    }
                }

                if ((rootPanel.GetPanel("Doors")?.active ?? false) && !string.IsNullOrEmpty(DoorWarpSelection.room))
                {
                    foreach (string door in DoorWarp.DoorsByRoom[DoorWarpSelection.room])
                    {
                        if (door == DoorWarpSelection.door)
                        {
                            rootPanel.GetButton(door, "Doors")?.SetTextColor(Color.yellow);
                        }
                        else
                        {
                            rootPanel.GetButton(door, "Doors")?.SetTextColor(Color.white);
                        }
                    }
                }
            }
            else
            {
                foreach (Bench bench in Bench.Benches)
                {
                    if (!rootPanel.GetPanel(bench.areaName).active) continue;

                    /*
                    if (!bench.visited && !gs.UnlockAllBenches)
                    {
                        rootPanel.GetButton(bench.name, bench.areaName).SetTextColor(Color.red);
                    }
                    */
                    else
                    {
                        rootPanel.GetButton(bench.name, bench.areaName).SetTextColor(bench.benched ? Color.yellow : Color.white);
                    }
                }
            }
        }

        private static void WarpClicked(string buttonName)
        {
            if (Benchwarp.instance.GlobalSettings.DoorWarp)
            {
                if (!string.IsNullOrEmpty(DoorWarpSelection.door)) ChangeScene.ChangeToScene(DoorWarpSelection.room, DoorWarpSelection.door);
                return;
            }


            //if (Benchwarp.instance.GlobalSettings.UnlockAllBenches)
            RepairBench(PlayerData.instance.respawnScene);

            GameManager.instance.StartCoroutine(Benchwarp.instance.Respawn());
        }

        #region Settings button method

        private static void WarpOnlyClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.WarpOnly = !Benchwarp.instance.GlobalSettings.WarpOnly;
            Benchwarp.instance.SaveGlobalSettings();
            rootPanel.Destroy();
            sceneNamePanel.Destroy();
            BuildMenu(canvas);
        }


        static (string, string)[] sdBenches = new (string, string)[]
            {
                ("Hive_01", "Hive Bench"),
                ("Ruins1_31", "Toll Machine Bench"),
                ("Abyss_18", "Toll Machine Bench"),
                ("Fungus3_50", "Toll Machine Bench")
            };
        private static void RepairBench(string sceneName)
        {
            if (sdBenches.FirstOrDefault(p => p.Item1 == sceneName).Item2 is string id)
            {
                GameManager.instance.sceneData.SaveMyState(new PersistentBoolData
                {
                    activated = true,
                    sceneName = sceneName,
                    semiPersistent = false,
                    id = id
                });
            }

            switch (sceneName)
            {
                case "Crossroads_04":
                    PlayerData.instance.visitedCrossroads = true;
                    break;
                case "Room_Tram":
                    PlayerData.instance.openedTramLower = true;
                    PlayerData.instance.tramOpenedDeepnest = true;
                    break;
                case "Room_Tram_RG":
                    PlayerData.instance.openedTramRestingGrounds = true;
                    PlayerData.instance.tramOpenedCrossroads = true;
                    break;
            }
        }

        /*
        private static void UnlockAllClicked(string buttonName)
        {
            if (buttonName != null)
            {
                Benchwarp.instance.GlobalSettings.UnlockAllBenches = !Benchwarp.instance.GlobalSettings.UnlockAllBenches;
                Benchwarp.instance.SaveGlobalSettings();
            }

            if (!Benchwarp.instance.GlobalSettings.UnlockAllBenches) return;

            PlayerData pd = PlayerData.instance;

            FieldInfo[] fields = typeof(PlayerData).GetFields();

            // Most of these are unnecessary, but some titlecards can lock you into a bench
            foreach
            (
                FieldInfo fi in fields.Where
                (
                    x => x.Name.StartsWith("visited")
                        || x.Name.StartsWith("tramOpened")
                        || x.Name.StartsWith("openedTram")
                        || x.Name.StartsWith("tramOpened")
                )
            )
            {
                pd.SetBoolInternal(fi.Name, true);
            }

            //This actually fixes the unlockable benches
            SceneData sd = GameManager.instance.sceneData;

            foreach ((string sceneName, string id) in new (string, string)[]
            {
                ("Hive_01", "Hive Bench"),
                ("Ruins1_31", "Toll Machine Bench"),
                ("Abyss_18", "Toll Machine Bench"),
                ("Fungus3_50", "Toll Machine Bench")
            })
            {
                sd.SaveMyState
                (
                    new PersistentBoolData
                    {
                        sceneName = sceneName,
                        id = id,
                        activated = true,
                        semiPersistent = false
                    }
                );
            }
        }
        */

        private static void ShowSceneClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.ShowScene = !Benchwarp.instance.GlobalSettings.ShowScene;
            Benchwarp.instance.SaveGlobalSettings();
        }

        private static void SwapNamesClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.SwapNames = !Benchwarp.instance.GlobalSettings.SwapNames;
            Benchwarp.instance.SaveGlobalSettings();
            RebuildMenu();
        }

        private static void AlwaysToggleAllClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.AlwaysToggleAll = !Benchwarp.instance.GlobalSettings.AlwaysToggleAll;
            Benchwarp.instance.SaveGlobalSettings();
        }

        private static void DoorWarpClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.DoorWarp = !Benchwarp.instance.GlobalSettings.DoorWarp;
            Benchwarp.instance.SaveGlobalSettings();
            RebuildMenu();
        }

        #endregion

        private static void AllClicked(string buttonName)
        {
            if (benchPanels.Any(s => !rootPanel.GetPanel(s).active))
            {
                foreach (string s in benchPanels)
                    if (!rootPanel.GetPanel(s).active)
                        rootPanel.TogglePanel(s);
            }
            else
            {
                foreach (string s in benchPanels)
                    if (rootPanel.GetPanel(s).active)
                        rootPanel.TogglePanel(s);
            }
        }

        private static void FlipClicked(string buttonName)
        {
            if (string.IsNullOrEmpty(DoorWarpSelection.door)) return;

            Door orig = DoorWarp.Doors.FirstOrDefault(d => d.room == DoorWarpSelection.room && d.door == DoorWarpSelection.door);
            if (orig is null) return;

            DoorWarpSelection.area = orig.mapsTo.area;
            DoorWarpSelection.room = orig.mapsTo.room;
            DoorWarpSelection.door = orig.mapsTo.door;

            CanvasPanel areaPanel = rootPanel.GetPanel("Areas");
            CanvasPanel roomPanel = rootPanel.GetPanel("Rooms");
            CanvasPanel doorPanel = rootPanel.GetPanel("Doors");

            if (!areaPanel.active) areaPanel.ToggleActive();
            areaPanel.GetButton(orig.mapsTo.area).ButtonClicked();
            roomPanel.GetButton(orig.mapsTo.room).ButtonClicked();
            doorPanel.GetButton(orig.mapsTo.door).ButtonClicked();
        }
    }
}