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
    }
}