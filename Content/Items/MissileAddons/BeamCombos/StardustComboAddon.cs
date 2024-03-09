using MetroidMod.Common.GlobalItems;
using MetroidMod.ID;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.MissileAddons.BeamCombos
{
	public class StardustComboAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stardust Blizzard");
			/* Tooltip.SetDefault(string.Format("[c/9696FF:Missile Launcher Addon]\n") +
			"Slot Type: Charge\n" +
			"Hold Click to charge\n" + 
			"~Charge shots create 8 Stardust Dragons on impact that spiral outward, covering terrain in ice which freezes foes\n" + 
			"~Additionally a Stardust Guardian spawns at the center which attacks any enemy caught in the ice\n" + 
			"~Charge shots cost 10 missiles\n" +
			"Both the ice and the Guardian last 20 seconds"); */

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 70000;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Missile.StardustCombo>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.missileSlotType = 0;
			mItem.addonChargeDmg = 1f;
			mItem.addonMissileCost = 10;
			mItem.missileChangeType = MissileChangeSlotID.StardustCombo;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.FragmentStardust, 15)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
