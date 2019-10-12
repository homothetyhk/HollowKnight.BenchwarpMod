using GlobalEnums;

namespace Benchwarp
{
    public class Bench
    {
        public static Bench[] Benches;

        public readonly string name;
        public readonly string areaName;
        public readonly string sceneName;
        public readonly string respawnMarker;
        public readonly int respawnType;
        public readonly MapZone mapZone;
        public readonly bool preload;
        public readonly string style;

        public bool visited
        {
            get => Benchwarp.instance.Settings.GetBool(false, sceneName);
            set => Benchwarp.instance.Settings.SetBool(value, sceneName);
        }
        public bool benched => PlayerData.instance.respawnScene == sceneName &&
            PlayerData.instance.respawnMarkerName == respawnMarker &&
            PlayerData.instance.respawnType == respawnType;

        public Bench(string _name, string _areaName, string _sceneName, string _respawnMarker, int _respawnType, MapZone _mapZone, bool _preload = false, string _style = null)
        {
            name = _name; // may not be unique
            areaName = _areaName; // may be abbreviated, see below
            sceneName = _sceneName;
            respawnMarker = _respawnMarker;
            respawnType = _respawnType;
            mapZone = _mapZone;
            preload = _preload;
            style = _style;
        }

        public void SetBench()
        {
            if (!Benchwarp.instance.GlobalSettings.UnlockAllBenches && !visited && sceneName != "Tutorial_01") return;
            PlayerData.instance.respawnScene = sceneName;
            PlayerData.instance.respawnMarkerName = respawnMarker;
            PlayerData.instance.respawnType = respawnType;
            PlayerData.instance.mapZone = mapZone;
        }

        public static void GenerateBenchData()
        {
            Benches = new Bench[]
            {
                new Bench("King's Pass", "Cliffs", "Tutorial_01", "Death Respawn Marker", 0, MapZone.KINGS_PASS),
                new Bench("Dirtmouth", "Cliffs", "Town", "RestBench", 1, MapZone.TOWN, true, "Left"),
                new Bench("Mato", "Cliffs", "Room_nailmaster", "RestBench", 1, MapZone.CLIFFS, true, "Mato"),

                new Bench("Hot Springs", "Crossroads", "Crossroads_30", "RestBench", 1, MapZone.CROSSROADS, true, "Right"),
                new Bench("Stag", "Crossroads", "Crossroads_47", "RestBench", 1, MapZone.CROSSROADS),
                new Bench("Salubra", "Crossroads", "Crossroads_04", "RestBench", 1, MapZone.CROSSROADS, true, "Ornate"),
                new Bench("Ancestral Mound", "Crossroads", "Crossroads_ShamanTemple", "BoneBench", 1, MapZone.SHAMAN_TEMPLE, true, "Bone"),
                new Bench("Black Egg Temple", "Crossroads", "Room_Final_Boss_Atrium", "RestBench", 1, MapZone.FINAL_BOSS, true, "Black"),

                new Bench("Waterfall", "Greenpath", "Fungus1_01b", "RestBench", 1, MapZone.GREEN_PATH),
                new Bench("Stone Sanctuary", "Greenpath", "Fungus1_37", "RestBench", 1, MapZone.GREEN_PATH, true, "Stone"),
                new Bench("Toll", "Greenpath", "Fungus1_31", "RestBench", 1, MapZone.GREEN_PATH),
                new Bench("Stag", "Greenpath", "Fungus1_16_alt", "RestBench", 1, MapZone.GREEN_PATH),
                new Bench("Lake of Unn", "Greenpath", "Room_Slug_Shrine", "RestBench", 1, MapZone.GREEN_PATH, true, "Shrine"),
                new Bench("Sheo", "Greenpath", "Fungus1_15", "RestBench", 1, MapZone.GREEN_PATH, true, "Sheo"),

                new Bench("Archives", "Canyon", "Fungus3_archive", "RestBench", 1, MapZone.FOG_CANYON, true, "Archive"),

                new Bench("Queen's Station", "Wastes", "Fungus2_02", "RestBench", 1, MapZone.QUEENS_STATION),
                new Bench("Leg Eater", "Wastes", "Fungus2_26", "RestBench", 1, MapZone.WASTES, true, "Corpse"),
                new Bench("Bretta", "Wastes", "Fungus2_13", "RestBench", 1, MapZone.WASTES),
                new Bench("Mantis Village", "Wastes", "Fungus2_31", "RestBench", 1, MapZone.WASTES, true, "Mantis"),

                new Bench("Quirrel", "City", "Ruins1_02", "RestBench", 1, MapZone.CITY),
                new Bench("Toll", "City", "Ruins1_31", "RestBench", 1, MapZone.CITY),
                new Bench("City Storerooms", "City", "Ruins1_29", "RestBench", 1, MapZone.CITY),
                new Bench("Watcher's Spire", "City", "Ruins1_18", "RestBench", 1, MapZone.CITY),
                new Bench("King's Station", "City", "Ruins2_08", "RestBench", 1, MapZone.CITY),
                new Bench("Pleasure House", "City", "Ruins_Bathhouse", "RestBench", 1, MapZone.CITY, true, "Simple"),

                new Bench("Waterways", "Waterways", "Waterways_02", "RestBench", 1, MapZone.WATERWAYS, true, "Tilted"),
                new Bench("Godhome Atrium", "Waterways", "GG_Atrium", "RestBench", 1, MapZone.GODS_GLORY, true, "Wide"),
                new Bench("Godhome Roof", "Waterways", "GG_Atrium_Roof", "RestBench (1)", 1, MapZone.GODS_GLORY),
                new Bench("Hall of Gods", "Waterways", "GG_Workshop", "RestBench (1)", 1, MapZone.GODS_GLORY),

                new Bench("Hot Springs", "Deepnest", "Deepnest_30", "RestBench", 1, MapZone.DEEPNEST),
                new Bench("Failed Tramway", "Deepnest", "Deepnest_14", "RestBench", 1, MapZone.DEEPNEST),
                new Bench("Beast's Den", "Deepnest", "Deepnest_Spider_Town", "RestBench Return", 1, MapZone.BEASTS_DEN, true, "Beast"),

                new Bench("Toll", "Basin", "Abyss_18", "RestBench", 1, MapZone.ABYSS),
                new Bench("Hidden Station", "Basin", "Abyss_22", "RestBench", 1, MapZone.ABYSS),

                new Bench("Oro", "Edge", "Deepnest_East_06", "RestBench", 1, MapZone.OUTSKIRTS, true, "Oro"),
                new Bench("Camp", "Edge", "Deepnest_East_13", "RestBench", 1, MapZone.OUTSKIRTS, true, "Camp"),
                new Bench("Colosseum", "Edge", "Room_Colosseum_02", "RestBench", 1, MapZone.COLOSSEUM, true, "Fool"),
                new Bench("Hive", "Edge", "Hive_01", "RestBench", 1, MapZone.HIVE),

                new Bench("Dark Room", "Peak", "Mines_29", "RestBench", 1, MapZone.MINES),
                new Bench("Crystal Guardian", "Peak", "Mines_18", "RestBench", 1, MapZone.MINES),

                new Bench("Stag", "Grounds", "RestingGrounds_09", "RestBench", 1, MapZone.RESTING_GROUNDS),
                new Bench("Grey Mourner", "Grounds", "RestingGrounds_12", "RestBench", 1, MapZone.RESTING_GROUNDS),

                new Bench("Cornifer", "Gardens", "Fungus1_24", "RestBench", 1, MapZone.ROYAL_GARDENS, true, "Guardian"),
                new Bench("Toll", "Gardens", "Fungus3_50", "RestBench", 1, MapZone.ROYAL_GARDENS),
                new Bench("Stag", "Gardens", "Fungus3_40", "RestBench", 1, MapZone.ROYAL_GARDENS),

                new Bench("Entrance", "Palace", "White_Palace_01", "WhiteBench", 1, MapZone.WHITE_PALACE, true, "White"),
                new Bench("Atrium", "Palace", "White_Palace_03_hub", "WhiteBench", 1, MapZone.WHITE_PALACE),
                new Bench("Balcony", "Palace", "White_Palace_06", "RestBench", 1, MapZone.WHITE_PALACE),

                new Bench("Upper Tram", "Tram", "Room_Tram_RG", "RestBench", 1, MapZone.TRAM_UPPER),
                new Bench("Lower Tram", "Tram", "Room_Tram", "RestBench", 1, MapZone.TRAM_LOWER, true, "Tram")
            };
        }

    }
}
