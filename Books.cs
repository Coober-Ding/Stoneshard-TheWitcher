using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddBooks()
    {
        AddWitcherSkillBook();
    }

    private void AddWitcherSkillBook()
    {
        UndertaleGameObject o_inv_book_witcher1 = Msl.GetObject("o_inv_book_witcher1");
        UndertaleGameObject o_inv_book_witcher2 = Msl.GetObject("o_inv_book_witcher2");
        UndertaleGameObject o_loot_book_witcher1 = Msl.GetObject("o_loot_book_witcher1");
        UndertaleGameObject o_loot_book_witcher2 = Msl.GetObject("o_loot_book_witcher2");

        o_inv_book_witcher1.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                gain_xp = 50
                skills_array = [""Witcher""]
            "),

            new MslEvent(eventType: EventType.Other, subtype: 24, code: @"
                var _list = scr_atr(""recipesWitcherAlchemyOpened"")
                if (is_undefined(_list))
                {
                    _list = __dsDebuggerListCreate()
                    scr_atr_set(""recipesWitcherAlchemyOpened"", _list)
                }

                var _already_learned = true
                var _title = ds_list_find_value(global.other_hover, 77)

                if (ds_list_find_index(_list, ""hanged_man_venom"") < 0)
                {
                    ds_list_add(_list, ""hanged_man_venom"", ""vampire_oil"", ""necrophage_oil"",
                                        ""specter_oil"", ""insectoid_oil"", ""hybrid_oil"", ""ogroid_oil"")

                    var _name = ds_map_find_value_ext(global.crafting_category, ""weapon_oil"")
                    with (scr_notificationCreate(s_notification_icon_recipe, _title, _name, room_speed + 10, false))
                        scr_guiLayoutOffsetUpdate(id, (global.cameraWidth - sprite_width) / 2,
                            ((global.cameraHeight / 2) - 140) + (0 * (sprite_height + 2)))

                    scr_actionsLog(""learnSchematic"", [scr_id_get_name(o_player), _name])
                    _already_learned = false
                }

                if (ds_list_find_index(_list, ""thunderbolt_potion"") < 0)
                {
                    ds_list_add(_list, ""thunderbolt_potion"", ""blizzard_potion"", ""petri_philter"", ""swallow_potion"",
                                        ""tawny_owl"", ""golden_oriole"")

                    var _name = ds_map_find_value_ext(global.crafting_category, ""witcher_potion"")
                    with (scr_notificationCreate(s_notification_icon_recipe, _title, _name, room_speed + 20, false))
                        scr_guiLayoutOffsetUpdate(id, (global.cameraWidth - sprite_width) / 2,
                            ((global.cameraHeight / 2) - 140) + (1 * (sprite_height + 2)))

                    scr_actionsLog(""learnSchematic"", [scr_id_get_name(o_player), _name])
                    _already_learned = false
                }

                if (ds_list_find_index(_list, ""ghoul_decoction"") < 0)
                {
                    ds_list_add(_list, ""ghoul_decoction"", ""crawler_decoction"", ""harpy_decoction"", ""troll_decoction"",
                                        ""gulon_decoction"")

                    var _name = ds_map_find_value_ext(global.crafting_category, ""witcher_decoction"")
                    with (scr_notificationCreate(s_notification_icon_recipe, _title, _name, room_speed + 30, false))
                        scr_guiLayoutOffsetUpdate(id, (global.cameraWidth - sprite_width) / 2,
                            ((global.cameraHeight / 2) - 140) + (2 * (sprite_height + 2)))

                    scr_actionsLog(""learnSchematic"", [scr_id_get_name(o_player), _name])
                    _already_learned = false
                }

                if (_already_learned)
                {
                    if (!instance_exists(o_witcherAlchemyCraftingMenu))
                    {
                        with (scr_guiCreateContainer(global.guiBaseContainerSideLeft, o_witcherAlchemyCraftingMenu))
                        {
                            parent = other.id
                            event_user(1)
                        }
                    }
                }
                else
                {
                    audio_play_sound(snd_skill_book_use, 4, 0)

                    with (o_witcherAlchemyCraftingMenu)
                    {
                        event_user(11)
                        event_user(13)
                        event_user(12)
                    }
                }

                event_inherited()
            ")
        );

        o_inv_book_witcher2.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                gain_xp = 50
                skills_array = [
                    ""Witcher"", o_skill_quen_sign_ico, o_skill_axii_sign_ico, o_skill_yrden_sign_ico,
                    o_skill_aard_sign_ico, o_skill_igni_sign_ico]
            ")
        );

        o_loot_book_witcher1.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                inv_object = o_inv_book_witcher1
                number = 0
            ")
        );

        o_loot_book_witcher2.ApplyEvent(
            new MslEvent(eventType: EventType.Create, subtype: 0, code: @"
                event_inherited()
                inv_object = o_inv_book_witcher2
                number = 0
            ")
        );

        TableUtils.InjectTableItemStats(
            id: "book_witcher1",
            Price: 250,
            EffPrice: 50,
            Material: TableUtils.ItemStatsMaterial.paper,
            tier: TableUtils.ItemStatsTier.Tier1,
            Subcat: TableUtils.ItemStatsSubcategory.treatise,
            Weight: TableUtils.ItemStatsWeight.Light,
            tags: TableUtils.ItemStatsTags.special
        );

        TableUtils.InjectTableItemStats(
            id: "book_witcher2",
            Price: 250,
            EffPrice: 50,
            Material: TableUtils.ItemStatsMaterial.paper,
            tier: TableUtils.ItemStatsTier.Tier2,
            Subcat: TableUtils.ItemStatsSubcategory.treatise,
            Weight: TableUtils.ItemStatsWeight.Light,
            tags: TableUtils.ItemStatsTags.special
        );

        Msl.InjectTableBooksLocalization(
            new LocalizationBook(
                id: "book_witcher1",
                name: new Dictionary<ModLanguage, string>
                {
                    { ModLanguage.English, "Notes on Aetheric Alchemy" },
                    { ModLanguage.Chinese, "以太炼金札记" }
                },
                content: new Dictionary<ModLanguage, string>
                {
                    { ModLanguage.English, string.Join("##",
                        "( ... )",
                        "I am Idarran of Ulivo. The tear of the Spheres cast me into a continent the locals call Aldor—its air tastes of minerals I do not know, its flora whispers formulas I have not yet named. Perfect.",
                        "( ... )",
                        "Here, Aether replaces much of what my homeland’s chaos once did. By dissolving Aether into common solvents, I forced it to behave like a catalyst, letting unfamiliar herbs and ores stand in for my old world’s reagents.",
                        "( ... )",
                        "On Sword Oils",
                        "— Principle: name the prey, poison the blade accordingly. An oil is a restrained curse bound to steel.",
                        "— Base (render-fat or resin) + Pulverized Ore + Herb Tincture + Stabilizer (Aether condensate in drops).",
                        "Wolf Oil (Aldor Variant): Base Tallow + Nightglass Powder + Veilnettle Tincture + 3 drops Aether Condensate.",
                        "Specter Oil (Aldor Variant): Resin Base + Moon-Salt + Coldmoss Extract + 2 drops Aether.",
                        "( ... )",
                        "On Potions",
                        "— Potions are negotiations with the body. Offer clarity, demand a price.",
                        "Golden Oriole (Reproduced): Nettle + Burnet + Aether Microfiltrate.",
                        "Thunderbolt (Rebalanced): Hop Cone + Henbane + White Honey Trace.",
                        "( ... )",
                        "On Decoctions",
                        "— A decoction is an argument you win with the monster’s body.",
                        "Gulon Decoction: Gulon Liver + Henbane + Rhubarb (1 vial).",
                        "( ... )",
                        "Record your failures. They are the most literate teachers. I leave these notes for the next wanderer—use them, or outgrow them."
                    ) },
                    { ModLanguage.Chinese, string.Join("##",
                        "（ . . . ）",
                        "我，乌里沃的艾达兰。天球交汇的撕裂把我抛入一片名为“奥尔多”的大陆——空气里带着陌生的矿味，植物在耳边低语它们尚未被命名的配方。完美。",
                        "（ . . . ）",
                        "在此地，“以太”承担了我故乡“混沌”的许多职能。我将以太以微量溶入常见溶媒，使其充当催化，使不相识的草药与矿石得以扮演旧世界试剂的角色。",
                        "（ . . . ）",
                        "关于剑油",
                        "—— 原理：先指名猎物，再以毒束于钢。剑油是一种被约束的诅咒。",
                        "—— 基底（油脂或树脂）＋研磨矿粉＋草药酊剂＋稳定剂（以太冷凝，滴计）。",
                        "混种兽油·奥尔多式：油脂基底＋“夜玻”粉末＋“帷荨”酊剂＋以太冷凝3滴。",
                        "幽灵油·奥尔多式：树脂基底＋“月盐”＋“寒藓”萃取＋以太2滴。",
                        "（ . . . ）",
                        "关于魔药",
                        "—— 魔药是与血肉的谈判：给它清晰，向它索取代价。",
                        "金莺（复现）：荨麻＋地榆＋以太微滤。",
                        "雷霆（再平衡）：蛇麻＋天仙子＋白蜂蜜痕量。",
                        "（ . . . ）",
                        "关于煎药",
                        "—— 煎药是一场用怪物之身赢下的辩论。",
                        "谷隆煎药：谷隆肝＋天仙子＋大黄（1小瓶）。",
                        "（ . . . ）",
                        "把失败写下来。它们是最有学问的导师。我把这些笔记留给后来者——拿去用，或把它们超越。"
                    ) }
                },
                midText: new Dictionary<ModLanguage, string>
                {
                    { ModLanguage.English, "\"Notes on Aetheric Alchemy\"##~gr~Allows you to learn:~/~##~lg~The Witcher's Alchemy~/~#~lg~Weapon Oil Recipes~/~#~lg~Witcher Potion Recipes~/~#~lg~Witcher Decoction Recipes~/~##Reading this book grants some ~y~Experience~/~." },
                    { ModLanguage.Chinese, "《以太炼金札记》##~gr~可学习：~/~##~lg~猎魔人炼金术~/~#~lg~剑油配方~/~#~lg~魔药配方~/~#~lg~煎药配方~/~##阅读本书可获得~y~经验~/~。" }
                },
                description: new Dictionary<ModLanguage, string>
                {
                    { ModLanguage.English, "Idarran’s first-person notes on oils, potions, and decoctions adapted to Aldor." },
                    { ModLanguage.Chinese, "艾达兰以第一人称记录他在奥尔多对剑油、魔药与煎药的改造方法与实用配方示例。" }
                },
                type: new Dictionary<ModLanguage, string>
                {
                    { ModLanguage.English, "Written by Idarran of Ulivo" },
                    { ModLanguage.Chinese, "作者：乌里沃的艾达兰" }
                }
            ),
            new LocalizationBook(
                id: "book_witcher2",
                name: new Dictionary<ModLanguage, string>
                {
                    { ModLanguage.English, "On Aetheric Signs" },
                    { ModLanguage.Chinese, "以太符印论" }
                },
                content: new Dictionary<ModLanguage, string>
                {
                    { ModLanguage.English, string.Join("##",
                        "( ... )",
                        "Two magics faced each other when I fell between worlds. My homeland’s currents—runes, Signs, disciplined chaos. Aldor’s lattice—Aether, pressure, and resonance.",
                        "( ... )",
                        "Comparative Anatomy of Power",
                        "— Homeland Sign = Intent shaped through gesture + breath + focus. Output is angular, discrete.",
                        "— Aldor Spellwork = Formula through Aether differentials. Output is wave-like, continuous.",
                        "They are not rivals, they are dialects.",
                        "( ... )",
                        "Rewriting Witcher Signs with Aether",
                        "Quen (Aether Shell): Replace rigid shield with resonant membrane. Costs less focus",
                        "Yrden (Aether Net): Lay a low-frequency grid. Slows incorporeal entities by forcing partial manifestation.",
                        "Axii (Harmonic Impulse): No domination—only detuning. Nudge hostile minds off-key, safer, subtler.",
                        "Aard (Vector Push): Convert cone burst into directed vector. Shorter wind-up, tighter knockback.",
                        "Igni (Combustion Bias): Seed heat in the target, not the air. Sparks fewer, burns truer.",
                        "( ... )",
                        "On Discipline",
                        "Aether forgives neither panic nor vanity. Breathe as if counting the teeth of a key. Then turn.",
                        "( ... )",
                        "This is only the prologue. Every Sign can be translated—and improved—provided you accept that language is a tool, not a temple."
                    ) },
                    { ModLanguage.Chinese, string.Join("##",
                        "（ . . . ）",
                        "当我坠入两个世界之间时，两种魔法彼此对视。故乡的脉流——符印、法印、被驯服的混沌；奥尔多的骨架——以太、压强与共振。",
                        "（ . . . ）",
                        "“力量”的比较解剖",
                        "—— 故乡法印＝以“手势＋呼吸＋专注”塑形的意志，输出呈“角质化、离散”特征。",
                        "—— 奥尔多术式＝以“以太差”驱动的公式，输出呈“波形、连续”特征。",
                        "它们不是对手，而是两种方言。",
                        "（ . . . ）",
                        "以奥尔多体系改写猎魔人法印",
                        "昆恩（以太壳）：将“刚性盾”改为“共振膜”。占用专注更少。",
                        "亚登（以太网）：铺设低频网格。强迫灵体“半现形”，从而减速与固着。",
                        "亚克西（谐波脉）：非支配，而是“跑调”。将敌意心智推向失谐，隐秘而稳妥。",
                        "阿尔德（向量推）：把锥形冲击改写为定向向量。蓄势更短；击退更紧。",
                        "伊格尼（内燃偏置）：在目标体内播种热量，而非灼烧空气。火花更少，燃烧更真。",
                        "（ . . . ）",
                        "关于“操练”",
                        "以太既不宽恕恐慌，也不宽恕虚荣。像数钥齿那样呼吸，然后——转动它。",
                        "（ . . . ）",
                        "这只是序章。所有法印都可被翻译，乃至改良——前提是你承认语言是工具，而非神龛。"
                    ) }
                },
                midText: new Dictionary<ModLanguage, string>
                {
                    { ModLanguage.English, "\"On Aetheric Signs\"##~gr~Allows you to learn:~/~##~lg~Quen Sign~/~#~lg~Axii Sign~/~#~lg~Yrden Sign~/~#~lg~Aard Sign~/~#~lg~Igni Sign~/~##Reading this book grants some ~y~Experience~/~." },
                    { ModLanguage.Chinese, "《以太符印论》##~gr~可学习：~/~##~lg~昆恩法印~/~#~lg~亚克西法印~/~#~lg~亚登法印~/~#~lg~阿尔德法印~/~#~lg~伊格尼法印~/~##阅读本书可获得~y~经验~/~。" }
                },
                description: new Dictionary<ModLanguage, string>
                {
                    { ModLanguage.English, "Idarran’s comparative study of Aldor’s Aether and homeland Signs, with practical rewrites." },
                    { ModLanguage.Chinese, "艾达兰对“以太”与故乡法印的对照研究，并给出可实操的改写方案与训练要点。" }
                },
                type: new Dictionary<ModLanguage, string>
                {
                    { ModLanguage.English, "Written by Idarran of Ulivo" },
                    { ModLanguage.Chinese, "作者：乌里沃的艾达兰" }
                }
            )

        );
    }
}