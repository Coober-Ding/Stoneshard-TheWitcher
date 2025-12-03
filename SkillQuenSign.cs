using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddSkill_Quen_Sign()
    {
        AdjustSkillIcon("s_skills_quen_sign");
        AdjustSpellCastSprites("s_quensign_cast_", 15, 62);

        Msl.InjectTableSkillsLocalization(
            new LocalizationSkill(
                id: "Quen_Sign",
                name: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Quen Sign"},
                    {ModLanguage.Chinese, "昆恩法印"}
                },
                description: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, @"Opens the menu for ~w~crafting weapon coating oil~/~."},
                    {ModLanguage.Chinese, @"触发~w~6~/~回合耐久为~lg~/*Shield_Duration*/~/~点的~lg~“法力护盾”~/~：##精力自动恢复~r~-25%~/~##触发时有~w~/*Cure_Chance*/%~/~的概率会消除~r~流血~/~和~o~着火~/~状态。##护盾会吸收所有物理、自然和魔法伤害，直到破损为止。"}
                }
            )
        );

        Msl.InjectTableSpeechesLocalization(
            new LocalizationSpeech(
                id: "Quen_Sign",
                new Dictionary<ModLanguage, string> {
                    {ModLanguage.English, "QUEN!"},
                    {ModLanguage.Chinese, "昆恩！"}
                }
            )
        );

        Msl.InjectTableSpeechesLocalization(
            new LocalizationSpeech(
                id: "MC_Quen_Sign",
                new Dictionary<ModLanguage, string> {
                    {ModLanguage.English, "QU...EN..."},
                    {ModLanguage.Chinese, "昆...恩..."}
                }
            )
        );

        Msl.InjectTableSkillsStats(
            id: "Quen_Sign",
            Object: "o_b_magical_shield",
            hook: Msl.SkillsStatsHook.MAGICMASTERY,
            Range: "0",
            KD: 8,
            MP: 20,
            Duration: 6,
            Class: Msl.SkillsStatsClass.spell,
            Branch: "magic_mastery",
            Spell: true
        );

        UndertaleGameObject o_skill_quen_sign = Msl.AddObject(
            name: "o_skill_quen_sign",
            parentName: "o_skill",
            spriteName: "s_skills_quen_sign",
            isVisible: true,
            isPersistent: false,
            isAwake: true,
            collisionShapeFlags: CollisionShapeFlags.Circle
        );

        //string Shield_Duration = "40 * power(owner.Magic_Power / 100, 3)";
        string Shield_Duration = "30 * power(owner.Magic_Power / 100, 2)";
        string Cure_Chance = "45 * owner.Magic_Power / 100";

        o_skill_quen_sign.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                skill = ""Quen_Sign""
                scr_skill_atr(""Quen_Sign"")
                ds_list_add(attribute, ds_map_find_value(global.attribute, ""Magic_Power""))
                ignore_interact = true
                is_moving = false
            "),

            new MslEvent(eventType: EventType.Other, subtype: 13, code: @$"
                event_inherited()
                scr_stop_player()
                if instance_exists(owner)
                {{
                    var _cure = is_crit ? 1 : scr_chance_value({Cure_Chance})
                    if (_cure)
                    {{
                        with (scr_instance_exists_in_list(o_db_bleed_parent, owner.buffs))
                            instance_destroy()
                        with (scr_instance_exists_in_list(o_db_gaping_wound, owner.buffs))
                            instance_destroy()
                        with (scr_instance_exists_in_list(o_db_fire, owner.buffs))
                            instance_destroy()
                    }}
                }}
            "),

            new MslEvent(eventType: EventType.Other, subtype: 17, code: @$"
                if instance_exists(owner)
                {{
                    ds_map_replace(data, ""Shield_Duration"", {Shield_Duration})
                    ds_map_replace(data, ""Cure_Chance"", {Cure_Chance})
                }}
                event_inherited()
            ")
        );

        UndertaleGameObject o_skill_quen_sign_ico = Msl.AddObject(
            name: "o_skill_quen_sign_ico",
            parentName: "o_skill_ico",
            spriteName: "s_skills_quen_sign",
            isVisible: true, 
            isPersistent: false, 
            isAwake: true,
            collisionShapeFlags: CollisionShapeFlags.Circle
        );

        o_skill_quen_sign_ico.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                child_skill = o_skill_quen_sign
                event_perform_object(child_skill, ev_create, 0)
            ")
        );

        // Buff: Magical Shield

        Msl.InjectTableModifiersLocalization(
            new LocalizationModifier(
                id: "o_b_magical_shield",
                name: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Magical Shield"},
                    {ModLanguage.Chinese, "法力护盾"}
                },
                description: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, @"Magical Shield absorbs all physical, natural and magical damage until broken. Current duration: ~lg~/*Shield_Duration*///*Max_Duration*/~/~"},
                    {ModLanguage.Chinese, @"法力护盾会吸收所有物理、自然和魔法伤害，直到破损为止。当前耐久：~lg~/*Shield_Duration*///*Max_Duration*/~/~"}
                }
            )
        );

        UndertaleGameObject o_onUnitEffect_MagicalShield_BG = Msl.AddObject(
            name: "o_onUnitEffect_MagicalShield_BG",
            parentName: "o_onUnitEffectSprite",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        o_onUnitEffect_MagicalShield_BG.ApplyEvent(
            new MslEvent(eventType: EventType.Other, subtype: 25, code: @"
                event_inherited();
                spriteIndexStart = s_magical_shield_bg_start
                spriteIndexLoop = s_magical_shield_bg_loop
                spriteIndexEnd = s_magical_shield_bg_end
            ")
        );

        UndertaleGameObject o_onUnitEffect_MagicalShield = Msl.AddObject(
            name: "o_onUnitEffect_MagicalShield",
            parentName: "o_onUnitEffectSprite",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        o_onUnitEffect_MagicalShield.ApplyEvent(
            new MslEvent(eventType: EventType.Other, subtype: 25, code: @"
                event_inherited();
                spriteIndexStart = s_magical_shield_start
                spriteIndexLoop = s_magical_shield_loop
                spriteIndexEnd = s_magical_shield_end
            ")
        );

        UndertaleGameObject o_onUnitEffect_MagicalShieldo_Lighting = Msl.AddObject(
            name: "o_onUnitEffect_MagicalShieldo_Lighting",
            parentName: "o_onUnitEffectSprite",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        o_onUnitEffect_MagicalShieldo_Lighting.ApplyEvent(
            new MslEvent(eventType: EventType.Other, subtype: 25, code: @"
                event_inherited();
                spriteIndexStart = s_magical_shield_lighting_start
                spriteIndexLoop = s_magical_shield_lighting_loop
                spriteIndexEnd = s_magical_shield_lighting_end
            ")
        );

        UndertaleGameObject o_b_magical_shield = Msl.AddObject(
            name: "o_b_magical_shield",
            parentName: "o_magical_buff",
            spriteName: "s_b_magical_shield",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        o_b_magical_shield.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                stack = 1
                duration = 6
                should_execute = false
                Max_Duration = 25
                Shield_Duration = Max_Duration
                scr_buff_atr()
                buff_snd = snd_abillity_shield_block
                magicalShieldAnimationBG = noone
                magicalShieldAnimation = noone
                magicalShieldAnimationLighting = noone
            "),

            new MslEvent(eventType: EventType.Destroy, subtype: 0, code: @"
                event_inherited()
                if instance_exists(owner)
                {
                    scr_audio_play_at(choose(snd_spell_stone_armor_explode_1, snd_spell_stone_armor_explode_2))
                }

                magicalShieldAnimationBG = scr_onUnitEffectDestroy(magicalShieldAnimationBG)
                magicalShieldAnimation = scr_onUnitEffectDestroy(magicalShieldAnimation)
                magicalShieldAnimationLighting = scr_onUnitEffectDestroy(magicalShieldAnimationLighting)
            "),

            new MslEvent(eventType: EventType.Alarm, subtype: 2, code: @$"
                event_inherited()
                ds_map_clear(data)
                ds_map_add(data, ""MP_Restoration"", -25)
                if instance_exists(owner)
                {{
                    magicalShieldAnimationBG = scr_onUnitEffectCreate(
                        owner.id, o_onUnitEffect_MagicalShield_BG, 2, 0, 0, true)
                    magicalShieldAnimation = scr_onUnitEffectCreate(
                        owner.id, o_onUnitEffect_MagicalShield, -350, 0, 0, true)
                    magicalShieldAnimationLighting = scr_onUnitEffectCreate(
                        owner.id, o_onUnitEffect_MagicalShieldo_Lighting, -351, 0, 0, true)
                    Max_Duration = {Shield_Duration}

                    if (is_crit)
                        Max_Duration *= max(1, owner.Miracle_Power / 100)

                    Shield_Duration = Max_Duration
                }}
            "),

            new MslEvent(eventType: EventType.Other, subtype: 14, code: @"
                event_inherited()
                if (should_execute)
                {
                    with (target)
                        scr_guiAnimation_ext(x, y, s_magical_shield_proc)

                    Shield_Duration -= damage
                }
            "),

            new MslEvent(eventType: EventType.Other, subtype: 25, code: @"
                event_inherited()
                if instance_exists(owner)
                {
                    ds_map_replace(text_map, ""Max_Duration"", Max_Duration)
                    ds_map_replace(text_map, ""Shield_Duration"", Shield_Duration)
                }
            ")
        );

        // Melee attack
        Msl.LoadGML("gml_GlobalScript_scr_attack")
            .MatchFrom("var DMG_r = 0")
            .InsertAbove(@"
                with (scr_instance_exists_in_list(o_b_magical_shield, _target.buffs))
                {
                    should_execute = true
                    P_proc = false
                }
            ")
            .Save();

        // Spell attack
        Msl.LoadGML("gml_GlobalScript_scr_skill_damage")
            .MatchFrom("    var dmg = 0")
            .InsertBelow(@"
    with (scr_instance_exists_in_list(o_b_magical_shield, _target.buffs))
    {
        should_execute = true
    }")
            .Save();

        // Arrow attack
        Msl.LoadGML("gml_Object_o_arrow_Other_10")
            .MatchFrom("if P_proc")
            .InsertAbove(@"
            with (scr_instance_exists_in_list(o_b_magical_shield, _target.buffs))
            {
                should_execute = true
                P_proc = false
            }")
            .Save();

        // Throwed item attack
        Msl.LoadGML("gml_Object_o_throwed_loot_Other_10")
            .MatchFrom("if P_proc")
            .InsertAbove(@"
            with (scr_instance_exists_in_list(o_b_magical_shield, _target.buffs))
            {
                should_execute = true
                P_proc = false
            }")
            .Save();

        // Finish execution

        Msl.LoadAssemblyAsString("gml_GlobalScript_scr_damage_calculation")
            .MatchFrom("pop.v.v self.is_deal_damage")
            .InsertBelow(@"pushbltn.v builtin.argument0
pushi.e -9
push.v [stacktop]self.buffs
pushi.e o_b_magical_shield
conv.i.v
call.i gml_Script_scr_instance_exists_in_list(argc=2)
pushi.e -9
pushenv [1097]

:[1095]
pushi.e 0
pop.v.b self.should_execute
pushi.e 0
conv.i.v
pushi.e 4
conv.i.v
pushi.e snd_block_1
conv.i.v
pushi.e snd_block_2
conv.i.v
pushi.e snd_block_3
conv.i.v
pushi.e snd_block_4
conv.i.v
call.i choose(argc=4)
call.i audio_play_sound(argc=3)
popz.v
push.v self.Shield_Duration
pushi.e 0
cmp.i.v LTE
bf [1097]

:[1096]
call.i instance_destroy(argc=0)
popz.v

:[1097]
popenv [1095]")
            .Save();


        // Throwed net
        Msl.LoadGML("gml_Object_o_net_throw_Alarm_0")
            .MatchFrom("_shield = scr_instance_exists_in_list")
            .ReplaceBy("_shield = (scr_instance_exists_in_list(o_b_aether_shield) || scr_instance_exists_in_list(o_b_magical_shield))")
            .Save();

        // Throwed web
        Msl.LoadGML("gml_Object_o_web_spit_Alarm_0")
            .MatchFrom("_shield = scr_instance_exists_in_list")
            .ReplaceBy("_shield = (scr_instance_exists_in_list(o_b_aether_shield) || scr_instance_exists_in_list(o_b_magical_shield))")
            .Save();

        // Damage reduction by magical shield
        Msl.LoadGML("gml_GlobalScript_scr_damage_physical_calc")
            .MatchFrom("var _dmgReal = math_round(argument3 * (max(((argument1 - argument4 * _partDamageNormalizer - argument0.tmpDEF * _partDamageNormalizer * (1 - Armor_Piercing / 100)) * (1 - argument2 / 100)), 0)))")
            .InsertAbove(@"
        with (scr_instance_exists_in_list(o_b_magical_shield, argument0.buffs))
        {
            damage = argument1
            event_user(4)
            argument1 = 0
        }
            ")
            .MatchFrom("var _dmgReal = math_round(argument3 * (max(((argument1 - argument4 * _partDamageNormalizer - 0.5 * argument0.tmpDEF * _partDamageNormalizer * argument6) * (1 - argument2 / 100)), 0)))")
            .InsertAbove(@"
        with (scr_instance_exists_in_list(o_b_magical_shield, argument0.buffs))
        {
            damage = argument1
            event_user(4)
            argument1 = 0            
        }
            ")
            .Save();

        UndertaleSprite animation = Msl.GetSprite("s_magical_shield_proc");
        animation.CollisionMasks.RemoveAt(0);
        animation.OriginX = 20;
        animation.OriginY = 15;
        animation.IsSpecialType = true;
        animation.SVersion = 3;
        animation.GMS2PlaybackSpeed = 0.5f;
        animation.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        string[] sprites = {
            "s_magical_shield_bg_start", "s_magical_shield_bg_loop", "s_magical_shield_bg_end",
            "s_magical_shield_start", "s_magical_shield_loop", "s_magical_shield_end",
            "s_magical_shield_lighting_start", "s_magical_shield_lighting_loop", "s_magical_shield_lighting_end"
        };

        foreach (var spr in sprites)
        {
            animation = Msl.GetSprite(spr);
            animation.CollisionMasks.RemoveAt(0);
            animation.OriginX = 41;
            animation.OriginY = 51;
            animation.IsSpecialType = true;
            animation.SVersion = 3;
            animation.GMS2PlaybackSpeed = 0.3f;
            animation.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;
        }

        AdjustBuffIcon("s_b_magical_shield");
    }

    private void AdjustBuffIcon(string name)
    {
        UndertaleSprite buff_ico = Msl.GetSprite(name);
        buff_ico.OriginX = 13;
        buff_ico.OriginY = 13;
        buff_ico.IsSpecialType = true;
        buff_ico.SVersion = 3;
        buff_ico.GMS2PlaybackSpeed = 1;
        buff_ico.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;
    }
}