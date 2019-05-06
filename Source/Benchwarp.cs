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
        public override void Initialize()
        {
            instance = this;

            instance.Log("Initializing");

            if (!Benchwarp.instance.GlobalSettings.UnlockAllBenches) Benchwarp.instance.GlobalSettings.UnlockAllBenches = false;

            GameObject UIObj = new GameObject();
            UIObj.AddComponent<GUIController>();
            GameObject.DontDestroyOnLoad(UIObj);

            ModHooks.Instance.ApplicationQuitHook += SaveGlobalSettings;
            ModHooks.Instance.SetPlayerBoolHook += benchWatcher;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ClearSettings;
            if (Benchwarp.instance.GlobalSettings.UnlockAllBenches) ModHooks.Instance.SavegameLoadHook += UnlockAllBenches;

            GUIController.Instance.BuildMenus();
        }

        public override string GetVersion()
        {
            return "1.0";
        }

        public IEnumerator Respawn()
        {
            UIManager.instance.UIClosePauseMenu();

            // Set some stuff which would normally be set by LoadSave
            HeroController.instance.AffectedByGravity(false);
            HeroController.instance.transitionState = HeroTransitionState.EXITING_SCENE;
            GameManager.instance.cameraCtrl.FadeOut(CameraFadeType.LEVEL_TRANSITION);

            yield return new WaitForSecondsRealtime(0.5f);

            // Actually respawn the character
            GameManager.instance.ReadyForRespawn(false);

            yield return new WaitWhile(() => GameManager.instance.IsInSceneTransition);

            // Revert pause menu timescale
            Time.timeScale = 1f;
            GameManager.instance.FadeSceneIn();

            // We have to set the game non-paused because TogglePauseMenu sucks and UIClosePauseMenu doesn't do it for us.
            GameManager.instance.isPaused = false;
        }

        public void benchWatcher(string target, bool val)
        {
            if (target == "atBench" && val)
            {
                if (PlayerData.instance.respawnScene == "Town") Benchwarp.instance.Settings.hasVisitedDirtmouth = true;
                if (PlayerData.instance.respawnScene == "Room_nailmaster") Benchwarp.instance.Settings.hasVisitedMato = true;
                if (PlayerData.instance.respawnScene == "Crossroads_30") Benchwarp.instance.Settings.hasVisitedXRHotSprings = true;
                if (PlayerData.instance.respawnScene == "Crossroads_47") Benchwarp.instance.Settings.hasVisitedXRStag = true;
                if (PlayerData.instance.respawnScene == "Crossroads_04") Benchwarp.instance.Settings.hasVisitedSalubra = true;
                if (PlayerData.instance.respawnScene == "Crossroads_ShamanTemple") Benchwarp.instance.Settings.hasVisitedAncestralMound = true;
                if (PlayerData.instance.respawnScene == "Room_Final_Boss_Atrium") Benchwarp.instance.Settings.hasVisitedBlackEggTemple = true;
                if (PlayerData.instance.respawnScene == "Fungus1_01b") Benchwarp.instance.Settings.hasVisitedWaterfall = true;
                if (PlayerData.instance.respawnScene == "Fungus1_37") Benchwarp.instance.Settings.hasVisitedStoneSanctuary = true;
                if (PlayerData.instance.respawnScene == "Fungus1_31") Benchwarp.instance.Settings.hasVisitedGPToll = true;
                if (PlayerData.instance.respawnScene == "Fungus1_16_alt") Benchwarp.instance.Settings.hasVisitedGPStag = true;
                if (PlayerData.instance.respawnScene == "Room_Slug_Shrine") Benchwarp.instance.Settings.hasVisitedLakeofUnn = true;
                if (PlayerData.instance.respawnScene == "Fungus1_15") Benchwarp.instance.Settings.hasVisitedSheo = true;
                if (PlayerData.instance.respawnScene == "Fungus3_archive") Benchwarp.instance.Settings.hasVisitedTeachersArchives = true;
                if (PlayerData.instance.respawnScene == "Fungus2_02") Benchwarp.instance.Settings.hasVisitedQueensStation = true;
                if (PlayerData.instance.respawnScene == "Fungus2_26") Benchwarp.instance.Settings.hasVisitedLegEater = true;
                if (PlayerData.instance.respawnScene == "Fungus2_13") Benchwarp.instance.Settings.hasVisitedBretta = true;
                if (PlayerData.instance.respawnScene == "Fungus2_31") Benchwarp.instance.Settings.hasVisitedMantisVillage = true;
                if (PlayerData.instance.respawnScene == "Ruins1_02") Benchwarp.instance.Settings.hasVisitedQuirrel = true;
                if (PlayerData.instance.respawnScene == "Ruins1_31") Benchwarp.instance.Settings.hasVisitedCoTToll = true;
                if (PlayerData.instance.respawnScene == "Ruins1_29") Benchwarp.instance.Settings.hasVisitedCityStorerooms = true;
                if (PlayerData.instance.respawnScene == "Ruins1_18") Benchwarp.instance.Settings.hasVisitedWatchersSpire = true;
                if (PlayerData.instance.respawnScene == "Ruins2_08") Benchwarp.instance.Settings.hasVisitedKingsStation = true;
                if (PlayerData.instance.respawnScene == "Ruins_Bathhouse") Benchwarp.instance.Settings.hasVisitedPleasureHouse = true;
                if (PlayerData.instance.respawnScene == "Waterways_02") Benchwarp.instance.Settings.hasVisitedWaterways = true;
                if (PlayerData.instance.respawnScene == "GG_Atrium") Benchwarp.instance.Settings.hasVisitedGodhome = true;
                if (PlayerData.instance.respawnScene == "GG_Workshop") Benchwarp.instance.Settings.hasVisitedHallofGods = true;
                if (PlayerData.instance.respawnScene == "Deepnest_30") Benchwarp.instance.Settings.hasVisitedDNHotSprings = true;
                if (PlayerData.instance.respawnScene == "Deepnest_14") Benchwarp.instance.Settings.hasVisitedFailedTramway = true;
                if (PlayerData.instance.respawnScene == "Deepnest_Spider_Town") Benchwarp.instance.Settings.hasVisitedBeastsDen = true;
                if (PlayerData.instance.respawnScene == "Abyss_18") Benchwarp.instance.Settings.hasVisitedABToll = true;
                if (PlayerData.instance.respawnScene == "Abyss_22") Benchwarp.instance.Settings.hasVisitedABStag = true;
                if (PlayerData.instance.respawnScene == "Deepnest_East_06") Benchwarp.instance.Settings.hasVisitedOro = true;
                if (PlayerData.instance.respawnScene == "Deepnest_East_13") Benchwarp.instance.Settings.hasVisitedCamp = true;
                if (PlayerData.instance.respawnScene == "Room_Colosseum_02") Benchwarp.instance.Settings.hasVisitedColosseum = true;
                if (PlayerData.instance.respawnScene == "Hive_01") Benchwarp.instance.Settings.hasVisitedHive = true;
                if (PlayerData.instance.respawnScene == "Mines_29") Benchwarp.instance.Settings.hasVisitedDarkRoom = true;
                if (PlayerData.instance.respawnScene == "Mines_18") Benchwarp.instance.Settings.hasVisitedCrystalGuardian = true;
                if (PlayerData.instance.respawnScene == "Fungus1_24") Benchwarp.instance.Settings.hasVisitedQGCornifer = true;
                if (PlayerData.instance.respawnScene == "Fungus3_50") Benchwarp.instance.Settings.hasVisitedQGToll = true;
                if (PlayerData.instance.respawnScene == "Fungus3_40") Benchwarp.instance.Settings.hasVisitedQGStag = true;
                if (PlayerData.instance.respawnScene == "White_Palace_01") Benchwarp.instance.Settings.hasVisitedWPEntrance = true;
                if (PlayerData.instance.respawnScene == "White_Palace_03_hub") Benchwarp.instance.Settings.hasVisitedWPAtrium = true;
                if (PlayerData.instance.respawnScene == "White_Palace_06") Benchwarp.instance.Settings.hasVisitedWPBalcony = true;
                if (PlayerData.instance.respawnScene == "Room_Tram_RG") Benchwarp.instance.Settings.hasVisitedUpperTram = true;
                if (PlayerData.instance.respawnScene == "Room_Tram") Benchwarp.instance.Settings.hasVisitedLowerTram = true;
                if (PlayerData.instance.respawnScene == "RestingGrounds_09") Benchwarp.instance.Settings.hasVisitedRGStag = true;
                if (PlayerData.instance.respawnScene == "RestingGrounds_12") Benchwarp.instance.Settings.hasVisitedGreyMourner = true;
            }
            PlayerData.instance.SetBoolInternal(target, val);
        }
        public void UnlockAllBenches(int id)
        {
            if (!Benchwarp.instance.Settings.UnlockAllBenchesFinished)
            {
                Benchwarp.instance.Settings.hasVisitedDirtmouth = true;
                Benchwarp.instance.Settings.hasVisitedMato = true;
                Benchwarp.instance.Settings.hasVisitedXRHotSprings = true;
                Benchwarp.instance.Settings.hasVisitedXRStag = true;
                Benchwarp.instance.Settings.hasVisitedSalubra = true;
                Benchwarp.instance.Settings.hasVisitedAncestralMound = true;
                Benchwarp.instance.Settings.hasVisitedBlackEggTemple = true;
                Benchwarp.instance.Settings.hasVisitedWaterfall = true;
                Benchwarp.instance.Settings.hasVisitedStoneSanctuary = true;
                Benchwarp.instance.Settings.hasVisitedGPToll = true;
                Benchwarp.instance.Settings.hasVisitedGPStag = true;
                Benchwarp.instance.Settings.hasVisitedLakeofUnn = true;
                Benchwarp.instance.Settings.hasVisitedSheo = true;
                Benchwarp.instance.Settings.hasVisitedTeachersArchives = true;
                Benchwarp.instance.Settings.hasVisitedQueensStation = true;
                Benchwarp.instance.Settings.hasVisitedLegEater = true;
                Benchwarp.instance.Settings.hasVisitedBretta = true;
                Benchwarp.instance.Settings.hasVisitedMantisVillage = true;
                Benchwarp.instance.Settings.hasVisitedQuirrel = true;
                Benchwarp.instance.Settings.hasVisitedCoTToll = true;
                Benchwarp.instance.Settings.hasVisitedCityStorerooms = true;
                Benchwarp.instance.Settings.hasVisitedWatchersSpire = true;
                Benchwarp.instance.Settings.hasVisitedKingsStation = true;
                Benchwarp.instance.Settings.hasVisitedPleasureHouse = true;
                Benchwarp.instance.Settings.hasVisitedWaterways = true;
                Benchwarp.instance.Settings.hasVisitedGodhome = true;
                Benchwarp.instance.Settings.hasVisitedHallofGods = true;
                Benchwarp.instance.Settings.hasVisitedDNHotSprings = true;
                Benchwarp.instance.Settings.hasVisitedFailedTramway = true;
                Benchwarp.instance.Settings.hasVisitedBeastsDen = true;
                Benchwarp.instance.Settings.hasVisitedABToll = true;
                Benchwarp.instance.Settings.hasVisitedABStag = true;
                Benchwarp.instance.Settings.hasVisitedOro = true;
                Benchwarp.instance.Settings.hasVisitedCamp = true;
                Benchwarp.instance.Settings.hasVisitedColosseum = true;
                Benchwarp.instance.Settings.hasVisitedHive = true;
                Benchwarp.instance.Settings.hasVisitedDarkRoom = true;
                Benchwarp.instance.Settings.hasVisitedCrystalGuardian = true;
                Benchwarp.instance.Settings.hasVisitedQGCornifer = true;
                Benchwarp.instance.Settings.hasVisitedQGToll = true;
                Benchwarp.instance.Settings.hasVisitedQGStag = true;
                Benchwarp.instance.Settings.hasVisitedWPEntrance = true;
                Benchwarp.instance.Settings.hasVisitedWPAtrium = true;
                Benchwarp.instance.Settings.hasVisitedWPBalcony = true;
                Benchwarp.instance.Settings.hasVisitedUpperTram = true;
                Benchwarp.instance.Settings.hasVisitedLowerTram = true;
                Benchwarp.instance.Settings.hasVisitedRGStag = true;
                Benchwarp.instance.Settings.hasVisitedGreyMourner = true;

                Benchwarp.instance.Settings.UnlockAllBenchesFinished = true;
            }
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
            }
        }

    }
}
