
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles
{
	public class ChozoJackhammerProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
			Main.projFrames[Type] = 3;
		}

		public override void SetDefaults() {
			Projectile.width = 38;
			Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = true;
			Projectile.aiStyle = -1;
			Projectile.hide = true;
		}
		public override void AI() {
			Player player = Main.player[Projectile.owner];
			Projectile.timeLeft = 60;
			if (Projectile.soundDelay <= 0) {
				SoundEngine.PlaySound(SoundID.Item22, Projectile.Center);
				Projectile.soundDelay = 20;
			}
			int delay = 0;
			if (delay == 0)
			{
				delay = 10;
				Projectile.frame++;
			}
			if (Projectile.frame >= 3)
			{
				delay++;
				Projectile.frame = 0;
			}

			Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);
			if (Main.myPlayer == Projectile.owner) {
				if (player.channel) {
					float holdoutDistance = player.HeldItem.shootSpeed * Projectile.scale;
					Vector2 holdoutOffset = holdoutDistance * Vector2.Normalize(Main.MouseWorld - playerCenter);
					if (holdoutOffset.X != Projectile.velocity.X || holdoutOffset.Y != Projectile.velocity.Y) {
						Projectile.netUpdate = true;
					}
					Projectile.velocity = holdoutOffset;
				}
				else {
					Projectile.Kill();
				}
			}

			if (Projectile.velocity.X > 0f) {
				player.ChangeDir(1);
			}
			else if (Projectile.velocity.X < 0f) {
				player.ChangeDir(-1);
			}

			Projectile.spriteDirection = Projectile.direction;
			player.ChangeDir(Projectile.direction);
			player.heldProj = Projectile.whoAmI;
			player.SetDummyItemTime(2);
			Projectile.Center = playerCenter;
			Projectile.rotation = Projectile.velocity.ToRotation() + (player.direction == -1 ? MathHelper.Pi : 0f);
			player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
			Projectile.velocity.X *= 1f + Main.rand.Next(-3, 4) * 0.01f;
			if (Main.rand.NextBool(10)) {
				Dust dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity * Main.rand.Next(6, 10) * 0.15f, Projectile.width, Projectile.height, 0, 0f, 0f, 80, Color.White, 1f);
				dust.position.X -= 4f;
				dust.noGravity = true;
				dust.velocity.X *= 0.5f;
				dust.velocity.Y = -Main.rand.Next(3, 8) * 0.1f;
			}
		}
	}
}
