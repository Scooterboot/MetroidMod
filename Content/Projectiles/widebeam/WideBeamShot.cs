using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.widebeam
{
	public class WideBeamShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wide Beam Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			
			mProjectile.amplitude = 10f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 63;
		Color color = MetroidMod.wideColor;
		Color color2 = MetroidMod.wideColor;
		public override void AI()
		{
			if(Projectile.Name.Contains("Ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
				color2 = default(Color);
			}
			else if(Projectile.Name.Contains("Wave"))
			{
				dustType = 62;
				color = MetroidMod.waveColor2;
				color2 = default(Color);
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if(Projectile.numUpdates == 0)
			{
				if(Projectile.Name.Contains("Wave"))
					Projectile.frame++;
				else
				{
					Projectile.frameCounter++;
					if(Projectile.frameCounter > 3)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
				}
			}
			if(Projectile.frame > 1)
				Projectile.frame = 0;
			
			mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			
			if(Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, color2, Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, dustType, true, 1f, color2);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile,Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
	
	public class WaveWideBeamShot : WideBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Beam Shot";
			Projectile.tileCollide = false;
			
			mProjectile.amplitude = 14f*Projectile.scale;
		}
	}
	
	public class IceWideBeamShot : WideBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Beam Shot";
		}
	}
	
	public class IceWaveWideBeamShot : WaveWideBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Beam Shot";
		}
	}
}
