using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Benchwarp
{
    /// <summary>
    /// Lazy loading cache for all style sprites. Button sprites are handled by GUIController.
    /// </summary>
    public static class SpriteManager
    {
        private const string _prefix = "Benchwarp.Resources.Styles.";
        private static readonly Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        private static readonly string[] resourceNames = typeof(SpriteManager).Assembly.GetManifestResourceNames();

        public static HashSet<string> GetValidStyles(IEnumerable<string> styles)
        {
            HashSet<string> h = new HashSet<string>();
            foreach (string s in styles)
            {
                var bs = BenchStyle.GetStyle(s);
                bool normal = false;
                bool lit = false;
                string prefix = _prefix + bs.spriteName;

                for (int i = 0; i < resourceNames.Length; i++)
                {
                    if (resourceNames[i].StartsWith(prefix))
                    {
                        string tail = resourceNames[i].Substring(prefix.Length);
                        if (tail.ToLower() == ".png") normal = true;
                        else if (tail.ToLower() == "_lit.png") lit = true;
                    }
                }
                if (normal && (!bs.distinctLitSprite || lit)) h.Add(s);
            }

            return h;
        }

        private static Sprite LoadAndCache(string path)
        {
            string name = path.Substring(_prefix.Length); // Benchwarp.Images.
            name.Remove(name.Length - 4); // .png
            Sprite sprite = FromStream(typeof(SpriteManager).Assembly.GetManifestResourceStream(path));
            _sprites[name] = sprite;
            return sprite;
        }

        public static Sprite GetSprite(string name)
        {
            if (_sprites.TryGetValue(name, out Sprite sprite)) return sprite;

            string path = _prefix + name;
            path = resourceNames.FirstOrDefault(n => n.StartsWith(path));
            if (string.IsNullOrEmpty(path))
            {
                Benchwarp.instance.LogError($"{name} does not correspond to any embedded resource under {_prefix}");
                return null;
            }

            return LoadAndCache(path);
        }

        private static Sprite FromStream(Stream s)
        {
            Texture2D tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            byte[] buffer = ToArray(s);
            tex.LoadImage(buffer, markNonReadable: true);
            tex.filterMode = FilterMode.Trilinear;
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 64);
        }

        private static byte[] ToArray(Stream s)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                s.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
