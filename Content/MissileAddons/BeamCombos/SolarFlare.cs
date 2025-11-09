using System;
using System.IO;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.BeamAddons;
using MetroidMod.Content.Projectiles;
using MetroidMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MissileAddons.BeamCombos
{
	public class SolarFlare : ModMissileAddon
	{
		public override bool AddOnlyAddonItem => false;
		public override Color PrimaryColor => MetroidMod.plaRedColor;
		public override Color SecondaryColor => MetroidMod.plaRedSecondaryColor;
		public override int ShotDust => DustID.YellowTorch;
		public override int ShotFrames => 2;
		public override bool HoldFire => true;
		private float BeamLength
		{
			get => mProjectile.Projectile.localAI[1];
			set => mProjectile.Projectile.localAI[1] = value;
		}
		private const float Max_Range = 2200f;
		private float maxRange = 0f;

		private float scaleUp = 0f;
		private int num = 0;

		private SoundEffectInstance soundInstance;
		private SoundEffectInstance soundInstance2;
		private string LoopSound => $"{Mod.Name}/Assets/Sounds/MissileAddons/{Name}/Loop";
		private string LoopSound2 => $"{Mod.Name}/Assets/Sounds/MissileAddons/NovaLaser/Loop";
		public override void SetStaticDefaults()
		{
			AddonSlot = MissileAddonSlotID.Charge;
			base.SetStaticDefaults();
		}
		public override void SetProjectileDefaults(MProjectile mProjectile)
		{
			base.SetProjectileDefaults(mProjectile);
			mProjectile.Projectile.width = 26;
			mProjectile.Projectile.height = 26;
			mProjectile.Projectile.scale = 1f;
			mProjectile.Projectile.tileCollide = false;
			mProjectile.Projectile.penetrate = -1;
			mProjectile.Projectile.extraUpdates = 5;
			mProjectile.Projectile.usesLocalNPCImmunity = true;
			mProjectile.Projectile.localNPCHitCooldown = 4 * (1 + mProjectile.Projectile.extraUpdates);
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = 70000;
			item.rare = ItemRarityID.LightRed;
			base.SetItemDefaults(item);
		}
		public override void HoldFireBehavior(Player player, Projectile lead)
		{
			Item item = player.HeldItem;
			Vector2 oPos = player.RotatedRelativePoint(player.MountedCenter, true);
			Lead = lead;
			if (!Initialized && Lead.active)
			{
				float MY = Main.mouseY + Main.screenPosition.Y;
				float MX = Main.mouseX + Main.screenPosition.X;
				if (player.gravDir == -1f)
				{
					MY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
				}
				float targetrotation = (float)Math.Atan2(MY - oPos.Y, MX - oPos.X);
				Vector2 velocity = targetrotation.ToRotationVector2() * item.shootSpeed;
				Projectile.NewProjectile(player.GetSource_ItemUse(item), oPos.X, oPos.Y, velocity.X, velocity.Y, ProjectileType, 0, 0, player.whoAmI);
				Initialized = true;
			}
		}
		public override void AI(MProjectile mProjectile)
		{
			Projectile P = mProjectile.Projectile;
			Player O = Main.player[P.owner];
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			//if (O.HeldItem.GetGlobalItem<MGlobalItem>().statMissiles <= 0)
			//{
			//	P.Kill();
			//}
			if (!Lead.active || Lead.owner != P.owner || Lead.type != ModContent.ProjectileType<ChargeLead>() || !O.controlUseItem || O.HeldItem.GetGlobalItem<MGlobalItem>().isBeam)
			{
				Initialized = false;
				P.Kill();
				return;
			}
			else
			{
				if (!Initialized)
				{
					Initialized = true;
				}

				if (P.owner == Main.myPlayer)
				{
					if (soundInstance == null || soundInstance.State != SoundState.Playing)
					{
						SoundEngine.PlaySound(new(ShotSound), O.position);
						SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(new(LoopSound), O.position), out ActiveSound result);
						soundInstance = result.Sound;
						if (Main.soundVolume > 0f)
						{
							soundInstance.Volume = 0f;
						}
						SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(new(LoopSound2), O.position), out result);
						soundInstance2 = result.Sound;
					}
					else if (P.numUpdates == 0 && Main.soundVolume > 0f)
					{
						soundInstance.Volume = Math.Min(soundInstance.Volume + (0.05f * Main.soundVolume), 0.75f * Main.soundVolume);
						soundInstance2.Volume = Math.Min(soundInstance2.Volume + (0.05f * Main.soundVolume), 0.75f * Main.soundVolume);
					}
				}
				P.velocity = Vector2.Normalize(Lead.velocity);
				P.Center = oPos;
				P.timeLeft = 2;
				P.rotation = P.velocity.ToRotation() - MathHelper.PiOver2;

				maxRange = Math.Min(maxRange + 16f, Max_Range);

				var entitySource = O.GetSource_ItemUse(O.HeldItem);
				for (P.ai[1] = 0f; P.ai[1] <= maxRange; P.ai[1] += 4f)
				{
					Vector2 end = oPos + (P.velocity * P.ai[1]);
					if (CollideMethods.CheckCollide(end, 0, 0))
					{
						P.ai[1] -= 4f;
						if (num <= 0)
						{
							end = oPos + (P.velocity * P.ai[1]);
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
					Vector2 sPos = oPos + (P.velocity * i);
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
							Vector2 dPos = oPos - (P.Size / 2f * scaleUp) + (P.velocity * k);
							Main.dust[Dust.NewDust(dPos, (int)(P.width * scaleUp), (int)(P.width * scaleUp), 6, 0, 0, 100, default(Color), 3f)].noGravity = true;
						}
					}
				}

				Vector2 dustPos = oPos - (P.Size / 2f * scaleUp) + (P.velocity * P.ai[1]);
				int size = (int)(P.width * scaleUp);
				float num1 = P.velocity.ToRotation() + ((Main.rand.NextBool(2) ? -1.0f : 1.0f) * MathHelper.PiOver2);
				float num2 = (float)((Main.rand.NextDouble() * 0.8f) + 1.0f);
				Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
				Dust dust = Main.dust[Dust.NewDust(dustPos, size, size, 6, dustVel.X, dustVel.Y, 100, default(Color), 4f)];
				dust.noGravity = true;
				dust.velocity *= 3f;

				Color color = MetroidMod.novColor;
				DelegateMethods.v3_1 = new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
				Utils.PlotTileLine(P.Center, P.Center + (P.velocity * P.ai[1]), 26, DelegateMethods.CastLight);

				if (P.numUpdates == 0)
				{
					scaleUp = Math.Min(scaleUp + 0.1f, 1.7f);//2f);
					P.frame++;
					if (P.frame >= Main.projFrames[ProjectileType])
					{
						P.frame = 0;
					}
				}
				//ChargeLead chLead = (ChargeLead)Lead.ModProjectile;
				//chLead.extraScale = 1.125f * scaleUp;
			}
			P.netUpdate = true;
		}
		public override void CutTiles(MProjectile m)
		{
			Projectile P = m.Projectile;
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(P.Center, P.Center + (P.velocity * (P.ai[1] + 4f)), (P.width + 16) * P.scale, DelegateMethods.CutTiles);
		}

		public override void OnHitNPC(MProjectile mProjectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(39, 600, true);
			target.immune[mProjectile.Projectile.owner] = 4;
		}
		public override bool? Colliding(MProjectile mp, Rectangle projHitbox, Rectangle targetHitbox)
		{
			Projectile P = mp.Projectile;
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), P.Center,
				P.Center + (P.velocity * P.ai[1]), P.width, ref point);
		}
		public override void OnKill(MProjectile mProjectile, int timeLeft)
		{
			soundInstance?.Stop(true);
			soundInstance2?.Stop(true);
		}
		public override bool PreDrawProjectile(MProjectile mProjectile, ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			if (Lead.active && Lead != null)
			{
				Projectile P = mProjectile.Projectile;
				Player O = Main.player[P.owner];
				Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);

				Texture2D tex = ModContent.Request<Texture2D>(ShotTexture).Value;

				int tHeight = tex.Height / Main.projFrames[P.type];

				Vector2 scale = new Vector2(scaleUp, 1f);

				int bodyFrameCount = 3;

				int tailHeight = 22;
				int headHeight = 22;
				int bodyHeight = 30 / bodyFrameCount;

				float leadDist = Vector2.Distance(oPos, Lead.Center);

				for (float i = leadDist; i <= P.ai[1]; i += bodyHeight)
				{
					Vector2 pos = P.Center + (P.velocity * i);

					int height = Math.Min(bodyHeight, (int)(P.ai[1] - i - (headHeight / 2)));

					int frame = Main.rand.Next(bodyFrameCount);

					if (height > 0)
					{
						sb.Draw(tex, pos - Main.screenPosition,
						new Rectangle?(new Rectangle(0, tailHeight + 2 + (tHeight * P.frame) + (bodyFrameCount * frame), tex.Width, height)),
						P.GetAlpha(Color.White), P.rotation,
						new Vector2(tex.Width / 2f, 0f),
						scale, SpriteEffects.None, 0f);
					}
				}

				if (P.ai[1] > leadDist + (headHeight / 2))
				{
					Vector2 pos2 = P.Center + (P.velocity * P.ai[1]);
					sb.Draw(tex, pos2 - Main.screenPosition,
					new Rectangle?(new Rectangle(0, tailHeight + 2 + (bodyHeight * bodyFrameCount) + 2 + (tHeight * P.frame), tex.Width, headHeight)),
					P.GetAlpha(Color.White), P.rotation,
					new Vector2(tex.Width / 2f, headHeight - 3),
					scale, SpriteEffects.None, 0f);
				}
			}

			return false;
		}
		public override void SendExtraAI(BinaryWriter writer) => writer.Write(BeamLength);
		public override void ReceiveExtraAI(BinaryReader reader) => BeamLength = reader.ReadInt32();
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.FragmentSolar, 15)
				.AddIngredient(ItemID.LunarBar, 5)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
	public class SolarLaserFlareShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.penetrate = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 4;
			mProjectile.amplitude = 40f;
		}

		private int dir = 1;
		private float num = 0f;
		private bool initialize = false;
		public override void AI()
		{
			if (!initialize)
			{
				//num = Main.rand.Next(16);
				if (Main.rand.NextBool(2))
				{
					dir = -1;
				}
				amplitude = 20f + Main.rand.Next(40);
				initialize = true;
			}
			Projectile P = Projectile;
			Player O = Main.player[P.owner];
			Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			Projectile Lead = Main.projectile[(int)P.ai[0]];
			if (!Lead.active || Lead.owner != P.owner || Lead.type != ModContent.ProjectileType<ChargeLead>() || !O.controlUseItem || O.HeldItem.GetGlobalItem<MGlobalItem>().isBeam)
			{
				P.Kill();
				return;
			}
			else
			{
				Projectile Beam = Main.projectile[(int)P.ai[1]];
				P.velocity = Beam.velocity * 8f;

				float leadDist = Vector2.Distance(oPos, Lead.Center);

				if ((P.localAI[0] + P.localAI[1]) < leadDist)
				{
					P.localAI[1] = Math.Abs(P.localAI[1]);
				}

				for (int i = 0; i < P.oldPos.Length; i++)
				{
					float oldnum = Math.Max(num - (0.0375f * (i + 1)), 0f);

					float oldt = (float)Math.PI * oldnum;

					float oldshift = amplitude * (float)Math.Sin(oldt) * dir;

					float oldlength = P.localAI[0] + (P.localAI[1] * oldnum);

					Vector2 oldpos = oPos + (Beam.velocity * oldlength);

					float oldrot = (float)Math.Atan2(P.velocity.Y, P.velocity.X);
					P.oldPos[i].X = oldpos.X + ((float)Math.Cos(oldrot + ((float)Math.PI / 2)) * oldshift);
					P.oldPos[i].Y = oldpos.Y + ((float)Math.Sin(oldrot + ((float)Math.PI / 2)) * oldshift);
					P.oldPos[i] -= P.Size / 2f;
				}

				float t = (float)Math.PI * num;

				float shift = amplitude * (float)Math.Sin(t) * dir;

				float length = P.localAI[0] + (P.localAI[1] * num);

				Vector2 pos = oPos + (Beam.velocity * length);

				float rot = (float)Math.Atan2(P.velocity.Y, P.velocity.X);
				P.position.X = pos.X + ((float)Math.Cos(rot + ((float)Math.PI / 2)) * shift);
				P.position.Y = pos.Y + ((float)Math.Sin(rot + ((float)Math.PI / 2)) * shift);
				P.position -= P.Size / 2f;

				num = Math.Min(num + 0.0375f, 1f);
				if (num >= 1f)
				{
					P.Kill();
				}
			}

			Color color = MetroidMod.plaRedColor;
			Lighting.AddLight(P.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			if (P.numUpdates == 0)
			{
				P.frame++;
			}
			if (P.frame > 1)
			{
				P.frame = 0;
			}

			if (P.numUpdates == 0)
			{
				int dust = Dust.NewDust(P.position, P.width, P.height, 6, 0, 0, 100, default(Color), P.scale);
				Main.dust[dust].noGravity = true;
			}

			Vector2 velocity = P.position - P.oldPos[0];
			if (velocity.Length() < Vector2.Normalize(P.velocity).Length())
			{
				velocity = Vector2.Normalize(P.velocity);
			}
			P.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + MathHelper.PiOver2;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(lightColor.R, lightColor.G, lightColor.B, 50);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, $"{Mod.Name}/Assets/Textures/MissileAddons/SolarFlare/Flare");
			return false;
		}
	}
	public class SolarLaserFlameTrail : MProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 9;
		}
		private readonly int maxTimeLeft = 60;
		private static readonly int width = 24;
		private static readonly int height = 36;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = width;
			Projectile.height = height;
			Projectile.scale = 0.5f;
			Projectile.timeLeft = maxTimeLeft * 2;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 6;
		}

		private bool initialize = false;
		public override void AI()
		{
			Projectile P = Projectile;
			P.rotation = 0f;

			if (!initialize)
			{
				P.frame = Main.rand.Next(3);
				P.position.Y -= 2f * P.scale;
				initialize = true;
			}

			Color color = MetroidMod.plaRedColor;
			Lighting.AddLight(P.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			P.ai[0] += 1f;
			if (P.ai[0] > 3f)
			{
				float num297 = 0.7f + (0.3f * (P.scale - 1f));
				int num3;
				for (int num299 = 0; num299 < 1; num299 = num3 + 1)
				{
					int num300 = Dust.NewDust(new Vector2(P.position.X, P.position.Y), P.width, P.height, 6, P.velocity.X * 0.2f, P.velocity.Y * 0.2f, 100, default(Color), 1f);
					Dust dust3;
					if (!Main.rand.NextBool(3))
					{
						Main.dust[num300].noGravity = true;
						dust3 = Main.dust[num300];
						dust3.scale *= 3f;
						Dust dust52 = Main.dust[num300];
						dust52.velocity.X = dust52.velocity.X * 2f;
						Dust dust53 = Main.dust[num300];
						dust53.velocity.Y = dust53.velocity.Y * 2f;
					}
					dust3 = Main.dust[num300];
					dust3.scale *= 1.5f;
					Dust dust54 = Main.dust[num300];
					dust54.velocity.X = dust54.velocity.X * 1.2f;
					Dust dust55 = Main.dust[num300];
					dust55.velocity.Y = dust55.velocity.Y * 1.2f;
					dust3 = Main.dust[num300];
					dust3.scale *= num297;
					num3 = num299;
				}
			}

			if (P.ai[0] <= maxTimeLeft)
			{
				P.scale += 2f / maxTimeLeft;
			}
			else
			{
				P.scale -= 2f / maxTimeLeft;
			}
			if (P.scale < 0.5f)
			{
				P.scale = 0.5f;
			}

			mProjectile.Explode((int)(P.width * P.scale));

			if (P.numUpdates <= 0)
			{
				P.frame++;
				if (P.frame >= 3)
				{
					P.frame = 0;
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(24, 600, true);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(lightColor.R, lightColor.G, lightColor.B, 100);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = Projectile;
			if (P.ai[0] > 3f)
			{
				SpriteEffects effects = SpriteEffects.None;
				if (P.spriteDirection == -1)
				{
					effects = SpriteEffects.FlipHorizontally;
				}
				Texture2D tex = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/MissileAddons/SolarFlare/Trail").Value;
				int num108 = tex.Height / Main.projFrames[P.type];
				int frame = P.frame;
				float scale = P.scale;
				if (P.scale >= 1.75f)
				{
					scale -= 1f;
					frame += 6;
				}
				else if (P.scale >= 1.25f)
				{
					scale -= 0.5f;
					frame += 3;
				}
				int y4 = num108 * frame;

				sb.Draw(tex, new Vector2((int)(P.Center.X - Main.screenPosition.X), (int)(P.position.Y + P.height - Main.screenPosition.Y)),
				new Rectangle?(new Rectangle(0, y4, tex.Width, num108)),
				P.GetAlpha(Color.White), 0f,
				new Vector2(tex.Width / 2f, (float)num108 - 2),
				scale, effects, 0f);
			}
			return false;
		}
	}
}
