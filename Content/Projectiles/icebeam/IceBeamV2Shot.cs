using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.icebeam
{
	public class IceBeamV2Shot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Beam V2 Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 13;
			Projectile.height = 13;
			Projectile.scale = 1.5f;
		}

		public override void AI()
		{
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Color color = MetroidMod.iceColor;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if(Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 59, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
				
				Projectile.frame++;
			}
			if(Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}
		}
		public override void OnKill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, 59);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
}
