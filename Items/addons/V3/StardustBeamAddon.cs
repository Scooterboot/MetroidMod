using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons.V3
{
	public class StardustBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Beam");
			Tooltip.SetDefault(string.Format("[c/FF9696:Power Beam Addon V3]\n") +
				"Slot Type: Secondary\n" +
				"Shots freeze enemies\n" + 
				"~Each time the enemy is shot, they will become 20% slower\n" + 
				"~After 5 shots the enemy will become completely frozen\n" + 
				string.Format("[c/78BE78:+175% damage]\n") +
				string.Format("[c/BE7878:+50% overheat use]\n") +
				string.Format("[c/BE7878:-30% speed]"));
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
			item.createTile = mod.TileType("StardustBeamTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.addonSlotType = 1;
		}

		/*public override void AddRecipes()
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
		}*/
	}
}
