using ModShardLauncher;

namespace TheWitcher
{
    public static partial class TableUtils
    {
        public enum WeaponsTier
        {
            Tier1,
            Tier2,
            Tier3,
            Tier4,
            Tier5
        }

        public enum WeaponsSlot
        {
            sword,
            axe,
            mace,
            dagger,
            twohandedsword,
            spear,
            twohandedaxe,
            twohandedmace,
            bow,
            crossbow,
            sling,
            twohandedstaff,
            chain,
            lute
        }

        public enum WeaponsRarity
        {
            Common,
            Unique
            // Legendary // Doesn't seem to be used post-RtR 
        }

        public enum WeaponsMaterial
        {
            wood,
            metal,
            leather
        }

        public enum WeaponsTags
        {
            // SPECIAL
            unique,
            magic,
            special,
            specialexc,
            WIP,

            // ALDOR
            aldor,
            aldorcommon,
            aldoruncommon,
            aldorrare,
            aldormagic,

            // FOREIGN
            fjall,
            elven,
            skadia,
            nistra
        }

        private static readonly Dictionary<WeaponsTier, string> WeaponsTierMap = new Dictionary<WeaponsTier, string>
        {
            { WeaponsTier.Tier1, "1" },
            { WeaponsTier.Tier2, "2" },
            { WeaponsTier.Tier3, "3" },
            { WeaponsTier.Tier4, "4" },
            { WeaponsTier.Tier5, "5" }
        };

        private static readonly Dictionary<WeaponsSlot, string> WeaponsSlotMap = new Dictionary<WeaponsSlot, string>
        {
            { WeaponsSlot.sword, "sword" },
            { WeaponsSlot.axe, "axe" },
            { WeaponsSlot.mace, "mace" },
            { WeaponsSlot.dagger, "dagger" },
            { WeaponsSlot.twohandedsword, "2hsword" },
            { WeaponsSlot.spear, "spear" },
            { WeaponsSlot.twohandedaxe, "2haxe" },
            { WeaponsSlot.twohandedmace, "2hmace" },
            { WeaponsSlot.bow, "bow" },
            { WeaponsSlot.crossbow, "crossbow" },
            { WeaponsSlot.sling, "sling" },
            { WeaponsSlot.twohandedstaff, "2hStaff" },
            { WeaponsSlot.chain, "chain" },
            { WeaponsSlot.lute, "lute" }
        };

        private static readonly Dictionary<WeaponsTags, string> WeaponsTagsMap = new Dictionary<WeaponsTags, string>
        {
            { WeaponsTags.unique, "unique" },
            { WeaponsTags.magic, "magic" },
            { WeaponsTags.special, "special" },
            { WeaponsTags.specialexc, "special exc" },
            { WeaponsTags.WIP, "WIP" },
            { WeaponsTags.aldor, "aldor" },
            { WeaponsTags.aldorcommon, "aldor common" },
            { WeaponsTags.aldoruncommon, "aldor uncommon" },
            { WeaponsTags.aldorrare, "aldor rare" },
            { WeaponsTags.aldormagic, "aldor magic" },
            { WeaponsTags.fjall, "fjall" },
            { WeaponsTags.elven, "elven" },
            { WeaponsTags.skadia, "skadia" },
            { WeaponsTags.nistra, "nistra" }
        };

        public static void InjectTableWeapons(
            // Would love to use a hook but devs fucked up the table's categories' names
            string name,
            WeaponsTier Tier,
            string id,
            WeaponsSlot Slot,
            WeaponsRarity rarity,
            WeaponsMaterial Mat,
            WeaponsTags tags,
            string? Subtype = null,

            byte Rng = 1,
            int? Price = null,
            byte Markup = 1,
            short? MaxDuration = null,
            short? Armor_Piercing = null,
            short? Armor_Damage = null,
            short? Bodypart_Damage = null,
            short? Slashing_Damage = null,
            short? Piercing_Damage = null,
            short? Blunt_Damage = null,
            short? Rending_Damage = null,
            short? Fire_Damage = null,
            short? Shock_Damage = null,
            short? Poison_Damage = null,
            short? Caustic_Damage = null,
            short? Frost_Damage = null,
            short? Arcane_Damage = null,
            short? Unholy_Damage = null,
            short? Sacred_Damage = null,
            short? Psionic_Damage = null,
            short? FMB = null,
            short? Hit_Chance = null,
            short? CRT = null,
            short? CRTD = null,
            short? CTA = null,
            short? PRR = null,
            short? Block_Power = null,
            short? Block_Recovery = null,
            byte? Bleeding_Chance = null,
            byte? Daze_Chance = null,
            byte? Stun_Chance = null,
            byte? Knockback_Chance = null,
            byte? Immob_Chance = null,
            byte? Stagger_Chance = null,
            short? MP = null,
            short? MP_Restoration = null,
            short? Cooldown_Reduction = null,
            short? Abilities_Energy_Cost = null,
            short? Skills_Energy_Cost = null,
            short? Spells_Energy_Cost = null,
            short? Magic_Power = null,
            short? Miscast_Chance = null,
            short? Miracle_Chance = null,
            short? Miracle_Power = null,
            short? Bonus_Range = null,
            short? max_hp = null,
            short? Health_Restoration = null,
            short? Healing_Received = null,
            short? Crit_Avoid = null,
            short? Fatigue_Gain = null,
            short? Lifesteal = null,
            short? Manasteal = null,
            short? Damage_Received = null,
            short? Pyromantic_Power = null,
            short? Geomantic_Power = null,
            short? Venomantic_Power = null,
            short? Electroantic_Power = null,
            short? Cryomantic_Power = null,
            short? Arcanistic_Power = null,
            short? Astromantic_Power = null,
            short? Psimantic_Power = null,
            short? Balance = null, // Could be byte ?
            string? upgrade = null,
            bool fireproof = false,
            bool NoDrop = false,
            string? audio = null
            )
        {
            // Table filename
            const string tableName = "gml_GlobalScript_table_weapons";

            // Load table if it exists
            List<string> table = Msl.ThrowIfNull(ModLoader.GetTable(tableName));

            // Prepare line
            string newline = $"{name};{GetEnumValue(Tier, WeaponsTierMap)};{id};{GetEnumValue(Slot, WeaponsSlotMap)};{Subtype};{rarity};{Mat};{Price};{Markup};{MaxDuration};{Rng};;{Armor_Piercing};{Armor_Damage};{Bodypart_Damage};;{Slashing_Damage};{Piercing_Damage};{Blunt_Damage};{Rending_Damage};{Fire_Damage};{Shock_Damage};{Poison_Damage};{Caustic_Damage};{Frost_Damage};{Arcane_Damage};{Unholy_Damage};{Sacred_Damage};{Psionic_Damage};;{FMB};{Hit_Chance};{CRT};{CRTD};{CTA};{PRR};{Block_Power};{Block_Recovery};;{Bleeding_Chance};{Daze_Chance};{Stun_Chance};{Knockback_Chance};{Immob_Chance};{Stagger_Chance};;{MP};{MP_Restoration};{Cooldown_Reduction};{Abilities_Energy_Cost};{Skills_Energy_Cost};{Spells_Energy_Cost};{Magic_Power};{Miscast_Chance};{Miracle_Chance};{Miracle_Power};{Bonus_Range};;{max_hp};{Health_Restoration};{Healing_Received};{Crit_Avoid};{Fatigue_Gain};{Lifesteal};{Manasteal};{Damage_Received};;{Pyromantic_Power};{Geomantic_Power};{Venomantic_Power};{Electroantic_Power};{Cryomantic_Power};{Arcanistic_Power};{Astromantic_Power};{Psimantic_Power};;{Balance};{GetEnumValue(tags, WeaponsTagsMap)};{upgrade};{(fireproof ? 1 : "")};{(NoDrop ? 1 : "")};{audio};";

            // Add line to table
            table.Add(newline);
            ModLoader.SetTable(table, tableName);
        }
    }
}
