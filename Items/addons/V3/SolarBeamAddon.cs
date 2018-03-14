using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons.V3
{
	public class SolarBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Beam");
			Tooltip.SetDefault(string.Format("[c/FF9696:Power Beam Addon V3]\n") +
				"Slot Type: Primary B\n" + 
				"Shots pierce enemies\n" + 
				"Shots set enemies ablaze with the Daybroken debuff\n" + 
				string.Format("[c/78BE78:+175% damage]\n") +
				string.Format("[c/BE7878:+150% overheat use]\n") +
				string.Format("[c/BE7878:-15% speed]"));
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
			item.createTile = mod.TileType("SolarBeamTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.addonSlotType = 4;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlasmaBeamGreenAddon");
            		recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
            		recipe.AddIngredient(ItemID.Emerald, 5);
			recipe.AddIngredient(ItemID.SoulofSight, 5);
            		recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlasmaBeamRedAddon");
            		recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
            		recipe.AddIngredient(ItemID.Emerald, 5);
			recipe.AddIngredient(ItemID.SoulofSight, 5);
		    	recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
