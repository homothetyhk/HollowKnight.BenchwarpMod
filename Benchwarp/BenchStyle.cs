using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Benchwarp
{
    public class BenchStyle
    {
        public static Dictionary<string, BenchStyle> _styles;
        public static BenchStyle GetStyle(string style)
        {
            return _styles.TryGetValue(style, out var bs) ? bs : null;
        }
        public static IEnumerable<string> StyleNames => _styles.Keys;

        public BenchStyle() { }
        public string style;
        public Vector3 offset;
        public bool distinctLitSprite;

        public Vector3 localScale;
        public Vector3 litOffset;
        public string spriteName;

        // fsm parameters
        public bool tilter;
        public float tiltAmount;
        public Vector3 adjustVector;

        public void ApplyFsmAndPositionChanges(GameObject bench, Vector3 position)
        {
            bench.transform.position = position + offset;
            bench.transform.localScale = new Vector3(localScale.x, localScale.y, 1f);
            bench.transform.Find("Lit").localPosition = litOffset;

            if (tilter)
            {
                bench.transform.SetRotation2D(tiltAmount);
            }
            else
            {
                bench.transform.SetRotation2D(0);
            }

            PlayMakerFSM fsm = bench.LocateMyFSM("Bench Control");
            var fv = fsm.FsmVariables;
            fv.FindFsmBool("Tilter").Value = tilter;
            fv.FindFsmFloat("Tilt Amount").Value = tiltAmount;
            fv.FindFsmVector3("Adjust Vector").Value = adjustVector;
        }

        static BenchStyle()
        {
            _styles = JsonUtil.Deserialize<Dictionary<string, BenchStyle>>("Benchwarp.Resources.styles.json");
        }
    }
}
