using System; 
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.addons.V2
{
	public class WideBeamAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wide Beam");
			Tooltip.SetDefault(string.Format("[c/FF9696:Power Beam Addon V2]\n") +
				"Slot Type: Primary A\n" +
				"Beam fires 3 shots at once, effectively tripling its damage\n" +
				string.Format("[c/78BE78:+50% damage]\n") +
				string.Format("[c/BE7878:+75% overheat use]\n") +
				string.Format("[c/78BE78:+15% speed]"));
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
			item.createTile = mod.TileType("WideBeamTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.addonSlotType = 3;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SpazerAddon", 1);
		    recipe.AddIngredient(ItemID.Topaz, 15);
		    recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
		    recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
	}
}
