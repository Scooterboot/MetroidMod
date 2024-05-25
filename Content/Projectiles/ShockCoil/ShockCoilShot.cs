using System;
using System.IO;
using MetroidMod.Common.Configs;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.ShockCoil
{
	public class ShockCoilShot : MProjectile
	{
		//HOW DOES NETUPDATE WORK REEEEE Dr
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 12;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 1f;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.tileCollide = false;
		}

		Vector2 targetPos;
		bool setTargetPos = false;

		Projectile Lead;

		NPC target;

		/*const float Max_Range = 250f;
        float range = Max_Range;
        const float Max_Distance = 250f;
        float distance = Max_Distance;*/

		Vector2 oPos;
		Vector2 mousePos;

		SoundEffectInstance soundInstance;
		bool soundPlayed = false;
		int soundDelay = 30;

		int ampSyncCooldown = 20;
		//int shots = 1;
		int immuneTime = 0;
		int dmg = 0;

		float[] amp = new float[3];
		float[] ampDest = new float[3];

		private int GetDepth(MProjectile mp)
		{
			return mp.waveDepth;
		}
		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_Parent parent && parent.Entity is Player player && player.HeldItem.type == ModContent.ItemType<PowerBeam>())
			{
				if (player.HeldItem.ModItem is PowerBeam hold)
				{
					shot = hold.shotEffect.ToString();

				}
			}
			dmg = Projectile.damage;
			base.OnSpawn(source);
		}

		public override void AI()
		{
			Projectile P = Projectile;
			MProjectile meep = mProjectile;
			Player O = Main.player[P.owner];
			MPlayer mp = O.GetModPlayer<MPlayer>();

			Vector2 V = P.velocity;
			P.knockBack = 0;

			Lead = Main.projectile[O.heldProj];
			if (P.numUpdates == 0)
			{
				P.frame++;
			}
			if (P.frame >= 12)
			{
				P.frame = 0;
			}
			//range = Math.Min(GetDepth(meep), Max_Range);
			//distance = Math.Min(GetDepth(meep), Max_Distance);
			if(immuneTime > 0)
			{
				P.damage = 0;
				immuneTime--;
			}
			else
			{
				P.damage = dmg;
			}
			mProjectile.WaveBehavior(P);

			float range = (GetDepth(meep) * 16) + 32f;
			float distance = (GetDepth(meep) * 16) + 32f;

			oPos = O.RotatedRelativePoint(O.MountedCenter, true);

			if (Lead != null && Lead.active && Lead.type == ModContent.ProjectileType<ChargeLead>() && Lead.owner == Main.myPlayer)
			{
				if (P.owner == Main.myPlayer && !O.dead)
				{
					P.netUpdate = true;
					Vector2 diff = Main.MouseWorld - oPos;
					diff.Normalize();

					mousePos = oPos + diff * Math.Min(Vector2.Distance(oPos, Main.MouseWorld), range);

					target = null;
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						if (Main.npc[i].active && Main.npc[i].lifeMax > 5 && !Main.npc[i].dontTakeDamage && !Main.npc[i].friendly)
						{
							NPC npc = Main.npc[i];
							Rectangle npcRect = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);

							float point = 0f;
							if (Vector2.Distance(oPos, npc.Center) < range && Collision.CheckAABBvLineCollision(npcRect.TopLeft(), npcRect.Size(), oPos, P.Center, P.width, ref point))
							{
								range = Vector2.Distance(oPos, npc.Center);
								mousePos = oPos + diff * Math.Min(Vector2.Distance(oPos, Main.MouseWorld), range);
							}

							bool flag = (Vector2.Distance(oPos, npc.Center) <= range + distance && Vector2.Distance(npc.Center, mousePos) <= distance);

							if (Main.npc[i].CanBeChasedBy(P, false))
							{
								if (target == null || !target.active)
								{
									if (flag)
									{
										target = npc;
									}
								}
								else
								{
									if (npc != target && flag && Vector2.Distance(npc.Center, mousePos) < Vector2.Distance(target.Center, mousePos))
									{
										target = npc;
										//mp.statCharge = 0;//reset when changing targets. makes this stupid useless in crowds
									}

									if (Vector2.Distance(oPos, target.Center) > range + distance || Vector2.Distance(target.Center, mousePos) > distance)
									{
										target = null;
									}
								}
							}
						}
					}
					if (target == null || !target.active)
					{
						targetPos = Lead.Center;
						//P.netUpdate = true;
					}
					if (!setTargetPos)
					{
						targetPos = P.Center;
						setTargetPos = true;
						//P.netUpdate = true;
						return;
					}
					else if (target != null && target.active)
					{
						targetPos = target.Center;
						//P.netUpdate = true;
					}
					else
					{
						if (P.numUpdates == 0)
						{
							mp.statCharge = 0;
							targetPos = oPos + diff * range;
							//P.netUpdate = true;
							//targetPos.X += Main.rand.Next(-15, 16) * (Vector2.Distance(oPos, P.Center) / Max_Range);
							//targetPos.Y += Main.rand.Next(-15, 16) * (Vector2.Distance(oPos, P.Center) / Max_Range);
						}
					}

					if (P.numUpdates == 0)
					{
						if (soundDelay <= 0)
						{
							if (!soundPlayed)
							{
								SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilSound, O.position), out ActiveSound result);
								soundInstance = result.Sound;
								soundPlayed = true;
								soundDelay = 50;
							}
							if (mp.statCharge == MPlayer.maxCharge && mp.statOverheat < mp.maxOverheat)
							{
								SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilAffinity2, O.position), out ActiveSound result);
								soundInstance = result.Sound;
								soundDelay = 40;
							}

							else
							{
								if (soundInstance != null)
								{
									soundInstance.Stop(true);
								}
								SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilSound, O.position), out ActiveSound result);
								soundInstance = result.Sound;
								soundDelay = 40;
							}
						}
						else
						{
							soundDelay--;
						}
						for (int i = 0; i < 3; i++)
						{
							ampDest[i] = Main.rand.Next(-15, 16);
						}
					}

					if (ampSyncCooldown-- <= 0)
					{
						ampSyncCooldown = 20;
						Projectile.netUpdate2 = true;
					}
					float speed = Math.Max(8f, Vector2.Distance(targetPos, P.Center) * 0.25f);
					float targetAngle = (float)Math.Atan2(targetPos.Y - P.Center.Y, targetPos.X - P.Center.X);
					P.velocity = targetAngle.ToRotationVector2() * speed;
				}
				if (O.controlUseItem)
				{
					P.timeLeft = 5;
				}
				else
				{
					P.Kill();
				}

				if (P.numUpdates == 0)
				{
					for (int i = 0; i < 3; i++)
					{
						ampDest[i] = Main.rand.Next(-15, 16);
					}
				}

				for (int i = 0; i < 3; i++)
				{
					if (amp[i] < ampDest[i])
					{
						amp[i] += 3;
					}
					else
					{
						amp[i] -= 3;
					}
				}
				if (mp.statOverheat >= mp.maxOverheat || O.HeldItem.GetGlobalItem<MGlobalItem>().statUA <= O.HeldItem.GetGlobalItem<MGlobalItem>().addonUACost)
				{
					P.Kill();
					mp.statCharge = 0;
					SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilReload, O.position);
				}
			}
		}
		public override bool? CanHitNPC(NPC target3)
		{
			if (target != target3 || !shot.Contains("wave") && !shot.Contains("nebula") && !Collision.CanHitLine(Lead.Center, Projectile.width, Projectile.height, targetPos, Projectile.width, Projectile.height) || immuneTime > 0)
			{
				return false;
			}
			return base.CanHitNPC(target3);
		}
		public override void CutTiles()
		{
			Player p = Main.player[Projectile.owner];
			if (p.controlUseItem)
			{
				DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
				Utils.PlotTileLine(p.Center, Projectile.Center, (Projectile.width + 16) * Projectile.scale, DelegateMethods.CutTiles);
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Player p = Main.player[Projectile.owner];
			if (p.controlUseItem)
			{
				float point = 0f;
				return projHitbox.Intersects(targetHitbox) ||
					Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), oPos, Projectile.Center, Projectile.width, ref point);
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = Projectile;
			MProjectile meep = mProjectile;
			Color color = MetroidMod.powColor;
			Player O = Main.player[Projectile.owner];
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
			int num108 = tex.Height / Main.projFrames[P.type];
			int y4 = num108 * P.frame;
			oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			P.scale = .8f;
			float range = (GetDepth(meep) * 16) + 48f;
			if (O.controlUseItem && !O.dead)
			{

				float targetrot = (float)Math.Atan2(P.Center.Y - Lead.Center.Y, P.Center.X - Lead.Center.X);
				float dist = Math.Max(Vector2.Distance(Lead.Center, P.Center), 1);

				double trot = targetrot + Math.PI / 2;

				float shift = 0;
				int num = (int)Math.Max(Math.Ceiling(dist / 8), 1);
				float num4 = num / 4;
				Vector2[] pos = new Vector2[num];
				for (int i = 0; i < num; i++)
				{
					float scale = P.scale;
					if (P.frame == 0)
					{
						scale *= 0.8f;
					}

					if (num4 >= 1)
					{
						if (i < num4)
						{
							shift = MathHelper.Lerp(0, amp[0], (i / num4));
						}
						else if (i < num / 2)
						{
							shift = MathHelper.Lerp(amp[0], amp[1], ((i - num4) / num4));
						}
						else if (i < num4 * 3)
						{
							shift = MathHelper.Lerp(amp[1], amp[2], ((i - num / 2) / num4));
						}
						else
						{
							shift = MathHelper.Lerp(amp[2], 0, ((i - num4 * 3) / num4));
							scale *= (num4 - (i - num4 * 3) * 0.5f) / num4;
						}
					}

					pos[i] = Lead.Center + targetrot.ToRotationVector2() * (dist / num) * i;
					pos[i].X += (float)Math.Cos(trot) * shift * (Vector2.Distance(oPos, P.Center) / range);
					pos[i].Y += (float)Math.Sin(trot) * shift * (Vector2.Distance(oPos, P.Center) / range);

					float rot = (float)Math.Atan2(pos[i].Y - Lead.Center.Y, pos[i].X - Lead.Center.X + (float)Math.PI / 2);
					if (i > 0)
					{
						rot = (float)Math.Atan2(pos[i].Y - pos[i - 1].Y, pos[i].X - pos[i - 1].X) + (float)Math.PI / 2;
					}
					sb.Draw(tex, pos[i] - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), P.GetAlpha(Color.White), rot, new Vector2(tex.Width / 2f, (float)num108 / 2), new Vector2(scale, 1f), SpriteEffects.None, 0f);


					Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

				}
			}
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			soundInstance?.Stop(true);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(oPos);
			writer.WriteVector2(targetPos);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			oPos = reader.ReadVector2();
			targetPos = reader.ReadVector2();
			base.ReceiveExtraAI(reader);
		}
		public override void OnHitNPC(NPC target2, NPC.HitInfo hit, int damageDone)
		{
			Player O = Main.player[Projectile.owner];
			MPlayer mp = O.GetModPlayer<MPlayer>();
			int heal = ((int)(damageDone * (mp.statCharge / MPlayer.maxCharge));// * (O.statLife / O.statLifeMax2));
			float minDamage = MConfigItems.Instance.minSpeedShockCoil + (Luminite? 1.0f : DiffBeam? 0.5f :0);
			float maxDamage = MConfigItems.Instance.maxSpeedShockCoil + (Luminite ? 1.0f : DiffBeam ? 0.5f : 0);
			float ranges = maxDamage - minDamage;
			double damaage = Math.Clamp(mp.statCharge / MPlayer.maxCharge * ranges + minDamage, minDamage, maxDamage);
			//float bonusShots = (mp.statCharge * (shots - 1) / MPlayer.maxCharge) + 1f;
			int immunity = (int)(O.HeldItem.useTime / (double)damaage); //(int)(O.HeldItem.useTime / bonusShots / (double)damaage);
			//mp.statOverheat += mp.overheatCost; // /shots;
			mp.statCharge = Math.Min(mp.statCharge + 1.5f, MPlayer.maxCharge);
			if (mp.Energy < mp.MaxEnergy && !mp.PrimeHunter && (Luminite || DiffBeam))
			{
				if (heal > mp.MaxEnergy - mp.Energy)
				{
					mp.Energy = mp.MaxEnergy;
				}
				else
				{
					mp.Energy += heal;
				}
			}
			SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilAffinity1, Projectile.position);
			if (damageDone > 0)
			{
				immuneTime = 4 * immunity;
				Projectile.localNPCHitCooldown = immunity;
				/*foreach (NPC G in Main.npc)
				{
					//G.immune[O.whoAmI] = (int)(O.HeldItem.useTime / bonusShots / (double)damaage);
					Projectile.localNPCHitCooldown = (int)(O.HeldItem.useTime / bonusShots / (double)damaage);
				}*/
			}
			base.OnHitNPC(target2, hit, damageDone);
		}
	}
}
