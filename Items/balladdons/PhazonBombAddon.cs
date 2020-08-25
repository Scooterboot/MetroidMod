using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.balladdons
{
	public class PhazonBombAddon : HunterDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phazon Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb");
			//"-Right Click to set off a bomb\n" +
			//"-Bombs deal 10 hunter damage");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 77;
			item.noMelee = true;
			item.width = 32;
			item.height = 32;
			item.maxStack = 1;
			item.value = 50000;
			item.rare = 8;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("PhazonBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.ballSlotType = 1;
            mItem.bombType = 9;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PhazonBar", 5);
            recipe.AddTile(null, "NovaWorkTableTile");
			recipe.SetResult(this);
			recipe.AddRecipe();
        }
	}
}
