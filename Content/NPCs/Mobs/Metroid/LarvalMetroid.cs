using System;
using System.IO;
using MetroidMod.Common.Configs;
using MetroidMod.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.Utilities;

namespace MetroidMod.Content.NPCs.Mobs.Metroid
{
	public class LarvalMetroid : MNPC
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.UsesNewTargetting[Type] = true;

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()  //Alright so this here method thingy lets you tweak the bestiary display
			{
				CustomTexturePath = $"{nameof(MetroidMod)}/Content/NPCs/Mobs/Metroid/LarvalMetroid",
				Position = new Vector2(0f, 0f), // these two variables ONLY APPLY TO THE LIST TILES
				Scale = 1f,
				PortraitPositionXOverride = 0f,
				PortraitPositionYOverride = 0f,
				PortraitScale = 1f // Portrait refers to the full picture when clicking on the icon in the bestiary
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
		}
		public override void SetDefaults()
		{
			NPC.width = 64;
			NPC.height = 46;
			NPC.damage = 30;
			NPC.defense = 400;
			NPC.lifeMax = 320;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(0, 0, 19, 86);
			NPC.knockBackResist = 1f;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
			//banner = npc.type;
			//bannerItem = mod.ItemType("MetroidBanner");

		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (MConfigMain.Instance.disablemobspawn == true)
			{
				return 0f;
			}
			float chance = 0;
			if (Main.hardMode)
			{
				chance = SpawnCondition.Dungeon.Chance * 0.05f;
			}
			return chance;
		}
		public override bool? CanFallThroughPlatforms()
		{
			return true;
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			if (STATE == (int)StateID.Aggroed)
			{
				STATE = (int)StateID.Sucking;
				NPC.target = target.whoAmI;
				KnockoffScale = 0;
				NPC.netUpdate = true;
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit)
		{
			if (STATE == (int)StateID.Aggroed)
			{
				STATE = (int)StateID.Sucking;
				NPC.target = target.WhoAmIToTargettingIndex;
				KnockoffScale = 0;
				NPC.netUpdate = true;
			}
		}
		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			modifiers.Knockback *= 0;
		}
		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			if (target.iceBarrier || target.frostBurn) return false;
			if (STATE == (int)StateID.Sucking || STATE == (int)StateID.Frozen) return false;
			return base.CanHitPlayer(target, ref cooldownSlot);
		}
		public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
		{
			npcHitbox.Height += 18;
			return base.ModifyCollisionData(victimHitbox, ref immunityCooldownSlot, ref damageMultiplier, ref npcHitbox);
		}
		public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			if (STATE == (int)StateID.Sucking && hit.Knockback > 0)
			{
				KnockoffScale += hit.Knockback * 1.5f;
			}
		}
		public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (STATE == (int)StateID.Sucking)
			{
				if (hit.Knockback > 0)
				{
					KnockoffScale += hit.Knockback;
					if (Main.player[projectile.owner].heldProj == projectile.whoAmI)
					{
						KnockoffScale += hit.Knockback * 0.5f;
					}
				}
				if (projectile.coldDamage)
				{
					KnockoffScale += 2;
				}

				if (projectile.type == ((ModMBWeapon)MBAddonLoader.GetAddon<MorphBallAddons.Bomb>()).ProjectileType || projectile.type == ((ModMBSpecial)MBAddonLoader.GetAddon<MorphBallAddons.PowerBomb>()).ProjectileType)
				{
					KnockoffScale += 8;
				}
			}
		}
		public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if (STATE == (int)StateID.Frozen)
			{
				modifiers.Knockback *= 0.3f;
				modifiers.ScalingArmorPenetration += 1f;
			}
			else
			{
				float DR = 0.5f;
				if (NPC.HasBuff(BuffID.Frostburn) || NPC.HasBuff(BuffID.Frostburn2))
				{
					DR -= 0.25f;
					modifiers.ScalingArmorPenetration += 1f;
				}
				if (item.type == ItemID.IceBlade || item.type == ItemID.Frostbrand)
				{
					KnockoffScale += 4;
					modifiers.ScalingArmorPenetration += 1f;
				}
				modifiers.FinalDamage *= 1 - DR;
			}
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (STATE == (int)StateID.Frozen || (projectile.Name.Contains("Missile") && projectile.Name.Contains("Stardust")))
			{
				modifiers.Knockback *= 0.3f;
				modifiers.ScalingArmorPenetration += 1f;
				if (projectile.Name.Contains("Missile") || projectile.Name.Contains("Spreader") || projectile.Name.Contains("Combo"))
				{
					modifiers.SetCrit();
					modifiers.DamageVariationScale *= 0;
				}
			}
			else
			{
				float DR = 0.5f;
				if (NPC.HasBuff(BuffID.Frostburn) || NPC.HasBuff(BuffID.Frostburn2))
				{
					DR -= 0.25f;
					modifiers.ScalingArmorPenetration += 1f;
				}
				if (projectile.coldDamage)
				{
					if (projectile.knockBack < 1.5f)
					{
						projectile.knockBack = 1.5f;
					}
					KnockoffScale += 4;
					modifiers.ScalingArmorPenetration += 1f;
				}
				modifiers.FinalDamage *= 1 - DR;
			}
		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (projectile.hostile && projectile.coldDamage)
			{
				projectile.usesLocalNPCImmunity = true;
				projectile.localNPCHitCooldown = 20;
				return true;
			}
			return base.CanBeHitByProjectile(projectile);
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
					// Sets the spawning conditions of this NPC that is listed in the bestiary.
					//BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
					//BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
					BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,

					// Sets the description of this NPC that is listed in the bestiary.
					new FlavorTextBestiaryInfoElement("Mods.MetroidMod.Bestiary.Metroid")
				});
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < 20; i++)
				{
					Dust d = Dust.NewDustDirect(NPC.position, NPC.height, NPC.height, DustID.t_Slime, 0, 0, 120, Color.LimeGreen, 1.5f);
					d.velocity = NPC.DirectionTo(d.position) * 3;
				}
				Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center + new Vector2(10, 0), NPC.velocity, Mod.Find<ModGore>("MetroidGore1").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center + new Vector2(-10, 0), NPC.velocity, Mod.Find<ModGore>("MetroidGore1").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center + new Vector2(0, -10), NPC.velocity, Mod.Find<ModGore>("MetroidGore1").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center + new Vector2(10, 10), NPC.velocity, Mod.Find<ModGore>("MetroidGore2").Type, NPC.scale);
				Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center + new Vector2(10, 10), NPC.velocity, Mod.Find<ModGore>("MetroidGore2").Type, NPC.scale);
			}
		}

		private float STATE
		{
			get { return NPC.ai[0]; }
			set { NPC.ai[0] = value; }
		}
		private float AI_Counter
		{
			get { return NPC.ai[1]; }
			set { NPC.ai[1] = value; }
		}
		private float KnockoffScale
		{
			get { return NPC.ai[2]; }
			set { NPC.ai[2] = value; }
		}
		private enum StateID : int
		{
			Idle,
			Aggroed,
			Sucking,
			Frozen
		}

		public override void AI()
		{
			if (NPC.noTileCollide && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
			{
				NPC.noTileCollide = false;
			}
			if (NPC.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().froze)
			{
				STATE = (int)StateID.Frozen;
				NPC.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().speedDecrease = 0;
				NPC.netUpdate = true;
			}
			if (STATE == (int)StateID.Idle)
			{
				AI_Counter++;
				if (NPC.velocity.X == 0)
				{
					NPC.direction *= -1;
					NPC.velocity.X = NPC.direction;
					AI_Counter = 120;
				}
				if (NPC.velocity.Y == 0)
				{
					NPC.directionY *= -1;
					NPC.velocity.Y = NPC.directionY;
					NPC.targetRect = new Rectangle((int)NPC.Center.X + 250 * NPC.direction, (int)NPC.Center.Y + 120 * NPC.directionY, 1, 1);
				}
				if (AI_Counter > 120)
				{
					AI_Counter = 0;
					NPC.targetRect = new Rectangle((int)NPC.Center.X + 250 * NPC.direction, (int)NPC.Center.Y - 120 * NPC.directionY, 1, 1);
				}
				else
				{
					NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
					NPC.directionY = NPC.velocity.Y > 0 ? 1 : -1;
				}


				Vector2 targetPos = new Vector2(NPC.targetRect.X, NPC.targetRect.Y);
				//Dust.NewDustPerfect(targetPos, DustID.BlueFairy, Vector2.Zero);
				float speed = 4;
				Vector2 move = NPC.DirectionTo(targetPos) * speed;
				float home = 48f;
				NPC.velocity = ((home - 1f) * NPC.velocity + move) / home;

				NPCUtils.TargetSearchResults results = NPCUtils.SearchForTarget(NPC, NPCUtils.TargetSearchFlag.All, 
					(Player p) => p.Distance(NPC.Center) < 600 && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, p.position, p.width, p.height) 
					&& ! p.buffImmune[ModContent.BuffType<MetroidSucc>()] && !p.HasBuff<MetroidSucc>() && !p.HasBuff(BuffID.Frozen), 
					(NPC n) => !n.TypeName.Contains("Metroid") && !n.dontTakeDamage && !n.immortal && Collision.CanHitLine(NPC.position, NPC.width, NPC.height, n.position, n.width, n.height) &&
					!n.buffImmune[ModContent.BuffType<MetroidSucc>()] && !n.HasBuff<MetroidSucc>() && !n.HasBuff<IceFreeze>() && !n.HasBuff<InstantFreeze>() && !n.coldDamage);
				if (results.FoundTarget)
				{
					Rectangle r = results.NearestTargetHitbox;
					Vector2 tPos = new Vector2(r.Center.X, r.Center.Y);
					NPC.target = results.NearestTargetIndex;
					NPC.targetRect = r;
					STATE = (int)StateID.Aggroed;
					AI_Counter = 0;
					NPC.netUpdate = true;
					//Dust.NewDustPerfect(tPos, DustID.GreenFairy, Vector2.Zero);
				}
			}
			if (STATE == (int)StateID.Aggroed)
			{
				if (!NPC.HasValidTarget)
				{
					STATE = (int)StateID.Idle;
				}
				else
				{
					float speed = 7;
					if (NPC.HasPlayerTarget)
					{
						Player p = Main.player[NPC.target];
						NPC.targetRect = p.Hitbox;
						if (p.iceBarrier || p.frostBurn)
						{
							speed = -7;
						}
					}
					else if (NPC.HasNPCTarget)
					{
						NPC.targetRect = Main.npc[NPC.TranslatedTargetIndex].Hitbox;
						int num = 0;
						float num2 = 0;
						Rectangle hitbox = NPC.Hitbox;
						NPC.GetMeleeCollisionData(NPC.targetRect, NPC.whoAmI, ref num, ref num2, ref hitbox);
						if (hitbox.Intersects(NPC.targetRect))
						{
							STATE = (int)StateID.Sucking;
							KnockoffScale = 0;
							NPC.netUpdate = true;
						}
					}
					else
					{
						STATE = (int)StateID.Idle;
					}
					Vector2 targetPos = new Vector2(NPC.targetRect.Center.X, NPC.targetRect.Y - 20);
					if (Collision.SolidCollision(NPC.targetRect.TopLeft() + new Vector2(0, -20), NPC.targetRect.Width, 20))
					{
						targetPos = NPC.targetRect.Center();
					}
					if (!Collision.CanHitLine(NPC.position, NPC.width, NPC.height, targetPos, 0, 0))
					{
						AI_Counter++;
						//Dust.NewDustPerfect(targetPos, DustID.RedTorch, Vector2.Zero);
					}
					else if (AI_Counter > 0)
					{
						AI_Counter--;
					}
					if (AI_Counter > 120)
					{
						STATE = (int)StateID.Idle;
						AI_Counter = 0;
					}
					Vector2 move = NPC.DirectionTo(targetPos) * speed;
					float home = 32f;
					NPC.velocity = ((home - 1f) * NPC.velocity + move) / home;
				}
			}
			if (STATE == (int)StateID.Sucking)
			{
				if (!NPC.HasValidTarget)
				{
					STATE = (int)StateID.Idle;
					NPC.netUpdate = true;
				}
				else
				{
					float targetSpeed = 0;
					if (NPC.HasPlayerTarget)
					{
						Player p = Main.player[NPC.target];
						NPC.targetRect = p.Hitbox;
						p.AddBuff(ModContent.BuffType<MetroidSucc>(), 2);
						targetSpeed = p.velocity.Length();
						if (p.HasBuff(BuffID.Frostburn) || p.HasBuff(BuffID.Frostburn2) || p.HasBuff(BuffID.Frozen) || p.iceBarrier || p.frostBurn)
						{
							STATE = (int)StateID.Idle;
							KnockoffScale += 20;
							int dmg = 250;
							if (p.HasBuff(BuffID.Frostburn))
							{
								int b = p.FindBuffIndex(BuffID.Frostburn);
								NPC.AddBuff(BuffID.Frostburn, p.buffTime[b]);
								dmg += 50;
								p.DelBuff(b);
							}
							if (p.HasBuff(BuffID.Frostburn2))
							{
								int b = p.FindBuffIndex(BuffID.Frostburn2);
								NPC.AddBuff(BuffID.Frostburn2, p.buffTime[b]);
								p.DelBuff(b);
								dmg += 50;
							}
							NPC.SimpleStrikeNPC(dmg, p.direction, true, 10);
						}
					}
					else if (NPC.HasNPCTarget)
					{
						NPC n = Main.npc[NPC.TranslatedTargetIndex];
						NPC.targetRect = n.Hitbox;
						n.AddBuff(ModContent.BuffType<MetroidSucc>(), 2);
						if (n.HasBuff<IceFreeze>() || n.HasBuff<InstantFreeze>() || n.HasBuff(BuffID.Frostburn) || n.HasBuff(BuffID.Frostburn2) || n.coldDamage)
						{
							STATE = (int)StateID.Idle;
							KnockoffScale += 10;
							NPC.SimpleStrikeNPC(300, n.direction, true, 6);
							if (n.HasBuff(BuffID.Frostburn))
							{
								int b = n.FindBuffIndex(BuffID.Frostburn);
								NPC.AddBuff(BuffID.Frostburn, n.buffTime[b]);
								n.DelBuff(b);
							}
							if (n.HasBuff(BuffID.Frostburn2))
							{
								int b = n.FindBuffIndex(BuffID.Frostburn2);
								NPC.AddBuff(BuffID.Frostburn2, n.buffTime[b]);
								n.DelBuff(b);
							}
						}
					}
					else
					{
						STATE = (int)StateID.Idle;
						NPC.netUpdate = true;
					}
					int num = 0;
					float num2 = 0;
					Rectangle hitbox = NPC.Hitbox;
					NPC.GetMeleeCollisionData(NPC.targetRect, NPC.whoAmI, ref num, ref num2, ref hitbox);
					if (!hitbox.Intersects(NPC.targetRect))
					{
						STATE = (int)StateID.Aggroed;
						NPC.netUpdate = true;
					}
					else
					{
						Vector2 targetPos = new Vector2(NPC.targetRect.Center.X, NPC.targetRect.Y - 20);
						float speed = Math.Min(8 + targetSpeed, NPC.Distance(targetPos));
						Vector2 move = NPC.DirectionTo(targetPos) * speed;
						float home = Math.Max(1f, KnockoffScale);
						if (NPC.Distance(targetPos) <= speed && KnockoffScale <= 1)
						{
							NPC.velocity = targetPos - NPC.Center;
						}
						else if (speed > 0)
						{
							NPC.velocity = ((home - 1f) * NPC.velocity + move) / home;
						}
						if (NPC.life < NPC.lifeMax)
						{
							NPC.life++;
						}
						NPC.noTileCollide = true;
					}

				}
			}
			if (KnockoffScale > 1)
			{
				KnockoffScale -= 0.25f;
			}
			if (STATE == (int)StateID.Frozen)
			{
				NPC.noTileCollide = false;
				NPC.noGravity = false;
				if (NPC.velocity.Y == 0)
				{
					NPC.velocity.X *= 0.96f;
				}
				if (!NPC.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().froze)
				{
					STATE = (int)StateID.Idle;
					NPC.netUpdate = true;
				}
			}
			else
			{
				NPC.noGravity = true;
			}
			Point tilePos = NPC.position.ToTileCoordinates();
			if (ColdZoneCheck(tilePos.X - 8, tilePos.X + NPC.width / 16 + 8, tilePos.Y - 8, tilePos.Y + NPC.height / 16 + 8) > 50)
			{
				NPC.AddBuff(BuffID.Frostburn, 300);
			}

		}
		private static int ColdZoneCheck(int startX, int endX, int startY, int endY)
		{
			if (startX < 0)
				startX = 0;

			if (endX >= Main.maxTilesX)
				endX = Main.maxTilesX;

			if (startY < 0)
				startY = 0;

			if (endY >= Main.maxTilesY)
				endY = Main.maxTilesY;

			int num = 0;
			for (int i = startX; i < endX + 1; i++)
			{
				for (int j = startY; j < endY + 1; j++)
				{
					//Dust.NewDustPerfect(new Vector2(i * 16 + 8, j * 16 + 8), DustID.IceTorch, Vector2.Zero).noGravity = true;
					if (Main.tile[i, j] == null)
						return num;

					if (Main.tile[i, j].HasTile && TileID.Sets.IcesSnow[Main.tile[i, j].TileType])
						num++;
				}
			}

			return num;
		}

		private int teethFrameCounter = 0;
		private int teethFrame = 0;
		private int elecFrameCounter = 0;
		private int elecFrame = 0;
		private int shellAnimCounter = 0;
		public override void FindFrame(int frameHeight)
		{
			int frameWidth = 64;
			frameHeight = 46;
			if (STATE == (int)StateID.Frozen)
			{
				elecFrameCounter = 0;
				NPC.rotation = 0;
			}
			else
			{
				NPC.rotation = NPC.velocity.X * 0.025f;
				NPC.frameCounter++;
				teethFrameCounter++;
				elecFrameCounter++;
				if (teethFrame >= frameHeight && teethFrame <= frameHeight * 3)
				{
					shellAnimCounter++;
				}
				else
				{
					shellAnimCounter--;
				}
			}
			if (NPC.frameCounter >= 20)
			{
				NPC.frameCounter -= 7;
				NPC.frame.Y += frameHeight;
				if (NPC.frame.Y >= frameHeight * 4)
				{
					NPC.frame.Y = 0;
					NPC.frameCounter = 0;
				}
			}

			if (teethFrameCounter >= (STATE == (int)StateID.Sucking ? 5 : 7))
			{
				teethFrame += frameHeight;
				if (teethFrame >= frameHeight * 6)
				{
					teethFrame = 0;
				}
				teethFrameCounter = 0;
			}

			if (STATE == (int)StateID.Sucking)
			{
				NPC.frameCounter++;
				NPC.frame.X = frameWidth;
				if (elecFrameCounter <= 5 || elecFrameCounter > 22)
				{
					NPC.frame.X = 0;
				}
				if (elecFrameCounter > 10 && elecFrameCounter <= 17)
				{
					NPC.frame.X = frameWidth * 2;
				}
				if (elecFrameCounter % 4 == 0)
				{
					elecFrame += frameHeight;
					if (elecFrame >= frameHeight * 4)
					{
						elecFrame = 0;
					}
				}
				if (elecFrameCounter > 25)
				{
					elecFrameCounter = 0;
				}
			}
			else if (elecFrameCounter > 100)
			{
				NPC.frame.X = frameWidth;
				if (elecFrameCounter > 105 && elecFrameCounter <= 135)
				{
					NPC.frame.X = frameWidth * 2;
					if (elecFrameCounter % 4 == 0)
					{
						elecFrame += frameHeight;
						if (elecFrame >= frameHeight * 4)
						{
							elecFrame = 0;
						}
					}
				}
				if (elecFrameCounter > 140)
				{
					elecFrameCounter = 0;
				}
			}
			else
			{
				NPC.frame.X = 0;
			}

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;

			Texture2D texTeeth = (Texture2D)ModContent.Request<Texture2D>($"{Texture}_Teeth");
			Texture2D texInner = (Texture2D)ModContent.Request<Texture2D>($"{Texture}_Inner");
			Texture2D texGlow = (Texture2D)ModContent.Request<Texture2D>($"{Texture}_Glow");
			Texture2D texElec = (Texture2D)ModContent.Request<Texture2D>($"{Texture}_Electricity");
			Texture2D texOuter = (Texture2D)ModContent.Request<Texture2D>($"{Texture}_Outer");

			int frameHeightTeeth = texTeeth.Height / 6;
			int frameHeightInner = texInner.Height / 4;
			int frameHeightElec = texElec.Height / 4;
			int frameHeightOuter = texOuter.Height;

			Rectangle rectTeeth = new Rectangle(0, teethFrame, texTeeth.Width, texTeeth.Height / 6);
			Rectangle rectInner = new Rectangle(NPC.frame.X, NPC.frame.Y, texInner.Width / 3, texInner.Height / 4);
			Rectangle rectElec = new Rectangle(0, elecFrame, texElec.Width, texElec.Height / 4);
			Rectangle rectOuter = new Rectangle(0, 0, texOuter.Width, texOuter.Height);

			Vector2 originTeeth = new Vector2(texTeeth.Width * 0.5f, frameHeightTeeth * 0.5f);
			Vector2 originInner = new Vector2(texInner.Width / 3 * 0.5f, frameHeightInner * 0.5f);
			Vector2 originElec = new Vector2(texElec.Width * 0.5f, frameHeightElec * 0.5f);
			Vector2 originOuter = new Vector2(texOuter.Width * 0.5f, frameHeightOuter * 0.5f);

			Vector2 shellScale = new Vector2(1f + (Math.Abs(shellAnimCounter) * 0.003f), 1f - (Math.Abs(shellAnimCounter) * 0.002f));

			if (STATE == (int)StateID.Frozen)
			{
				drawColor = Lighting.GetColor(NPC.Center.ToTileCoordinates());
			}
			Vector2 teethOffset = new Vector2(0, 16).RotatedBy(NPC.rotation);
			DrawData teethData = new DrawData(texTeeth, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - texTeeth.Width * 0.5f + originTeeth.X, NPC.position.Y - Main.screenPosition.Y + NPC.height - frameHeightTeeth + originTeeth.Y) + teethOffset, new Rectangle?(rectTeeth), drawColor, NPC.rotation, originTeeth, shellScale * NPC.scale, effects, 0f);
			DrawData innerData = new DrawData(texInner, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - texInner.Width / 3 * 0.5f + originInner.X, NPC.position.Y - Main.screenPosition.Y + NPC.height - frameHeightInner + originInner.Y), new Rectangle?(rectInner), drawColor, NPC.rotation, originInner, NPC.scale, effects, 0f);
			DrawData glowData = new DrawData(texGlow, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - texInner.Width / 3 * 0.5f + originInner.X, NPC.position.Y - Main.screenPosition.Y + NPC.height - frameHeightInner + originInner.Y), new Rectangle?(rectInner), Color.White * 0.25f, NPC.rotation, originInner, NPC.scale, effects, 0f);
			DrawData elecData = new DrawData(texElec, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - texElec.Width * 0.5f + originElec.X, NPC.position.Y - Main.screenPosition.Y + NPC.height - frameHeightElec + originElec.Y), new Rectangle?(rectElec), Color.White, NPC.rotation, originElec, NPC.scale, effects, 0f);
			DrawData outerData = new DrawData(texOuter, new Vector2(NPC.position.X - Main.screenPosition.X + NPC.width / 2 - texOuter.Width * 0.5f + originOuter.X, NPC.position.Y - Main.screenPosition.Y + NPC.height - frameHeightOuter + originOuter.Y), new Rectangle?(rectOuter), drawColor * 0.6f, NPC.rotation, originOuter, shellScale * NPC.scale, effects, 0f);


			if (STATE == (int)StateID.Frozen)
			{
				outerData.color = drawColor;
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);

				MiscShaderData shaderData = GameShaders.Misc["MetroidModDualTint"];
				shaderData.UseColor(new Color(0f, 0.714f, 1f));
				shaderData.UseSecondaryColor(new Color(0f, 0.286f, 1f));
				shaderData.UseOpacity(0.2f);
				shaderData.UseSaturation(1f);
				shaderData.UseImage0(TextureAssets.Npc[NPC.type]);

				shaderData.Apply(teethData);
				shaderData.Apply(innerData);
				shaderData.Apply(outerData);
			}

			teethData.Draw(spriteBatch);
			innerData.Draw(spriteBatch);
			glowData.Draw(spriteBatch);
			if (elecFrameCounter > 100 || STATE == (int)StateID.Sucking)
				elecData.Draw(spriteBatch);

			outerData.Draw(spriteBatch);

			if (STATE == (int)StateID.Frozen)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.Transform);
			}
			return false;
		}
	}


	// Old Code
	//public class LarvalMetroid : MNPC
	//{
	//	private float newScale = -1;
	//	public float movingSpeed = 0;
	//	public bool movingUp = false;
	//	public bool grappled = false;
	//	public bool frozen = false;
	//	public bool spawn = false;

	//	public override void SetStaticDefaults()
	//	{
	//		// DisplayName.SetDefault("Larval Metroid");
	//		Main.npcFrameCount[Type] = 4;
	//		NPCID.Sets.MPAllowedEnemies[Type] = true;

	//		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
	//		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
	//		NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
	//	}

	//	public override void SetDefaults()
	//	{
	//		NPC.width = 38;
	//		NPC.height = 38;
	//		NPC.damage = 20;
	//		NPC.defense = 23;
	//		NPC.lifeMax = 100;
	//		NPC.HitSound = SoundID.NPCHit1;
	//		NPC.DeathSound = SoundID.NPCDeath1;
	//		NPC.noGravity = true;
	//		NPC.value = Item.buyPrice(0, 0, 1, 60);
	//		NPC.knockBackResist = 0.75f;
	//		NPC.aiStyle = -1;
	//		NPC.npcSlots = 1;
	//		//banner = npc.type;
	//		//bannerItem = mod.ItemType("MetroidBanner");

	//		/* NPC scale networking fix. */
	//		if (Main.rand != null && Main.netMode != NetmodeID.MultiplayerClient)
	//			newScale = (Main.rand.Next(5, 10) * 0.1f);
	//	}

	//	public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	//	{
	//		// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
	//		bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
	//			// Sets the spawning conditions of this NPC that is listed in the bestiary.
	//			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
	//			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
	//			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,

	//			// Sets the description of this NPC that is listed in the bestiary.
	//			new FlavorTextBestiaryInfoElement("Mods.MetroidMod.Bestiary.Metroid")
	//		});
	//	}
	//	public override float SpawnChance(NPCSpawnInfo spawnInfo)
	//	{
	//		if (MConfigMain.Instance.disablemobspawn == true)
	//		{
	//			return 0f;
	//		}
	//		if (Main.hardMode || NPC.downedBoss2)
	//		{
	//			float chance1 = 0.03f;
	//			float chance2 = 0.5f;
	//			if (Main.hardMode)
	//			{
	//				chance1 = 0.5f;
	//				chance2 = 0.75f;
	//			}
	//			return (SpawnCondition.Corruption.Chance + SpawnCondition.Crimson.Chance) * chance1 + SpawnCondition.DungeonNormal.Chance * chance2;
	//		}
	//		return SpawnCondition.DungeonNormal.Chance * 0.5f;
	//	}

	//	public override bool PreAI()
	//	{
	//		if (!spawn && newScale != -1)
	//		{
	//			SetStats();
	//			spawn = true;
	//			NPC.netUpdate = true;
	//		}
	//		return true;
	//	}
	//	public override void AI()
	//	{
	//		frozen = NPC.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().froze;
	//		if (grappled)
	//		{
	//			if (Main.player[NPC.target].dead || !Main.player[NPC.target].active || frozen)
	//			{
	//				grappled = false;
	//				return;
	//			}
	//			NPC.rotation = 0;
	//			NPC.position = new Vector2(Main.player[NPC.target].Center.X - (NPC.width / 2), Main.player[NPC.target].Center.Y - (NPC.height / 2) - 16);
	//			Main.player[NPC.target].velocity.X *= 0.95f;
	//		}
	//		else if (!frozen)
	//		{
	//			NPC.TargetClosest();

	//			if (Main.player[NPC.target].Center.X < NPC.Center.X)
	//			{
	//				if (NPC.velocity.X > -2) { NPC.velocity.X -= 0.2f; }
	//			}
	//			else if (Main.player[NPC.target].Center.X > NPC.Center.X)
	//			{
	//				if (NPC.velocity.X < 2) { NPC.velocity.X += 0.2f; }
	//			}
	//			if (Main.player[NPC.target].Center.Y < NPC.Center.Y)
	//			{
	//				if (NPC.velocity.Y > -2) NPC.velocity.Y -= 0.2f;
	//			}
	//			else if (Main.player[NPC.target].Center.Y > NPC.Center.Y)
	//			{
	//				if (NPC.velocity.Y < 2) NPC.velocity.Y += 0.2f;
	//			}

	//			if (movingUp)
	//			{
	//				movingSpeed -= 0.02f;
	//			}
	//			else
	//			{
	//				movingSpeed += 0.02f;
	//			}
	//			if (movingSpeed <= -0.20f)
	//			{
	//				movingUp = false;
	//			}
	//			if (movingSpeed >= 0.20f)
	//			{
	//				movingUp = true;
	//			}
	//			NPC.velocity.Y += movingSpeed;

	//			Vector2 vector = NPC.velocity;
	//			NPC.velocity = Collision.TileCollision(NPC.position, NPC.velocity, NPC.width, NPC.height, false, false);
	//			if (NPC.velocity.X != vector.X)
	//			{
	//				NPC.velocity.X = -vector.X;
	//			}
	//			if (NPC.velocity.Y != vector.Y)
	//			{
	//				NPC.velocity.Y = -vector.Y;
	//			}

	//			Player player = Main.player[NPC.target];
	//			if (Vector2.Distance(NPC.Center, player.Center) <= 25f)
	//			{
	//				grappled = true;
	//			}
	//			NPC.noGravity = true;
	//		}
	//		if (frozen)
	//		{
	//			NPC.damage = 0;
	//			NPC.frame.Y = 0;
	//			NPC.noGravity = false;
	//			NPC.rotation += NPC.velocity.X * 0.1f;
	//			if (NPC.velocity.Y == 0f)
	//			{
	//				NPC.velocity.X = NPC.velocity.X * 0.98f;
	//				if ((double)NPC.velocity.X > -0.01 && (double)NPC.velocity.X < 0.01)
	//				{
	//					NPC.velocity.X = 0f;
	//				}
	//			}
	//		}

	//		if (Main.netMode == NetmodeID.Server && NPC.whoAmI < 200)
	//		{
	//			NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI, 0f, 0f, 0f, 0);
	//		}
	//	}

	//	/*public override void OnHitPlayer(Player player, int damage, bool crit)
	//	{
	//		if(grappled)
	//		{
	//			hitDir = 0;
	//			player.knockbackResist = 0f;
	//		}
	//	}*/
	//	/*public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
	//	{
	//		if (frozen && damage >= 20)
	//		{
	//			damage = (int)((double)(damage * (2 - (double)NPC.scale)) + (double)NPC.defense * 0.5);
	//		}
	//		return true;
	//	}*/
	//	public override void HitEffect(NPC.HitInfo hit)
	//	{
	//		if (NPC.life <= 0 && Main.netMode != NetmodeID.Server)
	//		{
	//			Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MetroidGore1").Type, NPC.scale);
	//			Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MetroidGore1").Type, NPC.scale);
	//			Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MetroidGore2").Type, NPC.scale);
	//			Gore.NewGore(NPC.GetSource_FromThis(), NPC.Center, NPC.velocity, Mod.Find<ModGore>("MetroidGore2").Type, NPC.scale);
	//		}
	//	}
	//	public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
	//	{
	//		if (projectile.type == ((ModMBWeapon)MBAddonLoader.GetAddon<MorphBallAddons.Bomb>()).ProjectileType || projectile.type == ((ModMBSpecial)MBAddonLoader.GetAddon<MorphBallAddons.PowerBomb>()).ProjectileType)
	//		{
	//			grappled = false;
	//		}
	//	}
	//	public override void FindFrame(int frameHeight)
	//	{
	//		int num = 1;
	//		if (!Main.dedServ)
	//		{
	//			num = Terraria.GameContent.TextureAssets.Npc[Type].Value.Height / Main.npcFrameCount[Type];//Main.npcTexture[Type].Height / Main.npcFrameCount[NPC.type];
	//		}
	//		if (!frozen)
	//		{
	//			if (!grappled) NPC.rotation = NPC.velocity.X * 0.1f;
	//			NPC.frameCounter += 1.0;
	//			if (NPC.frameCounter >= 10.0)
	//			{
	//				NPC.frame.Y = NPC.frame.Y + num;
	//				NPC.frameCounter = 0.0;
	//			}
	//			if (NPC.frame.Y >= num * Main.npcFrameCount[Type])
	//			{
	//				NPC.frame.Y = 0;
	//			}
	//		}
	//		else
	//		{
	//			NPC.frame.Y = num;
	//		}
	//	}

	//	private void SetStats()
	//	{
	//		NPC.scale = newScale;
	//		NPC.defense = NPC.defDefense = (int)(NPC.defense * NPC.scale);
	//		NPC.damage = NPC.defDamage = (int)(NPC.damage * NPC.scale);
	//		NPC.life = NPC.lifeMax = (int)(NPC.life * NPC.scale);
	//		NPC.value = ((int)(NPC.value * NPC.scale));
	//		NPC.npcSlots *= NPC.scale;
	//		NPC.knockBackResist *= 2f - NPC.scale;
	//	}

	//	public override void SendExtraAI(BinaryWriter writer)
	//	{
	//		writer.Write((double)newScale);
	//	}
	//	public override void ReceiveExtraAI(BinaryReader reader)
	//	{
	//		newScale = (float)reader.ReadDouble();
	//	}
	//}

}
