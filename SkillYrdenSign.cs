using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddSkill_Yrden_Sign()
    {
        AdjustSkillIcon("s_skills_yrden_sign");
        AdjustSpellCastSprites("s_yrdensign_cast_", 15, 62);

        Msl.InjectTableSkillsLocalization(
            new LocalizationSkill(
                id: "Yrden_Sign",
                name: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Yrden Sign"},
                    {ModLanguage.Chinese, "亚登法印"}
                },
                description: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, @""},
                    {ModLanguage.Chinese, string.Join("##",
                        "依据指定的位置，在地面放置~w~3~/~个持续~w~9~/~回合的~lg~亚登法印~/~：",
                        "法印被触发前处于隐蔽状态，触发后每回合对敌人造成~p~/*Arcane_Damage*/点秘术伤害~/~。",
                        "造成伤害后有~lg~/*Bleed_Chance*/%~/~几率使敌人腿部出血，有~lg~/*Immob_Chance*/%~/~几率使敌人移动受限~w~2~/~回合（受敌人的击退抗性影响）。",
                        "每与一个~lg~亚登法印~/~相邻，当前法印的出血几率~lg~+/*Bleed_Chance*/%~/~，移动受限几率~lg~+/*Immob_Chance*/%~/~。",
                        "站在法印上的普通敌人所受伤害~r~+9%~/~，幽魂类敌人所受伤害~r~+33%~/~。",
                        "施法者站在~lg~亚登法印~/~上时，失手~lg~-9%~/~，精力恢复~lg~+9%~/~，暴击几率~lg~+3%~/~。"
                    )}
                }
            )
        );

        Msl.InjectTableSpeechesLocalization(
            new LocalizationSpeech(
                id: "Yrden_Sign",
                new Dictionary<ModLanguage, string> {
                    {ModLanguage.English, "YRDEN!"},
                    {ModLanguage.Chinese, "亚登！"}
                }
            )
        );

        Msl.InjectTableSpeechesLocalization(
            new LocalizationSpeech(
                id: "MC_Yrden_Sign",
                new Dictionary<ModLanguage, string> {
                    {ModLanguage.English, "YR...DEN..."},
                    {ModLanguage.Chinese, "亚...等..."}
                }
            )
        );

        string Immob_Chance = "(25 * (owner.Magic_Power + owner.Arcanistic_Power)) / 100";
        string Arcane_Damage = "(2 * (owner.Magic_Power + owner.Arcanistic_Power)) / 100";
        string Bleed_Chance = "(15 * (owner.Magic_Power + owner.Arcanistic_Power)) / 100";

        Msl.InjectTableSkillsStats(
            id: "Yrden_Sign",
            Object: "o_yrden_sign_birth",
            hook: Msl.SkillsStatsHook.MAGICMASTERY,
            Target: Msl.SkillsStatsTarget.TargetPoint,
            Range: "5",
            KD: 24,
            MP: 20,
            Duration: 0,
            Class: Msl.SkillsStatsClass.spell,
            Branch: "magic_mastery",
            Spell: true,
            AP: "x",
            Bonus_Range: true,
            Crime: true
        );

        UndertaleGameObject o_skill_yrden_sign = Msl.AddObject(
            name: "o_skill_yrden_sign",
            parentName: "o_skill",
            spriteName: "s_skills_yrden_sign",
            isVisible: true,
            isPersistent: false,
            isAwake: true,
            collisionShapeFlags: CollisionShapeFlags.Circle
        );

        UndertaleGameObject o_yrden_sign_birth = Msl.AddObject(
            name: "o_yrden_sign_birth",
            spriteName: "s_yrdensign_cast_cast",
            parentName: "o_spellbirth",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        o_skill_yrden_sign.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                skill = ""Yrden_Sign""
                startcast_sprite_tag = ""s_yrdensign_cast_""
                scr_skill_atr(""Yrden_Sign"")
                ds_list_add(attribute,
                    ds_map_find_value(global.attribute, ""Magic_Power""),
                    ds_map_find_value(global.attribute, ""Arcanistic_Power""),
                    ds_map_find_value(global.attribute, ""Bonus_Range""))
                ignore_interact = true
                is_moving = false
                self_cast = true
                click_snd = snd_skill_sealofshackles_startcast
                use_count = 3
                max_use = 3
                target_array = []
                mark_array = []
            "),

            new MslEvent(eventType: EventType.Other, subtype: 17, code: @$"
                if (instance_exists(owner))
                {{
                    ds_map_replace(data, ""Arcane_Damage"", {Arcane_Damage})
                    ds_map_replace(data, ""Immob_Chance"", {Immob_Chance})
                    ds_map_replace(data, ""Bleed_Chance"", {Bleed_Chance})
                }}

                event_inherited()
            "),

            new MslEvent(eventType: EventType.Other, subtype: 13, code: @"
                event_inherited()
                use_count = max_use

                for (var i = 0; i < array_length(mark_array); i++)
                    scr_onUnitEffectDestroy(mark_array[i])

                with (o_yrden_sign_birth)
                    target_array = other.target_array

                target_array = []
                mark_array = []
            "),

            new MslEvent(eventType: EventType.Other, subtype: 25, code: @"
                event_inherited()
                use_count = max_use

                for (var i = 0; i < array_length(mark_array); i++)
                    scr_onUnitEffectDestroy(mark_array[i])

                mark_array = []
                target_array = []

                with (o_attacked_target)
                    instance_destroy()
            "),

            // Select multiple target points
            new MslEvent(eventType: EventType.Other, subtype: 19, code: @"
                event_inherited()

                with (o_player)
                    scr_setside(o_floor_target)

                var _cur = s_cursor_skill

                if (use_count == 1)
                {
                    with (o_skill_tile_indicator)
                    {
                        if (image_alpha != 1)
                        {
                            _cur = s_cursor
                            
                            with (o_controller)
                                event_user(2)
                        }
                    }
                    
                    if (_cur != s_cursor)
                    {
                        with (main_target)
                            draw_line_width_color(x, y - 8 - image_index, mouse_x, mouse_y, 3, c_white, c_white)
                    }
                }
                else if (use_count > 1)
                {
                    _cur = scr_skillUITargetObject()
                }

                with (o_floor_target)
                    cursor = _cur
            "),

            // Mark selected target points
            new MslEvent(eventType: EventType.Other, subtype: 20, code: @"
                event_inherited()
                var _can_interract = scr_skill_can_interact()

                if (_can_interract)
                {
                    target = scr_attack_tile(o_player)
                    
                    with (target)
                    {
                        if (object_index == o_attacked_target)
                            alarm[0] = -1
                    }
                    
                    array_push(target_array, [target.x, target.y])
                    var _id = id
                    
                    if (instance_exists(target) && (!object_is_ancestor(target.object_index, o_enemy) || !target.Unbreakable))
                    {
                        with (target)
                        {
                            if (object_is_ancestor(object_index, o_unit) || object_index == o_attacked_target)
                            {
                                with (scr_onUnitEffectCreate(id, o_onUnitEffectGroundmarker, -152, 0, 0, true))
                                    array_push(_id.mark_array, id)
                            }
                        }
                        
                        use_count--

                        if (use_count == 0)
                            scr_skill_prepare_to_use(_can_interract);
                    }
                }
                else
                {
                    scr_skill_denied()
                }
            ")
        );

        UndertaleGameObject o_skill_yrden_sign_ico = Msl.AddObject(
            name: "o_skill_yrden_sign_ico",
            parentName: "o_skill_ico",
            spriteName: "s_skills_yrden_sign",
            isVisible: true, 
            isPersistent: false, 
            isAwake: true,
            collisionShapeFlags: CollisionShapeFlags.Circle
        );

        o_skill_yrden_sign_ico.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                child_skill = o_skill_yrden_sign
                event_perform_object(child_skill, ev_create, 0)
            ")
        );

        UndertaleGameObject o_yrden_sign = Msl.AddObject(
            name: "o_yrden_sign",
            spriteName: "s_sign_of_yrden",
            parentName: "o_mark_spell_damage",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        o_yrden_sign.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                alpha = 0
                scr_set_lt()
                blend = make_color_rgb(0, 0, 0)
                duration = 9
                loop_start = 6
                loop_end = 6
                amimation_destroy = true
                animation = scr_animationCreate(x, y, s_sigilofdarkness_groundmarkparts, -y - 10, false, false, true)
                tile_grid_weight = 2
                hidden = true
                is_created = false
            "),

            new MslEvent(eventType: EventType.Destroy, subtype: 0, code: @"
                event_inherited()

                with (animation)
                    is_loop = false
            "),

            new MslEvent(eventType: EventType.Draw, subtype: 0, code: @"
                if (!hidden)
                    event_inherited()

                if (!is_created)
                {
                    is_created = true
                    scr_animationCreate(x, y, s_sign_of_yrden, depth, false, true, false)
                }
            "),

            // Triggered every turn
            new MslEvent(eventType: EventType.Other, subtype: 10, code: @"
                if (!instance_exists(owner))
                    duration = 0;

                event_inherited()
            "),

            new MslEvent(eventType: EventType.Other, subtype: 11, code: @$"
                hidden = false
                tile_grid_weight = 14
                tile_grid_marked = false

                Arcane_Damage = 0
                Bleeding_Chance = 0

                var _damage = {Arcane_Damage}

                if (target.id == owner.id)
                {{
                    with (target)
                    {{
                        scr_temp_incr_atr(""FMB"", -9, 1, id, id)
                        scr_temp_incr_atr(""MP_Restoration"", 9, 1, id, id)
                        scr_temp_incr_atr(""CRT"", 3, 1, id, id)
                    }}

                    damage_done = 0
                }}
                else
                {{
                    Arcane_Damage = _damage

                    with (target)
                    {{
                        if (typeID == ""spectre"")
                            scr_temp_incr_atr(""Damage_Received"", 33, 1, id, id)
                        else
                            scr_temp_incr_atr(""Damage_Received"", 9, 1, id, id)

                        scr_guiAnimation_ext(x, y, s_signofyrden_impact, 1, 1, 0, 0xFFFFFF, 0)
                        scr_audio_play_at(snd_vampire_rune_impact)
                    }}

                    event_inherited()
                }}

                if (damage_done > 0)
                {{
                    var _neighbor_count = 0
                    with (o_yrden_sign)
                    {{
                        if (id != other.id && owner.id == other.owner.id
                            && scr_tile_distance(id, other.id) <= 1)
                            _neighbor_count++  
                    }}

                    var _tmp_immob_chance = ({Immob_Chance}) * (1 + _neighbor_count)
                    if (instance_exists(target) && instance_exists(owner)
                            && scr_chance_value(_tmp_immob_chance - target.Knockback_Resistance))
                        scr_effect_create(o_db_immob, 2, target, owner)

                    var _body_part = ""legs""
                    var _resist = target.Bleeding_Resistance
                    if is_player(target)
                        _resist = target.Bleeding_Resistance_Legs

                    var _tmp_bleed_chance = ({Bleed_Chance}) * (1 + _neighbor_count)
                    if scr_chance_value(_tmp_bleed_chance - _resist)
                        scr_bleedChange(target, _body_part, irandom_range(1, 3), target.Bleeding_Resistance, id)

                }}
            ")
        );

        Msl.LoadGML("gml_GlobalScript_scr_unitTurnGetDamageFromTileMark")
            .MatchFrom("with (c_tile_mark)")
            .InsertAbove(@"
                if (object_is_ancestor(object_index, o_Specter))
                {
                    with (o_yrden_sign)
                    {
                        if (grid_x == other.grid_x && grid_y == other.grid_y)
                            event_user(7)
                    }
                }
            ")
            .Save();

        o_yrden_sign_birth.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                image_speed = 1
                scr_set_lt()
                cast_frame = 7
                is_flying = false
                spell = o_yrden_sign
                scr_audio_play_at(snd_vampire_rune_spell)
                target_array = []
            "),

            new MslEvent(eventType: EventType.Destroy, subtype: 0, code: @"
                event_inherited()

                if (is_player(owner))
                    scr_allturn()
            "),

            // Create Yrden signs on selected target points
            new MslEvent(eventType: EventType.Other, subtype: 10, code: @"
                if (!is_execute)
                {
                    if (instance_exists(o_player) && instance_exists(owner))
                    {
                        is_execute = true
                        var _tarlen = array_length(target_array)
                        for (var i = 0; i < _tarlen; i++)
                        {
                            var _point = target_array[i]
                            if (is_array(_point) && array_length(_point) == 2)
                            {
                                var xx = _point[0]
                                var yy = _point[1]
                                
                                // Prevent overlapping Yrden signs
                                if (scr_tile_mark_get_instance(xx div 26, yy div 26))
                                {
                                    var _array = scr_free_tile_array(xx, yy, 1, true)
                                    if (is_array(_array) && array_length(_array) > 0)
                                    {
                                        xx += _array[0]
                                        yy += _array[1]
                                    }
                                }

                                with (instance_create_depth(xx, yy, 0, spell))
                                {
                                    name = other.name
                                    owner = other.owner
                                    is_crit = other.is_crit

                                    damage = other.damage
                                    direction = point_direction(x, y, _point[0], _point[1])
                                    duration = scr_skill_get_duration(9, owner) + ((is_crit * owner.Miracle_Power) / 100)
                                }
                            }
                        }
                    }
                }
            ")
        );


        UndertaleSprite img = Msl.GetSprite("s_sign_of_yrden");
        img.CollisionMasks.RemoveAt(0);
        img.IsSpecialType = true;
        img.SVersion = 3;
        img.GMS2PlaybackSpeed = 0.3f;
        img.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;
        img.OriginX = 14;
        img.OriginY = 11;

        img = Msl.GetSprite("s_signofyrden_cast");
        img.CollisionMasks.RemoveAt(0);
        img.IsSpecialType = true;
        img.SVersion = 3;
        img.GMS2PlaybackSpeed = 0.3f;
        img.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;
        img.OriginX = 12;
        img.OriginY = 60;

        img = Msl.GetSprite("s_signofyrden_impact");
        img.CollisionMasks.RemoveAt(0);
        img.IsSpecialType = true;
        img.SVersion = 3;
        img.GMS2PlaybackSpeed = 0.3f;
        img.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;
        img.OriginX = 14;
        img.OriginY = 40;

    }
}