using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons.V2
{
	public class NovaBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Beam");
			Tooltip.SetDefault(string.Format("[c/FF9696:Power Beam Addon V2]\n") +
			"Slot Type: Primary B\n" + 
				"Shots pierce enemies\n" + 
				"Shots set enemies ablaze with Cursed Fire, or Frost Burns them if Ice Beam is installed\n" + 
				string.Format("[c/78BE78:+225% damage]\n") +
				string.Format("[c/BE7878:+100% overheat use]\n") +
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
			item.createTile = mod.TileType("NovaBeamTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 4;
			mItem.addonDmg = 2.25f;
			mItem.addonHeat = 1f;
			mItem.addonSpeed = -0.15f;
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
