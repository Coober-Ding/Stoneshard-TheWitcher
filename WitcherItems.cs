using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddWitcherItems()
    {
        AddMedallionWolf();
        AddAncientTrollGland();
        AddGeraltStealSword();
    }

    private void AddMedallionWolf()
    {
        UndertaleSprite ico = Msl.GetSprite("s_inv_wolfschoolmedallion");
        ico.CollisionMasks.RemoveAt(0);
        ico.IsSpecialType = true;
        ico.SVersion = 3;
        ico.Width = 27;
        ico.Height = 54;
        ico.OriginX = 0;
        ico.OriginY = 0;
        ico.MarginLeft = 2;
        ico.MarginRight = 24;
        ico.MarginBottom = 49;
        ico.MarginTop = 2;
        ico.GMS2PlaybackSpeed = 1;
        ico.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        foreach (var tte in ico.Textures)
        {
            tte.Texture.TargetX = 2;
            tte.Texture.TargetY = 3;
            tte.Texture.TargetWidth = 23;
            tte.Texture.TargetHeight = 48;
            tte.Texture.BoundingWidth = 27;
            tte.Texture.BoundingHeight = 54;
        }

        ico = Msl.GetSprite("s_loot_wolfschoolmedallion");
        ico.CollisionMasks.RemoveAt(0);
        ico.IsSpecialType = true;
        ico.SVersion = 3;
        ico.Width = 23;
        ico.Height = 18;
        ico.OriginX = 0;
        ico.OriginY = 0;
        ico.MarginLeft = 2;
        ico.MarginRight = 20;
        ico.MarginBottom = 15;
        ico.MarginTop = 2;
        ico.GMS2PlaybackSpeed = 1;
        ico.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        foreach (var tte in ico.Textures)
        {
            tte.Texture.TargetX = 0;
            tte.Texture.TargetY = 0;
            tte.Texture.TargetWidth = 23;
            tte.Texture.TargetHeight = 18;
            tte.Texture.BoundingWidth = 23;
            tte.Texture.BoundingHeight = 18;
        }

        UndertaleGameObject o_inv_witcher_medallion_wolf = Msl.AddObject(
            name: "o_inv_witcher_medallion_wolf",
            parentName: "o_inv_timer_consum",
            spriteName: "s_inv_wolfschoolmedallion",
            isVisible: true,
            isPersistent: true,
            isAwake: true
        );

        UndertaleGameObject o_loot_witcher_medallion_wolf = Msl.AddObject(
            name: "o_loot_witcher_medallion_wolf",
            parentName: "o_consument_loot",
            spriteName: "s_loot_wolfschoolmedallion",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        o_inv_witcher_medallion_wolf.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                scr_consum_atr(""witcher_medallion_wolf"")

                ds_map_set(data, ""quality"", (7 << 0))
                ds_map_set(data, ""Colour"", make_colour_rgb(229, 193, 85))
                if object_is_ancestor(object_index, o_inv_slot_parent)
                    alarm[11] = shineDelay

                ds_map_add_list(data, ""uniqueBossKill"", __dsDebuggerListCreate())

                scr_consum_attribute_simple_add(""Nature_Resistance"", 9);
                scr_consum_attribute_simple_add(""Magic_Resistance"", 9);
                scr_consum_attribute_simple_add(""Received_XP"", 6);
                scr_consum_attribute_simple_add(""VSN"", 1);

                slot = ""Amulet""
                can_equip = true

                enemy_count = 0
                secret_room = 0
            "),

            new MslEvent(eventType: EventType.Other, subtype: 24, code: @"
                enemy_count = 0
                secret_room = 0

                audio_play_sound(snd_skill_search, 4, 0)

                with (o_enemy)
                {
                    if (scr_is_prey_animal())
                        continue

                    if (!visible && scr_tile_distance(o_player, id) <= (o_player.VSN * 3))
                    {
                        // 非 NPC 敌人
                        if (!is_o_NPC_ancestor)
                        {
                            if (!object_is_ancestor(object_index, o_bird_parent) && !object_is_ancestor(object_index, o_Hive))
                            {
                                other.enemy_count++
                                scr_hearing_indicator_create()
                            }
                        }
                        // NPC 敌人
                        else if (!is_neutral)
                        {
                            other.enemy_count++
                            scr_hearing_indicator_create()
                        }
                    }
                }

                if (instance_exists(o_secret_door))
                {
                    with (o_secret_door)
                    {
                        if (!o_secret_door.is_open)
                        {
                            if (scr_tile_distance(o_player, id) <= (o_player.VSN * 3))
                            {
                                scr_characterStatsUpdateAdd(""secretRoomsFound"", 1)
                                o_secret_door.is_open = true
                                audio_play_sound(snd_secret_room_find, 4, 0)
                                event_user(1)
                                
                                with (o_fogrender)
                                    event_user(2)
                                
                                scr_psy_change(""MoraleSituational"", 10, ""trap_find"")
                                other.secret_room++
                            }
                        }
                    }
                }

                charge++
                event_inherited()
            ")
        );

        o_loot_witcher_medallion_wolf.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                inv_object = o_inv_witcher_medallion_wolf
                number = 0
            ")
        );

        TableUtils.InjectTableItemStats(
            id: "witcher_medallion_wolf",
            Price: 200,
            EffPrice: 45,
            Cat: TableUtils.ItemStatsCategory.treasure,
            Material: TableUtils.ItemStatsMaterial.silver,
            Weight: TableUtils.ItemStatsWeight.Light,
            tags: TableUtils.ItemStatsTags.special
        );

        Msl.InjectTableItemsLocalization(
            new LocalizationItem(
                id: "witcher_medallion_wolf",
                name: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Wolf School Medallion"},
                    {ModLanguage.Chinese, "狼学派徽章"}
                },
                effect: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Every ~lg~12~/~ turns, the medallion scans within a range ~lg~5~/~ times the wielder’s sight. If enemies are present, " +
                        "it vibrates and yanks sharply on its chain. You can also ~lg~use~/~ the medallion actively to perform a scan."},
                    {ModLanguage.Chinese, "每~lg~60~/~回合，徽章会在~lg~3~/~倍视野范围内做侦测，当敌人存在时就会震动并且猛拉挂着它的链子。也可主动~lg~使用~/~徽章进行侦测。"}
                },
                description: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "The Witcher’s medallion is a silver amulet, crafted in different shapes to represent the various witcher schools. "},
                    {ModLanguage.Chinese, "猎魔人徽章是一种银制的护符，做成不同的形状来代表猎魔人们所属的不同学派。"}
                }
            )
        );
    }

    private void AddAncientTrollGland()
    {
        UndertaleSprite ico = Msl.GetSprite("s_inv_ancient_troll_gland");
        ico.CollisionMasks.RemoveAt(0);
        ico.IsSpecialType = true;
        ico.SVersion = 3;
        ico.Width = 27;
        ico.Height = 54;
        ico.OriginX = 0;
        ico.OriginY = 0;
        ico.MarginLeft = 2;
        ico.MarginRight = 24;
        ico.MarginBottom = 48;
        ico.MarginTop = 3;
        ico.GMS2PlaybackSpeed = 1;
        ico.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        foreach (var tte in ico.Textures)
        {
            tte.Texture.TargetX = 2;
            tte.Texture.TargetY = 3;
            tte.Texture.TargetWidth = 23;
            tte.Texture.TargetHeight = 46;
            tte.Texture.BoundingWidth = 27;
            tte.Texture.BoundingHeight = 54;
        }

        ico = Msl.GetSprite("s_loot_ancient_troll_gland");
        ico.CollisionMasks.RemoveAt(0);
        ico.IsSpecialType = true;
        ico.SVersion = 3;
        ico.Width = 23;
        ico.Height = 14;
        ico.OriginX = 0;
        ico.OriginY = 0;
        ico.MarginLeft = 4;
        ico.MarginRight = 8;
        ico.MarginBottom = 11;
        ico.MarginTop = 2;
        ico.GMS2PlaybackSpeed = 1;
        ico.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        foreach (var tte in ico.Textures)
        {
            tte.Texture.TargetX = 0;
            tte.Texture.TargetY = 0;
            tte.Texture.TargetWidth = 23;
            tte.Texture.TargetHeight = 14;
            tte.Texture.BoundingWidth = 23;
            tte.Texture.BoundingHeight = 14;
        }

        UndertaleGameObject o_inv_ancient_troll_gland = Msl.AddObject(
            name: "o_inv_ancient_troll_gland",
            parentName: "o_inv_consum_passive",
            spriteName: "s_inv_ancient_troll_gland",
            isVisible: true,
            isPersistent: true,
            isAwake: true
        );

        UndertaleGameObject o_loot_ancient_troll_gland = Msl.AddObject(
            name: "o_loot_ancient_troll_gland",
            parentName: "c_food",
            spriteName: "s_loot_ancient_troll_gland",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        o_inv_ancient_troll_gland.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                scr_consum_atr(""ancient_troll_gland"")
                drop_gui_sound = snd_item_meat_drop
                pickup_sound = snd_item_meat_pick
                ds_map_set(data, ""quality"", (6 << 0))
                ds_map_set(data, ""Colour"", make_colour_rgb(130, 72, 188))
            ")
        );

        TableUtils.InjectTableItemStats(
            id: "ancient_troll_gland",
            Price: 600,
            EffPrice: 600,
            tier: TableUtils.ItemStatsTier.Tier4,
            Cat: TableUtils.ItemStatsCategory.ingredient,
            Material: TableUtils.ItemStatsMaterial.organic,
            Weight: TableUtils.ItemStatsWeight.Light,
            tags: TableUtils.ItemStatsTags.alchemy
        );

        Msl.InjectTableItemsLocalization(
            new LocalizationItem(
                id: "ancient_troll_gland",
                name: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Ancient Troll Gland"},
                    {ModLanguage.Chinese, "古代巨魔腺体"}
                },
                effect: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Can be used to craft ~lg~advanced witcher mutagen potions~/~."},
                    {ModLanguage.Chinese, "可用于制作猎魔人~lg~进阶突变药剂~/~。"}
                },
                description: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "The essence of an ancient troll’s vitality, regarded in Idarran as the finest ingredient for crafting advanced witcher mutagens."},
                    {ModLanguage.Chinese, "古代巨魔生命力的精华，被艾达兰视为制作猎魔人进阶突变药剂的最佳候选。"}
                }
            )
        );

        Msl.LoadGML("gml_Object_o_ancientTroll_Create_0")
            .MatchFrom("ds_list_add(loot_list_add")
            .InsertBelow(@"ds_list_add(loot_list_add, ""o_loot_ancient_troll_gland"", 100)")
            .Save();

        Msl.LoadGML("gml_Object_o_ancientTroll_dead_Create_0")
            .MatchFrom("ds_list_add(loot_list_add")
            .InsertBelow(@"ds_list_add(loot_list_add, ""o_loot_ancient_troll_gland"", 100)")
            .Save();
    }

    private void AddGeraltStealSword()
    {
        TableUtils.InjectTableWeapons(
            name: "Geralt Steel Sword",
            Tier: TableUtils.WeaponsTier.Tier2,
            id: "witchersword01",
            Slot: TableUtils.WeaponsSlot.twohandedsword,
            rarity: TableUtils.WeaponsRarity.Unique,
            Mat: TableUtils.WeaponsMaterial.metal,
            tags: TableUtils.WeaponsTags.specialexc,
            Price: 150,
            Markup: 1,
            MaxDuration: 95,
            Rng: 1,

            Slashing_Damage: 20,
            Armor_Piercing: 10,
            Block_Power: 6,
            PRR: 4,
            CTA: 2,
            Skills_Energy_Cost: 10
        );

        Msl.InjectTableWeaponTextsLocalization(
            new LocalizationWeaponText(
                id: "Geralt Steel Sword",
                name: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Geralt's Steel Sword"},
                    {ModLanguage.Chinese, "杰洛特的钢剑"}
                },
                description: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English,
                        "Geralt was once known for carrying two blades—steel for men, silver for monsters. " +
                        "But after being stranded in Aldor, he only had time to commission a well-balanced steel sword from a local blacksmith."
                    },
                    {ModLanguage.Chinese,
                        "杰洛特过去总是佩带双剑——钢剑对付人类，银剑斩杀怪物。 " +
                        "然而刚流落奥尔多时，他仅来得及请当地铁匠为自己打造一柄趁手的钢剑。"
                    }
                }
            )
        );
        // sprite中心点设置
        UndertaleSprite swordSprite = Msl.GetSprite("s_loot_geraltsteelsword");
        swordSprite.OriginX = 17;
        swordSprite.OriginY = 9;
    }
}
