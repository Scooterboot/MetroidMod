using System;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.novabeamV2
{
	public class NovaBeamV2Shot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nova Beam V2 Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.penetrate = 8;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;

			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 75;
		Color color = MetroidMod.novColor;
		public override void AI()
		{
			
			string S  = PowerBeam.SetCondition();
			if (Projectile.Name.Contains("Ice"))
			{
				dustType = 135;
				color = MetroidMod.iceColor;
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
				mProjectile.amplitude = 7.5f * Projectile.scale;
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
			
			Vector2 velocity = Projectile.position - Projectile.oldPos[0];
			if(Vector2.Distance(Projectile.position, Projectile.position+velocity) < Vector2.Distance(Projectile.position,Projectile.position+Projectile.velocity))
			{
				velocity = Projectile.velocity;
			}
			Projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + 1.57f;
		}

		public override void OnKill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, dustType);
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			float scale = 0.5f;
			if(Projectile.Name.Contains("Ice") && Projectile.Name.Contains("Wave"))
			{
				scale = 1f;
			}
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, 10, scale);
			return false;
		}
	}
	
	public class WideNovaBeamV2Shot : NovaBeamV2Shot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/novabeamV2/WaveNovaBeamV2Shot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wide Nova Beam V2 Shot";
		}
	}
	
	public class WaveNovaBeamV2Shot : NovaBeamV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Nova Beam V2 Shot";
		}
	}
	
	public class IceNovaBeamV2Shot : NovaBeamV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Nova Beam V2 Shot";
		}
	}
	
	public class IceWideNovaBeamV2Shot : WideNovaBeamV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Nova Beam V2 Shot";
		}
	}
	
	public class IceWaveNovaBeamV2Shot : WaveNovaBeamV2Shot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Nova Beam V2 Shot";
		}
	}
}
