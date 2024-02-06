using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Boss
{
	public class NightmareLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nightmare");
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
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.scale = 1f;
		}

		Vector2 laserPos = Vector2.Zero;
		public override void AI()
		{
			NPC Head = Main.npc[(int)Projectile.ai[0]];
			NPC Arm = Main.npc[(int)Projectile.ai[1]];

			if (Head != null && Head.active && Arm != null && Arm.active)
			{
				laserPos = Arm.Center + new Vector2(17 * Head.direction, 15);
				if (Arm.ai[1] == 1)
				{
					laserPos = Arm.Center + new Vector2(17 * Head.direction, 16);
				}
				if (Arm.ai[1] == 2)
				{
					laserPos = Arm.Center + new Vector2(17 * Head.direction, 9);
				}
				if (Arm.type == ModContent.NPCType<NPCs.Nightmare.Nightmare_ArmFront>())
				{
					laserPos = Arm.Center + new Vector2(13 * Head.direction, 17);
					if (Arm.ai[1] == 2)
					{
						laserPos = Arm.Center + new Vector2(19 * Head.direction, 17);
					}
					if (Arm.ai[1] == 3)
					{
						laserPos = Arm.Center + new Vector2(25 * Head.direction, 19);
					}
				}
				Player player = Main.player[Head.target];
				Projectile.localAI[0]++;
				if (Projectile.localAI[0] < 24)
				{
					float targetRot = (float)Math.Atan2(player.Center.Y - Projectile.Center.Y, player.Center.X - Projectile.Center.X);
					Projectile.velocity = targetRot.ToRotationVector2() * 14;
					Projectile.Center = laserPos;
				}
				else if (Projectile.localAI[0] < 25)
				{
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
				}
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			}
			else
			{
				Projectile.Kill();
			}
		}

		bool drawFlag = false;
		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.localAI[0] <= 28)
			{
				Texture2D tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/Boss/NightmareLaserCharge").Value;
				int num108 = tex.Height / 7;
				int y4 = num108 * (int)(Projectile.localAI[0] / 4f);
				Main.spriteBatch.Draw(tex, laserPos - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), Projectile.GetAlpha(Color.White), 0f, new Vector2((float)tex.Width / 2f, (float)num108 / 2f), Projectile.scale, SpriteEffects.None, 0f);
			}
			if (Projectile.localAI[0] >= 24)
			{
				Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
				int num108 = tex.Height / Main.projFrames[Projectile.type];
				int y4 = num108 * Projectile.frame;

				float h = ((float)num108 * Projectile.scale);

				float dist = MathHelper.Clamp((Vector2.Distance(Projectile.Center, laserPos) + ((float)Projectile.height / 2f)) / h, 0f, 1f);
				int height = (int)((float)num108 * dist);
				if (dist >= 1f)
				{
					drawFlag = true;
				}
				if (drawFlag)
				{
					height = num108;
				}
				Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, height)), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2((float)tex.Width / 2f, (float)num108 / 3f), Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
