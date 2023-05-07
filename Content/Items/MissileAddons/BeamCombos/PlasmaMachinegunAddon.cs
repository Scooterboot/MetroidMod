using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.MissileAddons.BeamCombos
{
	public class PlasmaMachinegunAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Machinegun");
			/* Tooltip.SetDefault(string.Format("[c/9696FF:Missile Launcher Addon]\n") +
			"Slot Type: Charge\n" +
			"Hold Click to charge\n" + 
			"~Rapidly fires piercing projectiles at full charge\n" + 
			"~Initially costs 10 missiles, then drains 5 missiles per second during use"); */

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 50000;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Missile.PlasmaMachinegun>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.missileSlotType = 0;
			mItem.addonChargeDmg = 1f;
			mItem.addonMissileCost = 10;
			mItem.addonMissileDrain = 5;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.HallowedBar, 10)
				.AddIngredient(ItemID.CursedFlame, 10)
				.AddIngredient(ItemID.Emerald, 1)
				.AddIngredient(ItemID.SoulofMight, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
