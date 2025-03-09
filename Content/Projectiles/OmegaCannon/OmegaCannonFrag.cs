using System;
using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.OmegaCannon
{
	public class OmegaCannonFrag : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = ModContent.GetInstance<HunterDamageClass>();
			
			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.scale = 1f;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 60;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;

			Main.projFrames[Type] = 2;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox = new Rectangle((int)(Projectile.Center.X - 40 * Projectile.scale), (int)(Projectile.Center.Y - 40 * Projectile.scale), 
				(int)(80 * Projectile.scale), (int)(80 * Projectile.scale));

		}

		//public override void ModifyDamageHitbox(ref Rectangle hitbox)
		//{
		//	if (Projectile.timeLeft > 5)
		//	{
		//		hitbox = new Rectangle((int)Projectile.Center.X - 14, (int)Projectile.Center.Y - 14, 28, 28);
		//	}
		//	else
		//	{
		//		int amount = 20;
		//		hitbox = new Rectangle((int)Projectile.position.X - amount, (int)Projectile.position.Y - amount, Projectile.width + amount, Projectile.height + amount);
		//	}
		//}
		//public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		//{
		//	if (Projectile.timeLeft > 5)
		//	{
		//		Projectile.timeLeft = 5;
		//	}
		//}
		//public override void OnKill(int timeLeft)
		//{
		//	if (Projectile.ai[0] > 10)
		//	{
		//		int freq = 20;
		//		for (int i = 0; i < freq; i++)
		//		{
		//			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, 0, 0, 100, default(Color), 2);
		//			Main.dust[dust].velocity = new Vector2((Main.rand.Next(freq) - (freq / 2)) * 0.125f, (Main.rand.Next(freq) - (freq / 2)) * 0.125f);
		//			Main.dust[dust].noGravity = true;
		//		}
		//		SoundStyle sound = new($"{MetroidMod.Instance.Name}/Assets/Sounds/BeamImpactSound");
		//		SoundEngine.PlaySound(sound, Projectile.Center);
		//	}
		//}


		public override void AI()
		{
			Projectile P = Projectile;
			Projectile.scale = Projectile.ai[1];
			if (Projectile.timeLeft > 5)
			{
				Projectile.timeLeft = 100;
			}

			Projectile.ai[0]--;
			if ((int)Projectile.ai[0] % 5 == 0)
			{
				Projectile.frame = (Projectile.frame + 1) % 2;
			}
			if (Projectile.ai[0] < 64)
			{
				Projectile.velocity *= 0.925f;
				Projectile.alpha += 4;
			}
			if (Projectile.ai[0] < 0)
			{
				Projectile.Kill();
			}

			Color color = MetroidMod.powColor;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			Vector2 velocity = Projectile.position - Projectile.oldPos[0];
			if (Vector2.Distance(Projectile.position, Projectile.position + velocity) < Vector2.Distance(Projectile.position, Projectile.position + Projectile.velocity))
			{
				velocity = Projectile.velocity;
			}
			Projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + MathHelper.PiOver2;
		}
	}
}
