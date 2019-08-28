using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.missileaddons.BeamCombos
{
	public class IceSpreaderAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spreader");
			Tooltip.SetDefault(string.Format("[c/9696FF:Missile Launcher Addon]\n") +
			"Slot Type: Charge\n" +
			"Hold Click to charge\n" + 
			"~Charge shots cover terrain in ice on impact, freezing enemies\n" + 
			"~Charge shots cost 10 missiles");
		}
		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.value = 2500;
			item.rare = 4;
			/*item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("IceSpreaderTile");*/
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.missileSlotType = 0;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sapphire, 10);
            recipe.AddIngredient(ItemID.FrostCore, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
