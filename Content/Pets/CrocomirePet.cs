using System;
using Microsoft.Build.Construction;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Pets
{
	public class CrocomirePet : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 24;
			Main.projPet[Projectile.type] = true;

			// Projectile code is needed to customize the vanity pet display in the player select screen. Quick explanation:
			// * It uses fluent API syntax, just like Recipe
			// * You start with ProjectileID.Sets.SimpleLoop, specifying the start and end frames as well as the speed, and optionally if it should animate from the end after reaching the end, effectively "bouncing"
			// * To stop the animation if the player is not highlighted/is standing, as done by most grounded pets, add a .WhenNotSelected(0, 0) (you can customize it just like SimpleLoop)
			// * To set offset and direction, use .WithOffset(x, y) and .WithSpriteDirection(-1)
			// * To further customize the behavior and animation of the pet (as its AI does not run), you have access to a few vanilla presets in DelegateMethods.CharacterPreview to use via .WithCode(). You can also make your own, showcased in MinionBossPetProjectile
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 12)
				.WithOffset(-10f, -10f)
				.WithSpriteDirection(-1)
				.WithCode(DelegateMethods.CharacterPreview.Float);
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.BabyDino); // Copy the stats of the Zephyr Fish
			Projectile.height = 32;
			
			//AIType = ProjectileID.BerniePet; // Mimic as the Zephyr Fish during AI.
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			SpriteEffects effects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			Vector2 drawOrigin = new Vector2(tex.Width / 4 * 0.5f, (tex.Height / Main.projFrames[Projectile.type]) * 0.5f);
			Color color = lightColor * (1 - (Projectile.alpha / 255f));
			Rectangle rect = new Rectangle(0, Projectile.frame * tex.Height / Main.projFrames[Projectile.type], tex.Width / 4, tex.Height / Main.projFrames[Projectile.type]);

			if (Projectile.ai[1] > 0 && rect.Y >= 20 * tex.Height / Main.projFrames[Projectile.type])
			{
				Vector2 vibe = new Vector2(0, 0);
				if (Projectile.ai[1] > 70 && Projectile.ai[1] < 130) 
				{
					vibe.X = (float)Math.Sin(Projectile.ai[1] % 13) * 2;
				}

				rect.X = tex.Width / 4;
				Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + vibe, new Rectangle?(rect), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);

				Rectangle rect2 = rect;
				rect2.X = tex.Width / 2;
				float f = Math.Max((Projectile.ai[1] - 100) / 100f, 0);
				int off2 = (int)(f * rect.Height);
				rect2.Y += off2;
				rect2.Height = (int)((1 - f) * rect.Height);
				Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + vibe + new Vector2(0, off2 * 1.15f), new Rectangle?(rect2), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);

				Rectangle rect3 = rect;
				rect3.X = tex.Width * 3 / 4;
				float g = Math.Max((Projectile.ai[1] - 70) / 130f, 0);
				int off3 = (int)(g * rect.Height);
				rect3.Y += off3;
				rect3.Height = (int)((1 - g) * rect.Height);
				Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition + vibe + new Vector2(0, off3 * 1.15f), new Rectangle?(rect3), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);

			}
			else
			{
				Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle?(rect), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);

				Texture2D eyelidTex = ModContent.Request<Texture2D>($"{Texture}_Eyelids").Value;
				int frameX = 0;
				int frameY = 0;
				Vector2 off = new Vector2(0, (Projectile.frame >= 3 && Projectile.frame <= 5) ? -2 : 0);

				if (Projectile.localAI[0] < 12)
					frameX = Math.Min((int)Projectile.localAI[0], 5);
				else if (Projectile.localAI[0] < 18)
					frameX = (17 - (int)Projectile.localAI[0]);
				if (Projectile.frame == 11)
					frameY = 1;
				if (Projectile.frame > 11)
					frameY = 2;
				Rectangle eyeRect = new Rectangle(frameX * eyelidTex.Width / 6, frameY * eyelidTex.Height / 3, eyelidTex.Width / 6, eyelidTex.Height / 3);
				Main.EntitySpriteDraw(eyelidTex, Projectile.Center - Main.screenPosition + off, new Rectangle?(eyeRect), color, Projectile.rotation, drawOrigin, Projectile.scale, effects, 0);
			}
			return false;
		}

		public override bool PreAI() 
		{
			Player player = Main.player[Projectile.owner];

			if (Projectile.wet)
			{
				if (Projectile.ai[2] == 0)
					Projectile.ai[2] = 180;
			}
			else if (Projectile.velocity.Y == 0f)
			{
				Projectile.ai[2] = 0;
			}
			if (Projectile.ai[0] != 0)
			{
				if (player.velocity.Y == 0f && Projectile.alpha >= (Projectile.ai[1] > 0 ? 250 : 100) && !player.lavaWet)
				{
					Projectile.position.X = player.Center.X - (Projectile.width / 2);
					Projectile.position.Y = player.Center.Y - Projectile.height;
					Projectile.ai[0] = 0f;
					Projectile.ai[1] = 0;
				}
				else
				{
					Projectile.velocity.X = 0f;
					Projectile.velocity.Y = 0f;
					if (Projectile.alpha < 100)
					{
						int num66 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X, Projectile.velocity.Y, 0, default(Color), 1.2f);
						Dust expr_44AC_cp_0_cp_0 = Main.dust[num66];
						expr_44AC_cp_0_cp_0.velocity.X = expr_44AC_cp_0_cp_0.velocity.X + (Main.rand.NextFloat() - 0.5f);
						Dust expr_44D2_cp_0_cp_0 = Main.dust[num66];
						expr_44D2_cp_0_cp_0.velocity.Y = expr_44D2_cp_0_cp_0.velocity.Y + (Main.rand.NextFloat() + 0.5f) * -1f;
						if (Main.rand.Next(3) != 0)
						{
							Main.dust[num66].noGravity = true;
						}
					}
					Projectile.alpha += 10;
					if (Projectile.alpha > 255)
					{
						Projectile.alpha = 255;
					}
					Projectile.ai[0] = -1;
				}
			}
			else if (Projectile.ai[0] != 0 && Projectile.ai[0] != -1 && Projectile.ai[0] != -2)
			{

			}
			else
			{
				if (Projectile.alpha > 0)
				{
					int num147 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Projectile.velocity.X, Projectile.velocity.Y, 0, default(Color), 1.2f);
					Dust expr_9391_cp_0_cp_0 = Main.dust[num147];
					expr_9391_cp_0_cp_0.velocity.X = expr_9391_cp_0_cp_0.velocity.X + (Main.rand.NextFloat() - 0.5f);
					Dust expr_93B7_cp_0_cp_0 = Main.dust[num147];
					expr_93B7_cp_0_cp_0.velocity.Y = expr_93B7_cp_0_cp_0.velocity.Y + (Main.rand.NextFloat() + 0.5f) * -1f;
					if (Main.rand.Next(3) != 0)
					{
						Main.dust[num147].noGravity = true;
					}
					Projectile.alpha -= 10;
					if (Projectile.alpha < 0)
					{
						Projectile.alpha = 0;
					}
					Projectile.ai[1] = 0;
					if (!Projectile.wet)
						Projectile.ai[2] = 0;
				}
				if (Projectile.ai[1] == 0)
				{
					if (Projectile.velocity.Y != 0f)
					{
					}
					else if (Projectile.position.X - Projectile.oldPosition.X == 0f)
					{
						Projectile.spriteDirection = 1;
						if (Main.player[Projectile.owner].Center.X < Projectile.Center.X)
						{
							Projectile.spriteDirection = -1;
						}
						Projectile.frame = 0;
					}
					else
					{
						float num148 = Projectile.velocity.Length();
						Projectile.frameCounter += Math.Max((int)num148, 1);
						if (Projectile.frameCounter > 7)
						{
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}
						if (Projectile.frame > 10)
						{
							Projectile.frame = 3;
						}
					}
				}
				Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
				if (Projectile.velocity.Y > 10f)
				{
					Projectile.velocity.Y = 10f;
				}
			}
			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];
			Projectile.spriteDirection = Projectile.direction;
			// Keep the projectile from disappearing as long as the player isn't dead and has the pet buff.
			if (!player.dead && player.HasBuff(ModContent.BuffType<CrocomirePetBuff>())) {
				Projectile.timeLeft = 2;
			}
			Projectile.localAI[0]++;
			if (Projectile.localAI[0] > 275)
				Projectile.localAI[0] = 0;
			if (Projectile.ai[2] > 1 || Projectile.ai[1] > 0)
			{
				if (Projectile.lavaWet || Projectile.ai[1] > 0)
					Projectile.ai[1]++;
				if (Projectile.ai[1] > 180 || Projectile.Distance(player.Center) > 500)
					Projectile.ai[0] = 1;

				Projectile.ai[2]--;
				Projectile.localAI[1] = 0;
				Projectile.localAI[2] = 0;

				if (Projectile.ai[0] == 0)
				{
					if (Projectile.ai[1] == 70)
					{
						SoundEngine.PlaySound(new SoundStyle($"MetroidMod/Assets/Sounds/CrocomireDeath").WithPitchOffset(0.5f).WithVolumeScale(0.25f), Projectile.Center);
					}
					if (Projectile.ai[1] == 90 || Projectile.ai[1] == 120 || Projectile.ai[1] == 180)
					{
						SoundEngine.PlaySound(new SoundStyle($"MetroidMod/Assets/Sounds/CrocomireDeath").WithPitchOffset(0.2f).WithVolumeScale(0.25f), Projectile.Center);
					}
				}

				if (Projectile.frame < 18)
					Projectile.frame = 18;
				if (Projectile.ai[2] % 6 == 0 || (Projectile.ai[1] % 6 == 0 && Projectile.ai[2] <= 0))
				{
					Projectile.frame++;
					if (Projectile.frame > 23)
						Projectile.frame = 20;
				}
				if (Projectile.wet && Projectile.velocity.Y > -1.5f)
				{
					Projectile.velocity.Y -= 0.6f;
					if (Projectile.velocity.Y > 3f)
						Projectile.velocity.Y -= 0.6f;
				}
			}
			else
			{
				if (Projectile.velocity.X == 0)
				{
					Projectile.localAI[1]++;
					Projectile.localAI[2] = 0;
					if (Projectile.localAI[1] < 192)
					{
						int f = (int)(Projectile.localAI[1] / 12) % 4;
						Projectile.frame = (f == 3) ? 1 : f;
					}
					else
					{
						int t = (int)(Projectile.localAI[1] - 192) / 6;
						if (Projectile.wet && Projectile.localAI[1] % 12 == 0)
						{
							Dust.NewDust(Projectile.position, Projectile.width, Projectile.height / 2, DustID.BreatheBubble, 0, 0, 150);
						}
						switch (t)
						{
							case 0:
							case 1:
							case 2:
							case 3:
							case 4:
							case 5:
								Projectile.frame = 11 + t;
								break;
							case 6:
								Projectile.frame = 13;
								break;
							case 7:
								Projectile.frame = 14;
								break;
							case 8:
								Projectile.frame = 17;
								break;
							case 9:
								Projectile.frame = 11;
								break;
							default:
								Projectile.frame = 0;
								Projectile.localAI[1] = 0;
								break;
						}
					}
				}
				else
				{
					Projectile.localAI[1] = 0;
					/*
					Projectile.localAI[2]++;
					if ((int)Projectile.localAI[2] % 6 == 0)
						Projectile.frame++;
					if (Projectile.frame > 10)
					{
						Projectile.frame = 3;
					}
					*/
				}
			}
		}
	}
}
