using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Logger = Modding.Logger;

namespace Benchwarp
{
    public class GUIController : MonoBehaviour
    {
        public static void Setup()
        {
            GameObject GUIObj = new("Benchwarp GUI");
            _instance = GUIObj.AddComponent<GUIController>();
            DontDestroyOnLoad(GUIObj);
        }

        public static void Unload()
        {
            Destroy(_instance.canvas);
            Destroy(_instance.gameObject);
        }

        public Font TrajanBold { get; private set; }
        public Font TrajanNormal { get; private set; }

        private Font Arial { get; set; }
        
        public Dictionary<string, Texture2D> images = new();

        private string last2Keystrokes = "";

        private GameObject canvas;
        private static GUIController _instance;

        public void BuildMenus()
        {
            LoadResources();

            canvas = new GameObject();
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvas.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            canvas.AddComponent<GraphicRaycaster>();

            TopMenu.BuildMenu(canvas);

            DontDestroyOnLoad(canvas);
        }

        private void LoadResources()
        {
            TrajanBold = Modding.CanvasUtil.TrajanBold;
            TrajanNormal = Modding.CanvasUtil.TrajanNormal;

            try
            {
                Arial = Font.CreateDynamicFontFromOSFont
                (
                    Font.GetOSInstalledFontNames().First(x => x.ToLower().Contains("arial")),
                    13
                );
            }
            catch
            {
                Logger.LogWarn("Unable to find Arial! Using Perpetua.");
                Arial = Modding.CanvasUtil.GetFont("Perpetua");
            }

            if (TrajanBold == null || TrajanNormal == null || Arial == null)
            {
                Benchwarp.instance.LogError("Could not find game fonts");
            }

            Assembly asm = Assembly.GetExecutingAssembly();
            
            foreach (string res in asm.GetManifestResourceNames())
            {
                if (!res.StartsWith("Benchwarp.Resources.Images.")) continue;
                
                try
                {
                    using Stream imageStream = asm.GetManifestResourceStream(res);

                    byte[] buffer = new byte[imageStream.Length];
                    imageStream.Read(buffer, 0, buffer.Length);

                    Texture2D tex = new(1, 1);
                    tex.LoadImage(buffer.ToArray());

                    string[] split = res.Split('.');
                    string internalName = split[split.Length - 2];

                    images.Add(internalName, tex);
                }
                catch (Exception e)
                {
                    Benchwarp.instance.LogError("Failed to load image: " + res + "\n" + e);
                }
            }
        }

        public void Update()
        {
            try
            {
                TopMenu.Update();
                DetectHotkeys();
            }
            catch (Exception e)
            {
                Benchwarp.instance.LogError(e);
            }
        }

        private void DetectHotkeys()
        { 
            if (!(GameManager.instance != null && GameManager.instance.IsGamePaused() && Benchwarp.GS.EnableHotkeys))
            {
                last2Keystrokes = "";
                return;
            }
            
            foreach (KeyCode letter in BenchLetters)
            {
                if (Input.GetKeyDown(letter))
                {
                    if (last2Keystrokes.Length == 2)
                    {
                        last2Keystrokes = last2Keystrokes.Remove(0, 1);
                    }
                    last2Keystrokes += letter.ToString();
                }
            }
            if (Hotkeys.CurrentHotkeys.TryGetValue(last2Keystrokes, out int benchNum))
            {
                last2Keystrokes = "";
                switch (benchNum)
                {
                    case Hotkeys.LastBenchID:
                        break;
                    case Hotkeys.StartBenchID:
                        Events.SetToStart();
                        break;
                    case Hotkeys.WarpDeployID:
                        TopMenu.SetClicked(null);
                        break;
                    case Hotkeys.ToggleMenuID:
                        Benchwarp.GS.ShowMenu = !Benchwarp.GS.ShowMenu;
                        return;
                    case Hotkeys.DoorWarpID:
                        TopMenu.DoorWarpClicked(null);
                        return;
                    case Hotkeys.DeployBenchID:
                        TopMenu.DeployClicked(null);
                        return;
                    default:
                        if (0 <= benchNum && benchNum < Bench.Benches.Length) Bench.Benches[benchNum].SetBench();
                        else
                        {
                            Benchwarp.instance.LogError($"Unknown internal hotkey code: {benchNum}");
                            return;
                        }
                        break;
                }
                ChangeScene.WarpToRespawn();
            }
        }

        public static GUIController Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<GUIController>();

                if (_instance != null) return _instance;

                Benchwarp.instance.LogWarn("Couldn't find GUIController");

                GameObject GUIObj = new();
                _instance = GUIObj.AddComponent<GUIController>();
                DontDestroyOnLoad(GUIObj);

                return _instance;
            }
        }

        private static HashSet<KeyCode> BenchLetters = new()
        {
            KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z
        };
    }
}