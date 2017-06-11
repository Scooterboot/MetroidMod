using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Legs)]
    public class VariaSuitV2Greaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Varia Suit V2 Greaves");
            Tooltip.SetDefault("5% increased ranged damage\n" +
             "10% increased movement speed\n" +
             "+15 overheat capacity\n" +
             "Allows you to slide down walls");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 4;
            item.value = 12000;
            item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.moveSpeed += 0.1f;
            player.spikedBoots += 1;
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            mp.maxOverheat += 15;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "VariaSuitBreastplate");
            recipe.AddIngredient(ItemID.MythrilBar, 15);
            //recipe.AddIngredient(ItemID.SoulofLight, 5);
            //recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(null, "EnergyTank");
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "VariaSuitBreastplate");
            recipe.AddIngredient(ItemID.OrichalcumBar, 15);
            //recipe.AddIngredient(ItemID.SoulofLight, 5);
            //recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(null, "EnergyTank");
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
