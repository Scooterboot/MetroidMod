using System;
using MetroidMod.Content.DamageClasses;
using MetroidMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.OmegaCannon
{
	public class OmegaCannonShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Omega Cannon Shot");
			Main.projFrames[Projectile.type] = 2;

		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.scale = 1f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
		}
		//public override bool OnTileCollide(Vector2 oldVelocity)
		//{
		//	if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
		//	{
		//		Projectile.velocity.X = -oldVelocity.X;
		//	}

		//	if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
		//	{
		//		Projectile.velocity.Y = -oldVelocity.Y;
		//	}
		//	Projectile.timeLeft -= 120;

		//	return false;
		//}
		public override void AI()
		{
			Projectile.rotation = 0;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			
			if (Projectile.ai[1] == 0)
			{
				Projectile.scale = 1.5f;
			}
			if (Projectile.numUpdates == 0)
			{
				Projectile.rotation += 0.5f * Projectile.direction;
				Projectile.frame++;

				if (Projectile.timeLeft < 32 * (Projectile.extraUpdates + 1))
				{
					Projectile.velocity *= 0.95f;
				}
				if (Projectile.timeLeft % 7 == 0)
				{
					Dust.NewDust(Projectile.position + Projectile.Size / 4, Projectile.width / 2, Projectile.height / 2, ModContent.DustType<OmegaCannonTrail>(), 0, 0, 255, Color.White, Projectile.scale);
				}
			}
			if (Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}
			int dustType = 64;
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
			mProjectile.DustLine(Projectile.Center, Projectile.velocity, Projectile.rotation, 5, 1, dustType, 2f);
		}
		public override void OnKill(int timeLeft)
		{
			Projectile.penetrate = -1;
			mProjectile.Explode(2368);
			int shootNum = 15;
			float baseSpeed = 15f;
			int damage = Projectile.damage / 2;
			float knockBack = Projectile.knockBack / 2;
			int lifeTime = 90;
			float scale = Projectile.scale / 1.5f;
			if (Projectile.ai[1] != 0)
			{
				shootNum = 8;
				lifeTime = 70;
				baseSpeed = 12f;
			}

			float shootSpread = 360f;
			float spread = shootSpread * 0.0174f;
			double startAngle = Main.rand.NextFloat() * 3.14f;
			double deltaAngle = spread / shootNum;
			for (int i = 0; i < shootNum; i++)
			{
				double offsetAngle = startAngle + deltaAngle * i;
				Vector2 vel = new Vector2(baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle));
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, vel, ModContent.ProjectileType<OmegaCannonFrag>(), damage, knockBack, Projectile.owner, lifeTime, scale);
			}
			Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<OmegaCannonTrail>(), Vector2.Zero, 255, Color.White, Projectile.scale + 1f);

		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Projectile.timeLeft >= 1)
				modifiers.ArmorPenetration += 50;
			base.ModifyHitNPC(target, ref modifiers);
		}
		public override bool? CanCutTiles()
		{
			if (Projectile.timeLeft <= 1)
			{
				return false;
			}
			return null;
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height) && Projectile.Hitbox.Intersects(target.Hitbox))
			{
				return null;
			}
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
