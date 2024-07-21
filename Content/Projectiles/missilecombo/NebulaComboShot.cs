using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.missilecombo
{
	public class NebulaComboShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Singularity Shot");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.scale = 0f;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 5;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8 * (1 + Projectile.extraUpdates);
		}

		bool initialize = false;

		Projectile Lead;

		Projectile[] buster = new Projectile[4];

		const float Max_Range = 300f;
		float range = Max_Range;
		const float Max_Distance = 300f;
		float distance = Max_Distance;
		float accuracy = 11f;
		Vector2 oPos;
		Vector2 mousePos;

		SoundEffectInstance soundInstance;

		public override void AI()
		{
			Projectile P = Projectile;
			Player O = Main.player[P.owner];

			oPos = O.RotatedRelativePoint(O.MountedCenter, true);

			Lead = Main.projectile[(int)P.ai[0]];
			if (!Lead.active || Lead.owner != P.owner || Lead.type != ModContent.ProjectileType<ChargeLead>())
			{
				P.Kill();
				return;
			}

			if (!initialize && P.owner == Main.myPlayer)
			{
				var entitySource = Projectile.GetSource_FromAI();
				for (int i = 0; i < buster.Length; i++)
				{
					int b = Projectile.NewProjectile(entitySource, P.Center.X, P.Center.Y, 0f, 0f, ModContent.ProjectileType<NebulaBusterShot>(), (int)((float)P.damage * 0.25f), P.knockBack, P.owner);
					buster[i] = Main.projectile[b];
					buster[i].ai[0] = P.whoAmI;
					buster[i].ai[1] = i;
					buster[i].netUpdate = true;
				}

				initialize = true;
				Projectile.netUpdate = true;
			}

			range = Max_Range;
			distance = Max_Distance;

			if (P.owner == Main.myPlayer)
			{
				P.netUpdate = true;

				Vector2 diff = Main.MouseWorld - oPos;
				diff.Normalize();
				if (float.IsNaN(diff.X) || float.IsNaN(diff.Y))
				{
					diff = -Vector2.UnitY;
				}

				Vector2 targetPos = oPos + O.velocity + diff * Math.Min(Vector2.Distance(oPos, Main.MouseWorld), range);

				float speed = Math.Max(2f, Vector2.Distance(targetPos, P.Center) * 0.025f) * (0.5f + 0.5f * P.scale);
				float num244 = targetPos.X - P.Center.X;
				float num245 = targetPos.Y - P.Center.Y;
				float num246 = (float)Math.Sqrt((double)(num244 * num244 + num245 * num245));
				num246 = speed / num246;
				num244 *= num246;
				num245 *= num246;
				Vector2 vel = new Vector2((P.velocity.X * accuracy + num244) / (accuracy + 1f), (P.velocity.Y * accuracy + num245) / (accuracy + 1f));
				if (float.IsNaN(vel.X) || float.IsNaN(vel.Y))
				{
					vel = -Vector2.UnitY;
				}
				P.velocity = vel;

				if (soundInstance == null || soundInstance.State != SoundState.Playing)
				{
					SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.NebulaComboSoundLoop, O.position), out ActiveSound result);
					SoundEngine.PlaySound(new($"{Mod.Name}/Assets/Sounds/NebulaComboSoundStart"), O.position);
					soundInstance = result.Sound;
					if (Main.soundVolume > 0f)
					{
						soundInstance.Volume = 0f;
					}
				}
				else if (P.numUpdates == 0 && Main.soundVolume > 0f)
				{
					soundInstance.Volume = Math.Min(soundInstance.Volume + 0.05f * Main.soundVolume, 1f * Main.soundVolume);
				}
			}

			if (O.controlUseItem)
			{
				P.timeLeft = 10;
			}
			else
			{
				P.Kill();
			}

			if (P.numUpdates == 0)
			{
				P.rotation -= 0.104719758f * 2f;
				P.scale = Math.Min(P.scale + 0.05f, 1f);
				P.alpha = Math.Max(P.alpha - 15, 0);
			}

			P.position.X += (float)P.width / 2f;
			P.position.Y += (float)P.height / 2f;
			P.width = (int)(100f * P.scale);
			P.height = (int)(100f * P.scale);
			P.position.X -= (float)P.width / 2f;
			P.position.Y -= (float)P.height / 2f;

			if (P.numUpdates == 0)
			{
				float dist = Vector2.Distance(Lead.Center, P.Center);
				Vector2 diff2 = Vector2.Normalize(P.Center - Lead.Center);
				if (float.IsNaN(diff2.X) || float.IsNaN(diff2.Y))
				{
					diff2 = -Vector2.UnitY;
				}
				Vector2 diff3 = Vector2.Normalize(Lead.velocity);
				if (float.IsNaN(diff3.X) || float.IsNaN(diff3.Y))
				{
					diff3 = -Vector2.UnitY;
				}

				for (float i = 0f; i < dist; i += 30f)
				{
					Vector2 pos1 = Lead.Center + diff3 * i;
					Vector2 pos2 = Lead.Center + diff2 * i;

					float scale = MathHelper.Lerp(0.1f, P.scale, (i / dist));

					int dWidth = (int)(100f * scale);
					int dHeight = (int)(100f * scale);

					Vector2 dustPos = Vector2.Lerp(pos1, pos2, i / dist) - new Vector2(dWidth, dHeight) / 2f;
					int num891 = Dust.NewDust(dustPos, dWidth, dHeight, 255, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num891].noGravity = true;
				}

				P.frame++;
				if (P.frame > 1)
				{
					P.frame = 0;
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Lead != null && Lead.active)
			{
				Vector2 targetCenter = new Vector2(targetHitbox.X + targetHitbox.Width / 2, targetHitbox.Y + targetHitbox.Height / 2);
				return (Vector2.Distance(targetCenter, Projectile.Center) < 60f + (targetHitbox.Width + targetHitbox.Height) / 4);
			}
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			Projectile P = Projectile;

			if (soundInstance != null)
			{
				soundInstance.Stop(true);
			}

			for (int num93 = 0; num93 < 4; num93++)
			{
				int num94 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 240, 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num94].position = P.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * (float)P.width / 2f;
			}
			for (int num95 = 0; num95 < 30; num95++)
			{
				int num96 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 62, 0f, 0f, 200, default(Color), 3.7f);
				Main.dust[num96].position = P.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * (float)P.width / 2f;
				Main.dust[num96].noGravity = true;
				Dust dust = Main.dust[num96];
				dust.velocity *= 3f;
				num96 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 90, 0f, 0f, 100, default(Color), 1.5f);
				Main.dust[num96].position = P.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * (float)P.width / 2f;
				dust = Main.dust[num96];
				dust.velocity *= 2f;
				Main.dust[num96].noGravity = true;
				Main.dust[num96].fadeIn = 1f;
				Main.dust[num96].color = Color.Crimson * 0.5f;
			}
			for (int num97 = 0; num97 < 10; num97++)
			{
				int num98 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 62, 0f, 0f, 0, default(Color), 2.7f);
				Main.dust[num98].position = P.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy((double)P.velocity.ToRotation(), default(Vector2)) * (float)P.width / 2f;
				Main.dust[num98].noGravity = true;
				Dust dust = Main.dust[num98];
				dust.velocity *= 3f;
			}
			for (int num99 = 0; num99 < 10; num99++)
			{
				int num100 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 240, 0f, 0f, 0, default(Color), 1.5f);
				Main.dust[num100].position = P.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy((double)P.velocity.ToRotation(), default(Vector2)) * (float)P.width / 2f;
				Main.dust[num100].noGravity = true;
				Dust dust = Main.dust[num100];
				dust.velocity *= 3f;
			}
			var entitySource = Projectile.GetSource_FromAI();
			for (int num101 = 0; num101 < 2; num101++)
			{
				int num102 = Gore.NewGore(entitySource, P.position + new Vector2((float)(P.width * Main.rand.Next(100)) / 100f, (float)(P.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num102].position = P.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * (float)P.width / 2f;
				Gore gore = Main.gore[num102];
				gore.velocity *= 0.3f;
				Gore gore17 = Main.gore[num102];
				gore17.velocity.X = gore17.velocity.X + (float)Main.rand.Next(-10, 11) * 0.05f;
				Gore gore18 = Main.gore[num102];
				gore18.velocity.Y = gore18.velocity.Y + (float)Main.rand.Next(-10, 11) * 0.05f;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = Projectile;

			Texture2D tex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/missilecombo/NebulaComboShot").Value,
			tex2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/missilecombo/NebulaComboShot2").Value;

			SpriteEffects spriteEffects = SpriteEffects.None;
			if (P.spriteDirection == -1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			Color color25 = Lighting.GetColor((int)((double)P.position.X + (double)P.width * 0.5) / 16, (int)(((double)P.position.Y + (double)P.height * 0.5) / 16.0));

			Vector2 vector53 = P.Center - Main.screenPosition;
			Color alpha4 = P.GetAlpha(color25);
			Vector2 origin8 = new Vector2((float)tex.Width, (float)tex.Height) / 2f;

			Color color57 = alpha4 * 0.8f;
			color57.A /= 2;
			Color color58 = Color.Lerp(alpha4, Color.Black, 0.5f);
			color58.A = alpha4.A;
			float num274 = 0.95f + (P.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
			color58 *= num274;
			float scale13 = 0.6f + P.scale * 0.6f * num274;

			if (Lead != null && Lead.active)
			{
				float dist = Math.Max(Vector2.Distance(Lead.Center, P.Center), 1);
				Vector2 diff2 = Vector2.Normalize(P.Center - Lead.Center);
				if (float.IsNaN(diff2.X) || float.IsNaN(diff2.Y))
				{
					diff2 = -Vector2.UnitY;
				}

				int k = 1;
				for (float i = 0f; i < dist; i += 1f + (30f * (i / dist)))
				{
					SpriteEffects se = SpriteEffects.None;
					if (k == -1)
					{
						se = SpriteEffects.FlipHorizontally;
					}

					Vector2 pos1 = Lead.Center + Vector2.Normalize(Lead.velocity) * i;
					Vector2 pos2 = Lead.Center + diff2 * i;

					Vector2 fPos = Vector2.Lerp(pos1, pos2, i / dist) - Main.screenPosition;

					float rot = ((float)Math.PI * 2f / dist) * i;
					sb.Draw(tex2, fPos, null, alpha4, rot + P.rotation * k, origin8, MathHelper.Lerp(0.1f, P.scale, (i / dist)), se, 0f);
					k *= -1;
				}

				Texture2D tex3 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/missilecombo/NebulaBusterShot").Value;
				int num108 = tex3.Height / 2;
				int y4 = num108 * P.frame;

				Texture2D tex4 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/Projectiles/missilecombo/NebulaBusterShot2").Value;
				int numH = tex4.Height / 4;

				float scale = P.scale;
				if (P.frame == 0)
				{
					scale *= 0.8f;
				}

				int num = (int)Math.Max(Math.Ceiling(dist / 8), 1);
				Vector2[] pos = new Vector2[num];

				diff2 = Vector2.Normalize((P.Center + new Vector2(Main.rand.Next(-30, 31), Main.rand.Next(-30, 31))) - Lead.Center);
				if (float.IsNaN(diff2.X) || float.IsNaN(diff2.Y))
				{
					diff2 = -Vector2.UnitY;
				}
				Vector2 diff3 = Vector2.Normalize(Lead.velocity + new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11)));
				if (float.IsNaN(diff3.X) || float.IsNaN(diff3.Y))
				{
					diff3 = -Vector2.UnitY;
				}

				for (int i = 0; i < num; i++)
				{
					float dScale = ((float)i / num);
					Vector2 pos1 = Lead.Center + diff3 * dist * dScale;
					Vector2 pos2 = Lead.Center + diff2 * dist * dScale;

					pos[i] = Vector2.Lerp(pos1, pos2, dScale);

					if (i > 0)
					{
						float rot = (float)Math.Atan2((pos[i].Y - pos[i - 1].Y), (pos[i].X - pos[i - 1].X)) + (float)Math.PI / 2;

						sb.Draw(tex3,
						pos[i] - Main.screenPosition,
						new Rectangle?(new Rectangle(0, y4, tex3.Width, num108)),
						P.GetAlpha(Color.White),
						rot,
						new Vector2((float)tex3.Width / 2f, (float)num108 / 2),
						new Vector2(scale, 1f),
						SpriteEffects.None,
						0f);
					}
				}

				sb.Draw(tex2, vector53, null, color58, -P.rotation + 0.35f, origin8, scale13, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
				sb.Draw(tex2, vector53, null, alpha4, -P.rotation, origin8, P.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);

				for (int j = 0; j < buster.Length; j++)
				{
					if (buster[j] != null && buster[j].active)
					{
						dist = Math.Max(Vector2.Distance(buster[j].Center, P.Center), 1);

						num = (int)Math.Max(Math.Ceiling(dist / 2), 1);
						pos = new Vector2[num];

						float bRot = ((float)Math.PI / 2f * buster[j].ai[1]) + (P.rotation / 2f);
						Vector2 rotPoint = P.Center + bRot.ToRotationVector2() * distance * P.scale;

						diff2 = Vector2.Normalize(rotPoint - P.Center);
						if (float.IsNaN(diff2.X) || float.IsNaN(diff2.Y))
						{
							diff2 = -Vector2.UnitY;
						}
						diff3 = Vector2.Normalize(buster[j].Center - P.Center);
						if (float.IsNaN(diff3.X) || float.IsNaN(diff3.Y))
						{
							diff3 = -Vector2.UnitY;
						}

						for (int i = 0; i < num; i++)
						{
							float dScale = ((float)i / num);
							Vector2 pos1 = P.Center + diff2 * dist * dScale;
							Vector2 pos2 = P.Center + diff3 * dist * dScale;

							pos[i] = Vector2.Lerp(pos1, pos2, dScale);

							if (i > 0)
							{
								float rot = (float)Math.Atan2((pos[i].Y - pos[i - 1].Y), (pos[i].X - pos[i - 1].X)) + (float)Math.PI / 2;

								sb.Draw(tex4,
								pos[i] - Main.screenPosition,
								new Rectangle?(new Rectangle(0, 0, tex4.Width, tex4.Height)),
								color58,
								rot,
								new Vector2((float)tex4.Width / 2f, (float)tex4.Height / 2),
								new Vector2(MathHelper.Lerp(1f, 0.25f, dScale), 1f),
								SpriteEffects.None,
								0f);
							}
						}

						for (int i = 0; i < num; i++)
						{
							float dScale = ((float)i / num);
							Vector2 pos1 = P.Center + diff2 * dist * dScale;
							Vector2 pos2 = P.Center + diff3 * dist * dScale;

							pos[i] = Vector2.Lerp(pos1, pos2, dScale);

							if (i > 0)
							{
								float rot = (float)Math.Atan2((pos[i].Y - pos[i - 1].Y), (pos[i].X - pos[i - 1].X)) + (float)Math.PI / 2;

								sb.Draw(tex3,
								pos[i] - Main.screenPosition,
								new Rectangle?(new Rectangle(0, y4, tex3.Width, num108)),
								P.GetAlpha(Color.White),
								rot,
								new Vector2((float)tex3.Width / 2f, (float)num108 / 2),
								new Vector2(MathHelper.Lerp(1f, 0.625f, dScale) * scale, 1f),
								SpriteEffects.None,
								0f);
							}

							Lighting.AddLight(pos[i], (MetroidMod.waveColor2.R / 255f) * P.scale, (MetroidMod.waveColor2.G / 255f) * P.scale, (MetroidMod.waveColor2.B / 255f) * P.scale);

							if (Main.rand.NextBool(25))
							{
								Vector2 dPos = pos[i] - new Vector2(tex3.Width / 2, tex3.Width / 2);
								Main.dust[Dust.NewDust(dPos, tex3.Width, tex3.Width, 255, 0, 0, 100, default(Color), 2f)].noGravity = true;
							}
						}
					}
				}
			}

			/*sb.Draw(tex2, vector53, null, color58, -P.rotation + 0.35f, origin8, scale13, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(tex2, vector53, null, alpha4, -P.rotation, origin8, P.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);*/
			sb.Draw(tex, vector53, null, color57, -P.rotation * 0.7f, origin8, P.scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0f);
			sb.Draw(tex2, vector53, null, alpha4 * 0.8f, P.rotation * 0.5f, origin8, P.scale * 0.9f, spriteEffects, 0f);
			alpha4.A = 0;

			sb.Draw(tex, vector53, null, alpha4, P.rotation, origin8, P.scale, spriteEffects, 0f);
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			if (this.initialize)
				for (int i = 0; i < this.buster.Length; ++i)
					writer.Write(this.buster[i].whoAmI);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			for (int i = 0; i < this.buster.Length; ++i)
				this.buster[i] = Main.projectile[reader.ReadInt32()];

			if (!initialize)
				initialize = true;
		}
	}
}
