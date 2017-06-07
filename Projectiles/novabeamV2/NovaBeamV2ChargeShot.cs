using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.novabeamV2
{
	public class NovaBeamV2ChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Beam V2 Charge Shot");
			Main.projFrames[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
			projectile.penetrate = 11;
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
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if(projectile.numUpdates == 0)
			{
				projectile.frame++;
			}
			if(projectile.frame > 1)
			{
				projectile.frame = 0;
			}
			
			if(projectile.Name.Contains("Wide") || projectile.Name.Contains("Wave"))
			{
				mProjectile.WaveBehavior(projectile, !projectile.Name.Contains("Wave"));
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
			if(projectile.Name.Contains("Ice") && projectile.Name.Contains("Wave"))
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
			projectile.Name = "Wide Nova Beam V2 Charge Shot";
			
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
			projectile.Name = "Wave Nova Beam V2 Charge Shot";
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
			projectile.Name = "Wave Wide Nova Beam V2 Charge Shot";
			mProjectile.amplitude = 16f*projectile.scale;
		}
	}
	
	public class IceNovaBeamV2ChargeShot : NovaBeamV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Nova Beam V2 Charge Shot";
		}
	}
	
	public class IceWideNovaBeamV2ChargeShot : WideNovaBeamV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wide Nova Beam V2 Charge Shot";
		}
	}
	
	public class IceWaveNovaBeamV2ChargeShot : WaveNovaBeamV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Nova Beam V2 Charge Shot";
		}
	}
	
	public class IceWaveWideNovaBeamV2ChargeShot : WaveWideNovaBeamV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Wide Nova Beam V2 Charge Shot";
		}
	}
}