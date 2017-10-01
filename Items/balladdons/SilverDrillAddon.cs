using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.balladdons
{
	public class SilverDrillAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Silver Morph Ball Drill");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Drill\n" +
			"-Left Click while morphed to drill\n" +
			"-45% pickaxe power");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.maxStack = 1;
			item.value = 5000;
			item.rare = 0;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.ballSlotType = 0;
			mItem.drillPower = 45;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SilverBar, 5);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
