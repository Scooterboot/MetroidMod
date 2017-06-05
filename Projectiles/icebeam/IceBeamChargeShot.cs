using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.icebeam
{
	public class IceBeamChargeShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Beam Charge Shot";
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
		}

		public override void AI()
		{
			Color color = MetroidMod.iceColor;
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if(projectile.numUpdates == 0)
			{
				projectile.rotation += 0.5f*projectile.direction;
			}
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 59, 0, 0, 100, default(Color), projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.Diffuse(projectile, 59);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.DrawCentered(projectile, sb);
			return false;
		}
	}
}