using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
	public class IceBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Beam");
			Tooltip.SetDefault("Power Beam Addon\n" +
			"Slot Type: Secondary\n" +
			"Shots freeze enemies\n" + 
			"~Each time the enemy is shot, they will become 20% slower\n" + 
			"~After 5 shots the enemy will become completely frozen\n" + 
			"+100% damage\n" +
			"+25% overheat use\n" +
				"---\n" +
				"Missile Launcher Addon\n" +
				"Slot Type: Primary\n" +
				"Shots freeze enemies instantly\n" +
				"+50% damage");
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
			item.createTile = mod.TileType("IceBeamTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.addonSlotType = 1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 3);
            		recipe.AddIngredient(ItemID.SnowBlock, 25);
            		recipe.AddIngredient(ItemID.IceBlock, 10);
            		recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 3);
            		recipe.AddIngredient(ItemID.Sapphire, 7);
            		recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
