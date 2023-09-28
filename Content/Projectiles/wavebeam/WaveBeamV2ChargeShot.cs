using System;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.wavebeam
{
	public class WaveBeamV2ChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wave Beam V2 Charge Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 2f;
			Projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 62;
		Color color = MetroidMod.waveColor2;
		public override void AI()
		{
			
			string S  = PowerBeam.SetCondition();
			if (S.Contains("ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if(Projectile.numUpdates == 0)
			{
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
	
	public class IceWaveBeamV2ChargeShot : WaveBeamV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Beam V2 Charge Shot";
		}
	}
}
