using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.balladdons
{
	public class SpiderBallAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spider Ball");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Utility\n" +
			"-Press the Spider Ball Key to activate Spider Ball\n" + 
			"-Allows you to climb on walls and ceilings");
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 1;
			item.value = 9000;
			item.rare = 3;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("SpiderBallTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.ballSlotType = 3;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Emerald, 2);
			recipe.AddIngredient(null, "EnergyShard", 4);
			recipe.AddIngredient(ItemID.Cobweb, 150);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
