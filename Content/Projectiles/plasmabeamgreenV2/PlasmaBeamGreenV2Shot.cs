using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.plasmabeamgreenV2
{
	public class PlasmaBeamGreenV2Shot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Beam Green V2 Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.penetrate = 6;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		int dustType = 61;
		Color color = MetroidMod.plaGreenColor;
		public override void AI()
		{
			if(Projectile.Name.Contains("Ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
			}
			else if(Projectile.Name.Contains("Wave"))
			{
				dustType = 15;
				color = MetroidMod.plaGreenColor2;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if(Main.projFrames[Projectile.type] > 1)
			{
				if(Projectile.numUpdates == 0)
				{
					Projectile.frame++;
				}
				if(Projectile.frame > 1)
				{
					Projectile.frame = 0;
				}
			}
			
			if(Projectile.Name.Contains("Wide") || Projectile.Name.Contains("Wave"))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
			
			if(Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, dustType);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
	
	public class WidePlasmaBeamGreenV2Shot : PlasmaBeamGreenV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Plasma Beam Green V2 Shot";
			Main.projFrames[Projectile.type] = 2;
			
			mProjectile.amplitude = 10f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 6;
		}
	}
	
	public class WavePlasmaBeamGreenV2Shot : PlasmaBeamGreenV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Beam Green V2 Shot";
			Main.projFrames[Projectile.type] = 1;
			Projectile.tileCollide = false;
			
			mProjectile.amplitude = 8f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 6;
		}
	}
	
	public class WaveWidePlasmaBeamGreenV2Shot : WavePlasmaBeamGreenV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Wide Plasma Beam Green V2 Shot";
			mProjectile.amplitude = 16f*Projectile.scale;
		}
	}
	
	public class IcePlasmaBeamGreenV2Shot : PlasmaBeamGreenV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Beam Green V2 Shot";
		}
	}
	
	public class IceWidePlasmaBeamGreenV2Shot : WidePlasmaBeamGreenV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Plasma Beam Green V2 Shot";
		}
	}
	
	public class IceWavePlasmaBeamGreenV2Shot : WavePlasmaBeamGreenV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Beam Green V2 Shot";
			mProjectile.delay = 3;
		}
	}
	
	public class IceWaveWidePlasmaBeamGreenV2Shot : WaveWidePlasmaBeamGreenV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Plasma Beam Green V2 Shot";
			mProjectile.delay = 3;
		}
	}
}
