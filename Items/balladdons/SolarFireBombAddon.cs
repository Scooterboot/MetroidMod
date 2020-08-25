using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.balladdons
{
	public class SolarFireBombAddon : HunterDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Fire Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb");
			//"-Right Click to set off a bomb\n" +
			//"-Bombs deal 10 hunter damage");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 150;
			item.noMelee = true;
			item.width = 32;
			item.height = 32;
			item.maxStack = 1;
			item.value = 50000;
			item.rare = 10;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("SolarFireBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.ballSlotType = 1;
            mItem.bombType = 12;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FragmentSolar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
