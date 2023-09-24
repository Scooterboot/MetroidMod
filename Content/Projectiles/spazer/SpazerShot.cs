using System;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.spazer
{
	public class SpazerShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Spazer Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.scale = 2f;
			
			mProjectile.amplitude = 7.5f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 64;
		Color color = MetroidMod.powColor;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			string S = PowerBeam.SetCondition(player);
			if (S.Contains("ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
			}
			else if(S.Contains("wave"))
			{
				dustType = 62;
				color = MetroidMod.waveColor;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			
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
			mProjectile.PlasmaDraw(Projectile,Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
	
	public class WaveSpazerShot : SpazerShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Spazer Shot";
			Projectile.tileCollide = false;

			mProjectile.amplitude = 12f*Projectile.scale;
			mProjectile.wavesPerSecond = 1f;
		}
	}
	
	public class IceSpazerShot : SpazerShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Spazer Shot";
			Player player = Main.player[Projectile.owner];
			string S = PowerBeam.SetCondition(player);
			if (S.Contains("wave"))
			{
				Projectile.tileCollide = false;
				Projectile.Name += "Wave";
				mProjectile.amplitude = 12f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
			}
		}
	}
}
