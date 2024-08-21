using MetroidMod.Common.Players;
using MetroidMod.Content.SuitAddons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Accessories
{
	// legacy name because old suit addon system
	// [LegacyName("SpaceJumpBootsAddon")]
	public class SpinBoost : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SpaceJumpBoots>();
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.value = 40000;
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.ItemTile.SpinBoostTile>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<HiJumpBoots>(1)
				.AddIngredient(ItemID.CloudinaBottle, 1)
				.AddIngredient(SuitAddonLoader.GetAddon<EnergyTank>().ItemType, 1)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(1)
				.AddIngredient<HiJumpBoots>(1)
				.AddIngredient(ItemID.BlizzardinaBottle, 1)
				.AddIngredient(SuitAddonLoader.GetAddon<EnergyTank>().ItemType, 1)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(1)
				.AddIngredient<HiJumpBoots>(1)
				.AddIngredient(ItemID.SandstorminaBottle, 1)
				.AddIngredient(SuitAddonLoader.GetAddon<EnergyTank>().ItemType, 1)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe(1)
				.AddIngredient<HiJumpBoots>(1)
				.AddIngredient(ItemID.TsunamiInABottle, 1)
				.AddIngredient(SuitAddonLoader.GetAddon<EnergyTank>().ItemType, 1)
				.AddTile(TileID.Anvils)
				.Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.spaceJumpBoots = true;
			mp.itsSpinBoost = true;
			mp.hiJumpBoost = true;
		}
	}
}
