using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddNewNPCs()
    {
        AddNewNPC_Idarran();
        EditRoom_BrynnUniversityCellar();
        AddTask_Idarran();
        EditRoom_BrynnNW();
    }

    private void AddNewNPC_Idarran()
    {
        UndertaleSprite s_npc_Idarran = Msl.GetSprite("s_npc_Idarran");
        s_npc_Idarran.CollisionMasks.RemoveAt(0);
        s_npc_Idarran.IsSpecialType = true;
        s_npc_Idarran.SVersion = 3;
        s_npc_Idarran.Width = 44;
        s_npc_Idarran.Height = 49;
        s_npc_Idarran.MarginLeft = 11;
        s_npc_Idarran.MarginRight = 31;
        s_npc_Idarran.MarginBottom = 39;
        s_npc_Idarran.MarginTop = 5;
        s_npc_Idarran.OriginX = 23;
        s_npc_Idarran.OriginY = 36;
        s_npc_Idarran.GMS2PlaybackSpeed = 1;
        s_npc_Idarran.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        UndertaleSprite s_npc_Idarran_fight = Msl.GetSprite("s_npc_Idarran_fight");
        s_npc_Idarran_fight.CollisionMasks.RemoveAt(0);
        s_npc_Idarran_fight.IsSpecialType = true;
        s_npc_Idarran_fight.SVersion = 3;
        s_npc_Idarran_fight.Width = 44;
        s_npc_Idarran_fight.Height = 49;
        s_npc_Idarran_fight.MarginLeft = 11;
        s_npc_Idarran_fight.MarginRight = 31;
        s_npc_Idarran_fight.MarginBottom = 39;
        s_npc_Idarran_fight.MarginTop = 5;
        s_npc_Idarran_fight.OriginX = 23;
        s_npc_Idarran_fight.OriginY = 36;
        s_npc_Idarran_fight.GMS2PlaybackSpeed = 1;
        s_npc_Idarran_fight.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        UndertaleSprite s_npc_Idarran_ko = Msl.GetSprite("s_npc_Idarran_ko");
        s_npc_Idarran_ko.CollisionMasks.RemoveAt(0);
        s_npc_Idarran_ko.IsSpecialType = true;
        s_npc_Idarran_ko.SVersion = 3;
        s_npc_Idarran_ko.Width = 42;
        s_npc_Idarran_ko.Height = 42;
        s_npc_Idarran_ko.MarginLeft = 7;
        s_npc_Idarran_ko.MarginRight = 33;
        s_npc_Idarran_ko.MarginBottom = 31;
        s_npc_Idarran_ko.MarginTop = 14;
        s_npc_Idarran_ko.OriginX = 22;
        s_npc_Idarran_ko.OriginY = 24;
        s_npc_Idarran_ko.GMS2PlaybackSpeed = 1;
        s_npc_Idarran_ko.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        UndertaleGameObject o_npc_Idarran = Msl.AddObject(
            name: "o_npc_Idarran",
            parentName: "o_npc_brynn",
            spriteName: "s_npc_Idarran",
            isVisible: true,
            isPersistent: false,
            isAwake: true,
            collisionShapeFlags: CollisionShapeFlags.Circle
        );

        o_npc_Idarran.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                name = ds_map_find_value(global.npc_info, ""Idarran"")
                occupation = ""geneticist""
                avatar = s_npc_Idarran_P
                ds_list_clear(myfloor_list)
                ds_list_add(myfloor_list, ""H1"", ""H1"", ""H1"", ""H1"")
                myfloor = ""H1""
                dialog_id = ""geneticist_idarran""
                scr_npc_set_global_info(""idarran_chats_left"", 3)
                grass_target = noone
                flee_limit = 0
                ai_script = scr_enemy_choose_state

                chat = true
                rumors = false
                scr_buying_loot_category(
                    ""alcohol"", ""beverage"", ""drug"", ""ingredient"",
                    ""jewelry"", ""valuable"", ""medicine"", ""potion"")
                ds_list_add(singular_stock_list, o_inv_book_witcher1, o_inv_book_witcher2, o_inv_caravan_alchemy)
                scr_npc_gold_init(200, 600)
                Restock_Time = 24
                Ingredient_BP_Mod += 0.33
                Ingredient_SP_Mod += 0.33
                Food_SP_Mod += 0.33
                Treatise_SP_Mod += 0.33
                Alcohol_SP_Mod += 0.33
                Selling_Prices += 0.1

                scr_create_skill_map(""Discharge"")
                scr_create_skill_map(""Impulse"")
                scr_create_skill_map(""Short_Circuit"")
                scr_create_skill_map(""Chain_Lightning"")
                scr_create_skill_map(""Tempest"")
                scr_create_skill_map(""Curse"")
                scr_create_skill_map(""Life_Leech"")
                scr_create_skill_map(""Seal_of_Shackles"")
                scr_create_skill_map(""Seal_of_Power"")
                scr_create_skill_passive(o_pass_skill_dispersion)
                scr_create_skill_passive(o_pass_skill_residual_charge)
                scr_create_skill_passive(o_pass_skill_unlimited_power)
                scr_create_skill_passive(o_pass_skill_inner_reserves)
            "),

            new MslEvent(eventType: EventType.PreCreate, subtype: 0, code: @"
                event_inherited()
                descID = ""student""
            "),

            new MslEvent(eventType: EventType.Alarm, subtype: 1, code: @"
                event_inherited()
                scr_npc_set_global_info(""idarran_chats_left"", irandom_range(1, 3))
            "),

            new MslEvent(eventType: EventType.Other, subtype: 19, code: @"
                event_inherited()
                scr_selling_loot_object(
                    o_inv_antitoxin, irandom_range(1, 2),
                    o_inv_herbal, irandom_range(1, 3),
                    o_inv_antivenom, irandom_range(2, 3),
                    o_inv_inhaler, irandom_range(1, 2),

                    o_inv_oil, irandom_range(1, 3),
                    o_inv_spirit, irandom_range(2, 4),

                    o_inv_stool, irandom_range(1, 3),
                    o_inv_flyagaric, irandom_range(1, 3),
                    o_inv_henbane, irandom_range(1, 3),
                    o_inv_citrus, irandom_range(0, 1),
                    o_inv_ginger, irandom_range(0, 1),
                    o_inv_hornet_honey, irandom_range(0, 1),
                    o_inv_honey, irandom_range(1, 3),
                    o_inv_ghoul_heart, irandom_range(0, 1),
                    o_inv_harpy_egg_raw, irandom_range(0, 1),
                    o_inv_troll_gland, irandom_range(0, 1),
                    o_inv_gulon_liver, irandom_range(0, 1),
                    o_inv_mandibles, irandom_range(0, 1),

                    choose(o_inv_hanged_man_venom, o_inv_vampire_oil), 1,
                    choose(o_inv_necrophage_oil, o_inv_specter_oil, o_inv_insectoid_oil), 1,
                    choose(o_inv_hybrid_oil, o_inv_ogroid_oil), 1,
                    choose(o_inv_thunderbolt_potion, o_inv_blizzard_potion, o_inv_petri_philter), 1,
                    choose(o_inv_swallow_potion, o_inv_tawny_owl, o_inv_golden_oriole), 1,
                    choose(o_inv_ghoul_decoction, o_inv_crawler_decoction,
                        o_inv_harpy_decoction, o_inv_troll_decoction, o_inv_gulon_decoction), 1
                )
                scr_selling_loot_category(""herb"", irandom_range(16, 24))
            ")
        );

        /*
        Utils.InjectItemsToTable(
            table: "gml_GlobalScript_table_mobs_stats",
            anchor: "// NPCS;;;;;;;;;;;;",
            defaultKey: null,
            new Dictionary<string, string>
            {
                ["name"] = "Idarran",
                ["Tier"] = "5",
                ["ID"] = "o_npc_Idarran",
                ["type"] = "human",
                ["faction"] = "GrandMagistrate",
                ["pattern"] = "Mage",
                ["weapon"] = "2hStaff",
                ["armor"] = "Light",
                ["size"] = "medium",
                ["matter"] = "flesh",
                ["VIS"] = "12",
                ["HP"] = "145",
                ["MP"] = "220",
                ["Head_DEF"] = "3",
                ["Body_DEF"] = "6",
                ["Arms_DEF"] = "3",
                ["Legs_DEF"] = "6",
                ["Hit_Chance"] = "86",
                ["EVS"] = "40",
                ["PRR"] = "27",
                ["Block_Power"] = "20",
                ["Block_Recovery"] = "50",
                ["Crit_Avoid"] = "20",
                ["CRT"] = "5",
                ["CRTD"] = "15",
                ["CTA"] = "10",
                ["FMB"] = "6",
                ["Magic_Power"] = "100",
                ["Miscast_Chance"] = "-20",
                ["Miracle_Chance"] = "35",
                ["Miracle_Power"] = "50",
                ["MP_Restoration"] = "40",
                ["Cooldown_Reduction"] = "-70",
                ["Fortitude"] = "50",
                ["Health_Restoration"] = "15",
                ["Manasteal"] = "20",
                ["Bleeding_Resistance"] = "25",
                ["Knockback_Resistance"] = "25",
                ["Stun_Resistance"] = "30",
                ["Daze_Chance"] = "35",
                ["Knockback_Chance"] = "35",
                ["STR k"] = "0.1",
                ["AGL k"] = "0.15",
                ["Vitality k"] = "0.15",
                ["PRC k"] = "0.4",
                ["WIL k"] = "0.4",
                ["Checksum"] = "1.2",
                ["STR"] = "8",
                ["AGL"] = "11",
                ["Vitality"] = "11",
                ["PRC"] = "30",
            }
        );
        */

    }

    private void EditRoom_BrynnUniversityCellar()
    {
        UndertaleRoom room = Msl.GetRoom("r_BrynnUniversityCellar");
        UndertaleRoom.Layer LayerTarget = Msl.GetLayer(room, UndertaleRoom.LayerType.Instances, "Targets");
        UndertaleRoom.Layer LayerNPC = Msl.GetLayer(room, UndertaleRoom.LayerType.Instances, "NPC");

        UndertaleRoom.GameObject student04uni = LayerNPC.InstancesData.Instances.First(
                t => t.ObjectDefinition.Name.Content == "o_npc_Student04UNI");
        LayerNPC.InstancesData.Instances.Remove(student04uni);
        room.GameObjects.Remove(student04uni);

        UndertaleRoom.GameObject stand_position = Msl.ThrowIfNull(
            LayerTarget.InstancesData.Instances.First(
                t => t.ObjectDefinition.Name.Content == "o_NPC_target" && t.X == 338 && t.Y == 520));

        UndertaleRoom.GameObject sitedown_position = Msl.ThrowIfNull(
            LayerTarget.InstancesData.Instances.First(
                t => t.ObjectDefinition.Name.Content == "o_NPC_target" && t.X == 520 && t.Y == 182));

        UndertaleSprite s_npc_Idarran_alchemy = Msl.GetSprite("s_npc_Idarran_alchemy");
        s_npc_Idarran_alchemy.CollisionMasks.RemoveAt(0);
        s_npc_Idarran_alchemy.IsSpecialType = true;
        s_npc_Idarran_alchemy.SVersion = 3;
        s_npc_Idarran_alchemy.Width = 29;
        s_npc_Idarran_alchemy.Height = 52;
        s_npc_Idarran_alchemy.MarginLeft = 4;
        s_npc_Idarran_alchemy.MarginRight = 27;
        s_npc_Idarran_alchemy.MarginBottom = 42;
        s_npc_Idarran_alchemy.MarginTop = 3;
        s_npc_Idarran_alchemy.OriginX = 19;
        s_npc_Idarran_alchemy.OriginY = 42;
        s_npc_Idarran_alchemy.GMS2PlaybackSpeed = 0.3f;
        s_npc_Idarran_alchemy.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        UndertaleSprite s_npc_Idarran_experiment = Msl.GetSprite("s_npc_Idarran_experiment");
        s_npc_Idarran_experiment.CollisionMasks.RemoveAt(0);
        s_npc_Idarran_experiment.IsSpecialType = true;
        s_npc_Idarran_experiment.SVersion = 3;
        s_npc_Idarran_experiment.Width = 29;
        s_npc_Idarran_experiment.Height = 52;
        s_npc_Idarran_experiment.MarginLeft = 4;
        s_npc_Idarran_experiment.MarginRight = 27;
        s_npc_Idarran_experiment.MarginBottom = 42;
        s_npc_Idarran_experiment.MarginTop = 3;
        s_npc_Idarran_experiment.OriginX = 19;
        s_npc_Idarran_experiment.OriginY = 34;
        s_npc_Idarran_experiment.GMS2PlaybackSpeed = 0.3f;
        s_npc_Idarran_experiment.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        UndertaleSprite s_npc_Idarran_work = Msl.GetSprite("s_npc_Idarran_reading");
        s_npc_Idarran_work.CollisionMasks.RemoveAt(0);
        s_npc_Idarran_work.IsSpecialType = true;
        s_npc_Idarran_work.SVersion = 3;
        s_npc_Idarran_work.Width = 48;
        s_npc_Idarran_work.Height = 48;
        s_npc_Idarran_work.MarginLeft = 11;
        s_npc_Idarran_work.MarginRight = 33;
        s_npc_Idarran_work.MarginBottom = 36;
        s_npc_Idarran_work.MarginTop = 9;
        s_npc_Idarran_work.OriginX = 13;
        s_npc_Idarran_work.OriginY = 50;
        s_npc_Idarran_work.GMS2PlaybackSpeed = 0.3f;
        s_npc_Idarran_work.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        UndertaleRoom.GameObject grass_target = room.AddGameObject(
            LayerTarget,
            "o_NPC_target",
            Msl.AddCode("npc_sprite = s_npc_Idarran_experiment", "gml_RoomCC_r_BrynnUniversityCellar_101_Create"),
            x: 650, y: 546
        );

        string creationCode = @$"
            target_array = [{stand_position.InstanceID}, {stand_position.InstanceID}, {stand_position.InstanceID}, {sitedown_position.InstanceID}]
            myfloor_counter = ""H1""
            idle_state = false
            time_period_night = time_period_day
            grass_target = {grass_target.InstanceID}
        ";

        room.AddGameObject(
            LayerNPC,
            "o_npc_Idarran",
            Msl.AddCode(creationCode, "gml_RoomCC_r_BrynnUniversityCellar_100_Create"),
            x: 609, y: 432
        );

        Msl.LoadGML("gml_RoomCC_r_BrynnUniversityCellar_35_Create")
            .MatchFrom("npc_sprite = s_npc_Student04uni_idle")
            .ReplaceBy("npc_sprite = s_npc_Idarran_alchemy")
            .Save();

        Msl.LoadGML("gml_RoomCC_r_BrynnUniversityCellar_34_Create")
            .MatchFrom("npc_sprite = s_npc_Student04uni_work")
            .ReplaceBy("npc_sprite = s_npc_Idarran_reading\nis_rest = true")
            .Save();
    }

    private void EditRoom_BrynnNW()
    {
        UndertaleGameObject o_npc_dialogue_trigger_brynn_genetics = Msl.AddObject(
            name: "o_npc_dialogue_trigger_brynn_genetics",
            parentName: "o_npc_dialogue_trigger",
            spriteName: "s_torchishka",
            isVisible: false,
            isPersistent: false,
            isAwake: true,
            collisionShapeFlags: CollisionShapeFlags.Box
        );

        o_npc_dialogue_trigger_brynn_genetics.Group = 1;

        o_npc_dialogue_trigger_brynn_genetics.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                dialog_id = ""brynn_genetic_recruitment""
            "),

            new MslEvent(eventType: EventType.Other, subtype: 11, code: @"
                if scr_dialogue_complete(""introGeneticExperiment01"")
                {
                    instance_destroy()
                    return;
                }

                if instance_exists(o_player)
                {
                    var _nearest = noone
                    with (o_npc_brynn)
                    {
                        if (occupation == ""student"")
                        {
                            if (visible && scr_tile_distance(o_player, id) <= o_player.VSN)
                                _nearest = id
                        }
                    }

                    if (instance_exists(_nearest))
                    {
                        owner = _nearest
                        event_inherited()
                    }
                }
            ")
        );

        UndertaleRoom room = Msl.GetRoom("r_Brynn_NW");
        UndertaleRoom.Layer LayerTarget = Msl.GetLayer(room, UndertaleRoom.LayerType.Instances, "Targets");
        UndertaleRoom.GameObject trigger = room.AddGameObject(
            LayerTarget,
            "o_npc_dialogue_trigger_brynn_genetics",
            x: 1482, y: 962
        );

        trigger.ScaleX = 9;
        trigger.ScaleY = 12;
        trigger.ImageSpeed = 1;
    }

    private void AddTask_Idarran()
    {
        // Add dialog data
        UndertaleGameObject ob = Msl.AddObject("o_brynn_idarran_dialog_initializer", isPersistent: true);
        Msl.AddNewEvent(ob, "", EventType.Other, 10);
        UndertaleRoom start = Msl.GetRoom("START");
        start.AddGameObject("Instances", ob);

        Msl.LoadGML(Msl.EventName("o_brynn_idarran_dialog_initializer", EventType.Other, 10))
            .MatchAll()
            .InsertBelow(ModFiles, "o_brynn_idarran_dialog_initializer.gml")
            .Save();
        Msl.LoadGML("gml_Object_o_dataLoader_Other_10")
            .MatchFrom("scr_dialogue_loader_init")
            .InsertBelow("with (o_brynn_idarran_dialog_initializer) { event_user(0) }")
            .Save();
    }
}