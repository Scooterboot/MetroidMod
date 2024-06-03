using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.missilecombo
{
	public class SolarLaserShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Supernova Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10 * (1 + Projectile.extraUpdates);
		}
		private float BeamLength
		{
			get => Projectile.localAI[1];
			set => Projectile.localAI[1] = value;
		}
		const float Max_Range = 2200f;
		float maxRange = 0f;

		int num = 0;

		float scaleUp = 0f;

		Projectile Lead;

		SoundEffectInstance soundInstance;
		SoundEffectInstance soundInstance2;

		bool initialize = false;
		public override void AI()
		{
			Projectile P = Projectile;
			Player O = Main.player[P.owner];
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);

			Lead = Main.projectile[(int)P.ai[0]];
			if (!Lead.active || Lead.owner != P.owner || Lead.type != ModContent.ProjectileType<ChargeLead>() || !O.controlUseItem)
			{
				P.Kill();
				return;
			}
			else
			{
				if (!initialize)
				{

					initialize = true;
				}

				if (P.owner == Main.myPlayer)
				{
					if (soundInstance == null || soundInstance.State != SoundState.Playing)
					{
						SoundEngine.PlaySound(Sounds.Items.Weapons.SolarComboSoundStart, O.position);
						SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.SolarComboSoundLoop, O.position), out ActiveSound result);
						soundInstance = result.Sound;
						if (Main.soundVolume > 0f)
						{
							soundInstance.Volume = 0f;
						}
						SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.NovaLaserLoop, O.position), out result);
						soundInstance2 = result.Sound;
					}
					else if (P.numUpdates == 0 && Main.soundVolume > 0f)
					{
						soundInstance.Volume = Math.Min(soundInstance.Volume + 0.05f * Main.soundVolume, 0.75f * Main.soundVolume);
						soundInstance2.Volume = Math.Min(soundInstance2.Volume + 0.05f * Main.soundVolume, 0.75f * Main.soundVolume);
					}
				}
				P.velocity = Vector2.Normalize(Lead.velocity);
				P.Center = oPos;
				P.timeLeft = 2;
				P.rotation = P.velocity.ToRotation() - 1.57f;

				maxRange = Math.Min(maxRange + 16f, Max_Range);

				var entitySource = Projectile.GetSource_FromAI();
				for (P.ai[1] = 0f; P.ai[1] <= maxRange; P.ai[1] += 4f)
				{
					Vector2 end = oPos + P.velocity * P.ai[1];
					if (CollideMethods.CheckCollide(end, 0, 0))
					{
						P.ai[1] -= 4f;
						if (num <= 0)
						{
							end = oPos + P.velocity * P.ai[1];
							int proj = Projectile.NewProjectile(entitySource, end.X, end.Y, 0f, 0f, ModContent.ProjectileType<SolarLaserFlameTrail>(), P.damage, P.knockBack, P.owner);
							num = 4;
						}
						break;
					}
				}
				if (num > 0 && P.numUpdates == 0)
				{
					num--;
				}

				float leadDist = Vector2.Distance(oPos, Lead.Center);
				for (float i = leadDist; i < P.ai[1]; i += P.width)
				{
					Vector2 sPos = oPos + P.velocity * i;
					if (sPos.X > Main.screenPosition.X - 100f && sPos.X < Main.screenPosition.X + Main.screenWidth + 100f &&
						sPos.Y > Main.screenPosition.Y - 100f && sPos.Y < Main.screenPosition.Y + Main.screenHeight + 100f)
					{
						if (Main.rand.NextBool(50) && P.ai[1] > leadDist)
						{
							int numX = 1;
							if (Main.rand.NextBool(2))
							{
								numX = -1;
							}

							int proj = Projectile.NewProjectile(entitySource, sPos.X, sPos.Y, P.velocity.X, P.velocity.Y, ModContent.ProjectileType<SolarLaserFlareShot>(), P.damage, P.knockBack, P.owner);
							Projectile sProj = Main.projectile[proj];
							sProj.ai[0] = Lead.whoAmI;
							sProj.ai[1] = P.whoAmI;
							sProj.localAI[0] = i;
							sProj.localAI[1] = (Main.rand.Next(50) + 60) * numX;
						}

						if (Main.rand.NextBool(10))
						{
							float k = Math.Min(i, P.ai[1]);
							Vector2 dPos = (oPos - (P.Size / 2f) * scaleUp) + P.velocity * k;
							Main.dust[Dust.NewDust(dPos, (int)((float)P.width * scaleUp), (int)((float)P.width * scaleUp), 6, 0, 0, 100, default(Color), 3f)].noGravity = true;
						}
					}
				}

				Vector2 dustPos = oPos - (P.Size / 2f) * scaleUp + P.velocity * P.ai[1];
				int size = (int)((float)P.width * scaleUp);
				float num1 = P.velocity.ToRotation() + (Main.rand.NextBool(2) ? -1.0f : 1.0f) * 1.57f;
				float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
				Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
				Dust dust = Main.dust[Dust.NewDust(dustPos, size, size, 6, dustVel.X, dustVel.Y, 100, default(Color), 4f)];
				dust.noGravity = true;
				dust.velocity *= 3f;

				Color color = MetroidMod.novColor;
				DelegateMethods.v3_1 = new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
				Utils.PlotTileLine(P.Center, P.Center + P.velocity * P.ai[1], 26, DelegateMethods.CastLight);

				if (P.numUpdates == 0)
				{
					scaleUp = Math.Min(scaleUp + 0.1f, 1.7f);//2f);
					P.frame++;
					if (P.frame >= Main.projFrames[Projectile.type])
					{
						P.frame = 0;
					}
				}
				ChargeLead chLead = (ChargeLead)Lead.ModProjectile;
				chLead.extraScale = 1.125f * scaleUp;
			}
			P.netUpdate = true;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void SendExtraAI(BinaryWriter writer) => writer.Write(BeamLength);
		public override void ReceiveExtraAI(BinaryReader reader) => BeamLength = reader.ReadInt32();
		public override void CutTiles()
		{
			Projectile P = Projectile;
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(P.Center, P.Center + P.velocity * (P.ai[1] + 4f), ((float)P.width + 16f) * scaleUp, DelegateMethods.CutTiles);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(189, 600, true);
			target.immune[Projectile.owner] = 2;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Projectile P = Projectile;
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), P.Center,
				P.Center + P.velocity * P.ai[1], (float)P.width * scaleUp, ref point);
		}

		public override void OnKill(int timeLeft)
		{
			if (soundInstance != null)
			{
				soundInstance.Stop(true);
			}
			if (soundInstance2 != null)
			{
				soundInstance2.Stop(true);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			if (Lead != null && Lead.active)
			{
				Projectile P = Projectile;
				Player O = Main.player[P.owner];
				Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);

				Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;

				int tHeight = tex.Height / Main.projFrames[Projectile.type];

				Vector2 scale = new Vector2(scaleUp, 1f);

				int bodyFrameCount = 3;

				int tailHeight = 22;
				int headHeight = 22;
				int bodyHeight = 30 / bodyFrameCount;

				float leadDist = Vector2.Distance(oPos, Lead.Center);

				for (float i = leadDist; i <= P.ai[1]; i += bodyHeight)
				{
					Vector2 pos = P.Center + P.velocity * i;

					int height = Math.Min(bodyHeight, (int)(P.ai[1] - i - headHeight / 2));

					int frame = Main.rand.Next(bodyFrameCount);

					if (height > 0)
					{
						sb.Draw(tex, pos - Main.screenPosition,
						new Rectangle?(new Rectangle(0, tailHeight + 2 + (tHeight * P.frame) + (bodyFrameCount * frame), tex.Width, height)),
						P.GetAlpha(Color.White), P.rotation,
						new Vector2((float)tex.Width / 2f, 0f),
						scale, SpriteEffects.None, 0f);
					}
				}

				if (P.ai[1] > leadDist + headHeight / 2)
				{
					Vector2 pos2 = P.Center + P.velocity * P.ai[1];
					sb.Draw(tex, pos2 - Main.screenPosition,
					new Rectangle?(new Rectangle(0, tailHeight + 2 + (bodyHeight * bodyFrameCount) + 2 + (tHeight * P.frame), tex.Width, headHeight)),
					P.GetAlpha(Color.White), P.rotation,
					new Vector2((float)tex.Width / 2f, headHeight - 3),
					scale, SpriteEffects.None, 0f);
				}
			}

			return false;
		}
	}
}
