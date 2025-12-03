using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddSkill_Igni_Sign()
    {
        AdjustSkillIcon("s_skills_igni_sign");
        AdjustSpellCastSprites("s_ignisign_cast_", 15, 62);

        Msl.InjectTableSkillsLocalization(
            new LocalizationSkill(
                id: "Igni_Sign",
                name: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Igni Sign"},
                    {ModLanguage.Chinese, "伊格尼法印"}
                },
                description: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, @"No translation"},
                    {ModLanguage.Chinese, string.Join("##",
                        "对方圆~w~2~/~格之内一片区域造成~o~/*Fire_Damage*/点灼烧伤害~/~，~o~点燃~/~没有物体和生灵的方格。",
                        "有~w~/*Ignition_Chance*/%~/~的几率~o~点燃~/~所有受到影响的目标，燃烧持续~w~2-3~/~个回合；令他们~w~10~/~个回合之内灼烧抗性~r~/*Fire_Resistance*/%~/~，这个效果可以叠加，最多~w~3~/~层。",
                        "对~o~着火~/~目标的灼烧伤害~r~+30%~/~，并使其护甲耐久~r~减半~/~。每有一个目标未被~o~点燃~/~，伊格尼法印当前剩余冷却时间便缩短~lg~3~/~个回合。"
                    )}
                }
            )
        );

        Msl.InjectTableSpeechesLocalization(
            new LocalizationSpeech(
                id: "Igni_Sign",
                new Dictionary<ModLanguage, string> {
                    {ModLanguage.English, "IGNI!"},
                    {ModLanguage.Chinese, "伊格尼！"}
                }
            )
        );

        Msl.InjectTableSpeechesLocalization(
            new LocalizationSpeech(
                id: "MC_Igni_Sign",
                new Dictionary<ModLanguage, string> {
                    {ModLanguage.English, "I...IGNE..."},
                    {ModLanguage.Chinese, "伊...哥尼..."}
                }
            )
        );

        Msl.InjectTableSkillsStats(
            id: "Igni_Sign",
            Object: "o_igni_sign_birth",
            hook: Msl.SkillsStatsHook.MAGICMASTERY,
            Target: Msl.SkillsStatsTarget.TargetArea,
            Range: "2",
            AOE_Lenght: 2,
            AOE_Width: 3,
            Pattern: Msl.SkillsStatsPattern.pyramid,
            KD: 12,
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

        UndertaleGameObject o_skill_igni_sign = Msl.AddObject(
            name: "o_skill_igni_sign",
            parentName: "o_skill",
            spriteName: "s_skills_igni_sign",
            isVisible: true,
            isPersistent: false,
            isAwake: true,
            collisionShapeFlags: CollisionShapeFlags.Circle
        );

        UndertaleGameObject o_skill_igni_sign_ico = Msl.AddObject(
            name: "o_skill_igni_sign_ico",
            parentName: "o_skill_ico",
            spriteName: "s_skills_igni_sign",
            isVisible: true,
            isPersistent: false,
            isAwake: true,
            collisionShapeFlags: CollisionShapeFlags.Circle
        );

        UndertaleGameObject o_igni_sign_birth = Msl.AddObject(
            name: "o_igni_sign_birth",
            spriteName: "s_ignisign_cast_cast",
            parentName: "o_spellbirth",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        UndertaleGameObject o_igni_wave = Msl.AddObject(
            name: "o_igni_wave",
            parentName: "o_aoe_spell",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        string Fire_Damage = "13 * ((owner.Magic_Power + owner.Pyromantic_Power) / 100)";
        string Fire_Resistance = "math_round((-10 * (owner.Magic_Power + owner.Pyromantic_Power)) / 100)";
        string Ignition_Chance = "5 * owner.WIL * (owner.Magic_Power + owner.Pyromantic_Power) / 100";

        o_skill_igni_sign.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                skill = ""Igni_Sign""
                startcast_sprite_tag = ""s_ignisign_cast_""
                scr_skill_atr(""Igni_Sign"")
                ds_list_add(attribute,
                    ds_map_find_value(global.attribute, ""Magic_Power""),
                    ds_map_find_value(global.attribute, ""Pyromantic_Power""),
                    ds_map_find_value(global.attribute, ""WIL""))
                ignore_interact = true
                is_moving = false
                main_spell = o_igni_wave
                damage_type = ""Fire_Damage""
                click_snd = snd_skill_sealofshackles_startcast
            "),

            new MslEvent(eventType: EventType.Other, subtype: 17, code: @$"
                if (instance_exists(owner))
                {{
                    ds_map_replace(data, ""Fire_Damage"", {Fire_Damage})
                    ds_map_replace(data, ""Fire_Resistance"", {Fire_Resistance})
                    ds_map_replace(data, ""Ignition_Chance"", {Ignition_Chance})
                }}

                event_inherited()
            "),

            new MslEvent(eventType: EventType.Other, subtype: 10, code: ModFiles.GetCode("o_skill_other_10.gml"))
        );

        o_skill_igni_sign_ico.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                child_skill = o_skill_igni_sign
                event_perform_object(child_skill, ev_create, 0)
            ")
        );

        o_igni_sign_birth.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                scr_light_off()
                blend = make_colour_rgb(231, 137, 22)
                cast_frame = 1
                spell = o_igni_wave
                is_flying = true
            "),

            new MslEvent(eventType: EventType.Other, subtype: 10, code: @"
                event_inherited()

                with (o_skill_aoe_zone)
                {
                    if (main_owner == other.owner)
                        alarm[1] = scr_tile_distance(o_player, id) * 3
                }
            "),

            new MslEvent(eventType: EventType.PreCreate, subtype: 0, code: @"
                event_inherited()
                blend = 4279667175
                lumalpha = 0.5
            ")
        );

        o_igni_wave.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                image_speed = 0.5
                speed = 10
                audio_play_sound(snd_spell_ring_of_fire, 3, 0)
            "),

            // Damage Calculation
            new MslEvent(eventType: EventType.Other, subtype: 10, code: @$"
                scr_damage_init()
                if (instance_exists(owner) && instance_exists(target))
                {{
                    Fire_Damage = {Fire_Damage}

                    if (object_is_ancestor(target.object_index, o_unit))
                    {{
                        if (scr_instance_exists_in_list(o_db_fire, target.buffs))
                            Fire_Damage *= 1.3
                    }}

                    event_inherited()

                    if (instance_exists(o_player))
                    {{
                        if (!disable)
                        {{
                            var _x = target.x + random_range(-4, 4)
                            var _y = target.y + random_range(-4, 4)

                             with (instance_create_depth(_x, _y, 0, o_flamewave_spellimpact))
                             {{
                                angle = other.angle
                                owner = other.owner
                                sprite_index = s_firewall
                                image_angle = random_range(-15, 15)

                                if (scr_water_filter(_x, _y, _x, _y))
                                {{
                                    repeat (irandom_range(1, 3))
                                        instance_create_depth((x - 13) + irandom_range(-6, 6), (y - 13) + irandom_range(-6, 6), 0, o_burning_decails);
                                    
                                    repeat (irandom(3))
                                    {{
                                        with (instance_create_depth(x + irandom_range(-3, 3), y, 0, o_firemagick_paricle))
                                        {{
                                            speed = 2 + random(2);
                                            direction = random_range(45, 145);
                                        }}
                                    }}
                                }}
                             }}

                            if (target.object_index != o_skill_aoe_zone && target.object_index != o_attacked_target)
                            {{
                                if (object_is_ancestor(target.object_index, o_unit))
                                {{
                                    scr_temp_effect_update(object_index, target, ""Fire_Resistance"", {Fire_Resistance}, 10, 3)

                                    if (scr_chance_value({Ignition_Chance} - target.Fire_Resistance))
                                    {{
                                        scr_effect_create(o_db_fire, choose(2, 3), target, owner);
                                        with (target)
                                        {{
                                            ArmorDurability /= 2

                                            if (ArmorDurability < 0)
                                                ArmorDurability = 0

                                            scr_def_calc(false)
                                        }}
                                    }}
                                    else
                                    {{
                                        scr_skill_change_KD(o_skill_igni_sign_ico, -3)
                                    }}
                                }}
                            }}
                            else if (scr_water_filter(target.x, target.y, target.x, target.y))
                            {{
                                var dur = (3 * (owner.Magic_Power + owner.Pyromantic_Power)) / 100
                                with (instance_create_depth(target.x, target.y, 0, o_p_fire))
                                {{
                                    owner = other.id
                                    duration = dur
                                }}
                            }}
                        }}
                    }}
                }}
            "),

            new MslEvent(eventType: EventType.Draw, subtype: 0, code: "")
        );
    }
}