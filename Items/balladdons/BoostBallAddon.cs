using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.balladdons
{
	public class BoostBallAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boost Ball");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Boost\n" +
			"-Hold Boost Ball Key to charge a speed boost\n" + 
			"-Release the key to accelerate in the direction you are moving");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.maxStack = 1;
			item.value = 11000;
			item.rare = 3;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("BoostBallTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.ballSlotType = 4;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Diamond, 2);
			recipe.AddIngredient(null, "ChoziteBar", 8);
			recipe.AddIngredient(null, "EnergyTank");
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
