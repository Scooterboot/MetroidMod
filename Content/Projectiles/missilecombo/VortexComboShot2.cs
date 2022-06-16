using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.missilecombo
{
	public class VortexComboShot2 : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Combo Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.scale = 0.75f;
		}

		public override void AI()
		{
			Projectile P = Projectile;
			P.rotation = (float)Math.Atan2((double)P.velocity.Y, (double)P.velocity.X) + 1.57f;
			Color color = MetroidMod.lumColor;
			Lighting.AddLight(P.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if(P.numUpdates == 0)
			{
				int dust = Dust.NewDust(P.position, P.width, P.height, 229, 0, 0, 100, default(Color), 1f);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, 229, true, 2f);
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 200);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile,Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
}
