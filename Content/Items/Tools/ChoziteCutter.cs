#region Using directives

using MetroidMod.Common.Systems;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

#endregion

namespace MetroidMod.Content.Items.Tools
{
	public class ChoziteCutter : ModItem
	{

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Chozite Cutter");
			// Tooltip.SetDefault("Removes weapon-destructable blocks. \nDoes not break wires.");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 1;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.rare = 1;
			Item.tileBoost = 20;
		}

		// Netsyncing ?
		public override bool? UseItem(Player player)
		{
			if (MUtils.CanReachWiring(player, Item))
			{
				return RemoveBlockAt(player, Player.tileTargetX, Player.tileTargetY);
			}

			return false;
		}

		public static bool RemoveBlockAt(Player player, int i, int j)
		{
			if (MSystem.mBlockType[i, j] == BreakableTileID.None)
			{
				return false;
			}

			Vector2 itemPosition = player.Center;
			IEntitySource source = new EntitySource_Parent(player);

			if (MSystem.mBlockType[i, j] == BreakableTileID.CrumbleInstant)
			{
				Item.NewItem(source, itemPosition, ModContent.ItemType<Tiles.Destroyable.CrumbleBlock>());
			}
			if (MSystem.mBlockType[i, j] == BreakableTileID.CrumbleSpeed)
			{
				Item.NewItem(source, itemPosition, ModContent.ItemType<Tiles.Destroyable.CrumbleBlockSpeed>());
			}
			if (MSystem.mBlockType[i, j] == BreakableTileID.Bomb)
			{
				Item.NewItem(source, itemPosition, ModContent.ItemType<Tiles.Destroyable.BombBlock>());
			}
			if (MSystem.mBlockType[i, j] == BreakableTileID.Missile)
			{
				Item.NewItem(source, itemPosition, ModContent.ItemType<Tiles.Destroyable.MissileBlock>());
			}
			if (MSystem.mBlockType[i, j] == BreakableTileID.Fake)
			{
				Item.NewItem(source, itemPosition, ModContent.ItemType<Tiles.Destroyable.FakeBlock>());
			}
			if (MSystem.mBlockType[i, j] == BreakableTileID.Boost)
			{
				Item.NewItem(source, itemPosition, ModContent.ItemType<Tiles.Destroyable.BoostBlock>());
			}
			if (MSystem.mBlockType[i, j] == BreakableTileID.PowerBomb)
			{
				Item.NewItem(source, itemPosition, ModContent.ItemType<Tiles.Destroyable.PowerBombBlock>());
			}
			if (MSystem.mBlockType[i, j] == BreakableTileID.SuperMissile)
			{
				Item.NewItem(source, itemPosition, ModContent.ItemType<Tiles.Destroyable.SuperMissileBlock>());
			}
			if (MSystem.mBlockType[i, j] == BreakableTileID.ScrewAttack)
			{
				Item.NewItem(source, itemPosition, ModContent.ItemType<Tiles.Destroyable.ScrewAttackBlock>());
			}
			if (MSystem.mBlockType[i, j] == BreakableTileID.FakeHint)
			{
				Item.NewItem(source, itemPosition, ModContent.ItemType<Tiles.Destroyable.FakeBlockHint>());
			}
			if (MSystem.mBlockType[i, j] == BreakableTileID.CrumbleSlow)
			{
				Item.NewItem(source, itemPosition, ModContent.ItemType<Tiles.Destroyable.CrumbleBlockSlow>());
			}
			if (MSystem.mBlockType[i, j] == BreakableTileID.BombChain)
			{
				Item.NewItem(source, itemPosition, ModContent.ItemType<Tiles.Destroyable.BombBlockChain>());
			}

			MSystem.mBlockType[i, j] = BreakableTileID.None;
			MSystem.dontRegen[i, j] = false;
			MSystem.hit[i, j] = false;
			SoundEngine.PlaySound(SoundID.Dig, Main.MouseWorld);
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.WireCutter)
				.AddIngredient<Miscellaneous.ChoziteBar>(5)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
