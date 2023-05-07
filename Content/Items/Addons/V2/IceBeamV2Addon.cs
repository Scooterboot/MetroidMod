using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.Addons.V2
{
	public class IceBeamV2Addon : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Beam V2");
			/* Tooltip.SetDefault(string.Format("[c/FF9696:Power Beam Addon V2]\n") +
				"Slot Type: Secondary\n" +
				"Shots freeze enemies\n" + 
				"~Each time the enemy is shot, they will become 20% slower\n" + 
				"~After 5 shots the enemy will become completely frozen\n" + 
				string.Format("[c/78BE78:+225% damage]\n") +
				string.Format("[c/BE7878:+50% overheat use]\n") +
				string.Format("[c/BE7878:-30% speed]")); */

			Item.ResearchUnlockCount = 1;
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
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Beam.IceBeamV2Tile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.addonSlotType = 1;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageIceBeamV2;
			mItem.addonHeat = Common.Configs.MConfigItems.Instance.overheatIceBeamV2;
			mItem.addonSpeed = Common.Configs.MConfigItems.Instance.speedIceBeamV2;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<IceBeamAddon>(1)
				.AddIngredient(ItemID.SpectreBar, 8)
				.AddIngredient(ItemID.BeetleHusk, 1)
				.AddIngredient<Miscellaneous.FrozenCore>(1)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
