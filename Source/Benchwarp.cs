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
    public class Benchwarp : Mod<SaveSettings, GlobalSettings>
    {

        internal static Benchwarp instance;
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloaded)
        {
            if (instance != null) return;

            instance = this;

            instance.Log("Initializing");

            Bench.GenerateBenchData();
            BenchMaker.GetPrefabs(preloaded);
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += BenchMaker.TryToDeploy;

            GameObject UIObj = new GameObject();
            UIObj.AddComponent<GUIController>();
            GameObject.DontDestroyOnLoad(UIObj);

            ModHooks.Instance.SetPlayerBoolHook += BenchWatcher;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ClearSettings;
            ModHooks.Instance.ApplicationQuitHook += SaveGlobalSettings;

            GUIController.Instance.BuildMenus();
            
        }

        public override string GetVersion()
        {
            return "1.8";
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
            HeroController.instance.SetMPCharge(1);
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

            // Sloppy way to force the soul meter to update
            HeroController.instance.SetMPCharge(1);


            //This allows the next pause to stop the game correctly
            TimeController.GenericTimeScale = 1f;

            // Restores audio to normal levels. Unfortunately, some warps pop atm when music changes over
            GameManager.instance.actorSnapshotUnpaused.TransitionTo(0f);
            GameManager.instance.ui.AudioGoToGameplay(.2f);

            // Break the flower poggers
            if (!PlayerData.instance.GetBool(nameof(PlayerData.hasXunFlower)))
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

        public void ClearSettings(Scene arg0, Scene arg1)
        {
            if (arg1.name == "Menu_Title")
            {
                foreach (Bench bench in Bench.Benches)
                {
                    bench.visited = false;
                }

                Benchwarp.instance.Settings.benchDeployed = false;
                Benchwarp.instance.Settings.benchName = null;
                Benchwarp.instance.Settings.benchScene = null;
                Benchwarp.instance.Settings.benchX = 0f;
                Benchwarp.instance.Settings.benchY = 0f;
            }
        }

    }
}
