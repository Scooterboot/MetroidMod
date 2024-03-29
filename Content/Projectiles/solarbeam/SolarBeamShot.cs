using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.solarbeam
{
	public class SolarBeamShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Beam Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.penetrate = 12;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		int dustType = 6;
		Color color = MetroidMod.plaRedColor;
		public override void AI()
		{
			if(Projectile.Name.Contains("Stardust"))
			{
				dustType = 87;
			}
			Lighting.AddLight(Projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if(Projectile.numUpdates == 0)
			{
				Projectile.frame++;
			}
			if(Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}
			
			if(Projectile.Name.Contains("Vortex") || Projectile.Name.Contains("Nebula"))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Nebula"));
			}
			if(Projectile.Name.Contains("Nebula"))
			{
				mProjectile.HomingBehavior(Projectile);
			}
			
			if(Projectile.numUpdates == 0 || !Projectile.Name.Contains("Stardust"))
			{
				int dType = Utils.SelectRandom<int>(Main.rand, new int[] { 6,158 });
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dType, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
				if(Projectile.Name.Contains("Stardust"))
				{
					dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 87, 0, 0, 100, default(Color), Projectile.scale);
					Main.dust[dust].noGravity = true;
				}
			}
			
			Vector2 velocity = Projectile.position - Projectile.oldPos[0];
			if(Vector2.Distance(Projectile.position, Projectile.position+velocity) < Vector2.Distance(Projectile.position,Projectile.position+Projectile.velocity))
			{
				velocity = Projectile.velocity;
			}
			Projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
		}

		public override void Kill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, dustType);
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 50);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}
	
	public class VortexSolarBeamShot : SolarBeamShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/VortexSolarBeamShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Vortex Solar Beam Shot";
			
			mProjectile.amplitude = 7.5f*Projectile.scale;
			mProjectile.wavesPerSecond = 1.5f;
			mProjectile.delay = 3;
		}
	}
	
	public class NebulaSolarBeamShot : SolarBeamShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/SolarBeamShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Solar Beam Shot";
			Projectile.tileCollide = false;
			
			mProjectile.amplitude = 8f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}
	}
	
	public class NebulaVortexSolarBeamShot : NebulaSolarBeamShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/VortexSolarBeamShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Vortex Solar Beam Shot";
			mProjectile.amplitude = 14f*Projectile.scale;
			mProjectile.wavesPerSecond = 1.5f;
		}
	}
	
	public class StardustSolarBeamShot : SolarBeamShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/SolarBeamShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Solar Beam Shot";
		}
	}
	
	public class StardustVortexSolarBeamShot : VortexSolarBeamShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/VortexSolarBeamShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Vortex Solar Beam Shot";
		}
	}
	
	public class StardustNebulaSolarBeamShot : NebulaSolarBeamShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/SolarBeamShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Solar Beam Shot";
		}
	}
	
	public class StardustNebulaVortexSolarBeamShot : NebulaVortexSolarBeamShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/VortexSolarBeamShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Vortex Solar Beam Shot";
		}
	}
}
