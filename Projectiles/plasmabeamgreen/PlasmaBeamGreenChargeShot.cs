using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.plasmabeamgreen
{
	public class PlasmaBeamGreenChargeShot : MProjectile
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamgreen/PlasmaBeamGreenChargeShot";
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Beam Green Charge Shot");
			Main.projFrames[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 8;
			projectile.height = 8;
			projectile.scale = 2f;
			projectile.penetrate = 9;
			projectile.usesLocalNPCImmunity = true;
       	 	projectile.localNPCHitCooldown = 10;
		}

		int dustType = 61;
		Color color = MetroidMod.plaGreenColor;
		public override void AI()
		{
			if(projectile.Name.Contains("Ice"))
			{
				dustType = 59;
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
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			mProjectile.PlasmaDraw(projectile, Main.player[projectile.owner], sb);
			return false;
		}
	}
	
	public class SpazerPlasmaBeamGreenChargeShot : PlasmaBeamGreenChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamgreen/PlasmaBeamGreenChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Spazer Plasma Beam Green Charge Shot";
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 12;
		}
	}
	
	public class WavePlasmaBeamGreenChargeShot : PlasmaBeamGreenChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamgreen/PlasmaBeamGreenChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wave Plasma Beam Green Charge Shot";
			projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 12;
		}
	}
	
	public class WaveSpazerPlasmaBeamGreenChargeShot : WavePlasmaBeamGreenChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamgreen/PlasmaBeamGreenChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wave Spazer Plasma Beam Green Charge Shot";
			mProjectile.amplitude = 14.5f*projectile.scale;
		}
	}
	
	public class IcePlasmaBeamGreenChargeShot : PlasmaBeamGreenChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Plasma Beam Green Charge Shot";
		}
	}
	
	public class IceSpazerPlasmaBeamGreenChargeShot : SpazerPlasmaBeamGreenChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Spazer Plasma Beam Green Charge Shot";
		}
	}
	
	public class IceWavePlasmaBeamGreenChargeShot : WavePlasmaBeamGreenChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Plasma Beam Green Charge Shot";
		}
	}
	
	public class IceWaveSpazerPlasmaBeamGreenChargeShot : WaveSpazerPlasmaBeamGreenChargeShot
	{
		public override string Texture
		{
			get
			{
				return mod.Name + "/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenChargeShot";
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Spazer Plasma Beam Green Charge Shot";
		}
	}
}