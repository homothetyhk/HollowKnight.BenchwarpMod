using Modding;

namespace Benchwarp
{
    public class SaveSettings : IModSettings
    {
        public bool benchDeployed
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public float benchX
        {
            get => GetFloat(0f);
            set => SetFloat(value);
        }

        public float benchY
        {
            get => GetFloat(0f);
            set => SetFloat(value);
        }

        public string benchScene
        {
            get => GetString(null);
            set => SetString(value);
        }



        public string benchName
        {
            get => GetString(null);
            set => SetString(value);
        }

        public bool hasVisitedDirtmouth
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedMato
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedXRHotSprings
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedXRStag
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedSalubra
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedAncestralMound
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedBlackEggTemple
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedWaterfall
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedStoneSanctuary
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedGPToll
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedGPStag
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedLakeofUnn
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedSheo
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedTeachersArchives
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedQueensStation
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedLegEater
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedBretta
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedMantisVillage
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedQuirrel
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedCoTToll
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedCityStorerooms
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedWatchersSpire
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedKingsStation
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedPleasureHouse
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedWaterways
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedGodhome
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedHallofGods
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedDNHotSprings
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedFailedTramway
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedBeastsDen
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedABToll
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedABStag
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedOro
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedCamp
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedColosseum
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedHive
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedDarkRoom
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedCrystalGuardian
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedQGCornifer
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedQGToll
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedQGStag
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedWPEntrance
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedWPAtrium
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedWPBalcony
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedUpperTram
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedLowerTram
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedRGStag
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool hasVisitedGreyMourner
        {
            get => GetBool(false);
            set => SetBool(value);
        }

        public static SaveSettings _instance;
    }
    public class GlobalSettings : IModSettings
    {
        public bool WarpOnly
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool UnlockAllBenches
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool ShowScene
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool SwapNames
        {
            get => GetBool(false);
            set => SetBool(value);
        }
        public bool EnableDeploy
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public string benchStyle
        {
            get => GetString("Right");
            set => SetString(value);
        }

        public bool DeployCooldown
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool Noninteractive
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool NoMidAirDeploy
        {
            get => GetBool(true);
            set => SetBool(value);
        }

        public bool BlacklistRooms
        {
            get => GetBool(true);
            set => SetBool(value);
        }
    }
}
