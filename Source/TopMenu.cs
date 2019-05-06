using System;
using UnityEngine;
using GlobalEnums;

namespace Benchwarp
{
    public static class TopMenu
    {
        private static CanvasPanel panel;

        public static void BuildMenu(GameObject canvas)
        {
            panel = new CanvasPanel(canvas, GUIController.Instance.images["ButtonsMenuBG"], new Vector2(342f, 15f), new Vector2(1346f, 0f), new Rect(0f, 0f, 0f, 0f));

            Rect buttonRect = new Rect(0, 0, GUIController.Instance.images["ButtonRect"].width, GUIController.Instance.images["ButtonRect"].height);

            int fontSize = 12;

            //Main buttons
            panel.AddButton("Warp", GUIController.Instance.images["ButtonRect"], new Vector2(-54f, 40f), Vector2.zero, WarpClicked, buttonRect, GUIController.Instance.trajanBold, "Warp");

            panel.AddButton("All", GUIController.Instance.images["ButtonRect"], new Vector2(-54f, 0f), Vector2.zero, AllClicked, buttonRect, GUIController.Instance.trajanBold, "All");
            panel.AddButton("Cliffs", GUIController.Instance.images["ButtonRect"], new Vector2(46f, 0f), Vector2.zero, CliffsClicked, buttonRect, GUIController.Instance.trajanBold, "Cliffs");
            panel.AddButton("Crossroads", GUIController.Instance.images["ButtonRect"], new Vector2(146f, 0f), Vector2.zero, CrossroadsClicked, buttonRect, GUIController.Instance.trajanBold, "Crossroads");
            panel.AddButton("Greenpath", GUIController.Instance.images["ButtonRect"], new Vector2(246f, 0f), Vector2.zero, GreenpathClicked, buttonRect, GUIController.Instance.trajanBold, "Greenpath");
            panel.AddButton("Canyon", GUIController.Instance.images["ButtonRect"], new Vector2(346f, 0f), Vector2.zero, CanyonClicked, buttonRect, GUIController.Instance.trajanBold, "Canyon");
            panel.AddButton("Wastes", GUIController.Instance.images["ButtonRect"], new Vector2(446f, 0f), Vector2.zero, WastesClicked, buttonRect, GUIController.Instance.trajanBold, "Wastes");
            panel.AddButton("City", GUIController.Instance.images["ButtonRect"], new Vector2(546f, 0f), Vector2.zero, CityClicked, buttonRect, GUIController.Instance.trajanBold, "City");
            panel.AddButton("RoyalWaterways", GUIController.Instance.images["ButtonRect"], new Vector2(646f, 0f), Vector2.zero, RoyalWaterwaysClicked, buttonRect, GUIController.Instance.trajanBold, "Waterways");
            panel.AddButton("Deepnest", GUIController.Instance.images["ButtonRect"], new Vector2(746f, 0f), Vector2.zero, DeepnestClicked, buttonRect, GUIController.Instance.trajanBold, "Deepnest");
            panel.AddButton("Basin", GUIController.Instance.images["ButtonRect"], new Vector2(846f, 0f), Vector2.zero, BasinClicked, buttonRect, GUIController.Instance.trajanBold, "Basin");
            panel.AddButton("Edge", GUIController.Instance.images["ButtonRect"], new Vector2(946f, 0f), Vector2.zero, EdgeClicked, buttonRect, GUIController.Instance.trajanBold, "Edge");
            panel.AddButton("Peak", GUIController.Instance.images["ButtonRect"], new Vector2(1046f, 0f), Vector2.zero, PeakClicked, buttonRect, GUIController.Instance.trajanBold, "Peak");
            panel.AddButton("Grounds", GUIController.Instance.images["ButtonRect"], new Vector2(1146f, 0f), Vector2.zero, GroundsClicked, buttonRect, GUIController.Instance.trajanBold, "Grounds");
            panel.AddButton("Gardens", GUIController.Instance.images["ButtonRect"], new Vector2(1246f, 0f), Vector2.zero, GardensClicked, buttonRect, GUIController.Instance.trajanBold, "Gardens");
            panel.AddButton("Palace", GUIController.Instance.images["ButtonRect"], new Vector2(1346f, 0f), Vector2.zero, PalaceClicked, buttonRect, GUIController.Instance.trajanBold, "Palace");
            panel.AddButton("Tram", GUIController.Instance.images["ButtonRect"], new Vector2(1446f, 0f), Vector2.zero, TramClicked, buttonRect, GUIController.Instance.trajanBold, "Tram");

            //Dropdown panels
            panel.AddPanel("Cliffs Panel", GUIController.Instance.images["DropdownBG"], new Vector2(45f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Crossroads Panel", GUIController.Instance.images["DropdownBG"], new Vector2(145f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Greenpath Panel", GUIController.Instance.images["DropdownBG"], new Vector2(245f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Canyon Panel", GUIController.Instance.images["DropdownBG"], new Vector2(345f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Wastes Panel", GUIController.Instance.images["DropdownBG"], new Vector2(445f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("City Panel", GUIController.Instance.images["DropdownBG"], new Vector2(545f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Waterways Panel", GUIController.Instance.images["DropdownBG"], new Vector2(645f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Deepnest Panel", GUIController.Instance.images["DropdownBG"], new Vector2(745f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Basin Panel", GUIController.Instance.images["DropdownBG"], new Vector2(845f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Edge Panel", GUIController.Instance.images["DropdownBG"], new Vector2(945f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Peak Panel", GUIController.Instance.images["DropdownBG"], new Vector2(1045f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Grounds Panel", GUIController.Instance.images["DropdownBG"], new Vector2(1145f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Gardens Panel", GUIController.Instance.images["DropdownBG"], new Vector2(1245f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Palace Panel", GUIController.Instance.images["DropdownBG"], new Vector2(1345f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));
            panel.AddPanel("Tram Panel", GUIController.Instance.images["DropdownBG"], new Vector2(1445f, 20f), Vector2.zero, new Rect(0, 0, GUIController.Instance.images["DropdownBG"].width, 270f));

            //Cheats panel
            panel.GetPanel("Cliffs Panel").AddButton("KingsPass", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, KingsPassClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "King's Pass", fontSize);
            panel.GetPanel("Cliffs Panel").AddButton("Dirtmouth", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, DirtmouthClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Dirtmouth", fontSize);
            panel.GetPanel("Cliffs Panel").AddButton("Mato", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 105f), Vector2.zero, MatoClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Mato", fontSize);
            
            //Crossroads panel
            panel.GetPanel("Crossroads Panel").AddButton("XRHotSprings", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, XRHotSpringsClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Hot Springs", fontSize);
            panel.GetPanel("Crossroads Panel").AddButton("XRStag", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, XRStagClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Stag", fontSize);
            panel.GetPanel("Crossroads Panel").AddButton("Salubra", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 105f), Vector2.zero, SalubraClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Salubra", fontSize);
            panel.GetPanel("Crossroads Panel").AddButton("AncestralMound", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 145f), Vector2.zero, AncestralMoundClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Ancestral Mound", fontSize);
            panel.GetPanel("Crossroads Panel").AddButton("BlackEggTemple", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 185f), Vector2.zero, BlackEggTempleClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Black Egg Temple", fontSize);

            //Greenpath panel
            panel.GetPanel("Greenpath Panel").AddButton("Waterfall", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, WaterfallClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Waterfall", fontSize);
            panel.GetPanel("Greenpath Panel").AddButton("StoneSanctuary", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, StoneSanctuaryClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Stone Sanctuary", fontSize);
            panel.GetPanel("Greenpath Panel").AddButton("GPToll", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 105f), Vector2.zero, GPTollClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Toll", fontSize);
            panel.GetPanel("Greenpath Panel").AddButton("GPStag", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 145f), Vector2.zero, GPStagClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Stag", fontSize);
            panel.GetPanel("Greenpath Panel").AddButton("LakeofUnn", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 185f), Vector2.zero, LakeofUnnClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Lake of Unn", fontSize);
            panel.GetPanel("Greenpath Panel").AddButton("Sheo", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 225f), Vector2.zero, SheoClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Sheo", fontSize);

            //Canyon panel
            panel.GetPanel("Canyon Panel").AddButton("Archives", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, TeachersArchivesClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Archives", fontSize);

            //Wastes panel
            panel.GetPanel("Wastes Panel").AddButton("QueensStation", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, QueensStationClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Queen's Station", fontSize);
            panel.GetPanel("Wastes Panel").AddButton("LegEater", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, LegEaterClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Leg Eater", fontSize);
            panel.GetPanel("Wastes Panel").AddButton("Bretta", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 105f), Vector2.zero, BrettaClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Bretta", fontSize);
            panel.GetPanel("Wastes Panel").AddButton("MantisVillage", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 145f), Vector2.zero, MantisVillageClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Mantis Village", fontSize);

            //City panel
            panel.GetPanel("City Panel").AddButton("Quirrel", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, QuirrelClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Quirrel", fontSize);
            panel.GetPanel("City Panel").AddButton("CoTToll", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, CoTTollClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Toll", fontSize);
            panel.GetPanel("City Panel").AddButton("CityStorerooms", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 105f), Vector2.zero, CityStoreroomsClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "City Storerooms", fontSize);
            panel.GetPanel("City Panel").AddButton("WatchersSpire", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 145f), Vector2.zero, WatchersSpireClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Watcher's Spire", fontSize);
            panel.GetPanel("City Panel").AddButton("KingsStation", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 185f), Vector2.zero, KingsStationClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "King's Station", fontSize);
            panel.GetPanel("City Panel").AddButton("PleasureHouse", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 225f), Vector2.zero, PleasureHouseClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Pleasure House", fontSize);

            //Waterways panel
            panel.GetPanel("Waterways Panel").AddButton("Waterways", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, WaterwaysClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Waterways", fontSize);
            panel.GetPanel("Waterways Panel").AddButton("Godhome", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, GodhomeClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Godhome", fontSize);
            panel.GetPanel("Waterways Panel").AddButton("HallofGods", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 105f), Vector2.zero, HallofGodsClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Hall of Gods", fontSize);

            //Deepnest panel
            panel.GetPanel("Deepnest Panel").AddButton("DNHotSprings", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, DNHotSpringsClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Hot Springs", fontSize);
            panel.GetPanel("Deepnest Panel").AddButton("FailedTramway", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, FailedTramwayClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Failed Tramway", fontSize);
            panel.GetPanel("Deepnest Panel").AddButton("BeastsDen", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 105f), Vector2.zero, BeastsDenClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Beast's Den", fontSize);

            //Basin panel
            panel.GetPanel("Basin Panel").AddButton("ABToll", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, ABTollClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Toll", fontSize);
            panel.GetPanel("Basin Panel").AddButton("ABStag", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, ABStagClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Stag", fontSize);

            //Edge panel
            panel.GetPanel("Edge Panel").AddButton("Oro", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, OroClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Oro", fontSize);
            panel.GetPanel("Edge Panel").AddButton("Camp", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, CampClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Camp", fontSize);
            panel.GetPanel("Edge Panel").AddButton("Colosseum", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 105f), Vector2.zero, ColosseumClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Colosseum", fontSize);
            panel.GetPanel("Edge Panel").AddButton("Hive", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 145f), Vector2.zero, HiveClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Hive", fontSize);

            //Peak panel
            panel.GetPanel("Peak Panel").AddButton("DarkRoom", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, DarkRoomClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Dark Room", fontSize);
            panel.GetPanel("Peak Panel").AddButton("CrystalGuardian", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, CrystalGuardianClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Crystal Guardian", fontSize);

            //Grounds panel
            panel.GetPanel("Grounds Panel").AddButton("RGStag", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, RGStagClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Stag", fontSize);
            panel.GetPanel("Grounds Panel").AddButton("GreyMourner", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, GreyMournerClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Grey Mourner", fontSize);

            //Gardens panel
            panel.GetPanel("Gardens Panel").AddButton("QGCornifer", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, QGCorniferClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Cornifer", fontSize);
            panel.GetPanel("Gardens Panel").AddButton("QGToll", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, QGTollClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Toll", fontSize);
            panel.GetPanel("Gardens Panel").AddButton("QGStag", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 105f), Vector2.zero, QGStagClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Stag", fontSize);

            //Palace panel
            panel.GetPanel("Palace Panel").AddButton("WPEntrance", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, WPEntranceClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Entrance", fontSize);
            panel.GetPanel("Palace Panel").AddButton("WPAtrium", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, WPAtriumClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Atrium", fontSize);
            panel.GetPanel("Palace Panel").AddButton("WPBalcony", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 105f), Vector2.zero, WPBalconyClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Balcony", fontSize);

            //Tram panel
            panel.GetPanel("Tram Panel").AddButton("UpperTram", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 25f), Vector2.zero, UpperTramClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Upper Tram", fontSize);
            panel.GetPanel("Tram Panel").AddButton("LowerTram", GUIController.Instance.images["ButtonRectEmpty"], new Vector2(5f, 65f), Vector2.zero, LowerTramClicked, new Rect(0f, 0f, 80f, 40f), GUIController.Instance.trajanNormal, "Lower Tram", fontSize);

            panel.FixRenderOrder();
        }

        public static void Update()
        {
            if (panel == null)
            {
                return;
            }

            if (GameManager.instance.IsGamePaused())
            {
                panel.SetActive(true, false);
            }
            else if (!GameManager.instance.IsGamePaused())
            {
                panel.SetActive(false, true);
            }



            if (panel.GetPanel("Cliffs Panel").active)
            {
                panel.GetButton("KingsPass", "Cliffs Panel").SetTextColor(PlayerData.instance.respawnScene == "Tutorial_01" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedDirtmouth) panel.GetButton("Dirtmouth", "Cliffs Panel").SetTextColor(Color.red);
                else panel.GetButton("Dirtmouth", "Cliffs Panel").SetTextColor(PlayerData.instance.respawnScene == "Town" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedMato) panel.GetButton("Mato", "Cliffs Panel").SetTextColor(Color.red);
                else panel.GetButton("Mato", "Cliffs Panel").SetTextColor(PlayerData.instance.respawnScene == "Room_nailmaster" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Crossroads Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedXRHotSprings) panel.GetButton("XRHotSprings", "Crossroads Panel").SetTextColor(Color.red);
                else panel.GetButton("XRHotSprings", "Crossroads Panel").SetTextColor(PlayerData.instance.respawnScene == "Crossroads_30" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedXRStag) panel.GetButton("XRStag", "Crossroads Panel").SetTextColor(Color.red);
                else panel.GetButton("XRStag", "Crossroads Panel").SetTextColor(PlayerData.instance.respawnScene == "Crossroads_47" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedSalubra) panel.GetButton("Salubra", "Crossroads Panel").SetTextColor(Color.red);
                else panel.GetButton("Salubra", "Crossroads Panel").SetTextColor(PlayerData.instance.respawnScene == "Crossroads_04" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedAncestralMound) panel.GetButton("AncestralMound", "Crossroads Panel").SetTextColor(Color.red);
                else panel.GetButton("AncestralMound", "Crossroads Panel").SetTextColor(PlayerData.instance.respawnScene == "Crossroads_ShamanTemple" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedBlackEggTemple) panel.GetButton("BlackEggTemple", "Crossroads Panel").SetTextColor(Color.red);
                else panel.GetButton("BlackEggTemple", "Crossroads Panel").SetTextColor(PlayerData.instance.respawnScene == "Room_Final_Boss_Atrium" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Greenpath Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedWaterfall) panel.GetButton("Waterfall", "Greenpath Panel").SetTextColor(Color.red);
                else panel.GetButton("Waterfall", "Greenpath Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus1_01b" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedStoneSanctuary) panel.GetButton("StoneSanctuary", "Greenpath Panel").SetTextColor(Color.red);
                else panel.GetButton("StoneSanctuary", "Greenpath Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus1_37" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedGPToll) panel.GetButton("GPToll", "Greenpath Panel").SetTextColor(Color.red);
                else panel.GetButton("GPToll", "Greenpath Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus1_31" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedGPStag) panel.GetButton("GPStag", "Greenpath Panel").SetTextColor(Color.red);
                else panel.GetButton("GPStag", "Greenpath Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus1_16_alt" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedLakeofUnn) panel.GetButton("LakeofUnn", "Greenpath Panel").SetTextColor(Color.red);
                else panel.GetButton("LakeofUnn", "Greenpath Panel").SetTextColor(PlayerData.instance.respawnScene == "Room_Slug_Shrine" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedSheo) panel.GetButton("Sheo", "Greenpath Panel").SetTextColor(Color.red);
                else panel.GetButton("Sheo", "Greenpath Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus1_15" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Canyon Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedTeachersArchives) panel.GetButton("Archives", "Canyon Panel").SetTextColor(Color.red);
                else panel.GetButton("Archives", "Canyon Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus3_archive" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Wastes Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedQueensStation) panel.GetButton("QueensStation", "Wastes Panel").SetTextColor(Color.red);
                else panel.GetButton("QueensStation", "Wastes Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus2_02" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedLegEater) panel.GetButton("LegEater", "Wastes Panel").SetTextColor(Color.red);
                else panel.GetButton("LegEater", "Wastes Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus2_26" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedBretta) panel.GetButton("Bretta", "Wastes Panel").SetTextColor(Color.red);
                else panel.GetButton("Bretta", "Wastes Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus2_13" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedMantisVillage) panel.GetButton("MantisVillage", "Wastes Panel").SetTextColor(Color.red);
                else panel.GetButton("MantisVillage", "Wastes Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus2_31" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("City Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedQuirrel) panel.GetButton("Quirrel", "City Panel").SetTextColor(Color.red);
                else panel.GetButton("Quirrel", "City Panel").SetTextColor(PlayerData.instance.respawnScene == "Ruins1_02" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedCoTToll) panel.GetButton("CoTToll", "City Panel").SetTextColor(Color.red);
                else panel.GetButton("CoTToll", "City Panel").SetTextColor(PlayerData.instance.respawnScene == "Ruins1_31" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedCityStorerooms) panel.GetButton("CityStorerooms", "City Panel").SetTextColor(Color.red);
                else panel.GetButton("CityStorerooms", "City Panel").SetTextColor(PlayerData.instance.respawnScene == "Ruins1_29" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedWatchersSpire) panel.GetButton("WatchersSpire", "City Panel").SetTextColor(Color.red);
                else panel.GetButton("WatchersSpire", "City Panel").SetTextColor(PlayerData.instance.respawnScene == "Ruins1_18" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedKingsStation) panel.GetButton("KingsStation", "City Panel").SetTextColor(Color.red);
                else panel.GetButton("KingsStation", "City Panel").SetTextColor(PlayerData.instance.respawnScene == "Ruins2_08" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedPleasureHouse) panel.GetButton("PleasureHouse", "City Panel").SetTextColor(Color.red);
                else panel.GetButton("PleasureHouse", "City Panel").SetTextColor(PlayerData.instance.respawnScene == "Ruins_Bathhouse" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Waterways Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedWaterways) panel.GetButton("Waterways", "Waterways Panel").SetTextColor(Color.red);
                else panel.GetButton("Waterways", "Waterways Panel").SetTextColor(PlayerData.instance.respawnScene == "Waterways_02" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedGodhome) panel.GetButton("Godhome", "Waterways Panel").SetTextColor(Color.red);
                else panel.GetButton("Godhome", "Waterways Panel").SetTextColor(PlayerData.instance.respawnScene == "GG_Atrium" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedHallofGods) panel.GetButton("HallofGods", "Waterways Panel").SetTextColor(Color.red);
                else panel.GetButton("HallofGods", "Waterways Panel").SetTextColor(PlayerData.instance.respawnScene == "GG_Workshop" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Deepnest Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedDNHotSprings) panel.GetButton("DNHotSprings", "Deepnest Panel").SetTextColor(Color.red);
                else panel.GetButton("DNHotSprings", "Deepnest Panel").SetTextColor(PlayerData.instance.respawnScene == "Deepnest_30" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedFailedTramway) panel.GetButton("FailedTramway", "Deepnest Panel").SetTextColor(Color.red);
                else panel.GetButton("FailedTramway", "Deepnest Panel").SetTextColor(PlayerData.instance.respawnScene == "Deepnest_14" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedBeastsDen) panel.GetButton("BeastsDen", "Deepnest Panel").SetTextColor(Color.red);
                else panel.GetButton("BeastsDen", "Deepnest Panel").SetTextColor(PlayerData.instance.respawnScene == "Deepnest_Spider_Town" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Basin Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedABToll) panel.GetButton("ABToll", "Basin Panel").SetTextColor(Color.red);
                else panel.GetButton("ABToll", "Basin Panel").SetTextColor(PlayerData.instance.respawnScene == "Abyss_18" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedABStag) panel.GetButton("ABStag", "Basin Panel").SetTextColor(Color.red);
                else panel.GetButton("ABStag", "Basin Panel").SetTextColor(PlayerData.instance.respawnScene == "Abyss_22" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Edge Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedOro) panel.GetButton("Oro", "Edge Panel").SetTextColor(Color.red);
                else panel.GetButton("Oro", "Edge Panel").SetTextColor(PlayerData.instance.respawnScene == "Deepnest_East_06" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedCamp) panel.GetButton("Camp", "Edge Panel").SetTextColor(Color.red);
                else panel.GetButton("Camp", "Edge Panel").SetTextColor(PlayerData.instance.respawnScene == "Deepnest_East_13" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedColosseum) panel.GetButton("Colosseum", "Edge Panel").SetTextColor(Color.red);
                else panel.GetButton("Colosseum", "Edge Panel").SetTextColor(PlayerData.instance.respawnScene == "Room_Colosseum_02" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedHive) panel.GetButton("Hive", "Edge Panel").SetTextColor(Color.red);
                else panel.GetButton("Hive", "Edge Panel").SetTextColor(PlayerData.instance.respawnScene == "Hive_01" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Peak Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedDarkRoom) panel.GetButton("DarkRoom", "Peak Panel").SetTextColor(Color.red);
                else panel.GetButton("DarkRoom", "Peak Panel").SetTextColor(PlayerData.instance.respawnScene == "Mines_29" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedCrystalGuardian) panel.GetButton("CrystalGuardian", "Peak Panel").SetTextColor(Color.red);
                else panel.GetButton("CrystalGuardian", "Peak Panel").SetTextColor(PlayerData.instance.respawnScene == "Mines_18" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Grounds Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedRGStag) panel.GetButton("RGStag", "Grounds Panel").SetTextColor(Color.red);
                else panel.GetButton("RGStag", "Grounds Panel").SetTextColor(PlayerData.instance.respawnScene == "RestingGrounds_09" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedGreyMourner) panel.GetButton("GreyMourner", "Grounds Panel").SetTextColor(Color.red);
                else panel.GetButton("GreyMourner", "Grounds Panel").SetTextColor(PlayerData.instance.respawnScene == "RestingGrounds_12" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Gardens Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedQGCornifer) panel.GetButton("QGCornifer", "Gardens Panel").SetTextColor(Color.red);
                else panel.GetButton("QGCornifer", "Gardens Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus1_24" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedQGToll) panel.GetButton("QGToll", "Gardens Panel").SetTextColor(Color.red);
                else panel.GetButton("QGToll", "Gardens Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus3_50" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedQGStag) panel.GetButton("QGStag", "Gardens Panel").SetTextColor(Color.red);
                else panel.GetButton("QGStag", "Gardens Panel").SetTextColor(PlayerData.instance.respawnScene == "Fungus3_40" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Palace Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedWPEntrance) panel.GetButton("WPEntrance", "Palace Panel").SetTextColor(Color.red);
                else panel.GetButton("WPEntrance", "Palace Panel").SetTextColor(PlayerData.instance.respawnScene == "White_Palace_01" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedWPAtrium) panel.GetButton("WPAtrium", "Palace Panel").SetTextColor(Color.red);
                else panel.GetButton("WPAtrium", "Palace Panel").SetTextColor(PlayerData.instance.respawnScene == "White_Palace_03_hub" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedWPBalcony) panel.GetButton("WPBalcony", "Palace Panel").SetTextColor(Color.red);
                else panel.GetButton("WPBalcony", "Palace Panel").SetTextColor(PlayerData.instance.respawnScene == "White_Palace_06" ? Color.yellow : Color.white);
            }

            if (panel.GetPanel("Tram Panel").active)
            {
                if (!Benchwarp.instance.Settings.hasVisitedUpperTram) panel.GetButton("UpperTram", "Tram Panel").SetTextColor(Color.red);
                else panel.GetButton("UpperTram", "Tram Panel").SetTextColor(PlayerData.instance.respawnScene == "Room_Tram_RG" ? Color.yellow : Color.white);

                if (!Benchwarp.instance.Settings.hasVisitedLowerTram) panel.GetButton("LowerTram", "Tram Panel").SetTextColor(Color.red);
                else panel.GetButton("LowerTram", "Tram Panel").SetTextColor(PlayerData.instance.respawnScene == "Room_Tram" ? Color.yellow : Color.white);
            }
           }




        private static void WarpClicked(string buttonName)
        {
                GameManager.instance.StartCoroutine(Benchwarp.instance.Respawn());
        }

        #region Dropdown toggle methods
        private static void AllClicked(string buttonName)
        {
            panel.TogglePanel("Cliffs Panel");
            panel.TogglePanel("Crossroads Panel");
            panel.TogglePanel("Greenpath Panel");
            panel.TogglePanel("Canyon Panel");
            panel.TogglePanel("Wastes Panel");
            panel.TogglePanel("City Panel");
            panel.TogglePanel("Waterways Panel");
            panel.TogglePanel("Deepnest Panel");
            panel.TogglePanel("Basin Panel");
            panel.TogglePanel("Edge Panel");
            panel.TogglePanel("Peak Panel");
            panel.TogglePanel("Grounds Panel");
            panel.TogglePanel("Gardens Panel");
            panel.TogglePanel("Palace Panel");
            panel.TogglePanel("Tram Panel");
        }

        private static void CliffsClicked(string buttonName)
        {
            panel.TogglePanel("Cliffs Panel");
        }

        private static void CrossroadsClicked(string buttonName)
        {
            panel.TogglePanel("Crossroads Panel");
        }

        private static void GreenpathClicked(string buttonName)
        {
            panel.TogglePanel("Greenpath Panel");
        }
        private static void CanyonClicked(string buttonName)
        {
            panel.TogglePanel("Canyon Panel");
        }
        private static void WastesClicked(string buttonName)
        {
            panel.TogglePanel("Wastes Panel");
        }
        private static void CityClicked(string buttonName)
        {
            panel.TogglePanel("City Panel");
        }
        private static void DeepnestClicked(string buttonName)
        {
            panel.TogglePanel("Deepnest Panel");
        }
        private static void RoyalWaterwaysClicked(string buttonName)
        {
            panel.TogglePanel("Waterways Panel");
        }
        private static void BasinClicked(string buttonName)
        {
            panel.TogglePanel("Basin Panel");
        }
        private static void EdgeClicked(string buttonName)
        {
            panel.TogglePanel("Edge Panel");
        }
        private static void PeakClicked(string buttonName)
        {
            panel.TogglePanel("Peak Panel");
        }
        private static void GroundsClicked(string buttonName)
        {
            panel.TogglePanel("Grounds Panel");
        }
        private static void GardensClicked(string buttonName)
        {
            panel.TogglePanel("Gardens Panel");
        }
        private static void PalaceClicked(string buttonName)
        {
            panel.TogglePanel("Palace Panel");
        }
        private static void TramClicked(string buttonName)
        {
            panel.TogglePanel("Tram Panel");
        }
        #endregion

        #region Bench button methods
        private static void KingsPassClicked(string buttonName)
        {
            PlayerData.instance.respawnScene = "Tutorial_01";
            PlayerData.instance.mapZone = (MapZone)2;
            PlayerData.instance.respawnType = 0;
            PlayerData.instance.respawnMarkerName = "Death Respawn Marker";
        }

        private static void DirtmouthClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedDirtmouth)
            {
                PlayerData.instance.respawnScene = "Town";
                PlayerData.instance.mapZone = (MapZone)4;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
            
        }

        private static void MatoClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedMato)
            {
                PlayerData.instance.respawnScene = "Room_nailmaster";
                PlayerData.instance.mapZone = (MapZone)3;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void XRHotSpringsClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedXRHotSprings)
            {
                PlayerData.instance.respawnScene = "Crossroads_30";
                PlayerData.instance.mapZone = (MapZone)5;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }

        private static void XRStagClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedXRStag)
            {
                PlayerData.instance.respawnScene = "Crossroads_47";
                PlayerData.instance.mapZone = (MapZone)5;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }

        private static void SalubraClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedSalubra)
            {
                PlayerData.instance.respawnScene = "Crossroads_04";
                PlayerData.instance.mapZone = (MapZone)5;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }

        private static void AncestralMoundClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedAncestralMound)
            {
                PlayerData.instance.respawnScene = "Crossroads_ShamanTemple";
                PlayerData.instance.mapZone = (MapZone)22;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "BoneBench";
            }
        }
        private static void BlackEggTempleClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedBlackEggTemple)
            {
                PlayerData.instance.respawnScene = "Room_Final_Boss_Atrium";
                PlayerData.instance.mapZone = (MapZone)30;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void WaterfallClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedWaterfall)
            {
                PlayerData.instance.respawnScene = "Fungus1_01b";
                PlayerData.instance.mapZone = (MapZone)6;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void StoneSanctuaryClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedStoneSanctuary)
            {
                PlayerData.instance.respawnScene = "Fungus1_37";
                PlayerData.instance.mapZone = (MapZone)6;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void GPTollClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedGPToll)
            {
                PlayerData.instance.respawnScene = "Fungus1_31";
                PlayerData.instance.mapZone = (MapZone)6;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void GPStagClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedGPStag)
            {
                PlayerData.instance.respawnScene = "Fungus1_16_alt";
                PlayerData.instance.mapZone = (MapZone)6;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void LakeofUnnClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedLakeofUnn)
            {
                PlayerData.instance.respawnScene = "Room_Slug_Shrine";
                PlayerData.instance.mapZone = (MapZone)6;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void SheoClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedSheo)
            {
                PlayerData.instance.respawnScene = "Fungus1_15";
                PlayerData.instance.mapZone = (MapZone)6;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void TeachersArchivesClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedTeachersArchives)
            {
                PlayerData.instance.respawnScene = "Fungus3_archive";
                PlayerData.instance.mapZone = (MapZone)34;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void QueensStationClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedQueensStation)
            {
                PlayerData.instance.respawnScene = "Fungus2_02";
                PlayerData.instance.mapZone = (MapZone)24;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void LegEaterClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedLegEater)
            {
                PlayerData.instance.respawnScene = "Fungus2_26";
                PlayerData.instance.mapZone = (MapZone)9;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void MantisVillageClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedMantisVillage)
            {
                PlayerData.instance.respawnScene = "Fungus2_31";
                PlayerData.instance.mapZone = (MapZone)9;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void BrettaClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedBretta)
            {
                PlayerData.instance.respawnScene = "Fungus2_13";
                PlayerData.instance.mapZone = (MapZone)9;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void QuirrelClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedQuirrel)
            {
                PlayerData.instance.respawnScene = "Ruins1_02";
                PlayerData.instance.mapZone = (MapZone)16;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void CoTTollClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedCoTToll)
            {
                PlayerData.instance.respawnScene = "Ruins1_31";
                PlayerData.instance.mapZone = (MapZone)16;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void CityStoreroomsClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedCityStorerooms)
            {
                PlayerData.instance.respawnScene = "Ruins1_29";
                PlayerData.instance.mapZone = (MapZone)16;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void WatchersSpireClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedWatchersSpire)
            {
                PlayerData.instance.respawnScene = "Ruins1_18";
                PlayerData.instance.mapZone = (MapZone)16;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void KingsStationClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedKingsStation)
            {
                PlayerData.instance.respawnScene = "Ruins2_08";
                PlayerData.instance.mapZone = (MapZone)16;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void PleasureHouseClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedPleasureHouse)
            {
                PlayerData.instance.respawnScene = "Ruins_Bathhouse";
                PlayerData.instance.mapZone = (MapZone)16;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void DNHotSpringsClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedDNHotSprings)
            {
                PlayerData.instance.respawnScene = "Deepnest_30";
                PlayerData.instance.mapZone = (MapZone)10;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void FailedTramwayClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedFailedTramway)
            {
                PlayerData.instance.respawnScene = "Deepnest_14";
                PlayerData.instance.mapZone = (MapZone)10;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void BeastsDenClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedBeastsDen)
            {
                PlayerData.instance.respawnScene = "Deepnest_Spider_Town";
                PlayerData.instance.mapZone = (MapZone)49;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench Return";
            }
        }
        private static void WaterwaysClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedWaterways)
            {
                PlayerData.instance.respawnScene = "Waterways_02";
                PlayerData.instance.mapZone = (MapZone)23;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void GodhomeClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedGodhome)
            {
                PlayerData.instance.respawnScene = "GG_Atrium";
                PlayerData.instance.mapZone = (MapZone)50;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void HallofGodsClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedHallofGods)
            {
                PlayerData.instance.respawnScene = "GG_Workshop";
                PlayerData.instance.mapZone = (MapZone)50;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench (1)";
            }
        }
        private static void DarkRoomClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedDarkRoom)
            {
                PlayerData.instance.respawnScene = "Mines_29";
                PlayerData.instance.mapZone = (MapZone)14;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void CrystalGuardianClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedCrystalGuardian)
            {
                PlayerData.instance.respawnScene = "Mines_18";
                PlayerData.instance.mapZone = (MapZone)14;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void ABTollClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedABToll)
            {
                PlayerData.instance.respawnScene = "Abyss_18";
                PlayerData.instance.mapZone = (MapZone)19;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void ABStagClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedABStag)
            {
                PlayerData.instance.respawnScene = "Abyss_22";
                PlayerData.instance.mapZone = (MapZone)19;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void WPEntranceClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedWPEntrance)
            {
                PlayerData.instance.respawnScene = "White_Palace_01";
                PlayerData.instance.mapZone = (MapZone)21;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "WhiteBench";
            }
        }
        private static void WPAtriumClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedWPAtrium)
            {
                PlayerData.instance.respawnScene = "White_Palace_03_hub";
                PlayerData.instance.mapZone = (MapZone)21;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "WhiteBench";
            }
        }
        private static void WPBalconyClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedWPBalcony)
            {
                PlayerData.instance.respawnScene = "White_Palace_06";
                PlayerData.instance.mapZone = (MapZone)21;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void OroClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedOro)
            {
                PlayerData.instance.respawnScene = "Deepnest_East_06";
                PlayerData.instance.mapZone = (MapZone)25;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void CampClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedCamp)
            {
                PlayerData.instance.respawnScene = "Deepnest_East_13";
                PlayerData.instance.mapZone = (MapZone)25;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void ColosseumClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedColosseum)
            {
                PlayerData.instance.respawnScene = "Room_Colosseum_02";
                PlayerData.instance.mapZone = (MapZone)18;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void HiveClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedHive)
            {
                PlayerData.instance.respawnScene = "Hive_01";
                PlayerData.instance.mapZone = (MapZone)11;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void RGStagClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedRGStag)
            {
                PlayerData.instance.respawnScene = "RestingGrounds_09";
                PlayerData.instance.mapZone = (MapZone)15;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void GreyMournerClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedGreyMourner)
            {
                PlayerData.instance.respawnScene = "RestingGrounds_12";
                PlayerData.instance.mapZone = (MapZone)15;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void QGCorniferClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedQGCornifer)
            {
                PlayerData.instance.respawnScene = "Fungus1_24";
                PlayerData.instance.mapZone = (MapZone)7;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void QGTollClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedQGToll)
            {
                PlayerData.instance.respawnScene = "Fungus3_50";
                PlayerData.instance.mapZone = (MapZone)7;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void QGStagClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedQGStag)
            {
                PlayerData.instance.respawnScene = "Fungus3_40";
                PlayerData.instance.mapZone = (MapZone)7;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void UpperTramClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedUpperTram)
            {
                PlayerData.instance.respawnScene = "Room_Tram_RG";
                PlayerData.instance.mapZone = (MapZone)28;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        private static void LowerTramClicked(string buttonName)
        {
            if (Benchwarp.instance.Settings.hasVisitedLowerTram)
            {
                PlayerData.instance.respawnScene = "Room_Tram";
                PlayerData.instance.mapZone = (MapZone)29;
                PlayerData.instance.respawnType = 1;
                PlayerData.instance.respawnMarkerName = "RestBench";
            }
        }
        #endregion
    }
}
