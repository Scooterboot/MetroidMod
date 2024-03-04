using System.IO;
using MetroidMod.Content.NPCs.OmegaPirate;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Boss
{
	public class OmegaPirateLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Omega Pirate");
		}
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1200;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 100;
		}

		public override void AI()
		{
			Color dustColor = Color.Lerp(OmegaPirate.minGlowColor, OmegaPirate.maxGlowColor, Projectile.localAI[0]);
			for (int i = 0; i < 3; i++)
			{
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 63, 0f, 0f, 100, dustColor, 5f);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].noLight = true;
				Main.dust[dust2].velocity *= 1.4f;
			}
		}

		public override bool PreDraw(ref Color lightColor) => false;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)Projectile.localAI[0]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.localAI[0] = (float)reader.ReadDouble();
		}
	}
}
