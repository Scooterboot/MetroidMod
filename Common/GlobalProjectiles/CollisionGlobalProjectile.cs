using System;
using MetroidMod.Content.Hatches;
using MetroidMod.Content.Hatches.Variants;
using MetroidMod.Content.Switches;
using MetroidMod.Content.Switches.Variants;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Common.GlobalProjectiles
{
	internal class CollisionGlobalProjectile : GlobalProjectile
	{
		public override bool PreAI(Projectile projectile)
		{
			IterateProjectileHitTiles(projectile, (x, y) =>
			{
				ProjectileTileHitAt(projectile, x, y, false);
			});
			return true;
		}

		public override void OnKill(Projectile projectile, int timeLeft)
		{
			IterateProjectileHitTiles(projectile, (x, y) =>
			{
				ProjectileTileHitAt(projectile, x, y, true);
			});
		}

		public void ProjectileTileHitAt(Projectile projectile, int i, int j, bool isDeath)
		{
			bool isMissile = projectile.Name.Contains("Missile");
			bool isSuper = isMissile && (
				projectile.Name.Contains("Super") ||
				projectile.Name.Contains("Nebula") ||
				projectile.Name.Contains("Stardust"));
			bool isPowerBomb = projectile.Name.Contains("Bomb") &&
				projectile.Name.Contains("Explosion") && (
					projectile.Name.Contains("Power") ||
					projectile.Name.Contains("Solar") ||
					projectile.Name.Contains("Vortex"));

			if (IsBlueInteractingProjectile(projectile, isDeath))
			{
				OpenHatch<BlueHatch>(i, j);
				TriggerSwitch<BlueSwitch>(i, j);
			}

			if (isMissile)
			{
				OpenHatch<RedHatch>(i, j);
				TriggerSwitch<RedSwitch>(i, j);
			}

			if (isSuper)
			{
				OpenHatch<GreenHatch>(i, j);
				TriggerSwitch<GreenSwitch>(i, j);
			}

			if(isPowerBomb)
			{
				OpenHatch<YellowHatch>(i, j);
				TriggerSwitch<YellowSwitch>(i, j);
			}	
		}

		private void OpenHatch<T>(int i, int j) where T: ModHatch
		{
			if (TileUtils.TryGetTileEntityAs(i, j, out HatchTileEntity tileEntity))
			{
				bool isCorrectHatch = tileEntity.Hatch is T;
				if (tileEntity.Behavior.IsTurnedBlue && typeof(T) == typeof(BlueHatch))
				{
					isCorrectHatch = true;
				}

				if (isCorrectHatch)
				{
					tileEntity.Behavior.HitProjectile();
				}
			}
		}

		private void TriggerSwitch<T>(int i, int j) where T : ModBubbleSwitch
		{
			Tile tile = Main.tile[i, j];

			if (tile.HasTile && tile.TileType == ModContent.GetInstance<T>().Tile.Type)
			{
				ModContent.GetInstance<BubbleSwitchActivationSystem>().HitSwitchAt(i, j);
			}
		}


		/// <summary>
		/// Returns whether the given projectile is capable of interacting with
		/// blue wiring elements (such as hatches and switches)
		/// by hitting them with its hitbox.
		/// </summary>
		public static bool IsBlueInteractingProjectile(Projectile projectile, bool isDeath)
		{
			bool isFromEnemy = !projectile.friendly;
			bool isSummon = projectile.aiStyle == ProjAIStyleID.Pet || projectile.minion;
			bool isHarmless = projectile.damage <= 0;

			if (isFromEnemy || isSummon || isHarmless) return false;

			// Presumably, this check stays here to ensure projectiles
			// only interact when they actually impact or detonate, which
			// for bouncing projectiles won't happen during until isDeath
			bool isBouncing = projectile.tileCollide && !isDeath;

			if (isBouncing) return false;

			bool isCharge = projectile.Name.Contains("Charge Attack") ||
				projectile.Name.Contains("Screw Attack");

			if (isCharge) return false;

			return true;
		}


		private void IterateProjectileHitTiles(Projectile projectile, Action<int, int> tileAction)
		{
			Point tl = projectile.TopLeft.ToTileCoordinates();
			Point br = projectile.BottomRight.ToTileCoordinates();

			int x1 = Math.Clamp(tl.X, 0, Main.maxTilesX);
			int y1 = Math.Clamp(tl.Y, 0, Main.maxTilesY);
			int x2 = Math.Clamp(br.X + 1, 0, Main.maxTilesX);
			int y2 = Math.Clamp(br.Y + 1, 0, Main.maxTilesY);

			for (int y = y1; y < y2; y++)
			{
				for (int x = x1; x < x2; x++)
				{
					tileAction(x, y);
				}
			}
		}
	}
}
