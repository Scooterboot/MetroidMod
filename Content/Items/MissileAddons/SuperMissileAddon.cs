using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.MissileAddons
{
	public class SuperMissileAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Missile");
			Tooltip.SetDefault(string.Format("[c/9696FF:Missile Launcher Addon]\n") +
			"Slot Type: Primary\n" +
			"Shots are more powerful and create a larger explosion\n" + 
			string.Format("[c/78BE78:+200% damage]\n") +
			string.Format("[c/BE7878:-50% speed]"));

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 2500;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Missile.SuperMissile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.missileSlotType = 1;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageSuperMissile;
			mItem.addonSpeed = Common.Configs.MConfigItems.Instance.speedSuperMissile;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddRecipeGroup(MetroidMod.T2HMBarRecipeGroupID, 8)
				.AddIngredient(ItemID.SoulofNight, 5)
				.AddIngredient<Tiles.MissileExpansion>(1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
