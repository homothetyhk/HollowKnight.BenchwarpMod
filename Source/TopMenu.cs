using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GlobalEnums;

namespace Benchwarp
{
    public static class TopMenu
    {
        private static CanvasPanel panel;
        private static CanvasPanel sceneNamePanel;
        private static GameObject canvas;
        private static float cooldown;
        private static bool onCooldown;
        private static List<string> benchPanels;
        private static int fontSize;

        public static void BuildMenu(GameObject _canvas)
        {
            canvas = _canvas;

            sceneNamePanel = new CanvasPanel(_canvas, GUIController.Instance.images["ButtonsMenuBG"], new Vector2(0f, 0f), new Vector2(1346f, 0f), new Rect(0f, 0f, 0f, 0f));
            sceneNamePanel.AddText("SceneName", "Tutorial_01", new Vector2(5f, 1060f), Vector2.zero, GUIController.Instance.trajanNormal, 18);

            panel = new CanvasPanel(_canvas, GUIController.Instance.images["ButtonsMenuBG"], new Vector2(342f, 15f), new Vector2(1346f, 0f), new Rect(0f, 0f, 0f, 0f));

            Rect buttonRect = new Rect(0, 0, GUIController.Instance.images["ButtonRect"].width, GUIController.Instance.images["ButtonRect"].height);

            fontSize = 12;

            void MakePanel(string name, Vector2 position)
            {
                panel.AddPanel(name, GUIController.Instance.images["ButtonRectEmpty"], position, Vector2.zero, new Rect(0f, 0f, GUIController.Instance.images["DropdownBG"].width, 270f));
                panel.AddButton(name, GUIController.Instance.images["ButtonRect"], position + new Vector2(1f, -20f), Vector2.zero, (s) => panel.TogglePanel(name), buttonRect, GUIController.Instance.trajanBold, name);
            }

            //Main buttons
            panel.AddButton("Warp", GUIController.Instance.images["ButtonRect"], new Vector2(-154f, 40f), Vector2.zero, WarpClicked, buttonRect, GUIController.Instance.trajanBold, "Warp");

            if (Benchwarp.instance.GlobalSettings.EnableDeploy)
            {
                panel.AddButton("Deploy", GUIController.Instance.images["ButtonRect"], new Vector2(-154f, 300f), Vector2.zero, DeployClicked, buttonRect, GUIController.Instance.trajanBold, "Deploy");
                panel.AddButton("Set", GUIController.Instance.images["ButtonRect"], new Vector2(-54f, 300f), Vector2.zero, SetClicked, buttonRect, GUIController.Instance.trajanBold, "Set");
                panel.AddButton("Destroy", GUIController.Instance.images["ButtonRect"], new Vector2(46f, 300f), Vector2.zero, (s) => BenchMaker.DestroyBench(), buttonRect, GUIController.Instance.trajanBold, "Destroy");
                MakePanel("Style", new Vector2(145f, 320f));
                {
                    Vector2 position = new Vector2(5f, 25f);
                    foreach (string style in BenchMaker.Styles)
                    {
                        panel.GetPanel("Style").AddButton(style, GUIController.Instance.images["ButtonRectEmpty"], position, Vector2.zero, StyleChanged, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, style, fontSize);
                        position += new Vector2(0f, 30f);
                    }
                }
                MakePanel("Options", new Vector2(245f, 320f));
                panel.GetPanel("Options").AddButton("Cooldown", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, CooldownClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Cooldown", fontSize);
                panel.GetPanel("Options").AddButton("Noninteractive", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, NoninteractiveClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Noninteractive", fontSize);
                panel.GetPanel("Options").AddButton("No Mid-Air Deploy", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 105f), Vector2.zero, NoMidAirDeployClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "No Mid-Air Deploy", fontSize);
                panel.GetPanel("Options").AddButton("Blacklist", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 145f), Vector2.zero, BlacklistClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Blacklist", fontSize);
            }

            MakePanel("Settings", new Vector2(1445f, 20f));
            panel.GetPanel("Settings").AddButton("WarpOnly", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, WarpOnlyClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Warp Only", fontSize);
            panel.GetPanel("Settings").AddButton("UnlockAll", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, UnlockAllClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Unlock All", fontSize);
            panel.GetPanel("Settings").AddButton("ShowScene", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 105f), Vector2.zero, ShowSceneClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Show Room Name", fontSize);
            panel.GetPanel("Settings").AddButton("SwapNames", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 145f), Vector2.zero, SwapNamesClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Use Room Names", fontSize);
            panel.GetPanel("Settings").AddButton("EnableDeploy", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 185f), Vector2.zero, EnableDeployClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Enable Deploy", fontSize);

            if (Benchwarp.instance.GlobalSettings.WarpOnly) return;

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
                if (!Benchwarp.instance.GlobalSettings.SwapNames)
                {
                    panel.GetPanel(bench.areaName).AddButton(
                        bench.name, 
                        GUIController.Instance.images["ButtonRectEmpty"], 
                        panelButtonHeight[bench.areaName], 
                        Vector2.zero, 
                        (string s) => bench.SetBench(), 
                        new Rect(0f, 0f, 80f, 40f), 
                        GUIController.Instance.trajanNormal, 
                        bench.name, 
                        fontSize
                        );
                }
                else
                {
                    panel.GetPanel(bench.areaName).AddButton(
                        bench.name,
                        GUIController.Instance.images["ButtonRectEmpty"],
                        panelButtonHeight[bench.areaName],
                        Vector2.zero,
                        (string s) => bench.SetBench(),
                        new Rect(0f, 0f, 80f, 40f),
                        GUIController.Instance.trajanNormal,
                        bench.sceneName,
                        fontSize
                        );
                }
            }
            
            panel.AddButton("All", GUIController.Instance.images["ButtonRect"], new Vector2(-154f, 0f), Vector2.zero, AllClicked, buttonRect, GUIController.Instance.trajanBold, "All");

            panel.FixRenderOrder();
        }

        public static void Update()
        {
            if (cooldown > 0)
            {
                cooldown -= Time.unscaledDeltaTime;
            }

            if (panel == null || sceneNamePanel == null)
            {
                return;
            }

            if (Benchwarp.instance.GlobalSettings.ShowScene)
            {
                sceneNamePanel.SetActive(true, false);
                sceneNamePanel.GetText("SceneName").UpdateText(GameManager.instance.sceneName);
            }
            else sceneNamePanel.SetActive(false, true);

            if (GameManager.instance.IsGamePaused())
            {
                panel.SetActive(true, false);
            }
            else if (!GameManager.instance.IsGamePaused())
            {
                panel.SetActive(false, true);
            }

            if (HeroController.instance == null || GameManager.instance == null) return;

            if (Benchwarp.instance.GlobalSettings.EnableDeploy)
            {
                if (onCooldown)
                {
                    panel.GetButton("Deploy").UpdateText(((int)cooldown).ToString());
                }
                if (cooldown <= 0 && onCooldown)
                {
                    panel.GetButton("Deploy").UpdateText("Deploy");
                    onCooldown = false;
                }
                if (onCooldown ||
                    (Benchwarp.instance.GlobalSettings.BlacklistRooms && BenchMaker.Blacklist()) ||
                    (Benchwarp.instance.GlobalSettings.NoMidAirDeploy && !HeroController.instance.CheckTouchingGround()))
                {
                    panel.GetButton("Deploy").SetTextColor(Color.red);
                }
                else panel.GetButton("Deploy").SetTextColor(Color.white);


                panel.GetButton("Set").SetTextColor(Benchwarp.instance.Settings.benchScene == PlayerData.instance.respawnScene && Benchwarp.instance.Settings.benchName == PlayerData.instance.respawnMarkerName
                    ? Color.yellow : Color.white);

                if (panel.GetPanel("Style").active)
                {
                    foreach (string style in BenchMaker.Styles)
                    {
                        panel.GetButton(style, "Style").SetTextColor(Benchwarp.instance.GlobalSettings.benchStyle == style ? Color.yellow : Color.white);
                    }
                }

                if (panel.GetPanel("Options").active)
                {
                    panel.GetButton("Cooldown", "Options").SetTextColor(Benchwarp.instance.GlobalSettings.DeployCooldown ? Color.yellow : Color.white);
                    panel.GetButton("Noninteractive", "Options").SetTextColor(Benchwarp.instance.GlobalSettings.Noninteractive ? Color.yellow : Color.white);
                    panel.GetButton("No Mid-Air Deploy", "Options").SetTextColor(Benchwarp.instance.GlobalSettings.NoMidAirDeploy ? Color.yellow : Color.white);
                    panel.GetButton("Blacklist", "Options").SetTextColor(Benchwarp.instance.GlobalSettings.BlacklistRooms ? Color.yellow : Color.white);
                }
            }

            if (panel.GetPanel("Settings").active)
            {
                panel.GetButton("WarpOnly", "Settings").SetTextColor(Benchwarp.instance.GlobalSettings.WarpOnly ? Color.yellow : Color.white);
                panel.GetButton("UnlockAll", "Settings").SetTextColor(Benchwarp.instance.GlobalSettings.UnlockAllBenches ? Color.yellow : Color.white);
                panel.GetButton("ShowScene", "Settings").SetTextColor(Benchwarp.instance.GlobalSettings.ShowScene ? Color.yellow : Color.white);
                panel.GetButton("SwapNames", "Settings").SetTextColor(Benchwarp.instance.GlobalSettings.SwapNames ? Color.yellow : Color.white);
                panel.GetButton("EnableDeploy", "Settings").SetTextColor(Benchwarp.instance.GlobalSettings.EnableDeploy ? Color.yellow : Color.white);
            }

            foreach (Bench bench in Bench.Benches)
            {
                if (panel.GetPanel(bench.areaName).active)
                {
                    if (!bench.visited && !Benchwarp.instance.GlobalSettings.UnlockAllBenches && bench.sceneName != "Tutorial_01")
                    {
                        panel.GetButton(bench.name, bench.areaName).SetTextColor(Color.red);
                    }
                    else
                    {
                        panel.GetButton(bench.name, bench.areaName).SetTextColor(bench.benched ? Color.yellow : Color.white);
                    }
                }
            }    
        }
           
        private static void WarpClicked(string buttonName)
        {
            if (Benchwarp.instance.GlobalSettings.UnlockAllBenches) UnlockAllClicked(buttonName); // makes various pd changes if necessary

            GameManager.instance.StartCoroutine(Benchwarp.instance.Respawn());
        }

        private static void DeployClicked(string buttonName)
        {
            if (onCooldown) return;
            if (Benchwarp.instance.GlobalSettings.BlacklistRooms && BenchMaker.Blacklist()) return;
            if (Benchwarp.instance.GlobalSettings.NoMidAirDeploy && !HeroController.instance.CheckTouchingGround())
            {
                return;
            }
            BenchMaker.DestroyBench();

            Benchwarp.instance.Settings.benchDeployed = true;
            Benchwarp.instance.Settings.benchX = HeroController.instance.gameObject.transform.position.x;
            Benchwarp.instance.Settings.benchY = HeroController.instance.gameObject.transform.position.y;
            Benchwarp.instance.Settings.benchScene = GameManager.instance.sceneName;

            BenchMaker.MakeBench();
            SetClicked(null);
            if (Benchwarp.instance.GlobalSettings.DeployCooldown)
            {
                cooldown = 60f;
                onCooldown = true;
            }
        }

        private static void SetClicked(string buttonName)
        {
            if (!Benchwarp.instance.Settings.benchDeployed) return;
            PlayerData.instance.respawnScene = Benchwarp.instance.Settings.benchScene;
            PlayerData.instance.respawnType = 1;
            PlayerData.instance.respawnMarkerName = Benchwarp.instance.Settings.benchName;
        }

        #region Deploy options

        private static void StyleChanged(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.benchStyle = buttonName;
            Benchwarp.instance.SaveGlobalSettings();
        }

        private static void CooldownClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.DeployCooldown = !Benchwarp.instance.GlobalSettings.DeployCooldown;
            Benchwarp.instance.SaveGlobalSettings();
            cooldown = 0f;
        }

        private static void NoninteractiveClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.Noninteractive = !Benchwarp.instance.GlobalSettings.Noninteractive;
            Benchwarp.instance.SaveGlobalSettings();
            if (!Benchwarp.instance.GlobalSettings.Noninteractive && BenchMaker.DeployedBench != null)
            {
                BenchMaker.MakeBench();
            }
        }

        private static void NoMidAirDeployClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.NoMidAirDeploy = !Benchwarp.instance.GlobalSettings.NoMidAirDeploy;
            Benchwarp.instance.SaveGlobalSettings();
        }

        private static void BlacklistClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.BlacklistRooms = !Benchwarp.instance.GlobalSettings.BlacklistRooms;
            Benchwarp.instance.SaveGlobalSettings();
        }

        #endregion

        #region Settings button method
        private static void WarpOnlyClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.WarpOnly = !Benchwarp.instance.GlobalSettings.WarpOnly;
            Benchwarp.instance.SaveGlobalSettings();
            panel.Destroy();
            sceneNamePanel.Destroy();
            BuildMenu(canvas);
        }

        private static void UnlockAllClicked(string buttonName)
        {
            if (buttonName != "Warp")
            {
                Benchwarp.instance.GlobalSettings.UnlockAllBenches = !Benchwarp.instance.GlobalSettings.UnlockAllBenches;
                Benchwarp.instance.SaveGlobalSettings();
            }
            
            if (Benchwarp.instance.GlobalSettings.UnlockAllBenches)
            {
                PlayerData pd = PlayerData.instance;

                //Most of these are unnecessary, but some titlecards can lock you into a bench
                pd.SetBoolInternal("visitedAbyss", true);
                pd.SetBoolInternal("visitedAbyssLower", true);
                pd.SetBoolInternal("visitedCliffs", true);
                pd.SetBoolInternal("visitedCrossroads", true);
                pd.SetBoolInternal("visitedDeepnest", true);
                pd.SetBoolInternal("visitedDirtmouth", true);
                pd.SetBoolInternal("visitedGreenpath", true);
                pd.SetBoolInternal("visitedFogCanyon", true);
                pd.SetBoolInternal("visitedFungus", true);
                pd.SetBoolInternal("visitedHive", true);
                pd.SetBoolInternal("visitedGodhome", true);
                pd.SetBoolInternal("visitedMines", true);
                pd.SetBoolInternal("visitedOutskirts", true);
                pd.SetBoolInternal("visitedRestingGrounds", true);
                pd.SetBoolInternal("visitedRoyalGardens", true);
                pd.SetBoolInternal("visitedRestingGrounds", true);
                pd.SetBoolInternal("visitedRuins", true);
                pd.SetBoolInternal("visitedWaterways", true);
                pd.SetBoolInternal("visitedWhitePalace", true);

                // Only two of these do anything
                pd.SetBoolInternal("tramOpenedCrossroads", true);
                pd.SetBoolInternal("openedTramRestingGrounds", true);
                pd.SetBoolInternal("tramOpenedDeepnest", true);
                pd.SetBoolInternal("openedTramLower", true);
                pd.SetBoolInternal("tollBenchAbyss", true);
                pd.SetBoolInternal("tollBenchCity", true);
                pd.SetBoolInternal("tollBenchQueensGardens", true);

                //This actually fixes the unlockable benches
                SceneData sd = GameManager.instance.sceneData;
                sd.SaveMyState(new PersistentBoolData
                {
                    sceneName = "Hive_01",
                    id = "Hive Bench",
                    activated = true,
                    semiPersistent = false
                });
                sd.SaveMyState(new PersistentBoolData
                {
                    sceneName = "Ruins1_31",
                    id = "Toll Machine Bench",
                    activated = true,
                    semiPersistent = false
                });
                sd.SaveMyState(new PersistentBoolData
                {
                    sceneName = "Abyss_18",
                    id = "Toll Machine Bench",
                    activated = true,
                    semiPersistent = false
                });
                sd.SaveMyState(new PersistentBoolData
                {
                    sceneName = "Fungus3_50",
                    id = "Toll Machine Bench",
                    activated = true,
                    semiPersistent = false
                });
            }
        }

        private static void ShowSceneClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.ShowScene = !Benchwarp.instance.GlobalSettings.ShowScene;
            Benchwarp.instance.SaveGlobalSettings();
        }

        private static void SwapNamesClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.SwapNames = !Benchwarp.instance.GlobalSettings.SwapNames;
            Benchwarp.instance.SaveGlobalSettings();
            panel.Destroy();
            sceneNamePanel.Destroy();
            BuildMenu(canvas);
        }

        private static void EnableDeployClicked(string buttonName)
        {
            Benchwarp.instance.GlobalSettings.EnableDeploy = !Benchwarp.instance.GlobalSettings.EnableDeploy;
            Benchwarp.instance.SaveGlobalSettings();
            BenchMaker.DestroyBench();
            panel.Destroy();
            sceneNamePanel.Destroy();
            BuildMenu(canvas);
        }
        #endregion

        #region Dropdown toggle methods
        private static void AllClicked(string buttonName)
        {
            if (benchPanels.Any(s => !panel.GetPanel(s).active))
            {
                foreach (string s in benchPanels) if (!panel.GetPanel(s).active) panel.TogglePanel(s);
            }
            else
            {
                foreach (string s in benchPanels) if (panel.GetPanel(s).active) panel.TogglePanel(s);
            }
        }

        private static void StyleClicked(string buttonName)
        {
            panel.TogglePanel("Style");
        }

        private static void OptionsClicked(string buttonName)
        {
            panel.TogglePanel("Options");
        }

        private static void SettingsClicked(string buttonName)
        {
            panel.TogglePanel("Settings");
        }
        #endregion
    }
}
