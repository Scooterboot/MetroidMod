using System;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.wavebeam
{
	public class WaveBeamChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wave Beam Charge Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			
			string S  = PowerBeam.SetCondition();
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 2f;
			Projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
			if (S.Contains("ice"))
			{
				Main.projFrames[Projectile.type] = 1;
			}
		}

		int dustType = 62;
		Color color = MetroidMod.waveColor;
		public override void AI()
		{
			
			string S  = PowerBeam.SetCondition();
			if (S.Contains("ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
			}
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if(Projectile.numUpdates == 0)
			{
				Projectile.rotation += 0.5f*Projectile.direction;
				if(Main.projFrames[Projectile.type] > 1)
				{
					Projectile.frame++;
				}
			}
			if(Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}
			
			mProjectile.WaveBehavior(Projectile);
			
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.Diffuse(Projectile, dustType);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
