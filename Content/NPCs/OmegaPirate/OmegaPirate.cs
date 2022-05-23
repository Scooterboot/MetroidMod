using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MetroidModPorted;
using MetroidModPorted.Content.NPCs.OmegaPirate;
using System.IO;

namespace MetroidModPorted.Content.NPCs.OmegaPirate
{
	[AutoloadBossHead]
	public class OmegaPirate : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omega Pirate");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					20,
					24,
					31,
					39,
					44,
					69,
					70,
					ModContent.BuffType<Buffs.PhazonDebuff>()
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
		}

		public override void SetDefaults()
		{
			NPC.width = 62;
			NPC.height = 62;
			NPC.damage = 0;
			NPC.defense = 30;
			NPC.lifeMax = 20000;
			NPC.dontTakeDamage = false;
			NPC.scale = 1f;
			NPC.boss = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(0, 0, 7, 0);
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = false;
			NPC.noGravity = false;
			NPC.behindTiles = true;
			for (int i = 0; i < NPC.buffImmune.Length; i++)
			{
				NPC.buffImmune[i] = true;
			}
			NPC.aiStyle = -1;
			NPC.npcSlots = 5;
			if (!Main.dedServ) { Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/OmegaPirate"); }
			NPC.chaseable = false;
			
			NPC.ai = new float[8];

			NPC.BossBar = ModContent.GetInstance<BossBars.OmegaPirateBossBar>();
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossLifeScale);
			NPC.damage = 0;//(int)(NPC.damage * 0.8f);
			damage = (int)(damage * 2 * 0.8f);
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Boss.OmegaPirateBag>()));

			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Miscellaneous.PurePhazon>(), 1, 30, 41));
			//notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.OmegaPirateMusicBox>(), 6));
			//notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.OmegaPirateMask>(), 8));
			//notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.OmegaPirateTrophy>(), 11));

			npcLoot.Add(notExpertRule);
		}
		public override void OnKill()
		{
			Common.Systems.MSystem.bossesDown |= MetroidBossDown.downedOmegaPirate;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			/*if (Main.netMode != 2)
			{
				if(currentState > 0)
				{
					for (int m = 0; m < (NPC.life <= 0 ? 20 : 5); m++)
					{
						int dustID = Dust.NewDust(NPC.position, NPC.width, NPC.height, 5, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, Color.White, NPC.life <= 0 && m % 2 == 0 ? 3f : 1f);
						if (NPC.life <= 0 && m % 2 == 0)
						{
							Main.dust[dustID].noGravity = true;
						}
					}
				}
				if (NPC.life <= 0)
				{
					
				}
			}*/
		}

		public override bool? CanBeHitByItem(Player player, Item item) => false;
		public override bool? CanBeHitByProjectile(Projectile projectile) => false;
		
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			if(fullAlpha < 0.1f)
			{
				return false;
			}
			scale = 1.5f;
			position = new Vector2(NPC.Center.X,NPC.position.Y+206);
			if(NPC.life < NPC.lifeMax)
			{
				return true;
			}
			return null;
		}
		
		public override void BossHeadSlot(ref int index)
		{
			index = NPCHeadLoader.GetBossHeadSlot(BossHeadTexture);
			if(fullAlpha < 0.1f)
			{
				index = -1;
			}
		}
		
		int damage = 70;

		int _body,
			_rArmArmor, _lArmArmor,
			_rLegArmor, _lLegArmor,
			_rCannon, _lCannon;
		NPC Body => Main.npc[_body];
		//NPC RArmArmor => Main.npc[_rArmArmor];
		//NPC LArmArmor => Main.npc[_lArmArmor];
		//NPC RLegArmor => Main.npc[_rLegArmor];
		//NPC LLegArmor => Main.npc[_lLegArmor];
		NPC RArmArmor
		{
			get
			{
				if(Main.npc[_rArmArmor].type == ModContent.NPCType<OmegaPirate_WeakPoint>())
				{
					return Main.npc[_rArmArmor];
				}
				return null;
			}
		}
		NPC LArmArmor
		{
			get
			{
				if(Main.npc[_lArmArmor].type == ModContent.NPCType<OmegaPirate_WeakPoint>())
				{
					return Main.npc[_lArmArmor];
				}
				return null;
			}
		}
		NPC RLegArmor
		{
			get
			{
				if(Main.npc[_rLegArmor].type == ModContent.NPCType<OmegaPirate_WeakPoint>())
				{
					return Main.npc[_rLegArmor];
				}
				return null;
			}
		}
		NPC LLegArmor
		{
			get
			{
				if(Main.npc[_lLegArmor].type == ModContent.NPCType<OmegaPirate_WeakPoint>())
				{
					return Main.npc[_lLegArmor];
				}
				return null;
			}
		}
		NPC RCannon => Main.npc[_rCannon];
		NPC LCannon => Main.npc[_lCannon];

		int[] _rArm = new int[3],
			_lArm = new int[3],
			_rLeg = new int[3],
			_lLeg = new int[3];

		NPC GetArm(bool left, int i)
		{
			if (left)
				return (Main.npc[_lArm[i]]);
			return (Main.npc[_rArm[i]]);
		}
		NPC GetLeg(bool left, int i)
		{
			if (left)
				return (Main.npc[_lLeg[i]]);
			return (Main.npc[_rLeg[i]]);
		}

		internal int NPCArmorHP
		{
			get
			{
				int amt = 0;
				if (RArmArmor != null && RArmArmor.active) { amt += RArmArmor.life; }
				if (LArmArmor != null && LArmArmor.active) { amt += LArmArmor.life; }
				if (RLegArmor != null && RLegArmor.active) { amt += RLegArmor.life; }
				if (LLegArmor != null && LLegArmor.active) { amt += LLegArmor.life; }
				return amt;
			}
		}

		Vector2 BodyOffset;
		Vector2[] BodyPos = new Vector2[2],
		RArmPos = new Vector2[5],
		LArmPos = new Vector2[5],
		RLegPos = new Vector2[6],
		LLegPos = new Vector2[6],
		RCannonPos = new Vector2[3],
		LCannonPos = new Vector2[3],
		HeadPos = new Vector2[2];
		
		float BodyRot, PelvisRot;
		float[] RArmRot = new float[2],
		LArmRot = new float[2],
		RLegRot = new float[3],
		LLegRot = new float[3],
		HeadRot = new float[2],
		RCannonRot = new float[2],
		LCannonRot = new float[2];
		
		float PelvisOffset;
		Vector2 RArmOffset, LArmOffset;
		
		float BodyOffsetRot;
		float[] RArmOffsetRot = new float[5],
		LArmOffsetRot = new float[5],
		RLegOffsetRot = new float[6],
		LLegOffsetRot = new float[6],
		RCannonOffsetRot = new float[3],
		LCannonOffsetRot = new float[3],
		HeadOffsetRot = new float[2];
		
		float BodyDist;
		float[] RArmDist = new float[5],
		LArmDist = new float[5],
		RLegDist = new float[6],
		LLegDist = new float[6],
		RCannonDist = new float[3],
		LCannonDist = new float[3],
		HeadDist = new float[2];
		
		static Vector2[] DefaultBodyPos = { new Vector2(-5,73), new Vector2(11,-61) },
		DefaultRightArmPos = { new Vector2(-26,-74), new Vector2(-16,26), new Vector2(-3,-5), new Vector2(-3,13), new Vector2(28,58) },
		DefaultLeftArmPos = { new Vector2(36,-74), new Vector2(-16,26), new Vector2(-3,-5), new Vector2(-3,13), new Vector2(28,58) },
		DefaultRightLegPos = { new Vector2(-16,2), new Vector2(10,30), new Vector2(-8,40), new Vector2(4,18), new Vector2(-1,19), new Vector2(2,8) },
		DefaultLeftLegPos = { new Vector2(14,2), new Vector2(10,30), new Vector2(-8,40), new Vector2(4,18), new Vector2(-1,19), new Vector2(2,8) },
		DefaultRightCannonPos = { new Vector2(-30,-100), new Vector2(-2,-20), new Vector2(10,0) },
		DefaultLeftCannonPos = { new Vector2(24,-100), new Vector2(2,-20), new Vector2(10,0) },
		DefaultHeadPos = { new Vector2(28,-86), new Vector2(-14,4) };
		
		Vector2[] CurrentBodyPos = new Vector2[2],
		CurrentRightArmPos = new Vector2[5],
		CurrentLeftArmPos = new Vector2[5],
		CurrentRightLegPos = new Vector2[6],
		CurrentLeftLegPos = new Vector2[6],
		CurrentRightCannonPos = new Vector2[3],
		CurrentLeftCannonPos = new Vector2[3],
		CurrentHeadPos = new Vector2[2];
		
		Vector2 fullScale = new Vector2(1f,1f);
		
		void SetPositions()
		{
			for(int i = 0; i < 6; i++)
			{
				CurrentRightLegPos[i] = DefaultRightLegPos[i];
				CurrentLeftLegPos[i] = DefaultLeftLegPos[i];
				if(i < 5)
				{
					CurrentRightArmPos[i] = DefaultRightArmPos[i];
					CurrentLeftArmPos[i] = DefaultLeftArmPos[i];
				}
				if(i < 3)
				{
					CurrentRightCannonPos[i] = DefaultRightCannonPos[i];
					CurrentLeftCannonPos[i] = DefaultLeftCannonPos[i];
				}
				if(i < 2)
				{
					CurrentBodyPos[i] = DefaultBodyPos[i];
					CurrentHeadPos[i] = DefaultHeadPos[i];
				}
			}
			
			CurrentRightLegPos[0].X += PelvisOffset;
			CurrentLeftLegPos[0].X -= PelvisOffset;
			
			CurrentRightArmPos[0] += RArmOffset;
			CurrentLeftArmPos[0] += LArmOffset;
			
			
			BodyOffsetRot = (float)Math.Atan2(CurrentBodyPos[1].Y,CurrentBodyPos[1].X);
			BodyDist = CurrentBodyPos[1].Length();
			
			for(int i = 0; i < 6; i++)
			{
				RLegOffsetRot[i] = (float)Math.Atan2(CurrentRightLegPos[i].Y,CurrentRightLegPos[i].X);
				RLegDist[i] = CurrentRightLegPos[i].Length();
				
				LLegOffsetRot[i] = (float)Math.Atan2(CurrentLeftLegPos[i].Y,CurrentLeftLegPos[i].X);
				LLegDist[i] = CurrentLeftLegPos[i].Length();
				
				if(i < 5)
				{
					RArmOffsetRot[i] = (float)Math.Atan2(CurrentRightArmPos[i].Y,CurrentRightArmPos[i].X);
					RArmDist[i] = CurrentRightArmPos[i].Length();
					
					LArmOffsetRot[i] = (float)Math.Atan2(CurrentLeftArmPos[i].Y,CurrentLeftArmPos[i].X);
					LArmDist[i] = CurrentLeftArmPos[i].Length();
				}
				if(i < 3)
				{
					RCannonOffsetRot[i] = (float)Math.Atan2(CurrentRightCannonPos[i].Y,CurrentRightCannonPos[i].X);
					RCannonDist[i] = CurrentRightCannonPos[i].Length();
					
					LCannonOffsetRot[i] = (float)Math.Atan2(CurrentLeftCannonPos[i].Y,CurrentLeftCannonPos[i].X);
					LCannonDist[i] = CurrentLeftCannonPos[i].Length();
				}
				if(i < 2)
				{
					HeadOffsetRot[i] = (float)Math.Atan2(CurrentHeadPos[i].Y,CurrentHeadPos[i].X);
					HeadDist[i] = CurrentHeadPos[i].Length();
				}
			}
			
			if(NPC.direction == -1)
			{
				RArmOffsetRot[0] = (float)Math.Atan2(CurrentLeftArmPos[0].Y,CurrentLeftArmPos[0].X);
				RArmDist[0] = CurrentLeftArmPos[0].Length();
				
				LArmOffsetRot[0] = (float)Math.Atan2(CurrentRightArmPos[0].Y,CurrentRightArmPos[0].X);
				LArmDist[0] = CurrentRightArmPos[0].Length();
				
				RLegOffsetRot[0] = (float)Math.Atan2(CurrentLeftLegPos[0].Y,CurrentLeftLegPos[0].X);
				RLegDist[0] = CurrentLeftLegPos[0].Length();
				
				LLegOffsetRot[0] = (float)Math.Atan2(CurrentRightLegPos[0].Y,CurrentRightLegPos[0].X);
				LLegDist[0] = CurrentRightLegPos[0].Length();
				
				for(int i = 0; i < 2; i++)
				{
					RCannonOffsetRot[i] = (float)Math.Atan2(CurrentLeftCannonPos[i].Y,CurrentLeftCannonPos[i].X);
					RCannonDist[i] = CurrentLeftCannonPos[i].Length();
					
					LCannonOffsetRot[i] = (float)Math.Atan2(CurrentRightCannonPos[i].Y,CurrentRightCannonPos[i].X);
					LCannonDist[i] = CurrentRightCannonPos[i].Length();
				}
			}
			
			BodyPos[0] = NPC.Center + (CurrentBodyPos[0] + BodyOffset) * fullScale;
			BodyPos[1] = BodyPos[0] + Angle.AngleFlip(BodyOffsetRot + BodyRot, NPC.direction).ToRotationVector2()*BodyDist * fullScale;
			
			RArmPos[0] = BodyPos[0] + Angle.AngleFlip(RArmOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2()*RArmDist[0] * fullScale;
			RArmPos[1] = RArmPos[0] + Angle.AngleFlip(RArmOffsetRot[1] + RArmRot[0], NPC.direction).ToRotationVector2()*RArmDist[1] * fullScale;
			RArmPos[2] = RArmPos[0] + Angle.AngleFlip(RArmOffsetRot[2] + RArmRot[0], NPC.direction).ToRotationVector2()*RArmDist[2] * fullScale;
			RArmPos[3] = RArmPos[1] + Angle.AngleFlip(RArmOffsetRot[3] + RArmRot[1], NPC.direction).ToRotationVector2()*RArmDist[3] * fullScale;
			RArmPos[4] = RArmPos[1] + Angle.AngleFlip(RArmOffsetRot[4] + RArmRot[1], NPC.direction).ToRotationVector2()*RArmDist[4] * fullScale;
			
			LArmPos[0] = BodyPos[0] + Angle.AngleFlip(LArmOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2()*LArmDist[0] * fullScale;
			LArmPos[1] = LArmPos[0] + Angle.AngleFlip(LArmOffsetRot[1] + LArmRot[0], NPC.direction).ToRotationVector2()*LArmDist[1] * fullScale;
			LArmPos[2] = LArmPos[0] + Angle.AngleFlip(LArmOffsetRot[2] + LArmRot[0], NPC.direction).ToRotationVector2()*LArmDist[2] * fullScale;
			LArmPos[3] = LArmPos[1] + Angle.AngleFlip(LArmOffsetRot[3] + LArmRot[1], NPC.direction).ToRotationVector2()*LArmDist[3] * fullScale;
			LArmPos[4] = LArmPos[1] + Angle.AngleFlip(LArmOffsetRot[4] + LArmRot[1], NPC.direction).ToRotationVector2()*LArmDist[4] * fullScale;
			
			RLegPos[0] = BodyPos[0] + Angle.AngleFlip(RLegOffsetRot[0] + PelvisRot, NPC.direction).ToRotationVector2()*RLegDist[0] * fullScale;
			RLegPos[1] = RLegPos[0] + Angle.AngleFlip(RLegOffsetRot[1] + RLegRot[0], NPC.direction).ToRotationVector2()*RLegDist[1] * fullScale;
			RLegPos[2] = RLegPos[1] + Angle.AngleFlip(RLegOffsetRot[2] + RLegRot[1], NPC.direction).ToRotationVector2()*RLegDist[2] * fullScale;
			RLegPos[3] = RLegPos[0] + Angle.AngleFlip(RLegOffsetRot[3] + RLegRot[0], NPC.direction).ToRotationVector2()*RLegDist[3] * fullScale;
			RLegPos[4] = RLegPos[1] + Angle.AngleFlip(RLegOffsetRot[4] + RLegRot[1], NPC.direction).ToRotationVector2()*RLegDist[4] * fullScale;
			RLegPos[5] = RLegPos[2] + Angle.AngleFlip(RLegOffsetRot[5] + RLegRot[2], NPC.direction).ToRotationVector2()*RLegDist[5] * fullScale;
			
			LLegPos[0] = BodyPos[0] + Angle.AngleFlip(LLegOffsetRot[0] + PelvisRot, NPC.direction).ToRotationVector2()*LLegDist[0] * fullScale;
			LLegPos[1] = LLegPos[0] + Angle.AngleFlip(LLegOffsetRot[1] + LLegRot[0], NPC.direction).ToRotationVector2()*LLegDist[1] * fullScale;
			LLegPos[2] = LLegPos[1] + Angle.AngleFlip(LLegOffsetRot[2] + LLegRot[1], NPC.direction).ToRotationVector2()*LLegDist[2] * fullScale;
			LLegPos[3] = LLegPos[0] + Angle.AngleFlip(LLegOffsetRot[3] + LLegRot[0], NPC.direction).ToRotationVector2()*LLegDist[3] * fullScale;
			LLegPos[4] = LLegPos[1] + Angle.AngleFlip(LLegOffsetRot[4] + LLegRot[1], NPC.direction).ToRotationVector2()*LLegDist[4] * fullScale;
			LLegPos[5] = LLegPos[2] + Angle.AngleFlip(LLegOffsetRot[5] + LLegRot[2], NPC.direction).ToRotationVector2()*LLegDist[5] * fullScale;
			
			RCannonPos[0] = BodyPos[0] + Angle.AngleFlip(RCannonOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2()*RCannonDist[0] * fullScale;
			RCannonPos[1] = RCannonPos[0] + Angle.AngleFlip(RCannonOffsetRot[1] + RCannonRot[0], NPC.direction).ToRotationVector2()*RCannonDist[1] * fullScale;
			RCannonPos[2] = RCannonPos[1] + Angle.AngleFlip(RCannonOffsetRot[2] + RCannonRot[1], NPC.direction).ToRotationVector2()*RCannonDist[2] * fullScale;
			
			LCannonPos[0] = BodyPos[0] + Angle.AngleFlip(LCannonOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2()*LCannonDist[0] * fullScale;
			LCannonPos[1] = LCannonPos[0] + Angle.AngleFlip(LCannonOffsetRot[1] + LCannonRot[0], NPC.direction).ToRotationVector2()*LCannonDist[1] * fullScale;
			LCannonPos[2] = LCannonPos[1] + Angle.AngleFlip(LCannonOffsetRot[2] + LCannonRot[1], NPC.direction).ToRotationVector2()*LCannonDist[2] * fullScale;
			
			HeadPos[0] = BodyPos[0] + Angle.AngleFlip(HeadOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2()*HeadDist[0] * fullScale;
			HeadPos[1] = HeadPos[0] + Angle.AngleFlip(HeadOffsetRot[1] + HeadRot[0], NPC.direction).ToRotationVector2()*HeadDist[1] * fullScale;
		}
		
		Vector2 cannonTargetPos;
		float cannonTargetTransition = 0f;
		
		bool initialized = false;
		public override bool PreAI()
		{
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			if(!initialized && Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.netUpdate = true;
				NPC.TargetClosest(true);
				
				Player player = Main.player[NPC.target];
				
				NPC.direction = 1;
				if (Main.rand.Next(2) == 0)
					NPC.direction = -1;

				NPC.velocity.Y = 0.1f;
				NPC.Center = new Vector2(player.Center.X - 150 * NPC.direction, player.Center.Y - 1500);
				
				cannonTargetPos = player.Center;
				
				SetPositions();

				var entitySource = NPC.GetSource_FromAI();
				_body = NPC.NewNPC(entitySource, (int)BodyPos[1].X, (int)BodyPos[1].Y, ModContent.NPCType<OmegaPirate_HitBox>(), NPC.whoAmI, NPC.whoAmI);

				for (int i = 0; i < 3; i++)
				{
					_rArm[i] = NPC.NewNPC(entitySource, (int)RArmPos[i + 2].X, (int)RArmPos[i + 2].Y, ModContent.NPCType<OmegaPirate_HitBox>(), NPC.whoAmI, NPC.whoAmI, 1 + i);
					_lArm[i] = NPC.NewNPC(entitySource, (int)LArmPos[i + 2].X, (int)LArmPos[i + 2].Y, ModContent.NPCType<OmegaPirate_HitBox>(), NPC.whoAmI, NPC.whoAmI, 1 + i);
					_rLeg[i] = NPC.NewNPC(entitySource, (int)RLegPos[i + 3].X, (int)RLegPos[i + 3].Y, ModContent.NPCType<OmegaPirate_HitBox>(), NPC.whoAmI, NPC.whoAmI, 4 + i);
					_lLeg[i] = NPC.NewNPC(entitySource, (int)LLegPos[i + 3].X, (int)LLegPos[i + 3].Y, ModContent.NPCType<OmegaPirate_HitBox>(), NPC.whoAmI, NPC.whoAmI, 4 + i);
				}

				_rCannon = NPC.NewNPC(entitySource, (int)RCannonPos[2].X, (int)RCannonPos[2].Y, ModContent.NPCType<OmegaPirate_HitBox>(), NPC.whoAmI, NPC.whoAmI, 7);
				_lCannon = NPC.NewNPC(entitySource, (int)LCannonPos[2].X, (int)LCannonPos[2].Y, ModContent.NPCType<OmegaPirate_HitBox>(), NPC.whoAmI, NPC.whoAmI, 7);
					
				_rArmArmor = NPC.NewNPC(entitySource, (int)RArmPos[2].X, (int)RArmPos[2].Y, ModContent.NPCType<OmegaPirate_WeakPoint>(), NPC.whoAmI, NPC.whoAmI);
				_lArmArmor = NPC.NewNPC(entitySource, (int)LArmPos[2].X, (int)LArmPos[2].Y, ModContent.NPCType<OmegaPirate_WeakPoint>(), NPC.whoAmI, NPC.whoAmI);
				_rLegArmor = NPC.NewNPC(entitySource, (int)RLegPos[3].X, (int)RLegPos[3].Y, ModContent.NPCType<OmegaPirate_WeakPoint>(), NPC.whoAmI, NPC.whoAmI, 1);
				_lLegArmor = NPC.NewNPC(entitySource, (int)LLegPos[3].X, (int)LLegPos[3].Y, ModContent.NPCType<OmegaPirate_WeakPoint>(), NPC.whoAmI, NPC.whoAmI, 1);

				initialized = true;
			}
			return true;
		}
		
		//Spawn animation
		float[] HeadAnim_Spawn = {0,-20,0};
		float head_SpawnTransition = 0f;
		float[] BodyAnim_Spawn = {0,-10,-5};
		
		float[][] RArmAnim_Spawn = new float[][]{
		new float[] {15,10,20},
		new float[] {-5,10,20}};
		
		float[][] LArmAnim_Spawn = new float[][]{
		new float[] {10, -10,-20},
		new float[] {-10,-10,-20}};
		
		float[][] RLegAnim_Spawn = new float[][]{
		new float[] {0,-10,-20},
		new float[] {0,-90,-24},
		new float[] {0,-70, 15}};
		
		float[][] LLegAnim_Spawn = new float[][]{
		new float[] {0, 95,20},
		new float[] {0,-15, 0},
		new float[] {0, 15,15}};
		
		float[] BodyOffset_Spawn = {0,50,5};
		float anim_Spawn = 1f;
		
		//Walk animation
		float[] BodyAnim_Walk = {-5, -2f,0,-2f,  -5, -2f,0,-2f,  -5};
		float[] RArmAnim_Walk = { 20, 12,0,-12,  -20,-12,0, 12,   20};
		float[] LArmAnim_Walk = {-20,-12,0, 12,   20, 12,0,-12,  -20};
		float[] LShAnim_Walk_Absorb = {30,35,40,45,  50,45,40,35, 30};
		float LArmAnim_Walk_Absorb = 40;
		float anim_Walk_AbsorbTransition = 0f;
		
		float[][] RLegAnim_Walk = new float[][]{
		new float[] {-20, 20, 50,40,  20,0,-20,-20,  -20},
		new float[] {-24,-40,-20,10,  0, 0, 0, -12,  -24},
		new float[] { 15,-5,  5, 30,  15,15,15, 15,   15f}};
		
		float[][] LLegAnim_Walk = new float[][]{
		new float[] {20,0,-20,-20,  -20, 20, 50,40,  20},
		new float[] {0, 0, 0, -12,  -24,-40,-20,10,  0},
		new float[] {15,15,15, 15,   15,-5,  5, 30,  15}};
		
		float[] legAnim_Walk_Speed = {15,16,8,7, 15,16,8,7};
		float anim_Walk = 1f;
		
		//Jump animation
		float[] BodyAnim_Jump = {-6,0f,-5f,-15f};
		
		float[][] RArmAnim_Jump = new float[][]{
		new float[] {30f,-20f,10f,15f},
		new float[] {40f,-25f,15f,25f}};
		
		float[][] LArmAnim_Jump = new float[][]{
		new float[] {-20f,60f,10f,-10f},
		new float[] {-10f,85f,15f,-10f}};
		
		float[][] LArmAnim_Jump_Absorb = new float[][]{
		new float[] {20f,50f,20f,10f},
		new float[] {40f,40f,40f,40f}};
		
		float[][] RLegAnim_Jump = new float[][]{
		new float[] {  0f, 45f,-15f,  0f},
		new float[] {-50f,-20f, -5f,-65f},
		new float[] { 15f,-20f, -5f,  0f}};
		
		float[][] LLegAnim_Jump = new float[][]{
		new float[] { 50f,-20f,10f, 60f},
		new float[] {-10f,-10f, 0f,-24f},
		new float[] { 15f,-10f, 0f, 15f}};
		
		float anim_Jump = 1f;
		float anim_JumpTransition = 0f;
		
		//Shockwave attack animation
		float[] BodyAnim_Shockwave = {-2.5f,-1f,0f,-30,-60};
		float[] HeadAnim_Shockwave = {    0,  0, 0,-30,-60};
		
		float[][] RArmAnim_Shockwave = new float[][]{
		new float[] {50f,145f,155f,87.5f,20f},
		new float[] {50f,125f,135f,70.5f,0f}};
		
		float[][] LArmAnim_Shockwave = new float[][]{
		new float[] {50f,145f,155f,77.5f, 0f},
		new float[] {50f,145f,175f, 105f,35f}};
		
		float[][] RLegAnim_Shockwave = new float[][]{
		new float[] {-20f,-20f,-20f,  -10f,  0f},
		new float[] {-22f,-18f,-18f,-41.5f,-65f},
		new float[] { 15f, 15f, 15f,  7.5f,  0f}};
		
		float[][] LLegAnim_Shockwave = new float[][]{
		new float[] {15f,12f,12f,33.5f, 55f},
		new float[] { 0f, 0f, 0f, -15f,-30f},
		new float[] {15f,15f,15f,  15f, 15f}};
		
		float anim_Shockwave = 1f;
		float anim_ShockwaveTransition = 0f;
		float anim_ShockwaveTransition_Head = 0f;
		
		//Giant leap attack animation
		float[] BodyAnim_Leap = {-10f,-3.5f,-1f,0f,-60f};
		float[] HeadAnim_Leap = {  0f,   0f, 0f,0f,-60f};
		
		float[][] RArmAnim_Leap = new float[][]{
		new float[] {30f,50f,145f,155f,20f},
		new float[] {40f,50f,125f,135f,0f}};
		
		float[][] LArmAnim_Leap = new float[][]{
		new float[] {-30f,50f,145f,155f,0f},
		new float[] {-20f,50f,145f,175f,35f}};
		
		float[][] RLegAnim_Leap = new float[][]{
		new float[] {  0f, 45f, 45f,-15f,  0f},
		new float[] {-65f,-20f,-20f, -5f,-65f},
		new float[] {  0f,-20f,-20f, -5f,  0f}};
		
		float[][] LLegAnim_Leap = new float[][]{
		new float[] { 62f,-20f,-20f,10f, 55f},
		new float[] {-20f,-10f,-10f, 0f,-30f},
		new float[] { 15f,-10f,-10f, 0f, 15f}};
		
		float anim_Leap = 1f;
		float anim_LeapTransition = 0f;
		float anim_LeapTransition_Head = 0f;
		
		//Claw attack animation
		float[][] ArmAnim_Claw = new float[][]{
		new float[] {  0,  0,60, 80},
		new float[] {-10,-20,60,100}};
		
		float anim_Claw = 1f;
		float anim_ClawTransition = 0f;
		
		//Cannon attack animation
		float[] BodyAnim_CannonFire = {0f,5f};
		
		float[][] RArmAnim_CannonFire = new float[][]{
		new float[] {0f,8f},
		new float[] {10f,15f}};
		
		float[][] LArmAnim_CannonFire = new float[][]{
		new float[] {-10f,-2f},
		new float[] {10f,15f}};
		
		float[][] RLegAnim_CannonFire = new float[][]{
		new float[] {-20f,-20f},
		new float[] {-24f,-24f},
		new float[] {15f,15f}};
		
		float[][] LLegAnim_CannonFire = new float[][]{
		new float[] {20f,20f},
		new float[] {0f,0f},
		new float[] {15f,15f}};
		
		float[] RCannonAnim_CannonFire = {0f,30f};
		float[] LCannonAnim_CannonFire = {0f,30f};
		
		float anim_CannonFire = 1f;
		float anim_CannonFireTransition = 0f;
		
		float bodyCannonTargetRot = 0f;
		float rightCannonTRot = 0f;
		float leftCannonTRot = 0f;
		
		bool rCannonRecoil = false;
		float rCannonRecoilAnim = 0f;
		bool lCannonRecoil = false;
		float lCannonRecoilAnim = 0f;
		
		bool laserAnim = false;
		float rLaserAngle = 0f;
		float lLaserAngle = 0f;
		float laserAlpha = 0f;
		float laserFrame = 0f;
		
		bool armGlowAnim = false;
		float armGlowAlpha = 0f;
		
		float bodyAlpha = 1f;
		float fullAlpha = 0f;
		
		
		//Phazon regen phase start animation
		float[] BodyAnim_PhazonStart = {10f,-10f,-5f,-15f,-5f,0f};
		float[] HeadAnim_PhazonStart = {10f,-20f,10f,-10f, 0f,0f};
		
		float[][] RArmAnim_PhazonStart = new float[][]{
		new float[] {-10f,10f,-10f, 10f,40f,0f},
		new float[] {-10f,10f,-20f,-10f,50f,0f}};
		
		float[][] LArmAnim_PhazonStart = new float[][]{
		new float[] {0f,-10f,60f,20f,70, 0f},
		new float[] {0f,-10f,80f, 0f,60f,0f}};
		
		float[][] RLegAnim_PhazonStart = new float[][]{
		new float[] {-15f,-20f,-15f,-10f,-10f,-20f},
		new float[] {-23f,-90f,-90f,-90f,-33f,-24f},
		new float[] { 15f,-60f,-70f,-70f, 15f,15f}};
		
		float[][] LLegAnim_PhazonStart = new float[][]{
		new float[] {15f, 95f, 95f, 95f,30f,20f},
		new float[] {30f,-12f,-18f,-25f,-5f, 0f},
		new float[] {15f, 15f, 15f, 15f,15f,15f}};
		
		float anim_PhazonStart = 1f;
		float anim_PhazonStartTransition = 0f;
		
		//Phazon armor regeneration animation
		float[] BodyAnim_PhazonRegen = {0f,5f,4f,7f,-10f,0f};
		
		float[][] RArmAnim_PhazonRegen = new float[][]{
		new float[] {0f,170f,175f, 70f,60f,0f},
		new float[] {0f,160f,170f,100f,90f,0f}};
		
		float[][] LArmAnim_PhazonRegen = new float[][]{
		new float[] {0f,120f,125f, 70f, 60f,0f},
		new float[] {0f,130f,125f,160f,150f,0f}};
		
		float[][] RLegAnim_PhazonRegen = new float[][]{
		new float[] {-20f,-20f,-20f,-15f,-20f,-20f},
		new float[] {-24f,-24f,-24f,-23f,-90f,-24f},
		new float[] { 15f, 15f, 15f, 15f,-60f, 15f}};
		
		float[][] LLegAnim_PhazonRegen = new float[][]{
		new float[] {20f,20f,20f,15f, 95f,20f},
		new float[] { 0f, 0f, 0f,30f,-12f, 0f},
		new float[] {15f,15f,15f,15f, 15f,15f}};
		
		float anim_PhazonRegen = 1f;
		float anim_PhazonRegenTransition = 0f;
		
		//Death animation
		float[] BodyAnim_Death = {5f,15f, -5f, -2.5f,-10f,-60f};
		float[] HeadAnim_Death = {5f, 5f,-15f,-12.5f,-20f,-70f};
		
		float[][] RArmAnim_Death = new float[][]{
		new float[] {170f,155f,  5f,  5f, 10f,40f},
		new float[] {160f,145f,-15f,-15f,-10f,30f}};
		
		float[][] LArmAnim_Death = new float[][]{
		new float[] {120f,105f,  5f,  5f, 10f,80f},
		new float[] {130f,115f,-15f,-15f,-10f,60f}};
		
		float[][] RLegAnim_Death = new float[][]{
		new float[] {-20f,-20f,-20f,-30f,-55f, -90f},
		new float[] {-24f,-24f,-24f,-26f,-55f, -90f},
		new float[] { 15f, 15f, 15f, 15f,-65f,-100f}};
		
		float[][] LLegAnim_Death = new float[][]{
		new float[] {20f,20f,20f, -7.5f,-35f, -90f},
		new float[] { 0f, 0f, 0f,-27.5f,-55f, -90f},
		new float[] {15f,15f,15f,   15f,-65f,-100f}};
		
		float anim_Death = 1f;
		float anim_DeathTransition = 0f;
		
		float[] anim_Death_Speed = {0f,0f,0f,31f,27f,28f};
		
		float[] anim_Death_HeightOffset = {0f,0f,0f,0f,8f,10f};
		
		
		float DefaultMouthAnim = 5f;
		
		int mouthAnim = 0; //1 = shockwave, 2 = cannon, 3 = leap, 4 = hurt0, 5 = hurt1, 6 = hurt2, 7 = damaged
		
		float[] MouthAnim_Shockwave = {5f,-5f,0f,-10f,-5f,0f,5f};
		float anim_MouthShockwave = 1f;
		
		float[] MouthAnim_Hurt = {5f,-15f,-11f,-7f,-3f,1f,5f};
		float anim_MouthHurt = 1f;
		
		float[] MouthAnim_Damaged = {5f,-10f,5f,-15f,-8f,-2f,5f};
		float anim_MouthDamaged = 1f;
		
		
		void SetAnimation(string type, float anim, float transition = 1f)
		{
			if(type == "walk")
			{
				RArmOffset.X = MathHelper.Lerp(RArmOffset.X,0f,transition);
				LArmOffset.X = MathHelper.Lerp(LArmOffset.X,0f,transition);
				
				float offset = Math.Min(10 * (anim-1)/2, 10);
				if(anim >= 5)
				{
					offset = Math.Max(10 * (2 - (anim-5))/2, 0);
				}
				if(NPC.direction == -1)
				{
					offset = Math.Max(10 * (2 - (anim-1))/2, 0);
					if(anim >= 5)
					{
						offset = Math.Min(10 * (anim-5)/2, 10);
					}
				}
				PelvisOffset = MathHelper.Lerp(PelvisOffset,offset,transition);
				for(int i = 0; i < 3; i++)
				{
					RLegRot[i] = MathHelper.Lerp(RLegRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_Walk[i],anim)),transition);
					LLegRot[i] = MathHelper.Lerp(LLegRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_Walk[i],anim)),transition);
				}
				BodyRot = MathHelper.Lerp(BodyRot,-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,BodyAnim_Walk,anim)),transition);
				for(int i = 0; i < 2; i++)
				{
					RArmRot[i] = MathHelper.Lerp(RArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_Walk,anim)),transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Walk,anim)),transition);
				}
				RCannonRot[0] = BodyRot;
				RCannonRot[1] = BodyRot;
				LCannonRot[0] = BodyRot;
				LCannonRot[1] = BodyRot;
			}
			if(type == "absorb")
			{
				float armTargetRot = Angle.Vector2Angle(BodyPos[1],Main.player[NPC.target].Center,NPC.direction);
				float armrot0 = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LShAnim_Walk_Absorb,anim));
				float armrot1 = -(float)Angle.ConvertToRadians((double)LArmAnim_Walk_Absorb);
				float rot = (armTargetRot * 0.75f) - 0.4f;
				if(rot > 0.5f)
				{
					rot = 0.5f;
				}
				armrot0 += rot * 0.8f;
				armrot1 += rot;
				
				LArmRot[0] = MathHelper.Lerp(LArmRot[0], armrot0, transition);
				LArmRot[1] = MathHelper.Lerp(LArmRot[1], armrot1, transition);
			}
			if(type == "jump")
			{
				RArmOffset.X = MathHelper.Lerp(RArmOffset.X,0f,transition);
				LArmOffset.X = MathHelper.Lerp(LArmOffset.X,0f,transition);
				
				float offset = Math.Min(10 * (anim-1f), 10);
				if(anim >= 2)
				{
					offset = Math.Max(10 * (1 - (anim-2f)), 0);
				}
				if(NPC.direction == -1)
				{
					offset = Math.Max(10 * (1 - (anim-1f)), 0);
					if(anim >= 2)
					{
						offset = Math.Min(10 * (anim-2f), 10);
					}
				}
				PelvisOffset = MathHelper.Lerp(PelvisOffset,offset,transition);
				for(int i = 0; i < 3; i++)
				{
					RLegRot[i] = MathHelper.Lerp(RLegRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_Jump[i],anim)),transition);
					LLegRot[i] = MathHelper.Lerp(LLegRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_Jump[i],anim)),transition);
				}
				BodyRot = MathHelper.Lerp(BodyRot,-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,BodyAnim_Jump,anim)),transition);
				for(int i = 0; i < 2; i++)
				{
					RArmRot[i] = MathHelper.Lerp(RArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_Jump[i],anim)),transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Jump[i],anim)),transition);
				}
				RCannonRot[0] = BodyRot;
				RCannonRot[1] = BodyRot;
				LCannonRot[0] = BodyRot;
				LCannonRot[1] = BodyRot;
			}
			if(type == "jump absorb")
			{
				float armTargetRot = Angle.Vector2Angle(BodyPos[1],Main.player[NPC.target].Center,NPC.direction);
				float armrot0 = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Jump_Absorb[0],anim));
				float armrot1 = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Jump_Absorb[1],anim));
				float rot = (armTargetRot * 0.25f) - 0.4f;
				if(rot > 0.5f)
				{
					rot = 0.5f;
				}
				armrot0 += rot * 0.8f;
				armrot1 += rot;
				
				LArmRot[0] = MathHelper.Lerp(LArmRot[0], armrot0, transition);
				LArmRot[1] = MathHelper.Lerp(LArmRot[1], armrot1, transition);
			}
			if(type == "shockwave")
			{
				PelvisOffset = MathHelper.Lerp(PelvisOffset,0f,transition);
				
				float offset = 0f;
				if(anim >= 3)
				{
					offset = 10f * ((anim-3)/2);
				}
				RArmOffset.X = MathHelper.Lerp(RArmOffset.X,offset,transition);
				LArmOffset.X = MathHelper.Lerp(LArmOffset.X,-offset,transition);
				
				float[] rlegrot = new float[3];
				float[] llegrot = new float[3];
				for(int i = 0; i < 3; i++)
				{
					rlegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_Shockwave[i],anim));
					llegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_Shockwave[i],anim));
				}
				float[] rarmrot = new float[2];
				float[] larmrot = new float[2];
				for(int i = 0; i < 2; i++)
				{
					rarmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_Shockwave[i],anim));
					larmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Shockwave[i],anim));
				}
				
				if(NPC.direction == 1)
				{
					for(int i = 0; i < 3; i++)
					{
						RLegRot[i] = MathHelper.Lerp(RLegRot[i],rlegrot[i],transition);
						LLegRot[i] = MathHelper.Lerp(LLegRot[i],llegrot[i],transition);
					}
					for(int i = 0; i < 2; i++)
					{
						RArmRot[i] = MathHelper.Lerp(RArmRot[i],rarmrot[i],transition);
						LArmRot[i] = MathHelper.Lerp(LArmRot[i],larmrot[i],transition);
					}
				}
				else
				{
					for(int i = 0; i < 3; i++)
					{
						RLegRot[i] = MathHelper.Lerp(RLegRot[i],llegrot[i],transition);
						LLegRot[i] = MathHelper.Lerp(LLegRot[i],rlegrot[i],transition);
					}
					for(int i = 0; i < 2; i++)
					{
						RArmRot[i] = MathHelper.Lerp(RArmRot[i],larmrot[i],transition);
						LArmRot[i] = MathHelper.Lerp(LArmRot[i],rarmrot[i],transition);
					}
				}
				
				BodyRot = MathHelper.Lerp(BodyRot,-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,BodyAnim_Shockwave,anim)),transition);
				RCannonRot[0] = BodyRot;
				RCannonRot[1] = BodyRot;
				LCannonRot[0] = BodyRot;
				LCannonRot[1] = BodyRot;
			}
			if(type == "leap")
			{
				float offset = 0f;
				if(anim >= 4)
				{
					offset = 10f * (anim-4);
				}
				RArmOffset.X = MathHelper.Lerp(RArmOffset.X,offset,transition);
				LArmOffset.X = MathHelper.Lerp(LArmOffset.X,-offset,transition);
				
				float offset2 = Math.Min(10 * (anim-1f), 10);
				if(anim >= 3)
				{
					offset2 = Math.Max(10 * (1 - (anim-3f)), 0);
				}
				PelvisOffset = MathHelper.Lerp(PelvisOffset,offset2,transition);
				float[] rlegrot = new float[3];
				float[] llegrot = new float[3];
				for(int i = 0; i < 3; i++)
				{
					rlegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_Leap[i],anim));
					llegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_Leap[i],anim));
				}
				float[] rarmrot = new float[2];
				float[] larmrot = new float[2];
				for(int i = 0; i < 2; i++)
				{
					rarmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_Leap[i],anim));
					larmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Leap[i],anim));
				}
				
				if(NPC.direction == 1)
				{
					for(int i = 0; i < 3; i++)
					{
						RLegRot[i] = MathHelper.Lerp(RLegRot[i],rlegrot[i],transition);
						LLegRot[i] = MathHelper.Lerp(LLegRot[i],llegrot[i],transition);
					}
					for(int i = 0; i < 2; i++)
					{
						RArmRot[i] = MathHelper.Lerp(RArmRot[i],rarmrot[i],transition);
						LArmRot[i] = MathHelper.Lerp(LArmRot[i],larmrot[i],transition);
					}
				}
				else
				{
					for(int i = 0; i < 3; i++)
					{
						RLegRot[i] = MathHelper.Lerp(RLegRot[i],llegrot[i],transition);
						LLegRot[i] = MathHelper.Lerp(LLegRot[i],rlegrot[i],transition);
					}
					for(int i = 0; i < 2; i++)
					{
						RArmRot[i] = MathHelper.Lerp(RArmRot[i],larmrot[i],transition);
						LArmRot[i] = MathHelper.Lerp(LArmRot[i],rarmrot[i],transition);
					}
				}
				
				BodyRot = MathHelper.Lerp(BodyRot,-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,BodyAnim_Leap,anim)),transition);
				RCannonRot[0] = BodyRot;
				RCannonRot[1] = BodyRot;
				LCannonRot[0] = BodyRot;
				LCannonRot[1] = BodyRot;
			}
			if(type == "cannon")
			{
				PelvisOffset = MathHelper.Lerp(PelvisOffset,0f,transition);
				RArmOffset.X = MathHelper.Lerp(RArmOffset.X,0f,transition);
				LArmOffset.X = MathHelper.Lerp(LArmOffset.X,0f,transition);
				
				float[] rlegrot = new float[3];
				float[] llegrot = new float[3];
				for(int i = 0; i < 3; i++)
				{
					rlegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_CannonFire[i],anim));
					llegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_CannonFire[i],anim));
				}
				float[] rarmrot = new float[2];
				float[] larmrot = new float[2];
				for(int i = 0; i < 2; i++)
				{
					rarmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_CannonFire[i],anim));
					larmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_CannonFire[i],anim));
				}
				float rcannonrot = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RCannonAnim_CannonFire,anim));
				float lcannonrot = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LCannonAnim_CannonFire,anim));
				
				if(NPC.direction == 1)
				{
					for(int i = 0; i < 3; i++)
					{
						RLegRot[i] = MathHelper.Lerp(RLegRot[i],rlegrot[i],transition);
						LLegRot[i] = MathHelper.Lerp(LLegRot[i],llegrot[i],transition);
					}
					for(int i = 0; i < 2; i++)
					{
						RArmRot[i] = MathHelper.Lerp(RArmRot[i],rarmrot[i],transition);
						LArmRot[i] = MathHelper.Lerp(LArmRot[i],larmrot[i],transition);
					}
					RCannonRot[0] = rcannonrot;
					LCannonRot[0] = lcannonrot;
				}
				else
				{
					for(int i = 0; i < 3; i++)
					{
						RLegRot[i] = MathHelper.Lerp(RLegRot[i],llegrot[i],transition);
						LLegRot[i] = MathHelper.Lerp(LLegRot[i],rlegrot[i],transition);
					}
					for(int i = 0; i < 2; i++)
					{
						RArmRot[i] = MathHelper.Lerp(RArmRot[i],larmrot[i],transition);
						LArmRot[i] = MathHelper.Lerp(LArmRot[i],rarmrot[i],transition);
					}
					RCannonRot[0] = lcannonrot;
					LCannonRot[0] = rcannonrot;
				}
				RCannonRot[1] = 0f;
				LCannonRot[1] = 0f;
				
				BodyRot = MathHelper.Lerp(BodyRot,-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,BodyAnim_CannonFire,anim)),transition);
			}
			
			if(type == "phazon start")
			{
				PelvisOffset = MathHelper.Lerp(PelvisOffset,0f,transition);
				RArmOffset.X = MathHelper.Lerp(RArmOffset.X,0f,transition);
				LArmOffset.X = MathHelper.Lerp(LArmOffset.X,0f,transition);
				
				float[] rlegrot = new float[3];
				float[] llegrot = new float[3];
				for(int i = 0; i < 3; i++)
				{
					rlegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_PhazonStart[i],anim));
					llegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_PhazonStart[i],anim));
				}
				float[] rarmrot = new float[2];
				float[] larmrot = new float[2];
				for(int i = 0; i < 2; i++)
				{
					rarmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_PhazonStart[i],anim));
					larmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_PhazonStart[i],anim));
				}
				
				if(NPC.direction == 1)
				{
					for(int i = 0; i < 3; i++)
					{
						RLegRot[i] = MathHelper.Lerp(RLegRot[i],rlegrot[i],transition);
						LLegRot[i] = MathHelper.Lerp(LLegRot[i],llegrot[i],transition);
					}
					for(int i = 0; i < 2; i++)
					{
						RArmRot[i] = MathHelper.Lerp(RArmRot[i],rarmrot[i],transition);
						LArmRot[i] = MathHelper.Lerp(LArmRot[i],larmrot[i],transition);
					}
				}
				else
				{
					for(int i = 0; i < 3; i++)
					{
						RLegRot[i] = MathHelper.Lerp(RLegRot[i],llegrot[i],transition);
						LLegRot[i] = MathHelper.Lerp(LLegRot[i],rlegrot[i],transition);
					}
					for(int i = 0; i < 2; i++)
					{
						RArmRot[i] = MathHelper.Lerp(RArmRot[i],larmrot[i],transition);
						LArmRot[i] = MathHelper.Lerp(LArmRot[i],rarmrot[i],transition);
					}
				}
				
				BodyRot = MathHelper.Lerp(BodyRot,-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,BodyAnim_PhazonStart,anim)),transition);
				RCannonRot[0] = BodyRot;
				RCannonRot[1] = BodyRot;
				LCannonRot[0] = BodyRot;
				LCannonRot[1] = BodyRot;
				HeadRot[0] = MathHelper.Lerp(HeadRot[0],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,HeadAnim_PhazonStart,anim)),transition);
			}
			if(type == "phazon regen")
			{
				PelvisOffset = MathHelper.Lerp(PelvisOffset,0f,transition);
				RArmOffset.X = MathHelper.Lerp(RArmOffset.X,0f,transition);
				LArmOffset.X = MathHelper.Lerp(LArmOffset.X,0f,transition);
				
				float[] rlegrot = new float[3];
				float[] llegrot = new float[3];
				for(int i = 0; i < 3; i++)
				{
					rlegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_PhazonRegen[i],anim));
					llegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_PhazonRegen[i],anim));
				}
				float[] rarmrot = new float[2];
				float[] larmrot = new float[2];
				for(int i = 0; i < 2; i++)
				{
					rarmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_PhazonRegen[i],anim));
					larmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_PhazonRegen[i],anim));
				}
				
				if(NPC.direction == 1)
				{
					for(int i = 0; i < 3; i++)
					{
						RLegRot[i] = MathHelper.Lerp(RLegRot[i],rlegrot[i],transition);
						LLegRot[i] = MathHelper.Lerp(LLegRot[i],llegrot[i],transition);
					}
					for(int i = 0; i < 2; i++)
					{
						RArmRot[i] = MathHelper.Lerp(RArmRot[i],rarmrot[i],transition);
						LArmRot[i] = MathHelper.Lerp(LArmRot[i],larmrot[i],transition);
					}
				}
				else
				{
					for(int i = 0; i < 3; i++)
					{
						RLegRot[i] = MathHelper.Lerp(RLegRot[i],llegrot[i],transition);
						LLegRot[i] = MathHelper.Lerp(LLegRot[i],rlegrot[i],transition);
					}
					for(int i = 0; i < 2; i++)
					{
						RArmRot[i] = MathHelper.Lerp(RArmRot[i],larmrot[i],transition);
						LArmRot[i] = MathHelper.Lerp(LArmRot[i],rarmrot[i],transition);
					}
				}
				
				BodyRot = MathHelper.Lerp(BodyRot,-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,BodyAnim_PhazonRegen,anim)),transition);
				RCannonRot[0] = BodyRot;
				RCannonRot[1] = BodyRot;
				LCannonRot[0] = BodyRot;
				LCannonRot[1] = BodyRot;
				HeadRot[0] = BodyRot;
			}
			
			if(type == "death")
			{
				float offset = 0f;
				if(anim >= 5)
				{
					offset = 5f * (anim-5);
				}
				RArmOffset.X = MathHelper.Lerp(RArmOffset.X,offset,transition);
				LArmOffset.X = MathHelper.Lerp(LArmOffset.X,-offset,transition);
				
				offset = 0f;
				if(anim >= 5)
				{
					offset = 10f * (anim-5);
				}
				RArmOffset.X = MathHelper.Lerp(RArmOffset.X,offset,transition);
				LArmOffset.X = MathHelper.Lerp(LArmOffset.X,-offset,transition);
				
				float[] rlegrot = new float[3];
				float[] llegrot = new float[3];
				for(int i = 0; i < 3; i++)
				{
					rlegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_Death[i],anim));
					llegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_Death[i],anim));
				}
				float[] rarmrot = new float[2];
				float[] larmrot = new float[2];
				for(int i = 0; i < 2; i++)
				{
					rarmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_Death[i],anim));
					larmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Death[i],anim));
				}
				
				if(NPC.direction == 1)
				{
					for(int i = 0; i < 3; i++)
					{
						RLegRot[i] = MathHelper.Lerp(RLegRot[i],rlegrot[i],transition);
						LLegRot[i] = MathHelper.Lerp(LLegRot[i],llegrot[i],transition);
					}
					for(int i = 0; i < 2; i++)
					{
						RArmRot[i] = MathHelper.Lerp(RArmRot[i],rarmrot[i],transition);
						LArmRot[i] = MathHelper.Lerp(LArmRot[i],larmrot[i],transition);
					}
				}
				else
				{
					for(int i = 0; i < 3; i++)
					{
						RLegRot[i] = MathHelper.Lerp(RLegRot[i],llegrot[i],transition);
						LLegRot[i] = MathHelper.Lerp(LLegRot[i],rlegrot[i],transition);
					}
					for(int i = 0; i < 2; i++)
					{
						RArmRot[i] = MathHelper.Lerp(RArmRot[i],larmrot[i],transition);
						LArmRot[i] = MathHelper.Lerp(LArmRot[i],rarmrot[i],transition);
					}
				}
				
				BodyRot = MathHelper.Lerp(BodyRot,-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,BodyAnim_Death,anim)),transition);
				RCannonRot[0] = BodyRot;
				RCannonRot[1] = BodyRot;
				LCannonRot[0] = BodyRot;
				LCannonRot[1] = BodyRot;
				HeadRot[0] = MathHelper.Lerp(HeadRot[0],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,HeadAnim_Death,anim)),transition);
			}
		}
		void SetBodyOffset(float hOffset = 0f)
		{
			BodyPos[0] = NPC.Center + (CurrentBodyPos[0] + BodyOffset) * fullScale;
			RLegPos[2] = RLegPos[1] + Angle.AngleFlip(RLegOffsetRot[2] + RLegRot[1], NPC.direction).ToRotationVector2()*RLegDist[2] * fullScale;
			LLegPos[2] = LLegPos[1] + Angle.AngleFlip(LLegOffsetRot[2] + LLegRot[1], NPC.direction).ToRotationVector2()*LLegDist[2] * fullScale;
			if(RLegPos[2].Y >= LLegPos[2].Y)
			{
				BodyOffset.Y = 98f - ((RLegPos[2].Y+26f) - BodyPos[0].Y) + hOffset;
			}
			else
			{
				BodyOffset.Y = 98f - ((LLegPos[2].Y+26f) - BodyPos[0].Y) + hOffset;
			}
		}
		
		
		float[] PhazonArmorRegenAlpha = new float[4];
		
		Vector2 PhazonAppearPosition,
				PhazonDisappearPosition;
		ReLogic.Utilities.SlotId PhazonAppearSound;
		
		NPC[] DarkPortal = new NPC[4];
		
		
		float clawGlowAlpha = 0f;
		int glowNum = 1;
		float lClawGlowAlpha = 0f;
		int lGlowNum = -1;
		float rClawGlowAlpha = 0f;
		int rGlowNum = -1;
		
		NPC AbsorbProj;
		
		int addedAbsorbDamage = 0;
		int absorbDamageMax = 200;
		
		int grounded = 0;
		bool eyeFlame = false;
		
		int clawDamage = 80;
		int shockwaveDamage = 120;
		int laserDamage = 100;
		int grenadeDamage = 60;
		float grenadeSpeed = 18f;
		float grenadeGravity = 0.35f;
		int grenadeTimeBeforeGravity = 20;
		
		public static Color minGlowColor = new Color(96,200,255,10);
		public static Color maxGlowColor = new Color(255,220,0,10);
		
		public override bool CheckDead()
		{
			if (NPC.ai[0] != 3)
			{
				for(int i = 0; i < NPC.ai.Length; i++)
				{
					NPC.ai[i] = 0;
				}
				NPC.ai[0] = 3;
				NPC.damage = 0;
				NPC.netUpdate = true;
				NPC.life = NPC.lifeMax;
				NPC.dontTakeDamage = true;
				Body.dontTakeDamage = true;
				return false;
			}
			/*else
			{
				float heightdiff = 202-NPC.height;
				NPC.position.Y += heightdiff;
				BodyOffset.Y -= heightdiff;
			}*/
			return true;
		}
		
		public override void AI()
		{
			int numH = 202;
			
			float xDiff = (Main.player[NPC.target].Center.X+Main.player[NPC.target].velocity.X) - cannonTargetPos.X;
			float yDiff = (Main.player[NPC.target].Center.Y+Main.player[NPC.target].velocity.Y) - cannonTargetPos.Y;
			cannonTargetPos.X += xDiff*0.1f;
			cannonTargetPos.Y += yDiff*0.1f;
			
			if (Main.player[NPC.target].dead || Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) > 3000f || Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 3000f)
			{
				NPC.TargetClosest(true);
				if (Main.player[NPC.target].dead || Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) > 3000f || Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 3000f)
				{
					// despawn
					
					fullAlpha -= 0.2f;
					if(fullAlpha <= 0f)
					{
						NPC.alpha = 255;
						NPC.active = false;
					}
				}
			}
			else
			{
				if(NPC.ai[0] != 2)
				{
					fullAlpha = Math.Min(fullAlpha+0.02f,1f);
				}
				
				// spawn phase
				if(NPC.ai[0] == 0)
				{
					if(NPC.velocity.Y == 0f)
						NPC.ai[1] = 1;

					if(NPC.ai[1] == 1)
					{
						if(anim_Spawn < 2f)
						{
							anim_Spawn += 0.2f;
						}
						else
						{
							NPC.ai[2]++;
							if(NPC.ai[2] > 80)
							{
								eyeFlame = true;
								head_SpawnTransition = Math.Min(head_SpawnTransition + 0.05f, 1f);
								cannonTargetTransition = Math.Min(cannonTargetTransition + 0.05f, 1f);
							}
							if(NPC.ai[2] > 120)
							{
								anim_Spawn = Math.Min(anim_Spawn + 0.05f,3f);
								if(anim_Spawn >= 3f)
								{
									for(int i = 0; i < NPC.ai.Length; i++)
									{
										NPC.ai[i] = 0;
									}
									NPC.ai[0] = 1;
									anim_Walk = 1f;
									if(NPC.direction == -1)
									{
										anim_Walk = 5f;
									}
									NPC.TargetClosest(true);
									NPC.netUpdate = true;
								}
							}
						}
					}
					
					float[] rlegrot = new float[3],
							llegrot = new float[3],
							rarmrot = new float[2],
							larmrot = new float[2];
					for(int i = 0; i < 3; i++)
					{
						rlegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_Spawn[i],anim_Spawn));
						llegrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_Spawn[i],anim_Spawn));
					}
					BodyRot = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,BodyAnim_Spawn,anim_Spawn));
					for(int i = 0; i < 2; i++)
					{
						rarmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_Spawn[i],anim_Spawn));
						larmrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Spawn[i],anim_Spawn));
					}
					RCannonRot[0] = BodyRot;
					RCannonRot[1] = BodyRot;
					LCannonRot[0] = BodyRot;
					LCannonRot[1] = BodyRot;
					
					RLegRot = rlegrot;
					LLegRot = llegrot;
					RArmRot = rarmrot;
					LArmRot = larmrot;
					if(NPC.direction == -1)
					{
						RLegRot = llegrot;
						LLegRot = rlegrot;
						RArmRot = larmrot;
						LArmRot = rarmrot;
					}
					
					HeadRot[0] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,HeadAnim_Spawn,anim_Spawn));
					
					float headrot = Angle.Vector2Angle(HeadPos[0], Main.player[NPC.target].Center, NPC.direction, 1, 0.25f, -0.3f, 0.3f);
					
					if(head_SpawnTransition > 0)
					{
						HeadRot[0] = MathHelper.Lerp(HeadRot[0],headrot,head_SpawnTransition);
					}
					
					BodyOffset.Y = Angle.LerpArray(0f,BodyOffset_Spawn,anim_Spawn);
				}
				// main phase
				else if(NPC.ai[0] == 1)
				{
					NPC.damage = damage;
					
					// walk, absorb, & attack
					if(NPC.ai[1] == 0)
					{
						Player player = Main.player[NPC.target];
						
						float speed = 0.075f * (1.35f - 0.35f*(NPC.life/NPC.lifeMax));
						
						anim_Walk += speed;
						if(anim_Walk >= 9)
						{
							anim_Walk = 1f;
						}
						
						float moveSpeed = legAnim_Walk_Speed[(int)(anim_Walk-1)] * speed;
						
						if(NPC.direction == 1 && player.Center.X < NPC.position.X)
						{
							this.ChangeDir(-1);
						}
						if(NPC.direction == -1 && player.Center.X > NPC.position.X+NPC.width)
						{
							this.ChangeDir(1);
						}
						
						if(NPC.direction == 1)
						{
							if (NPC.velocity.X < 0f)
							{
								NPC.velocity.X *= 0.98f;
							}
							NPC.velocity.X += 0.1f * (speed / 0.075f);
						}
						else if(NPC.direction == -1)
						{
							if (NPC.velocity.X > 0f)
							{
								NPC.velocity.X *= 0.98f;
							}
							NPC.velocity.X -= 0.1f * (speed / 0.075f);
						}
						if (NPC.velocity.X > moveSpeed)
						{
							NPC.velocity.X = moveSpeed;
						}
						if (NPC.velocity.X < -moveSpeed)
						{
							NPC.velocity.X = -moveSpeed;
						}
						
						
						cannonTargetTransition = Math.Min(cannonTargetTransition + 0.05f,1f);
						
						HeadRot[0] = Angle.Vector2Angle(HeadPos[0], player.Center, NPC.direction, 1, 0.25f, -0.3f, 0.3f);
						
						SetAnimation("walk", anim_Walk);
						
						SetBodyOffset();
						
						
						// Absorb
						if(NPC.ai[2] < 60)
						{
							anim_Walk_AbsorbTransition = Math.Max(anim_Walk_AbsorbTransition - 0.05f, 0f);
							NPC.ai[2]++;
						}
						else
						{
							anim_Walk_AbsorbTransition = Math.Min(anim_Walk_AbsorbTransition + 0.05f, 1f);

							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								if (AbsorbProj == null || !AbsorbProj.active)
								{
									Vector2 spawnPos = LArmPos[4];
									var entitySource = NPC.GetSource_FromAI();
									int a = NPC.NewNPC(entitySource, (int)spawnPos.X, (int)spawnPos.Y, ModContent.NPCType<OmegaPirateAbsorbField>(), NPC.whoAmI, NPC.whoAmI);
									AbsorbProj = Main.npc[a];

									if (addedAbsorbDamage > 0)
									{
										AbsorbProj.ai[2] = addedAbsorbDamage;
									}
									else
									{
										addedAbsorbDamage = (int)AbsorbProj.ai[2];
									}
								}
								else
								{
									AbsorbProj.Center = LArmPos[4];
									addedAbsorbDamage = (int)AbsorbProj.ai[2];
								}
								AbsorbProj.netUpdate = true;
							}
						}						
						SetAnimation("absorb", anim_Walk, anim_Walk_AbsorbTransition);						
						
						// Claw Attack
						if(NPC.ai[3] < 30)
						{
							if(Math.Abs(player.Center.X - NPC.Center.X) < 240 && player.position.Y > NPC.position.Y && player.position.Y < NPC.position.Y+numH)
							{
								NPC.ai[3]++;
							}
							else
							{
								if(NPC.ai[3] > 0)
								{
									NPC.ai[3]--;
								}
							}
							anim_ClawTransition = Math.Max(anim_ClawTransition-0.05f,0f);
						}
						else if(NPC.ai[3] >= 30 && NPC.ai[3] <= 32)
						{
							rGlowNum = 3;
							
							anim_ClawTransition = Math.Min(anim_ClawTransition+0.05f,1f);
							anim_Claw = Math.Min(anim_Claw+0.125f,4f);
							
							if(anim_Claw >= 1.9f && NPC.ai[3] == 30)
							{
								SoundEngine.PlaySound(Sounds.NPCs.OmegaPirate_SwipeSound, RArmPos[4]);
								NPC.ai[3] = 31;
							}
							
							if(anim_Claw >= 2.9f && NPC.ai[3] <= 31)
							{
								Vector2 clawPos = RArmPos[4];
								float trot = (float)Math.Atan2(player.Center.Y-clawPos.Y,(player.Center.X-clawPos.X)*NPC.direction);
								trot *= 0.9f;
								if(trot > 1.5f)
								{
									trot = 1.5f;
								}
								if(trot < -0.5f)
								{
									trot = -0.5f;
								}
								float clawProjSpeed = 9f;
								Vector2 clawVel = new Vector2((float)Math.Cos(trot)*NPC.direction,(float)Math.Sin(trot))*clawProjSpeed;
								
								int slash = Projectile.NewProjectile(NPC.GetSource_FromAI(), clawPos.X,clawPos.Y,clawVel.X,clawVel.Y,ModContent.ProjectileType<Projectiles.Boss.OmegaPirateSlash>(),(int)((float)clawDamage/2f),8f);
								Main.projectile[slash].ai[0] = NPC.whoAmI;
								Main.projectile[slash].spriteDirection = -NPC.direction;
								NPC.ai[3] = 32;
							}
							
							if(anim_Claw >= 4f)
							{
								NPC.ai[3] = 33;
							}
						}
						else if(NPC.ai[3] >= 33)
						{
							NPC.ai[3]++;
							if(NPC.ai[3] > 90)
							{
								NPC.ai[3] = 0;
							}
							anim_ClawTransition = Math.Max(anim_ClawTransition-0.05f,0f);
						}
						if(anim_ClawTransition <= 0)
						{
							anim_Claw = 1f;
						}
						
						float[] armrot = new float[2];
						for(int i = 0; i < 2; i++)
						{
							armrot[i] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,ArmAnim_Claw[i],anim_Claw));
							
							RArmRot[i] = MathHelper.Lerp(RArmRot[i],armrot[i],anim_ClawTransition);
						}
						
						// Grenade Cannons
						if(Math.Abs(player.Center.X - NPC.Center.X) >= 200 || player.position.Y+player.height < Body.position.Y+Body.height)
						{
							NPC.ai[6]++;
							NPC.ai[7]++;
						}
						if(NPC.ai[6] > 90)
						{
							Vector2 grenadeVel = TrajectoryVelocity(RCannonPos[1],Main.player[NPC.target].Center,grenadeSpeed,grenadeGravity,grenadeTimeBeforeGravity);
							int nade = Projectile.NewProjectile(NPC.GetSource_FromAI(), RCannonPos[2].X,RCannonPos[2].Y,grenadeVel.X,grenadeVel.Y,ModContent.ProjectileType<Projectiles.Boss.OmegaPirateGrenade>(),(int)((float)grenadeDamage/2f),1f);
							
							SoundEngine.PlaySound(Sounds.NPCs.ElitePirate_CannonFireSound, RCannonPos[2]);
							rCannonRecoil = true;
							NPC.ai[6] = Main.rand.Next(71);
						}
						if(NPC.ai[7] > 90)
						{
							Vector2 grenadeVel = TrajectoryVelocity(LCannonPos[1],Main.player[NPC.target].Center,grenadeSpeed,grenadeGravity,grenadeTimeBeforeGravity);
							int nade = Projectile.NewProjectile(NPC.GetSource_FromAI(), LCannonPos[2].X,LCannonPos[2].Y,grenadeVel.X,grenadeVel.Y,ModContent.ProjectileType<Projectiles.Boss.OmegaPirateGrenade>(),(int)((float)grenadeDamage/2f),1f);
							
							SoundEngine.PlaySound(Sounds.NPCs.ElitePirate_CannonFireSound, LCannonPos[2]);
							lCannonRecoil = true;
							NPC.ai[7] = Main.rand.Next(71);
						}
						
						// Jump
						if(Math.Abs(player.Center.X-NPC.Center.X) < 750 && player.Center.Y < Body.Center.Y && (player.velocity.Y == 0f || NPC.ai[4] >= 1))
						{
							NPC.ai[4]++;
						}
						else
						{
							if(NPC.ai[4] > 0)
							{
								NPC.ai[4]--;
							}
						}
						
						float maxPlayerDist = 900f;
						
						if(Math.Abs(player.Center.X-NPC.Center.X) >= maxPlayerDist)
						{
							NPC.ai[5] = Math.Max(NPC.ai[5],290);
						}
						
						if(NPC.ai[4] > 60)
						{
							//NPC.TargetClosest(true);
							NPC.netUpdate = true;
							//jump
							NPC.ai[1] = 1;
							//NPC.ai[2] = 0;
							NPC.ai[3] = 0;
							NPC.ai[4] = 0;
							NPC.ai[5] = Math.Min(NPC.ai[5],290);
							NPC.ai[6] = Math.Min(NPC.ai[6],60);
							NPC.ai[7] = Math.Min(NPC.ai[7],60);
						}
						else if(NPC.ai[3] < 30 || (NPC.ai[3] > 33 && anim_ClawTransition <= 0f))
						{
							NPC.ai[5]++;
							if(NPC.ai[5] > 300)
							{
								//NPC.TargetClosest(true);
								NPC.netUpdate = true;
								
								if(Math.Abs(player.Center.X-NPC.Center.X) < maxPlayerDist)
								{
									int num = 0;
									if(Math.Abs(player.Center.X-NPC.Center.X) >= 450)
									{
										num = Main.rand.Next((int)Math.Abs(player.Center.X-NPC.Center.X)-450);
									}
									
									if(num < 50 && player.Center.Y > Body.position.Y-150 && Math.Abs(player.Center.X-NPC.Center.X) < 600)
									{
										//shockwave
										NPC.ai[1] = 2;
									}
									else
									{
										//cannon
										NPC.ai[1] = 3;
									}
								}
								else
								{
									// leap
									NPC.ai[1] = 4;
								}
								
								NPC.ai[2] = 0;
								NPC.ai[3] = 0;
								NPC.ai[4] = 0;
								NPC.ai[5] = 0;
								NPC.ai[6] = 0;
								NPC.ai[7] = 0;
							}
						}
					}
					// jump
					else if(NPC.ai[1] == 1)
					{
						Player player = Main.player[NPC.target];
						
						if(NPC.ai[3] == 0)
						{
							if(NPC.velocity.X != 0f)
							{
								NPC.velocity.X *= 0.98f;
								NPC.velocity.X -= 0.1f * Math.Sign(NPC.velocity.X);
							}
							if(Math.Abs(NPC.velocity.X) <= 0.1f)
							{
								NPC.velocity.X = 0f;
							}
							
							anim_JumpTransition += 0.04f;
							if(anim_JumpTransition > 1f)
							{
								NPC.ai[3] = 1;
								anim_JumpTransition = 1f;
							}
						}
						if(NPC.ai[3] >= 1 && NPC.ai[3] <= 2)
						{
							if(NPC.ai[3] == 1)
							{
								NPC.velocity.X = MathHelper.Clamp((player.Center.X-NPC.Center.X)*0.025f,-10,10);
								NPC.velocity.Y = -20f;
								NPC.ai[3] = 2;
							}
							
							if(anim_Jump < 2f)
							{
								anim_Jump = Math.Min(anim_Jump+0.25f,2f);
							}
							else if(NPC.velocity.Y > 0f)
							{
								anim_Jump = Math.Min(anim_Jump+(NPC.velocity.Y/50f)+0.005f,3f);
							}
							
							if(NPC.velocity.Y == 0f)
							{
								NPC.ai[3] = 3;
							}
							
							anim_Walk = 1f;
						}
						if(NPC.ai[3] == 3)
						{
							NPC.velocity.X = 0f;
							if(anim_Jump < 3f)
							{
								anim_Jump += 0.1f;
							}
							anim_Jump = Math.Min(anim_Jump+0.1f,4f);
							if(anim_Jump >= 4f)
							{
								anim_JumpTransition -= 0.05f;
								if(anim_JumpTransition <= 0f)
								{
									anim_JumpTransition = 0f;
									NPC.TargetClosest(true);
									NPC.netUpdate = true;
									NPC.ai[1] = 0;
									//NPC.ai[2] = 0;
									NPC.ai[3] = 0;
									NPC.ai[4] = 0;
									NPC.ai[5] = Math.Min(NPC.ai[5],290);
									NPC.ai[6] = Math.Min(NPC.ai[6],60);
									NPC.ai[7] = Math.Min(NPC.ai[7],60);
									anim_Jump = 1f;
								}
							}
						}
						
						// Absorb
						if(NPC.ai[2] < 40)
						{
							anim_Walk_AbsorbTransition = Math.Max(anim_Walk_AbsorbTransition-0.05f,0f);
							NPC.ai[2]++;
						}
						else
						{
							anim_Walk_AbsorbTransition = Math.Min(anim_Walk_AbsorbTransition+0.05f,1f);

							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								if (AbsorbProj == null || !AbsorbProj.active)
								{
									Vector2 spawnPos = LArmPos[4];
									int a = NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnPos.X, (int)spawnPos.Y, ModContent.NPCType<OmegaPirateAbsorbField>(), NPC.whoAmI, NPC.whoAmI);
									AbsorbProj = Main.npc[a];

									if (addedAbsorbDamage > 0)
									{
										AbsorbProj.ai[2] = addedAbsorbDamage;
									}
									else
									{
										addedAbsorbDamage = (int)AbsorbProj.ai[2];
									}
								}
								else
								{
									AbsorbProj.Center = LArmPos[4];
									addedAbsorbDamage = (int)AbsorbProj.ai[2];
								}
								AbsorbProj.netUpdate = true;
							}
						}
						
						cannonTargetTransition = Math.Max(cannonTargetTransition - 0.05f,0f);
						
						HeadRot[0] = Angle.Vector2Angle(HeadPos[0], player.Center, NPC.direction, 1, 0.25f, -0.3f, 0.3f);
						
						SetAnimation("walk", anim_Walk);
						SetAnimation("absorb", anim_Walk, anim_Walk_AbsorbTransition);
						SetAnimation("jump", anim_Jump,anim_JumpTransition);
						SetAnimation("jump absorb", anim_Jump,anim_JumpTransition*anim_Walk_AbsorbTransition);
						
						SetBodyOffset();
					}
					// shockwave attack
					else if(NPC.ai[1] == 2)
					{
						if(AbsorbProj != null && AbsorbProj.active)
						{
							AbsorbProj.ai[3] = 1;
							AbsorbProj.Center = LArmPos[4];
						}
						
						if(NPC.velocity.X != 0f)
						{
							NPC.velocity.X *= 0.98f;
							NPC.velocity.X -= 0.1f * Math.Sign(NPC.velocity.X);
						}
						if(Math.Abs(NPC.velocity.X) <= 0.1f)
						{
							NPC.velocity.X = 0f;
						}
						
						if(NPC.ai[2] == 0)
						{
							if(anim_ShockwaveTransition <= 0f)
							{
								SoundEngine.PlaySound(Sounds.NPCs.OmegaPirate_GroundSlamVoice, HeadPos[0]);
								mouthAnim = 1;
							}
							anim_ShockwaveTransition += 0.075f;
							if(anim_ShockwaveTransition >= 1f)
							{
								anim_ShockwaveTransition = 1f;
								NPC.ai[2] = 1;
							}
						}
						if(NPC.ai[2] == 1)
						{
							anim_Shockwave += 0.075f;
							if(anim_Shockwave >= 5f)
							{
								anim_Shockwave = 5f;

								Vector2 shockPos = new Vector2(MathHelper.Lerp(RArmPos[4].X, LArmPos[4].X, 0.5f), NPC.position.Y + numH);

								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									int shock1 = Projectile.NewProjectile(NPC.GetSource_FromAI(), shockPos.X + 15f, shockPos.Y, 0f, 0f, ModContent.ProjectileType<Projectiles.Boss.OmegaPirateShockwave>(), (int)((float)(shockwaveDamage + addedAbsorbDamage) / 2f), 8f, Main.myPlayer, .5f);
									Main.projectile[shock1].localAI[0] = 1;
									Main.projectile[shock1].localAI[1] = (float)addedAbsorbDamage / absorbDamageMax;
									Main.projectile[shock1].spriteDirection = 1;
									int shock2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), shockPos.X - 15f, shockPos.Y, 0f, 0f, ModContent.ProjectileType<Projectiles.Boss.OmegaPirateShockwave>(), (int)((float)(shockwaveDamage + addedAbsorbDamage) / 2f), 8f, Main.myPlayer, .5f);
									Main.projectile[shock2].localAI[0] = 1;
									Main.projectile[shock2].localAI[1] = (float)addedAbsorbDamage / absorbDamageMax;
									Main.projectile[shock2].spriteDirection = -1;

									Main.projectile[shock1].netUpdate = Main.projectile[shock2].netUpdate = true;
									addedAbsorbDamage = 0;
								}
								SoundEngine.PlaySound(Sounds.NPCs.OmegaPirate_GroundSlamSound, shockPos);
								SoundEngine.PlaySound(Sounds.NPCs.ElitePirate_ShockwaveSound, shockPos);
								
								NPC.ai[2] = 2;
							}
							else
							{
								Color dustColor = Color.Lerp(minGlowColor,maxGlowColor,(float)addedAbsorbDamage/absorbDamageMax);
								int size = 60;
								for(int i = 0; i < anim_Shockwave; i++)
								{
									int dust1 = Dust.NewDust(RArmPos[4]-new Vector2(size/2,size/2), size, size, 63, 0f, 0f, 100, dustColor, 1f+i);
									Main.dust[dust1].noGravity = true;
								}
								for(int i = 0; i < anim_Shockwave; i++)
								{
									int dust2 = Dust.NewDust(LArmPos[4]-new Vector2(size/2,size/2), size, size, 63, 0f, 0f, 100, dustColor, 1f+i);
									Main.dust[dust2].noGravity = true;
								}
								
								armGlowAnim = true;
								lGlowNum = 1;
								rGlowNum = 1;
							}
							anim_Walk = 1f;
							if(NPC.direction == -1)
							{
								anim_Walk = 5f;
							}
						}
						if(NPC.ai[2] == 2)
						{
							anim_ShockwaveTransition -= 0.015f;
							if(anim_ShockwaveTransition <= 0f)
							{
								NPC.TargetClosest(true);
								NPC.netUpdate = true;
								anim_ShockwaveTransition = 0f;
								NPC.ai[1] = 0;
								NPC.ai[2] = 0;
								NPC.ai[3] = 0;
								NPC.ai[4] = 0;
								NPC.ai[5] = Main.rand.Next(251);
								NPC.ai[6] = 0;
								NPC.ai[7] = 0;
								anim_Shockwave = 1f;
								anim_ShockwaveTransition_Head = 0f;
							}
						}
						
						if(anim_Shockwave >= 2f && anim_Shockwave < 5f)
						{
							anim_ShockwaveTransition_Head = Math.Min(anim_ShockwaveTransition_Head+0.1f,1f);
							cannonTargetTransition = Math.Max(cannonTargetTransition - 0.1f,0f);
						}
						else
						{
							anim_ShockwaveTransition_Head = Math.Max(anim_ShockwaveTransition_Head-0.025f,0f);
							cannonTargetTransition = Math.Min(cannonTargetTransition+0.0125f,1f);
						}
						
						HeadRot[0] = Angle.Vector2Angle(HeadPos[0], Main.player[NPC.target].Center, NPC.direction, 1, 0.25f, -0.3f, 0.3f);
						float hrot = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,HeadAnim_Shockwave,anim_Shockwave));
						HeadRot[0] = MathHelper.Lerp(HeadRot[0],hrot,anim_ShockwaveTransition_Head);
						
						anim_Walk_AbsorbTransition = Math.Max(anim_Walk_AbsorbTransition-0.05f,0f);
						
						SetAnimation("walk", anim_Walk);
						SetAnimation("absorb", anim_Walk, anim_Walk_AbsorbTransition);
						SetAnimation("shockwave", anim_Shockwave, anim_ShockwaveTransition);
						
						SetBodyOffset();
					}
					// fire cannons
					else if(NPC.ai[1] == 3)
					{
						Player player = Main.player[NPC.target];

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							if (AbsorbProj != null && AbsorbProj.active)
							{
								AbsorbProj.ai[3] = 1;
								AbsorbProj.Center = LArmPos[4];
								AbsorbProj.netUpdate = true;
							}
						}
						
						if(NPC.velocity.X != 0f)
						{
							NPC.velocity.X *= 0.98f;
							NPC.velocity.X -= 0.1f * Math.Sign(NPC.velocity.X);
						}
						if(Math.Abs(NPC.velocity.X) <= 0.1f)
						{
							NPC.velocity.X = 0f;
						}
						
						if(NPC.direction == 1 && player.Center.X < NPC.Center.X)
						{
							this.ChangeDir(-1);
						}
						if(NPC.direction == -1 && player.Center.X > NPC.Center.X)
						{
							this.ChangeDir(1);
						}
						
						if(NPC.ai[2] == 0)
						{
							if(anim_CannonFireTransition <= 0f)
							{
								SoundEngine.PlaySound(Sounds.NPCs.OmegaPirate_CannonVoice, HeadPos[0]);
								mouthAnim = 2;
							}
							anim_CannonFireTransition += 0.075f;
							if(anim_CannonFireTransition >= 1f)
							{
								NPC.ai[2] = 1;
								anim_CannonFireTransition = 1f;
							}
							cannonTargetPos = Vector2.Lerp(cannonTargetPos,player.Center,anim_CannonFireTransition);
						}
						int numCh = 50;
						if(NPC.ai[2] == 1)
						{
							Vector2 cSoundPos = Vector2.Lerp(RCannonPos[2],LCannonPos[2],0.5f);
							NPC.ai[3]++;
							
							if(NPC.ai[3] <= numCh)
							{
								laserAnim = true;
								rLaserAngle = Angle.Vector2Angle(RCannonPos[2], cannonTargetPos);
								lLaserAngle = Angle.Vector2Angle(LCannonPos[2], cannonTargetPos);
								
								Color dustColor = Color.Lerp(minGlowColor,maxGlowColor,(float)addedAbsorbDamage/absorbDamageMax);
								
								int size = 20;
								Vector2 rLaserPos = RCannonPos[2] + rLaserAngle.ToRotationVector2()*17f;
								for(int i = 0; i < (NPC.ai[3]/numCh)*5f; i++)
								{
									int dust1 = Dust.NewDust(rLaserPos-new Vector2(size/2,size/2), size, size, 63, 0f, 0f, 100, dustColor, 1f+i);
									Main.dust[dust1].noGravity = true;
								}
								Vector2 lLaserPos = LCannonPos[2] + lLaserAngle.ToRotationVector2()*17f;
								for(int i = 0; i < (NPC.ai[3]/numCh)*5f; i++)
								{
									int dust2 = Dust.NewDust(lLaserPos-new Vector2(size/2,size/2), size, size, 63, 0f, 0f, 100, dustColor, 1f+i);
									Main.dust[dust2].noGravity = true;
								}
							}
							if(NPC.ai[3] == 1)
							{
								SoundEngine.PlaySound(Sounds.NPCs.OmegaPirate_CannonChargeSound, cSoundPos);
							}
							
							if(NPC.ai[3] == numCh)
							{
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									Vector2 rLaserPos = RCannonPos[2] + rLaserAngle.ToRotationVector2() * 10f;
									Vector2 rLaserVel = rLaserAngle.ToRotationVector2() * 15f;
									int rLaser = Projectile.NewProjectile(NPC.GetSource_FromAI(), rLaserPos.X, rLaserPos.Y, rLaserVel.X, rLaserVel.Y, ModContent.ProjectileType<Projectiles.Boss.OmegaPirateLaser>(), (int)((float)(laserDamage + addedAbsorbDamage) / 2f), 8f);
									Main.projectile[rLaser].localAI[0] = (float)addedAbsorbDamage / absorbDamageMax;

									Vector2 lLaserPos = LCannonPos[2] + lLaserAngle.ToRotationVector2() * 10f;
									Vector2 lLaserVel = lLaserAngle.ToRotationVector2() * 15f;
									int lLaser = Projectile.NewProjectile(NPC.GetSource_FromAI(), lLaserPos.X, lLaserPos.Y, lLaserVel.X, lLaserVel.Y, ModContent.ProjectileType<Projectiles.Boss.OmegaPirateLaser>(), (int)((float)(laserDamage + addedAbsorbDamage) / 2f), 8f);
									Main.projectile[lLaser].localAI[0] = (float)addedAbsorbDamage / absorbDamageMax;

									Main.projectile[rLaser].netUpdate = Main.projectile[lLaser].netUpdate = true;
								}
								addedAbsorbDamage = 0;
								
								SoundEngine.PlaySound(Sounds.NPCs.OmegaPirate_CannonFireSound, cSoundPos);
							}
							if(NPC.ai[3] > numCh)
							{
								anim_CannonFire = Math.Min(anim_CannonFire+0.375f,2f);
							}
							if(anim_CannonFire >= 2f)
							{
								NPC.ai[2] = 2;
							}
							
							anim_Walk = 1f;
							if(NPC.direction == -1)
							{
								anim_Walk = 5f;
							}
						}
						if(NPC.ai[2] == 2)
						{
							cannonTargetTransition = Math.Min(cannonTargetTransition + 0.05f,1f);
							
							anim_CannonFire = Math.Max(anim_CannonFire-0.05f,1f);
							if(anim_CannonFire <= 1f)
							{
								anim_CannonFireTransition = Math.Max(anim_CannonFireTransition-0.1f,0f);
								if(anim_CannonFireTransition <= 0f)
								{
									NPC.TargetClosest(true);
									NPC.netUpdate = true;
									NPC.ai[1] = 0;
									NPC.ai[2] = 0;
									NPC.ai[3] = 0;
									NPC.ai[4] = 0;
									NPC.ai[5] = Main.rand.Next(201);
									NPC.ai[6] = 0;
									NPC.ai[7] = 0;
									anim_CannonFire = 1f;
									anim_CannonFireTransition = 0f;
								}
							}
						}
						else
						{
							cannonTargetTransition = Math.Max(cannonTargetTransition - 0.075f,0f);
						}
						
						if(NPC.ai[2] <= 1 && NPC.ai[3] <= numCh)
						{
							bodyCannonTargetRot = Angle.Vector2Angle(NPC.Center, cannonTargetPos, NPC.direction, 1, 0.25f, -0.3f, 0.3f);
							rightCannonTRot = Angle.Vector2Angle(RCannonPos[1], cannonTargetPos, NPC.direction);
							leftCannonTRot = Angle.Vector2Angle(LCannonPos[1], cannonTargetPos, NPC.direction);
						}
						
						anim_Walk_AbsorbTransition = Math.Max(anim_Walk_AbsorbTransition-0.05f,0f);
						
						SetAnimation("walk", anim_Walk);
						SetAnimation("absorb", anim_Walk, anim_Walk_AbsorbTransition);
						SetAnimation("cannon", anim_CannonFire, anim_CannonFireTransition);
						
						HeadRot[0] = Angle.Vector2Angle(HeadPos[0], player.Center, NPC.direction, 1, 0.25f, -0.3f, 0.3f);
						
						float bTargetRot = MathHelper.Lerp(0f,bodyCannonTargetRot,anim_CannonFireTransition);
						BodyRot += bTargetRot;
						RArmRot[0] -= bTargetRot * 0.25f;
						RArmRot[1] -= bTargetRot * 0.5f;
						LArmRot[0] -= bTargetRot * 0.25f;
						LArmRot[1] -= bTargetRot * 0.5f;
						
						RCannonRot[0] += rightCannonTRot * 0.75f;
						RCannonRot[1] += rightCannonTRot;
						LCannonRot[0] += leftCannonTRot * 0.75f;
						LCannonRot[1] += leftCannonTRot;
						
						SetBodyOffset();
					}
					// leap
					else if(NPC.ai[1] == 4)
					{
						Player player = Main.player[NPC.target];

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							if (AbsorbProj != null && AbsorbProj.active)
							{
								AbsorbProj.ai[3] = 1;
								AbsorbProj.Center = LArmPos[4];
								AbsorbProj.netUpdate = true;
							}
						}
						
						if(NPC.ai[2] == 0)
						{
							if(NPC.velocity.X != 0f)
							{
								NPC.velocity.X *= 0.98f;
								NPC.velocity.X -= 0.1f * Math.Sign(NPC.velocity.X);
							}
							if(Math.Abs(NPC.velocity.X) <= 0.1f)
							{
								NPC.velocity.X = 0f;
							}
							
							if(anim_LeapTransition <= 0f)
							{
								SoundEngine.PlaySound(Sounds.NPCs.OmegaPirate_LeapVoice, HeadPos[0]);
								mouthAnim = 3;
							}
							anim_LeapTransition += 0.04f;
							if(anim_LeapTransition > 1f)
							{
								anim_LeapTransition = 1f;
								NPC.ai[3]++;
								if(NPC.ai[3] > 30)
								{
									NPC.ai[2] = 1;
									NPC.ai[3] = 0;
								}
							}
						}
						if(NPC.ai[2] >= 1 && NPC.ai[2] <= 2)
						{
							if(NPC.ai[2] == 1)
							{
								Vector2 src = new Vector2(NPC.Center.X,NPC.position.Y+numH);
								Vector2 dest = new Vector2(player.Center.X,player.position.Y+player.height);
								NPC.velocity = TrajectoryVelocity(src,dest,25f,0.5f);
								NPC.ai[2] = 2;
							}
							
							if(anim_Leap < 3f)
							{
								anim_Leap = Math.Min(anim_Leap+0.25f,3f);
							}
							else if(NPC.velocity.Y > 0f)
							{
								anim_Leap = Math.Min(anim_Leap+(NPC.velocity.Y/50f)+0.005f,4f);
							}
							
							if(NPC.velocity.Y == 0f)
							{
								NPC.ai[2] = 3;
							}
							
							anim_Walk = 1f;
							if(NPC.direction == -1)
							{
								anim_Walk = 5f;
							}
							
						}
						if(NPC.ai[2] == 3)
						{
							NPC.velocity.X = 0f;
							if(anim_Leap < 4f)
							{
								anim_Leap += 0.1f;
							}
							anim_Leap = Math.Min(anim_Leap+0.1f,5f);
							if(anim_Leap >= 5f)
							{
								Vector2 shockPos = new Vector2(MathHelper.Lerp(RArmPos[4].X,LArmPos[4].X,0.5f),NPC.position.Y+numH);

								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									int shock1 = Projectile.NewProjectile(NPC.GetSource_FromAI(), shockPos.X + 15f, shockPos.Y, 0f, 0f, ModContent.ProjectileType<Projectiles.Boss.OmegaPirateShockwave>(), (int)((float)(shockwaveDamage + addedAbsorbDamage) / 2f), 8f, Main.myPlayer, .5f);
									Main.projectile[shock1].localAI[0] = 1;
									Main.projectile[shock1].localAI[1] = (float)addedAbsorbDamage / absorbDamageMax;
									Main.projectile[shock1].spriteDirection = 1;
									int shock2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), shockPos.X - 15f, shockPos.Y, 0f, 0f, ModContent.ProjectileType<Projectiles.Boss.OmegaPirateShockwave>(), (int)((float)(shockwaveDamage + addedAbsorbDamage) / 2f), 8f, Main.myPlayer, .5f);
									Main.projectile[shock2].localAI[0] = 1;
									Main.projectile[shock2].localAI[1] = (float)addedAbsorbDamage / absorbDamageMax;
									Main.projectile[shock2].spriteDirection = -1;

									Main.projectile[shock1].netUpdate = Main.projectile[shock2].netUpdate = true;
								}
								addedAbsorbDamage = 0;
								
								SoundEngine.PlaySound(Sounds.NPCs.OmegaPirate_GroundSlamSound, shockPos);
								SoundEngine.PlaySound(Sounds.NPCs.ElitePirate_ShockwaveSound, shockPos);
								
								NPC.ai[2] = 4;
							}
						}
						if(NPC.ai[2] >= 1 && NPC.ai[2] <= 3)
						{
							Color dustColor = Color.Lerp(minGlowColor,maxGlowColor,(float)addedAbsorbDamage/absorbDamageMax);
							int size = 60;
							for(int i = 0; i < anim_Leap; i++)
							{
								int dust1 = Dust.NewDust(RArmPos[4]-new Vector2(size/2,size/2), size, size, 63, 0f, 0f, 100, dustColor, 1f+i);
								Main.dust[dust1].noGravity = true;
							}
							for(int i = 0; i < anim_Leap; i++)
							{
								int dust2 = Dust.NewDust(LArmPos[4]-new Vector2(size/2,size/2), size, size, 63, 0f, 0f, 100, dustColor, 1f+i);
								Main.dust[dust2].noGravity = true;
							}
							
							armGlowAnim = true;
							lGlowNum = 1;
							rGlowNum = 1;
						}
						if(NPC.ai[2] == 4)
						{
							anim_LeapTransition -= 0.015f;
							if(anim_LeapTransition <= 0f)
							{
								anim_LeapTransition = 0f;
								NPC.TargetClosest(true);
								NPC.netUpdate = true;
								NPC.ai[1] = 0;
								NPC.ai[2] = 0;
								NPC.ai[3] = 0;
								NPC.ai[4] = 0;
								NPC.ai[5] = Main.rand.Next(201);
								NPC.ai[6] = 0;
								NPC.ai[7] = 0;
								anim_Leap = 1f;
							}
						}
						
						if(anim_Leap < 5f)
						{
							if(anim_Leap >= 4f)
							{
								anim_LeapTransition_Head = Math.Min(anim_LeapTransition_Head+0.1f,1f);
							}
							if(anim_Leap >= 2f)
							{
								cannonTargetTransition = Math.Max(cannonTargetTransition - 0.1f,0f);
							}
						}
						else
						{
							anim_LeapTransition_Head = Math.Max(anim_LeapTransition_Head-0.025f,0f);
							cannonTargetTransition = Math.Min(cannonTargetTransition+0.0125f,1f);
						}
						
						HeadRot[0] = Angle.Vector2Angle(HeadPos[0], Main.player[NPC.target].Center, NPC.direction, 1, 0.25f, -0.3f, 0.3f);
						float hrot = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,HeadAnim_Leap,anim_Leap));
						HeadRot[0] = MathHelper.Lerp(HeadRot[0],hrot,anim_LeapTransition_Head);
						
						anim_Walk_AbsorbTransition = Math.Max(anim_Walk_AbsorbTransition-0.05f,0f);
						
						SetAnimation("walk", anim_Walk);
						SetAnimation("absorb", anim_Walk, anim_Walk_AbsorbTransition);
						SetAnimation("leap", anim_Leap, anim_LeapTransition);
						
						SetBodyOffset();
					}
					
					if((RArmArmor == null || !RArmArmor.active) && (LArmArmor == null || !LArmArmor.active) &&
						(RLegArmor == null || !RLegArmor.active) && (LLegArmor == null || !LLegArmor.active))
					{
						for(int i = 0; i < NPC.ai.Length; i++)
						{
							NPC.ai[i] = 0;
						}
						NPC.ai[0] = 2;
					}
				}
				// phazon regen phase
				else if(NPC.ai[0] == 2)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						if (AbsorbProj != null && AbsorbProj.active)
						{
							AbsorbProj.ai[3] = 1;
							AbsorbProj.Center = LArmPos[4];
							AbsorbProj.netUpdate = true;
						}
					}
					cannonTargetTransition = Math.Max(cannonTargetTransition - 0.1f,0f);
					
					anim_Walk = 1f;
					anim_Walk_AbsorbTransition = 0f;
					anim_Jump = 1f;
					anim_JumpTransition = 0f;
					anim_Shockwave = 1f;
					anim_ShockwaveTransition = 0f;
					anim_ShockwaveTransition_Head = 0f;
					anim_Leap = 1f;
					anim_LeapTransition = 0f;
					anim_LeapTransition_Head = 0f;
					anim_Claw = 1f;
					anim_ClawTransition = 0f;
					anim_CannonFire = 1f;
					anim_CannonFireTransition = 0f;
					
					addedAbsorbDamage = 0;
					
					
					float moveSpeed = 0;
					
					Player player = Main.player[NPC.target];
					
					// animate into phase
					if(NPC.ai[1] == 0)
					{
						if(anim_PhazonStartTransition < 1f)
						{
							if(anim_PhazonStartTransition == 0f)
							{
								SoundEngine.PlaySound(Sounds.NPCs.OmegaPirateHurtVoice, HeadPos[0]);
								mouthAnim = 4+Main.rand.Next(2);
							}
							float animTransSpeed = 0.1f;
							moveSpeed = -15f * animTransSpeed;
							
							anim_PhazonStartTransition = Math.Min(anim_PhazonStartTransition+animTransSpeed,1f);
						}
						else
						{
							if(NPC.ai[2] < 90)
							{
								if(anim_PhazonStart < 2f)
								{
									float animSpeed = 0.01f + (0.09f * (anim_PhazonStart-1f));
									anim_PhazonStart = Math.Min(anim_PhazonStart+animSpeed,2f);
								}
								else
								{
									NPC.ai[2]++;
								}
							}
							else
							{
								if(anim_PhazonStart < 3f)
								{
									if(anim_PhazonStart == 2f)
									{
										SoundEngine.PlaySound(Sounds.NPCs.OmegaPirate_DamagedVoice, HeadPos[0]);
										mouthAnim = 7;
									}
									float animSpeed = 0.005f + (0.045f * (3f - anim_PhazonStart));
									anim_PhazonStart = Math.Min(anim_PhazonStart+animSpeed,3f);
								}
								else if(anim_PhazonStart < 4f)
								{
									anim_PhazonStart = Math.Min(anim_PhazonStart+0.05f,4f);
								}
								else if(NPC.ai[3] < 120)
								{
									if(NPC.ai[3] == 0)
									{
										SoundEngine.PlaySound(Sounds.NPCs.OmegaPirateCore_TransitionSound, HeadPos[0]);
									}
									NPC.ai[3]++;
									anim_PhazonStart = 4f + NPC.ai[3]/120f;
									if(NPC.ai[3] > 10)
									{
										float num = NPC.ai[3]-10f;
										if(num % 4 <= 1)
										{
											anim_PhazonStart -= Math.Min(0.1f * (num/55f),0.1f);
										}
									}
									if(NPC.ai[3] > 30)
									{
										bodyAlpha = 1f - (NPC.ai[3]-30)/90f;
									}
								}
								else if(anim_PhazonStart < 6f)
								{
									anim_PhazonStart = MathHelper.Clamp(anim_PhazonStart+0.025f,5f,6f);
								}
								else if(fullScale.X > 0f)
								{
									if(fullScale.X == 1f)
									{
										SoundEngine.PlaySound(Sounds.NPCs.OmegaPirateCore_Disappear, HeadPos[0]);
									}
									fullScale.X = Math.Max(fullScale.X-0.05f,0f);
								}
								else
								{
									fullAlpha = 0f;
									NPC.damage = 0;
									NPC.ai[1] = 1;
									NPC.ai[2] = 0;
									NPC.ai[4] = 0;

									if (Main.netMode != NetmodeID.MultiplayerClient)
									{
										NPC.netUpdate = true;
										NPC.ai[3] = 180 + Main.rand.Next(61);
										NPC.ai[5] = 10 + Main.rand.Next(30);
									}

									anim_PhazonStart = 1f;
									anim_PhazonStartTransition = 0f;
									NPC.dontTakeDamage = true;
								}
							}
						}
						
						SetAnimation("phazon start", anim_PhazonStart, anim_PhazonStartTransition);
					}
					// be invisible
					else if(NPC.ai[1] == 1)
					{
						if(NPC.ai[2] == 0)
						{
							PhazonDisappearPosition = NPC.position;
							int perc = 50;
							if(player.Center.X > NPC.Center.X)
							{
								perc = 66;
							}
							else
							{
								perc = 33;
							}
							int xdir = 1;
							if(Main.rand.Next(100) > perc)
							{
								xdir = -1;
							}
							PhazonAppearPosition.X = player.Center.X + (400+Main.rand.Next(301))*xdir;
							PhazonAppearPosition.Y = player.Center.Y-200f+Main.rand.Next(401);
							int yNum = 400;
							while(yNum > 0 && !Collision.SolidCollision(new Vector2(PhazonAppearPosition.X-(NPC.width/2),PhazonAppearPosition.Y-numH), NPC.width, numH+1))
							{
								PhazonAppearPosition.Y += 1f;
								yNum--;
							}
							while(yNum > 0 && Collision.SolidCollision(new Vector2(PhazonAppearPosition.X-(NPC.width/2),PhazonAppearPosition.Y-numH), NPC.width, numH))
							{
								PhazonAppearPosition.Y -= 1f;
								yNum--;
							}
						}
						else
						{
							if(SoundEngine.TryGetActiveSound(PhazonAppearSound, out ActiveSound result) && result.IsPlaying)
							{
								Vector2 vector = new Vector2(Main.screenPosition.X + (float)Main.screenWidth * 0.5f, Main.screenPosition.Y + (float)Main.screenHeight * 0.5f);
								float pan = (PhazonDisappearPosition.X - vector.X) / ((float)Main.screenWidth * 0.5f);
								if (pan < -1f)
								{
									pan = -1f;
								}
								if (pan > 1f)
								{
									pan = 1f;
								}
								// TODO: Fix
								//result.Pan = pan;
							}
						}

						if(NPC.ai[2]++ >= NPC.ai[3])
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								NPC.netUpdate = true;
								NPC.position.Y = PhazonAppearPosition.Y - numH;
								NPC.position.X = PhazonAppearPosition.X - (NPC.width / 2);
							}
							NPC.ai[1] = 2;
							NPC.ai[2] = 1;
							NPC.ai[3] = 0;
							NPC.ai[4] = 0;
							NPC.ai[5] = 0;
							anim_PhazonRegen = 1f;
							anim_PhazonRegenTransition = 1f;
							Body.dontTakeDamage = false;
							NPC.dontTakeDamage = false;
						}
						else
						{
							if(NPC.ai[2] > 0)
							{
								PhazonDisappearPosition = Vector2.Lerp(NPC.position,PhazonAppearPosition-new Vector2(NPC.width/2,numH),NPC.ai[2] / NPC.ai[3]);
							}
							if(NPC.ai[2] >= NPC.ai[5])
							{
								if(NPC.ai[4] <= 0 && Main.rand.Next(100) > 60)
								{
									PhazonAppearSound = SoundEngine.PlaySound(Sounds.NPCs.OmegaPirateCore_Voice, PhazonDisappearPosition);
									NPC.ai[4] = 1;//180;
								}
							}
						}
						/*if(NPC.ai[4] > 0)
						{
							NPC.ai[4]--;
						}*/
					}
					// appear and regen phazon armor
					else if(NPC.ai[1] == 2)
					{
						NPC.damage = damage;
						fullAlpha = 1f;
						
						if(NPC.ai[3] < NPC.lifeMax*0.1f)
						{
							NPC.direction = 1;
							if(NPC.Center.X > player.Center.X)
							{
								NPC.direction = -1;
							}
							
							if(anim_PhazonRegen < 2f)
							{
								if(fullScale.X < 1f)
								{
									if(fullScale.X == 0f)
									{
										SoundEngine.PlaySound(Sounds.NPCs.OmegaPirateCore_Appear, HeadPos[0]);
									}
									fullScale.X = Math.Min(fullScale.X+0.05f,1f);
								}
								else
								{
									anim_PhazonRegen = Math.Min(anim_PhazonRegen+0.075f,2f);
								}
							}
							else
							{
								if(anim_PhazonRegen <= 2f)
								{
									NPC.ai[2] = 1;
								}
								if(anim_PhazonRegen >= 3f)
								{
									NPC.ai[2] = -1;
								}
								anim_PhazonRegen = MathHelper.Clamp(anim_PhazonRegen+0.5f*NPC.ai[2],2f,3f);
								
								if(NPC.ai[5] == 0)
								{
									if (Main.netMode != NetmodeID.MultiplayerClient)
									{
										for (int i = 0; i < DarkPortal.Length; i++)
										{
											Vector2 spawnPos = NPC.Center;
											if (i == 0)
											{
												spawnPos.X -= (100 + Main.rand.Next(21));
												spawnPos.Y -= (75 + Main.rand.Next(26));
											}
											if (i == 1)
											{
												spawnPos.X += (100 + Main.rand.Next(21));
												spawnPos.Y -= (75 + Main.rand.Next(26));
											}
											if (i == 2)
											{
												spawnPos.X -= (75 + Main.rand.Next(26));
												spawnPos.Y += (100 + Main.rand.Next(21));
											}
											if (i == 3)
											{
												spawnPos.X += (75 + Main.rand.Next(26));
												spawnPos.Y += (100 + Main.rand.Next(21));
											}
											int dp = NPC.NewNPC(NPC.GetSource_FromAI(), (int)spawnPos.X, (int)spawnPos.Y, ModContent.NPCType<Omega_DarkPortal>(), NPC.whoAmI, 0, 0, 0, NPC.whoAmI);
											DarkPortal[i] = Main.npc[dp];
										}
										NPC.netUpdate = true;
									}
									NPC.ai[5] = 1;
								}
								
								float armorRegenSpeed = 0.015f;
								if(PhazonArmorRegenAlpha[0] < 1f)
								{
									if (Main.netMode != NetmodeID.MultiplayerClient)
									{
										if (PhazonArmorRegenAlpha[0] <= 0f)
											_rLegArmor = NPC.NewNPC(NPC.GetSource_FromAI(), (int)RLegPos[3].X, (int)RLegPos[3].Y, ModContent.NPCType<OmegaPirate_WeakPoint>(), NPC.whoAmI, NPC.whoAmI, 1);

										for (int i = 0; i < DarkPortal.Length; i++)
										{
											DarkPortal[i].ai[1] = -1;
											if (RLegArmor != null && RLegArmor.active)
											{
												DarkPortal[i].ai[0] = RLegArmor.position.X + Main.rand.Next(RLegArmor.width);
												DarkPortal[i].ai[1] = RLegArmor.position.Y + Main.rand.Next(RLegArmor.height);
												DarkPortal[i].netUpdate = true;
											}
										}
										NPC.netUpdate = true;
									}
									PhazonArmorRegenAlpha[0] = Math.Min(PhazonArmorRegenAlpha[0]+armorRegenSpeed,1f);
								}
								else if(PhazonArmorRegenAlpha[1] < 1f)
								{
									if (Main.netMode != NetmodeID.MultiplayerClient)
									{
										if (PhazonArmorRegenAlpha[1] <= 0f)
										{
											_lLegArmor = NPC.NewNPC(NPC.GetSource_FromAI(), (int)LLegPos[3].X, (int)LLegPos[3].Y, ModContent.NPCType<OmegaPirate_WeakPoint>(), NPC.whoAmI, NPC.whoAmI, 1);
										}
										for (int i = 0; i < DarkPortal.Length; i++)
										{
											DarkPortal[i].ai[1] = -1;
											if (LLegArmor != null && LLegArmor.active)
											{
												DarkPortal[i].ai[0] = LLegArmor.position.X + Main.rand.Next(LLegArmor.width);
												DarkPortal[i].ai[1] = LLegArmor.position.Y + Main.rand.Next(LLegArmor.height);
												DarkPortal[i].netUpdate = true;
											}
										}
										NPC.netUpdate = true;
									}
									PhazonArmorRegenAlpha[1] = Math.Min(PhazonArmorRegenAlpha[1]+armorRegenSpeed,1f);
								}
								else if(PhazonArmorRegenAlpha[2] < 1f)
								{
									if (Main.netMode != NetmodeID.MultiplayerClient)
									{
										if (PhazonArmorRegenAlpha[2] <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
										{
											_rArmArmor = NPC.NewNPC(NPC.GetSource_FromAI(), (int)RArmPos[2].X, (int)RArmPos[2].Y, ModContent.NPCType<OmegaPirate_WeakPoint>(), NPC.whoAmI, NPC.whoAmI);
										}
										for (int i = 0; i < DarkPortal.Length; i++)
										{
											DarkPortal[i].ai[1] = -1;
											if (RArmArmor != null && RArmArmor.active)
											{
												DarkPortal[i].ai[0] = RArmArmor.position.X + Main.rand.Next(RArmArmor.width);
												DarkPortal[i].ai[1] = RArmArmor.position.Y + Main.rand.Next(RArmArmor.height);
												DarkPortal[i].netUpdate = true;
											}
										}
										NPC.netUpdate = true;
									}
									PhazonArmorRegenAlpha[2] = Math.Min(PhazonArmorRegenAlpha[2]+armorRegenSpeed,1f);
								}
								else if(PhazonArmorRegenAlpha[3] < 1f)
								{
									if (Main.netMode != NetmodeID.MultiplayerClient)
									{
										if (PhazonArmorRegenAlpha[3] <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
										{
											_lArmArmor = NPC.NewNPC(NPC.GetSource_FromAI(), (int)LArmPos[2].X, (int)LArmPos[2].Y, ModContent.NPCType<OmegaPirate_WeakPoint>(), NPC.whoAmI, NPC.whoAmI);
										}
										for (int i = 0; i < DarkPortal.Length; i++)
										{
											DarkPortal[i].ai[1] = -1;
											if (LArmArmor != null && LArmArmor.active)
											{
												DarkPortal[i].ai[0] = LArmArmor.position.X + Main.rand.Next(LArmArmor.width);
												DarkPortal[i].ai[1] = LArmArmor.position.Y + Main.rand.Next(LArmArmor.height);
												DarkPortal[i].netUpdate = true;
											}
										}
										NPC.netUpdate = true;
									}
									PhazonArmorRegenAlpha[3] = Math.Min(PhazonArmorRegenAlpha[3]+armorRegenSpeed,1f);
								}
								else
								{
									// This should be deterministic enough to not have to network it.
									if (Main.netMode != NetmodeID.MultiplayerClient)
									{
										for (int i = 0; i < DarkPortal.Length; i++)
										{
											if (DarkPortal[i] != null && DarkPortal[i].active)
											{
												DarkPortal[i].ai[2] = 1;
												DarkPortal[i].netUpdate = true;
											}
										}
									}
									if(anim_PhazonRegenTransition > 0f)
									{
										if(anim_PhazonRegenTransition == 1f)
										{
											SoundEngine.PlaySound(Sounds.NPCs.OmegaPirateCore_TransitionSound2, HeadPos[0]);
										}
										anim_PhazonRegenTransition = Math.Max(anim_PhazonRegenTransition-0.025f,0f);
										bodyAlpha = Math.Min(bodyAlpha+0.025f,1f);
										Body.dontTakeDamage = true;
									}
									else
									{
										fullAlpha = 1f;
										bodyAlpha = 1f;
										anim_PhazonStart = 1f;
										anim_PhazonStartTransition = 0f;
										anim_PhazonRegen = 1f;
										anim_PhazonRegenTransition = 0f;
										NPC.damage = damage;
										Body.dontTakeDamage = true;
										for(int i = 0; i < NPC.ai.Length; i++)
										{
											NPC.ai[i] = 0;
										}
										NPC.ai[0] = 1;
										
										for(int i = 0; i < PhazonArmorRegenAlpha.Length; i++)
										{
											PhazonArmorRegenAlpha[i] = 0f;
										}
									}
								}
							}
						}
						else
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								for (int i = 0; i < DarkPortal.Length; i++)
								{
									if (DarkPortal[i] != null && DarkPortal[i].active)
									{
										DarkPortal[i].ai[1] = -1;
										DarkPortal[i].ai[2] = 1;
										DarkPortal[i].netUpdate = true;
									}
								}
							}
							
							if(anim_PhazonRegen < 3f)
							{
								anim_PhazonRegen = 3f;
							}
							else if(anim_PhazonRegen < 4f)
							{
								if(anim_PhazonRegen == 3f)
								{
									SoundEngine.PlaySound(Sounds.NPCs.OmegaPirateHurtVoice, HeadPos[0]);
								}
								float animSpeed = 0.1f;
								moveSpeed = -15f * animSpeed;
								anim_PhazonRegen = Math.Min(anim_PhazonRegen+animSpeed,4f);
							}
							else if(NPC.ai[4] < 20)
							{
								if(anim_PhazonRegen < 5f)
								{
									float animSpeed = 0.01f + (0.09f * (anim_PhazonRegen-4f));
									anim_PhazonRegen = Math.Min(anim_PhazonRegen+animSpeed,5f);
								}
								else
								{
									NPC.ai[4]++;
								}
							}
							else if(anim_PhazonRegen < 6)
							{
								anim_PhazonRegen = Math.Min(anim_PhazonRegen+0.05f,6f);
							}
							else if(fullScale.X > 0f)
							{
								if(fullScale.X == 1f)
								{
									SoundEngine.PlaySound(Sounds.NPCs.OmegaPirateCore_Disappear, HeadPos[0]);
								}
								fullScale.X = Math.Max(fullScale.X-0.05f,0f);
							}
							else
							{
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									fullAlpha = 0f;
									NPC.damage = 0;
									NPC.ai[1] = 1;
									NPC.ai[2] = 0;
									NPC.ai[3] = 120 + Main.rand.Next(121);
									NPC.ai[4] = 0;
									NPC.ai[5] = 10 + Main.rand.Next(30);
									anim_PhazonRegen = 1f;
									anim_PhazonRegenTransition = 0f;
									NPC.damage = 0;
									NPC.netUpdate = true;
									Body.netUpdate = true;
									NPC.dontTakeDamage = true;
									Body.dontTakeDamage = true;
								}
							}
						}
						
						SetAnimation("walk", anim_Walk);
						SetAnimation("phazon regen", anim_PhazonRegen, anim_PhazonRegenTransition);
					}
					
					if(NPC.direction == 1)
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X *= 0.98f;
						}
						NPC.velocity.X += 0.1f * moveSpeed;
					}
					else if(NPC.direction == -1)
					{
						if (NPC.velocity.X > 0f)
						{
							NPC.velocity.X *= 0.98f;
						}
						NPC.velocity.X -= 0.1f * moveSpeed;
					}
					if (NPC.velocity.X > Math.Abs(moveSpeed))
					{
						NPC.velocity.X = Math.Abs(moveSpeed);
					}
					if (NPC.velocity.X < -Math.Abs(moveSpeed))
					{
						NPC.velocity.X = -Math.Abs(moveSpeed);
					}
					
					SetBodyOffset();
				}
				//death anim
				else if(NPC.ai[0] == 3)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						for (int i = 0; i < DarkPortal.Length; i++)
						{
							if (DarkPortal[i] != null && DarkPortal[i].active)
							{
								DarkPortal[i].ai[1] = -1;
								DarkPortal[i].ai[2] = 1;
								DarkPortal[i].netUpdate = true;
							}
						}
					}
					
					if(RArmArmor != null && RArmArmor.active)
						RArmArmor.active = false;
					if(LArmArmor != null && LArmArmor.active)
						LArmArmor.active = false;
					if(RLegArmor != null && RLegArmor.active)
						RLegArmor.active = false;
					if(LLegArmor != null && LLegArmor.active)
						LLegArmor.active = false;
					
					bodyAlpha = Math.Min(bodyAlpha + 0.015f, 1f);
					fullAlpha = 1f;
					fullScale.X = Math.Min(fullScale.X + 0.1f, 1f);
					
					float animSpeed = 0f;
					if(anim_Death < 2f)
					{
						anim_DeathTransition = Math.Min(anim_DeathTransition+0.11f,1f);
						animSpeed = Math.Min(0.1f,2f-anim_Death);
					}
					else if(anim_Death < 3f)
					{
						float a = 0.01f + (0.02f * (anim_Death-2f));
						animSpeed = Math.Min(a,3f-anim_Death);
					}
					/*else if(anim_Death < 6f)
					{
						if(anim_Death > 3.5f)
						{
							eyeFlame = false;
						}
						float a = 0.005f + (0.02f * (anim_Death-3f));
						animSpeed = Math.Min(a,6f-anim_Death);
					}*/
					else
					{
						eyeFlame = false;
						NPC.ai[2]++;
						if(NPC.ai[2] > 60)
						{
							NPC.life = 0;
							NPC.HitEffect(0, 0);
							NPC.checkDead();
						}
					}
					/*float moveSpeed = Angle.LerpArray(0f,anim_Death_Speed,anim_Death) * animSpeed;
					
					if(NPC.direction == 1)
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X *= 0.98f;
						}
						NPC.velocity.X += 0.1f * moveSpeed;
					}
					else if(NPC.direction == -1)
					{
						if (NPC.velocity.X > 0f)
						{
							NPC.velocity.X *= 0.98f;
						}
						NPC.velocity.X -= 0.1f * moveSpeed;
					}
					if (NPC.velocity.X > Math.Abs(moveSpeed))
					{
						NPC.velocity.X = Math.Abs(moveSpeed);
					}
					if (NPC.velocity.X < -Math.Abs(moveSpeed))
					{
						NPC.velocity.X = -Math.Abs(moveSpeed);
					}*/
					NPC.velocity.X = 0;
					
					anim_Death = Math.Min(anim_Death+animSpeed,7f);
					SetAnimation("death", anim_Death, anim_DeathTransition);
					
					float hOffset = Angle.LerpArray(0f,anim_Death_HeightOffset,anim_Death);
					if(anim_Death < 6f)
					{
						SetBodyOffset(hOffset);
					}
				}
			}
			
			if(fullAlpha <= 0f)
			{
				NPC.GivenName = " ";
			}
			else
			{
				NPC.GivenName = "";
			}
			
			//mouth animations
			HeadRot[1] = -(float)Angle.ConvertToRadians((double)DefaultMouthAnim);
			if(mouthAnim == 1 || mouthAnim == 2 || mouthAnim == 3)
			{
				if(anim_MouthShockwave < 7f)
				{
					float animSpeed = 0.09f;
					if(mouthAnim == 2)
					{
						animSpeed = 0.12f;
					}
					if(mouthAnim == 3)
					{
						animSpeed = 0.067f;
					}
					anim_MouthShockwave = Math.Min(anim_MouthShockwave+animSpeed,7f);
					HeadRot[1] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,MouthAnim_Shockwave,anim_MouthShockwave));
				}
				else
				{
					anim_MouthShockwave = 1f;
					mouthAnim = 0;
				}
			}
			else if(mouthAnim == 4 || mouthAnim == 5 || mouthAnim == 6)
			{
				if(anim_MouthHurt < 7f)
				{
					float animSpeed = 0.09f;
					if(mouthAnim == 6)
					{
						animSpeed = 0.15f;
					}
					anim_MouthHurt = Math.Min(anim_MouthHurt+animSpeed,7f);
					HeadRot[1] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,MouthAnim_Hurt,anim_MouthHurt));
				}
				else
				{
					anim_MouthHurt = 1f;
					mouthAnim = 0;
				}
			}
			else if(mouthAnim == 7)
			{
				if(anim_MouthDamaged < 7f)
				{
					anim_MouthDamaged = Math.Min(anim_MouthDamaged+0.094f,7f);
					HeadRot[1] = -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,MouthAnim_Damaged,anim_MouthDamaged));
				}
				else
				{
					anim_MouthDamaged = 1f;
					mouthAnim = 0;
				}
			}
			
			if(cannonTargetTransition > 0f)
			{
				Vector2 RCannonTargetPos = RCannonPos[1] + TrajectoryVelocity(RCannonPos[1],Main.player[NPC.target].Center,grenadeSpeed,grenadeGravity,grenadeTimeBeforeGravity);
				Vector2 LCannonTargetPos = LCannonPos[1] + TrajectoryVelocity(LCannonPos[1],Main.player[NPC.target].Center,grenadeSpeed,grenadeGravity,grenadeTimeBeforeGravity);
				int rdir = Math.Sign(RCannonTargetPos.X-RCannonPos[1].X);
				int ldir = Math.Sign(LCannonTargetPos.X-LCannonPos[1].X);
				float RCannonTargetRot = Angle.Vector2Angle(RCannonPos[1], RCannonTargetPos, rdir, 1, 1f, -1.57f+BodyRot, 0.6f+BodyRot);
				float LCannonTargetRot = Angle.Vector2Angle(LCannonPos[1], LCannonTargetPos, ldir, 1, 1f, -1.57f+BodyRot, 0.6f+BodyRot);
				RCannonRot[0] = MathHelper.Lerp(RCannonRot[0],RCannonTargetRot*0.5f,cannonTargetTransition);
				RCannonRot[1] = MathHelper.Lerp(RCannonRot[1],RCannonTargetRot,cannonTargetTransition);
				LCannonRot[0] = MathHelper.Lerp(LCannonRot[0],LCannonTargetRot*0.5f,cannonTargetTransition);
				LCannonRot[1] = MathHelper.Lerp(LCannonRot[1],LCannonTargetRot,cannonTargetTransition);
			}
			if(rCannonRecoil)
			{
				rCannonRecoilAnim = Math.Min(rCannonRecoilAnim+0.375f,1f);
				if(rCannonRecoilAnim >= 1f)
				{
					rCannonRecoil = false;
				}
			}
			else
			{
				rCannonRecoilAnim = Math.Max(rCannonRecoilAnim-0.05f,0f);
			}
			if(rCannonRecoilAnim > 0f)
			{
				RCannonRot[0] -= (float)Angle.ConvertToRadians((double)MathHelper.Lerp(0f,RCannonAnim_CannonFire[1],rCannonRecoilAnim));
			}
			if(lCannonRecoil)
			{
				lCannonRecoilAnim = Math.Min(lCannonRecoilAnim+0.375f,1f);
				if(lCannonRecoilAnim >= 1f)
				{
					lCannonRecoil = false;
				}
			}
			else
			{
				lCannonRecoilAnim = Math.Max(lCannonRecoilAnim-0.05f,0f);
			}
			if(lCannonRecoilAnim > 0f)
			{
				LCannonRot[0] -= (float)Angle.ConvertToRadians((double)MathHelper.Lerp(0f,LCannonAnim_CannonFire[1],lCannonRecoilAnim));
			}
			
			if(laserAnim)
			{
				laserAlpha = Math.Min(laserAlpha+0.02f,1f);
				laserAnim = false;
			}
			else
			{
				laserAlpha = Math.Max(laserAlpha-0.1f,0f);
			}
			if(laserAlpha > 0f)
			{
				laserFrame += 0.33f;
				if(laserFrame > 1f)
				{
					laserFrame = 0f;
				}
			}
			else
			{
				laserFrame = 0f;
			}
			
			if(armGlowAnim)
			{
				armGlowAlpha = Math.Min(armGlowAlpha+0.02f,1f);
				armGlowAnim = false;
			}
			else
			{
				armGlowAlpha = Math.Max(armGlowAlpha-0.1f,0f);
			}
			
			SetPositions();
			
			if(bodyAlpha >= 1f)
			{
				if(eyeFlame)
				{
					Vector2 rEyePos = new Vector2(-1,-7);
					float rEyeOffsetRot = (float)Math.Atan2(rEyePos.Y,rEyePos.X*NPC.direction);
					float rEyeAngle = rEyeOffsetRot + HeadRot[0]*NPC.direction;
					Vector2 eyeR = HeadPos[0] + rEyeAngle.ToRotationVector2()*rEyePos.Length();
					
					Vector2 lEyePos = new Vector2(13,-7);
					float lEyeOffsetRot = (float)Math.Atan2(lEyePos.Y,lEyePos.X*NPC.direction);
					float lEyeAngle = lEyeOffsetRot + HeadRot[0]*NPC.direction;
					Vector2 eyeL = HeadPos[0] + lEyeAngle.ToRotationVector2()*lEyePos.Length();
					
					int dust = Dust.NewDust(eyeR - new Vector2(7,7), 10, 10, 87, NPC.velocity.X * 0.3f, NPC.velocity.Y * 0.3f, 100, Color.White, 0.8f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.3f;
					Main.dust[dust].velocity.Y -= 1f;
					
					dust = Dust.NewDust(eyeL - new Vector2(7,7), 10, 10, 87, NPC.velocity.X * 0.3f, NPC.velocity.Y * 0.3f, 100, Color.White, 0.8f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.3f;
					Main.dust[dust].velocity.Y -= 1f;
				}
			}
			else
			{
				if(fullAlpha > 0f)
				{
					float dustScale = 2f-fullScale.X;
					int dust = Dust.NewDust(Body.Center-new Vector2(Body.width/2,Body.height/2)*fullScale,(int)((float)Body.width*fullScale.X),(int)((float)Body.height*fullScale.Y),68,0f,0f,100,default(Color),dustScale);
					Main.dust[dust].noGravity = true;
					
					for(int j = 0; j < 3; j++)
					{
						NPC RArm = GetArm(false, j),
							LArm = GetArm(true, j),
							RLeg = GetLeg(false, j),
							LLeg = GetLeg(true, j);

						dust = Dust.NewDust(RArm.Center-new Vector2(RArm.width/2,RArm.height/2)*fullScale,(int)((float)RArm.width*fullScale.X),(int)((float)RArm.height*fullScale.Y),68,0f,0f,100,default(Color),dustScale);
						Main.dust[dust].noGravity = true;
						dust = Dust.NewDust(LArm.Center-new Vector2(LArm.width/2,LArm.height/2)*fullScale,(int)((float)LArm.width*fullScale.X),(int)((float)LArm.height*fullScale.Y),68,0f,0f,100,default(Color),dustScale);
						Main.dust[dust].noGravity = true;
						dust = Dust.NewDust(RLeg.Center-new Vector2(RLeg.width/2,RLeg.height/2)*fullScale,(int)((float)RLeg.width*fullScale.X),(int)((float)RLeg.height*fullScale.Y),68,0f,0f,100,default(Color),dustScale);
						Main.dust[dust].noGravity = true;
						dust = Dust.NewDust(LLeg.Center-new Vector2(LLeg.width/2,LLeg.height/2)*fullScale,(int)((float)LLeg.width*fullScale.X),(int)((float)LLeg.height*fullScale.Y),68,0f,0f,100,default(Color),dustScale);
						Main.dust[dust].noGravity = true;
					}
				}
			}
			
			clawGlowAlpha += 0.01f * glowNum;
			if(clawGlowAlpha > 1.5f)
			{
				clawGlowAlpha = 1.5f;
				glowNum = -1;
			}
			if(clawGlowAlpha < -1f)
			{
				clawGlowAlpha = -1f;
				glowNum = 1;
			}
			
			lClawGlowAlpha += 0.05f * lGlowNum;
			if(lClawGlowAlpha > 1f)
			{
				lClawGlowAlpha = 1f;
				lGlowNum = -1;
			}
			if(lClawGlowAlpha <= 0f)
			{
				lClawGlowAlpha = 0f;
			}
			rClawGlowAlpha += 0.05f * rGlowNum;
			if(rClawGlowAlpha > 1f)
			{
				rClawGlowAlpha = 1f;
				rGlowNum = -1;
			}
			if(rClawGlowAlpha <= 0f)
			{
				rClawGlowAlpha = 0f;
			}
			
			
			if(((NPC.position.X < Main.player[NPC.target].position.X && NPC.position.X+NPC.width > Main.player[NPC.target].position.X+Main.player[NPC.target].width) || (NPC.ai[0] == 0 && NPC.ai[1] == 0)) && NPC.position.Y+numH < Main.player[NPC.target].position.Y+Main.player[NPC.target].height - 16f)
			{
				grounded = Math.Min(grounded+1,15);
				NPC.velocity.Y += 0.5f;
				if(NPC.velocity.Y == 0f)
				{
					NPC.velocity.Y = 0.1f;
				}
			}
			else
			{
				if(Collision.SolidCollision(new Vector2(NPC.position.X,NPC.position.Y+numH-16f), NPC.width, 16))
				{
					if (NPC.velocity.Y > -4f)
					{
						if (NPC.velocity.Y > 0f)
						{
							NPC.velocity.Y = 0f;
						}
						if (NPC.velocity.Y > -0.2f)
						{
							NPC.velocity.Y = NPC.velocity.Y - 0.025f;
						}
						else
						{
							NPC.velocity.Y = NPC.velocity.Y - 0.2f;
						}
					}
				}
				else
				{
					NPC.velocity.Y += 0.5f;
				}
				
				if(!Collision.SolidCollision(NPC.position, NPC.width, numH+1) && NPC.velocity.Y == 0f)
				{
					NPC.velocity.Y = 0.1f;
				}
				
				bool fall = false;
				if(NPC.position.Y+numH < Main.player[NPC.target].position.Y && NPC.ai[0] <= 1 && (NPC.ai[1] <= 1 || NPC.ai[1] == 4))
				{
					fall = true;
				}
				Vector2 velocity = Collision.TileCollision(NPC.position,new Vector2(0f,Math.Max(NPC.velocity.Y,0f)),NPC.width,numH,fall,fall);
				NPC.velocity.Y = Math.Min(velocity.Y,NPC.velocity.Y);
				if(NPC.velocity.Y == 0f)
				{
					grounded = Math.Min(grounded+1,15);
				}
				else
				{
					grounded = Math.Max(grounded-1,0);
				}
			}
			if (NPC.velocity.Y > 10f)
			{
				NPC.velocity.Y = 10f;
			}
			
			
			if(Body != null && Body.active)
				Body.Center = BodyPos[1];
			
			for(int i = 0; i < 3; i++)
			{
				NPC RArm = GetArm(false, i),
					LArm = GetArm(true, i),
					RLeg = GetLeg(false, i),
					LLeg = GetLeg(true, i);

				if (RArm != null && RArm.active)
					RArm.Center = RArmPos[i+2];
				
				if(LArm != null && LArm.active)
					LArm.Center = LArmPos[i+2];
				
				if(RLeg != null && RLeg.active)
					RLeg.Center = RLegPos[i+3];
				
				if(LLeg != null && LLeg.active)
					LLeg.Center = LLegPos[i+3];
			}
			
			if(RCannon != null && RCannon.active)
				RCannon.Center = RCannonPos[2];

			if(LCannon != null && LCannon.active)
				LCannon.Center = LCannonPos[2];
			
			if(RArmArmor != null && RArmArmor.active)
				RArmArmor.Center = RArmPos[2];
			
			if(LArmArmor != null && LArmArmor.active)
				LArmArmor.Center = LArmPos[2];
			
			if(RLegArmor != null && RLegArmor.active)
				RLegArmor.Center = RLegPos[3];
			
			if(LLegArmor != null && LLegArmor.active)
				LLegArmor.Center = LLegPos[3];
		}
		
		void ChangeDir(int dir)
		{
			if(NPC.direction == dir)
			{
				return;
			}
			else
			{
				NPC.direction = dir;
				NPC.netUpdate = true;
			}
		}
		
		static Vector2 TrajectoryVelocity(Vector2 source,Vector2 destination,float speed,float gravity,int timeBeforeGravity = 0)
		{
			float tbg = timeBeforeGravity*speed;
			float x = destination.X - source.X;
			float y = destination.Y - source.Y - Math.Max(Math.Abs(x)-tbg,0f) * gravity;
			float angle = (float)Math.Atan2(y,x);
			
			return angle.ToRotationVector2()*speed;
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			NPC.spriteDirection = NPC.direction;
			SpriteEffects effects = SpriteEffects.None;
			if (NPC.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			
			Texture2D texCoreHead = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateCore_Head").Value,
					texCoreBody = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateCore_Body").Value,
					texCoreShoulderR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateCore_ArmShoulderRight").Value,
					texCoreShoulderL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateCore_ArmShoulderLeft").Value,
					texCoreArmR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateCore_ArmRight").Value,
					texCoreArmL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateCore_ArmLeft").Value,
					texCoreLegR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateCore_LegRight").Value,
					texCoreLegL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateCore_LegLeft").Value,
					texCoreLegLowerR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateCore_LegLowerRight").Value,
					texCoreLegLowerL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateCore_LegLowerLeft").Value,
					texCoreLegFootR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateCore_LegFootRight").Value,
					texCoreLegFootL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirateCore_LegFootLeft").Value,
					texCoreArmorShoulderR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmShoulderRight_Phazon").Value,
					texCoreArmorShoulderL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmShoulderLeft_Phazon").Value,
					texCoreArmorLegR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_LegRight_Phazon").Value,
					texCoreArmorLegL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_LegLeft_Phazon").Value;
			
			Color CoreColor = new Color(255,255,255,255)*fullAlpha * (1f - bodyAlpha);
			Color ArmorColor = new Color(255,255,255,200)*fullAlpha * (1f - bodyAlpha);
			
			if(bodyAlpha < 1f)
			{
				//left arm
				DrawLimbTexture(NPC,sb,texCoreArmL,LArmPos[1],RArmPos[1],LArmRot[1],RArmRot[1],new Vector2(10,6),CoreColor,CoreColor,fullScale,effects);
				//left shoulder
				DrawLimbTexture(NPC,sb,texCoreShoulderL,LArmPos[0],RArmPos[0],LArmRot[0],RArmRot[0],new Vector2(30,26),CoreColor,CoreColor,fullScale,effects);
				//left shoulder armor
				DrawLimbTexture(NPC,sb,texCoreArmorShoulderL,LArmPos[0],RArmPos[0],LArmRot[0],RArmRot[0],new Vector2(32,38),ArmorColor*PhazonArmorRegenAlpha[3],ArmorColor*PhazonArmorRegenAlpha[2],fullScale,effects);
				
				//left foot
				DrawLimbTexture(NPC,sb,texCoreLegFootL,LLegPos[2],RLegPos[2],LLegRot[2],RLegRot[2],new Vector2(14,4),CoreColor,CoreColor,fullScale,effects);
				//left leg lower
				DrawLimbTexture(NPC,sb,texCoreLegLowerL,LLegPos[1],RLegPos[1],LLegRot[1],RLegRot[1],new Vector2(12,6),CoreColor,CoreColor,fullScale,effects);
				//left leg
				DrawLimbTexture(NPC,sb,texCoreLegL,LLegPos[0],RLegPos[0],LLegRot[0],RLegRot[0],new Vector2(4,6),CoreColor,CoreColor,fullScale,effects);
				//left leg armor
				DrawLimbTexture(NPC,sb,texCoreArmorLegL,LLegPos[0],RLegPos[0],LLegRot[0],RLegRot[0],new Vector2(14,12),ArmorColor*PhazonArmorRegenAlpha[1],ArmorColor*PhazonArmorRegenAlpha[0],fullScale,effects);
				
				//body
				DrawLimbTexture(NPC,sb,texCoreBody,BodyPos[0],BodyPos[0],BodyRot,BodyRot,new Vector2(32,102),CoreColor,CoreColor,fullScale,effects);
				//head
				DrawLimbTexture(NPC,sb,texCoreHead,HeadPos[0],HeadPos[0],HeadRot[0],HeadRot[0],new Vector2(12,34),CoreColor,CoreColor,fullScale,effects);
				
				//right foot
				DrawLimbTexture(NPC,sb,texCoreLegFootR,RLegPos[2],LLegPos[2],RLegRot[2],LLegRot[2],new Vector2(14,4),CoreColor,CoreColor,fullScale,effects);
				//right leg lower
				DrawLimbTexture(NPC,sb,texCoreLegLowerR,RLegPos[1],LLegPos[1],RLegRot[1],LLegRot[1],new Vector2(12,6),CoreColor,CoreColor,fullScale,effects);
				//right leg
				DrawLimbTexture(NPC,sb,texCoreLegR,RLegPos[0],LLegPos[0],RLegRot[0],LLegRot[0],new Vector2(4,0),CoreColor,CoreColor,fullScale,effects);
				//right leg armor
				DrawLimbTexture(NPC,sb,texCoreArmorLegR,RLegPos[0],LLegPos[0],RLegRot[0],LLegRot[0],new Vector2(14,12),ArmorColor*PhazonArmorRegenAlpha[0],ArmorColor*PhazonArmorRegenAlpha[1],fullScale,effects);
				
				//right arm
				DrawLimbTexture(NPC,sb,texCoreArmR,RArmPos[1],LArmPos[1],RArmRot[1],LArmRot[1],new Vector2(10,6),CoreColor,CoreColor,fullScale,effects);
				//right shoulder
				DrawLimbTexture(NPC,sb,texCoreShoulderR,RArmPos[0],LArmPos[0],RArmRot[0],LArmRot[0],new Vector2(30,26),CoreColor,CoreColor,fullScale,effects);
				//right shoulder armor
				DrawLimbTexture(NPC,sb,texCoreArmorShoulderR,RArmPos[0],LArmPos[0],RArmRot[0],LArmRot[0],new Vector2(32,38),ArmorColor*PhazonArmorRegenAlpha[2],ArmorColor*PhazonArmorRegenAlpha[3],fullScale,effects);
			}
			
			Texture2D texHead = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_Head").Value,
					texJaw = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_HeadJaw").Value,
					texBody = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_Body").Value,
					texCannonArmR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_CannonArmRight").Value,
					texCannonArmL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_CannonArmLeft").Value,
					texCannonR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_CannonRight").Value,
					texCannonL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_CannonLeft").Value,
					texCannonR_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_CannonRight_Glow").Value,
					texCannonL_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_CannonLeft_Glow").Value,
					texCannonR_Glow2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_CannonRight_Glow2").Value,
					texCannonL_Glow2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_CannonLeft_Glow2").Value,
					texShoulderR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmShoulderRight").Value,
					texShoulderL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmShoulderLeft").Value,
					texShoulderR_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmShoulderRight_Glow").Value,
					texShoulderL_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmShoulderLeft_Glow").Value,
					texArmR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmRight").Value,
					texArmL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmLeft").Value,
					texArmR_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmRight_Glow").Value,
					texArmL_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmLeft_Glow").Value,
					texArmR_Glow2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmRight_Glow2").Value,
					texArmL_Glow2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmLeft_Glow2").Value,
					texArmR_Glow3 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_ArmRight_Glow3").Value,
					texLegR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_LegRight").Value,
					texLegL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_LegLeft").Value,
					texLegR_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_LegRight_Glow").Value,
					texLegL_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_LegLeft_Glow").Value,
					texLegLowerR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_LegLowerRight").Value,
					texLegLowerL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_LegLowerLeft").Value,
					texFootR = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_LegFootRight").Value,
					texFootL = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/OmegaPirate/OmegaPirate_LegFootLeft").Value;
			
			float fAlpha = bodyAlpha*fullAlpha;
			
			Color rArmColor = NPC.GetAlpha(Lighting.GetColor((int)RArmPos[4].X / 16, (int)RArmPos[4].Y / 16)) * fAlpha,
			lArmColor = NPC.GetAlpha(Lighting.GetColor((int)LArmPos[4].X / 16, (int)LArmPos[4].Y / 16)) * fAlpha,
			lShColor = NPC.GetAlpha(Lighting.GetColor((int)LArmPos[0].X / 16, (int)LArmPos[0].Y / 16)) * fAlpha,
			rShColor = NPC.GetAlpha(Lighting.GetColor((int)RArmPos[0].X / 16, (int)RArmPos[0].Y / 16)) * fAlpha,
			lFootColor = NPC.GetAlpha(Lighting.GetColor((int)LLegPos[2].X / 16, (int)LLegPos[2].Y / 16)) * fAlpha,
			rFootColor = NPC.GetAlpha(Lighting.GetColor((int)RLegPos[2].X / 16, (int)RLegPos[2].Y / 16)) * fAlpha,
			lLLegColor = NPC.GetAlpha(Lighting.GetColor((int)LLegPos[4].X / 16, (int)LLegPos[4].Y / 16)) * fAlpha,
			rLLegColor = NPC.GetAlpha(Lighting.GetColor((int)RLegPos[4].X / 16, (int)RLegPos[4].Y / 16)) * fAlpha,
			lLegColor = NPC.GetAlpha(Lighting.GetColor((int)LLegPos[3].X / 16, (int)LLegPos[3].Y / 16)) * fAlpha,
			rLegColor = NPC.GetAlpha(Lighting.GetColor((int)RLegPos[3].X / 16, (int)RLegPos[3].Y / 16)) * fAlpha,
			lCannonColor = NPC.GetAlpha(Lighting.GetColor((int)LCannonPos[2].X / 16, (int)LCannonPos[2].Y / 16)) * fAlpha,
			rCannonColor = NPC.GetAlpha(Lighting.GetColor((int)RCannonPos[2].X / 16, (int)RCannonPos[2].Y / 16)) * fAlpha,
			bodyColor = NPC.GetAlpha(Lighting.GetColor((int)BodyPos[1].X / 16, (int)BodyPos[1].Y / 16)) * fAlpha,
			headColor = NPC.GetAlpha(Lighting.GetColor((int)HeadPos[0].X / 16, (int)HeadPos[0].Y / 16)) * fAlpha;
			
			Color rClawGlowColor = new Color(Color.White.R,Color.White.G,Color.White.B,100) * MathHelper.Clamp(clawGlowAlpha+rClawGlowAlpha,0f,1f) * fAlpha;
			Color lClawGlowColor = new Color(Color.White.R,Color.White.G,Color.White.B,100) * MathHelper.Clamp(clawGlowAlpha+lClawGlowAlpha,0f,1f) * fAlpha;
			
			Color glowColor = Color.Lerp(minGlowColor,maxGlowColor,(float)addedAbsorbDamage/absorbDamageMax) * fAlpha;
			
			Color laserColor = new Color(242,255,137,100) * laserAlpha * laserFrame * fAlpha;
			
			//left shoulder
			int lShFrame = 0;
			if(NPC.spriteDirection == 1)
			{
				if(LArmArmor != null && LArmArmor.active)
				{
					lShFrame = 0;
				}
				else
				{
					lShFrame = 1;
				}
			}
			else
			{
				if(RArmArmor != null && RArmArmor.active)
				{
					lShFrame = 0;
				}
				else
				{
					lShFrame = 1;
				}
			}
			DrawLimbTexture(NPC,sb,texShoulderL,LArmPos[0],RArmPos[0],LArmRot[0],RArmRot[0],new Vector2(32,39),lShColor,rShColor,fullScale,effects,lShFrame,2);
			if(lShFrame == 0)
			{
				DrawLimbTexture(NPC,sb,texShoulderL_Glow,LArmPos[0],RArmPos[0],LArmRot[0],RArmRot[0],new Vector2(32,38),Color.White*fAlpha,Color.White*fAlpha,fullScale,effects);
			}
			
			//left arm
			DrawLimbTexture(NPC,sb,texArmL,LArmPos[1],RArmPos[1],LArmRot[1],RArmRot[1],new Vector2(30,22),lArmColor,rArmColor,fullScale,effects);
			if(armGlowAlpha > 0f)
			{
				DrawLimbTexture(NPC,sb,texArmL_Glow2,LArmPos[1],RArmPos[1],LArmRot[1],RArmRot[1],new Vector2(30,22),glowColor*armGlowAlpha,glowColor*armGlowAlpha,fullScale,effects);
			}
			DrawLimbTexture(NPC,sb,texArmL_Glow,LArmPos[1],RArmPos[1],LArmRot[1],RArmRot[1],new Vector2(30,22),lClawGlowColor,rClawGlowColor,fullScale,effects);
			
			//left foot
			DrawLimbTexture(NPC,sb,texFootL,LLegPos[2],RLegPos[2],LLegRot[2],RLegRot[2],new Vector2(18,10),lFootColor,rFootColor,fullScale,effects);
			
			//left leg lower
			DrawLimbTexture(NPC,sb,texLegLowerL,LLegPos[1],RLegPos[1],LLegRot[1],RLegRot[1],new Vector2(14,6),lLLegColor,rLLegColor,fullScale,effects);
			
			//left leg
			int lLegFrame = 0;
			if(NPC.spriteDirection == 1)
			{
				if(LLegArmor != null && LLegArmor.active)
				{
					lLegFrame = 0;
				}
				else
				{
					lLegFrame = 1;
				}
			}
			else
			{
				if(RLegArmor != null && RLegArmor.active)
				{
					lLegFrame = 0;
				}
				else
				{
					lLegFrame = 1;
				}
			}
			DrawLimbTexture(NPC,sb,texLegL,LLegPos[0],RLegPos[0],LLegRot[0],RLegRot[0],new Vector2(14,13),lLegColor,rLegColor,fullScale,effects,lLegFrame,2);
			if(lLegFrame == 0)
			{
				DrawLimbTexture(NPC,sb,texLegL_Glow,LLegPos[0],RLegPos[0],LLegRot[0],RLegRot[0],new Vector2(14,12),Color.White*fAlpha,Color.White*fAlpha,fullScale,effects);
			}
			
			//left cannon arm
			DrawLimbTexture(NPC,sb,texCannonArmL,LCannonPos[0],RCannonPos[0],LCannonRot[0],RCannonRot[0],new Vector2(8,32),bodyColor,bodyColor,fullScale,effects);
			
			//left cannon
			DrawLimbTexture(NPC,sb,texCannonL,LCannonPos[1],RCannonPos[1],LCannonRot[1],RCannonRot[1],new Vector2(32,28),lCannonColor,rCannonColor,fullScale,effects);
			
			if(laserAlpha > 0f)
			{
				DrawLimbTexture(NPC,sb,texCannonL_Glow,LCannonPos[1],RCannonPos[1],LCannonRot[1],RCannonRot[1],new Vector2(32,28),glowColor*laserAlpha,glowColor*laserAlpha,fullScale,effects);
				
				Vector2 laserPos = LCannonPos[2] + lLaserAngle.ToRotationVector2()*17f;
				sb.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Pixel").Value, laserPos - Main.screenPosition, new Rectangle?(new Rectangle(0,0,3000,2)),laserColor,lLaserAngle,new Vector2(1f,1f),1f,SpriteEffects.None,0f);
			}
			DrawLimbTexture(NPC,sb,texCannonL_Glow2,LCannonPos[1],RCannonPos[1],LCannonRot[1],RCannonRot[1],new Vector2(32,28),Color.White*fAlpha,Color.White*fAlpha,fullScale,effects);
			
			//body
			DrawLimbTexture(NPC,sb,texBody,BodyPos[0],BodyPos[0],BodyRot,BodyRot,new Vector2(52,132),bodyColor,bodyColor,fullScale,effects);
			
			//head
			DrawLimbTexture(NPC,sb,texHead,HeadPos[0],HeadPos[0],HeadRot[0],HeadRot[0],new Vector2(28,54),headColor,headColor,fullScale,effects);
			
			//jaw
			float jawRot = HeadRot[0] + HeadRot[1];
			DrawLimbTexture(NPC,sb,texJaw,HeadPos[1],HeadPos[1],jawRot,jawRot,new Vector2(4,4),headColor,headColor,fullScale,effects);
			
			//right cannon arm
			DrawLimbTexture(NPC,sb,texCannonArmR,RCannonPos[0],LCannonPos[0],RCannonRot[0],LCannonRot[0],new Vector2(12,32),bodyColor,bodyColor,fullScale,effects);
			
			//right cannon
			DrawLimbTexture(NPC,sb,texCannonR,RCannonPos[1],LCannonPos[1],RCannonRot[1],LCannonRot[1],new Vector2(32,28),rCannonColor,lCannonColor,fullScale,effects);
			
			if(laserAlpha > 0f)
			{
				DrawLimbTexture(NPC,sb,texCannonR_Glow,RCannonPos[1],LCannonPos[1],RCannonRot[1],LCannonRot[1],new Vector2(32,28),glowColor*laserAlpha,glowColor*laserAlpha,fullScale,effects);
				
				Vector2 laserPos = RCannonPos[2] + rLaserAngle.ToRotationVector2()*17f;
				sb.Draw(ModContent.Request<Texture2D>($"{Mod.Name}/Assets/Textures/Pixel").Value, laserPos - Main.screenPosition, new Rectangle?(new Rectangle(0,0,3000,2)),laserColor,rLaserAngle,new Vector2(1f,1f),1f,SpriteEffects.None,0f);
			}
			DrawLimbTexture(NPC,sb,texCannonR_Glow2,RCannonPos[1],LCannonPos[1],RCannonRot[1],LCannonRot[1],new Vector2(32,28),Color.White*fAlpha,Color.White*fAlpha,fullScale,effects);
			
			//right foot
			DrawLimbTexture(NPC,sb,texFootR,RLegPos[2],LLegPos[2],RLegRot[2],LLegRot[2],new Vector2(18,10),rFootColor,lFootColor,fullScale,effects);
			
			//right leg lower
			DrawLimbTexture(NPC,sb,texLegLowerR,RLegPos[1],LLegPos[1],RLegRot[1],LLegRot[1],new Vector2(14,6),rLLegColor,lLLegColor,fullScale,effects);
			
			//right leg
			int rLegFrame = 0;
			if(NPC.spriteDirection == 1)
			{
				if(RLegArmor != null && RLegArmor.active)
				{
					rLegFrame = 0;
				}
				else
				{
					rLegFrame = 1;
				}
			}
			else
			{
				if(LLegArmor != null && LLegArmor.active)
				{
					rLegFrame = 0;
				}
				else
				{
					rLegFrame = 1;
				}
			}
			DrawLimbTexture(NPC,sb,texLegR,RLegPos[0],LLegPos[0],RLegRot[0],LLegRot[0],new Vector2(14,13),rLegColor,lLegColor,fullScale,effects,rLegFrame,2);
			if(rLegFrame == 0)
			{
				DrawLimbTexture(NPC,sb,texLegR_Glow,RLegPos[0],LLegPos[0],RLegRot[0],LLegRot[0],new Vector2(14,12),Color.White*fAlpha,Color.White*fAlpha,fullScale,effects);
			}
			
			//right shoulder
			int rShFrame = 0;
			if(NPC.spriteDirection == 1)
			{
				if(RArmArmor != null && RArmArmor.active)
				{
					rShFrame = 0;
				}
				else
				{
					rShFrame = 1;
				}
			}
			else
			{
				if(LArmArmor != null && LArmArmor.active)
				{
					rShFrame = 0;
				}
				else
				{
					rShFrame = 1;
				}
			}
			DrawLimbTexture(NPC,sb,texShoulderR,RArmPos[0],LArmPos[0],RArmRot[0],LArmRot[0],new Vector2(32,39),rShColor,lShColor,fullScale,effects,rShFrame,2);
			if(rShFrame == 0)
			{
				DrawLimbTexture(NPC,sb,texShoulderR_Glow,RArmPos[0],LArmPos[0],RArmRot[0],LArmRot[0],new Vector2(32,38),Color.White*fAlpha,Color.White*fAlpha,fullScale,effects);
			}
			
			//right arm
			DrawLimbTexture(NPC,sb,texArmR,RArmPos[1],LArmPos[1],RArmRot[1],LArmRot[1],new Vector2(30,22),rArmColor,lArmColor,fullScale,effects);
			if(armGlowAlpha > 0f)
			{
				DrawLimbTexture(NPC,sb,texArmL_Glow2,RArmPos[1],LArmPos[1],RArmRot[1],LArmRot[1],new Vector2(30,22),glowColor*armGlowAlpha,glowColor*armGlowAlpha,fullScale,effects);
			}
			DrawLimbTexture(NPC,sb,texArmR_Glow,RArmPos[1],LArmPos[1],RArmRot[1],LArmRot[1],new Vector2(30,22),rClawGlowColor,lClawGlowColor,fullScale,effects);
			DrawLimbTexture(NPC,sb,texArmR_Glow3,RArmPos[1],LArmPos[1],RArmRot[1],LArmRot[1],new Vector2(30,22),Color.White*fAlpha,Color.White*fAlpha,fullScale,effects);
			
			return false;
		}
		static void DrawLimbTexture(NPC npc, SpriteBatch sb, Texture2D tex, Vector2 Pos1, Vector2 Pos2, float Rot1, float Rot2, Vector2 Origin, Color color1, Color color2, Vector2 scale, SpriteEffects effects, int frame = 0, int frameCount = 1)
		{
			float LimbRot = Rot1;
			Vector2 LimbDrawPos = Pos1;
			Vector2 LimbOrigin = Origin;
			Color LimbColor = color1;
			int LimbFrameHeight = tex.Height / frameCount;
			if(npc.spriteDirection == -1)
			{
				LimbRot = Rot2;
				LimbDrawPos = Pos2;
				LimbOrigin.X = tex.Width - LimbOrigin.X;
				LimbColor = color2;
			}
			sb.Draw(tex, LimbDrawPos - Main.screenPosition, new Rectangle?(new Rectangle(0,frame*LimbFrameHeight,tex.Width,LimbFrameHeight)),LimbColor,LimbRot*npc.spriteDirection,LimbOrigin,scale,effects,0f);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((byte)_body);
			writer.Write((byte)_lArmArmor);
			writer.Write((byte)_rArmArmor);
			writer.Write((byte)_lLegArmor);
			writer.Write((byte)_rLegArmor);
			writer.Write((byte)_rCannon);
			writer.Write((byte)_lCannon);

			for (int i = 0; i < 3; ++i)
			{
				writer.Write((byte)_lArm[i]);
				writer.Write((byte)_rArm[i]);
				writer.Write((byte)_lLeg[i]);
				writer.Write((byte)_rLeg[i]);
			}
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			_body = reader.ReadByte();
			_lArmArmor = reader.ReadByte();
			_rArmArmor = reader.ReadByte();
			_lLegArmor = reader.ReadByte();
			_rLegArmor = reader.ReadByte();
			_rCannon = reader.ReadByte();
			_lCannon = reader.ReadByte();

			for (int i = 0; i < 3; ++i)
			{
				_lArm[i] = reader.ReadByte();
				_rArm[i] = reader.ReadByte();
				_lLeg[i] = reader.ReadByte();
				_rLeg[i] = reader.ReadByte();
			}

			if (!this.initialized)
			{
				this.SetPositions();
				this.initialized = true;
			}
		}
	}
}
