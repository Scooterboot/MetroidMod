#region Using directives

using MetroidMod.Common.Systems;
using MetroidMod.Content.Tiles.ItemTile;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

#endregion

namespace MetroidMod.Content.Items.Tiles.Destroyable
{
	public class FakeBlock : ModItem
	{
		public virtual ushort PlaceType => BreakableTileID.Fake;

		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 100;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 0, 2, 25);
			Item.ammo = ModContent.ItemType<FakeBlock>();
		}

		// Netsyncing ?
		public override bool? UseItem(Player player)
		{
			if (player.itemTime == 0 && player.itemAnimation > 0 && player.controlUseItem && player.whoAmI == Main.myPlayer)
			{
				return Place(player, Player.tileTargetX, Player.tileTargetY, PlaceType);
			}

			return false;
		}

		public static bool ExistsAt(int i, int j)
		{
			return MSystem.mBlockType[i, j] != BreakableTileID.None;
		}

		public static bool ExistsAt(int i, int j, ushort placeType)
		{
			return MSystem.mBlockType[i, j] == placeType;
		}

		public static bool SetRegen(int i, int j, bool regen)
		{
			bool old = MSystem.dontRegen[i, j];
			MSystem.dontRegen[i, j] = !regen;
			return old != regen;
		}

		public static bool Regens(int i, int j)
		{
			return !MSystem.dontRegen[i, j];
		}

		public static bool Place(Player player, int i, int j, ushort placeType)
		{
			if (ExistsAt(i, j)) return false;

			Vector2 position = new Vector2(i, j).ToWorldCoordinates();
			MSystem.mBlockType[i, j] = placeType;
			SoundEngine.PlaySound(SoundID.Dig, position);
			return true;
		}
	}
	public class FakeBlockHint : FakeBlock
	{
		public override ushort PlaceType => BreakableTileID.FakeHint;

		public override void AddRecipes()
		{
			CreateRecipe(5)
				.AddIngredient<FakeBlock>(5)
				.AddCondition(Condition.InGraveyard)
				.Register();
		}
	}
	public class BombBlock : FakeBlock
	{
		public override ushort PlaceType => BreakableTileID.Bomb;

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<FakeBlock>(20)
				.AddTile(MBAddonLoader.GetAddon<MorphBallAddons.Bomb>().TileType)
				.Register();

		}
	}
	public class BombBlockChain : FakeBlock
	{
		public override ushort PlaceType => BreakableTileID.BombChain;

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<FakeBlock>(20)
				.AddTile(MBAddonLoader.GetAddon<MorphBallAddons.Bomb>().TileType)
				.Register();

		}
	}
	public class CrumbleBlock : FakeBlock
	{
		public override ushort PlaceType => BreakableTileID.CrumbleInstant;

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<FakeBlock>(20)
				.AddCondition(Condition.InGraveyard)
				.Register();

		}
	}
	public class CrumbleBlockSpeed : FakeBlock
	{
		public override ushort PlaceType => BreakableTileID.CrumbleSpeed;

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<FakeBlock>(20)
				.AddCondition(Condition.InGraveyard)
				.Register();

		}
	}
	public class CrumbleBlockSlow : FakeBlock
	{
		public override ushort PlaceType => BreakableTileID.CrumbleSlow;

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<FakeBlock>(20)
				.AddCondition(Condition.InGraveyard)
				.Register();

		}
	}
	public class MissileBlock : FakeBlock
	{
		public override ushort PlaceType => BreakableTileID.Missile;

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<FakeBlock>(20)
				.AddTile<MissileExpansionTile>()
				.Register();

		}
	}
	public class SuperMissileBlock : FakeBlock
	{
		public override ushort PlaceType => BreakableTileID.SuperMissile;

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<FakeBlock>(20)
				.AddTile<Content.Tiles.ItemTile.Missile.SuperMissile>()
				.Register();

		}
	}
	public class PowerBombBlock : FakeBlock
	{
		public override ushort PlaceType => BreakableTileID.PowerBomb;

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<FakeBlock>(20)
				.AddTile(MBAddonLoader.GetAddon<MorphBallAddons.PowerBomb>().TileType)
				.Register();

		}
	}
	public class BoostBlock : FakeBlock
	{
		public override ushort PlaceType => BreakableTileID.Boost;

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<FakeBlock>(20)
				.AddTile<Content.Tiles.ItemTile.SpeedBoosterTile>()
				.Register();

		}
	}
	public class ScrewAttackBlock : FakeBlock
	{
		public override ushort PlaceType => BreakableTileID.ScrewAttack;

		public override void AddRecipes()
		{
			CreateRecipe(20)
				.AddIngredient<FakeBlock>(20)
				.AddTile<Content.Tiles.ItemTile.ScrewAttackTile>()
				.Register();

		}
	}
}
