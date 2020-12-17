using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Modding;
using UnityEngine;
using UnityEngine.UI;
using Logger = Modding.Logger;

namespace Benchwarp
{
    public class GUIController : MonoBehaviour
    {
        public Font TrajanBold { get; private set; }
        public Font TrajanNormal { get; private set; }

        private Font Arial { get; set; }
        
        public Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();

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
            CanvasUtil.CreateFonts();

            TrajanBold = CanvasUtil.TrajanBold;
            TrajanNormal = CanvasUtil.TrajanNormal;

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
                Arial = CanvasUtil.GetFont("Perpetua");
            }

            if (TrajanBold == null || TrajanNormal == null || Arial == null)
            {
                Benchwarp.instance.LogError("Could not find game fonts");
            }

            Assembly asm = Assembly.GetExecutingAssembly();
            
            foreach (string res in asm.GetManifestResourceNames())
            {
                if (!res.StartsWith("Benchwarp.Images.")) continue;
                
                try
                {
                    using (Stream imageStream = asm.GetManifestResourceStream(res))
                    {

                        byte[] buffer = new byte[imageStream.Length];
                        imageStream.Read(buffer, 0, buffer.Length);

                        Texture2D tex = new Texture2D(1, 1);
                        tex.LoadImage(buffer.ToArray());

                        string[] split = res.Split('.');
                        string internalName = split[split.Length - 2];
                        
                        images.Add(internalName, tex);

                        Benchwarp.instance.Log("Loaded image: " + internalName);
                    }
                }
                catch (Exception e)
                {
                    Benchwarp.instance.LogError("Failed to load image: " + res + "\n" + e);
                }
            }
        }

        public void Update()
        {
            TopMenu.Update();
            DetectHotkeys();
        }

        private void DetectHotkeys() {
            if (!GameManager.instance.IsGamePaused())
            {
                return;
            }
            
            foreach (var letter in BenchLetters)
            {
                if (Input.GetKeyDown(letter.Key))
                {
                    if (last2Keystrokes.Length == 2)
                    {
                        last2Keystrokes = last2Keystrokes.Remove(0, 1);
                    }
                    last2Keystrokes = last2Keystrokes + letter.Value;
                }
            }
            if (BenchHotkeys.TryGetValue(last2Keystrokes, out int benchNum))
            {
                last2Keystrokes = "";
                Benchwarp.instance.ApplyUnlockAllFixes();
                Bench.Benches[benchNum].SetBench();
                GameManager.instance.StartCoroutine(Benchwarp.instance.Respawn());
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

                GameObject GUIObj = new GameObject();
                _instance = GUIObj.AddComponent<GUIController>();
                DontDestroyOnLoad(GUIObj);

                return _instance;
            }
        }

        private static Dictionary<KeyCode, char> BenchLetters = new Dictionary<KeyCode, char>() {
            {KeyCode.A, 'a'},
            {KeyCode.B, 'b'},
            {KeyCode.C, 'c'},
            {KeyCode.D, 'd'},
            {KeyCode.E, 'e'},
            {KeyCode.F, 'f'},
            {KeyCode.G, 'g'},
            {KeyCode.H, 'h'},
            {KeyCode.K, 'k'},
            {KeyCode.L, 'l'},
            {KeyCode.M, 'm'},
            {KeyCode.N, 'n'},
            {KeyCode.O, 'o'},
            {KeyCode.P, 'p'},
            {KeyCode.Q, 'q'},
            {KeyCode.R, 'r'},
            {KeyCode.S, 's'},
            {KeyCode.T, 't'},
            {KeyCode.U, 'u'},
            {KeyCode.W, 'w'}
        };

        private static Dictionary<string, int> BenchHotkeys = new Dictionary<string, int>() {
            {"kp", 0},
            {"dm", 1},
            {"nm", 2},

            {"fs", 3},
            {"fc", 4},
            {"sa", 5},
            {"am", 6},
            {"be", 7},

            {"gw", 8},
            {"ss", 9},
            {"gt", 10},
            {"gp", 11},
            {"lu", 12},
            {"ns", 13},

            {"ta", 14},

            {"qs", 15},
            {"le", 16},
            {"br", 17},
            {"mv", 18},

            {"cq", 19},
            {"ct", 20},
            {"cs", 21},
            {"ws", 22},
            {"ks", 23},
            {"ph", 24},

            {"ww", 25},
            {"ga", 26},
            {"gr", 27},
            {"hg", 28},

            {"ds", 29},
            {"ft", 30},
            {"bd", 31},

            {"bt", 32},
            {"hs", 33},

            {"no", 34},
            {"ec", 35},
            {"cf", 36},
            {"bb", 37},

            {"pd", 38},
            {"cg", 39},

            {"rg", 40},
            {"gm", 41},

            {"qc", 42},
            {"qt", 43},
            {"qg", 44},

            {"pe", 45},
            {"pa", 46},
            {"pb", 47},

            {"ut", 48},
            {"lt", 49}
        };
    }
}