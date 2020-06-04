using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.balladdons
{
	public class VortexBombAddon : HunterDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Bomb");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Special\n" +
			"-Press the Power Bomb Key to set off a Vortex Bomb (20 second cooldown)\n" +
			"-Vortex Bombs create massive explosions which vacuum in foes and items\n" +
			"-Vortex Bombs can deal ~1400 damage total");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 25;
			item.noMelee = true;
			item.width = 32;
			item.height = 32;
			item.maxStack = 1;
			item.value = 30000;
			item.rare = 4;
			/*item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("VortexBombTile");*/
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.ballSlotType = 2;
			mItem.powerBombType = mod.ProjectileType("VortexBomb");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentVortex, 18);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}