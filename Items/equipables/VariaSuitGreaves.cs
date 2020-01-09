using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.equipables
{
	[AutoloadEquip(EquipType.Legs)]
	public class VariaSuitGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit Greaves");
			Tooltip.SetDefault("5% increased ranged damage\n" +
             "10% increased movement speed\n" +
             "+10 overheat capacity\n" +
             "Allows you to slide down walls");
        }
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 3;
            item.value = 6000;
            item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.moveSpeed += 0.10f;
            player.spikedBoots += 1;
            MPlayer mp = player.GetModPlayer<MPlayer>();
            mp.maxOverheat += 10;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PowerSuitGreaves");
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddIngredient(null, "EnergyTank");
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
