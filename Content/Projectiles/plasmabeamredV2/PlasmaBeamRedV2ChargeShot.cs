using System;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.plasmabeamredV2
{
	public class PlasmaBeamRedV2ChargeShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Beam Red V2 Charge Shot";
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 2f;
			Main.projFrames[Projectile.type] = 2;

			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 6;
		Color color = MetroidMod.plaRedColor;
		public override void AI()
		{
					 
			if (shot.Contains("ice"))
			{
				dustType = 135;
				color = MetroidMod.iceColor;
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

			if (shot.Contains("wave"))
			{
				Projectile.tileCollide = false;
			}
			if (shot.Contains("wide") || (shot.Contains("wave")))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
			if (shot.Contains("wide") && !shot.Contains("wave"))
			{
				mProjectile.amplitude = 14f * Projectile.scale;
			}
			if (shot.Contains("wave") && !shot.Contains("wide"))
			{
				mProjectile.amplitude = 12f * Projectile.scale;
			}
			if (shot.Contains("wave") && shot.Contains("wide"))
			{
				mProjectile.amplitude = 16f * Projectile.scale;
			}

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale);
			Main.dust[dust].noGravity = true;
		}

		public override void OnKill(int timeLeft)
		{
			mProjectile.Diffuse(Projectile, dustType);
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			if(Projectile.Name.Contains("Ice") && Projectile.Name.Contains("Wave") && Projectile.Name.Contains("Wide"))
			{
				mProjectile.PlasmaDraw(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			}
			else
			{
				mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, 7, 0.65f);
			}
			return false;
		}
	}
	
	public class WidePlasmaBeamRedV2ChargeShot : PlasmaBeamRedV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Plasma Beam Red V2 Charge Shot";
		}
	}
	
	public class WavePlasmaBeamRedV2ChargeShot : PlasmaBeamRedV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Beam Red V2 Charge Shot";
		}
	}
	
	public class IcePlasmaBeamRedV2ChargeShot : PlasmaBeamRedV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Beam Red V2 Charge Shot";
		}
	}
	
	public class IceWidePlasmaBeamRedV2ChargeShot : WidePlasmaBeamRedV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Plasma Beam Red V2 Charge Shot";
		}
	}
	
	public class IceWavePlasmaBeamRedV2ChargeShot : WavePlasmaBeamRedV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Beam Red V2 Charge Shot";
		}
	}
	
	public class IceWaveWidePlasmaBeamRedV2ChargeShot : WavePlasmaBeamRedV2ChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Plasma Beam Red V2 Charge Shot";
			Main.projFrames[Projectile.type] = 1;
		}
	}
}
