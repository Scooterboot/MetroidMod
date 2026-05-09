using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

		private Vector2[] chaintargetPos;
		//private Vector2 targetPos;
		private bool setTargetPos = false;
		private bool[] chainsetTargetPos;

		private Projectile Lead;

		//public NPC target;
		private NPC[] chainTarget;

		//private Vector2 oPos;
		private Vector2[] chainoPos;
		private Vector2 mousePos;

		private SoundEffectInstance soundInstance;
		private bool soundPlayed = false;
		private int soundDelay = 30;

		private int ampSyncCooldown = 20;
		private int shots=1;
		private int immuneTime = 0;
		private int dmg;

		private readonly float[] amp = new float[3];
		private readonly float[] ampDest = new float[3];
		//public float range;
		private float[] chainrange;
		private float distance;

		private float GetCharge()
		{
			return Luminite ? MConfigItems.Instance.damageLuminiteBeam : DiffBeam ? MConfigItems.Instance.damageChargeBeamV2 : MConfigItems.Instance.damageChargeBeam;
		}
		private static int GetDepth(MProjectile mp)
		{
			return mp.waveDepth;
		}
		public override void OnSpawn(IEntitySource source)
		{
			if (source is EntitySource_Parent parent && parent.Entity is Player player && player.HeldItem.ModItem is ArmCannon hold2)
			{
				shot = hold2.shotEffect.ToString();
				//shots = hold2.shotAmt;
				//chainLength = 1;
				//chainrange = new float[hold2.shotAmt].ToArray();
				//chaintargetPos = new Vector2[hold2.shotAmt].ToArray();
				//chainsetTargetPos = [.. Enumerable.Repeat(false, hold2.shotAmt)];
				//chainTarget = new NPC[Main.maxNPCs].ToArray();
				//chainoPos = new Vector2[hold2.shotAmt].ToArray();
				//Lead = Main.projectile[hold2.chargeLead];
				if (shot.Contains("red"))
				{
					shots = 2;
				}
				if (shot.Contains("green"))
				{
					shots = 6;
				}
				if (shot.Contains("nova"))
				{
					shots = 8;
				}
				if (shot.Contains("solar"))
				{
					shots = 12;
				}
				chainrange = new float[shots].ToArray();
				chaintargetPos = new Vector2[shots].ToArray();
				chainsetTargetPos = [.. Enumerable.Repeat(false, shots)];
				chainTarget = new NPC[Main.maxNPCs].ToArray();
				chainoPos = new Vector2[shots].ToArray();
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
			distance = (GetDepth(meep) * 16f) + 32f;
			//Vector2 V = P.velocity;
			P.knockBack = 0;
			//TODO: THIS IS THE CULPRIT IN MULTI
			float speed = Math.Max(8f, Vector2.Distance(chaintargetPos[0], P.Center) * 0.25f);
			float targetAngle = (float)Math.Atan2(chaintargetPos[0].Y - P.Center.Y, chaintargetPos[0].X - P.Center.X);
			P.velocity = targetAngle.ToRotationVector2() * speed;
			P.netUpdate = true;
			Lead = Main.projectile[O.heldProj];
			if (P.numUpdates == 0)
			{
				P.frame++;
				if (P.owner == Main.myPlayer && !O.dead)
				{
					for (int i = 0; i < shots; i++)
					{

						//range = (GetDepth(meep) * 16) + 32f;
						chainrange[i] = (GetDepth(meep) * 16f) + 32f;
						//chainLength = 1;
						//oPos = O.RotatedRelativePoint(O.MountedCenter, true);

						P.netUpdate = true;
						Vector2 diff = Main.MouseWorld - Lead.Center;
						diff.Normalize();
						chainoPos[0] = Lead.Center;
						if (i > 0)
						{
							chainoPos[i] = chaintargetPos[i - 1];
						}
						mousePos = chainoPos[i] + (diff * Math.Min(Vector2.Distance(chainoPos[i], Main.MouseWorld), chainrange[i]));
						//chainTarget[i] = null;
						//target = null;
						foreach (var npc in Main.ActiveNPCs)
						{
							//chainoPos[0] = Lead.Center;
							//if (i > 0)
							//{
							//	chainoPos[i] = chaintargetPos[i - 1];
							//}

							//NPC npc = Main.npc[who.whoAmI];
							//targeted[npc.whoAmI] = false;
							if (npc.lifeMax > 5 && !npc.dontTakeDamage && !npc.friendly)
							{
								Rectangle npcRect = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);

								float point = 0f;
								if (Vector2.Distance(chainoPos[i], npc.Center) < distance && Collision.CheckAABBvLineCollision(npcRect.TopLeft(), npcRect.Size(), chainoPos[i], chaintargetPos[i], P.width, ref point))
								{

									//if (npc != chainTarget[i])
									//{
									//	chainrange[i] = Vector2.Distance(chainoPos[i], npc.Center);
									//}

									chainrange[i] = Vector2.Distance(chainoPos[i], npc.Center);
									mousePos = chainoPos[i] + (diff * Math.Min(Vector2.Distance(chainoPos[i], Main.MouseWorld), chainrange[i]));
									//mousePos = Lead.Center + (diff * Math.Min(Vector2.Distance(Lead.Center, Main.MouseWorld), chainrange[0]));

								}
								bool flag = Vector2.Distance(chainoPos[i], npc.Center) <= chainrange[i] + distance && Vector2.Distance(npc.Center, mousePos) <= distance;

								if (npc.CanBeChasedBy(P, false))
								{
									if (chainTarget[i] == null || !chainTarget[i].active)
									{
										if (flag)
										{
											chainTarget[i] = npc;
										}
									}
									else
									{
										if (npc != chainTarget[i] && flag && Vector2.Distance(npc.Center, mousePos) < Vector2.Distance(chainTarget[i].Center, mousePos))
										{
											chainTarget[i] = npc;
										}

										else if (Vector2.Distance(chainoPos[i], chainTarget[i].Center) > chainrange[i] + distance || Vector2.Distance(chainTarget[i].Center, mousePos) > distance)
										{
											chainTarget[i] = null;
										}
									}
								}
							}
						}
						if (chainTarget[i] == null || !chainTarget[i].active)
						{
							chaintargetPos[i] = Lead.Center;
						}
						if (!chainsetTargetPos[i])
						{
							chaintargetPos[i] = chainoPos[i];
							chainsetTargetPos[i] = true;
							return;
						}
						else if (chainTarget[i] != null && chainTarget[i].active)
						{
							chaintargetPos[i] = chainTarget[i].Center;
						}
						else
						{
							mp.statCharge = 0;
							chaintargetPos[i] = chainoPos[i]; // Lead.Center + (diff * chainrange[0]);
						}

						if (soundDelay <= 0)
						{
							if (!soundPlayed)
							{
								SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilSound, O.position), out ActiveSound result);
								soundInstance = result.Sound;
								soundPlayed = true;
								soundDelay = 50 * shots;
							}
							if (mp.statCharge >= MPlayer.maxCharge && mp.statOverheat < mp.maxOverheat)
							{
								SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilAffinity2, O.position), out ActiveSound result);
								soundInstance = result.Sound;
								soundDelay = 40 * shots;
							}

							else
							{
								soundInstance?.Stop(true);
								SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilSound, O.position), out ActiveSound result);
								soundInstance = result.Sound;
								soundDelay = 40 * shots;
							}
						}
						else
						{
							soundDelay--;
						}
						for (int j = 0; j < 3; j++)
						{
							ampDest[j] = Main.rand.Next(-15, 16);
						}

						if (ampSyncCooldown-- <= 0)
						{
							ampSyncCooldown = 20;
						}
					}

				}
			}
			if (P.frame >= 12)
			{
				mp.statCharge = Math.Min(mp.statCharge + (GetCharge()/shots), MPlayer.maxCharge);
				P.frame = 0;
			}
			//range = Math.Min(GetDepth(meep), Max_Range);
			//distance = Math.Min(GetDepth(meep), Max_Distance);
			if (immuneTime > 0)
			{
				P.damage = 0;
				immuneTime--;
			}
			else
			{
				float multiplier = (mp.statCharge + (GetCharge()*10f)) / MPlayer.maxCharge;
				P.damage = (int)(multiplier * dmg);
			}
			mProjectile.WaveBehavior(P);


			
			if (O.controlUseItem)
			{
				P.timeLeft = 5;
			}
			else
			{
				P.Kill();
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
			if (mp.statOverheat >= mp.maxOverheat || O.HeldItem.GetGlobalItem<MGlobalItem>().statUA <= 0 || O.dead)//O.HeldItem.GetGlobalItem<MGlobalItem>().addonUACost)
			{
				P.Kill();
				mp.statCharge = 0f;
				if (!O.dead)
					SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilReload, O.position);
			}
		}
		public override bool? CanHitNPC(NPC target)
		{
			for (int i = 0; i < shots; i++)
			{
				if (chainTarget[i] == target)
				{
					if ((!shot.Contains("wave") && !shot.Contains("nebula") && !Collision.CanHitLine(chainoPos[i], Projectile.width, Projectile.height, chaintargetPos[i], Projectile.width, Projectile.height)))
					{
						return false;
					}
					return true;
				}
			}
			return false;
		}
		public override void CutTiles()
		{
			Player p = Main.player[Projectile.owner];
			if (p.controlUseItem)
			{
				for (int i = 0; i < shots; i++)
				{
					DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
					Utils.PlotTileLine(chainoPos[i], chaintargetPos[i], Projectile.width, DelegateMethods.CutTiles);
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Player p = Main.player[Projectile.owner];
			for (int i = 0; i < shots; i++)
			{
				float point = 0f;
				if((projHitbox.Intersects(targetHitbox) ||Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), chainoPos[i], chaintargetPos[i], Projectile.width, ref point))&&p.controlUseItem)
				{
					return true;
				}
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteBatch sb = Main.spriteBatch;
			Projectile P = Projectile;
			//mProjectile meep = mProjectile;
			Color color = MetroidMod.powColor;
			Player O = Main.player[P.owner];
			Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[P.type].Value;
			int num108 = tex.Height / Main.projFrames[P.type];
			int y4 = num108 * P.frame;
			//oPos = O.RotatedRelativePoint(O.MountedCenter, true);
			Lead = Main.projectile[O.heldProj];
			P.scale = .8f;
			if (O.controlUseItem && !O.dead)
			{
				for (int j = 0; j < shots; j++)
				{
					if (j == 0)
					{
						chainoPos[0] = Lead.Center;
					}
					else
					{
						chainoPos[j] = chaintargetPos[j - 1];
					}
					float targetrot = (float)Math.Atan2(chaintargetPos[j].Y - chainoPos[j].Y, chaintargetPos[j].X - chainoPos[j].X);
					float dist = Math.Max(Vector2.Distance(chainoPos[j], chaintargetPos[j]), 1);

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

						pos[i] = chainoPos[j] + (targetrot.ToRotationVector2() * (dist / num) * i);
						pos[i].X += (float)Math.Cos(trot) * shift * (Vector2.Distance(chainoPos[j], chaintargetPos[j]) / chainrange[j]);//make sum
						pos[i].Y += (float)Math.Sin(trot) * shift * (Vector2.Distance(chainoPos[j], chaintargetPos[j]) / chainrange[j]);

						float rot = (float)Math.Atan2(pos[i].Y - chainoPos[j].Y, pos[i].X - chainoPos[j].X + ((float)Math.PI / 2));
						if (i > 0)
						{
							rot = (float)Math.Atan2(pos[i].Y - pos[i - 1].Y, pos[i].X - pos[i - 1].X) + ((float)Math.PI / 2);
						}
						sb.Draw(tex, pos[i] - Main.screenPosition, new Rectangle?(new Rectangle(0, y4, tex.Width, num108)), P.GetAlpha(Color.White), rot, new Vector2(tex.Width / 2f, (float)num108 / 2), new Vector2(scale, 1f), SpriteEffects.None, 0f);


						Lighting.AddLight(P.Center, color.R / 255f, color.G / 255f, color.B / 255f);
					}
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
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			base.ReceiveExtraAI(reader);
		}
		public override void OnHitNPC(NPC target2, NPC.HitInfo hit, int damageDone)
		{
			Player O = Main.player[Projectile.owner];
			MPlayer mp = O.GetModPlayer<MPlayer>();
			int heal = (int)(damageDone / 10 * (mp.statCharge / MPlayer.maxCharge));// * (O.statLife / O.statLifeMax2));
			float minDamage = GetCharge() / 5f;// MConfigItems.Instance.minSpeedShockCoil;// + (Luminite? 1.0f : DiffBeam? 0.5f : 0f);
			float maxDamage = GetCharge()/2f;//MConfigItems.Instance.maxSpeedShockCoil;
			float ranges = maxDamage - minDamage;
			double damaage = (double)Math.Clamp((mp.statCharge / MPlayer.maxCharge * ranges) + minDamage, minDamage, maxDamage);
			//float bonusShots = (mp.statCharge * (shots - 1) / MPlayer.maxCharge) + 1f;
			int immunity = (int)(O.HeldItem.useTime / (double)damaage); //(int)(O.HeldItem.useTime / bonusShots / (double)damaage);
																		//mp.statOverheat += mp.overheatCost; // /shots;
			//mp.statCharge =Math.Min(mp.statCharge++, MPlayer.maxCharge);
			if (!mp.PrimeHunter && (Luminite || DiffBeam))
			{
				mp.Energy = Math.Min(mp.Energy += heal, mp.MaxEnergy);
			}
			SoundEngine.PlaySound(Sounds.Items.Weapons.ShockCoilAffinity1, target2.Center);
			if (damageDone > 0)
			{
				immuneTime = 4* immunity;
				//Projectile.localNPCHitCooldown = immunity;
				target2.immune[O.whoAmI] = immunity;
				/*foreach (NPC G in Main.npc)
				{
					//G.immune[O.whoAmI] = (int)(O.HeldItem.useTime / bonusShots / (double)damaage);
					Projectile.localNPCHitCooldown = (int)(O.HeldItem.useTime / bonusShots / (double)damaage);
				}*/
			}
			else
			{
				immuneTime = 0;
			}
				base.OnHitNPC(target2, hit, damageDone);
		}
	}
}
