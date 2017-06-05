using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.novabeamV2
{
	public class NovaBeamV2ChargeShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Nova Beam V2 Charge Shot";
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
			projectile.penetrate = 11;
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
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if(projectile.numUpdates == 0)
			{
				projectile.frame++;
			}
			if(projectile.frame > 1)
			{
				projectile.frame = 0;
			}
			
			if(projectile.name.Contains("Wide") || projectile.name.Contains("Wave"))
			{
				mProjectile.WaveBehavior(projectile, !projectile.name.Contains("Wave"));
			}
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, default(Color), projectile.scale);
			Main.dust[dust].noGravity = true;
			
			Vector2 velocity = projectile.position - projectile.oldPos[0];
			if(Vector2.Distance(projectile.position, projectile.position+velocity) < Vector2.Distance(projectile.position,projectile.position+projectile.velocity))
			{
				velocity = projectile.velocity;
			}
			projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
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
			float scale = 0.75f;
			if(projectile.name.Contains("Ice") && projectile.name.Contains("Wave"))
			{
				scale = 1f;
			}
			mProjectile.PlasmaDrawTrail(projectile, Main.player[projectile.owner], sb, 10, scale);
			return false;
		}
	}
	
	public class WideNovaBeamV2ChargeShot : NovaBeamV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wide Nova Beam V2 Charge Shot";
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 5;
		}
	}
	
	public class WaveNovaBeamV2ChargeShot : NovaBeamV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wave Nova Beam V2 Charge Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 12f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 5;
		}
	}
	
	public class WaveWideNovaBeamV2ChargeShot : WaveNovaBeamV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Wave Wide Nova Beam V2 Charge Shot";
			mProjectile.amplitude = 16f*projectile.scale;
		}
	}
	
	public class IceNovaBeamV2ChargeShot : NovaBeamV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Nova Beam V2 Charge Shot";
		}
	}
	
	public class IceWideNovaBeamV2ChargeShot : WideNovaBeamV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wide Nova Beam V2 Charge Shot";
		}
	}
	
	public class IceWaveNovaBeamV2ChargeShot : WaveNovaBeamV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wave Nova Beam V2 Charge Shot";
		}
	}
	
	public class IceWaveWideNovaBeamV2ChargeShot : WaveWideNovaBeamV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.name = "Ice Wave Wide Nova Beam V2 Charge Shot";
		}
	}
}