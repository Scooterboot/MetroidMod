using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

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
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if(Projectile.numUpdates == 0)
			{
                Projectile.rotation += 0.5f * Projectile.direction;
                Projectile.frame++;
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
		public override void Kill(int timeLeft)
		{
			Projectile.width *= 75;
			Projectile.height *= 75;
			Projectile.position.X = Projectile.position.X - (Projectile.width / 2);
			Projectile.position.Y = Projectile.position.Y - (Projectile.height / 2);
			foreach (NPC target in Main.npc)
			{
				if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height))
				{
					Projectile.Damage();
					Projectile.usesLocalNPCImmunity = true;
					Projectile.localNPCHitCooldown = 10;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
