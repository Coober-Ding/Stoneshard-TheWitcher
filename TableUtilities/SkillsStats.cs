using ModShardLauncher;


namespace TheWitcher
{
    public static partial class TableUtils
    {
        public enum SkillsStatsHook
        {
            BASIC,
            RANGED,
            SWORDS,
            TWOHANDEDSWORDS,
            TWOHANDEDMACES,
            TWOHANDEDAXES,
            AXES,
            MACES,
            STAVES,
            SHIELDS,
            DAGGERS,
            DUALWIELDING,
            SPEARS,
            COMBAT,
            ATHLETICS,
            SURVIVAL,
            PYRO,
            GEO,
            ELECTRO,
            ARMOR,
            MAGICMASTERY,
            UNDEAD,
            PROLOGUESTATUES,
            PROLOGUEARCHON,
            PROSELYTES,
            BRIGANDS,
            ANCIENTTROLL,
            BEASTS,
            MANTICORE
        }

        public enum SkillsStatsTarget
        {
            NoTarget,
            TargetObject,
            TargetPoint,
            TargetArea,
            TargetAlly
        }

        public enum SkillsStatsPattern
        {
            normal,
            five,
            line,
            circle,
            pyramid,
            pyramid_shift
        }

        public enum SkillsStatsValidator
        {
            none,
            AVOID_TILEMARKS,
            DASH
        }

        public enum SkillsStatsClass
        {
            skill,
            spell,
            attack,
        }

        public enum SkillsStatsBranch
        {
            none, // For some reason the string none has to be written, rather than leaving the field empty. Inconsistent but it is what it is.
            ranged,
            sword,
            two_handed_sword,
            two_handed_mace,
            two_handed_axe,
            axe,
            mace,
            staff,
            shield,
            dagger,
            dual,
            spear,
            combat,
            athletic,
            pyromancy,
            geomancy,
            electromancy,
            armor,
            magic_mastery,
            necromancy,
            sanguimancy
        }

        public enum SkillsStatsMetacategory
        {
            none,
            weapon,
            utility
        }

        private static readonly Dictionary<SkillsStatsHook, string> HookMap = new Dictionary<SkillsStatsHook, string>
        {
            { SkillsStatsHook.BASIC, "BASIC" },
            { SkillsStatsHook.RANGED, "RANGED" },
            { SkillsStatsHook.SWORDS, "SWORDS" },
            { SkillsStatsHook.TWOHANDEDSWORDS, "2H SWORDS" },
            { SkillsStatsHook.TWOHANDEDMACES, "2H MACES" },
            { SkillsStatsHook.TWOHANDEDAXES, "2H AXES" },
            { SkillsStatsHook.AXES, "AXES" },
            { SkillsStatsHook.MACES, "MACES" },
            { SkillsStatsHook.STAVES, "STAVES" },
            { SkillsStatsHook.SHIELDS, "SHIELDS" },
            { SkillsStatsHook.DAGGERS, "DAGGERS" },
            { SkillsStatsHook.DUALWIELDING, "DUAL WIELDING" },
            { SkillsStatsHook.SPEARS, "SPEARS" },
            { SkillsStatsHook.COMBAT, "COMBAT" },
            { SkillsStatsHook.ATHLETICS, "ATHLETICS" },
            { SkillsStatsHook.SURVIVAL, "SURVIVAL" },
            { SkillsStatsHook.PYRO, "PYRO" },
            { SkillsStatsHook.GEO, "GEO" },
            { SkillsStatsHook.ELECTRO, "ELECTRO" },
            { SkillsStatsHook.ARMOR, "ARMOR" },
            { SkillsStatsHook.MAGICMASTERY, "MAGIC MASTERY" },
            { SkillsStatsHook.UNDEAD, "UNDEAD" },
            { SkillsStatsHook.PROLOGUESTATUES, "PROLOGUE STATUES" },
            { SkillsStatsHook.PROLOGUEARCHON, "PROLOGUE ARCHON" },
            { SkillsStatsHook.PROSELYTES, "PROSELYTES" },
            { SkillsStatsHook.BRIGANDS, "BRIGANDS" },
            { SkillsStatsHook.ANCIENTTROLL, "ANCIENT TROLL" },
            { SkillsStatsHook.BEASTS, "BEASTS" },
            { SkillsStatsHook.MANTICORE, "MANTICORE" }
        };

        private static readonly Dictionary<SkillsStatsTarget, string> TargetMap = new Dictionary<SkillsStatsTarget, string>
        {
            { SkillsStatsTarget.NoTarget, "No Target" },
            { SkillsStatsTarget.TargetObject, "Target Object" },
            { SkillsStatsTarget.TargetPoint, "Target Point" },
            { SkillsStatsTarget.TargetArea, "Target Area" },
            { SkillsStatsTarget.TargetAlly, "Target Ally" }
        };

        private static readonly Dictionary<SkillsStatsValidator, string> ValidatorMap = new Dictionary<SkillsStatsValidator, string>
        {
            { SkillsStatsValidator.none, "" },
            { SkillsStatsValidator.AVOID_TILEMARKS, "AVOID_TILEMARKS" },
            { SkillsStatsValidator.DASH, "DASH" }
        };

        private static readonly Dictionary<SkillsStatsMetacategory, string> MetacategoryMap = new Dictionary<SkillsStatsMetacategory, string>
        {
            { SkillsStatsMetacategory.none, "" },
            { SkillsStatsMetacategory.weapon, "weapon" },
            { SkillsStatsMetacategory.utility, "utility" }
        };

        private static readonly Dictionary<SkillsStatsBranch, string> BranchMap = new Dictionary<SkillsStatsBranch, string>
        {
            { SkillsStatsBranch.none, "none" },
            { SkillsStatsBranch.ranged, "ranged" },
            { SkillsStatsBranch.sword, "sword" },
            { SkillsStatsBranch.two_handed_sword, "2hsword" },
            { SkillsStatsBranch.two_handed_mace, "2hmace" },
            { SkillsStatsBranch.two_handed_axe, "2haxe" },
            { SkillsStatsBranch.axe, "axe" },
            { SkillsStatsBranch.mace, "mace" },
            { SkillsStatsBranch.staff, "staff" },
            { SkillsStatsBranch.shield, "shield" },
            { SkillsStatsBranch.dagger, "dagger" },
            { SkillsStatsBranch.dual, "dual" },
            { SkillsStatsBranch.spear, "spear" },
            { SkillsStatsBranch.combat, "combat" },
            { SkillsStatsBranch.athletic, "athletic" },
            { SkillsStatsBranch.pyromancy, "pyromancy" },
            { SkillsStatsBranch.geomancy, "geomancy" },
            { SkillsStatsBranch.electromancy, "electromancy" },
            { SkillsStatsBranch.armor, "armor" },
            { SkillsStatsBranch.magic_mastery, "magic_mastery" },
            { SkillsStatsBranch.necromancy, "necromancy" },
            { SkillsStatsBranch.sanguimancy, "sanguimancy" }
        };

        public static void InjectTableSkillsStats(
            SkillsStatsHook hook,
            string id,
            string? Object = null,
            SkillsStatsTarget Target = SkillsStatsTarget.NoTarget,
            string Range = "0",
            ushort KD = 0,
            ushort MP = 0,
            ushort Reserv = 0,
            ushort Duration = 0,
            byte AOE_Lenght = 0,
            byte AOE_Width = 0,
            bool is_movement = false,
            SkillsStatsPattern Pattern = SkillsStatsPattern.normal,
            SkillsStatsValidator Validators = SkillsStatsValidator.none,
            SkillsStatsClass Class = SkillsStatsClass.skill,
            bool Bonus_Range = false, // could be byte ? Not sure as only values are 0 and 1
            string? Starcast = null,
            string Branch = "none",
            bool is_knockback = false,
            bool Crime = false,
            SkillsStatsMetacategory metacategory = SkillsStatsMetacategory.none,
            short FMB = 0,
            string AP = "x",
            bool Attack = false,
            bool Stance = false,
            bool Charge = false,
            bool Maneuver = false,
            bool Spell = false
            )
        {
            // Table filename
            const string tableName = "gml_GlobalScript_table_skills_stats";

            // Load table if it exists
            List<string> table = Msl.ThrowIfNull(ModLoader.GetTable(tableName));

            // Prepare line
            string newline = $"{id};{Object};{GetEnumValue(Target, TargetMap)};{Range};{KD};{MP};{Reserv};{Duration};{AOE_Lenght};{AOE_Width};{(is_movement ? "1" : "0")};{Pattern};{GetEnumValue(Validators, ValidatorMap)};{Class};{(Bonus_Range ? "1" : "0")};{Starcast};{Branch};{(is_knockback ? "1" : "0")};{(Crime ? "1" : "")};{GetEnumValue(metacategory, MetacategoryMap)};{FMB};{AP};{(Attack ? "1" : "")};{(Stance ? "1" : "")};{(Charge ? "1" : "")};{(Maneuver ? "1" : "")};{(Spell ? "1" : "")};";

            // Find Hook
            string hookStr = "// " + GetEnumValue(hook, HookMap);
            (int ind, string? foundLine) = table.Enumerate().FirstOrDefault(x => x.Item2.Contains(hookStr));

            // Add line to table
            if (foundLine != null)
            {
                table.Insert(ind + 1, newline);
                ModLoader.SetTable(table, tableName);
                //Console.WriteLine($"Injected Skill Stat {id} into {tableName} under {hook}");
            }
            else
            {
                //Console.WriteLine($"Cannot find Hook {hook} in table {tableName}");
                throw new Exception($"Hook {hook} not found in table {tableName}");
            }
        }
    }
}
