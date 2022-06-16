using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.plasmabeamred
{
	public class PlasmaBeamRedChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Beam Red Charge Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 2f;
		}

		int dustType = 6;
		Color color = MetroidMod.plaRedColor;
		public override void AI()
		{
			if(Projectile.Name.Contains("Ice"))
			{
				dustType = 135;
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
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, 7, 0.65f);
			return false;
		}
	}
	
	public class SpazerPlasmaBeamRedChargeShot : PlasmaBeamRedChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamred/PlasmaBeamRedChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer Plasma Beam Red Charge Shot";
			
			mProjectile.amplitude = 10f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}
	}
	
	public class WavePlasmaBeamRedChargeShot : PlasmaBeamRedChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamred/PlasmaBeamRedChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Beam Red Charge Shot";
			Projectile.tileCollide = false;
			
			mProjectile.amplitude = 10f*Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 4;
		}
	}
	
	public class WaveSpazerPlasmaBeamRedChargeShot : WavePlasmaBeamRedChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamred/PlasmaBeamRedChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Plasma Beam Red Charge Shot";
			mProjectile.amplitude = 14.5f*Projectile.scale;
		}
	}
	
	public class IcePlasmaBeamRedChargeShot : PlasmaBeamRedChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamred/IcePlasmaBeamRedChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Beam Red Charge Shot";
		}
	}
	
	public class IceSpazerPlasmaBeamRedChargeShot : SpazerPlasmaBeamRedChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamred/IcePlasmaBeamRedChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Plasma Beam Red Charge Shot";
		}
	}
	
	public class IceWavePlasmaBeamRedChargeShot : WavePlasmaBeamRedChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamred/IcePlasmaBeamRedChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Beam Red Charge Shot";
		}
	}
	
	public class IceWaveSpazerPlasmaBeamRedChargeShot : WaveSpazerPlasmaBeamRedChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamred/IcePlasmaBeamRedChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer Plasma Beam Red Charge Shot";
		}
	}
}
