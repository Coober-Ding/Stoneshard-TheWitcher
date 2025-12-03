using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddSkill_Aard_Sign()
    {
        AdjustSkillIcon("s_skills_aard_sign");
        AdjustSpellCastSprites("s_aardsign_cast_", 15, 62);

        Msl.InjectTableSkillsLocalization(
            new LocalizationSkill(
                id: "Aard_Sign",
                name: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Aard Sign"},
                    {ModLanguage.Chinese, "阿尔德法印"}
                },
                description: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, @"No translation"},
                    {ModLanguage.Chinese, string.Join("##",
                        "产生念动冲击波，对一定范围内的敌人造成~w~/*Blunt_Damage*/点钝击伤害~/~，本次伤害击退几率~w~/*Knockback_Chance*/%~/~、击晕几率~w~/*Daze_Chance*/%~/~、失衡几率~w~/*Stagger_Chance*/%~/~。",
                        "若目标处于~r~慌乱~/~、~r~眩晕~/~等负面精神状态，念动冲击波会对目标额外造成~p~/*Psionic_Damage*/点灵能伤害~/~。",
                        "命中目标会令其~w~4~/~回合内控制抗性~r~/*Stun_Resistance*/%~/~、位移抗性~r~/*Stun_Resistance*/%~/~、灵能抗性~r~/*Psionic_Resistance*/%~/~。这个效果可以叠加，最多~w~2~/~层。",
                        "每有一个目标被法印直接击晕，所有技能当前剩余冷却时间便缩短~lg~2~/~个回合。"
                    )}
                }
            )
        );

        Msl.InjectTableSpeechesLocalization(
            new LocalizationSpeech(
                id: "Aard_Sign",
                new Dictionary<ModLanguage, string> {
                    {ModLanguage.English, "AARD!"},
                    {ModLanguage.Chinese, "阿尔德！"}
                }
            )
        );

        Msl.InjectTableSpeechesLocalization(
            new LocalizationSpeech(
                id: "MC_Aard_Sign",
                new Dictionary<ModLanguage, string> {
                    {ModLanguage.English, "A...ERD..."},
                    {ModLanguage.Chinese, "奥...儿得..."}
                }
            )
        );

        Msl.InjectTableSkillsStats(
            id: "Aard_Sign",
            Object: "o_aard_sign_birth",
            hook: Msl.SkillsStatsHook.MAGICMASTERY,
            Target: Msl.SkillsStatsTarget.TargetArea,
            Range: "2",
            AOE_Lenght: 2,
            AOE_Width: 3,
            Pattern: Msl.SkillsStatsPattern.pyramid,
            KD: 8,
            MP: 16,
            Reserv: 18,
            Duration: 0,
            Class: Msl.SkillsStatsClass.spell,
            Branch: "magic_mastery",
            Spell: true,
            AP: "x",
            Bonus_Range: false,
            Crime: true
        );

        UndertaleGameObject o_skill_aard_sign = Msl.AddObject(
            name: "o_skill_aard_sign",
            parentName: "o_skill",
            spriteName: "s_skills_aard_sign",
            isVisible: true,
            isPersistent: false,
            isAwake: true,
            collisionShapeFlags: CollisionShapeFlags.Circle
        );

        UndertaleGameObject o_skill_aard_sign_ico = Msl.AddObject(
            name: "o_skill_aard_sign_ico",
            parentName: "o_skill_ico",
            spriteName: "s_skills_aard_sign",
            isVisible: true,
            isPersistent: false,
            isAwake: true,
            collisionShapeFlags: CollisionShapeFlags.Circle
        );

        UndertaleGameObject o_aard_sign_birth = Msl.AddObject(
            name: "o_aard_sign_birth",
            parentName: "o_spellbirth",
            spriteName: "s_aardsign_cast_cast",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        UndertaleGameObject o_aard_wave = Msl.AddObject(
            name: "o_aard_wave",
            parentName: "o_aoe_spell",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        string Blunt_Damage = "13 * owner.Magic_Power / 100";
        string Psionic_Damage = "owner.WIL * (owner.Magic_Power + owner.Psimantic_Power) / 100";
        string Knockback_Chance = "80 * owner.Magic_Power / 100";
        string Stagger_Chance = "100 * owner.Magic_Power / 100";
        string Daze_Chance = "20 * owner.Magic_Power / 100";
        string Stun_Resistance = "-(owner.WIL * owner.Magic_Power / 100)";
        string Psionic_Resistance = "-(5 * owner.WIL * owner.Magic_Power / 100)";

        o_skill_aard_sign.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                skill = ""Aard_Sign""
                startcast_sprite_tag = ""s_aardsign_cast_""
                scr_skill_atr(""Aard_Sign"")
                ds_list_add(attribute,
                    ds_map_find_value(global.attribute, ""Magic_Power""),
                    ds_map_find_value(global.attribute, ""Psimantic_Power""),
                    ds_map_find_value(global.attribute, ""WIL""))
                ignore_interact = true
                is_moving = false
                main_spell = o_aard_wave
                damage_type = ""Blunt_Damage""
                click_snd = snd_skill_sealofshackles_startcast
            "),

            new MslEvent(eventType: EventType.Other, subtype: 17, code: @$"
                if (instance_exists(owner))
                {{
                    ds_map_replace(data, ""Blunt_Damage"", {Blunt_Damage})
                    ds_map_replace(data, ""Knockback_Chance"", {Knockback_Chance})
                    ds_map_replace(data, ""Stagger_Chance"", {Stagger_Chance})
                    ds_map_replace(data, ""Daze_Chance"", {Daze_Chance})
                    ds_map_replace(data, ""Stun_Resistance"", {Stun_Resistance})
                    ds_map_replace(data, ""Psionic_Damage"", {Psionic_Damage})
                    ds_map_replace(data, ""Psionic_Resistance"", {Psionic_Resistance})
                }}

                event_inherited()
            "),

            new MslEvent(eventType: EventType.Other, subtype: 10, code: ModFiles.GetCode("o_skill_other_10.gml"))
        );

        o_skill_aard_sign_ico.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                child_skill = o_skill_aard_sign
                event_perform_object(child_skill, ev_create, 0)
            ")
        );

        o_aard_sign_birth.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                scr_light_off()
                blend = make_colour_rgb(231, 137, 22)
                cast_frame = 1
                spell = o_aard_wave
                is_flying = true
            "),

            new MslEvent(eventType: EventType.Other, subtype: 10, code: @"
                if (!is_execute)
                {
                    with (owner)
                    {
                        is_life = false
                        // scr_audio_play_at(snd_skill_tremor_strike)
                        // scr_audio_play_at(snd_spell_stone_armor_explode_1)
                        scr_audio_play_at(snd_golem_near_death_explode_2)
                    }
                }

                event_inherited()
            "),

            new MslEvent(eventType: EventType.Destroy, subtype: 0, code: @"
                with (owner)
                    is_life = true
            ")
        );

        o_aard_wave.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                sprite_index = choose(s_earthquake_01, s_earthquake_02, s_earthquake_03, s_earthquake_04)
                scr_damage_init()
                scr_light_off()
                is_execute = false
                cast_frame = 6
                speed = 0
                image_speed = 1
                image_xscale = 0
                destroy_on_cast_frame = false

                with (o_player)
                    turn_available = false
            "),

            new MslEvent(eventType: EventType.Destroy, subtype: 0, code: @"
                if (is_player(owner))
                    scr_allturn()

                with (o_player)
                    turn_available = true
            "),

            new MslEvent(eventType: EventType.Alarm, subtype: 0, code: ""),
            new MslEvent(eventType: EventType.Alarm, subtype: 10, code: ""),

            new MslEvent(eventType: EventType.Step, subtype: 0, code: @"
                image_angle = direction
                image_xscale = lerp(image_xscale, 1, 0.2)

                if (!is_execute)
                {
                    if (image_index >= (cast_frame + 0.5))
                    {
                        is_execute = true
                        
                        with (o_skill_aoe_zone)
                        {
                            if (main_owner == other.owner)
                                event_perform(ev_alarm, 1)
                        }
                    }
                }
            "),

            new MslEvent(eventType: EventType.Other, subtype: 7, code: "instance_destroy()"),

            // Combat calculation
            new MslEvent(eventType: EventType.Other, subtype: 10, code: @$"
                scr_damage_init()
                if (instance_exists(owner) && instance_exists(target))
                {{
                    scr_animationCreate(x, y, choose(s_earthquake_01, s_earthquake_02, s_earthquake_03, s_earthquake_04), -y + 18)
                    
                    if (target.object_index != o_skill_aoe_zone)
                    {{
                        Blunt_Damage = {Blunt_Damage}

                        if (object_is_ancestor(target.object_index, o_enemy))
                        {{
                            if (scr_instance_exists_in_list(o_db_confuse, target.buffs)
                                    || scr_instance_exists_in_list(o_db_daze, target.buffs))
                            {{
                                Psionic_Damage = {Psionic_Damage}
                            }}

                            event_inherited()

                            if (scr_chance_value(({Knockback_Chance}) - target.Knockback_Resistance))
                                scr_cast_knockback(owner, target)

                            var _stagger_chance = {Stagger_Chance}
                            var _daze_chance = {Daze_Chance}
                            with (target)
                            {{
                                diss += 100

                                if (scr_chance_value(_stagger_chance - Knockback_Resistance))
                                {{
                                    var _stagger = scr_instance_exists_in_list(o_db_stagger)
                                    if (!_stagger)
                                    {{
                                        scr_effect_create(o_db_stagger, 3, id, other.owner)
                                    }}
                                    else
                                    {{
                                        with (_stagger)
                                            duration++
                                    }}
                                }}

                                if (scr_chance_value(_daze_chance - Stun_Resistance))
                                {{
                                    scr_skill_category_change_KD(o_skill_category, 2)
                                    var _daze = scr_instance_exists_in_list(o_db_daze)
                                    if (!_daze)
                                    {{
                                        scr_effect_create(o_db_daze, 2, id, other.owner)
                                    }}
                                    else
                                    {{
                                        with (_daze)
                                            duration++
                                    }}
                                }}
                            }}
                        }}

                        if (is_hit)
                        {{
                            scr_temp_effect_update(object_index, target, ""Stun_Resistance"", {Stun_Resistance}, 4, 2)
                            scr_temp_effect_update(object_index, target, ""Knockback_Resistance"", {Stun_Resistance}, 4, 2)
                            scr_temp_effect_update(object_index, target, ""Psionic_Resistance"", {Psionic_Resistance}, 4, 2)

                            with (instance_create_depth(target.x, target.y, 0, o_spellimpact))
                                col = c_white
                        }}
                    }}
                }}
            "),

            new MslEvent(eventType: EventType.Other, subtype: 25, code: @"
                with (owner)
                {
                    Hit_Chance += 100
                    FMB -= 100
                }
            "),

            new MslEvent(eventType: EventType.Draw, subtype: 0, code: "draw_self()"),

            new MslEvent(eventType: EventType.Collision, subtype: (uint) DataLoader.data.GameObjects.IndexOf(DataLoader.data.GameObjects.First(x => x.Name.Content == "o_unit")), code: ""),
            new MslEvent(eventType: EventType.Collision, subtype: (uint) DataLoader.data.GameObjects.IndexOf(DataLoader.data.GameObjects.First(x => x.Name.Content == "o_broken_stuff")), code: ""),
            new MslEvent(eventType: EventType.Collision, subtype: (uint) DataLoader.data.GameObjects.IndexOf(DataLoader.data.GameObjects.First(x => x.Name.Content == "o_abstract_stuff")), code: "")
        );

    }
}