using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace MetroidModPorted.Content.Projectiles.solarbeam
{
	public class SolarBeamChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Beam Charge Shot");
			Main.projFrames[Type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.scale = 2f;
			Projectile.penetrate = 16;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		int dustType = 6;
		Color color = MetroidModPorted.novColor;
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
			
			int dType = Utils.SelectRandom<int>(Main.rand, new int[] { 6,158 });
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dType, 0, 0, 100, default(Color), Projectile.scale*2);
			Main.dust[dust].noGravity = true;
			if(Projectile.Name.Contains("Stardust"))
			{
				dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemTopaz, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
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
			mProjectile.Diffuse(Projectile, dustType);
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
	
	public class VortexSolarBeamChargeShot : SolarBeamChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/VortexSolarBeamChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Vortex Solar Beam Charge Shot";
			
			mProjectile.amplitude = 10f*Projectile.scale;
			mProjectile.wavesPerSecond = 1.5f;
			mProjectile.delay = 4;
		}
	}
	
	public class NebulaSolarBeamChargeShot : SolarBeamChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/SolarBeamChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Solar Beam Charge Shot";
			Projectile.tileCollide = false;
			
			mProjectile.amplitude = 12f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}
	}
	
	public class NebulaVortexSolarBeamChargeShot : NebulaSolarBeamChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/VortexSolarBeamChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Nebula Vortex Solar Beam Charge Shot";
			mProjectile.amplitude = 16f*Projectile.scale;
			mProjectile.wavesPerSecond = 1.5f;
		}
	}
	
	public class StardustSolarBeamChargeShot : SolarBeamChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/SolarBeamChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Solar Beam Charge Shot";
		}
	}
	
	public class StardustVortexSolarBeamChargeShot : VortexSolarBeamChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/VortexSolarBeamChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Vortex Solar Beam Charge Shot";
		}
	}
	
	public class StardustNebulaSolarBeamChargeShot : NebulaSolarBeamChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/SolarBeamChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Solar Beam Charge Shot";
		}
	}
	
	public class StardustNebulaVortexSolarBeamChargeShot : NebulaVortexSolarBeamChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/solarbeam/VortexSolarBeamChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Vortex Solar Beam Charge Shot";
		}
	}
}
