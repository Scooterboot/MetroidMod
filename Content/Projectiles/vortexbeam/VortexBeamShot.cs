using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.vortexbeam
{
	public class VortexBeamShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vortex Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.scale = 2f;
			
			mProjectile.amplitude = 7f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 229;
		Color color = MetroidMod.lumColor;
		float scale = 1f;
		public override void AI()
		{
			string S = Items.Weapons.PowerBeam.shooty;
			if (S.Contains("stardust"))
			{
				dustType = 88;
				color = MetroidMod.iceColor;
				scale = 0.5f;
			}
			else if(S.Contains("nebula"))
			{
				dustType = 255;
				color = MetroidMod.waveColor;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			
			mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Nebula"));
			if(S.Contains("Nebula"))
			{
				Projectile.tileCollide = false;

				mProjectile.amplitude = 10f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
				mProjectile.HomingBehavior(Projectile);
			}
			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale*0.5f);
				Main.dust[dust].noGravity = true;
				if(S.Contains("stardust"))
				{
					dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 87, 0, 0, 100, default(Color), Projectile.scale);
					Main.dust[dust].noGravity = true;
				}
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
	
	public class NebulaVortexBeamShot : VortexBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Vortex Beam Shot";
		}
	}
	
	public class StardustVortexBeamShot : VortexBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Vortex Beam Shot";
		}
	}
}
