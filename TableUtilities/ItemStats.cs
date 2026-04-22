using ModShardLauncher;


namespace TheWitcher
{
    public static partial class TableUtils
    {
        public enum ItemStatsTier
        {
            none,
            Tier1,
            Tier2,
            Tier3,
            Tier4,
            Tier5
        }

        public enum ItemStatsCategory
        {
            none,
            medicine,
            beverage,
            junk,
            tool,
            drug,
            alcohol,
            ammo,
            valuable,
            ingredient,
            food,
            trophy,
            commodity,
            material,
            additive,
            resource,
            upgrade,
            flag,
            bag,
            quest,
            scroll,
            book,
            treasure,
            recipe,
            schematic
        }

        public enum ItemStatsSubcategory
        {
            none,
            hide,
            ingredient,
            alchemy,
            gem,
            potion,
            meat,
            meat_large,
            fish,
            vegetable,
            fruit,
            berry,
            herb,
            mushroom,
            dairy,
            pastry,
            dish,
            quest,
            bird,
            treatise
        }

        public enum ItemStatsMaterial
        {
            cloth,
            glass,
            organic,
            metal,
            wood,
            leather,
            gold,
            pottery,
            gem,
            silver,
            stone,
            paper
        }

        public enum ItemStatsWeight
        {
            Light,
            Medium,
            VeryLight,
            Heavy,
            Net
        }

        public enum ItemStatsTags
        {
            none,
            special,
            WIP,
            common,
            uncommon,
            rare,
            unique,
            elven,
            elvencommon,
            elvenuncommon,
            elvenrare,
            commonanimal,
            uncommonanimal,
            rareanimal,
            alchemy,
            commonalchemy,
            uncommonalchemy,
            commonraw,
            uncommonraw,
            rareraw,
            commoncooked,
            uncommoncooked,
            rarecooked,
            crypt,
            catacombs,
            bastion
        }

        // 映射字典
        private static readonly Dictionary<ItemStatsTier, string> TierMap = new Dictionary<ItemStatsTier, string>
        {
            { ItemStatsTier.none, "" },
            { ItemStatsTier.Tier1, "1" },
            { ItemStatsTier.Tier2, "2" },
            { ItemStatsTier.Tier3, "3" },
            { ItemStatsTier.Tier4, "4" },
            { ItemStatsTier.Tier5, "5" }
        };

        private static readonly Dictionary<ItemStatsCategory, string> CategoryMap = new Dictionary<ItemStatsCategory, string>
        {
            { ItemStatsCategory.none, "" },
            { ItemStatsCategory.medicine, "medicine" },
            { ItemStatsCategory.beverage, "beverage" },
            { ItemStatsCategory.junk, "junk" },
            { ItemStatsCategory.tool, "tool" },
            { ItemStatsCategory.drug, "drug" },
            { ItemStatsCategory.alcohol, "alcohol" },
            { ItemStatsCategory.ammo, "ammo" },
            { ItemStatsCategory.valuable, "valuable" },
            { ItemStatsCategory.ingredient, "ingredient" },
            { ItemStatsCategory.food, "food" },
            { ItemStatsCategory.trophy, "trophy" },
            { ItemStatsCategory.commodity, "commodity" },
            { ItemStatsCategory.material, "material" },
            { ItemStatsCategory.additive, "additive" },
            { ItemStatsCategory.resource, "resource" },
            { ItemStatsCategory.upgrade, "upgrade" },
            { ItemStatsCategory.flag, "flag" },
            { ItemStatsCategory.bag, "bag" },
            { ItemStatsCategory.quest, "quest" },
            { ItemStatsCategory.scroll, "scroll" },
            { ItemStatsCategory.book, "book" },
            { ItemStatsCategory.treasure, "treasure" },
            { ItemStatsCategory.recipe, "recipe" },
            { ItemStatsCategory.schematic, "schematic" }
        };

        private static readonly Dictionary<ItemStatsSubcategory, string> SubcategoryMap = new Dictionary<ItemStatsSubcategory, string>
        {
            { ItemStatsSubcategory.none, "" },
            { ItemStatsSubcategory.hide, "hide" },
            { ItemStatsSubcategory.ingredient, "ingredient" },
            { ItemStatsSubcategory.alchemy, "alchemy" },
            { ItemStatsSubcategory.gem, "gem" },
            { ItemStatsSubcategory.potion, "potion" },
            { ItemStatsSubcategory.meat, "meat" },
            { ItemStatsSubcategory.meat_large, "meat_large" },
            { ItemStatsSubcategory.fish, "fish" },
            { ItemStatsSubcategory.vegetable, "vegetable" },
            { ItemStatsSubcategory.fruit, "fruit" },
            { ItemStatsSubcategory.berry, "berry" },
            { ItemStatsSubcategory.herb, "herb" },
            { ItemStatsSubcategory.mushroom, "mushroom" },
            { ItemStatsSubcategory.dairy, "dairy" },
            { ItemStatsSubcategory.pastry, "pastry" },
            { ItemStatsSubcategory.dish, "dish" },
            { ItemStatsSubcategory.quest, "quest" },
            { ItemStatsSubcategory.bird, "bird" },
            { ItemStatsSubcategory.treatise, "treatise" }
        };

        private static readonly Dictionary<ItemStatsWeight, string> WeightMap = new Dictionary<ItemStatsWeight, string>
        {
            { ItemStatsWeight.Light, "Light" },
            { ItemStatsWeight.Medium, "Medium" },
            { ItemStatsWeight.VeryLight, "Very Light" },
            { ItemStatsWeight.Heavy, "Heavy" },
            { ItemStatsWeight.Net, "Net" }
        };

        private static readonly Dictionary<ItemStatsTags, string> TagsMap = new Dictionary<ItemStatsTags, string>
        {
            { ItemStatsTags.none, "" },
            { ItemStatsTags.special, "special" },
            { ItemStatsTags.WIP, "WIP" },
            { ItemStatsTags.common, "common" },
            { ItemStatsTags.uncommon, "uncommon" },
            { ItemStatsTags.rare, "rare" },
            { ItemStatsTags.unique, "unique" },
            { ItemStatsTags.elven, "elven" },
            { ItemStatsTags.elvencommon, "elven common" },
            { ItemStatsTags.elvenuncommon, "elven uncommon" },
            { ItemStatsTags.elvenrare, "elven rare" },
            { ItemStatsTags.commonanimal, "common animal" },
            { ItemStatsTags.uncommonanimal, "uncommon animal" },
            { ItemStatsTags.rareanimal, "rare animal" },
            { ItemStatsTags.alchemy, "alchemy" },
            { ItemStatsTags.commonalchemy, "common alchemy" },
            { ItemStatsTags.uncommonalchemy, "uncommon alchemy" },
            { ItemStatsTags.commonraw, "common raw" },
            { ItemStatsTags.uncommonraw, "uncommon raw" },
            { ItemStatsTags.rareraw, "rare raw" },
            { ItemStatsTags.commoncooked, "common cooked" },
            { ItemStatsTags.uncommoncooked, "uncommon cooked" },
            { ItemStatsTags.rarecooked, "rare cooked" },
            { ItemStatsTags.crypt, "crypt" },
            { ItemStatsTags.catacombs, "catacombs" },
            { ItemStatsTags.bastion, "bastion" }
        };

        // 获取枚举值的字符串表示
        private static string GetEnumValue<T>(T enumValue, Dictionary<T, string> map) where T : Enum
        {
            return map.TryGetValue(enumValue, out var value) ? value : enumValue.ToString();
        }

        public static void InjectTableItemStats(
        string id,
        ItemStatsMaterial Material,
        ItemStatsWeight Weight,
        int? Price = null,
        int? EffPrice = null,
        ItemStatsTier tier = ItemStatsTier.none,
        ItemStatsCategory Cat = ItemStatsCategory.none,
        ItemStatsSubcategory Subcat = ItemStatsSubcategory.none,
        ushort? Fresh = null,
        ushort? Duration = null,
        ushort? Stacks = null,
        ushort? Diet = null, // 类型?
        short? Hunger = null,
        float? Hunger_Change = null,
        short? Hunger_Resistance = null,
        short? Thirsty = null,
        float? Thirst_Change = null,
        short? Immunity = null,
        float? Immunity_Change = null,
        short? Intoxication = null,
        float? Toxicity_Change = null,
        short? Toxicity_Resistance = null,
        short? Pain = null,
        float? Pain_Change = null,
        short? Pain_Resistance = null,
        short? Pain_Limit = null,
        short? Morale = null,
        float? Morale_Change = null,
        ushort? MoraleTemporary = null,
        ushort? MoraleDiet = null,
        short? Sanity = null,
        float? Sanity_Change = null,
        short? Condition = null,
        short? max_hp = null,
        short? max_hp_res = null,
        short? HP_Turn = null,
        short? Health_Restoration = null,
        short? Healing_Received = null,
        short? max_mp = null,
        short? max_mp_res = null,
        short? MP_Restoration = null,
        short? MP_turn = null,
        short? Fatigue = null,
        float? Fatigue_Change = null,
        short? Fatigue_Gain = null,
        short? Received_XP = null,
        short? Cooldown_Reduction = null,
        short? Weapon_Damage = null,
        short? Magic_Power = null,
        short? Hit_Chance = null,
        short? FMB = null,
        short? CRTD = null,
        short? Fortitude = null,
        short? VSN = null,
        short? Bleeding_Resistance = null,
        short? Knockback_Resistance = null,
        short? Stun_Resistance = null,
        short? Physical_Resistance = null,
        short? Nature_Resistance = null,
        short? Magic_Resistance = null,
        short? Slashing_Resistance = null,
        short? Piercing_Resistance = null,
        short? Blunt_Resistance = null,
        short? Rending_Resistance = null,
        short? Fire_Resistance = null,
        short? Shock_Resistance = null,
        short? Poison_Resistance = null,
        short? Caustic_Resistance = null,
        short? Frost_Resistance = null,
        short? Arcane_Resistance = null,
        short? Unholy_Resistance = null,
        short? Sacred_Resistance = null,
        short? Psionic_Resistance = null,
        short? Bleeding_Resistance_2 = null,
        short? Nausea_Chance = null,
        short? Poisoning_Chance = null,
        short? Poisoning_Duration = null,
        short? SLING_AMMO = null,
        short? DESTROY_ON_HIT = null,
        short? IS_LIQUID = null,
        short? IS_CONTAINER = null,
        bool purse = false,
        bool bottle = false,
        string? upgrade = null,
        short? fodder = null,
        short? stack = null,
        bool fireproof = false,
        bool dropsOnce = false,
        ItemStatsTags tags = ItemStatsTags.none
        )
        {
            const string tableName = "gml_GlobalScript_table_items_stats";

            List<string> table = Msl.ThrowIfNull(ModLoader.GetTable(tableName));

            string newline = $"{id};;{Price};{EffPrice};{GetEnumValue(tier, TierMap)};{GetEnumValue(Cat, CategoryMap)};{GetEnumValue(Subcat, SubcategoryMap)};{Material};{GetEnumValue(Weight, WeightMap)};;{Fresh};{Duration};{Stacks};{Diet};;{Hunger};{Hunger_Change};{Hunger_Resistance};;{Thirsty};{Thirst_Change};;{Immunity};{Immunity_Change};;{Intoxication};{Toxicity_Change};{Toxicity_Resistance};;{Pain};{Pain_Change};{Pain_Resistance};{Pain_Limit};;{Morale};{Morale_Change};{MoraleTemporary};{MoraleDiet};{Sanity};{Sanity_Change};;{Condition};{max_hp};{max_hp_res};{HP_Turn};{Health_Restoration};{Healing_Received};;{max_mp};{max_mp_res};{MP_Restoration};{MP_turn};;{Fatigue};{Fatigue_Change};{Fatigue_Gain};;{Received_XP};{Cooldown_Reduction};{Weapon_Damage};{Magic_Power};{Hit_Chance};{FMB};{CRTD};{Fortitude};{VSN};;{Bleeding_Resistance};{Knockback_Resistance};{Stun_Resistance};;{Physical_Resistance};{Nature_Resistance};{Magic_Resistance};{Slashing_Resistance};{Piercing_Resistance};{Blunt_Resistance};{Rending_Resistance};{Fire_Resistance};{Shock_Resistance};{Poison_Resistance};{Caustic_Resistance};{Frost_Resistance};{Arcane_Resistance};{Unholy_Resistance};{Sacred_Resistance};{Psionic_Resistance};;{Nausea_Chance};{Poisoning_Chance};{Poisoning_Duration};;{SLING_AMMO};{DESTROY_ON_HIT};{IS_LIQUID};{IS_CONTAINER};;;{(purse ? "1" : "")};{(bottle ? "1" : "")};{upgrade};{fodder};{stack};{(fireproof ? "1" : "")};{(dropsOnce ? "1" : "")};{GetEnumValue(tags, TagsMap)};";

            table.Add(newline);
            ModLoader.SetTable(table, tableName);
        }
    }
}