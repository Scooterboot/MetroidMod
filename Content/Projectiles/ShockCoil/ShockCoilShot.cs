using System;
using System.Collections.Generic;
using System.IO;
using MetroidMod.Common.Configs;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

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

		private Vector2 targetPos;
		private bool setTargetPos = false;
		public bool isaLink = false;
		private bool hasLink = false;

		private Projectile Lead;
		public Projectile parent = null;
		public Projectile child = null;

		public NPC target;

		/*const float Max_Range = 250f;
        float range = Max_Range;
        const float Max_Distance = 250f;
        float distance = Max_Distance;*/

		private Vector2 oPos;
		public Vector2 mousePos;

		private SoundEffectInstance soundInstance;
		private bool soundPlayed = false;
		private int soundDelay = 30;

		private int ampSyncCooldown = 20;
		private int shots = 1;
		private int immuneTime = 0;
		private int dmg = 0;
		public int shotCounter = 1;

		private readonly float[] amp = new float[3];
		private readonly float[] ampDest = new float[3];
		public float range;
		public float distance;

		private int GetDepth(MProjectile mp)
		{
			return mp.waveDepth;
		}
		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_Parent parent && parent.Entity is Player player && (player.HeldItem.type == ModContent.ItemType<PowerBeam>() || player.HeldItem.type == ModContent.ItemType<ArmCannon>()))
			{
				if (player.HeldItem.ModItem is PowerBeam hold)
				{
					shot = hold.shotEffect.ToString();
				}
				else if (player.HeldItem.ModItem is ArmCannon hold2)
				{
					shot = hold2.shotEffect.ToString();
					shots = hold2.shotAmt;
				}
			}
			dmg = Projectile.damage;
			base.OnSpawn(source);
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
		{
			Projectile P = Projectile;
			MProjectile meep = mProjectile;
			Player O = Main.player[P.owner];
			MPlayer mp = O.GetModPlayer<MPlayer>();
			ShockCoilShot main = (ShockCoilShot)Main.projectile[assign].ModProjectile;
			Vector2 V = P.velocity;
			P.knockBack = 0;

			Lead = !isaLink ? Main.projectile[O.heldProj] : parent;
			//if ((!parent.active||parent == null) && isaLink)
			//{
			//	P.Kill();
			//}
			if (P.numUpdates == 0)
			{
				P.frame++;
			}
			if (P.frame >= 12)
			{
				P.frame = 0;
			}
			if (immuneTime > 0)
			{
				P.damage = 0;
				immuneTime--;
			}
			else
			{
				P.damage = dmg;
			}
			mProjectile.WaveBehavior(P);

			range = (GetDepth(meep) * 16) + 32f;
			if (!isaLink)
			{
				distance = (GetDepth(meep) * 16) + 32f;
			}

			oPos = isaLink ? Lead.Center : O.RotatedRelativePoint(O.MountedCenter, true);

			if (P.owner == Main.myPlayer && !O.dead)
			{
				P.netUpdate = true;
				Vector2 diff = Main.MouseWorld - oPos;
				diff.Normalize();

				mousePos = oPos + (diff * Math.Min(Vector2.Distance(oPos, Main.MouseWorld), range));

				target = null;
				child = null;
				hasLink = false;
				foreach (var who in Main.ActiveNPCs)
				{
					NPC npc = Main.npc[who.whoAmI];
					if (npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.friendly)
					{
						Rectangle npcRect = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);

						float point = 0f;
						if (Vector2.Distance(oPos, npc.Center) < range && Collision.CheckAABBvLineCollision(npcRect.TopLeft(), npcRect.Size(), oPos, P.Center, P.width, ref point))
						{
							if (!isaLink)
							{
								range = Vector2.Distance(oPos, npc.Center);
								mousePos = oPos + (diff * Math.Min(Vector2.Distance(oPos, Main.MouseWorld), range));
							}
							else if (npc != target)
							{
								range = Vector2.Distance(Lead.Center, npc.Center);
							}
						}

						bool flag = Vector2.Distance(oPos, npc.Center) <= range + distance && ((Vector2.Distance(npc.Center, mousePos) <= distance));

						if (npc.CanBeChasedBy(P, false))
						{
							if (target == null || !target.active)
							{
								if (flag)
								{
									target = npc;
									if (!hasLink && (child == null) &&(!isaLink && main.shotCounter < shots))
									{
										main.shotCounter++;
										hasLink = true;
										int shotProj = Projectile.NewProjectile(P.GetSource_FromAI(), target.Center, Vector2.Zero, ModContent.ProjectileType<ShockCoilChargeShot>(), 0, 0, O.whoAmI);
										MProjectile mProj = (MProjectile)Main.projectile[shotProj].ModProjectile;
										mProj.assign = shotProj;
										mProj.shot = shot;
										child = Main.projectile[shotProj];
										if (mProj is ShockCoilChargeShot shocky)
										{
											//shocky.link = target.whoAmI;
											shocky.target = target;
											shocky.Lead = Main.projectile[assign];
											shocky.distance = distance - range;
											//mProj.waveDir = waveDir;
											shocky.dmg = dmg;
											//shocky.assign = mProj.assign;
											shocky.shotCounter =main.shots;
											//Main.projectile[shotProj].netUpdate = true;
										}
									}
								}
							}
							else
							{
								if (npc != target && flag && (Vector2.Distance(npc.Center, mousePos) < Vector2.Distance(target.Center, mousePos)))
								{
									//if (isaLink)
									//{
									//	P.Kill();
									//	//parent?.Kill();
									//}
									target = npc;
									if (!hasLink&& child == null && (!isaLink && main.shotCounter < shots))
									{
										main.shotCounter++;
										hasLink = true;
										int shotProj = Projectile.NewProjectile(P.GetSource_FromAI(), target.Center, Vector2.Zero, ModContent.ProjectileType<ShockCoilChargeShot>(), 0, 0, O.whoAmI);
										MProjectile mProj = (MProjectile)Main.projectile[shotProj].ModProjectile;
										mProj.assign = shotProj;
										mProj.shot = shot;
										child = Main.projectile[shotProj];
										if (mProj is ShockCoilChargeShot shocky)
										{
											//shocky.link = target.whoAmI;
											shocky.target = target;
											shocky.Lead = Main.projectile[assign];
											shocky.distance = distance - range;
											//mProj.waveDir = waveDir;
											shocky.dmg = dmg;
											//shocky.assign = mProj.assign;
											shocky.shotCounter = main.shots;
											//Main.projectile[shotProj].netUpdate = true;
										}
									}
								}

								if ((Vector2.Distance(oPos, target.Center) > range + distance || Vector2.Distance(target.Center, mousePos) > distance))
								{
									target = null;
									hasLink = false;
									main.shotCounter = 1;
									//if (isaLink )
									//{
									//	P.Kill();
									//	//parent?.Kill();
									//}
									//child?.Kill();
									child = null;
								}
							}
						}
					}
				}
				if (isaLink && (!parent.active || parent == null))
				{
					P.Kill();
				}
				if ((child == null || !child.active)&& !isaLink)
				{
					main.shotCounter = 1;
					hasLink = false;
				}
				if (target == null || !target.active)
				{
					//main.shotCounter = 1;
					if (isaLink)
					{
						P.Kill();
					}
					//if (child.active && child != null)
					//{
					//	child.Kill();
					//}
					targetPos = Lead.Center;
				}
				if (!setTargetPos)
				{
					targetPos = P.Center;
					setTargetPos = true;
					return;
				}
				else if (target != null && target.active)
				{
					targetPos = target.Center;
				}
				else
				{
					if (P.numUpdates == 0)
					{
						mp.statCharge = 0;
						targetPos = oPos + (diff * range);
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
				}
				float speed = Math.Max(8f, Vector2.Distance(targetPos, P.Center) * 0.25f);
				float targetAngle = (float)Math.Atan2(targetPos.Y - P.Center.Y, targetPos.X - P.Center.X);
				P.velocity = targetAngle.ToRotationVector2() * speed;
				P.netUpdate = true;
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
			if (mp.statOverheat >= mp.maxOverheat || O.HeldItem.GetGlobalItem<MGlobalItem>().statUA <= 0)//O.HeldItem.GetGlobalItem<MGlobalItem>().addonUACost)
			{
				P.Kill();
				mp.statCharge = 0;
				SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilReload, O.position);
			}
		}
		public override bool? CanHitNPC(NPC target3)
		{
			if (target != target3 || (!shot.Contains("wave") && !shot.Contains("nebula") && !Collision.CanHitLine(Lead.Center, Projectile.width, Projectile.height, targetPos, Projectile.width, Projectile.height)) || immuneTime > 0)
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
			Color color = MetroidMod.powColor;
			Player O = Main.player[P.owner];
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
			int num108 = tex.Height / Main.projFrames[P.type];
			int y4 = num108 * P.frame;
			P.scale = .8f;
			Lead = !isaLink ? Main.projectile[O.heldProj] : parent;
			if (O.controlUseItem && !O.dead)
			{

				float targetrot = (float)Math.Atan2(P.Center.Y - Lead.Center.Y, P.Center.X - Lead.Center.X);
				float dist = Math.Max(Vector2.Distance(Lead.Center, P.Center), 1);

				double trot = targetrot + (Math.PI / 2);

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
							shift = MathHelper.Lerp(0, amp[0], i / num4);
						}
						else if (i < num / 2)
						{
							shift = MathHelper.Lerp(amp[0], amp[1], (i - num4) / num4);
						}
						else if (i < num4 * 3)
						{
							shift = MathHelper.Lerp(amp[1], amp[2], (i - (num / 2)) / num4);
						}
						else
						{
							shift = MathHelper.Lerp(amp[2], 0, (i - (num4 * 3)) / num4);
							scale *= (num4 - ((i - (num4 * 3)) * 0.5f)) / num4;
						}
					}

					pos[i] = Lead.Center + (targetrot.ToRotationVector2() * (dist / num) * i);
					pos[i].X += (float)Math.Cos(trot) * shift * (Vector2.Distance(oPos, P.Center) / range);
					pos[i].Y += (float)Math.Sin(trot) * shift * (Vector2.Distance(oPos, P.Center) / range);

					float rot = (float)Math.Atan2(pos[i].Y - Lead.Center.Y, pos[i].X - Lead.Center.X + ((float)Math.PI / 2));
					if (i > 0)
					{
						rot = (float)Math.Atan2(pos[i].Y - pos[i - 1].Y, pos[i].X - pos[i - 1].X) + ((float)Math.PI / 2);
					}
					sb.Draw(tex, pos[i] - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), P.GetAlpha(Color.White), rot, new Vector2(tex.Width / 2f, (float)num108 / 2), new Vector2(scale, 1f), SpriteEffects.None, 0f);


					Lighting.AddLight(P.Center, color.R / 255f, color.G / 255f, color.B / 255f);

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
			//writer.Write(range);
			//writer.Write(distance);
			//writer.Write(BeamLength);
			writer.WriteVector2(targetPos);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			//range = reader.ReadSingle();
			//distance = reader.ReadSingle();
			//BeamLength = reader.ReadSingle();
			//targetPos = reader.ReadVector2();
			base.ReceiveExtraAI(reader);
		}
		public override void OnHitNPC(NPC target2, NPC.HitInfo hit, int damageDone)
		{
			Player O = Main.player[Projectile.owner];
			MPlayer mp = O.GetModPlayer<MPlayer>();
			int heal = (int)(damageDone / 10 * (mp.statCharge / MPlayer.maxCharge));// * (O.statLife / O.statLifeMax2));
			float minDamage = MConfigItems.Instance.minSpeedShockCoil;// + (Luminite? 1.0f : DiffBeam? 0.5f : 0f);
			float maxDamage = MConfigItems.Instance.maxSpeedShockCoil + (Luminite ? 1.0f : DiffBeam ? 0.5f : 0f);
			float ranges = maxDamage - minDamage;
			double damaage = Math.Clamp((mp.statCharge / MPlayer.maxCharge * ranges) + minDamage, minDamage, maxDamage);
			//float bonusShots = (mp.statCharge * (shots - 1) / MPlayer.maxCharge) + 1f;
			int immunity = (int)(O.HeldItem.useTime / (double)damaage); //(int)(O.HeldItem.useTime / bonusShots / (double)damaage);
																		//mp.statOverheat += mp.overheatCost; // /shots;
			mp.statCharge = Math.Min(mp.statCharge + (2.0f / shots), MPlayer.maxCharge);
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
	public class ShockCoilChargeShot : MProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 1.5f;
			//Projectile.timeLeft = 10;
			Projectile.extraUpdates = 0;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
		}
		public Projectile Lead;
		public Projectile child;
		public float distance;
		//public int link;
		public NPC target;
		private bool hastarget;
		private bool islinking = false;
		//int useTime = 0;
		public int dmg = 0;
		public int shotCounter= 1;
		//public override void OnSpawn(IEntitySource source)
		//{
		//	if (source is EntitySource_Parent parent && parent.Entity is Player player && (player.HeldItem.type == ModContent.ItemType<PowerBeam>() || player.HeldItem.type == ModContent.ItemType<ArmCannon>()))
		//	{
		//		if (player.HeldItem.ModItem is PowerBeam hold)
		//		{
		//			shot = hold.shotEffect.ToString();
		//		}
		//		else if (player.HeldItem.ModItem is ArmCannon hold2)
		//		{
		//			shot = hold2.shotEffect.ToString();
		//		}
		//	}
		//	base.OnSpawn(source);
		//}
		public override void AI()
		{
			Projectile P = Projectile;
			Player O = Main.player[P.owner];
			//Vector2 oPos = O.RotatedRelativePoint(O.MountedCenter, true);

			//Lead = Main.projectile[(int)P.ai[0]];
			if (!Lead.active||Lead ==null || Lead.owner != P.owner || !O.HeldItem.GetGlobalItem<MGlobalItem>().isBeam || !O.controlUseItem ||!target.active || target == null)
			{
				P.Kill();
				child?.Kill();
				return;
			}
			else
			{
				//target = Main.npc[link];
				P.Center = target.Center;
				if(O.controlUseItem)
					P.timeLeft = 5;
				//child = null;
				if (!islinking && P.owner == O.whoAmI && child == null)
				{
					int shotProj = Projectile.NewProjectile(P.GetSource_FromAI(), target.Center, Vector2.Zero, ModContent.ProjectileType<ShockCoilShot>(), dmg, 0, O.whoAmI);
					MProjectile mProj = (MProjectile)Main.projectile[shotProj].ModProjectile;
					mProj.assign = shotProj;
					mProj.shot = shot;
					child = Main.projectile[shotProj];
					if (mProj is ShockCoilShot shocky)
					{
						shocky.distance = ((mProjectile.waveDepth * 16f) + 32f) - distance;
						shocky.isaLink = true;
						shocky.parent = Main.projectile[assign];
						//shocky.mProjectile.shot = shot;
						shocky.shotCounter = shotCounter;
						Main.projectile[shotProj].netUpdate = true;
					}
					islinking = true;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		//public override void OnKill(int timeLeft)
		//{
		//	child?.Kill();
		//}
	}
}
