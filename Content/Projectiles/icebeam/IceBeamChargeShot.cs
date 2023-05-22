using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.icebeam
{
	public class IceBeamChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ice Beam Charge Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 2f;
			if (Items.Weapons.PowerBeam.shooty.Contains("wave"))
			{
				mProjectile.amplitude = 10f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
				mProjectile.delay = 4;
			}
		}

		public override void AI()
		{
			Color color = MetroidMod.iceColor;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if(Projectile.numUpdates == 0)
			{
				Projectile.rotation += 0.5f*Projectile.direction;
			}
			if (Items.Weapons.PowerBeam.shooty.Contains("wave"))
			{
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile);
			}
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 59, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.Diffuse(Projectile, 59);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
