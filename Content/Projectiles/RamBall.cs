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
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.scale = .5f;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Projectile.penetrate = -1;
			Projectile.timeLeft = 9000;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 7;
		}
		public override void AI()
		{
			Player P = Main.player[Projectile.owner];
			MPlayer mp = P.GetModPlayer<MPlayer>();
			Projectile.position.X = P.Center.X - 5;
			Projectile.position.Y = P.position.Y - Projectile.gfxOffY;
			Projectile.velocity = P.velocity;
			Projectile.velocity.Normalize();
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.alpha = 100;
			//Projectile.knockBack = mp.boostEffect;


			if (!mp.ballstate || mp.boostEffect <= 20 || P.velocity == Vector2.Zero || P.dead)
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
			P.GiveImmuneTimeForCollisionAttack(mp.boostEffect);
			mp.boostEffect = 0;
			base.OnHitNPC(target, hit, damageDone);
		}
	}
}
