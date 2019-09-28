using System.Collections.Generic;
using System.Collections;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using GlobalEnums;

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

            BenchMaker.GetPrefabs(preloaded);
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += BenchMaker.TryToDeploy;

            GameObject UIObj = new GameObject();
            UIObj.AddComponent<GUIController>();
            GameObject.DontDestroyOnLoad(UIObj);

            ModHooks.Instance.SetPlayerBoolHook += benchWatcher;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ClearSettings;
            ModHooks.Instance.ApplicationQuitHook += SaveGlobalSettings;

            GUIController.Instance.BuildMenus();
            
        }

        public override string GetVersion()
        {
            return "1.5";
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
            UIManager.instance.UIClosePauseMenu();

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
            HeroController.instance.SetMPCharge(0);
            HeroController.instance.AddMPCharge(100);


            //This allows the next pause to stop the game correctly
            TimeController.GenericTimeScale = 1f;

            // Restores audio to normal levels. Unfortunately, some warps pop atm when music changes over
            GameManager.instance.actorSnapshotUnpaused.TransitionTo(0f);
            GameManager.instance.ui.AudioGoToGameplay(.2f);
        }

        public void benchWatcher(string target, bool val)
        {
            if (target == "atBench" && val)
            {
                if (GameManager.instance.sceneName == "Town") Benchwarp.instance.Settings.hasVisitedDirtmouth = true;
                if (GameManager.instance.sceneName == "Room_nailmaster") Benchwarp.instance.Settings.hasVisitedMato = true;
                if (GameManager.instance.sceneName == "Crossroads_30") Benchwarp.instance.Settings.hasVisitedXRHotSprings = true;
                if (GameManager.instance.sceneName == "Crossroads_47") Benchwarp.instance.Settings.hasVisitedXRStag = true;
                if (GameManager.instance.sceneName == "Crossroads_04") Benchwarp.instance.Settings.hasVisitedSalubra = true;
                if (GameManager.instance.sceneName == "Crossroads_ShamanTemple") Benchwarp.instance.Settings.hasVisitedAncestralMound = true;
                if (GameManager.instance.sceneName == "Room_Final_Boss_Atrium") Benchwarp.instance.Settings.hasVisitedBlackEggTemple = true;
                if (GameManager.instance.sceneName == "Fungus1_01b") Benchwarp.instance.Settings.hasVisitedWaterfall = true;
                if (GameManager.instance.sceneName == "Fungus1_37") Benchwarp.instance.Settings.hasVisitedStoneSanctuary = true;
                if (GameManager.instance.sceneName == "Fungus1_31") Benchwarp.instance.Settings.hasVisitedGPToll = true;
                if (GameManager.instance.sceneName == "Fungus1_16_alt") Benchwarp.instance.Settings.hasVisitedGPStag = true;
                if (GameManager.instance.sceneName == "Room_Slug_Shrine") Benchwarp.instance.Settings.hasVisitedLakeofUnn = true;
                if (GameManager.instance.sceneName == "Fungus1_15") Benchwarp.instance.Settings.hasVisitedSheo = true;
                if (GameManager.instance.sceneName == "Fungus3_archive") Benchwarp.instance.Settings.hasVisitedTeachersArchives = true;
                if (GameManager.instance.sceneName == "Fungus2_02") Benchwarp.instance.Settings.hasVisitedQueensStation = true;
                if (GameManager.instance.sceneName == "Fungus2_26") Benchwarp.instance.Settings.hasVisitedLegEater = true;
                if (GameManager.instance.sceneName == "Fungus2_13") Benchwarp.instance.Settings.hasVisitedBretta = true;
                if (GameManager.instance.sceneName == "Fungus2_31") Benchwarp.instance.Settings.hasVisitedMantisVillage = true;
                if (GameManager.instance.sceneName == "Ruins1_02") Benchwarp.instance.Settings.hasVisitedQuirrel = true;
                if (GameManager.instance.sceneName == "Ruins1_31") Benchwarp.instance.Settings.hasVisitedCoTToll = true;
                if (GameManager.instance.sceneName == "Ruins1_29") Benchwarp.instance.Settings.hasVisitedCityStorerooms = true;
                if (GameManager.instance.sceneName == "Ruins1_18") Benchwarp.instance.Settings.hasVisitedWatchersSpire = true;
                if (GameManager.instance.sceneName == "Ruins2_08") Benchwarp.instance.Settings.hasVisitedKingsStation = true;
                if (GameManager.instance.sceneName == "Ruins_Bathhouse") Benchwarp.instance.Settings.hasVisitedPleasureHouse = true;
                if (GameManager.instance.sceneName == "Waterways_02") Benchwarp.instance.Settings.hasVisitedWaterways = true;
                if (GameManager.instance.sceneName == "GG_Atrium") Benchwarp.instance.Settings.hasVisitedGodhome = true;
                if (GameManager.instance.sceneName == "GG_Workshop") Benchwarp.instance.Settings.hasVisitedHallofGods = true;
                if (GameManager.instance.sceneName == "Deepnest_30") Benchwarp.instance.Settings.hasVisitedDNHotSprings = true;
                if (GameManager.instance.sceneName == "Deepnest_14") Benchwarp.instance.Settings.hasVisitedFailedTramway = true;
                if (GameManager.instance.sceneName == "Deepnest_Spider_Town") Benchwarp.instance.Settings.hasVisitedBeastsDen = true;
                if (GameManager.instance.sceneName == "Abyss_18") Benchwarp.instance.Settings.hasVisitedABToll = true;
                if (GameManager.instance.sceneName == "Abyss_22") Benchwarp.instance.Settings.hasVisitedABStag = true;
                if (GameManager.instance.sceneName == "Deepnest_East_06") Benchwarp.instance.Settings.hasVisitedOro = true;
                if (GameManager.instance.sceneName == "Deepnest_East_13") Benchwarp.instance.Settings.hasVisitedCamp = true;
                if (GameManager.instance.sceneName == "Room_Colosseum_02") Benchwarp.instance.Settings.hasVisitedColosseum = true;
                if (GameManager.instance.sceneName == "Hive_01") Benchwarp.instance.Settings.hasVisitedHive = true;
                if (GameManager.instance.sceneName == "Mines_29") Benchwarp.instance.Settings.hasVisitedDarkRoom = true;
                if (GameManager.instance.sceneName == "Mines_18") Benchwarp.instance.Settings.hasVisitedCrystalGuardian = true;
                if (GameManager.instance.sceneName == "Fungus1_24") Benchwarp.instance.Settings.hasVisitedQGCornifer = true;
                if (GameManager.instance.sceneName == "Fungus3_50") Benchwarp.instance.Settings.hasVisitedQGToll = true;
                if (GameManager.instance.sceneName == "Fungus3_40") Benchwarp.instance.Settings.hasVisitedQGStag = true;
                if (GameManager.instance.sceneName == "White_Palace_01") Benchwarp.instance.Settings.hasVisitedWPEntrance = true;
                if (GameManager.instance.sceneName == "White_Palace_03_hub") Benchwarp.instance.Settings.hasVisitedWPAtrium = true;
                if (GameManager.instance.sceneName == "White_Palace_06") Benchwarp.instance.Settings.hasVisitedWPBalcony = true;
                if (GameManager.instance.sceneName == "Room_Tram_RG") Benchwarp.instance.Settings.hasVisitedUpperTram = true;
                if (GameManager.instance.sceneName == "Room_Tram") Benchwarp.instance.Settings.hasVisitedLowerTram = true;
                if (GameManager.instance.sceneName == "RestingGrounds_09") Benchwarp.instance.Settings.hasVisitedRGStag = true;
                if (GameManager.instance.sceneName == "RestingGrounds_12") Benchwarp.instance.Settings.hasVisitedGreyMourner = true;
            }
            PlayerData.instance.SetBoolInternal(target, val);
        }

        public void ClearSettings(Scene arg0, Scene arg1)
        {
            if (arg1.name == "Menu_Title")
            {
                Benchwarp.instance.Settings.hasVisitedDirtmouth = false;
                Benchwarp.instance.Settings.hasVisitedMato = false;
                Benchwarp.instance.Settings.hasVisitedXRHotSprings = false;
                Benchwarp.instance.Settings.hasVisitedXRStag = false;
                Benchwarp.instance.Settings.hasVisitedSalubra = false;
                Benchwarp.instance.Settings.hasVisitedAncestralMound = false;
                Benchwarp.instance.Settings.hasVisitedBlackEggTemple = false;
                Benchwarp.instance.Settings.hasVisitedWaterfall = false;
                Benchwarp.instance.Settings.hasVisitedStoneSanctuary = false;
                Benchwarp.instance.Settings.hasVisitedGPToll = false;
                Benchwarp.instance.Settings.hasVisitedGPStag = false;
                Benchwarp.instance.Settings.hasVisitedLakeofUnn = false;
                Benchwarp.instance.Settings.hasVisitedSheo = false;
                Benchwarp.instance.Settings.hasVisitedTeachersArchives = false;
                Benchwarp.instance.Settings.hasVisitedQueensStation = false;
                Benchwarp.instance.Settings.hasVisitedLegEater = false;
                Benchwarp.instance.Settings.hasVisitedBretta = false;
                Benchwarp.instance.Settings.hasVisitedMantisVillage = false;
                Benchwarp.instance.Settings.hasVisitedQuirrel = false;
                Benchwarp.instance.Settings.hasVisitedCoTToll = false;
                Benchwarp.instance.Settings.hasVisitedCityStorerooms = false;
                Benchwarp.instance.Settings.hasVisitedWatchersSpire = false;
                Benchwarp.instance.Settings.hasVisitedKingsStation = false;
                Benchwarp.instance.Settings.hasVisitedPleasureHouse = false;
                Benchwarp.instance.Settings.hasVisitedWaterways = false;
                Benchwarp.instance.Settings.hasVisitedGodhome = false;
                Benchwarp.instance.Settings.hasVisitedHallofGods = false;
                Benchwarp.instance.Settings.hasVisitedDNHotSprings = false;
                Benchwarp.instance.Settings.hasVisitedFailedTramway = false;
                Benchwarp.instance.Settings.hasVisitedBeastsDen = false;
                Benchwarp.instance.Settings.hasVisitedABToll = false;
                Benchwarp.instance.Settings.hasVisitedABStag = false;
                Benchwarp.instance.Settings.hasVisitedOro = false;
                Benchwarp.instance.Settings.hasVisitedCamp = false;
                Benchwarp.instance.Settings.hasVisitedColosseum = false;
                Benchwarp.instance.Settings.hasVisitedHive = false;
                Benchwarp.instance.Settings.hasVisitedDarkRoom = false;
                Benchwarp.instance.Settings.hasVisitedCrystalGuardian = false;
                Benchwarp.instance.Settings.hasVisitedQGCornifer = false;
                Benchwarp.instance.Settings.hasVisitedQGToll = false;
                Benchwarp.instance.Settings.hasVisitedQGStag = false;
                Benchwarp.instance.Settings.hasVisitedWPEntrance = false;
                Benchwarp.instance.Settings.hasVisitedWPAtrium = false;
                Benchwarp.instance.Settings.hasVisitedWPBalcony = false;
                Benchwarp.instance.Settings.hasVisitedUpperTram = false;
                Benchwarp.instance.Settings.hasVisitedLowerTram = false;
                Benchwarp.instance.Settings.hasVisitedRGStag = false;
                Benchwarp.instance.Settings.hasVisitedGreyMourner = false;

                Benchwarp.instance.Settings.benchDeployed = false;
                Benchwarp.instance.Settings.benchName = null;
                Benchwarp.instance.Settings.benchScene = null;
                Benchwarp.instance.Settings.benchX = 0f;
                Benchwarp.instance.Settings.benchY = 0f;
                Benchwarp.instance.Settings.benchStyle = "Right";
            }
        }

    }
}
