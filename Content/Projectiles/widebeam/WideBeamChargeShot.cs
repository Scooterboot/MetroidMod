using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.widebeam
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
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 2f;
			
			mProjectile.amplitude = 14f*Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 63;
		Color color = MetroidMod.wideColor;
		Color color2 = MetroidMod.wideColor;
		public override void AI()
		{
			if(Projectile.Name.Contains("Ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;
				color2 = default(Color);
			}
			else if(Projectile.Name.Contains("Wave"))
			{
				dustType = 62;
				color = MetroidMod.waveColor2;
				color2 = default(Color);
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
			
			mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, color2, Projectile.scale);
			Main.dust[dust].noGravity = true;
		}
		public override void Kill(int timeLeft)
		{
			mProjectile.Diffuse(Projectile, dustType, color2);
		}
		
		public override bool PreDraw(ref Color lightColor)
		{
			if(Projectile.Name.Contains("Wave"))
			{
				mProjectile.PlasmaDraw(Projectile,Main.player[Projectile.owner], Main.spriteBatch);
			}
			else
			{
				mProjectile.PlasmaDrawTrail(Projectile,Main.player[Projectile.owner], Main.spriteBatch, 5, 1f);
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
			Projectile.Name = "Wave Wide Beam Charge Shot";
			Projectile.tileCollide = false;
			Main.projFrames[Projectile.type] = 2;
			
			mProjectile.amplitude = 18f*Projectile.scale;
		}
	}
	
	public class IceWideBeamChargeShot : WideBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wide Beam Charge Shot";
		}
	}
	
	public class IceWaveWideBeamChargeShot : WaveWideBeamChargeShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Beam Charge Shot";
		}
	}
}
