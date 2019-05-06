using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Benchwarp
{
    public class GUIController : MonoBehaviour
    {
        public Font trajanBold;
        public Font trajanNormal;
        public Font arial;
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

            GameObject.DontDestroyOnLoad(canvas);
        }

        private void LoadResources()
        {
            foreach (Font f in Resources.FindObjectsOfTypeAll<Font>())
            {
                if (f != null && f.name == "TrajanPro-Bold")
                {
                    trajanBold = f;
                }

                if (f != null && f.name == "TrajanPro-Regular")
                {
                    trajanNormal = f;
                }

                //Just in case for some reason the computer doesn't have arial
                if (f != null && f.name == "Perpetua")
                {
                    arial = f;
                }

                foreach (string font in Font.GetOSInstalledFontNames())
                {
                    if (font.ToLower().Contains("arial"))
                    {
                        arial = Font.CreateDynamicFontFromOSFont(font, 13);
                        break;
                    }
                }
            }

            if (trajanBold == null || trajanNormal == null || arial == null) Benchwarp.instance.LogError("Could not find game fonts");

            string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            foreach (string res in resourceNames)
            {
                if (res.StartsWith("Benchwarp.Images."))
                {
                    try
                    {
                        Stream imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(res);
                        byte[] buffer = new byte[imageStream.Length];
                        imageStream.Read(buffer, 0, buffer.Length);

                        Texture2D tex = new Texture2D(1, 1);
                        tex.LoadImage(buffer.ToArray());

                        string[] split = res.Split('.');
                        string internalName = split[split.Length - 2];
                        images.Add(internalName, tex);

                        Benchwarp.instance.Log("Loaded image: " + internalName);
                    }
                    catch (Exception e)
                    {
                        Benchwarp.instance.LogError("Failed to load image: " + res + "\n" + e.ToString());
                    }
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
                if (_instance == null)
                {
                    _instance = UnityEngine.Object.FindObjectOfType<GUIController>();
                    if (_instance == null)
                    {
                        Benchwarp.instance.LogWarn("Couldn't find GUIController");

                        GameObject GUIObj = new GameObject();
                        _instance = GUIObj.AddComponent<GUIController>();
                        GameObject.DontDestroyOnLoad(GUIObj);
                    }
                }
                return _instance;
            }
            set
            {
            }
        }
    }
}