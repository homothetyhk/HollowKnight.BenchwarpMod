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
            if (!(GameManager.UnsafeInstance != null && GameManager.instance.IsGamePaused() && Benchwarp.GS.EnableHotkeys))
            {
                last2Keystrokes = "";
                return;
            }

            for (KeyCode letter = KeyCode.A; letter <= KeyCode.Z; letter++)
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
            for (KeyCode alpha = KeyCode.Alpha0; alpha <= KeyCode.Alpha9; alpha++)
            {
                if (Input.GetKeyDown(alpha))
                {
                    if (last2Keystrokes.Length == 2)
                    {
                        last2Keystrokes = last2Keystrokes.Remove(0, 1);
                    }
                    last2Keystrokes += (alpha - KeyCode.Alpha0).ToString();
                }
            }
            for (KeyCode pad = KeyCode.Keypad0; pad <= KeyCode.Keypad9; pad++)
            {
                if (Input.GetKeyDown(pad))
                {
                    if (last2Keystrokes.Length == 2)
                    {
                        last2Keystrokes = last2Keystrokes.Remove(0, 1);
                    }
                    last2Keystrokes += (pad - KeyCode.Keypad0).ToString();
                }
            }

            if (Hotkeys.TryGetActionID(last2Keystrokes, out int actionID))
            {
                last2Keystrokes = "";
                Hotkeys.DoHotkeyAction(actionID);
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
    }
}