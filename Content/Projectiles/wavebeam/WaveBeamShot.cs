using System.IO;
using Microsoft.Xna.Framework;
using Terraria;

namespace MetroidMod.Content.Projectiles.wavebeam
{
	public class WaveBeamShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Wave Beam Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.tileCollide = false;

			mProjectile.amplitude = 8f * Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 3;
		}

		int dustType = 62;
		Color color = MetroidMod.waveColor;
		public override void AI()
		{
			if (shot.Contains("ice"))
			{
				dustType = 59;
				color = MetroidMod.iceColor;

				if (Projectile.numUpdates == 0)
				{
					Projectile.rotation += 0.5f * Projectile.direction;
				}
			}
			else
			{
				Projectile.rotation = 0;
			}
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			mProjectile.WaveBehavior(Projectile);

			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.penetrate);
			writer.Write(Projectile.maxPenetrate);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.penetrate = (int)reader.ReadInt32();
			Projectile.maxPenetrate = (int)reader.ReadInt32();
		}
		public override void OnKill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, dustType);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}
}
