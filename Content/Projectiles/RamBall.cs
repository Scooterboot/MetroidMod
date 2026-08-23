using System;
using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles
{
	public class RamBall : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ram Ball");
		}
		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.scale = .5f;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Projectile.penetrate = -1;
			Projectile.timeLeft = 9000;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}
		public override void AI()
		{
			Player P = Main.player[Projectile.owner];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			//Projectile.position.X = P.Center.X - 5;
			//Projectile.position.Y = P.position.Y - Projectile.gfxOffY;
			Projectile.Center = P.Center;
			Projectile.velocity = P.velocity;
			Projectile.velocity.Normalize();
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.alpha = (int)(((255f * (mp.boostEffect / 60f)) - 255f) * -1f);
			Projectile.direction = P.direction;
			//Projectile.knockBack = mp.boostEffect;


			if (!mp.ballstate || (mp.boostEffect <= 0 && mp.SMoveEffect <= 0) || P.velocity == Vector2.Zero || P.dead)
			{
				Projectile.Kill();
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player P = Main.player[Projectile.owner];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			P.velocity -= P.velocity;
			P.GiveImmuneTimeForCollisionAttack(Math.Max( mp.boostEffect,20));
			mp.boostEffect = 0;
		}
	}
}
