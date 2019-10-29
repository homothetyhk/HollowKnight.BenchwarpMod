using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalEnums;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using TMPro;

namespace Benchwarp
{
    public class Benchwarp : Mod<SaveSettings, GlobalSettings>, ITogglableMod
    {

        internal static Benchwarp instance;

        internal GameObject UIObj;

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloaded)
        {
            instance = this;

            instance.Log("Initializing");

            Bench.GenerateBenchData();
            BenchMaker.GetPrefabs(preloaded);
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += BenchMaker.TryToDeploy;

            UIObj = new GameObject();
            UIObj.AddComponent<GUIController>();
            GameObject.DontDestroyOnLoad(UIObj);

            ModHooks.Instance.SetPlayerBoolHook += BenchWatcher;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ClearSettings;
            ModHooks.Instance.ApplicationQuitHook += SaveGlobalSettings;

            ModHooks.Instance.GetPlayerStringHook += RescueSave;

            ModHooks.Instance.GetPlayerStringHook += RespawnAtDeployedBench;
            ModHooks.Instance.GetPlayerIntHook += RespawnAtDeployedBench2;
            ModHooks.Instance.SetPlayerStringHook += RemoveRespawnFromDeployedBench;

            GUIController.Instance.BuildMenus();
            
            if (Benchwarp.instance.Settings.benchDeployed && GameManager.instance.sceneName == Benchwarp.instance.Settings.benchScene)
            {
                BenchMaker.MakeBench();
            }

        }

        public override string GetVersion()
        {
            return "2.0";
        }

        public override List<(string, string)> GetPreloadNames()
        {

            return new List<(string, string)>
                {
                ("Crossroads_30", "RestBench"),
                ("Town", "RestBench"),
                ("Crossroads_04", "RestBench"),
                ("Crossroads_ShamanTemple", "BoneBench"),
                ("Fungus1_37", "RestBench"),
                ("Room_Slug_Shrine", "RestBench"),
                ("Fungus3_archive", "RestBench"),
                ("Fungus2_26", "RestBench"),
                ("Fungus2_31", "RestBench"),
                ("Ruins_Bathhouse", "RestBench"),
                ("Waterways_02", "RestBench"),
                ("GG_Atrium", "RestBench"),
                ("Deepnest_Spider_Town", "RestBench Return"),
                ("Deepnest_East_13", "RestBench"),
                ("Deepnest_East_13", "outskirts__0003_camp"),
                ("Room_Colosseum_02", "RestBench"),
                ("Fungus1_24", "RestBench"),
                ("Fungus1_24", "guardian_bench"),
                ("Room_Tram", "RestBench"),
                ("Room_nailmaster", "RestBench"),
                ("Deepnest_East_06", "RestBench"),
                ("Fungus1_15", "RestBench"),
                ("White_Palace_01", "WhiteBench"),
                ("Room_Final_Boss_Atrium", "RestBench")
                };
        }

        public IEnumerator Respawn()
        {
            GameManager.instance.SaveGame();
            GameManager.instance.ResetSemiPersistentItems();
            UIManager.instance.UIClosePauseMenu();
            HeroController.instance.SetMPCharge(0);
            PlayMakerFSM.BroadcastEvent("MP DRAIN");
            // Set some stuff which would normally be set by LoadSave
            HeroController.instance.AffectedByGravity(false);
            HeroController.instance.transitionState = HeroTransitionState.EXITING_SCENE;
            GameManager.instance.cameraCtrl.FadeOut(CameraFadeType.LEVEL_TRANSITION);

            yield return new WaitForSecondsRealtime(0.5f);

            // Actually respawn the character
            GameManager.instance.SetPlayerDataBool(nameof(PlayerData.atBench), false);
            GameManager.instance.ReadyForRespawn(false);

            yield return new WaitWhile(() => GameManager.instance.IsInSceneTransition);

            // Revert pause menu timescale
            Time.timeScale = 1f;
            GameManager.instance.FadeSceneIn();

            // We have to set the game non-paused because TogglePauseMenu sucks and UIClosePauseMenu doesn't do it for us.
            GameManager.instance.isPaused = false;

            // Restore various things normally handled by exiting the pause menu. None of these are necessary afaik
            GameCameras.instance.ResumeCameraShake();
            if (HeroController.instance != null)
            {
                HeroController.instance.UnPause();
            }
            MenuButtonList.ClearAllLastSelected();

            //This allows the next pause to stop the game correctly
            TimeController.GenericTimeScale = 1f;

            // Restores audio to normal levels. Unfortunately, some warps pop atm when music changes over
            GameManager.instance.actorSnapshotUnpaused.TransitionTo(0f);
            GameManager.instance.ui.AudioGoToGameplay(.2f);

            // Below this is Sean stuff -- I take no responsiblity

            // Break the flower poggers
            if (!PlayerData.instance.GetBool(nameof(PlayerData.hasXunFlower)) || PlayerData.instance.xunFlowerBroken)
            {
                yield break;
            }

            PlayerData.instance.SetBool(nameof(PlayerData.xunFlowerBroken), true);
            PlayerData.instance.IncrementInt(nameof(PlayerData.xunFlowerBrokeTimes));

            // No fsm extensions = unreadable code, unfortunately
            // Find "Flower?" state on Knight - ProxyFSM
            FsmState flower = HeroController.instance.proxyFSM.FsmStates.First(state => state.Name == "Flower?");

            // Activate flower broken effect
            HeroController.instance.proxyFSM.Fsm
                .GetOwnerDefaultTarget(flower.Actions.OfType<ActivateGameObject>().First().gameObject).SetActive(true);

            // Find message prefab, instantiate
            GameObject msg =
                Object.Instantiate(flower.Actions.OfType<SpawnObjectFromGlobalPool>().First().gameObject.Value);
            GameObject msgText = msg.transform.Find("Text").gameObject;
            GameObject msgIcon = msg.transform.Find("Icon").gameObject;

            // Set icon/text to be flower broken
            msgText.GetComponent<TextMeshPro>().text = Language.Language.Get("NOTIFICATION_FLOWER_BREAK", "UI");
            msgIcon.GetComponent<SpriteRenderer>().sprite =
                (Sprite) flower.Actions.OfType<SetSpriteRendererSprite>().First().sprite.Value;
        }

        public void BenchWatcher(string target, bool val)
        {
            if (target == "atBench" && val)
            {
                foreach (Bench bench in Bench.Benches)
                {
                    if (bench.benched) bench.visited = true;
                    else if (GameManager.instance.sceneName == bench.sceneName && Benchwarp.instance.Settings.benchScene != bench.sceneName) bench.visited = true;
                    else continue;
                    break;
                }
            }
            PlayerData.instance.SetBoolInternal(target, val);
        }


        private string RescueSave(string stringName)
        {
            if (!Benchwarp.instance.GlobalSettings.CheckForBrokenSaveFile) return PlayerData.instance.GetStringInternal(stringName);

            if (stringName == "respawnScene" && !Benchwarp.instance.Settings.atDeployedBench)
            {
                foreach (Bench bench in Bench.Benches)
                {
                    if (bench.benched) return PlayerData.instance.GetStringInternal(stringName);
                }
                if (PlayerData.instance.respawnType == 1)
                {
                    LogError("Attempted to load into unrecognized bench. Relocating to Dirtmouth.\n" +
                        "If you would like to disable this check in the future, please visit the Benchwarp GlobalSettings " +
                        "and change key \"CheckForBrokenSaveFile\" to false.");

                    PlayerData.instance.respawnScene = "Town";
                    PlayerData.instance.respawnMarkerName = "RestBench";
                    PlayerData.instance.mapZone = MapZone.TOWN;
                    GameManager.instance.SaveGame();
                    return "Town";
                }
            }
            return PlayerData.instance.GetStringInternal(stringName);
        }

        private void RemoveRespawnFromDeployedBench(string stringName, string value)
        {
            switch (stringName)
            {
                case nameof(PlayerData.respawnMarkerName):
                    if (value != BenchMaker.DEPLOYED_BENCH_RESPAWN_MARKER_NAME)
                    {
                        Benchwarp.instance.Settings.atDeployedBench = false;
                    }
                    break;
                case nameof(PlayerData.respawnScene):
                    if (value != Benchwarp.instance.Settings.benchScene)
                    {
                        Benchwarp.instance.Settings.atDeployedBench = false;
                    }
                    break;
            }
            PlayerData.instance.SetStringInternal(stringName, value);
        }

        private string RespawnAtDeployedBench(string stringName)
        {
            if (!Benchwarp.instance.Settings.atDeployedBench) return PlayerData.instance.GetStringInternal(stringName);
            switch (stringName)
            {
                case nameof(PlayerData.respawnMarkerName):
                    return BenchMaker.DEPLOYED_BENCH_RESPAWN_MARKER_NAME;
                case nameof(PlayerData.respawnScene):
                    return Benchwarp.instance.Settings.benchScene;
                default:
                    return PlayerData.instance.GetStringInternal(stringName);
            }
        }

        private int RespawnAtDeployedBench2(string intName)
        {
            if (!Benchwarp.instance.Settings.atDeployedBench || intName != nameof(PlayerData.respawnType))
            {
                return PlayerData.instance.GetIntInternal(intName);
            }
            else return 1;
        }

        public void ClearSettings(Scene arg0, Scene arg1)
        {
            if (arg1.name == "Menu_Title")
            {
                foreach (Bench bench in Bench.Benches)
                {
                    bench.visited = false;
                }

                Benchwarp.instance.Settings.benchDeployed = false;
                Benchwarp.instance.Settings.atDeployedBench = false;
                Benchwarp.instance.Settings.benchScene = null;
                Benchwarp.instance.Settings.benchX = 0f;
                Benchwarp.instance.Settings.benchY = 0f;
            }
        }

        public void Unload()
        {
            ModHooks.Instance.SetPlayerBoolHook -= BenchWatcher;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= ClearSettings;
            ModHooks.Instance.ApplicationQuitHook -= SaveGlobalSettings;

            ModHooks.Instance.GetPlayerStringHook -= RescueSave;

            ModHooks.Instance.GetPlayerStringHook -= RespawnAtDeployedBench;
            ModHooks.Instance.GetPlayerIntHook -= RespawnAtDeployedBench2;
            ModHooks.Instance.SetPlayerStringHook -= RemoveRespawnFromDeployedBench;

            BenchMaker.DestroyBench(DontDeleteData: true);
            Object.Destroy(TopMenu.canvas);
            Object.Destroy(UIObj);
        }
    }
}
