using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddWeaponOil()
    {
        UndertaleSprite ico = Msl.GetSprite("s_inv_weapon_oil");
        ico.CollisionMasks.RemoveAt(0);
        ico.IsSpecialType = true;
        ico.SVersion = 3;
        // ico.Width = 27;
        // ico.Height = 54;
        // ico.OriginX = 0;
        // ico.OriginY = 0;
        // ico.MarginLeft = 3;
        // ico.MarginRight = 23;
        // ico.MarginBottom = 47;
        // ico.MarginTop = 5;
        ico.GMS2PlaybackSpeed = 1;
        ico.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerGameFrame;

        // foreach (var tte in ico.Textures)
        // {
        //     tte.Texture.TargetX = 3;
        //     tte.Texture.TargetY = 9;
        //     tte.Texture.TargetWidth = 20;
        //     tte.Texture.TargetHeight = 37;
        //     tte.Texture.BoundingWidth = 27;
        //     tte.Texture.BoundingHeight = 54;
        // }

        ico = Msl.GetSprite("s_loot_weapon_oil");
        ico.CollisionMasks.RemoveAt(0);
        ico.IsSpecialType = true;
        ico.SVersion = 3;
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

        UndertaleGameObject o_inv_weapon_oil_water = Msl.GetObject("o_inv_weapon_oil_water");
        UndertaleGameObject o_loot_weapon_oil_water = Msl.GetObject("o_loot_weapon_oil_water");
        UndertaleGameObject o_inv_weapon_oil_empty = Msl.GetObject("o_inv_weapon_oil_empty");
        UndertaleGameObject o_loot_weapon_oil_empty = Msl.GetObject("o_loot_weapon_oil_empty");

        o_inv_weapon_oil_empty.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                drop_gui_sound = snd_med_drop
                pickup_sound = snd_med_pick
                is_execute = false
            ")
        );

        o_inv_weapon_oil_water.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                charge = 3
                drop_gui_sound = snd_beverage_drop
                pickup_sound = snd_beverage_pick
                dishes_object = o_inv_weapon_oil_empty
                sec_charge = charge
                max_charge = charge
            ")
        );

        o_loot_weapon_oil_water.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                inv_object = o_inv_weapon_oil_water
            ")
        );

        UndertaleGameObject o_inv_weapon_oil_parent = Msl.GetObject("o_inv_weapon_oil_parent");

        o_inv_weapon_oil_parent.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                drop_gui_sound = snd_beverage_drop
                pickup_sound = snd_beverage_pick
                charge = 3
                sec_charge = charge
                max_charge = charge
                bar_color = make_color_rgb(88, 175, 19)
                ds_map_set(data, ""quality"", (2 << 0))
                ds_map_set(data, ""Colour"", make_colour_rgb(89, 219, 76))
                can_merge = true
                skill = o_skill_weapon_coating
                dishes_object = o_inv_weapon_oil_empty
            "),

            new MslEvent(eventType: EventType.Destroy, subtype: 0, code: @"
                if (charge == 0)
                    scr_inventory_change_item(dishes_object)
                
                event_inherited()
            "),

            new MslEvent(eventType: EventType.Other, subtype: 10, code: "event_inherited()"),

            new MslEvent(eventType: EventType.Other, subtype: 24, code: @"
                event_inherited()
                var _name = ds_map_find_value(data, ""idName"")
                with (skill)
                    coating_oil = _name
            "),
            new MslEvent(eventType: EventType.Other, subtype: 25, code: @"
                event_inherited()
                with (loot_object)
                {
                    image_index = other.i_index
                    i_index = other.i_index
                }
            "),
            new MslEvent(eventType: EventType.Draw, subtype: 0, code: "scr_draw_consum_scale()")
        );

        UndertaleGameObject o_loot_weapon_oil_parent = Msl.GetObject("o_loot_weapon_oil_parent");

        o_loot_weapon_oil_parent.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                image_speed = 0
                number = 0
            ")
        );

        AddWeaponOilDamageMechanism();
        AddWeaponOilTexts();
        AddWeaponOilObject("hanged_man_venom");
        AddWeaponOilObject("vampire_oil");
        AddWeaponOilObject("necrophage_oil");
        AddWeaponOilObject("specter_oil");
        AddWeaponOilObject("insectoid_oil");
        AddWeaponOilObject("hybrid_oil");
        AddWeaponOilObject("ogroid_oil");
    }

    private int oil_idx = 0;
    private void AddWeaponOilObject(string id)
    {
        UndertaleGameObject obj = Msl.AddObject(
            name: $"o_inv_{id}",
            parentName: "o_inv_weapon_oil_parent",
            spriteName: "s_inv_weapon_oil",
            isVisible: true,
            isPersistent: true,
            isAwake: true
        );

        obj.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @$"
                event_inherited()
                scr_consum_atr(""{id}"")
                i_index = {oil_idx}
            ")
        );

        UndertaleGameObject loot = Msl.AddObject(
            name: $"o_loot_{id}",
            parentName: "o_loot_weapon_oil_parent",
            spriteName: "s_loot_weapon_oil",
            isVisible: true,
            isPersistent: false,
            isAwake: true
        );

        loot.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @$"
                event_inherited()
                inv_object = o_inv_{id}
            ")
        );

        TableUtils.InjectTableItemStats(
            id: id,
            Material: TableUtils.ItemStatsMaterial.glass,
            Weight: TableUtils.ItemStatsWeight.Medium,
            Price: 200,
            Duration: 60,
            Stacks: 3,
            Cat: TableUtils.ItemStatsCategory.tool,
            bottle: true,
            tags: TableUtils.ItemStatsTags.special
        );

        oil_idx++;
    }

    private void AddWeaponOilTexts()
    {
        Msl.InjectTableItemsLocalization(
            new LocalizationItem(
                id: "hanged_man_venom",
                name: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Hanged man's venom"},
                    {ModLanguage.Chinese, "吊死鬼之毒"}
                },
                effect: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Applied to a weapon, it increases the damage dealt to humanoids by ~lg~10%~/~."},
                    {ModLanguage.Chinese, "应用于武器，对类人生物造成的伤害增加~lg~10%~/~。"}
                },
                description: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Hanged Man's Venom is a toxin that is equally lethal to humans, elves and dwarves. Applied to a blade, it deals more damage than any other coating. This oil is ineffective against monsters."},
                    {ModLanguage.Chinese, "吊死鬼之毒对于人类、精灵与矮人而言是致命的毒素。只要涂抹在剑刃上，它会比其他涂油造成更多的伤害。这种涂油对于怪物没多大效用。"}
                }
            ),

            new LocalizationItem(
                id: "vampire_oil",
                name: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Vampire Oil"},
                    {ModLanguage.Chinese, "吸血鬼油"}
                },
                effect: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Applied to a weapon, it increases the damage dealt to vampires by ~lg~10%~/~."},
                    {ModLanguage.Chinese, "应用于武器，对变节信徒造成的伤害增加~lg~10%~/~。"}
                },
                description: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Whosoever seeks to destroy a vampire, to banish it from this world forever, should prepare St. Gregory's Oil, called Vampire Oil by witchers. No fleder or bruxa can withstand it."},
                    {ModLanguage.Chinese, "无论谁想消灭吸血鬼，将其永远驱逐出这个世界，都应该准备圣格雷戈里油，猎魔人称之为吸血鬼油。任何蝙蝠或巫婆都无法抵御它。"}
                }
            ),

            new LocalizationItem(
                id: "necrophage_oil",
                name: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Necrophage Oil"},
                    {ModLanguage.Chinese, "死灵油"}
                },
                effect: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Applied to a weapon, it increases the damage dealt to undead by ~lg~10%~/~."},
                    {ModLanguage.Chinese, "应用于武器，对不死生灵造成的伤害增加~lg~10%~/~。"}
                },
                description: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Necrophages are accustomed to poisonous vapours. Yet even the most rancid ghouls and graveirs cannot withstand the poison wounds inflicted by a blade coated with Necrophage oil."},
                    {ModLanguage.Chinese, "亡灵习惯于有毒的蒸汽。然而，即使是最腐臭的食尸鬼和墓穴怪，也无法抵挡涂有死灵油的刀刃所造成的毒伤。"}
                }
            ),

            new LocalizationItem(
                id: "specter_oil",
                name: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Specter Oil"},
                    {ModLanguage.Chinese, "鬼灵油"}
                },
                effect: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Applied to a weapon, it increases the damage dealt to spectres by ~lg~20%~/~."},
                    {ModLanguage.Chinese, "应用于武器，对幽魂造成的伤害增加~lg~20%~/~。"}
                },
                description: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "There is a mysterious boundary between the worlds of the dead and the living, one which is easier to cross for restless specters than for humans. To injure a spectral opponent, first anoint a blade with this oil. Only then will the weapon truly part the curtain dividing the worlds, thereby damaging the specter."},
                    {ModLanguage.Chinese, "在亡灵世界和活人世界之间有一个神秘的界限，不安分的幽魂比人类更容易跨越这个界限。要伤害幽魂对手，首先要在刀刃上涂上这种油。只有这样，武器才能真正割开分隔两个世界的帷幕，从而伤害幽魂。"}
                }
            ),

            new LocalizationItem(
                id: "insectoid_oil",
                name: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Insectoid Oil"},
                    {ModLanguage.Chinese, "类虫生物油"}
                },
                effect: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Applied to a weapon, it increases the damage dealt to insectoid (such as Crawler, Buzzer, Rock Eater etc.) by ~lg~20%~/~."},
                    {ModLanguage.Chinese, "应用于武器，对类虫生物（比如巨蜘、亡蜂、食岩虫等）造成的伤害增加~lg~20%~/~。"}
                },
                description: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "This blade grease increases damage dealt to arachnids and creatures similar to insects in their physiology. It is the most effective oil against monsters of this type. Witchers also use Insectoid oil to rid their fortresses of bugs and parasites."},
                    {ModLanguage.Chinese, "这种剑油能增加对蛛形纲动物和生理结构与昆虫相似的生物造成的伤害。这是对付这类怪物最有效的油。猎魔人还使用这种油清除城堡中的虫子和寄生虫。"}
                }
            ),

            new LocalizationItem(
                id: "hybrid_oil",
                name: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Hybrid Oil"},
                    {ModLanguage.Chinese, "混种兽油"}
                },
                effect: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Applied to a weapon, it increases the damage dealt to hybrids (such as Harpy and Gulon) by ~lg~15%~/~."},
                    {ModLanguage.Chinese, "应用于武器，对混种兽（比如哈比和谷隆）造成的伤害增加~lg~15%~/~。"}
                },
                description: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Hybrids combine the capabilities, the strengths, and the weaknesses of different creatures, so they should never be taken lightly. However, this oil is very effective against monsters of this type."},
                    {ModLanguage.Chinese, "混种兽结合了不同生物的能力、长处和弱点，因此绝不能掉以轻心。不过，这种剑油对混种兽非常有效。"}
                }
            ),

            new LocalizationItem(
                id: "ogroid_oil",
                name: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Ogroid Oil"},
                    {ModLanguage.Chinese, "食人魔油"}
                },
                effect: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Applied to a weapon, it increases the damage dealt to trolls by ~lg~15%~/~."},
                    {ModLanguage.Chinese, "应用于武器，对巨魔造成的伤害增加~lg~15%~/~。"}
                },
                description: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Trolls are giant, eat or ate human flesh. This oil is very effective against trolls."},
                    {ModLanguage.Chinese, "巨魔是巨人，吃人肉。这种油对巨魔非常有效。"}
                }
            )
        );
    }

    private void AddWeaponOilDamageMechanism()
    {
        Msl.LoadAssemblyAsString("gml_GlobalScript_scr_damage_calculation")
            .MatchFrom("call.i gml_Script_scr_actionsLogVisible(argc=1)")
            .InsertAbove(@"pushloc.v local._damage
call.i gml_Script_scr_coating_oil_damage_calc(argc=2)
pop.v.v local._oil_damage
push.v local._damage
pushloc.v local._oil_damage
add.v.v
pop.v.v local._damage
push.v arg.argument0")
            .MatchFromUntil("push.s \"Slashing\"", "pushloc.v local._slashing")
            .InsertBelow(@"push.s ""Weapon_Oil""
conv.s.v
pushloc.v local._oil_damage")
            .MatchFrom("call.i @@NewGMLArray@@(argc=26)")
            .ReplaceBy("call.i @@NewGMLArray@@(argc=28)")
            .Save();

        Msl.LoadGML("gml_GlobalScript_scr_actionsLogGetDamageColorTag")
            .MatchFrom("_colorTag = \"~dp~\"")
            .InsertBelow("else if (argument0 == \"Weapon_Oil\")\n        _colorTag = \"~dg~\";")
            .Save();

        Msl.InjectTableLogDamagesLocalization(
            new LocalizationLogText(
                id: "Weapon_Oil",
                name: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "weapon oil"},
                    {ModLanguage.Chinese, "点剑油"}
                }
            )
        );
    }
}