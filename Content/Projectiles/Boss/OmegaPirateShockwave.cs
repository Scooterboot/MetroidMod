using System;
using System.IO;
using MetroidMod.Content.NPCs.OmegaPirate;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.Boss
{
	public class OmegaPirateShockwave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Omega Pirate");
			Main.projFrames[Projectile.type] = 2;
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
			Projectile.width = 30;
			Projectile.height = 0;
			Projectile.scale = 1f;
		}

		int lightningFrame = 0;

		float scaleY = 0f;

		float alpha = 0.25f;
		public override void AI()
		{
			Projectile.frame++;
			if (Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}

			lightningFrame = Main.rand.Next(8);

			if (Projectile.ai[1] == 0)
			{
				scaleY = Math.Min(scaleY + (Projectile.ai[0] / 4f), Projectile.ai[0]);
				if (scaleY >= Projectile.ai[0])
				{
					Projectile.ai[1] = 1;
				}
			}
			if (Projectile.ai[1] > 0)
			{
				scaleY = Math.Max(scaleY - ((Projectile.ai[0] + (Projectile.ai[1] / 10)) / 100), 0f);
				if (scaleY < 0.1f)
				{
					Projectile.Kill();
				}
			}

			Projectile.position.X += Projectile.width / 2f;
			Projectile.position.Y += Projectile.height / 2f;
			Projectile.width = 30;
			Projectile.height = (int)(500 * scaleY);
			Projectile.position.X -= Projectile.width / 2f;
			Projectile.position.Y -= Projectile.height / 2f;

			if (Projectile.ai[0] >= 1.5f)
			{
				Projectile.localAI[0] = -1;
			}

			if (Projectile.ai[1] > 0 && Projectile.ai[0] > 0.1f)
			{
				Projectile.ai[1]++;
				if (Projectile.ai[1] == 2)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int shock1 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X + (30f * Projectile.spriteDirection), Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<OmegaPirateShockwave>(), Projectile.damage, 8f);
						if (Projectile.localAI[0] > 0)
						{
							Main.projectile[shock1].ai[0] = Projectile.ai[0] + 0.45f;
						}
						else
						{
							Main.projectile[shock1].ai[0] = Projectile.ai[0] - 0.05f;
						}
						Main.projectile[shock1].localAI[0] = Projectile.localAI[0];
						Main.projectile[shock1].localAI[1] = Projectile.localAI[1];
						Main.projectile[shock1].spriteDirection = Projectile.spriteDirection;
						Main.projectile[shock1].netUpdate = true;
					}
				}
			}

			Color dustColor = Color.Lerp(OmegaPirate.minGlowColor, OmegaPirate.maxGlowColor, Projectile.localAI[1]);

			int dust1 = Dust.NewDust(new Vector2(Projectile.position.X - 4, Projectile.Center.Y - 4), Projectile.width + 4, 5, 63, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, dustColor, 4f);
			Main.dust[dust1].noGravity = true;
			Main.dust[dust1].noLight = true;

			for (int i = 0; i < 3; i++)
			{
				int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 63, 0f, 0f, 100, dustColor, (1f + i) * 2f);
				Main.dust[dust2].noGravity = true;
				Main.dust[dust2].noLight = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Color color = Color.Lerp(OmegaPirate.minGlowColor, OmegaPirate.maxGlowColor, Projectile.localAI[1]);
			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			int num108 = tex.Width / Main.projFrames[Projectile.type];
			int x4 = num108 * Projectile.frame;
			Main.spriteBatch.Draw(tex, new Vector2((float)((int)(Projectile.Center.X - Main.screenPosition.X)), (float)((int)(Projectile.Center.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(x4, 0, num108, tex.Height)), Projectile.GetAlpha(Color.White) * alpha, Projectile.rotation, new Vector2((float)num108 / 2f, (float)tex.Height), new Vector2(1f, scaleY) * Projectile.scale, effects, 0f);

			Texture2D tex2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/Boss/OmegaPirateShockwaveLightning").Value;
			int num2 = tex2.Width / 8;
			int x2 = num2 * lightningFrame;
			int height = (int)((float)Math.Min(tex2.Height * scaleY, tex2.Height));
			Main.spriteBatch.Draw(tex2, new Vector2((float)((int)(Projectile.Center.X - Main.screenPosition.X)), (float)((int)(Projectile.Center.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(x2, 0, num2, height)), color, Projectile.rotation, new Vector2((float)num2 / 2f, (float)height), 0.5f * Math.Max(scaleY, 1f) * Projectile.scale, effects, 0f);

			effects |= SpriteEffects.FlipVertically;
			Main.spriteBatch.Draw(tex, new Vector2((float)((int)(Projectile.Center.X - Main.screenPosition.X)), (float)((int)(Projectile.Center.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(x4, 0, num108, tex.Height)), Projectile.GetAlpha(Color.White) * alpha, Projectile.rotation, new Vector2((float)num108 / 2f, 0f), new Vector2(1f, scaleY) * Projectile.scale, effects, 0f);
			Main.spriteBatch.Draw(tex2, new Vector2((float)((int)(Projectile.Center.X - Main.screenPosition.X)), (float)((int)(Projectile.Center.Y - Main.screenPosition.Y))), new Rectangle?(new Rectangle(x2, 0, num2, height)), color, Projectile.rotation, new Vector2((float)num2 / 2f, 0f), 0.5f * Math.Max(scaleY, 1f) * Projectile.scale, effects, 0f);
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)Projectile.localAI[0]);
			writer.Write((double)Projectile.localAI[1]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.localAI[0] = (float)reader.ReadDouble();
			Projectile.localAI[1] = (float)reader.ReadDouble();
		}
	}
}
