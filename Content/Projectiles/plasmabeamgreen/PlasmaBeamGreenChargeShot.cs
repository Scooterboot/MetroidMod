using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.plasmabeamgreen
{
	public class PlasmaBeamGreenChargeShot : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/PlasmaBeamGreenChargeShot";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Beam Green Charge Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.penetrate = 9;
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
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if(Projectile.numUpdates == 0)
			{
				Projectile.frame++;
			}
			if(Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}
			
			if(Projectile.Name.Contains("Spazer") || Projectile.Name.Contains("Wave"))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
			
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.Diffuse(Projectile, dustType);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
	
	public class SpazerPlasmaBeamGreenChargeShot : PlasmaBeamGreenChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/PlasmaBeamGreenChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer Plasma Beam Green Charge Shot";
			
			mProjectile.amplitude = 10f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 12;
		}
	}
	
	public class WavePlasmaBeamGreenChargeShot : PlasmaBeamGreenChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/PlasmaBeamGreenChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Beam Green Charge Shot";
			Projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 12;
		}
	}
	
	public class WaveSpazerPlasmaBeamGreenChargeShot : WavePlasmaBeamGreenChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/PlasmaBeamGreenChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Plasma Beam Green Charge Shot";
			mProjectile.amplitude = 14.5f*Projectile.scale;
		}
	}
	
	public class IcePlasmaBeamGreenChargeShot : PlasmaBeamGreenChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Beam Green Charge Shot";
		}
	}
	
	public class IceSpazerPlasmaBeamGreenChargeShot : SpazerPlasmaBeamGreenChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Plasma Beam Green Charge Shot";
		}
	}
	
	public class IceWavePlasmaBeamGreenChargeShot : WavePlasmaBeamGreenChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Beam Green Charge Shot";
		}
	}
	
	public class IceWaveSpazerPlasmaBeamGreenChargeShot : WaveSpazerPlasmaBeamGreenChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer Plasma Beam Green Charge Shot";
		}
	}
}
