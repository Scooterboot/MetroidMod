using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
	public class PlasmaBeamGreenAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Beam (Green)");
			Tooltip.SetDefault("Power Beam Addon\n" +
				"Slot Type: Primary B\n" +
				"Shots pierce enemies, hitting them multiple times\n" +
				"+300% damage\n" +
				"+100% overheat use");
		}
		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.value = 2500;
			item.rare = 4;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("PlasmaBeamGreenTile");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 3);
            recipe.AddIngredient(ItemID.CursedFlame, 15);
            recipe.AddIngredient(ItemID.Emerald, 5);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
	}
}
