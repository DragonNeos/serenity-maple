using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity
{
    class Constants
    {
        #region General Constants
        public static readonly ushort MajorVersion = 143;
        public static readonly string MinorVersion = "2";
        public static readonly byte Locale = 8;

        public static readonly bool Redirector = false;

        public static readonly string EventMessage = "Welcome to Serenity.";
        public static readonly string[] WorldNames = new string[]
        {
            "Scania", "Bera", "Broa",
            "Windia", "Khaini", "Bellocan",
            "Mardia", "Kradia", "Yellonde",
            "Demethos", "Galicia", "El Nido",
            "Zenith", "Arcania", "Chaos",
            "Nova", "Renegades"
        };
        #endregion

        #region Time Constants
        public static int GetCurrentDate()
        {
            string time = CurrentReadable_Time();
            return Convert.ToInt32((new StringBuilder(time.Substring(0, 4))).Append(time.Substring(5, 2)).Append(time.Substring(8, 2)).Append(time.Substring(11, 2)).ToString());
        }

        public static string CurrentReadable_Time()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        #region Item Constants
        public static class ItemConstants
        {
            public static bool isEquip(int itemId)
            {
                return (itemId / 1000000) == 1;
            }

            public static bool isPet(int itemId)
            {
                return (itemId / 1000000) == 5;
            }
        }
        #endregion

        #region Job Constants
        public static class JobConstants
        {
            public static byte GetSubcategory(short job)
            {
                if (isJett(job))
                {
                    return 10;
                }
                else if (isDualBlade(job))
                {
                    return 1;
                }
                else if (isCannon(job) || job == 1)
                {
                    return 2;
                }
                else if (job != 0 && job != 400)
                {
                    return 0;
                }

                return 0;
            }

            public static bool isKOC(int job)
            {
                return job >= 1000 && job < 2000;
            }

            public static bool isEvan(int job)
            {
                return job == 2001 || (job / 100 == 22);
            }

            public static bool isMercedes(int job)
            {
                return job == 2002 || (job / 100 == 23);
            }

            public static bool isJett(int job)
            {
                return job == 508 || (job / 10 == 57);
            }

            public static bool isPhantom(int job)
            {
                return job == 2003 || (job / 100 == 24);
            }

            public static bool isWildHunter(int job)
            {
                return job == 3000 || (job >= 3300 && job <= 3312);
            }

            public static bool isMechanic(int job)
            {
                return true;
            }

            public static bool isSeparatedSp(int job)
            {
                return isEvan(job) || isResist(job) || isMercedes(job) || isJett(job) || isPhantom(job) || isMihile(job) || isNova(job) || isAngelicBuster(job) || isKaiser(job) || isLuminous(job) || isHayato(job) || isKanna(job);
            }

            public static bool isDualBladeNoSP(int job)
            {
                return job == 430 ? true : job == 432 ? true : false;
            }

            public static bool isDemonSlayer(int job)
            {
                return job == 3001 || (job >= 3100 && job <= 3112);
            }

            public static bool isAran(int job)
            {
                return job >= 2000 && job <= 2112 && job != 2001 && job != 2002 && job != 2003;
            }

            public static bool isResist(int job)
            {
                return job / 1000 == 3;
            }

            public static bool isAdventurer(int job)
            {
                return job >= 0 && job < 1000;
            }

            public static bool isCannon(int job)
            {
                return job == 1 || job == 501 || (job >= 530 && job <= 532);
            }

            public static bool isDualBlade(int job)
            {
                return (job >= 430 && job <= 434);
            }

            public static bool isMihile(int job)
            {
                return job == 5000 || (job >= 5100 && job <= 5112);
            }

            public static bool isLuminous(int job)
            {
                return job == 2004 || (job >= 2700 && job <= 2712);
            }

            public static bool isKaiser(int job)
            {
                return job == 6000 || (job >= 6100 && job <= 6112);
            }

            public static bool isAngelicBuster(int job)
            {
                return job == 6001 || (job >= 6500 && job <= 6512);
            }

            public static bool isNova(int job)
            {
                return job / 1000 == 6;
            }

            public static bool isXenon(int job)
            {
                return job == 3002 || (job >= 3600 && job <= 3612);
            }

            public static bool isHayato(int job)
            {
                return job == 4001 || (job >= 4100 && job <= 4112);
            }

            public static bool isKanna(int job)
            {
                return job == 4002 || (job >= 4200 && job <= 4212);
            }

            public static bool isDemonAvenger(int job)
            {
                return job == 3001 || job == 3101 || (job >= 3120 && job <= 3122);
            }
        }
        #endregion

        #region Skill Constants
        public static class SkillConstants
        {
            public static int getNumSteal(int jobNum)
            {
                switch (jobNum)
                {
                    case 1:
                        return 4;
                    case 2:
                        return 4;
                    case 3:
                        return 3;
                    case 4:
                        return 2;
                }

                return 0;
            }
        }
        #endregion

        #region Character Creation Information
        public static class CharacterCreation
        {

            public static bool hasHairColor(int jobType)
            {
                return jobType == (int)CreateTypes.Resistance ||
                jobType == (int)CreateTypes.Explorer ||
                jobType == (int)CreateTypes.KnightsOfCygnus ||
                jobType == (int)CreateTypes.Aran ||
                jobType == (int)CreateTypes.Evan ||
                jobType == (int)CreateTypes.DualBlade ||
                jobType == (int)CreateTypes.Mihile ||
                jobType == (int)CreateTypes.Cannoneer ||
                jobType == (int)CreateTypes.Xenon ||
                jobType == (int)CreateTypes.Hayato ||
                jobType == (int)CreateTypes.Kanna;
            }

            public static bool hasSkinColor(int jobType)
            {
                return jobType == (int)CreateTypes.Resistance ||
                jobType == (int)CreateTypes.Explorer ||
                jobType == (int)CreateTypes.KnightsOfCygnus ||
                jobType == (int)CreateTypes.Aran ||
                jobType == (int)CreateTypes.Evan ||
                jobType == (int)CreateTypes.DualBlade ||
                jobType == (int)CreateTypes.Mihile ||
                jobType == (int)CreateTypes.Luminous ||
                jobType == (int)CreateTypes.Kaiser ||
                jobType == (int)CreateTypes.AngelicBuster ||
                jobType == (int)CreateTypes.Cannoneer ||
                jobType == (int)CreateTypes.Xenon ||
                jobType == (int)CreateTypes.Hayato ||
                jobType == (int)CreateTypes.Kanna;
            }

            public static bool hasFaceMark(int jobType)
            {
                return jobType == (int)CreateTypes.Demon ||
                    jobType == (int)CreateTypes.Xenon;
            }

            public static bool hasHat(int jobType)
            {
                return jobType == (int)CreateTypes.Hayato ||
                    jobType == (int)CreateTypes.Kanna;
            }

            public static bool hasBottom(int jobType)
            {
                return jobType == (int)CreateTypes.Resistance ||
                jobType == (int)CreateTypes.Explorer ||
                jobType == (int)CreateTypes.KnightsOfCygnus ||
                jobType == (int)CreateTypes.Aran ||
                jobType == (int)CreateTypes.Evan ||
                jobType == (int)CreateTypes.DualBlade ||
                jobType == (int)CreateTypes.Mihile ||
                jobType == (int)CreateTypes.Cannoneer;
            }

            public static bool hasCape(int jobType)
            {
                return jobType == (int)CreateTypes.Phantom ||
                    jobType == (int)CreateTypes.Luminous ||
                    jobType == (int)CreateTypes.Jett;
            }

            public static short GetJobByType(int jobType)
            {
                switch (jobType)
                {
                    case 0:
                        return (short)Job.Citizen;
                    case 1:
                        return (short)Job.Beginner;
                    case 2:
                        return (short)Job.Noblesse;
                    case 3:
                        return (short)Job.Legend;
                    case 4:
                        return (short)Job.Farmer;
                    case 5:
                        return (short)Job.Mercedes;
                    case 6:
                        return (short)Job.Demon;
                    case 7:
                        return (short)Job.Phantom;
                    case 8:
                        return (short)Job.Beginner; // Dual Blade is Beginner.
                    case 9:
                        return (short)Job.Mihile;
                    case 10:
                        return (short)Job.Luminous;
                    case 11:
                        return (short)Job.Kaiser;
                    case 12:
                        return (short)Job.AngelicBuster;
                    case 13:
                        return (short)Job.Beginner; // Cannoneer is Beginner.
                    case 14:
                        return (short)Job.Xenon;
                    case 15:
                        return (short)Job.Beginner; // Jett is Beginner.
                    case 16:
                        return (short)Job.Hayato;
                    case 17:
                        return (short)Job.Kanna;
                }

                return (short)Job.Beginner;
            }

            public static int GetMapByType(int jobType)
            {
                switch (jobType)
                {
                    case 0:
                        return 931000000;
                    case 1:
                        return 0;
                    case 2:
                        return 130030000;
                    case 3:
                        return 913000000;
                    case 4:
                        return 900010000;
                    case 5:
                        return 910150000;
                    case 6:
                        return 931050310;
                    case 7:
                        return 915000000;
                    case 8:
                        return 103050900;
                    case 9:
                        return 913070000;
                    default:
                        return 0;
                }
            }

            public enum CreateTypes : int
            {
                Resistance = 0,
                Explorer,
                KnightsOfCygnus,
                Aran,
                Evan,
                Mercedes,
                Demon,
                Phantom,
                DualBlade,
                Mihile,
                Luminous,
                Kaiser,
                AngelicBuster,
                Cannoneer,
                Xenon,
                Jett,
                Hayato,
                Kanna
            }
        }
        #endregion

        #region Job Information
        public enum Job : short
        {
            Beginner = 0,

            Swordman = 100,
            Fighter = 110,
            Crusader = 111,
            Hero = 112,
            Page = 120,
            WhiteKnight = 121,
            Paladin = 122,
            Spearman = 130,
            DragonKnight = 131,
            DarkKnight = 132,

            Magician = 200,
            FirePoisonMagician = 210,
            FirePoisonWizzard = 211,
            FirePoisonArch = 112,
            IceLightningMagician = 220,
            IceLightningWizzard = 221,
            IceLightningArch = 222,
            Cleric = 230,
            Priest = 231,
            Bishop = 232,

            Archer = 300,
            Hunter = 310,
            Ranger = 311,
            Bowmaster = 312,
            Crossbowman = 320,
            Sniper = 321,
            Marksman = 322,

            Rogue = 400,
            Assassin = 410,
            Hermit = 411,
            NightLord = 412,
            Bandit = 420,
            ChiefBandit = 421,
            Shadower = 422,
            BladeRecruit = 430,
            BladeAcolyte = 431,
            BladeSpecialist = 432,
            BladeLord = 433,
            BladeMaster = 434,

            Pirate = 500,
            Cannoneer1 = 501,
            Jett1 = 508,
            Brawler = 510,
            Marauder = 511,
            Buccaneer = 512,
            Gunslinger = 520,
            Outlaw = 521,
            Corsair = 522,

            Cannoneer2 = 530,
            Cannoneer3 = 531,
            Cannoneer4 = 532,
            Jett2 = 570,
            Jett3 = 571,
            Jett4 = 572,

            Manager = 800, //?

            GM = 900,
            SuperGM = 910,

            Noblesse = 1000,

            DawnWarrior1 = 1100,
            DawnWarrior2 = 1110,
            DawnWarrior3 = 1111,
            DawnWarrior4 = 1112,

            BlazeWizzard1 = 1200,
            BlazeWizzard2 = 1210,
            BlazeWizzard3 = 1211,
            BlazeWizzard4 = 1212,

            WindArcher1 = 1300,
            WindArcher2 = 1310,
            WindArcher3 = 1311,
            WindArcher4 = 1312,

            NightWalker1 = 1400,
            NightWalker2 = 1410,
            NightWalker3 = 1411,
            NightWalker4 = 1412,

            ThunderBreaker1 = 1500,
            ThunderBreaker2 = 1510,
            ThunderBreaker3 = 1511,
            ThunderBreaker4 = 1512,

            Legend = 2000,
            Aran1 = 2100,
            Aran2 = 2110,
            Aran3 = 2111,
            Aran4 = 2112,

            Farmer = 2001,
            Evan1 = 2201,
            Evan2 = 2210,
            Evan3 = 2211,
            Evan4 = 2212,
            Evan5 = 2213,
            Evan6 = 2214,
            Evan7 = 2215,
            Evan8 = 2216,
            Evan9 = 2217,
            Evan10 = 2218,

            Mercedes = 2002,
            Mercedes1 = 2300,
            Mercedes2 = 2310,
            Mercedes3 = 2311,
            Mercedes4 = 2312,

            Phantom = 2003,
            Phantom1 = 2400,
            Phantom2 = 2410,
            Phantom3 = 2411,
            Phantom4 = 2412,

            Luminous = 2004,
            Luminous1 = 2700,
            Luminous2 = 2710,
            Luminous3 = 2711,
            Luminous4 = 2712,

            Citizen = 3000,
            Demon = 3001,
            Xenon = 3002,

            DemonSlayer1 = 3100,
            DemonSlayer2 = 3110,
            DemonSlayer3 = 3111,
            DemonSlayer4 = 3112,

            DemonAvenger1 = 3101,
            DemonAvenger2 = 3120,
            DemonAvenger3 = 3121,
            DemonAvenger4 = 3122,

            BattleMage1 = 3200,
            BattleMage2 = 3210,
            BattleMage3 = 3211,
            BattleMage4 = 3212,

            WildHunter1 = 3300,
            WildHunter2 = 3310,
            WildHunter3 = 3311,
            WildHunter4 = 3312,

            Mechanic1 = 3500,
            Mechanic2 = 3510,
            Mechanic3 = 3511,
            Mechanic4 = 3512,

            Xenon1 = 3600,
            Xenon2 = 3610,
            Xenon3 = 3611,
            Xenon4 = 3612,

            Hayato = 4001,
            Kanna = 4002,
            Hayato1 = 4100,
            Hayato2 = 4110,
            Hayato3 = 4111,
            Hayato4 = 4112,
            Kanna1 = 4200,
            Kanna2 = 4210,
            Kanna3 = 4211,
            Kanna4 = 4212,

            Mihile = 5000,
            Mihile1 = 5100,
            Mihile2 = 5110,
            Mihile3 = 5111,
            Mihile4 = 5112,

            Kaiser = 6000,
            AngelicBuster = 6001,
            Kaiser1 = 6100,
            Kaiser2 = 6110,
            Kaiser3 = 6111,
            Kaiser4 = 6112,
            AngelicBuster1 = 6500,
            AngelicBuster2 = 6510,
            AngelicBuster3 = 6511,
            AngelicBuster4 = 6512,
        }
        #endregion
    }
}
