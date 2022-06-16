using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Projectiles;

namespace MetroidMod.Content.Projectiles.missiles
{
	public class SeekerMissileLead : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seeker Missile Lead");
		}
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 8800;
			Projectile.ownerHitCheck = true;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			//Projectile.ranged = true;
		}

		Color LightColor = Color.Cyan;//MetroidMod.powColor;
		bool soundPlayed = false;
		SoundEffectInstance soundInstance;
		int dustDelay = 0;
		int negateUseTime = 0;
		public override void AI()
		{
			Projectile P = Projectile;
			Player O = Main.player[P.owner];

			Item I = O.inventory[O.selectedItem];

			MPlayer mp = O.GetModPlayer<MPlayer>();
			MGlobalItem mi = I.GetGlobalItem<MGlobalItem>();

			float MY = Main.mouseY + Main.screenPosition.Y;
			float MX = Main.mouseX + Main.screenPosition.X;
			if (O.gravDir == -1f)
			{
				MY = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
			}
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);

			P.scale = ((float)mi.seekerCharge / (float)MGlobalItem.seekerMaxCharge) * (0.25f + (0.75f * ((float)(mi.numSeekerTargets + 1) / 6f)));
			float targetrotation = (float)Math.Atan2((MY - oPos.Y), (MX - oPos.X));
			P.rotation += 0.5f * P.direction;
			O.itemTime = 2;
			O.itemAnimation = 2;

			int range = I.width + 4;
			int width = (I.width / 2) - (P.width / 2);
			int height = (I.height / 2) - (P.height / 2);

			if (negateUseTime < I.useTime)
			{
				negateUseTime++;
			}

			Vector2 iPos = O.itemLocation;

			P.friendly = false;
			P.damage = 0;
			P.position = new Vector2(iPos.X + (float)Math.Cos(targetrotation) * range + width, iPos.Y + (float)Math.Sin(targetrotation) * range + height);
			P.alpha = 0;
			if (P.velocity.X < 0)
			{
				P.direction = -1;
			}
			else
			{
				P.direction = 1;
			}
			P.spriteDirection = P.direction;
			O.direction = P.direction;

			O.heldProj = P.whoAmI;
			O.itemRotation = (float)Math.Atan2((MY - oPos.Y) * O.direction, (MX - oPos.X) * O.direction) - O.fullRotation;

			P.position -= P.velocity;
			P.timeLeft = 60;
			if (O.whoAmI == Main.myPlayer)
			{
				if (mi.seekerCharge == 10 && SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.ChargeStartup_Seeker, oPos), out ActiveSound result))
				{
					soundInstance = result.Sound;
				}
			}
			if (mi.seekerCharge >= MGlobalItem.seekerMaxCharge)
			{
				if (dustDelay <= mi.numSeekerTargets)
				{
					int dust = Dust.NewDust(P.position + P.velocity, P.width, P.height, 63, 0, 0, 100, Color.Cyan, 2.0f);
					Main.dust[dust].noGravity = true;
					dustDelay = 5;
				}
			}
			dustDelay = Math.Max(dustDelay - 1, 0);
			Lighting.AddLight(P.Center, (LightColor.R / 255f) * P.scale, (LightColor.G / 255f) * P.scale, (LightColor.B / 255f) * P.scale);
			if (O.controlUseItem && !mp.ballstate && !mp.shineActive && !O.dead && !O.noItems)
			{
				if (P.owner == Main.myPlayer)
				{
					P.velocity = targetrotation.ToRotationVector2() * O.inventory[O.selectedItem].shootSpeed;
				}
			}
			else
			{
				if (mi.seekerCharge >= MGlobalItem.seekerMaxCharge)
				{
					O.itemTime = I.useTime;
					O.itemAnimation = I.useAnimation;
				}
				else
				{
					O.itemTime = I.useTime - negateUseTime;
					O.itemAnimation = I.useAnimation - negateUseTime;
				}
				if (O.whoAmI == Main.myPlayer)
				{
					if (soundInstance != null)
					{
						soundInstance.Stop(true);
					}
					soundPlayed = false;
				}
				P.Kill();
			}
		}
		public override void Kill(int timeLeft)
		{
			Player O = Main.player[Projectile.owner];
			MGlobalItem mi = O.inventory[O.selectedItem].GetGlobalItem<MGlobalItem>();
			mi.seekerCharge = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
