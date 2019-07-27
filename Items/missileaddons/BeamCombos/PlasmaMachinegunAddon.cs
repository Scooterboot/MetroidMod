using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.missileaddons.BeamCombos
{
	public class PlasmaMachinegunAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Machinegun");
			Tooltip.SetDefault(string.Format("[c/9696FF:Missile Launcher Addon]\n") +
			"Slot Type: Charge\n" +
			"Hold Click to charge\n" + 
			"~Rapidly fires piercing projectiles at full charge\n" + 
			"~Drains 1 missile per shot during use");
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
			item.createTile = mod.TileType("PlasmaLaserTile");*/
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.missileSlotType = 0;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Emerald, 5);
            recipe.AddIngredient(ItemID.CursedFlame, 15);
            recipe.AddIngredient(ItemID.AdamantiteBar, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Emerald, 5);
            recipe.AddIngredient(ItemID.CursedFlame, 15);
            recipe.AddIngredient(ItemID.TitaniumBar, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}
