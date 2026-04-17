using System.Text.RegularExpressions;

using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddSkill_Witcher_Alchemy()
    {
        AdjustSkillIcon("s_witcher_alchemy");
        AddWitcherAlchemy_Alcohol();

        // Skill - Witcher Alchemy

        Msl.InjectTableSkillsStats(
            id: "Witcher_Alchemy",
            Object: "object",
            hook: Msl.SkillsStatsHook.BASIC
        );

        UndertaleGameObject o_skill_witcher_alchemy = Msl.GetObject("o_skill_witcher_alchemy");

        AddWeaponOil();
        AddWitcherPotion();
        AddWitcherDecoction();
        AddWitcherAlchemyCraftingMenu();
        AddBrynnUniversityAlchemyStation();

        o_skill_witcher_alchemy.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                skill = ""Witcher_Alchemy""
                scr_skill_atr(""Witcher_Alchemy"")
            "),
            new MslEvent(eventType: EventType.Other, subtype: 13, code: @"
                with (o_player)
                    lock_movement = false
                if (!instance_exists(o_witcherAlchemyCraftingMenu))
                {
                    with (scr_guiCreateContainer(global.guiBaseContainerSideLeft, o_witcherAlchemyCraftingMenu))
                    {
                        parent = other.id
                        event_user(1)
                    }
                }
            "),
            new MslEvent(eventType: EventType.Other, subtype: 14, code: @"
                event_inherited()
                is_ready = true
            "),
            new MslEvent(eventType: EventType.Other, subtype: 17, code: @"
                event_inherited()
                MPcost = 0
                KD = 0
            ")
        );

        UndertaleGameObject o_skill_witcher_alchemy_ico = Msl.GetObject("o_skill_witcher_alchemy_ico");

        o_skill_witcher_alchemy_ico.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                child_skill = o_skill_witcher_alchemy
                event_perform_object(child_skill, ev_create, 0)
            ")

        // 每次游戏加载成功，这个事件就会执行一次。
        /*
        new MslEvent(eventType: EventType.Other, subtype: 18, code: @"
            event_inherited()

            var _list = scr_atr(""recipesWitcherAlchemyOpened"")
            if (is_undefined(_list))
            {
                _list = __dsDebuggerListCreate()
                scr_atr_set(""recipesWitcherAlchemyOpened"", _list)
            }

            with (o_craftingMenu)
            {
                event_user(11)
                event_user(13)
                event_user(12)
            }
        ")
        */
        );

        AddCaravanAlchemyStation();
    }

    private void AddWitcherAlchemy_Alcohol()
    {
        UndertaleGameObject o_inv_alcohol_essentia = Msl.GetObject("o_inv_alcohol_essentia");
        UndertaleGameObject o_loot_alcohol_essentia = Msl.GetObject("o_loot_alcohol_essentia");

        Msl.InjectTableItemStats(
            id: "alcohol_essentia",
            Price: 60,
            Cat: Msl.ItemStatsCategory.alcohol,
            Material: Msl.ItemStatsMaterial.glass,
            Weight: Msl.ItemStatsWeight.Light,
            Duration: 240,
            Thirsty: 70,
            Intoxication: 40,
            Toxicity_Change: 1,
            Pain_Change: -0.5f,
            Sanity_Change: -0.2f,
            bottle: true,
            tags: Msl.ItemStatsTags.special
        );

        o_inv_alcohol_essentia.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                scr_consum_atr(""alcohol_essentia"")
                max_charge = 1
                can_merge = false
                drop_gui_sound = snd_item_ether_inhaler_drop
                pickup_sound = snd_item_ether_inhaler_pick
                is_execute = false
                Hit_Bonus = 30
            "),

            new MslEvent(eventType: EventType.Other, subtype: 24, code: @"
                event_inherited()
                audio_play_sound(snd_gui_drink_potion, 3, 0)
                scr_effect_create(o_db_drunk, 360 * (((1.25 + (0.01 * scr_atr(""Hunger""))) - (0.025 * o_player.Vitality)) * (1 - (o_player.Toxicity_Resistance / 100))))
            "),

            new MslEvent(eventType: EventType.Other, subtype: 22, code: "instance_destroy()")
        );

        o_loot_alcohol_essentia.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                inv_object = o_inv_alcohol_essentia
                number = 0
            ")
        );
    }

    private void AddWitcherAlchemyCraftingMenu()
    {
        UndertaleGameObject o_witcherAlchemyCraftingMenu = Msl.GetObject("o_witcherAlchemyCraftingMenu");

        o_witcherAlchemyCraftingMenu.Group = 1;

        Msl.AddFunction(ModFiles.GetCode("scr_craft_alchemy_base.gml"), "scr_craft_alchemy_base");
        Msl.AddFunction(ModFiles.GetCode("scr_craft_oil.gml"), "scr_craft_oil");
        Msl.AddFunction(ModFiles.GetCode("scr_craft_alcohol_essentia.gml"), "scr_craft_alcohol_essentia");

        o_witcherAlchemyCraftingMenu.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                var _list = scr_atr(""recipesWitcherAlchemyOpened"")
                if (is_undefined(_list))
                {
                    _list = __dsDebuggerListCreate()
                    scr_atr_set(""recipesWitcherAlchemyOpened"", _list)
                }

                var _basic_items = [""oil"", ""alcohol_essentia""]
                for (var i = 0; i < array_length(_basic_items); i++)
                {
                    if (ds_list_find_index(_list, _basic_items[i]) < 0)
                    {
                        ds_list_add(_list, _basic_items[i])
                    }
                }

                event_inherited()
                with (mask)
                    title = ds_map_find_value(global.skill_name_text, ""Witcher_Alchemy"")
                with (craftButton)
                    text = ds_list_find_value(global.inv_text, 72)
                rightContainerBG = s_CookingBG_Consums_kit
            "),

            new MslEvent(eventType: EventType.Draw, subtype: 0, code: @"
                event_inherited()
                with (consumsContainer)
                {
                    for (var _i = 0; _i < guiChildrenCount; _i++)
                    {
                        with (ds_list_find_value(guiChildrenList, _i))
                        {
                            for (var _j = 0; _j < guiChildrenCount; _j++)
                            {
                                with (ds_list_find_value(guiChildrenList, _j))
                                {
                                    if (!select)
                                        draw_sprite_ext(s_point, 0, x - 1, y - 1, sprite_width + 2, sprite_height + 2, 0, make_color_rgb(17, 16, 26), 1);
                                }
                            }
                        }
                    }
                }
            "),

            new MslEvent(eventType: EventType.Other, subtype: 11, code: @"
                if instance_exists(parent)
                {
                    if (object_is_ancestor(parent.object_index, c_container))
                    {
                        var _list = parent.loot_list
                        var _size = ds_list_size(_list)
                        
                        if (_size > 0)
                            scr_loadContainerContent(_list, object_index, noone, id, false, false)
                        
                        workbench = true
                    }

                    if (parent.object_index == o_skill_witcher_alchemy)
                        rightContainerBG = s_CookingBG_Consums
                }
                if (scr_caravanPositionGetDistance() == 0)
                {
                    inventoryCategories = scr_guiCreateContainer(global.guiBaseContainerVisible, o_inventory_categories_menu, -12200, 0, 0, 2)
                    with (inventoryCategories)
                        event_user(0)
                }
                with (consumsContainer)
                    contentType = o_loot
                event_inherited()
            "),

            new MslEvent(eventType: EventType.Other, subtype: 12, code: ""),

            // Check if it is allowed to craft weapon oil or potions
            new MslEvent(eventType: EventType.Other, subtype: 13, code: @"
                item_array = []

                if (idName == ""oil"")
                    canCraft = scr_craft_oil()
                else if (idName == ""alcohol_essentia"")
                    canCraft = scr_craft_alcohol_essentia()
                else
                    canCraft = scr_crafting_recipe_components_check(activeRecipeButton, true)

                event_inherited()
            "),

            // Crafting
            new MslEvent(eventType: EventType.Other, subtype: 14,
                code: ModFiles.GetCode("o_witcherAlchemyCraftingMenu_other_14.gml")),

            new MslEvent(eventType: EventType.Other, subtype: 24, code: @"
                event_inherited()
                componentsContainer = scr_inventory_container_create(itemsContainer, 5, o_inv_slot, 7, 7)
                scr_inventory_container_cells_add(id, componentsContainer, 4)
                consumsContainer = scr_inventory_container_create(itemsContainer, 4, o_inv_slot, 21, 140)
                scr_inventory_container_cells_add(id, consumsContainer, 3)
            ")
        );

        // Add Witcher Alchemy Crafting to existing system
        Msl.LoadGML("gml_Object_o_craftingMenu_Other_23")
            .MatchFrom("case o_craftingFoodMenu:")
            .InsertAbove(@"
    case o_witcherAlchemyCraftingMenu:
        _recipesOpenedList = scr_atr(""recipesWitcherAlchemyOpened"")
        _recipesCategoriesOrderList = global.recipes_witcher_alchemy_categories_order_list
        _recipesCategoryOrderMap = global.recipes_witcher_alchemy_category_order_map
        break;")
            .Save();

        Msl.LoadGML("gml_Object_o_craftingMenu_Other_22")
            .MatchFrom("case o_craftingFoodMenu:")
            .InsertAbove(@"
    case o_witcherAlchemyCraftingMenu:
        _recipeKey = global.craftingWitcherAlchemyMenuRecipeKey
        _scrolledHeight = global.craftingWitcherAlchemyMenuScrolledHeight
        break;")
            .Save();

        Msl.LoadGML("gml_Object_o_craftingMenu_Other_21")
            .MatchFrom("case o_craftingFoodMenu:")
            .InsertAbove(@"
    case o_witcherAlchemyCraftingMenu:
        with (activeRecipeButton)
            global.craftingWitcherAlchemyMenuRecipeKey = idName
        with (scrollbar)
            global.craftingWitcherAlchemyMenuScrolledHeight = scrolledHeight
        break;")
            .Save();

        // Load recorded recipes
        Msl.LoadGML("gml_GlobalScript_scr_characterMapInit")
            .MatchFrom("var _recipesIterableArray = ")
            .InsertBelow(@"
        var _recipesWitcherAlchemyOpenedList = __dsDebuggerListCreate()
        array_push(_recipesIterableArray, global.recipes_witcher_alchemy_data, _recipesWitcherAlchemyOpenedList)")
            .MatchFrom("ds_map_add_list(global.characterDataMap, \"recipesFoodOpened\", _recipesFoodOpenedList)")
            .InsertAbove("ds_map_add_list(global.characterDataMap, \"recipesWitcherAlchemyOpened\", _recipesWitcherAlchemyOpenedList)")
            .Save();

        // Init certain global vars
        Msl.LoadGML("gml_GlobalScript_scr_sessionDataInit")
            .MatchFrom("global.craftingConsumsMenuScrolledHeight = 0")
            .InsertBelow(@"
    global.craftingWitcherAlchemyMenuRecipeKey = ""N/A""
    global.craftingWitcherAlchemyMenuScrolledHeight = 0
            ")
            .Save();

        // Adjust coordinates of alchemy container
        int index = DataLoader.data.GameObjects.IndexOf(
            DataLoader.data.GameObjects.First(x => x.Name.Content == "o_craftingConsumsMenu"));
        Msl.LoadGML("gml_GlobalScript_scr_adaptiveMenusGetOffset")
            .MatchFrom($"case {index}:")
            .InsertBelow("        case o_witcherAlchemyCraftingMenu:")
            .Save();

        // Add Witcher Alchemy to search for recipes
        Msl.LoadGML("gml_GlobalScript_scr_crafting_recipe_get_map")
            .MatchFromUntil("function scr_crafting_recipe_get_map", "}")
            .ReplaceBy(@"
function scr_crafting_recipe_get_map()
{
    var _to_check = [global.recipes_food_data, global.recipes_consums_data, global.recipes_witcher_alchemy_data]
    for (var i = 0; i < array_length(_to_check); i++)
    {
        var _recipeDataMap = ds_map_find_value(_to_check[i], argument0)
        if (!__is_undefined(_recipeDataMap))
            return _recipeDataMap;
    }
}")
            .Save();

        // Load Witcher Alchemy Recipes
        UndertaleGameObject ob = Msl.AddObject("o_mod_witcher_initializer", isPersistent: true);
        Msl.AddNewEvent(ob, ModFiles.GetCode("load_recipes.gml"), EventType.Create, 0);
        UndertaleRoom start = Msl.GetRoom("START");
        start.AddGameObject("Instances", ob);

        // Add interaction with items
        Msl.LoadGML("gml_Object_o_inv_slot_Other_13")
            .MatchFrom("case o_craftingConsumsMenu:")
            .InsertBelow("                case o_witcherAlchemyCraftingMenu:")
            .Save();

        // It seems we need to update this ASM codes every Stoneshard updates.
        /*
        int index_menu = DataLoader.data.GameObjects.IndexOf(
            DataLoader.data.GameObjects.First(x => x.Name.Content == "o_craftingConsumsMenu"));

        var fe0 = Msl.LoadAssemblyAsString("gml_Object_o_inv_slot_Mouse_4");
        var matched = fe0.MatchFromUntil($"pushi.e {index}", "bt [");
        var btNum = matched.ienumerable
            .Where(t => t.Item1 == ModShardLauncher.Match.Matching)
            .Select(t => Regex.Match(t.Item2, @"\bbt\s*\[(\d+)\]"))
            .FirstOrDefault(r => r.Success)?
            .Groups[1].Value.ThrowIfNull();

        Msl.LoadAssemblyAsString("gml_Object_o_inv_slot_Mouse_4")
            .MatchFromUntil($"pushi.e {index}", "bt [")
            .InsertBelow(@$"
:[1000]
dup.v 0
pushi.e {DataLoader.data.GameObjects.IndexOf(o_witcherAlchemyCraftingMenu)}
cmp.i.v EQ
bt [{btNum}]")
            .Save();
        */

    }

    private void AddCaravanAlchemyStation()
    {
        Msl.LoadAssemblyAsString("gml_Object_o_caravanBranch_Cooking_Create_0")
            .MatchFromUntil("pushloc.v local._alchemy", "popenv")
            .ReplaceBy("")
            .Save();

        Msl.LoadGML("gml_Object_o_AlchemyTable_hl_Create_0")
            .MatchFrom("name = ds_map_find_value(global.consum_name")
            .ReplaceBy("name = ds_map_find_value(global.caravan_upgrade_names, \"Alchemy\")")
            .Save();

        Msl.AddNewEvent(
            objectName: "o_AlchemyTable_hl",
            eventType: EventType.Other,
            subtype: 10,
            eventCode: @"
                if (!instance_exists(o_witcherAlchemyCraftingMenu))
                {
                    with (scr_guiCreateContainer(global.guiBaseContainerSideLeft, o_witcherAlchemyCraftingMenu))
                    {
                        parent = other.id
                        event_user(1)
                    }
                }
            "
        );

        Msl.AddNewEvent(
            objectName: "o_AlchemyTable_hl",
            eventType: EventType.Other,
            subtype: 15,
            eventCode: @""
        );
    }

    private void AddBrynnUniversityAlchemyStation()
    {
        UndertaleSprite spr = Msl.GetSprite("s_BrynnUniversityAlchemyTable");
        spr.CollisionMasks.RemoveAt(0);
        spr.OriginX = 26;
        spr.OriginY = 23;
        spr.IsSpecialType = true;
        spr.SVersion = 3;
        spr.GMS2PlaybackSpeed = 15;
        spr.GMS2PlaybackSpeedType = AnimSpeedType.FramesPerSecond;

        UndertaleRoom room = Msl.GetRoom("r_BrynnUniversityCellar");

        UndertaleRoom.Layer foreground = Msl.GetLayer(room, UndertaleRoom.LayerType.Instances, "ForegroundInstances");
        UndertaleRoom.GameObject loot_marker1 = Msl.ThrowIfNull(
            foreground.InstancesData.Instances.First(
                t => t.ObjectDefinition.Name.Content == "o_loot_marker" && t.X == 323 && t.Y == 519));
        foreground.InstancesData.Instances.Remove(loot_marker1);
        room.GameObjects.Remove(loot_marker1);

        UndertaleRoom.GameObject loot_marker2 = Msl.ThrowIfNull(
            foreground.InstancesData.Instances.First(
                t => t.ObjectDefinition.Name.Content == "o_loot_marker" && t.X == 321 && t.Y == 540));
        foreground.InstancesData.Instances.Remove(loot_marker2);
        room.GameObjects.Remove(loot_marker2);

        UndertaleRoom.GameObject loot_marker3 = room.AddGameObject(
            foreground,
            "o_loot_marker",
            Msl.AddCode(
                name: "gml_RoomCC_r_BrynnUniversityCellar_1000_Create",
                codeAsString: @"
                    chance = 100
                    loot = o_loot_oil
                    depthshift = 20
                    hasOwner = true
            "),
            x: 318, y: 516
        );

        UndertaleRoom.Layer stuff = Msl.GetLayer(room, UndertaleRoom.LayerType.Instances, "StuffInstances");
        UndertaleRoom.GameObject table = Msl.ThrowIfNull(
            stuff.InstancesData.Instances.First(
                t => t.ObjectDefinition.Name.Content == "o_BrynnUniversityStuff11" && t.X == 327 && t.Y == 526));
        table.ObjectDefinition.Sprite = Msl.GetSprite("s_BrynnUniversityAlchemyTable");

        UndertaleRoom.GameObject grass_target = room.AddGameObject(
            stuff,
            "o_AlchemyTable_hl",
            x: 327, y: 546
        );
    }
}
