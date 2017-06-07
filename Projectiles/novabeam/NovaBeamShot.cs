using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.novabeam
{
	public class NovaBeamShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Beam Shot");
			Main.projFrames[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 2f;
			projectile.penetrate = 8;
			projectile.usesLocalNPCImmunity = true;
       	 	projectile.localNPCHitCooldown = 10;
		}

		int dustType = 75;
		Color color = MetroidMod.novColor;
		public override void AI()
		{
			if(projectile.Name.Contains("Ice"))
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
			
			if(projectile.Name.Contains("Spazer") || projectile.Name.Contains("Wave"))
			{
				mProjectile.WaveBehavior(projectile, !projectile.Name.Contains("Wave"));
			}
			
			if(projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, default(Color), projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(projectile, dustType);
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.PlasmaDrawTrail(projectile, Main.player[projectile.owner], sb);
			return false;
		}
	}
	
	public class SpazerNovaBeamShot : NovaBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Spazer Nova Beam Shot";
			
			mProjectile.amplitude = 7.5f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}
	}
	
	public class WaveNovaBeamShot : NovaBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wave Nova Beam Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 8f*projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	
	public class WaveSpazerNovaBeamShot : WaveNovaBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wave Spazer Nova Beam Shot";
			mProjectile.amplitude = 12f*projectile.scale;
		}
	}
	
	public class IceNovaBeamShot : NovaBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Nova Beam Shot";
		}
	}
	
	public class IceSpazerNovaBeamShot : SpazerNovaBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Spazer Nova Beam Shot";
		}
	}
	
	public class IceWaveNovaBeamShot : WaveNovaBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Nova Beam Shot";
		}
	}
	
	public class IceWaveSpazerNovaBeamShot : WaveSpazerNovaBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Spazer Nova Beam Shot";
		}
	}
}