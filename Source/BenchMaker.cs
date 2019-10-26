using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Benchwarp
{
    public static class BenchMaker
    {
        public static GameObject DeployedBench;

        public const string DEPLOYED_BENCH_RESPAWN_MARKER_NAME = "DeployedBench";

        private static GameObject ExtraSprite;

        private static GameObject _rightBench;
        private static GameObject RightBench => GameObject.Instantiate(_rightBench);

        private static GameObject _leftBench;
        private static GameObject LeftBench => GameObject.Instantiate(_leftBench);

        private static GameObject _ornateBench;
        private static GameObject OrnateBench => GameObject.Instantiate(_ornateBench);

        private static GameObject _boneBench;
        private static GameObject BoneBench => GameObject.Instantiate(_boneBench);

        private static GameObject _stoneBench;
        private static GameObject StoneBench => GameObject.Instantiate(_stoneBench);

        private static GameObject _shrineBench;
        private static GameObject ShrineBench => GameObject.Instantiate(_shrineBench);

        private static GameObject _archiveBench;
        private static GameObject ArchiveBench => GameObject.Instantiate(_archiveBench);

        private static GameObject _corpseBench;
        private static GameObject CorpseBench => GameObject.Instantiate(_corpseBench);

        private static GameObject _mantisBench;
        private static GameObject MantisBench => GameObject.Instantiate(_mantisBench);

        private static GameObject _simpleBench;
        private static GameObject SimpleBench => GameObject.Instantiate(_simpleBench);

        private static GameObject _tiltedBench;
        private static GameObject TiltedBench => GameObject.Instantiate(_tiltedBench);

        private static GameObject _wideBench;
        private static GameObject WideBench => GameObject.Instantiate(_wideBench);

        private static GameObject _beastBench;
        private static GameObject BeastBench => GameObject.Instantiate(_beastBench);

        private static GameObject _campBench;
        private static GameObject CampBench => GameObject.Instantiate(_campBench);

        private static GameObject _campSprite;
        private static GameObject CampSprite => GameObject.Instantiate(_campSprite);

        private static GameObject _foolBench;
        private static GameObject FoolBench => GameObject.Instantiate(_foolBench);

        private static GameObject _guardianBench;
        private static GameObject GuardianBench => GameObject.Instantiate(_guardianBench);

        private static GameObject _guardianSprite;
        private static GameObject GuardianSprite => GameObject.Instantiate(_guardianSprite);

        private static GameObject _tramBench;
        private static GameObject TramBench => GameObject.Instantiate(_tramBench);

        private static GameObject _matoBench;
        private static GameObject MatoBench => GameObject.Instantiate(_matoBench);

        private static GameObject _oroBench;
        private static GameObject OroBench => GameObject.Instantiate(_oroBench);

        private static GameObject _sheoBench;
        private static GameObject SheoBench => GameObject.Instantiate(_sheoBench);

        private static GameObject _whiteBench;
        private static GameObject WhiteBench => GameObject.Instantiate(_whiteBench);

        private static GameObject _blackBench;
        private static GameObject BlackBench => GameObject.Instantiate(_blackBench);

        public static readonly List<string> Styles = new List<string>
        {
            "Left",
            "Right",
            "Bone",
            "Ornate",
            "Stone",
            "Shrine",
            "Archive",
            "Corpse",
            "Mantis",
            "Simple",
            "Tilted",
            "Wide",
            "Beast",
            "Camp",
            "Fool",
            "Garden",
            "Tram",
            "Mato",
            "Oro",
            "Sheo",
            "White",
            "Black"
        };

        public static void GetPrefabs(Dictionary<string, Dictionary<string, GameObject>> objects)
        {
            if (objects == null) return; //happens if mod is reloaded

            _rightBench = objects["Crossroads_30"]["RestBench"];
            _leftBench = objects["Town"]["RestBench"];
            _ornateBench = objects["Crossroads_04"]["RestBench"];
            _boneBench = objects["Crossroads_ShamanTemple"]["BoneBench"];
            _stoneBench = objects["Fungus1_37"]["RestBench"];
            _shrineBench = objects["Room_Slug_Shrine"]["RestBench"];
            _archiveBench = objects["Fungus3_archive"]["RestBench"];
            _corpseBench = objects["Fungus2_26"]["RestBench"];
            _mantisBench = objects["Fungus2_31"]["RestBench"];
            _simpleBench = objects["Ruins_Bathhouse"]["RestBench"];
            _tiltedBench = objects["Waterways_02"]["RestBench"];
            _wideBench = objects["GG_Atrium"]["RestBench"];
            _beastBench = objects["Deepnest_Spider_Town"]["RestBench Return"];
            _campBench = objects["Deepnest_East_13"]["RestBench"];
            _campSprite = objects["Deepnest_East_13"]["outskirts__0003_camp"];
            _foolBench = objects["Room_Colosseum_02"]["RestBench"];
            _guardianBench = objects["Fungus1_24"]["RestBench"];
            _guardianSprite = objects["Fungus1_24"]["guardian_bench"];
            _tramBench = objects["Room_Tram"]["RestBench"];
            _matoBench = objects["Room_nailmaster"]["RestBench"];
            _oroBench = objects["Deepnest_East_06"]["RestBench"];
            _sheoBench = objects["Fungus1_15"]["RestBench"];
            _whiteBench = objects["White_Palace_01"]["WhiteBench"];
            _blackBench = objects["Room_Final_Boss_Atrium"]["RestBench"];
        }

        private static Vector3 GetAdjust()
        {
            switch (Benchwarp.instance.GlobalSettings.benchStyle)
            {
                default:
                    return new Vector3(0f, -.68f, 0f);
                case "Bone":
                    return new Vector3(0f, -.7f, 0f);
                case "Beast":
                    return new Vector3(0f, -.5f, 0f);
                case "Fool":
                    return new Vector3(0f, -0.4f, 0f);
                case "Oro":
                    return new Vector3(0f, -.8f, 0.18f);
                case "White":
                    return new Vector3(0f, -.13f, 0f);
                case "Black":
                    return new Vector3(0f, -.8f, 0f);
            }
        }

        public static void DestroyBench(bool DontDeleteData = false)
        {
            if (DeployedBench != null) GameObject.Destroy(DeployedBench);
            if (ExtraSprite != null) GameObject.Destroy(ExtraSprite);

            if (DontDeleteData) return;

            Benchwarp.instance.Settings.benchDeployed = false;
            Benchwarp.instance.Settings.atDeployedBench = false;
        }

        public static void MakeBench()
        {
            if (!Benchwarp.instance.Settings.benchDeployed) return;

            Vector3 position = new Vector3(Benchwarp.instance.Settings.benchX, Benchwarp.instance.Settings.benchY, 0.02f) + GetAdjust();

            switch (Benchwarp.instance.GlobalSettings.benchStyle)
            {
                default:
                case "Right": DeployedBench = RightBench;
                    break;
                case "Left": DeployedBench = LeftBench;
                    break;
                case "Ornate": DeployedBench = OrnateBench;
                    break;
                case "Bone": DeployedBench = BoneBench;
                    break;
                case "Stone": DeployedBench = StoneBench;
                    break;
                case "Shrine": DeployedBench = ShrineBench;
                    break;
                case "Archive": DeployedBench = ArchiveBench;
                    break;
                case "Corpse": DeployedBench = CorpseBench;
                    break;
                case "Mantis": DeployedBench = MantisBench;
                    break;
                case "Simple": DeployedBench = SimpleBench;
                    break;
                case "Tilted": DeployedBench = TiltedBench;
                    break;
                case "Wide": DeployedBench = WideBench;
                    break;
                case "Beast": DeployedBench = BeastBench;
                    break;
                case "Camp": DeployedBench = CampBench;
                    break;
                case "Fool": DeployedBench = FoolBench;
                    break;
                case "Guardian": DeployedBench = GuardianBench;
                    break;
                case "Tram": DeployedBench = TramBench;
                    break;
                case "Mato": DeployedBench = MatoBench;
                    break;
                case "Oro": DeployedBench = OroBench;
                    break;
                case "Sheo": DeployedBench = SheoBench;
                    break;
                case "White": DeployedBench = WhiteBench;
                    break;
                case "Black": DeployedBench = BlackBench;
                    break;
            }
            DeployedBench.transform.position = position;
            DeployedBench.SetActive(true);
            if (Benchwarp.instance.GlobalSettings.benchStyle == "Guardian")
            {
                ExtraSprite = GuardianSprite;
                ExtraSprite.transform.position = position + new Vector3(0f, 0.5f, 0f);
                ExtraSprite.SetActive(true);
            }
            if (Benchwarp.instance.GlobalSettings.benchStyle == "Camp")
            {
                ExtraSprite = CampSprite;
                ExtraSprite.transform.position = position + new Vector3(0f, -0.5f, 0f);
                ExtraSprite.SetActive(true);
            }
            DeployedBench.SetActive(true);
            DeployedBench.name = DEPLOYED_BENCH_RESPAWN_MARKER_NAME;

            if (Benchwarp.instance.GlobalSettings.Noninteractive)
            {
                var actions = DeployedBench.LocateMyFSM("Bench Control").FsmStates.First(s => s.Name == "Idle").Actions.ToList();
                actions.RemoveAt(1); // never recognizes player as being in range
                DeployedBench.LocateMyFSM("Bench Control").FsmStates.First(s => s.Name == "Idle").Actions = actions.ToArray();
            }
        }

        public static void TryToDeploy(Scene arg0, Scene arg1)
        {
            if (Benchwarp.instance.Settings.benchDeployed && arg1.name == Benchwarp.instance.Settings.benchScene)
            {
                MakeBench();
            }
        }

        private static readonly List<string> darkRooms = new List<string> { "Fungus1_35", "Mines_07", "Mines_33", "Cliffs_04", "Deepnest_34", "Deepnest_35", "Deepnest_39", "Deepnest_41", "Deepnest_42" };
        public static bool Blacklist()
        {
            if (!PlayerData.instance.hasLantern && darkRooms.Contains(GameManager.instance.sceneName)) return false;
            return !HeroController.instance.gameObject.LocateMyFSM("Dream Nail").FsmVariables.FindFsmBool("Dream Warp Allowed").Value;
        }

    }
}
