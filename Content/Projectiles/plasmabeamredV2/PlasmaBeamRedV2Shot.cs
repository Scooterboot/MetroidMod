using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.plasmabeamredV2
{
	public class PlasmaBeamRedV2Shot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Plasma Beam Red V2 Shot";
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Main.projFrames[Projectile.type] = 2;

			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 6;
		Color color = MetroidMod.plaRedColor;
		public override void AI()
		{
			string S = Items.Weapons.PowerBeam.shooty;
			if (S.Contains("ice"))
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

			if (S.Contains("wave"))
			{
				Projectile.tileCollide = false;
			}
			if (S.Contains("wide") || (S.Contains("wave")))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
			if (S.Contains("wide") && !S.Contains("wave"))
			{
				mProjectile.amplitude = 10f * Projectile.scale;
			}
			if (S.Contains("wave") && !S.Contains("wide"))
			{
				mProjectile.amplitude = 8f * Projectile.scale;
			}
			if (S.Contains("wave") && S.Contains("wide"))
			{
				mProjectile.amplitude = 16f * Projectile.scale;
			}

			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, dustType);
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
				mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, 4);
			}
			return false;
		}
	}
	
	public class WidePlasmaBeamRedV2Shot : PlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Plasma Beam Red V2 Shot";
		}
	}
	
	public class WavePlasmaBeamRedV2Shot : PlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Plasma Beam Red V2 Shot";
			Projectile.tileCollide = false;
		}
	}
	
	public class IcePlasmaBeamRedV2Shot : PlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Beam Red V2 Shot";
		}
	}
	
	public class IceWidePlasmaBeamRedV2Shot : WidePlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Plasma Beam Red V2 Shot";
		}
	}
	
	public class IceWavePlasmaBeamRedV2Shot : WavePlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Plasma Beam Red V2 Shot";
		}
	}
	
	public class IceWaveWidePlasmaBeamRedV2Shot : WavePlasmaBeamRedV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Plasma Beam Red V2 Shot";
			Main.projFrames[Projectile.type] = 1;
		}
	}
}
