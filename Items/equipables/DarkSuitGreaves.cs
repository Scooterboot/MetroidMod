using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
    [AutoloadEquip(EquipType.Legs)]
    public class DarkSuitGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Suit Greaves");
            Tooltip.SetDefault("5% increased ranged damage\n" +
             "10% increased movement speed\n" +
             "+20 overheat capacity\n" +
             "Allows you to cling to walls");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 5;
            item.value = 24000;
            item.defense = 13;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.moveSpeed += 0.1f;
            player.spikedBoots += 2;
            MPlayer mp = player.GetModPlayer<MPlayer>();
            mp.maxOverheat += 20;
        }

        /*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "VariaSuitV2Greaves");
            //Dark World materials go here
            recipe.AddIngredient(null, "EnergyTank");
            //recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }*/
    }
}
