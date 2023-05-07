#region Using directives

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

using MetroidMod.Common.Systems;
using MetroidMod.ID;

#endregion

namespace MetroidMod.Content.Items.Tiles.Destroyable
{
	public class FakeBlock : ModItem
	{
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fake Block");
			// Tooltip.SetDefault("Deactivates a tile when hit by anything. \nUse the Chozite Cutter to break.");

			Item.ResearchUnlockCount = 100;
		}
		public ushort placeType = BreakableTileID.Fake;
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.rare = ItemRarityID.Blue;
		}

		// Netsyncing ?
		public override bool? UseItem(Player player)
		{
			if (player.itemTime == 0 && player.itemAnimation > 0 && player.controlUseItem && player.whoAmI == Main.myPlayer)
			{
				Vector2 pos = new(Player.tileTargetX * 16, Player.tileTargetY * 16);
				if (MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] == BreakableTileID.None)
				{
					MSystem.mBlockType[Player.tileTargetX, Player.tileTargetY] = placeType;
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, pos);
					return true;
				}
			}
			return false;
		}

		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bomb);
			recipe.AddIngredient(ItemID.SandBlock);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
	public class FakeBlockHint : FakeBlock
	{
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fake Block (Transparent)");
			// Tooltip.SetDefault("Deactivates a tile when hit by anything. \nUse the Chozite Cutter to break.");

			Item.ResearchUnlockCount = 100;
		}
		public override bool? UseItem(Player player)
		{
			placeType = BreakableTileID.FakeHint;
			return base.UseItem(player);
		}
	}
	public class BombBlock : FakeBlock
	{
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bomb Block");
			// Tooltip.SetDefault("Deactivates a tile when hit by a bomb. \nUse the Chozite Cutter to break.");

			Item.ResearchUnlockCount = 100;
		}
		public override bool? UseItem(Player player)
		{
			placeType = BreakableTileID.Bomb;
			return base.UseItem(player);
		}
	}
	public class BombBlockChain : FakeBlock
	{
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bomb Block (Chain)");
			// Tooltip.SetDefault("Deactivates a tile when hit by a bomb. \nTriggers adjacent BombBlocks. \nUse the Chozite Cutter to break.");

			Item.ResearchUnlockCount = 100;
		}
		public override bool? UseItem(Player player)
		{
			placeType = BreakableTileID.BombChain;
			return base.UseItem(player);
		}
	}
	public class CrumbleBlock : FakeBlock
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crumble Block (Instant)");
			// Tooltip.SetDefault("Deactivates a tile when a player stands on it \nUse Chozite Cutters to break.");

			Item.ResearchUnlockCount = 100;
		}
		public override bool? UseItem(Player player)
		{
			placeType = BreakableTileID.CrumbleInstant;
			return base.UseItem(player);
		}
	}
	public class CrumbleBlockSpeed : FakeBlock
	{
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crumble Block (SpeedBoost)");
			// Tooltip.SetDefault("Deactivates a tile shortly after a player stands on it \nUse the Chozite Cutter to break.");

			Item.ResearchUnlockCount = 100;
		}
		public override bool? UseItem(Player player)
		{
			placeType = BreakableTileID.CrumbleSpeed;
			return base.UseItem(player);
		}
	}
	public class CrumbleBlockSlow : FakeBlock
	{
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crumble Block (Delayed)");
			// Tooltip.SetDefault("Deactivates a tile shortly after a player stands on it \nUse the Chozite Cutter to break.");

			Item.ResearchUnlockCount = 100;
		}
		public override bool? UseItem(Player player)
		{
			placeType = BreakableTileID.CrumbleSlow;
			return base.UseItem(player);
		}
	}
	public class MissileBlock : FakeBlock
	{
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Missile Block");
			// Tooltip.SetDefault("Deactivates a tile when hit by a missile. \nUse the Chozite Cutter to break");

			Item.ResearchUnlockCount = 100;
		}
		public override bool? UseItem(Player player)
		{
			placeType = BreakableTileID.Missile;
			return base.UseItem(player);
		}
	}
	public class SuperMissileBlock : FakeBlock
	{
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Super Missile Block");
			// Tooltip.SetDefault("Deactivates a tile when hit by a super missile. \nUse the Chozite Cutter to break.");

			Item.ResearchUnlockCount = 100;
		}
		public override bool? UseItem(Player player)
		{
			placeType = BreakableTileID.SuperMissile;
			return base.UseItem(player);
		}
	}
	public class PowerBombBlock : FakeBlock
	{
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Power Bomb Block");
			// Tooltip.SetDefault("Deactivates a tile when hit by a power bomb. \nUse the Chozite Cutter to break.");

			Item.ResearchUnlockCount = 100;
		}
		public override bool? UseItem(Player player)
		{
			placeType = BreakableTileID.PowerBomb;
			return base.UseItem(player);
		}
	}
	public class BoostBlock : FakeBlock
	{
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Boost Block");
			// Tooltip.SetDefault("Deactivates a tile when run into. \nUse the Chozite Cutter to break.");

			Item.ResearchUnlockCount = 100;
		}
		public override bool? UseItem(Player player)
		{
			placeType = BreakableTileID.Boost;
			return base.UseItem(player);
		}
	}
	public class ScrewAttackBlock : FakeBlock
	{
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Screw Attack Block");
			// Tooltip.SetDefault("Deactivates a tile when hit by a screw attack. \nUse the Chozite Cutter to break.");

			Item.ResearchUnlockCount = 100;
		}
		public override bool? UseItem(Player player)
		{
			placeType = BreakableTileID.ScrewAttack;
			return base.UseItem(player);
		}
	}
}
