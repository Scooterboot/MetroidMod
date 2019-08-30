using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.missileaddons.BeamCombos
{
	public class VortexComboAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Storm");
			Tooltip.SetDefault(string.Format("[c/9696FF:Missile Launcher Addon]\n") +
			"Slot Type: Charge\n" +
			"Hold Click to charge\n" + 
			"~Fires vortexes that orbit the player at full charge\n" + 
			"~Each vortex fires a blast after 2 seconds\n" + 
			"~Releasing the charge makes all vortexes immediately fire\n" + 
			"~Drains 5 missiles per second during use");
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
			item.createTile = mod.TileType("VortexComboAddonTile");*/
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.missileSlotType = 0;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Topaz, 15);
            recipe.AddIngredient(ItemID.JungleSpores, 10);
            recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
