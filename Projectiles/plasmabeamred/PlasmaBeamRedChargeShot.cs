using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.plasmabeamred
{
	public class PlasmaBeamRedChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Beam Red Charge Shot");
			Main.projFrames[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
		}

		int dustType = 6;
		Color color = MetroidMod.plaRedColor;
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
			mProjectile.PlasmaDrawTrail(projectile, Main.player[projectile.owner], sb, 7, 0.65f);
			return false;
		}
	}
	
	public class SpazerPlasmaBeamRedChargeShot : PlasmaBeamRedChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamred/PlasmaBeamRedChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Spazer Plasma Beam Red Charge Shot";
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}
	}
	
	public class WavePlasmaBeamRedChargeShot : PlasmaBeamRedChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamred/PlasmaBeamRedChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wave Plasma Beam Red Charge Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	
	public class WaveSpazerPlasmaBeamRedChargeShot : WavePlasmaBeamRedChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamred/PlasmaBeamRedChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wave Spazer Plasma Beam Red Charge Shot";
			mProjectile.amplitude = 14.5f*projectile.scale;
		}
	}
	
	public class IcePlasmaBeamRedChargeShot : PlasmaBeamRedChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamred/IcePlasmaBeamRedChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Plasma Beam Red Charge Shot";
		}
	}
	
	public class IceSpazerPlasmaBeamRedChargeShot : SpazerPlasmaBeamRedChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamred/IcePlasmaBeamRedChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Spazer Plasma Beam Red Charge Shot";
		}
	}
	
	public class IceWavePlasmaBeamRedChargeShot : WavePlasmaBeamRedChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamred/IcePlasmaBeamRedChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Plasma Beam Red Charge Shot";
		}
	}
	
	public class IceWaveSpazerPlasmaBeamRedChargeShot : WaveSpazerPlasmaBeamRedChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamred/IcePlasmaBeamRedChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Spazer Plasma Beam Red Charge Shot";
		}
	}
}