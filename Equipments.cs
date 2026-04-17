using ModShardLauncher;
using ModShardLauncher.Mods;
using UndertaleModLib.Models;

namespace TheWitcher;

public partial class TheWitcher : Mod
{
    private void AddEuipments()
    {
        // AddEuipments_Ursine();
    }

    private void AddEuipments_Ursine()
    {
        Msl.InjectTableArmor(
            hook: Msl.ArmorHook.CHESTPIECES,
            name: "Ursine Armor",
            Tier: Msl.ArmorTier.Tier5,
            id: "ursinearmor01",
            Slot: Msl.ArmorSlot.Chest,
            Class: Msl.ArmorClass.Heavy,
            rarity: Msl.ArmorRarity.Unique,
            Mat: Msl.ArmorMaterial.metal,
            tags: Msl.ArmorTags.special,
            MaxDuration: 360,
            Price: 12450,
            Markup: 1.25f,
            DEF: 24
        );

        Msl.InjectTableWeaponTextsLocalization(
            new LocalizationWeaponText(
                id: "Ursine Armor",
                name: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "Ursine Armor"},
                    {ModLanguage.Chinese, "熊派盔甲"}
                },
                description: new Dictionary<ModLanguage, string>() {
                    {ModLanguage.English, "WIP"},
                    {ModLanguage.Chinese, "WIP"}
                }
            )
        );
    }
}