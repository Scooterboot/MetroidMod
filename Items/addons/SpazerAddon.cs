using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons
{
	public class SpazerAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spazer");
			Tooltip.SetDefault(string.Format("[c/9696FF:Power Beam Addon]\n") +
				"Slot Type: Primary A\n" +
				"Beam fires 3 shots at once, effectively tripling its damage\n" +
				string.Format("[c/78BE78:+25% damage]\n") +
				string.Format("[c/BE7878:+50% overheat use]"));
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
			item.createTile = mod.TileType("SpazerTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.addonSlotType = 3;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 6);
		    	recipe.AddIngredient(ItemID.Topaz, 15);
		    	recipe.AddIngredient(ItemID.JungleSpores, 10);
		    	recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
	}
}
