using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddWitcherPotion()
    {
        UndertaleSprite ico = Msl.GetSprite("s_inv_witcher_potion");
        ico.CollisionMasks.RemoveAt(0);
        ico.IsSpecialType = true;
        ico.SVersion = 3;
        // ico.Width = 27;
        // ico.Height = 54;
        // ico.OriginX = 0;
        // ico.OriginY = 0;
        // ico.MarginLeft = 1;
        // ico.MarginRight = 25;
        // ico.MarginBottom = 47;
        // ico.MarginTop = 5;
        ico.GMS2PlaybackSpeed = 1;
        ico.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        // foreach (var tte in ico.Textures)
        // {
        //     tte.Texture.TargetX = 4;
        //     tte.Texture.TargetY = 5;
        //     tte.Texture.TargetWidth = 18;
        //     tte.Texture.TargetHeight = 43;
        //     tte.Texture.BoundingWidth = 27;
        //     tte.Texture.BoundingHeight = 54;
        // }

        ico = Msl.GetSprite("s_loot_witcher_potion");
        ico.CollisionMasks.RemoveAt(0);
        ico.IsSpecialType = true;
        ico.SVersion = 3;
        ico.BBoxMode = 2;
        // ico.Width = 10;
        // ico.Height = 14;
        // ico.OriginX = 0;
        // ico.OriginY = 0;
        // ico.MarginLeft = 1;
        // ico.MarginRight = 7;
        // ico.MarginBottom = 12;
        // ico.MarginTop = 5;
        ico.GMS2PlaybackSpeed = 1;
        ico.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        UndertaleGameObject o_inv_witcher_potion_water = Msl.GetObject("o_inv_witcher_potion_water");
        UndertaleGameObject o_loot_witcher_potion_water = Msl.GetObject("o_loot_witcher_potion_water");
        UndertaleGameObject o_inv_witcher_potion_empty = Msl.GetObject("o_inv_witcher_potion_empty");
        UndertaleGameObject o_loot_witcher_potion_empty = Msl.GetObject("o_loot_witcher_potion_empty");

        o_inv_witcher_potion_empty.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                drop_gui_sound = snd_med_drop
                pickup_sound = snd_med_pick
                is_execute = false
            ")
        );

        o_inv_witcher_potion_water.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                drop_gui_sound = snd_beverage_drop
                pickup_sound = snd_beverage_pick
                dishes_object = o_inv_witcher_potion_empty
                max_charge = 1
            ")
        );

        o_loot_witcher_potion_water.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                inv_object = o_inv_witcher_potion_water
            ")
        );

        // Make these attributes show duration text when hovered
        Msl.LoadGML("gml_Object_o_textLoader_Other_25")
            .MatchFrom("ds_list_add(global.attribute_duration, ")
            .InsertBelow(@"ds_list_add(global.attribute_duration, ""EVS"", ""PRR"", ""Shock_Damage"", ""Magic_Power"", ""Miracle_Chance"", ""Miscast_Chance"", ""Backfire_Damage"", ""CTA"", ""Crit_Avoid"", ""Mainhand_Efficiency"", ""Offhand_Efficiency"", ""Abilities_Energy_Cost"")")
            .Save();

        UndertaleGameObject o_inv_witcher_potion = Msl.GetObject("o_inv_witcher_potion");
        UndertaleGameObject o_loot_witcher_potion = Msl.GetObject("o_loot_witcher_potion");

        o_inv_witcher_potion.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                can_merge = false
                drop_gui_sound = snd_beverage_drop
                pickup_sound = snd_beverage_pick
                max_charge = 1
                ds_map_set(data, ""quality"", (3 << 0))
                ds_map_set(data, ""Colour"", make_colour_rgb(76, 127, 255))
                dishes_object = o_inv_witcher_potion_empty
            "),

            new MslEvent(eventType: EventType.Other, subtype: 24, code: @"
                event_inherited()
                audio_play_sound(snd_gui_drink_potion, 3, 0)
                scr_random_speech(""useWitcherPotion"")

                with (o_player)
                    scr_guiAnimation(s_drinking, 1, 1, 0)

                if (!o_skill_trial_of_grasses.is_open)
                    scr_atr_incr(""Intoxication"", 100)
                
                with (o_perk_professional_witcher)
                {
                    item = other.object_index
                    event_user(5)
                }
            "),

            new MslEvent(eventType: EventType.Other, subtype: 25, code: @"
                event_inherited()
                with (loot_object)
                {
                    image_index = other.i_index
                    i_index = other.i_index
                }
            ")
        );

        o_loot_witcher_potion.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                image_speed = 0
                charge = 1
                number = 0
            ")
        );

        AddWitcherPotionObject(
            id: "thunderbolt_potion",
            effects: new Dictionary<string, int>()
            {
                {"Intoxication",     15},
                {"Weapon_Damage",    25},
                {"CRT",              15},
                {"EVS",             -5},
                {"PRR",             -5}
            },
            duration: 30,
            name: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "Thunderbolt"},
                {ModLanguage.Chinese, "雷霆"}
            },
            midtext: new Dictionary<ModLanguage, string>()
            {
                {ModLanguage.English, "Without having undergone ~o~The Trial of Grasses~/~, drinking this potion results in instant ~r~Deadly Intoxication~/~."},
                {ModLanguage.Chinese, "如果没有通过~o~青草试炼~/~，那么喝下此药会立即~r~迷醉濒死~/~。"}
            },
            description: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "Witchers take this potion before fighting strong, heavily armored opponents. Imbibing Thunderbolt causes witchers to enter into a battle trance. While in this state, witchers attack more efficiently and cause greater damage, while at the same time neglecting their own defense and becoming an easier target."},
                {ModLanguage.Chinese, "猎魔人在对抗强大且身披重甲的敌人前会服用这种药剂。服下雷霆药剂后，猎魔人会进入战斗狂热状态。在这种状态下，他们攻击更为高效、伤害更高，但同时会忽视自身防御，变得更容易受到攻击。"}
            }
        );

        AddWitcherPotionObject(
            id: "blizzard_potion",
            effects: new Dictionary<string, int>()
            {
                {"Intoxication",        15},
                {"EVS",                 25},
                {"PRR",                 25},
                {"Mainhand_Efficiency", 15},
                {"Offhand_Efficiency",  25}
            },
            duration: 30,
            name: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "Blizzard"},
                {ModLanguage.Chinese, "暴风雪"}
            },
            midtext: new Dictionary<ModLanguage, string>()
            {
                {ModLanguage.English, "Without having undergone ~o~The Trial of Grasses~/~, drinking this potion results in instant ~r~Deadly Intoxication~/~."},
                {ModLanguage.Chinese, "如果没有通过~o~青草试炼~/~，那么喝下此药会立即~r~迷醉濒死~/~。"}
            },
            description: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "Blizzard is one of the most powerful witchers' potions. It improves reflexes and reaction time, allowing a witcher to better parry and evade attacks. It is most helpful against groups of enemies and monsters that move fast or teleport, such as wraiths and various types of vampires."},
                {ModLanguage.Chinese, "暴雪是最强效的猎魔人魔药之一，能提升反应速度与神经反射，使猎魔人更善于格挡与闪避攻击。该药剂在对抗成群敌人或行动迅捷、会瞬移的怪物（如幽灵和各类吸血鬼）时尤为有效。"}
            }
        );

        AddWitcherPotionObject(
            id: "petri_philter",
            effects: new Dictionary<string, int>()
            {
                {"Intoxication",         30},
                {"Magic_Power",          25},
                {"Miracle_Chance",       10},
                {"Cooldown_Reduction",  -25},
                {"Miscast_Chance",       6},
                {"Backfire_Damage",      9}
            },
            duration: 30,
            name: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "Petri's Philter"},
                {ModLanguage.Chinese, "佩特里"}
            },
            midtext: new Dictionary<ModLanguage, string>()
            {
                {ModLanguage.English, "Without having undergone ~o~The Trial of Grasses~/~, drinking this potion results in instant ~r~Deadly Intoxication~/~."},
                {ModLanguage.Chinese, "如果没有通过~o~青草试炼~/~，那么喝下此药会立即~r~迷醉濒死~/~。"}
            },
            description: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "Throughout his life, the mage Petri sought a way to strengthen his magical powers. He failed in this endeavor, but several interesting potions came about through his attempts. One of these, known as Petri's Philter, is useful to witchers with magical skills. Witchers who prefer hand-to-hand combat rarely use Petri's Philter, however, especially since it is highly toxic and thus renders the use of any other potions impossible."},
                {ModLanguage.Chinese, "法师佩特里毕生致力于增强自身法力，虽未能如愿，但他的尝试催生了几种有趣的魔药。其中一种被称为“佩特里的魔药”，对拥有法术能力的猎魔人颇有价值。然而偏好近战的猎魔人很少使用它，尤其是因为其毒性极强，服用后便无法再使用其他药剂。"}
            }
        );

        AddWitcherPotionObject(
            id: "swallow_potion",
            effects: new Dictionary<string, int>()
            {
                {"Intoxication",       10},
                {"Pain",              -25},
                {"max_hp_res",         25},
                {"Condition",          25},
                {"Healing_Received",   25}
            },
            duration: 60,
            code: @"
                var _changeValue = ds_map_find_value_ext(attributes_data, ""Condition"", 0)
                with (o_player)
                {
                    var key = ds_map_find_first(Body_Parts_map)
                    var _size = ds_map_size(Body_Parts_map)

                    repeat (_size)
                    {
                        var _condition = ds_map_find_value(Body_Parts_map, key)
                        ds_map_set(Body_Parts_map, key, min(100, _condition + _changeValue))
                        key = ds_map_find_next(Body_Parts_map, key)
                    }
                }

                with (o_physical_debuff)
                {
                    if (is_player(target))
                        instance_destroy()
                }

                with (o_wound_debuff)
                {
                    if (is_player(target))
                        instance_destroy()
                }

                with (o_db_bleed_parent)
                {
                    if (is_player(target))
                        instance_destroy()
                }

                event_inherited()
            ",
            name: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "Swallow"},
                {ModLanguage.Chinese, "燕子"}
            },
            midtext: new Dictionary<ModLanguage, string>()
            {
                {ModLanguage.English, "~lg~Removes~/~ all ~r~negative~/~ physical effects. Without having undergone ~o~The Trial of Grasses~/~, drinking this potion results in instant ~r~Deadly Intoxication~/~."},
                {ModLanguage.Chinese, "可以~lg~移除~/~所有~r~负面~/~物理效果。##如果没有通过~o~青草试炼~/~，那么喝下此药会立即~r~迷醉濒死~/~。"}
            },
            description: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "There is no bird more beautiful than the swallow, the harbinger of spring. Even the dark mages who developed the formula for witchers' potions appreciated the charm of this bird, lending its name to the potion that accelerates regeneration of a mutated organism."},
                {ModLanguage.Chinese, "没有比燕子更美的鸟了，它是春天的使者。即便是研发猎魔人魔药配方的黑巫师，也赞赏这种鸟的魅力，并以它为名，为能加速突变体恢复的药剂命名。"}
            }
        );

        AddWitcherPotionObject(
            id: "tawny_owl",
            effects: new Dictionary<string, int>()
            {
                {"Intoxication",            10},
                {"max_mp_res",              40},
                {"Fatigue",                 -5},
                {"Abilities_Energy_Cost",   -10},
                {"MP_Restoration",          35},
                {"Nature_Resistance",       -5},
                {"Magic_Resistance",        -5}
            },
            duration: 60,
            name: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "Tawny Owl"},
                {ModLanguage.Chinese, "棕林鸮"}
            },
            midtext: new Dictionary<ModLanguage, string>()
            {
                {ModLanguage.English, "Without having undergone ~o~The Trial of Grasses~/~, drinking this potion results in instant ~r~Deadly Intoxication~/~."},
                {ModLanguage.Chinese, "如果没有通过~o~青草试炼~/~，那么喝下此药会立即~r~迷醉濒死~/~。"}
            },
            description: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "In preparation for an all-night vigil at the side of a cursed man, or before a battle which is certain to be prolonged, a witcher mixes a dose of Tawny Owl potion to boost his endurance."},
                {ModLanguage.Chinese, "在为守护一名被诅咒者通宵守夜，或即将迎来一场注定持久的战斗做准备时，猎魔人会调制一剂黄褐色猫头鹰药剂，以增强体力与精力。"}
            }
        );

        AddWitcherPotionObject(
            id: "golden_oriole",
            effects: new Dictionary<string, int>()
            {
                {"Intoxication",             5},
                {"Poison_Resistance",       50},
                {"Fire_Resistance",         30},
                {"Bleeding_Resistance",     15},
                {"Physical_Resistance",     10},
                {"Nature_Resistance",       15},
                {"Magic_Resistance",        15}
            },
            duration: 60,
            name: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "Golden Oriole"},
                {ModLanguage.Chinese, "金莺"}
            },
            midtext: new Dictionary<ModLanguage, string>()
            {
                {ModLanguage.English, "Without having undergone ~o~The Trial of Grasses~/~, drinking this potion results in instant ~r~Deadly Intoxication~/~."},
                {ModLanguage.Chinese, "如果没有通过~o~青草试炼~/~，那么喝下此药会立即~r~迷醉濒死~/~。"}
            },
            description: new Dictionary<ModLanguage, string>() {
                {ModLanguage.English, "Golden Oriole, one of the best-known witcher potions, increases resistance to special attacks, such as Poisoning, Incineration and Bleeding. The brew also negates the side effects of the Lapwing and Brock potions, while producing no side effects of its own. The potion increases Toxicity slightly when imbibed."},
                {ModLanguage.Chinese, "金莺药剂是最著名的猎魔人魔药之一，能增强对特殊攻击的抵抗力，如中毒、焚烧和流血。此外，它还能抵消田凫药剂与獾药剂的副作用，且自身几乎无副作用，仅在服用时略微增加毒性。"}
            }
        );

        Msl.InjectTableItemsLocalization(potion_texts.ToArray());

        // Add attribute data to crafting menu list
        Msl.LoadGML("gml_Object_o_craftingMenuRecipeButton_Other_25")
            .MatchAll()
            .InsertBelow(@$"
switch (idName)
{{
    {string.Join("\n", attribute_codes)}
}}
            ")
            .Save();
    }

    private int potion_idx = 0;
    private List<LocalizationItem> potion_texts = new List<LocalizationItem>();
    private List<string> attribute_codes = new List<string>();
    private void AddWitcherPotionObject(
        string id, Dictionary<string, int> effects, Dictionary<ModLanguage, string> name,
        Dictionary<ModLanguage, string> midtext, Dictionary<ModLanguage, string> description,
        string code = "event_inherited()", ushort duration = 30)
    {
        UndertaleGameObject inv = Msl.AddObject(
            name: $"o_inv_{id}",
            parentName: "o_inv_witcher_potion",
            spriteName: "s_inv_witcher_potion",
            isVisible: true,
            isPersistent: true,
            isAwake: true
        );

        UndertaleGameObject loot = Msl.AddObject(
            name: $"o_loot_{id}",
            parentName: "o_loot_witcher_potion",
            spriteName: "s_loot_witcher_potion",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        TableUtils.InjectTableItemStats(
            id: id,
            Price: 200,
            Cat: TableUtils.ItemStatsCategory.beverage,
            Subcat: TableUtils.ItemStatsSubcategory.potion,
            Material: TableUtils.ItemStatsMaterial.glass,
            Weight: TableUtils.ItemStatsWeight.Light,
            Duration: duration,
            tags: TableUtils.ItemStatsTags.special,
            bottle: true
        );

        string effects_string = string.Join(
            "\n", effects.Select(kv => $"scr_consum_set_attribute(\"{kv.Key}\", {kv.Value})")
        );

        attribute_codes.Add(@$"
case ""{id}"":
{effects_string}
break;
");

        inv.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @$"
                event_inherited()
                scr_consum_atr(""{id}"")
                i_index = {potion_idx}
                {effects_string}
            "),

            new MslEvent(eventType: EventType.Other, subtype: 24, code: code)
        );

        loot.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @$"
                event_inherited()
                inv_object = o_inv_{id}
            ")
        );

        potion_texts.Add(
            new LocalizationItem(
                id: id,
                name: name,
                effect: midtext,
                description: description
            )
        );

        potion_idx++;
    }

    private void AddTestSprites()
    {
        // AddTestPotionObject("weapon_oil_full");
        // AddTestPotionObject("weapon_oil_empty");
        // AddTestPotionObject("weapon_oil_water");
        // AddTestPotionObject("witcher_test_a");
        // AddTestPotionObject("witcher_test_b");
        // AddTestPotionObject("witcher_test_c");
        // AddTestPotionObject("decoction_test");
    }

    private void AddTestPotionObject(string id)
    {
        UndertaleGameObject inv = Msl.AddObject(
            name: $"o_inv_{id}",
            parentName: "o_inv_dishes_beverage",
            spriteName: $"s_inv_{id}",
            isVisible: true,
            isPersistent: true,
            isAwake: true
        );

        UndertaleGameObject loot = Msl.AddObject(
            name: $"o_loot_{id}",
            parentName: "o_consument_loot",
            spriteName: $"s_loot_{id}",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        TableUtils.InjectTableItemStats(
            id: id,
            Price: 200,
            Cat: TableUtils.ItemStatsCategory.beverage,
            Subcat: TableUtils.ItemStatsSubcategory.potion,
            Material: TableUtils.ItemStatsMaterial.glass,
            Weight: TableUtils.ItemStatsWeight.Light,
            Duration: 20,
            tags: TableUtils.ItemStatsTags.special,
            bottle: true
        );

        inv.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @$"
                event_inherited()
                scr_consum_atr(""{id}"")
                can_merge = false
                drop_gui_sound = snd_beverage_drop
                pickup_sound = snd_beverage_pick
                max_charge = 1
                draw_charges = false
                ds_map_set(data, ""quality"", (3 << 0))
                ds_map_set(data, ""Colour"", make_colour_rgb(76, 127, 255))
                dishes_object = o_inv_witcher_potion_water
            "),

            new MslEvent(eventType: EventType.Draw, subtype: 0, code: "scr_draw_consum_scale()")
        );

        loot.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @$"
                event_inherited()
                inv_object = o_inv_{id}
            ")
        );
    }
}