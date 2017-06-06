using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.novabeam
{
	public class NovaBeamChargeShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Nova Beam Charge Shot";
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
			projectile.penetrate = 11;
			projectile.usesLocalNPCImmunity = true;
       	 	projectile.localNPCHitCooldown = 10;
			Main.projFrames[projectile.type] = 2;
		}

		int dustType = 75;
		Color color = MetroidMod.novColor;
		public override void AI()
		{
			if(projectile.name.Contains("Ice"))
			{
				dustType = 135;
				color = MetroidMod.iceColor;
			}
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if(projectile.numUpdates == 0)
			{
				projectile.frame++;
			}
			if(projectile.frame > 1)
			{
				projectile.frame = 0;
			}
			
			if(projectile.name.Contains("Spazer") || projectile.name.Contains("Wave"))
			{
				mProjectile.WaveBehavior(projectile, !projectile.name.Contains("Wave"));
			}
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, default(Color), projectile.scale);
			Main.dust[dust].noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			mProjectile.Diffuse(projectile, dustType);
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.PlasmaDrawTrail(projectile, Main.player[projectile.owner], sb, 10, 0.75f);
			return false;
		}
	}
	
	public class SpazerNovaBeamChargeShot : NovaBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Spazer Nova Beam Charge Shot";
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 5;
		}
	}
	
	public class WaveNovaBeamChargeShot : NovaBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wave Nova Beam Charge Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 5;
		}
	}
	
	public class WaveSpazerNovaBeamChargeShot : WaveNovaBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wave Spazer Nova Beam Charge Shot";
			mProjectile.amplitude = 14.5f*projectile.scale;
		}
	}
	
	public class IceNovaBeamChargeShot : NovaBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Nova Beam Charge Shot";
		}
	}
	
	public class IceSpazerNovaBeamChargeShot : SpazerNovaBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Spazer Nova Beam Charge Shot";
		}
	}
	
	public class IceWaveNovaBeamChargeShot : WaveNovaBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wave Nova Beam Charge Shot";
		}
	}
	
	public class IceWaveSpazerNovaBeamChargeShot : WaveSpazerNovaBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wave Spazer Nova Beam Charge Shot";
		}
	}
}
