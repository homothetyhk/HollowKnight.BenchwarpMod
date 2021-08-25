using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using HutongGames.PlayMaker;

namespace Benchwarp
{
    public static class BenchMaker
    {
        public static GameObject DeployedBench;
        public const string DEPLOYED_BENCH_RESPAWN_MARKER_NAME = "DeployedBench";


        public static void MakeBench()
        {
            if (!Benchwarp.LS.benchDeployed) return;

            if (!ObjectCache.DidPreload)
            {
                GameObject marker = new GameObject();
                marker.transform.position = new Vector3(Benchwarp.LS.benchX, Benchwarp.LS.benchY, 7.4f);
                marker.tag = "RespawnPoint";
                marker.name = DEPLOYED_BENCH_RESPAWN_MARKER_NAME;
                marker.SetActive(true);
                return;
            }

            DeployedBench = ObjectCache.GetNewBench();
            DeployedBench.name = DEPLOYED_BENCH_RESPAWN_MARKER_NAME;
            DeployedBench.SetActive(true); 
            UpdateInteractive();
            RemoveSaveRespawnActions();
            BenchStyle.GetStyle(Benchwarp.GS.nearStyle).ApplyFsmAndPositionChanges(DeployedBench, new Vector3(Benchwarp.LS.benchX, Benchwarp.LS.benchY, 0.02f));
            ApplyStyleSprites();
        }

        public static void DestroyBench(bool DontDeleteData = false)
        {
            if (DeployedBench != null) GameObject.Destroy(DeployedBench);
            if (DontDeleteData) return;
            Benchwarp.LS.benchDeployed = false;
            Benchwarp.LS.atDeployedBench = false;
        }

        public static void UpdateInteractive()
        {
            if (DeployedBench == null || !ObjectCache.DidPreload) return;

            FsmState idle = DeployedBench.LocateMyFSM("Bench Control").FsmStates.First(s => s.Name == "Idle");
            if (Benchwarp.GS.Noninteractive)
            {
                idle.Transitions = Array.Empty<FsmTransition>();
            }
            else
            {
                if (idle.Transitions.Any(t => t.EventName == "IN RANGE")) return;
                idle.Transitions = new FsmTransition[]
                {
                    new FsmTransition
                    {
                        FsmEvent = FsmEvent.GetFsmEvent("IN RANGE"),
                        ToFsmState = idle.Fsm.States.FirstOrDefault(f => f.Name == "In Range"),
                        ToState = "In Range",
                    }
                };
            }
        }

        public static void RemoveSaveRespawnActions()
        {
            if (DeployedBench == null || !ObjectCache.DidPreload) return;

            FsmState restBurst = DeployedBench.LocateMyFSM("Bench Control").FsmStates.First(s => s.Name == "Rest Burst");
            if (restBurst.Actions.Length < 27) return;

            List<FsmStateAction> actions = restBurst.Actions.ToList();
            actions.RemoveRange(19, 8);
            actions.Add(new FsmLambda(() => Benchwarp.LS.atDeployedBench = true));
            restBurst.Actions = actions.ToArray();
        }

        public static void UpdateStyleFromMenu()
        {
            BenchStyle.GetStyle(Benchwarp.GS.nearStyle)
                .ApplyFsmAndPositionChanges(DeployedBench, new Vector3(Benchwarp.LS.benchX, Benchwarp.LS.benchY, 0.02f));
            ApplyStyleSprites();
        }

        public static void ApplyStyleSprites()
        {
            if (!ObjectCache.DidPreload) return;
            if (DeployedBench == null) return;

            try
            {
                string farStyle = Benchwarp.GS.farStyle;
                string nearStyle = Benchwarp.GS.nearStyle;

                if (BenchStyle.IsValidStyle(farStyle) && BenchStyle.GetStyle(farStyle) is BenchStyle fs)
                {
                    Sprite benchSprite = SpriteManager.GetSprite(fs.spriteName);
                    if (benchSprite != null)
                    {
                        DeployedBench.GetComponent<SpriteRenderer>().sprite = benchSprite;
                    }
                }
                if (BenchStyle.IsValidStyle(nearStyle) && BenchStyle.GetStyle(nearStyle) is BenchStyle ns)
                {
                    string spriteName = ns.distinctLitSprite ? $"{ns.spriteName}_lit" : ns.spriteName;
                    Sprite litSprite = SpriteManager.GetSprite(spriteName);
                    if (litSprite != null)
                    {
                        DeployedBench.transform.Find("Lit").gameObject.GetComponent<SpriteRenderer>().sprite = litSprite;
                    }
                }
            }
            catch (Exception e)
            {
                Benchwarp.instance.LogError(e);
            }
        }

        public static void TryToDeploy(Scene arg0, Scene arg1)
        {
            if (Benchwarp.LS.benchDeployed && arg1.name == Benchwarp.LS.benchScene)
            {
                MakeBench();
            }
        }

        public static bool IsDarkOrDreamRoom()
        {
            return (!PlayerData.instance.hasLantern && GameManager.instance.sm.darknessLevel == 2)
                || GameManager.instance.sm.mapZone == GlobalEnums.MapZone.DREAM_WORLD
                || GameManager.instance.sm.mapZone == GlobalEnums.MapZone.GODS_GLORY
                || GameManager.instance.sm.mapZone == GlobalEnums.MapZone.GODSEEKER_WASTE
                || GameManager.instance.sm.mapZone == GlobalEnums.MapZone.WHITE_PALACE;
        }
    }
}
