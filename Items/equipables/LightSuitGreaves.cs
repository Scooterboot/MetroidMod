using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Legs)]
    public class LightSuitGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Suit Greaves");
            Tooltip.SetDefault("5% increased ranged damage\n" +
            "20% increased movement speed\n" +
            "+25 overheat capacity\n" +
            "Allows you to cling to walls");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 7;
            item.value = 36000;
            item.defense = 17;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.moveSpeed += 0.20f;
            player.spikedBoots += 2;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 25;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DarkSuitGreaves");
            recipe.AddIngredient(ItemID.HallowedBar, 20);
            //recipe.AddIngredient(null, "", 10); Dark World Material
            recipe.AddIngredient(null, "EnergyTank");
            recipe.AddTile(null, "NovaWorkTable");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
