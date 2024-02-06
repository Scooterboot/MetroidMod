using MetroidMod.Common.GlobalItems;
using MetroidMod.ID;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.MissileAddons.BeamCombos
{
	public class IceSpreaderAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Spreader");
			/* Tooltip.SetDefault(string.Format("[c/9696FF:Missile Launcher Addon]\n") +
			"Slot Type: Charge\n" +
			"Hold Click to charge\n" + 
			"~Charge shots cover terrain in ice on impact, freezing enemies\n" + 
			"~Charge shots cost 10 missiles"); */

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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Missile.IceSpreader>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.missileSlotType = 0;
			mItem.addonChargeDmg = 1f;
			mItem.addonMissileCost = 10;
			mItem.missileChangeType = MissileChangeSlotID.IceSpreader;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.IceRod, 1)
				.AddRecipeGroup(MetroidMod.T3HMBarRecipeGroupID, 10)
				.AddIngredient(ItemID.Sapphire, 1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
