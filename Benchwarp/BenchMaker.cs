using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Benchwarp
{
    public static class BenchMaker
    {
        public static GameObject DeployedBench;
        public const string DEPLOYED_BENCH_RESPAWN_MARKER_NAME = "DeployedBench";

        public static void MakeDeployedBench()
        {
            if (!Benchwarp.LS.benchDeployed) return;

            if (!ObjectCache.DidPreload)
            {
                GameObject marker = new();
                marker.transform.position = new Vector3(Benchwarp.LS.benchX, Benchwarp.LS.benchY, 7.4f);
                marker.tag = "RespawnPoint";
                marker.name = DEPLOYED_BENCH_RESPAWN_MARKER_NAME;
                marker.SetActive(true);
                return;
            }

            DeployedBench = ObjectCache.GetNewBench();
            DeployedBench.name = DEPLOYED_BENCH_RESPAWN_MARKER_NAME;
            DeployedBench.SetActive(true);
            UpdateInteractive(DeployedBench, Benchwarp.GS.Noninteractive);
            RemoveSaveRespawnActions(DeployedBench);
            BenchStyle.GetStyle(Benchwarp.GS.nearStyle).ApplyFsmAndPositionChanges(DeployedBench, new Vector3(Benchwarp.LS.benchX, Benchwarp.LS.benchY, 0.02f));
            ApplyStyleSprites(DeployedBench, Benchwarp.GS.farStyle, Benchwarp.GS.nearStyle);
        }

        public static void DestroyBench(bool DontDeleteData = false)
        {
            if (DeployedBench != null) UObject.Destroy(DeployedBench);
            if (DontDeleteData) return;
            Benchwarp.LS.benchDeployed = false;
            Benchwarp.LS.atDeployedBench = false;
        }

        public static void UpdateInteractive(GameObject bench, bool noninteractive)
        {
            FsmState idle = bench.LocateMyFSM("Bench Control").FsmStates.First(s => s.Name == "Idle");
            if (noninteractive)
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

        public static void RemoveSaveRespawnActions(GameObject bench)
        {
            FsmState restBurst = bench.LocateMyFSM("Bench Control").FsmStates.First(s => s.Name == "Rest Burst");
            if (restBurst.Actions.Length < 27) return;

            List<FsmStateAction> actions = restBurst.Actions.ToList();
            actions.RemoveRange(19, 8);
            actions.Add(new FsmLambda(() => Benchwarp.LS.atDeployedBench = true));
            restBurst.Actions = actions.ToArray();
        }

        public static void ApplyStyleSprites(GameObject bench, string farStyle, string nearStyle)
        {
            if (!ObjectCache.DidPreload) return;
            if (DeployedBench == null) return;

            try
            {
                if (BenchStyle.IsValidStyle(farStyle) && BenchStyle.GetStyle(farStyle) is BenchStyle fs)
                {
                    Sprite benchSprite = SpriteManager.GetSprite(fs.spriteName);
                    if (benchSprite != null)
                    {
                        bench.GetComponent<SpriteRenderer>().sprite = benchSprite;
                    }
                }
                if (BenchStyle.IsValidStyle(nearStyle) && BenchStyle.GetStyle(nearStyle) is BenchStyle ns)
                {
                    string spriteName = ns.distinctLitSprite ? $"{ns.spriteName}_lit" : ns.spriteName;
                    Sprite litSprite = SpriteManager.GetSprite(spriteName);
                    if (litSprite != null)
                    {
                        bench.transform.Find("Lit").gameObject.GetComponent<SpriteRenderer>().sprite = litSprite;
                    }
                }
            }
            catch (Exception e)
            {
                Benchwarp.instance.LogError(e);
            }
        }

        public static bool IsUnsafeRoom()
        {
            return IsDreamRoom() || GameManager.instance.sceneName switch
            {
                "Room_Colosseum_Bronze" or "Room_Colosseum_Silver" or "Room_Colosseum_Gold" => true,
                _ => false
            };
        }

        public static bool IsDreamRoom()
        {
            return GameManager.instance.sm.mapZone switch
            {
                GlobalEnums.MapZone.DREAM_WORLD
                or GlobalEnums.MapZone.GODS_GLORY
                or GlobalEnums.MapZone.GODSEEKER_WASTE
                or GlobalEnums.MapZone.WHITE_PALACE => true,
                _ => false,
            };
        }

        internal static void UpdateStyleFromMenu()
        {
            BenchStyle.GetStyle(Benchwarp.GS.nearStyle)
                .ApplyFsmAndPositionChanges(DeployedBench, new Vector3(Benchwarp.LS.benchX, Benchwarp.LS.benchY, 0.02f));
            ApplyStyleSprites(DeployedBench, Benchwarp.GS.farStyle, Benchwarp.GS.nearStyle);
        }

        internal static void TryToDeploy(Scene arg0, Scene arg1)
        {
            if (Benchwarp.LS.benchDeployed && arg1.name == Benchwarp.LS.benchScene)
            {
                MakeDeployedBench();
            }
        }
    }
}
