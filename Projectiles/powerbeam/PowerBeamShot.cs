using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.powerbeam
{
	public class PowerBeamShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Power Beam Shot";
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 1.5f;
		}

		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.powColor;
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if(projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64, 0, 0, 100, default(Color), projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(projectile, 64);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.DrawCentered(projectile, sb);
			return false;
		}
	}
}