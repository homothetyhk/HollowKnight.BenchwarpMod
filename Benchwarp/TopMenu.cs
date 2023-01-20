using Benchwarp.CanvasUtil;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

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
        private static float cooldown;
        private static bool onCooldown;
        private static List<string> benchPanels;
        private static int fontSize;

        private static readonly Type t = typeof(GlobalSettings);

        private static readonly Dictionary<string, (string, UnityAction<string>, FieldInfo)[]> Panels =
            new()
            {
                ["Options"] = new (string, UnityAction<string>, FieldInfo)[]
                {
                    ("Cooldown", CooldownClicked, t.GetField(nameof(GlobalSettings.DeployCooldown))),
                    ("Noninteractive", NoninteractiveClicked, t.GetField(nameof(GlobalSettings.Noninteractive))),
                    ("No Mid-Air Deploy", NoMidAirDeployClicked, t.GetField(nameof(GlobalSettings.NoMidAirDeploy))),
                    ("No Dark or Dream Rooms", NoDarkOrDreamClicked, t.GetField(nameof(GlobalSettings.NoDarkOrDreamRooms))),
                    ("No Preload", NoPreloadClicked, t.GetField(nameof(GlobalSettings.NoPreload))),
                    ("Apply Style To All", VanillaBenchStylesClicked, t.GetField(nameof(GlobalSettings.ModifyVanillaBenchStyles))),
                },

                ["Settings"] = new (string, UnityAction<string>, FieldInfo)[]
                {
                    ("Warp Only", WarpOnlyClicked, t.GetField(nameof(GlobalSettings.WarpOnly))),
                    ("Unlock All", UnlockAllClicked, t.GetField(nameof(GlobalSettings.UnlockAllBenches))),
                    ("Show Room Name", ShowSceneClicked, t.GetField(nameof(GlobalSettings.ShowScene))),
                    ("Use Room Names", SwapNamesClicked, t.GetField(nameof(GlobalSettings.SwapNames))),
                    ("Enable Deploy", EnableDeployClicked, t.GetField(nameof(GlobalSettings.EnableDeploy))),
                    ("Always Toggle All", AlwaysToggleAllClicked, t.GetField(nameof(GlobalSettings.AlwaysToggleAll))),
                    ("Door Warp", DoorWarpClicked, t.GetField(nameof(GlobalSettings.DoorWarp))),
                    ("Enable Hotkeys", EnableHotkeysClicked, t.GetField(nameof(GlobalSettings.EnableHotkeys))),
                }
            };

        private static readonly Dictionary<string, (UnityAction<string>, Vector2)> Buttons = new()
        {
            ["Deploy"] = (DeployClicked, new Vector2(-154f, 400f)),
            ["Set"] = (SetClicked, new Vector2(-54f, 400f)),
            ["Destroy"] = (s => BenchMaker.DestroyBench(), new Vector2(46f, 400f)),
        };

        private static readonly Dictionary<string, (UnityAction<string>, Vector2)> CustomStartButtons = new()
        {
            ["Set Start"] = (s => Events.SetToStart(), new Vector2(-154f, 80f))
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
            if (!canvas) return; // Benchwarp is toggled off

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

            Rect buttonRect = new(0, 0, GUIController.Instance.images["ButtonRect"].width, GUIController.Instance.images["ButtonRect"].height);

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

            if (Benchwarp.GS.EnableDeploy)
            {
                foreach (KeyValuePair<string, (UnityAction<string>, Vector2)> pair in Buttons)
                {
                    rootPanel.AddButton
                    (
                        pair.Key,
                        GUIController.Instance.images["ButtonRect"],
                        pair.Value.Item2,
                        Vector2.zero,
                        pair.Value.Item1,
                        buttonRect,
                        GUIController.Instance.TrajanBold,
                        pair.Key,
                        fontSize: 11
                    );
                }

                CanvasPanel nearStyle = MakePanel("Near Style", new Vector2(145f, 420f));
                {
                    Vector2 position = new(5f, 25f);

                    foreach (string styleName in BenchStyle.StyleNames)
                    {
                        AddButton(nearStyle, styleName, NearStyleChanged, position);

                        position += new Vector2(0f, 23f);
                    }
                }
                

                CanvasPanel farStyle = MakePanel("Far Style", new Vector2(245f, 420f));
                {
                    Vector2 position = new(5f, 25f);

                    foreach (string styleName in BenchStyle.StyleNames)
                    {
                        AddButton(farStyle, styleName, FarStyleChanged, position);

                        position += new Vector2(0f, 23f);
                    }
                }
                


                CanvasPanel options = MakePanel("Options", new Vector2(345f, 420f));

                for (int i = 0; i < Panels["Options"].Length; i++)
                {
                    (string name, UnityAction<string> action, FieldInfo _) = Panels["Options"][i];

                    AddButton
                    (
                        options,
                        name,
                        action,
                        new Vector2(5f, 25 + i * 40)
                    );
                }
            }

            



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

            if (Benchwarp.GS.WarpOnly) return;

            DoorWarpSelection.Clear();
            if (Benchwarp.GS.DoorWarp)
            {
                CanvasPanel door3 = MakePanel("Doors", new Vector2(-5f, 20f));
                CanvasPanel door2 = MakePanel("Rooms", new Vector2(395f, 20f));
                CanvasPanel door1 = MakePanel("Areas", new Vector2(1045f, 20f));
                // List<string> doorAreas = DoorWarp.Doors.Select(d => d.area).Distinct().ToList();
                string[] doorAreas = DoorWarp.Areas;
                for (int i = 0; i < doorAreas.Length; i++)
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
                        string[] rooms = DoorWarp.RoomsByArea[areaSelected];
                        for (int j = 0; j < rooms.Length; j++)
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
                                    string[] doors = DoorWarp.DoorsByRoom[roomSelected];
                                    for (int k = 0; k < doors.Length; k++)
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
                                GridPosition(j, 6, 100f, 40f, new Vector2(5f, 25f)),
                                displayName: Events.GetSceneName(rooms[j])
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


            //if (!CustomStartLocation.Inactive) button should exist or else will give an nre
            {
                foreach (KeyValuePair<string, (UnityAction<string>, Vector2)> pair in CustomStartButtons)
                {
                    rootPanel.AddButton
                    (
                        pair.Key,
                        GUIController.Instance.images["ButtonRect"],
                        pair.Value.Item2,
                        Vector2.zero,
                        pair.Value.Item1,
                        buttonRect,
                        GUIController.Instance.TrajanBold,
                        pair.Key
                    );
                }
            }

            Vector2 panelDistance = new(-155f, 20f);

            Dictionary<string, Vector2> panelButtonHeight = new();
            benchPanels = new List<string>();

            for (int i = 0; i < Bench.Benches.Count; i++)
            {
                Bench bench = Bench.Benches[i];
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

                if (Benchwarp.GS.EnableHotkeys)
                {
                    rootPanel.GetPanel(bench.areaName)
                        .AddText
                        (
                            bench.name + " index",
                            $"{i:D2}",
                            panelButtonHeight[bench.areaName] - new Vector2(7f, 4.5f),
                            Vector2.zero,
                            GUIController.Instance.TrajanNormal,
                            fontSize - 1
                        );
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
                             !Benchwarp.GS.SwapNames ? Events.GetBenchName(bench) : Events.GetBenchSceneName(bench),
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
            Benchwarp bw = Benchwarp.instance;
            GlobalSettings gs = Benchwarp.GS;

            if (cooldown > 0)
            {
                cooldown -= Time.unscaledDeltaTime;
            }

            if (rootPanel == null || sceneNamePanel == null || GameManager.UnsafeInstance == null) return;
            if (gs.ShowScene)
            {
                sceneNamePanel.SetActive(true, false);
                string sceneText = Events.GetSceneName(GameManager.instance.sceneName);
                if (HeroController.SilentInstance != null)
                {
                    Vector2 heroPos = HeroController.instance.transform.position;
                    sceneText += $" {heroPos}";
                }

                CanvasText sceneNameText = sceneNamePanel.GetText("SceneName");
                
                if (gs.MaxSceneNames > 1)
                {
                    int nLines = 1;
                
                    for (int i = 1; i < Math.Min(USceneManager.sceneCount, gs.MaxSceneNames); i++)
                    {
                        sceneText += $"\n{Events.GetSceneName(USceneManager.GetSceneAt(i).name)}";
                        nLines++;
                    }

                    if (USceneManager.sceneCount > gs.MaxSceneNames)
                    {
                        sceneText += $"\n(+ {USceneManager.sceneCount - gs.MaxSceneNames} more)";
                        nLines++;
                    }
                    
                    sceneNameText.SetPosition(new Vector2(5f, 1080f - 20 * nLines));
                }

                sceneNameText.UpdateText(sceneText);
            }
            else sceneNamePanel.SetActive(false, true);


            if (!Benchwarp.GS.ShowMenu || HeroController.SilentInstance == null || !GameManager.instance.IsGameplayScene() || !GameManager.instance.IsGamePaused())
            {
                if (rootPanel.active) rootPanel.SetActive(false, true);
                return;
            }
            else
            {
                if (!rootPanel.active)
                {
                    RebuildMenu();
                }
            }

            if (gs.AlwaysToggleAll && !gs.DoorWarp && !gs.WarpOnly)
            {
                foreach (string s in benchPanels)
                    if (!rootPanel.GetPanel(s).active)
                        rootPanel.TogglePanel(s);
            }

            if (gs.EnableDeploy)
            {
                CanvasButton deploy = rootPanel.GetButton("Deploy");

                if (onCooldown)
                {
                    deploy.UpdateText(((int) cooldown).ToString());
                }

                if (cooldown <= 0 && onCooldown)
                {
                    deploy.UpdateText("Deploy");
                    onCooldown = false;
                }

                bool cantDeploy = onCooldown
                    || gs.NoDarkOrDreamRooms && BenchMaker.IsDarkOrDreamRoom()
                    || gs.NoMidAirDeploy && !HeroController.instance.CheckTouchingGround();

                deploy.SetTextColor(cantDeploy ? Color.red : Color.white);

                rootPanel.GetButton("Set")
                         .SetTextColor
                         (
                             Benchwarp.LS.atDeployedBench
                                 ? Color.yellow
                                 : Color.white
                         );

                if (rootPanel.GetPanel("Near Style").active)
                {
                    foreach (string style in BenchStyle.StyleNames)
                    {
                        rootPanel.GetButton(style, "Near Style").SetTextColor(BenchStyle.IsValidStyle(style) && !PlayerData.instance.atBench ? gs.nearStyle == style ? Color.yellow : Color.white : Color.red);
                    }
                }

                if (rootPanel.GetPanel("Far Style").active)
                {
                    foreach (string style in BenchStyle.StyleNames)
                    {
                        rootPanel.GetButton(style, "Far Style").SetTextColor(BenchStyle.IsValidStyle(style) && !PlayerData.instance.atBench ? gs.farStyle == style ? Color.yellow : Color.white : Color.red);
                    }
                }

                CanvasPanel options = rootPanel.GetPanel("Options");

                if (options.active)
                {
                    foreach ((string name, FieldInfo fi) in Panels["Options"].Select(x => (x.Item1, x.Item3)))
                    {
                        options.GetButton(name).SetTextColor((bool) fi.GetValue(gs) ? Color.yellow : Color.white);
                    }
                }
            }

            CanvasPanel settings = rootPanel.GetPanel("Settings");

            if (settings.active)
            {
                foreach ((string name, FieldInfo fi) in Panels["Settings"].Select(x => (x.Item1, x.Item3)))
                {
                    settings.GetButton(name).SetTextColor((bool) fi.GetValue(gs) ? Color.yellow : Color.white);
                }
            }

            //if (!CustomStartLocation.Inactive && rootPanel.GetButton("Set Start") is CanvasButton startButton)
            if (rootPanel.GetButton("Set Start") is CanvasButton startButton)
            {
                startButton.SetTextColor(Events.AtStart() ? Color.yellow : Color.white);
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
            else if (!gs.WarpOnly)
            {

                foreach (Bench bench in Bench.Benches)
                {
                    if (!rootPanel.GetPanel(bench.areaName).active) continue;

                    if (!bench.HasVisited() && !gs.UnlockAllBenches)
                    {
                        rootPanel.GetButton(bench.name, bench.areaName)
                            .SetTextColor(bench.IsLocked() ? Color.Lerp(Color.magenta, Color.black, 0.5f) : Color.red);
                    }
                    else
                    {
                        rootPanel.GetButton(bench.name, bench.areaName)
                            .SetTextColor(bench.AtBench() ? Color.yellow : Color.white);
                    }
                }
            }
        }

        private static void WarpClicked(string buttonName)
        {
            if (Benchwarp.GS.DoorWarp)
            {
                if (!string.IsNullOrEmpty(DoorWarpSelection.door)) ChangeScene.ChangeToScene(DoorWarpSelection.room, DoorWarpSelection.door);
                return;
            }

            ChangeScene.WarpToRespawn();
        }

        public static void DeployClicked(string buttonName)
        {
            if (!Benchwarp.GS.EnableDeploy) return;
            if (onCooldown) return;
            if (Benchwarp.GS.NoDarkOrDreamRooms && BenchMaker.IsDarkOrDreamRoom()) return;
            if (Benchwarp.GS.NoMidAirDeploy && !HeroController.instance.CheckTouchingGround()) return;

            BenchMaker.DestroyBench();

            Benchwarp.LS.benchDeployed = true;
            Benchwarp.LS.benchX = HeroController.instance.transform.position.x;
            Benchwarp.LS.benchY = HeroController.instance.transform.position.y;
            Benchwarp.LS.benchScene = GameManager.instance.sceneName;

            BenchMaker.MakeDeployedBench();

            SetClicked(null);

            if (!Benchwarp.GS.DeployCooldown) return;

            cooldown = 300f;
            onCooldown = true;
        }

        public static void SetClicked(string buttonName)
        {
            if (!Benchwarp.LS.benchDeployed || Benchwarp.GS.WarpOnly) return;
            Benchwarp.LS.atDeployedBench = true;
            Events.InvokeOnBenchSelected();
        }

        #region Deploy options

        private static void NearStyleChanged(string buttonName)
        {
            if (!BenchStyle.IsValidStyle(buttonName) || PlayerData.instance.atBench) return;
            Benchwarp.GS.nearStyle = buttonName;
            BenchMaker.UpdateStyleFromMenu();
            Benchwarp.instance.SaveGlobalSettings();
        }

        private static void FarStyleChanged(string buttonName)
        {
            if (!BenchStyle.IsValidStyle(buttonName) || PlayerData.instance.atBench) return;
            Benchwarp.GS.farStyle = buttonName;
            BenchMaker.UpdateStyleFromMenu();
            Benchwarp.instance.SaveGlobalSettings();
        }

        private static void CooldownClicked(string buttonName)
        {
            Benchwarp.GS.DeployCooldown = !Benchwarp.GS.DeployCooldown;
            Benchwarp.instance.SaveGlobalSettings();
            cooldown = 0f;
        }

        private static void NoninteractiveClicked(string buttonName)
        {
            Benchwarp.GS.Noninteractive = !Benchwarp.GS.Noninteractive;
            Benchwarp.instance.SaveGlobalSettings();
            if (BenchMaker.DeployedBench != null && ObjectCache.DidPreload)
            {
                BenchMaker.UpdateInteractive(BenchMaker.DeployedBench, Benchwarp.GS.Noninteractive);
            }
        }

        private static void NoMidAirDeployClicked(string buttonName)
        {
            Benchwarp.GS.NoMidAirDeploy = !Benchwarp.GS.NoMidAirDeploy;
            Benchwarp.instance.SaveGlobalSettings();
        }

        private static void NoDarkOrDreamClicked(string buttonName)
        {
            Benchwarp.GS.NoDarkOrDreamRooms = !Benchwarp.GS.NoDarkOrDreamRooms;
            Benchwarp.instance.SaveGlobalSettings();
        }

        private static void NoPreloadClicked(string buttonName)
        {
            Benchwarp.GS.NoPreload = !Benchwarp.GS.NoPreload;
            Benchwarp.instance.SaveGlobalSettings();
        }

        private static void VanillaBenchStylesClicked(string buttonName)
        {
            Benchwarp.GS.ModifyVanillaBenchStyles = !Benchwarp.GS.ModifyVanillaBenchStyles;
            Benchwarp.instance.SaveGlobalSettings();
        }

        #endregion

        #region Settings button method

        private static void WarpOnlyClicked(string buttonName)
        {
            Benchwarp.GS.WarpOnly = !Benchwarp.GS.WarpOnly;
            Benchwarp.instance.SaveGlobalSettings();
            rootPanel.Destroy();
            sceneNamePanel.Destroy();
            BuildMenu(canvas);
        }

        private static void UnlockAllClicked(string buttonName)
        {
            if (buttonName != null)
            {
                Benchwarp.GS.UnlockAllBenches = !Benchwarp.GS.UnlockAllBenches;
                Benchwarp.instance.SaveGlobalSettings();
            }
        }

        private static void ShowSceneClicked(string buttonName)
        {
            Benchwarp.GS.ShowScene = !Benchwarp.GS.ShowScene;
            Benchwarp.instance.SaveGlobalSettings();
        }

        private static void SwapNamesClicked(string buttonName)
        {
            Benchwarp.GS.SwapNames = !Benchwarp.GS.SwapNames;
            Benchwarp.instance.SaveGlobalSettings();
            RebuildMenu();
        }

        private static void EnableDeployClicked(string buttonName)
        {
            Benchwarp.GS.EnableDeploy = !Benchwarp.GS.EnableDeploy;
            Benchwarp.instance.SaveGlobalSettings();
            BenchMaker.DestroyBench();
            RebuildMenu();
        }

        private static void AlwaysToggleAllClicked(string buttonName)
        {
            Benchwarp.GS.AlwaysToggleAll = !Benchwarp.GS.AlwaysToggleAll;
            Benchwarp.instance.SaveGlobalSettings();
        }

        public static void DoorWarpClicked(string buttonName)
        {
            Benchwarp.GS.DoorWarp = !Benchwarp.GS.DoorWarp;
            Benchwarp.instance.SaveGlobalSettings();
            RebuildMenu();
        }

        private static void EnableHotkeysClicked(string buttonName)
        {
            Benchwarp.GS.EnableHotkeys = !Benchwarp.GS.EnableHotkeys;
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
            if (string.IsNullOrEmpty(DoorWarpSelection.door) || string.IsNullOrEmpty(DoorWarpSelection.room)) return;

            if (!DoorWarp.IndexedDoors.TryGetValue(DoorWarpSelection.room, out Dictionary<string, Door> roomDoors) || !roomDoors.TryGetValue(DoorWarpSelection.door, out Door orig))
            {
                return;
            }

            if (orig.Target.IsInvalid()) return;

            if (!DoorWarp.IndexedDoors.TryGetValue(orig.Target.room, out Dictionary<string, Door> roomDoors2) || !roomDoors2.TryGetValue(orig.Target.door, out Door target))
            {
                return;
            }

            CanvasPanel areaPanel = rootPanel.GetPanel("Areas");
            CanvasPanel roomPanel = rootPanel.GetPanel("Rooms");
            CanvasPanel doorPanel = rootPanel.GetPanel("Doors");

            if (!areaPanel.active) areaPanel.ToggleActive();
            areaPanel.GetButton(target.Area).ButtonClicked();
            roomPanel.GetButton(target.Self.room).ButtonClicked();
            doorPanel.GetButton(target.Self.door).ButtonClicked();
        }
    }
}