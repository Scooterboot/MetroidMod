using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidModPorted.Content.Projectiles.plasmabeamgreen
{
	public class PlasmaBeamGreenShot : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/PlasmaBeamGreenShot";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Beam Green Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.scale = 2f;
			Projectile.penetrate = 6;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		int dustType = 61;
		Color color = MetroidModPorted.plaGreenColor;
		public override void AI()
		{
			if(Projectile.Name.Contains("Ice"))
			{
				dustType = 59;
				color = MetroidModPorted.iceColor;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			if(Projectile.Name.Contains("Spazer") || Projectile.Name.Contains("Wave"))
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
	
	public class SpazerPlasmaBeamGreenShot : PlasmaBeamGreenShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/PlasmaBeamGreenShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Spazer Plasma Beam Green Shot";
			
			mProjectile.amplitude = 7.5f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 6;
		}
	}
	
	public class WavePlasmaBeamGreenShot : PlasmaBeamGreenShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/PlasmaBeamGreenShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Beam Green Shot";
			Projectile.tileCollide = false;
			
			mProjectile.amplitude = 8f*Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 6;
		}
	}
	
	public class WaveSpazerPlasmaBeamGreenShot : WavePlasmaBeamGreenShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/PlasmaBeamGreenShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Plasma Beam Green Shot";
			mProjectile.amplitude = 12f*Projectile.scale;
		}
	}
	
	public class IcePlasmaBeamGreenShot : PlasmaBeamGreenShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Beam Green Shot";
		}
	}
	
	public class IceSpazerPlasmaBeamGreenShot : SpazerPlasmaBeamGreenShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Plasma Beam Green Shot";
		}
	}
	
	public class IceWavePlasmaBeamGreenShot : WavePlasmaBeamGreenShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Beam Green Shot";
		}
	}
	
	public class IceWaveSpazerPlasmaBeamGreenShot : WaveSpazerPlasmaBeamGreenShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/plasmabeamgreen/IcePlasmaBeamGreenShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Spazer Plasma Beam Green Shot";
		}
	}
}
