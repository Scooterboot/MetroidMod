using Terraria.ID;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;

namespace MetroidMod.Content.Items.MissileAddons
{
	public class IceMissileAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Missile");
			Tooltip.SetDefault(string.Format("[c/9696FF:Missile Launcher Addon]\n") +
			"Slot Type: Primary\n" +
			"Shots freeze enemies instantly\n" + 
			string.Format("[c/78BE78:+50% damage]"));

			SacrificeTotal = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 14;
			Item.maxStack = 1;
			Item.value = 30000;
			Item.rare = ItemRarityID.LightRed;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.Missile.IceMissile>();
			MGlobalItem mItem = Item.GetGlobalItem<MGlobalItem>();
			mItem.missileSlotType = 1;
			mItem.addonDmg = Common.Configs.MConfigItems.Instance.damageIceMissile;
			mItem.addonSpeed = Common.Configs.MConfigItems.Instance.speedIceMissile;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient<Miscellaneous.ChoziteBar>(8)
				.AddIngredient(ItemID.IceBlock, 25)
				.AddIngredient(ItemID.Bone, 10)
				.AddIngredient(ItemID.Sapphire)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
