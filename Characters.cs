using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddCharacters()
    {
        AddPerk_ProfessionalWitcher();
        AddGeralt();
    }

    private void AddGeralt()
    {
        string[] sprites = new string[4] {
            "s_GeraltHead_normal",
            "s_GeraltHead_helmet_normal",
            "s_GeraltHead_helmet_blood",
            "s_GeraltHead_blood"
        };

        for (int i = 0; i < sprites.Length; i++)
        {
            UndertaleSprite sprite = Msl.GetSprite(sprites[i]);
            sprite.OriginX = 15;
            sprite.OriginY = 36;
        }

        UndertaleSprite body = Msl.GetSprite("s_GeraltBody");
        body.OriginX = 22;
        body.OriginY = 34;
        body.CollisionMasks.RemoveAt(0);
        body.IsSpecialType = true;
        body.SVersion = 3;
        body.GMS2PlaybackSpeed = 1;
        body.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        UndertaleGameObject o_white_wolf = Msl.GetObject("o_white_wolf");

        o_white_wolf.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                c_index = 4
                with (o_inventory)
                {
                    if (!is_start_equipment)
                    {
                        scr_atr_set_simple(""Head"", ""s_GeraltHead"")
                        scr_atr_set_simple(""CorpseSprite"", sprite_get_name(s_Geralt_dead))
                        scr_atr_set_simple(""BodySprite"", sprite_get_name(s_GeraltBody))
                        scr_playerSpriteInit()
                        if (!global.is_load_game)
                        {
                            with(scr_inventory_add_item(o_inv_witcher_medallion_wolf))
                            {
                                onStart_equipped = true
                                if (!equipped)
                                    event_user(5)
                            }
                            with (scr_equip(""Worn Cloak"", (1 << 0)))
                                scr_inv_atr_set(""Duration"", 100)
                            // Spawn to backpack first (same path as Kelvin sword) for pickup behavior parity.
                            with (scr_inventory_add_weapon(""Geralt Steel Sword"", (1 << 0)))
                                scr_inv_atr_set(""Duration"", 100)
                            with (scr_equip(""Fine Shirt"", (1 << 0)))
                                scr_inv_atr_set(""Duration"", 100)
                            with (scr_equip(""Apprentice Cowl"", (1 << 0)))
                                scr_inv_atr_set(""Duration"", 100)
                            with (scr_equip(""Peasant Shoes"", (1 << 0)))
                                scr_inv_atr_set(""Duration"", 100)
                            with (scr_inventory_add_weapon(""Training Crossbow"", (1 << 0)))
                                scr_inv_atr_set(""Duration"", 100);
                            with (scr_inventory_add_item(o_inv_moneybag))
                                scr_container_add_gold(250)
                            with (scr_inventory_add_weapon(""Peasant Pitchfork"", (1 << 0)))
                                scr_inv_atr_set(""Duration"", 20)
                            scr_inventory_add_item(o_inv_leafshaped_bolts)
                            scr_inventory_add_item(o_inv_dumpling)
                            scr_inventory_add_item(o_inv_wineskin)
                            scr_inventory_add_item(o_inv_splint)
                            scr_inventory_add_item(o_inv_splint)
                            scr_inventory_add_item(o_inv_rag)
                            scr_inventory_add_item(o_inv_salve)
                            scr_inventory_add_item(o_inv_salve)
                            scr_inventory_add_item(o_inv_lockpicks)
                            scr_inventory_add_item(o_inv_map_osbrook)
                        }
                        else
                            scr_load_player()
                        with (other.id)
                            alarm[11] = 3
                    }
                    is_start_equipment = true
                }
                sprite_index = __asset_get_index(scr_atr(""BodySprite""))
                if (!variable_instance_exists(id, ""medallion_turns""))
                    medallion_turns = 60
            "),

            new MslEvent(eventType: EventType.Other, subtype: 12, code: @"
                event_inherited()

                if (--medallion_turns > 0)
                    exit;

                medallion_turns = 60

                with (o_inv_witcher_medallion_wolf)
                {
                    if (equipped)
                    {
                        event_user(14)

                        if (secret_room > 0)
                            with (other)
                                scr_random_speech(""perceiveSecretRoomGeralt"", 100)
                        else if (enemy_count >= 5)
                            with (other)
                                scr_random_speech(""perceiveMassEnemyGeralt"", 35)
                        else if (enemy_count >= 3)
                            with (other)
                                scr_random_speech(""perceiveMediumEnemyGeralt"", 35)
                        else if (enemy_count >= 1)
                            with (other)
                                scr_random_speech(""perceiveFewEnemyGeralt"", 35)
                        
                    }
                }
            ")
        );

        Msl.LoadGML("gml_Object_o_dataLoader_Other_10")
            .MatchFrom("global.player_class = ")
            .InsertAbove(@"scr_classCreate(
                o_white_wolf, s_Geralt, ""Geralt"", ""Male"", ""Human"", ""Aldor"", ""WhiteWolf"",
                10, 11, 10, 11, 11,
                [
                    global.swords_tier1, global.swords2h_tier1, global.daggers_tier1, global.bows_tier1, global.armor_tier1, global.athletics_tier1, global.combat_tier1,
                    [""Witcher"", o_skill_quen_sign_ico, o_skill_axii_sign_ico, o_skill_yrden_sign_ico, o_skill_aard_sign_ico, o_skill_igni_sign_ico, o_skill_trial_of_grasses]
                ],
                [o_perk_professional_witcher], (1 << 0), false)")
            .Save();

        /*
        Msl.LoadAssemblyAsString("gml_Object_c_roadAltar_Other_10")
            .MatchFrom("pop.v.v local._playerPrayKey")
            .InsertBelow(@"push.s ""raceKey___""
conv.s.v
call.i gml_Script_scr_actionsLogUpdate(argc=1)
popz.v")
            .Save();
        */
    }

}