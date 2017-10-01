using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.balladdons
{
	public class AdamantiteDrillAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adamantite Morph Ball Drill");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Drill\n" +
			"-Left Click while morphed to drill\n" +
			"-180% pickaxe power");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.maxStack = 1;
			item.value = 108000;
			item.rare = 4;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.ballSlotType = 0;
			mItem.drillPower = 180;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AdamantiteBar, 5);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
