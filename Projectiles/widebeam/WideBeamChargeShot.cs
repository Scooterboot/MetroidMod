using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Projectiles.widebeam
{
	public class WideBeamChargeShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wide Beam Charge Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 16;
			projectile.height = 16;
			projectile.scale = 2f;
			
			mProjectile.amplitude = 14f*projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 63;
		Color color = MetroidMod.wideColor;
		Color color2 = MetroidMod.wideColor;
		public override void AI()
		{
			if(projectile.Name.Contains("Ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
				color2 = default(Color);
			}
			else if(projectile.Name.Contains("Wave"))
			{
				dustType = 62;
				color = MetroidMod.waveColor2;
				color2 = default(Color);
			}
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			Lighting.AddLight(projectile.Center, color.R/255f,color.G/255f,color.B/255f);
			if(Main.projFrames[projectile.type] > 1)
			{
				if(projectile.numUpdates == 0)
				{
					projectile.frame++;
				}
				if(projectile.frame > 1)
				{
					projectile.frame = 0;
				}
			}
			
			mProjectile.WaveBehavior(projectile, !projectile.Name.Contains("Wave"));
			
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, 0, 0, 100, color2, projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.Diffuse(projectile, dustType, color2);
		}
		
		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			if(projectile.Name.Contains("Wave"))
			{
				mProjectile.PlasmaDraw(projectile,Main.player[projectile.owner], sb);
			}
			else
			{
				mProjectile.PlasmaDrawTrail(projectile,Main.player[projectile.owner], sb, 5, 1f);
			}
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(this.dustType);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			this.dustType = reader.ReadInt32();
		}
	}
	
	public class WaveWideBeamChargeShot : WideBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Wave Wide Beam Charge Shot";
			projectile.tileCollide = false;
			Main.projFrames[projectile.type] = 2;
			
			mProjectile.amplitude = 18f*projectile.scale;
		}
	}
	
	public class IceWideBeamChargeShot : WideBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wide Beam Charge Shot";
		}
	}
	
	public class IceWaveWideBeamChargeShot : WaveWideBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.Name = "Ice Wave Wide Beam Charge Shot";
		}
	}
}