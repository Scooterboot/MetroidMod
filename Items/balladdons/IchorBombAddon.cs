using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Items.damageclass;

namespace MetroidMod.Items.balladdons
{
	public class IchorBombAddon : HunterDamageItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Morph Ball Bombs");
			Tooltip.SetDefault("Morph Ball Addon\n" +
			"Slot Type: Weapon\n" +
			"-Right Click to set off a bomb");
			//"-Right Click to set off a bomb\n" +
			//"-Bombs deal 10 hunter damage");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 30;
			item.noMelee = true;
			item.width = 32;
			item.height = 32;
			item.maxStack = 1;
			item.value = 15000;
			item.rare = 4;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("IchorBombTile");
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>();
			mItem.ballSlotType = 1;
            mItem.bombType = 5;
		}
        
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MythrilBar, 5);
            recipe.AddIngredient(ItemID.Ichor, 5);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.OrichalcumBar, 5);
            recipe.AddIngredient(ItemID.Ichor, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
	}
}
