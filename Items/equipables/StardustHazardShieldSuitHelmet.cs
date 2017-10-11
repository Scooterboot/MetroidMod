using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Head)]
    public class StardustHazardShieldSuitHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stardust Hazard Shield Suit Helmet");
            Tooltip.SetDefault("5% increased ranged damage\n" +
            "+33 overheat capacity\n" +
            "Improved night vision\n" +
            "Increased jump height");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 10;
            item.value = 48000;
            item.defense = 18;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.nightVision = true;
            player.jumpBoost = true;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 33;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "HazardShieldSuitHelmet");
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddIngredient(ItemID.FragmentStardust, 10);
            recipe.AddIngredient(null, "EnergyTank");
            recipe.AddTile(null, "NovaWorkTable"); //this or Ancient Manipulator?
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}