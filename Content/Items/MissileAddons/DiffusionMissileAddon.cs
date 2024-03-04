using MetroidMod.Common.GlobalItems;
using MetroidMod.ID;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.MissileAddons
{
	public class DiffusionMissileAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Diffusion Missile");
			/* Tooltip.SetDefault(string.Format("[c/9696FF:Missile Launcher Addon]\n") +
			"Slot Type: Charge\n" +
			"Hold Click to charge\n" + 
			"~Charge shots create 4 large diffusion flares on impact that spiral outward, dealing damage to enemies\n" + 
			"~Charge shots and flares deal 3x damage\n" +
			"~Costs 5 missiles"); */

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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Missile.DiffusionMissile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.missileSlotType = 0;
			mItem.addonChargeDmg = 3f;
			mItem.addonMissileCost = 5;
			mItem.missileChangeType = MissileChangeSlotID.Diffusion;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.ChlorophyteBar, 10)
				.AddIngredient(ItemID.Ruby, 1)
				.AddIngredient<Tiles.MissileExpansion>(1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
