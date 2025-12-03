using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddSkill_Axii_Sign()
    {
        AdjustSkillIcon("s_skills_axii_sign");
        AdjustSpellCastSprites("s_axiisign_cast_", 16, 62);

        Msl.InjectTableSkillsLocalization(
            new LocalizationSkill(
                id: "Axii_Sign",
                name: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Axii Sign"},
                    {ModLanguage.Chinese, "亚克西法印"}
                },
                description: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, @"No translation"},
                    {ModLanguage.Chinese, string.Join("##",
                        "有~lg~/*Charm_Chance*/%~/~的概率~lg~催眠~/~敌人~w~/*Charm_Time*/~/~回合（受~r~灵能抗性~/~影响），被催眠的敌人会优先攻击其他敌对单位。",
                        "如果目标~lg~没有察觉~/~或处于~r~眩晕~/~状态，那么必然催眠成功。如果目标处于~r~慌乱~/~状态，催眠成功率~lg~+20%~/~。",
                        "催眠失败或者敌人从催眠中醒来后会陷入~w~12~/~回合的~r~“慌乱”~/~。若催眠失败，则令该技能冷却时间~lg~减半~/~，同时令敌人所有技能的冷却时间~lg~+3~/~。",
                        "在与某些居民对话时，可以~lg~催眠~/~对方以获得便利，而代价是阵营~r~声望下降~/~。"
                    )}
                }
            )
        );

        Msl.InjectTableSpeechesLocalization(
            new LocalizationSpeech(
                id: "Axii_Sign",
                new Dictionary<ModLanguage, string> {
                    {ModLanguage.English, "AXII!"},
                    {ModLanguage.Chinese, "亚克西！"}
                }
            )
        );

        Msl.InjectTableSpeechesLocalization(
            new LocalizationSpeech(
                id: "MC_Axii_Sign",
                new Dictionary<ModLanguage, string> {
                    {ModLanguage.English, "A...KI..."},
                    {ModLanguage.Chinese, "雅克...西..."}
                }
            )
        );

        Msl.InjectTableSkillsStats(
            id: "Axii_Sign",
            Object: "o_axii_charm_birth",
            hook: Msl.SkillsStatsHook.MAGICMASTERY,
            Target: Msl.SkillsStatsTarget.TargetObject,
            Range: "5",
            KD: 26,
            MP: 48,
            Duration: 12,
            Class: Msl.SkillsStatsClass.spell,
            Branch: "magic_mastery",
            Spell: true,
            AP: "x",
            Bonus_Range: true,
            Crime: true
        );

        UndertaleGameObject o_skill_axii_sign = Msl.AddObject(
            name: "o_skill_axii_sign",
            parentName: "o_skill",
            spriteName: "s_skills_axii_sign",
            isVisible: true,
            isPersistent: false,
            isAwake: true,
            collisionShapeFlags: CollisionShapeFlags.Circle
        );

        string Charm_Chance = "5 * owner.WIL * (100 + owner.Psimantic_Power) / 100";
        string Charm_Time = "round(4 * (owner.Magic_Power + owner.Psimantic_Power) / 100)";

        o_skill_axii_sign.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                skill = ""Axii_Sign""
                startcast_sprite_tag = ""s_axiisign_cast_""
                scr_skill_atr(""Axii_Sign"")
                ds_list_add(attribute,
                    ds_map_find_value(global.attribute, ""Magic_Power""),
                    ds_map_find_value(global.attribute, ""Psimantic_Power""),
                    ds_map_find_value(global.attribute, ""WIL""),
                    ds_map_find_value(global.attribute, ""Bonus_Range""))
                ignore_interact = true
                is_moving = false
                click_snd = snd_skill_sealofshackles_startcast
            "),

            // Format the description text
            new MslEvent(eventType: EventType.Other, subtype: 17, code: @$"
                if instance_exists(owner)
                {{
                    ds_map_replace(data, ""Charm_Chance"", {Charm_Chance})
                    ds_map_replace(data, ""Charm_Time"", {Charm_Time})
                }}
                event_inherited()
            ")
        );

        UndertaleGameObject o_skill_axii_sign_ico = Msl.AddObject(
            name: "o_skill_axii_sign_ico",
            parentName: "o_skill_ico",
            spriteName: "s_skills_axii_sign",
            isVisible: true, 
            isPersistent: false, 
            isAwake: true,
            collisionShapeFlags: CollisionShapeFlags.Circle
        );

        o_skill_axii_sign_ico.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                child_skill = o_skill_axii_sign
                event_perform_object(child_skill, ev_create, 0)
            ")
        );

        Msl.InjectTableModifiersLocalization(
            new LocalizationModifier(
                id: "o_db_axii_charm",
                name: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Charm"},
                    {ModLanguage.Chinese, "魅惑"}
                },
                description: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "charmed by Axii Sign."},
                    {ModLanguage.Chinese, "被亚克西法印所魅惑。"}
                }
            )
        );

        UndertaleGameObject o_axii_charm_birth = Msl.AddObject(
            name: "o_axii_charm_birth",
            parentName: "o_spellbirth",
            spriteName: "s_axiisign_cast_cast",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        UndertaleGameObject o_db_axii_charm = Msl.AddObject(
            name: "o_db_axii_charm",
            parentName: "o_magical_buff",
            spriteName: "s_b_receptivity",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        o_axii_charm_birth.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                scr_audio_play_at(snd_skill_sealofinsight_startcast)
            "),

            // Control the chance of charm
            new MslEvent(eventType: EventType.Other, subtype: 10, code: @$"
                if (instance_exists(point) && variable_instance_exists(point, ""ai_is_on"") && point.ai_is_on)
                {{
                    var target = point
                    var _charm_chance = {Charm_Chance} - target.Psionic_Resistance

                    if (is_crit)
                        _charm_chance *= max(1, owner.Miracle_Power / 100)

                    if (scr_instance_exists_in_list(o_db_confuse, target.buffs))
                        _charm_chance += 20

                    var _charm_proc = scr_chance_value(_charm_chance)
                    if (target.state == ""idle"" || target.state == ""search"" || target.state == ""alarm""
                            || scr_instance_exists_in_list(o_db_daze, target.buffs))
                        _charm_proc = true

                    var _charm_time = scr_skill_get_duration({Charm_Time}, owner)
                    if (is_crit)
                        _charm_time = round(_charm_time * max(1, owner.Miracle_Power / 100))

                    if (_charm_proc)
                        scr_effect_create(o_db_axii_charm, {Charm_Time}, target, owner)
                    else
                    {{
                        scr_effect_create(o_db_confuse, is_crit ? 12 * max(1, owner.Miracle_Power / 100) : 12, target, target)
                        with (target)
                        {{
                            if (is_player())
                            {{
                                scr_skill_category_change_KD(o_skill_category, -3)
                            }}
                            else
                            {{
                                scr_skill_category_change_KD_enemy(id, 3)
                            }}
                        }}
                        with (owner)
                        {{
                            if (is_player())
                            {{
                                with (o_skill_axii_sign)
                                {{
                                    var _kd = scr_get_value_Dmap(skill, ""KD"")
                                    scr_set_kd(skill, ""KD"", floor(_kd / 2))
                                }}

                                with (o_skill_axii_sign_ico)
                                {{
                                    var _kd = scr_get_value_Dmap(skill, ""KD"")
                                    scr_set_kd(skill, ""KD"", floor(_kd / 2))
                                }}
                            }}
                            else
                            {{
                                var _kd = scr_get_value_Dmap(""Axii_Sign"", ""KD"")
                                scr_set_kd(""Axii_Sign"", floor(_kd / 2))
                            }}
                        }}
                    }}
                }}

                instance_destroy()
            ")
        );

        Msl.AddFunction(ModFiles.GetCode("scr_charm_npc_to_ally.gml"), "scr_charm_npc_to_ally");
        Msl.AddFunction(ModFiles.GetCode("scr_charm_enemy_to_ally.gml"), "scr_charm_enemy_to_ally");

        UndertaleGameObject o_onUnitEffect_axiicharm = Msl.AddObject(
            name: "o_onUnitEffect_axiicharm",
            parentName: "o_onUnitEffectSprite",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        o_onUnitEffect_axiicharm.ApplyEvent(
            new MslEvent(eventType: EventType.Other, subtype: 25, code: @"
                event_inherited()
                spriteIndexStart = s_axiicharm_start
                spriteIndexLoop = s_axiicharm_loop
                spriteIndexEnd = s_axiicharm_end
            ")
        );

        o_db_axii_charm.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                scr_buff_atr()
                buff_snd = snd_skill_sigil_of_binding_startcast
                stack = 0
                stage = 1
                signTarget = noone
                sign_loop = noone
                target_fraction = """"
                target_subfraction = """"
            "),

            new MslEvent(eventType: EventType.Destroy, subtype: 0, code: @"
                event_inherited()
                audio_stop_sound(sign_loop)
                signTarget = scr_onUnitEffectDestroy(signTarget)

                with (target)
                {
                    faction_key = other.target_fraction
                    subfaction_key = other.target_subfraction
                    scr_delete_from_enemy_list(other.target_subfraction)
                    target = noone
                    if (state != ""work"" && state != ""KO"" && state != ""transition"")
                        state = ""move_to_work""
                    scr_add_to_enemy_list(""Player"")
                    state = ""alarm""
                    scr_enemy_choose_state_new()
                    scr_random_speech(hostile_voice_tag, noone, id)
                }

                scr_effect_create(o_db_confuse, 12, target, target)
            "),

            // Charm the enemy
            new MslEvent(eventType: EventType.Alarm, subtype: 2, code: @"
                event_inherited()
                var _id = id
                with (target)
                {
                    other.signTarget = scr_onUnitEffectCreate(
                        id, o_onUnitEffect_axiicharm, -200, 0, 0, true)
                    with (other.signTarget)
                        _id.sign_loop = scr_audio_play_at_loop(snd_skill_sigil_of_binding_loop)

                    scr_charm_enemy_to_ally()
                }
            "),

            // Attack the charmed enemy will destroy this buff
            new MslEvent(eventType: EventType.Other, subtype: 13, code: @"
                if (attacker.id == owner.id)
                {
                    instance_destroy()
                }
            ")
        );

        // Adjust animation
        UndertaleSprite img = Msl.GetSprite("s_axiicharm_start");
        img.CollisionMasks.RemoveAt(0);
        img.OriginX = 25;
        img.OriginY = 56;
        img.IsSpecialType = true;
        img.SVersion = 3;
        img.GMS2PlaybackSpeed = 0.3f;
        img.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        img = Msl.GetSprite("s_axiicharm_loop");
        img.CollisionMasks.RemoveAt(0);
        img.OriginX = 25;
        img.OriginY = 56;
        img.MarginLeft = 5;
        img.MarginRight = 42;
        img.MarginBottom = 49;
        img.MarginTop = 26;
        img.IsSpecialType = true;
        img.SVersion = 3;
        img.GMS2PlaybackSpeed = 0.3f;
        img.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        img = Msl.GetSprite("s_axiicharm_end");
        img.CollisionMasks.RemoveAt(0);
        img.OriginX = 25;
        img.OriginY = 56;
        img.MarginLeft = 5;
        img.MarginRight = 42;
        img.MarginBottom = 51;
        img.MarginTop = 27;
        img.IsSpecialType = true;
        img.SVersion = 3;
        img.GMS2PlaybackSpeed = 0.3f;
        img.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        // Use Axii Sign in dialogues
        UndertaleGameObject ob = Msl.AddObject("o_axii_dialog_initializer", isPersistent: true);
        Msl.AddNewEvent(ob, "", EventType.Other, 10);
        UndertaleRoom room = Msl.GetRoom("START");
        room.AddGameObject("Instances", ob);

        Msl.AddFunction(ModFiles.GetCode("scr_mod_apply_axii_charm_in_dialog.gml"), "scr_mod_apply_axii_charm_in_dialog");
        Msl.AddFunction(ModFiles.GetCode("scr_mod_is_charmed_within.gml"), "scr_mod_is_charmed_within");
        Msl.LoadGML(Msl.EventName("o_axii_dialog_initializer", EventType.Other, 10))
            .MatchAll()
            .InsertBelow(ModFiles, "o_axii_dialog_initializer.gml")
            .Save();
        Msl.LoadGML("gml_Object_o_dataLoader_Other_10")
            .MatchFrom("scr_dialogue_loader_init")
            .InsertBelow("with (o_axii_dialog_initializer) { event_user(0) }")
            .Save();

        Msl.InjectTableDialogLocalization(
            new LocalizationSentence(
                id: "npc_bandit_fence_axii_charm",
                sentence: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "You don’t recognize me? It’s Ander! I even bought you a drink just before the shift started! ~r~[Hypnotize]~/~"},
                    {ModLanguage.Chinese, "你认不出我了？我是安德尔啊！刚刚上工前还请了你杯酒呢！~r~[催眠]~/~"}
                }
            ),
            new LocalizationSentence(
                id: "npc_bandit_fence_axii_was_charmed",
                sentence: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Oh… sorry, must’ve been seeing things. Skinflint Homs still waiting on you to finish the inventory..."},
                    {ModLanguage.Chinese, "哦... 抱歉刚眼看花了，铁公鸡还找你清点货物呢..."}
                }
            ),
            new LocalizationSentence(
                id: "npc_bandit_fence_axii_charm_inspection",
                sentence: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Hmph… you’ve got a strange face, kid. Did I already let you in?"},
                    {ModLanguage.Chinese, "斯... 你小子面生啊，我放你进去过了？"}
                }
            ),

            new LocalizationSentence(
                id: "skinflint_homs_dont_know_player",
                sentence: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Hmph… you’ve got a strange face, kid. Did I already let you in?"},
                    {ModLanguage.Chinese, "*戒备*谁放你进来的？报上名来。"}
                }
            ),
            new LocalizationSentence(
                id: "skinflint_homs_charm_pc",
                sentence: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Hmph… you’ve got a strange face, kid. Did I already let you in?"},
                    {ModLanguage.Chinese, "我有个主意，你看我这有袋金币，我把它给你，你卖我商品，很合理吧？你赚钱不吃亏，别问我是谁，如何？~r~[催眠]~/~"}
                }
            ),
            new LocalizationSentence(
                id: "skinflint_homs_was_charmed",
                sentence: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Hmph… you’ve got a strange face, kid. Did I already let you in?"},
                    {ModLanguage.Chinese, "*有节奏的*你给钱...我给货...我们都是好朋友..."}
                }
            ),
            new LocalizationSentence(
                id: "player_thieveryReaction_axii_charm_pc",
                sentence: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Hmph… you’ve got a strange face, kid. Did I already let you in?"},
                    {ModLanguage.Chinese, "你什么都没看见。~r~[催眠]~/~"}
                }
            ),
            new LocalizationSentence(
                id: "player_thieveryReaction_axii_charm",
                sentence: new Dictionary<ModLanguage, string>{
                    {ModLanguage.English, "Hmph… you’ve got a strange face, kid. Did I already let you in?"},
                    {ModLanguage.Chinese, "唉？我刚刚在干嘛来着..."}
                }
            )
        );
    }
}