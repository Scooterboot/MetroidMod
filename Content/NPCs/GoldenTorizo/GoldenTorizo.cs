using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MetroidModPorted.Common.Systems;

namespace MetroidModPorted.Content.NPCs.GoldenTorizo
{
	[AutoloadBossHead]
	public class GoldenTorizo : ModNPC
	{
		public override string BossHeadTexture => Mod.Name + "/Content/NPCs/GoldenTorizo/GoldenTorizo_Head_Boss";
		public override string Texture => Mod.Name + "/Content/NPCs/GoldenTorizo/GoldenTorizoBody";
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Torizo");
			Main.npcFrameCount[Type] = 2;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					31,
					ModContent.BuffType<Buffs.IceFreeze>(),
					ModContent.BuffType<Buffs.InstantFreeze>()
				}
			};
			NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				CustomTexturePath = $"{nameof(MetroidModPorted)}/Content/NPCs/GoldenTorizo/GoldenTorizo_BossLog",
				PortraitScale = 0.6f, // Portrait refers to the full picture when clicking on the icon in the bestiary
				PortraitPositionYOverride = 0f,
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}
		public override void SetDefaults()
		{
			NPC.width = 0;//56;
			NPC.height = 0;//94;
			NPC.scale = 1f;
			NPC.damage = 50;
			NPC.defense = 50;
			NPC.lifeMax = 35000;
			//NPC.dontTakeDamage = true;
			NPC.boss = true;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.value = Item.buyPrice(0, 0, 7, 0);
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.behindTiles = true;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
			if (!Main.dedServ) { Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Torizo"); }
			//BossBag = ModContent.ItemType<Items.Boss.GoldenTorizoBag>();
			NPC.chaseable = false;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("An enhanced version of the Torizo Statue but with golden armor plating. While this one lacks the spiritual possession, it is far more dangerous than the lumbering machines the Gizzard tribe possess. Its energy waves are much faster and follow any hostile target. The Golden armor plating gives it extraordinary defenses and an energy shield while jumping. Be careful to avoid it if you don't want a nasty end!")
			});
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.7f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.7f);
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Boss.GoldenTorizoBag>()));

			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(SuitAddonLoader.GetAddon<SuitAddons.ScrewAttack>().ItemType));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.TorizoMusicBox>(), 6));
			npcLoot.Add(notExpertRule);
		}
		public override void OnKill()
		{
			MSystem.bossesDown |= MetroidBossDown.downedGoldenTorizo;
		}
		public override bool? CanBeHitByItem(Player player, Item item) => false;
		public override bool? CanBeHitByProjectile(Projectile projectile) => false;
		
		ReLogic.Utilities.SlotId soundInstance;
		public override void HitEffect(int hitDirection, double damage)
		{
			if(Head != null && Head.active && (!SoundEngine.TryGetActiveSound(soundInstance, out ActiveSound result) || !result.IsPlaying))
			{
				soundInstance = SoundEngine.PlaySound(Sounds.NPCs.TorizoHit, Head.Center);
			}
		}
		
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.5f;
			position = new Vector2(NPC.Center.X,NPC.position.Y+168);
			if(NPC.life < NPC.lifeMax)
			{
				return true;
			}
			return null;
		}
		
		/*public override void BossHeadSlot(ref int index)
		{
			index = NPCHeadLoader.GetBossHeadSlot(MetroidMod.GoldenTorizoHead);
		}*/
		
		int _body, _head, _rHand, _lHand;
		NPC Body => Main.npc[_body];
		NPC RHand => Main.npc[_rHand];
		NPC LHand => Main.npc[_lHand];
		
		NPC Head
		{
			get
			{
				if(Main.npc[_head].type == ModContent.NPCType<GoldenTorizo_HitBox>() && Main.npc[_head].ai[1] == 0f)
				{
					return Main.npc[_head];
				}
				return null;
			}
		}
		
		int[] _rArm = new int[2],
			_lArm = new int[2],
			_rLeg = new int[2],
			_lLeg = new int[2];

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
		
		Vector2 BodyOffset;
		Vector2[] BodyPos = new Vector2[2],
		HeadPos = new Vector2[2],
		RArmPos = new Vector2[5],
		LArmPos = new Vector2[5],
		RHandPos = new Vector2[3],
		LHandPos = new Vector2[3],
		RLegPos = new Vector2[5],
		LLegPos = new Vector2[5];
		
		float BodyRot, HeadRot;
		float[] RArmRot = new float[3],
		LArmRot = new float[3],
		RLegRot = new float[3],
		LLegRot = new float[3];
		
		float BodyOffsetRot;
		float[] HeadOffsetRot = new float[2],
		RArmOffsetRot = new float[5],
		LArmOffsetRot = new float[5],
		RHandOffsetRot = new float[3],
		LHandOffsetRot = new float[3],
		RLegOffsetRot = new float[5],
		LLegOffsetRot = new float[5];
		
		float BodyDist;
		float[] HeadDist = new float[2],
		RArmDist = new float[5],
		LArmDist = new float[5],
		RHandDist = new float[3],
		LHandDist = new float[3],
		RLegDist = new float[5],
		LLegDist = new float[5];
		
		static Vector2[] DefaultBodyPos = { new Vector2(0,0), new Vector2(0,0) },
		DefaultHeadPos = { new Vector2(10,-19), new Vector2(2,-5) },
		DefaultRightArmPos = { new Vector2(1,-20), new Vector2(0,38), new Vector2(0,32), new Vector2(0,28),new Vector2(0,16) },
		DefaultLeftArmPos = { new Vector2(1,-20), new Vector2(0,38), new Vector2(0,32), new Vector2(0,28),new Vector2(0,16) },
		DefaultRightHandPos = { new Vector2(9,-7), new Vector2(15,3), new Vector2(3,13) },
		DefaultLeftHandPos = { new Vector2(9,-7), new Vector2(15,3), new Vector2(3,13) },
		DefaultRightLegPos = { new Vector2(-11,38), new Vector2(0,32), new Vector2(0,34), new Vector2(0,10),new Vector2(-2,8) },
		DefaultLeftLegPos = { new Vector2(-11,38), new Vector2(0,32), new Vector2(0,34), new Vector2(0,10),new Vector2(-2,8) };
		
		Vector2[] CurrentBodyPos = new Vector2[2],
		CurrentHeadPos = new Vector2[2],
		CurrentRightArmPos = new Vector2[5],
		CurrentLeftArmPos = new Vector2[5],
		CurrentRightHandPos = new Vector2[3],
		CurrentLeftHandPos = new Vector2[3],
		CurrentRightLegPos = new Vector2[5],
		CurrentLeftLegPos = new Vector2[5];
		
		Vector2 fullScale = new Vector2(1f,1f);
		
		void SetPositions()
		{
			for(int i = 0; i < 5; i++)
			{
				CurrentRightArmPos[i] = DefaultRightArmPos[i];
				CurrentLeftArmPos[i] = DefaultLeftArmPos[i];
				CurrentRightLegPos[i] = DefaultRightLegPos[i];
				CurrentLeftLegPos[i] = DefaultLeftLegPos[i];
				if(i < 3)
				{
					CurrentRightHandPos[i] = DefaultRightHandPos[i];
					CurrentLeftHandPos[i] = DefaultLeftHandPos[i];
				}
				if(i < 2)
				{
					CurrentBodyPos[i] = DefaultBodyPos[i];
					CurrentHeadPos[i] = DefaultHeadPos[i];
				}
			}
			
			BodyOffsetRot = (float)Math.Atan2(CurrentBodyPos[1].Y,CurrentBodyPos[1].X);
			BodyDist = CurrentBodyPos[1].Length();
			
			for(int i = 0; i < 5; i++)
			{
				RArmOffsetRot[i] = (float)Math.Atan2(CurrentRightArmPos[i].Y,CurrentRightArmPos[i].X);
				RArmDist[i] = CurrentRightArmPos[i].Length();
				LArmOffsetRot[i] = (float)Math.Atan2(CurrentLeftArmPos[i].Y,CurrentLeftArmPos[i].X);
				LArmDist[i] = CurrentLeftArmPos[i].Length();
				
				RLegOffsetRot[i] = (float)Math.Atan2(CurrentRightLegPos[i].Y,CurrentRightLegPos[i].X);
				RLegDist[i] = CurrentRightLegPos[i].Length();
				LLegOffsetRot[i] = (float)Math.Atan2(CurrentLeftLegPos[i].Y,CurrentLeftLegPos[i].X);
				LLegDist[i] = CurrentLeftLegPos[i].Length();
				
				if(i < 3)
				{
					RHandOffsetRot[i] = (float)Math.Atan2(CurrentRightHandPos[i].Y,CurrentRightHandPos[i].X);
					RHandDist[i] = CurrentRightHandPos[i].Length();
					LHandOffsetRot[i] = (float)Math.Atan2(CurrentLeftHandPos[i].Y,CurrentLeftHandPos[i].X);
					LHandDist[i] = CurrentLeftHandPos[i].Length();
				}
				
				if(i < 2)
				{
					HeadOffsetRot[i] = (float)Math.Atan2(CurrentHeadPos[i].Y,CurrentHeadPos[i].X);
					HeadDist[i] = CurrentHeadPos[i].Length();
				}
			}
			
			BodyPos[0] = NPC.Center + (CurrentBodyPos[0] + BodyOffset);
			BodyPos[1] = BodyPos[0] + Angle.AngleFlip(BodyOffsetRot + BodyRot, NPC.direction).ToRotationVector2()*BodyDist * fullScale;
			
			RArmPos[0] = BodyPos[0] + Angle.AngleFlip(RArmOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2()*RArmDist[0] * fullScale;
			RArmPos[1] = RArmPos[0] + Angle.AngleFlip(RArmOffsetRot[1] + RArmRot[0], NPC.direction).ToRotationVector2()*RArmDist[1] * fullScale;
			RArmPos[2] = RArmPos[1] + Angle.AngleFlip(RArmOffsetRot[2] + RArmRot[1], NPC.direction).ToRotationVector2()*RArmDist[2] * fullScale;
			RArmPos[3] = RArmPos[0] + Angle.AngleFlip(RArmOffsetRot[3] + RArmRot[0], NPC.direction).ToRotationVector2()*RArmDist[3] * fullScale;
			RArmPos[4] = RArmPos[1] + Angle.AngleFlip(RArmOffsetRot[4] + RArmRot[1], NPC.direction).ToRotationVector2()*RArmDist[4] * fullScale;
			
			RHandPos[0] = RArmPos[2] + Angle.AngleFlip(RHandOffsetRot[0] + RArmRot[2]+(float)Angle.ConvertToRadians(60), NPC.direction).ToRotationVector2()*RHandDist[0] * fullScale;
			RHandPos[1] = RArmPos[2] + Angle.AngleFlip(RHandOffsetRot[1] + RArmRot[2]+(float)Angle.ConvertToRadians(30), NPC.direction).ToRotationVector2()*RHandDist[1] * fullScale;
			RHandPos[2] = RArmPos[2] + Angle.AngleFlip(RHandOffsetRot[2] + RArmRot[2], NPC.direction).ToRotationVector2()*RHandDist[2] * fullScale;
			
			LArmPos[0] = BodyPos[0] + Angle.AngleFlip(LArmOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2()*LArmDist[0] * fullScale;
			LArmPos[1] = LArmPos[0] + Angle.AngleFlip(LArmOffsetRot[1] + LArmRot[0], NPC.direction).ToRotationVector2()*LArmDist[1] * fullScale;
			LArmPos[2] = LArmPos[1] + Angle.AngleFlip(LArmOffsetRot[2] + LArmRot[1], NPC.direction).ToRotationVector2()*LArmDist[2] * fullScale;
			LArmPos[3] = LArmPos[0] + Angle.AngleFlip(LArmOffsetRot[3] + LArmRot[0], NPC.direction).ToRotationVector2()*LArmDist[3] * fullScale;
			LArmPos[4] = LArmPos[1] + Angle.AngleFlip(LArmOffsetRot[4] + LArmRot[1], NPC.direction).ToRotationVector2()*LArmDist[4] * fullScale;
			
			LHandPos[0] = LArmPos[2] + Angle.AngleFlip(LHandOffsetRot[0] + LArmRot[2]+(float)Angle.ConvertToRadians(60), NPC.direction).ToRotationVector2()*LHandDist[0] * fullScale;
			LHandPos[1] = LArmPos[2] + Angle.AngleFlip(LHandOffsetRot[1] + LArmRot[2]+(float)Angle.ConvertToRadians(30), NPC.direction).ToRotationVector2()*LHandDist[1] * fullScale;
			LHandPos[2] = LArmPos[2] + Angle.AngleFlip(LHandOffsetRot[2] + LArmRot[2], NPC.direction).ToRotationVector2()*LHandDist[2] * fullScale;
			
			RLegPos[0] = BodyPos[0] + Angle.AngleFlip(RLegOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2()*RLegDist[0] * fullScale;
			RLegPos[1] = RLegPos[0] + Angle.AngleFlip(RLegOffsetRot[1] + RLegRot[0], NPC.direction).ToRotationVector2()*RLegDist[1] * fullScale;
			RLegPos[2] = RLegPos[1] + Angle.AngleFlip(RLegOffsetRot[2] + RLegRot[1], NPC.direction).ToRotationVector2()*RLegDist[2] * fullScale;
			RLegPos[3] = RLegPos[0] + Angle.AngleFlip(RLegOffsetRot[3] + RLegRot[0], NPC.direction).ToRotationVector2()*RLegDist[3] * fullScale;
			RLegPos[4] = RLegPos[1] + Angle.AngleFlip(RLegOffsetRot[4] + RLegRot[1], NPC.direction).ToRotationVector2()*RLegDist[4] * fullScale;
			
			LLegPos[0] = BodyPos[0] + Angle.AngleFlip(LLegOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2()*LLegDist[0] * fullScale;
			LLegPos[1] = LLegPos[0] + Angle.AngleFlip(LLegOffsetRot[1] + LLegRot[0], NPC.direction).ToRotationVector2()*LLegDist[1] * fullScale;
			LLegPos[2] = LLegPos[1] + Angle.AngleFlip(LLegOffsetRot[2] + LLegRot[1], NPC.direction).ToRotationVector2()*LLegDist[2] * fullScale;
			LLegPos[3] = LLegPos[0] + Angle.AngleFlip(LLegOffsetRot[3] + LLegRot[0], NPC.direction).ToRotationVector2()*LLegDist[3] * fullScale;
			LLegPos[4] = LLegPos[1] + Angle.AngleFlip(LLegOffsetRot[4] + LLegRot[1], NPC.direction).ToRotationVector2()*LLegDist[4] * fullScale;
			
			HeadPos[0] = BodyPos[0] + Angle.AngleFlip(HeadOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2()*HeadDist[0] * fullScale;
			HeadPos[1] = HeadPos[0] + Angle.AngleFlip(HeadOffsetRot[1] + HeadRot, NPC.direction).ToRotationVector2()*HeadDist[1] * fullScale;
		}
		
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
				
				if(NPC.ai[1] == 0)
				{
					NPC.direction = 1;
					if (Main.rand.Next(2) == 0)
						NPC.direction = -1;

					NPC.velocity.X = 0f;
					NPC.velocity.Y = 0.1f;
					NPC.Center = new Vector2(player.Center.X - 150 * NPC.direction, player.Center.Y - 1500);
				}
				else
				{
					SetAnimation("spawn", anim_Spawn);
					SetBodyOffset();
				}
				
				SetPositions();

				var entitySource = NPC.GetSource_FromAI();
				_body = NPC.NewNPC(entitySource, (int)BodyPos[1].X,(int)BodyPos[1].Y,ModContent.NPCType<GoldenTorizo_HitBox>(),NPC.whoAmI, NPC.whoAmI,1f);
				
				for(int i = 0; i < 2; i++)
				{
					_rArm[i] = NPC.NewNPC(entitySource, (int)RArmPos[i+3].X,(int)RArmPos[i+3].Y,ModContent.NPCType<GoldenTorizo_HitBox>(),NPC.whoAmI, NPC.whoAmI,2+i);
					_lArm[i] = NPC.NewNPC(entitySource, (int)LArmPos[i+3].X,(int)LArmPos[i+3].Y,ModContent.NPCType<GoldenTorizo_HitBox>(),NPC.whoAmI, NPC.whoAmI,2+i);
					_rLeg[i] = NPC.NewNPC(entitySource, (int)RLegPos[i+3].X,(int)RLegPos[i+3].Y,ModContent.NPCType<GoldenTorizo_HitBox>(),NPC.whoAmI, NPC.whoAmI,5+i);
					_lLeg[i] = NPC.NewNPC(entitySource, (int)LLegPos[i+3].X,(int)LLegPos[i+3].Y,ModContent.NPCType<GoldenTorizo_HitBox>(),NPC.whoAmI, NPC.whoAmI,5+i);
				}
				
				_rHand = NPC.NewNPC(entitySource, (int)RHandPos[0].X,(int)RHandPos[0].Y,ModContent.NPCType<GoldenTorizo_HitBox>(),NPC.whoAmI, NPC.whoAmI,4f);
				_lHand = NPC.NewNPC(entitySource, (int)LHandPos[0].X,(int)LHandPos[0].Y,ModContent.NPCType<GoldenTorizo_HitBox>(),NPC.whoAmI, NPC.whoAmI,4f);
				
				_head = NPC.NewNPC(entitySource, (int)HeadPos[1].X,(int)HeadPos[1].Y,ModContent.NPCType<GoldenTorizo_HitBox>(),NPC.whoAmI, NPC.whoAmI);
				
				initialized = true;
			}
			
			//fullScale = new Vector2(0.9f,0.9f);
			
			return true;
		}
		
		// Spawn Animation
		float[][] RArmAnim_Spawn = new float[][]{
		new float[] { 33f, 0f, 0f, 0f, 0f,10f,22.5f},
		new float[] {114f,90f,45f,45f,45f,55f,67.5f},
		new float[] { 60f,30f, 0f, 0f, 0f,10f,22.5f}};
		
		float[][] LArmAnim_Spawn = new float[][]{
		new float[] { 33f, 0f, 0f, 0f,-22f,-45f,-67.5f},
		new float[] {114f,90f,45f,45f, 23f, 12f,    0f},
		new float[] { 60f,30f, 0f, 0f,  0f,-15f,  -45f}};
		
		float[][] RLegAnim_Spawn = new float[][]{
		new float[] {135f,125f, 90f, 45f,0f,-20f,-20f},
		new float[] { 60f,  0f,-22f,-22f,0f,-20f,-41f},
		new float[] {  0f,  0f,  0f,  0f,0f,  0f,  0f}};
		
		float[][] LLegAnim_Spawn = new float[][]{
		new float[] {135f,125f, 90f, 45f,22.5f, 70f,43f},
		new float[] { 60f,  0f,-22f,-22f, -45f,-30f,19f},
		new float[] {  0f,  0f,  0f,  0f, -45f,-20f, 0f}};
		
		int[] HandFrame_Spawn = {0,1,2,2,2,2,2};
		int[] LFootFrame_Spawn = {0,0,0,0,1,1,0};
		
		float[] speed_Spawn = {25f,8f,9f,10f,23f,10f,0f};
		
		float anim_Spawn = 1f;
		
		// Walk Animation
		float[][] RArmAnim_Walk = new float[][]{
		new float[] {22.5f, 0f,-22.5f,-45f,-56.75f, -67.5f,-45f,-22.5f, 0f,11.75f, 22.5f},
		new float[] {67.5f,45f, 22.5f, 20f,    10f,     0f, 10f, 22.5f,45f,56.75f, 67.5f},
		new float[] {22.5f, 0f,-22.5f,-25f,   -35f,   -45f,-35f,-22.5f, 0f,11.75f, 22.5f}};
		
		float[][] LArmAnim_Walk = new float[][]{
		new float[] {-67.5f,-45f,-22.5f, 0f,11.75f, 22.5f, 0f,-22.5f,-45f,-56.75f, -67.5f},
		new float[] {    0f, 10f, 22.5f,45f,56.75f, 67.5f,45f, 22.5f, 20f,    10f,     0f},
		new float[] {  -45f,-35f,-22.5f, 0f,11.75f, 22.5f, 0f,-22.5f,-25f,   -35f,   -45f}};
	
		float[][] RLegAnim_Walk = new float[][]{
		new float[] {-20f,-25f,  0f, 35f, 70f, 43f,23f,11f,0f,-15f, -20f},
		new float[] {-41f,-45f,-60f,-50f,-20f, 19f,22f,11f,0f,-15f, -41f},
		new float[] {  0f,-60f,-65f,-30f,-20f,  0f, 0f, 0f,0f,  0f,   0f}};
		
		float[][] LLegAnim_Walk = new float[][]{
		new float[] {43f,23f,11f,0f,-15f, -20f,-25f,  0f, 35f, 70f, 45f},
		new float[] {19f,22f,11f,0f,-15f, -41f,-45f,-60f,-50f,-20f, 22f},
		new float[] { 0f, 0f, 0f,0f,  0f,   0f,-60f,-65f,-30f,-20f,  0f}};
		
		int[] RFootFrame_Walk = {0,1,1,1,1, 0,0,0,0,0};
		int[] LFootFrame_Walk = {0,0,0,0,0, 0,1,1,1,1};
		
		float[] speed_Walk = {8f,12f,13f,17f,16f, 8f,12f,13f,17f,16f};
		
		float anim_Walk = 1f;
		
		// Jump Animation
		float[][] RArmAnim_Jump = new float[][]{
		new float[] {-22.5f,-45f,-67.5f},
		new float[] { 22.5f, 20f,    0f},
		new float[] {    0f,  0f,  -45f}};
		
		float[][] LArmAnim_Jump = new float[][]{
		new float[] {-22.5f,-45f,-67.5f},
		new float[] { 22.5f, 20f,    0f},
		new float[] {    0f,  0f,  -45f}};
		
		float[][] RLegAnim_Jump = new float[][]{
		new float[] { 35f, 22f,    0f},
		new float[] {-53f,-22f,  -20f},
		new float[] {  0f,-45f,-22.5f}};
		
		float[][] LLegAnim_Jump = new float[][]{
		new float[] { 45f, 22f,    0f},
		new float[] {-45f,-22f,  -20f},
		new float[] {  0f,-45f,-22.5f}};
		
		int[] FootFrame_Jump = {0,1,1};
		
		float anim_Jump = 1f;
		float anim_JumpTransition = 0f;
		
		// Somersault Animation
		float[][] RArmAnim_SpinJump = new float[][]{
		new float[] {-22.5f,-45f,  0f},
		new float[] { 22.5f, 20f,110f},
	//	new float[] {    0f,  0f, 20f}};
		new float[] {    0f, 30f, 80f}};
		
		float[][] LArmAnim_SpinJump = new float[][]{
		new float[] {-22.5f,-45f,  0f},
		new float[] { 22.5f, 20f,110f},
	//	new float[] {    0f,  0f, 20f}};
		new float[] {    0f, 30f, 80f}};
		
		float[][] RLegAnim_SpinJump = new float[][]{
		new float[] { 35f, 22f, 135f},
		new float[] {-53f,-22f,   0f},
		new float[] {  0f,-45f,-110f}};
		
		float[][] LLegAnim_SpinJump = new float[][]{
		new float[] { 45f, 22f, 135f},
		new float[] {-45f,-22f,   0f},
		new float[] {  0f,-45f,-110f}};
		
		int[] HandFrame_SpinJump = {2,1,0};
		int[] FootFrame_SpinJump = {0,1,1};
		
		float anim_SpinJump = 1f;
		float anim_SpinJumpTransition = 0f;
		float anim_SpinJump_Spin = 0f;
		
		float[] BodyOffset_SpinJump = {19,0,0};
		
		// Mouth Bombs Animation
		float[][] RArmAnim_Bomb = new float[][]{
		new float[] { 0f,22.5f,32.5f, 45f},
		new float[] {45f,  70f,  90f,135f},
		new float[] { 0f,  30f,  45f, 90f}};
		
		float[][] LArmAnim_Bomb = new float[][]{
		new float[] { 0f,  20f,32.5f, 45f},
		new float[] {24f,67.5f,  90f,135f},
		new float[] { 0f,27.5f,  45f, 90f}};
		
		float anim_Bomb = 1f;
		float anim_BombTransition = 0f;
		
		// Claw Attack Animation
		
		float[][] RArmAnim_Claw = new float[][]{
		new float[] {-22.5f,45f, 90f,-45f,-22.5f,-22.5f,-22.5f,-22.5f, -22.5f},
		new float[] { 22.5f,90f,180f,  0f, 22.5f, 22.5f, 22.5f, 22.5f,  22.5f},
		new float[] {    0f,45f,135f,-45f,    0f,    0f,    0f,    0f,     0f}};
		
		float[][] LArmAnim_Claw = new float[][]{
		new float[] {-22.5f,-22.5f,-22.5f,-22.5f,-22.5f,45f, 90f,-45f, -22.5f},
		new float[] { 22.5f, 22.5f, 22.5f, 22.5f, 22.5f,90f,180f,  0f,  22.5f},
		new float[] {    0f,    0f,    0f,    0f,    0f,45f,135f,-45f,     0f}};
		
		float anim_Claw = 1f;
		float anim_ClawTransition = 0f;
		
		// Ranged Claw Animation
		
		
		void SetAnimation(string type, float anim, float transition = 1f)
		{
			if(type == "spawn")
			{
				BodyRot = MathHelper.Lerp(BodyRot,0f,transition);
				HeadRot = MathHelper.Lerp(HeadRot,0f,transition);
				for(int i = 0; i < 3; i++)
				{
					RLegRot[i] = MathHelper.Lerp(RLegRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_Spawn[i],anim)),transition);
					LLegRot[i] = MathHelper.Lerp(LLegRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_Spawn[i],anim)),transition);

					RArmRot[i] = MathHelper.Lerp(RArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_Spawn[i],anim)),transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Spawn[i],anim)),transition);
				}
				if(transition > 0.5f)
				{
					LFootFrame = LFootFrame_Spawn[(int)Math.Min(anim,LFootFrame_Spawn.Length)-1];
					RHandFrame = HandFrame_Spawn[(int)Math.Min(anim,HandFrame_Spawn.Length)-1];
					LHandFrame = HandFrame_Spawn[(int)Math.Min(anim,HandFrame_Spawn.Length)-1];
				}
			}
			if(type == "walk")
			{
				BodyRot = MathHelper.Lerp(BodyRot,0f,transition);
				HeadRot = MathHelper.Lerp(HeadRot,0f,transition);
				for(int i = 0; i < 3; i++)
				{
					RLegRot[i] = MathHelper.Lerp(RLegRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_Walk[i],anim)),transition);
					LLegRot[i] = MathHelper.Lerp(LLegRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_Walk[i],anim)),transition);

					RArmRot[i] = MathHelper.Lerp(RArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_Walk[i],anim)),transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Walk[i],anim)),transition);
				}
				if(transition > 0.5f)
				{
					RFootFrame = RFootFrame_Walk[(int)Math.Min(anim,RFootFrame_Walk.Length)-1];
					LFootFrame = LFootFrame_Walk[(int)Math.Min(anim,LFootFrame_Walk.Length)-1];
					RHandFrame = 2;
					LHandFrame = 2;
				}
			}
			if(type == "jump")
			{
				BodyRot = MathHelper.Lerp(BodyRot,0f,transition);
				HeadRot = MathHelper.Lerp(HeadRot,0f,transition);
				for(int i = 0; i < 3; i++)
				{
					RLegRot[i] = MathHelper.Lerp(RLegRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_Jump[i],anim)),transition);
					LLegRot[i] = MathHelper.Lerp(LLegRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_Jump[i],anim)),transition);

					RArmRot[i] = MathHelper.Lerp(RArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_Jump[i],anim)),transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Jump[i],anim)),transition);
				}
				if(transition >= 0.5f)
				{
					RFootFrame = FootFrame_Jump[(int)Math.Min(anim,FootFrame_Jump.Length)-1];
					LFootFrame = FootFrame_Jump[(int)Math.Min(anim,FootFrame_Jump.Length)-1];
					RHandFrame = 2;
					LHandFrame = 2;
				}
			}
			if(type == "spin jump")
			{
				BodyRot = MathHelper.Lerp(BodyRot,0f,transition);
				HeadRot = MathHelper.Lerp(HeadRot,0f,transition);
				for(int i = 0; i < 3; i++)
				{
					RLegRot[i] = MathHelper.Lerp(RLegRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RLegAnim_SpinJump[i],anim)),transition);
					LLegRot[i] = MathHelper.Lerp(LLegRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LLegAnim_SpinJump[i],anim)),transition);

					RArmRot[i] = MathHelper.Lerp(RArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_SpinJump[i],anim)),transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_SpinJump[i],anim)),transition);
				}
				if(transition >= 0.5f)
				{
					RFootFrame = FootFrame_SpinJump[(int)Math.Min(anim,FootFrame_Jump.Length)-1];
					LFootFrame = FootFrame_SpinJump[(int)Math.Min(anim,FootFrame_Jump.Length)-1];
					RHandFrame = HandFrame_SpinJump[(int)Math.Min(anim,FootFrame_Jump.Length)-1];
					LHandFrame = HandFrame_SpinJump[(int)Math.Min(anim,FootFrame_Jump.Length)-1];
				}
				
				BodyRot += (float)Angle.ConvertToRadians((double)anim_SpinJump_Spin)*transition;
				HeadRot += (float)Angle.ConvertToRadians((double)anim_SpinJump_Spin)*transition;
				for(int i = 0; i < 3; i++)
				{
					RArmRot[i] += (float)Angle.ConvertToRadians((double)anim_SpinJump_Spin)*transition;
					LArmRot[i] += (float)Angle.ConvertToRadians((double)anim_SpinJump_Spin)*transition;
					RLegRot[i] += (float)Angle.ConvertToRadians((double)anim_SpinJump_Spin)*transition;
					LLegRot[i] += (float)Angle.ConvertToRadians((double)anim_SpinJump_Spin)*transition;
				}
			}
			if(type == "bomb")
			{
				BodyRot = MathHelper.Lerp(BodyRot,0f,transition);
				HeadRot = MathHelper.Lerp(HeadRot,-(float)Angle.ConvertToRadians(45),transition);
				for(int i = 0; i < 3; i++)
				{
					RArmRot[i] = MathHelper.Lerp(RArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_Bomb[i],anim)),transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Bomb[i],anim)),transition);
				}
			}
			if(type == "claw")
			{
				BodyRot = MathHelper.Lerp(BodyRot,0f,transition);
				HeadRot = MathHelper.Lerp(HeadRot,0f,transition);
				for(int i = 0; i < 3; i++)
				{
					RArmRot[i] = MathHelper.Lerp(RArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,RArmAnim_Claw[i],anim)),transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i],-(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f,LArmAnim_Claw[i],anim)),transition);
				}
			}
		}
		void SetBodyOffset(float hOffset = 0f)
		{
			BodyPos[0] = NPC.Center + (CurrentBodyPos[0] + BodyOffset);
			RLegPos[2] = RLegPos[1] + Angle.AngleFlip(RLegOffsetRot[2] + RLegRot[1], NPC.direction).ToRotationVector2()*RLegDist[2] * fullScale;
			LLegPos[2] = LLegPos[1] + Angle.AngleFlip(LLegOffsetRot[2] + LLegRot[1], NPC.direction).ToRotationVector2()*LLegDist[2] * fullScale;
			if(RLegPos[2].Y >= LLegPos[2].Y)
			{
				BodyOffset.Y = 117f - ((RLegPos[2].Y+(13f*fullScale.Y)) - BodyPos[0].Y) + hOffset;
			}
			else
			{
				BodyOffset.Y = 117f - ((LLegPos[2].Y+(13f*fullScale.Y)) - BodyPos[0].Y) + hOffset;
			}
		}
		
		int clawDamage = 70;//30;
		public bool screwAttack = false;
		
		int screwFrame = 0;
		int screwFrameCounter = 0;
		int screwSoundCounter = 0;
		bool screwBoosted = false;
		float screwBoostAlpha = 0f;
		
		bool stepSoundPlayed = false;
		int soundCounter = 0;
		
		bool chestExplosion = false;
		int dustCounter = 0;
		
		int RHandFrame = 0;
		int LHandFrame = 0;
		int RFootFrame = 0;
		int LFootFrame = 0;
		int BodyFrame = 0;
		int HeadFrame = 0;
		int HeadFrameCounter = 0;
		
		float glowAlpha = 1f;
		int glowNum = 1;
		
		float spawnAlpha = 1f;
		int state = 0;
		float stateAlpha = 0f;
		
		public override void AI()
		{
			int numH = 117;//164;
			
			if (Main.player[NPC.target].dead || Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) > 2500f || Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2500f)
			{
				NPC.TargetClosest(true);
				if (Main.player[NPC.target].dead || Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) > 2500f || Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2500f)
				{
					// despawn
					
					NPC.alpha = Math.Min(NPC.alpha + 10, 255);
					if(NPC.alpha >= 255)
					{
						NPC.active = false;
					}
				}
			}
			else
			{
				NPC.alpha = Math.Max(NPC.alpha - 10, 0);
				
				// spawn
				if(NPC.ai[0] == 0)
				{
					NPC.dontTakeDamage = true;
					
					float speed = 0f;
					
					// drop from sky
					if(NPC.ai[1] == 0)
					{
						//if(NPC.velocity.Y != 0)
						if(NPC.velocity.Y > 0 && NPC.ai[2] == 0)
						{
							anim_Spawn = 4f;
						}
						else
						{
							NPC.ai[2] = 1;
							if(anim_Spawn > 1f)
							{
								speed = -0.2f;
								anim_Spawn = Math.Max(anim_Spawn+speed,1f);
							}
							else
							{
								anim_Spawn = 1f;
								NPC.ai[1] = 1;
								NPC.ai[2] = 0;
							}
						}
					}
					// stand up
					if(NPC.ai[1] == 1)
					{
						if(anim_Spawn < 7f)
						{
							NPC.ai[2]++;
							if(NPC.ai[2] > 90)
							{
								if(NPC.ai[2] % 3 == 0)
								{
									if(HeadFrame < 3)
									{
										HeadFrame++;
									}
								}
								if(NPC.ai[2] == 120 || NPC.ai[2] == 134)
								{
									HeadFrame = 0;
								}
							}
							if(NPC.ai[2] > 180)
							{
								speed = 0.1f;
								anim_Spawn = Math.Min(anim_Spawn+speed,7f);
							}
						}
						else
						{
							anim_Spawn = 7f;
							spawnAlpha -= 0.015f;
							if(spawnAlpha <= 0f)
							{
								NPC.TargetClosest(true);
								NPC.netUpdate = true;
								NPC.ai[0] = 1;
								NPC.ai[1] = 0;
								NPC.ai[2] = 0;
							}
						}
					}
					
					float moveSpeed = speed_Spawn[(int)Math.Min(anim_Spawn,speed_Spawn.Length)-1] * Math.Abs(speed) * fullScale.X;
					
					if(NPC.direction == Math.Sign(speed))
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X *= 0.98f;
						}
						NPC.velocity.X += 0.5f;
					}
					else if(NPC.direction == -Math.Sign(speed))
					{
						if (NPC.velocity.X > 0f)
						{
							NPC.velocity.X *= 0.98f;
						}
						NPC.velocity.X -= 0.5f;
					}
					if (NPC.velocity.X > moveSpeed)
					{
						NPC.velocity.X = moveSpeed;
					}
					if (NPC.velocity.X < -moveSpeed)
					{
						NPC.velocity.X = -moveSpeed;
					}
					
					SetAnimation("spawn", anim_Spawn);
					SetBodyOffset();
				}
				else
				{
					NPC.dontTakeDamage = false;
				}
				// walk
				if(NPC.ai[0] == 1)
				{
					Player player = Main.player[NPC.target];
					
					float speed = 0.2f;//0.15f;
					/*if(Head == null || !Head.active)
					{
						speed *= 1.3f;
					}*/
					
					bool walkFlagR = (anim_Walk > 6f-speed && anim_Walk <= 6f);
					bool walkFlagL = (anim_Walk > 11f-speed && (anim_Walk <= 11f || anim_Walk <= 1f));
					bool walkFlag = (walkFlagR || walkFlagL);
					if(NPC.ai[1] == 0 || NPC.ai[1] > 15 || !walkFlag)
					{
						anim_Walk += speed;
						if(anim_Walk >= 11)
						{
							anim_Walk = 1f;
						}
					}
					else
					{
						speed = 0;
						if(walkFlagR)
						{
							anim_Walk = 6f;
						}
						else if(walkFlagL)
						{
							anim_Walk = 1f;
						}
					}
					
					if(walkFlagR)
					{
						if(!stepSoundPlayed)
						{
							SoundEngine.PlaySound(Sounds.NPCs.TorizoStep, RLegPos[2]);
							stepSoundPlayed = true;
						}
					}
					else if(walkFlagL)
					{
						if(!stepSoundPlayed)
						{
							SoundEngine.PlaySound(Sounds.NPCs.TorizoStep, LLegPos[2]);
							stepSoundPlayed = true;
						}
					}
					else
					{
						stepSoundPlayed = false;
					}
					
					// Jump
					if(Math.Abs(player.Center.X-NPC.Center.X) < 500 && player.Center.Y < NPC.Center.Y && (player.velocity.Y == 0f || NPC.ai[2] >= 1) && NPC.ai[1] == 0f)
					{
						NPC.ai[2]++;
					}
					else
					{
						if(NPC.ai[2] > 0 && NPC.ai[1] == 0f)
						{
							NPC.ai[2]--;
						}
					}
					
					if(NPC.ai[2] > 60)
					{
						NPC.TargetClosest(true);
						NPC.netUpdate = true;
						NPC.ai[0] = 2;
						NPC.ai[1] = 0;
						NPC.ai[2] = 0;
						NPC.ai[3] = 0;
					}
					
					if(player.position.Y+player.height > NPC.position.Y-50)
					{
						if((NPC.direction == 1 && player.Center.X < NPC.position.X+48) || (NPC.direction == -1 && player.Center.X > NPC.position.X-48))
						{
							if(NPC.ai[1] == 0)
							{
								if(Main.rand.Next(2) == 0)
								{
									NPC.ai[1] = -1;
								}
								else
								{
									NPC.ai[1] = 1;
								}
							}
						}
					}
					else
					{
						if((NPC.direction == 1 && player.Center.X < NPC.position.X-28) || (NPC.direction == -1 && player.Center.X > NPC.position.X+28))
						{
							ChangeDir(-NPC.direction);
						}
					}
					if(NPC.ai[1] >= 1)
					{
						if(NPC.ai[1] == 1)
						{
							anim_ClawTransition += 0.3f;
							if(anim_ClawTransition >= 1f)
							{
								anim_ClawTransition = 1f;
								
								anim_Claw += 0.15f;
								if((anim_Claw >= 3f && anim_Claw <= 4f) || (anim_Claw >= 7f && anim_Claw <= 8f))
								{
									anim_Claw += 0.05f;
									
									for(int i = -1; i < 2; i++)
									{
										float dist = 50f;//54f;
										Vector2 clawPos = RArmPos[2];
										clawPos += Angle.AngleFlip(RArmRot[0]+1.57f + 0.4f*i,NPC.direction).ToRotationVector2() * dist;
										Vector2 clawVel = Angle.AngleFlip(RArmRot[0]+1.57f + 0.4f*i,NPC.direction).ToRotationVector2() * 4f;
										if(anim_Claw >= 7f)
										{
											clawPos = LArmPos[2];
											clawPos += Angle.AngleFlip(LArmRot[0]+1.57f + 0.4f*i,NPC.direction).ToRotationVector2() * dist;
											clawVel = Angle.AngleFlip(LArmRot[0]+1.57f + 0.4f*i,NPC.direction).ToRotationVector2() * 4f;
										}
										int slash = Projectile.NewProjectile(NPC.GetSource_FromAI(), clawPos.X,clawPos.Y,clawVel.X,clawVel.Y,ModContent.ProjectileType<Projectiles.Boss.TorizoSwipe>(),(int)((float)clawDamage/2f),8f);
									}
									if(soundCounter <= 0)
									{
										Vector2 sndPos = RArmPos[2];
										if(anim_Claw >= 7f)
										{
											sndPos = LArmPos[2];
										}
										SoundEngine.PlaySound(Sounds.NPCs.TorizoSwipe, sndPos);
										soundCounter = 4;
									}
								}
								if(anim_Claw >= 8f)
								{
									NPC.ai[1] = 2;
									anim_Claw = 8f;
								}
							}
						}
						if(NPC.ai[1] == 2)
						{
							anim_ClawTransition -= 0.1f;
							if(anim_ClawTransition <= 0f)
							{
								NPC.ai[1] = 3;
								anim_ClawTransition = 0f;
								anim_Claw = 1f;
							}
						}
						if(NPC.ai[1] >= 3)
						{
							NPC.ai[1]++;
							if(NPC.ai[1] > 30)
							{
								NPC.TargetClosest(true);
								NPC.netUpdate = true;
								NPC.ai[1] = 0f;
							}
						}
						if(soundCounter > 0)
						{
							soundCounter--;
						}
						//NPC.ai[2] = 0;
						NPC.ai[3] = 0;
					}
					else
					{
						NPC.ai[3]++;
						if(NPC.ai[3] > 200)
						{
							NPC.netUpdate = true;
							
							if(walkFlag)
							{
								if(walkFlagR)
								{
									anim_Walk = 6f;
								}
								else if(walkFlagL)
								{
									anim_Walk = 1f;
								}
								
								int num = 0;
								if(Math.Abs(player.Center.X-NPC.Center.X) > 100)
								{
									num = Main.rand.Next((int)Math.Abs(player.Center.X-NPC.Center.X)-100);
								}
								
								if(Head != null && Head.active && num < 50 && Math.Abs(player.Center.X-NPC.Center.X) < 300)
								{
									NPC.ai[0] = 3;
								}
								else
								{
									NPC.ai[0] = 4;
								}
								
								NPC.ai[1] = 0;
								NPC.ai[2] = 0;
								NPC.ai[3] = 0;
							}
						}
						
						if(NPC.ai[1] <= -1)
						{
							NPC.TargetClosest(true);
							NPC.netUpdate = true;
							NPC.ai[0] = 2;
							NPC.ai[1] = 1;
							NPC.ai[2] = 0;
							NPC.ai[3] = 0;
						}
						soundCounter = 0;
					}
					
					float moveSpeed = speed_Walk[(int)Math.Min(anim_Walk,speed_Walk.Length)-1] * speed * fullScale.X;
					
					if(NPC.direction == 1)
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X *= 0.98f;
						}
						NPC.velocity.X += 0.5f;
					}
					else if(NPC.direction == -1)
					{
						if (NPC.velocity.X > 0f)
						{
							NPC.velocity.X *= 0.98f;
						}
						NPC.velocity.X -= 0.5f;
					}
					if (NPC.velocity.X > moveSpeed)
					{
						NPC.velocity.X = moveSpeed;
					}
					if (NPC.velocity.X < -moveSpeed)
					{
						NPC.velocity.X = -moveSpeed;
					}
					
					SetAnimation("walk", anim_Walk);
					SetAnimation("claw", anim_Claw, anim_ClawTransition);
					
					SetBodyOffset();
					
					HeadFrame = 3;
					spawnAlpha = 0f;
				}
				// jump
				if(NPC.ai[0] == 2)
				{
					Player player = Main.player[NPC.target];
					
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
						
						anim_SpinJumpTransition += 0.085f;//0.075f;
						if(anim_SpinJumpTransition >= 1f)
						{
							anim_SpinJumpTransition = 1f;
							NPC.ai[2] = 1;
							NPC.ai[3] = 0;
						}
					}
					if(NPC.ai[2] >= 1 && NPC.ai[2] <= 2)
					{
						if(NPC.ai[2] == 1)
						{
							if(NPC.ai[1] == 1)
							{
								NPC.velocity.X = -7*NPC.direction;
								NPC.velocity.Y = -12f;
							}
							else
							{
								float diffX = (player.Center.X-NPC.Center.X);
								float diffY = (player.Center.Y-NPC.Center.Y);
								NPC.velocity.X = 7*Math.Sign(diffX) + MathHelper.Clamp(diffX*0.025f,-3,3);
								NPC.velocity.Y = -8f + MathHelper.Clamp(diffY*0.25f,-8,0f);//-16f;
							}
							NPC.ai[2] = 2;
						}
						
						if(NPC.ai[3] == 0)
						{
							if(NPC.velocity.Y > 3f && NPC.ai[1] == 0)
							{
								NPC.ai[2] = 1;
								NPC.TargetClosest(true);
								screwBoosted = false;
							}
						}
						if(NPC.Center.Y+100 < player.Center.Y)
						{
							NPC.ai[3] = 1;
						}
						
						if(anim_SpinJump < 3f)
						{
							anim_SpinJump = Math.Min(anim_SpinJump+0.25f,3f);
						}
						else
						{
							int sign = Math.Sign(NPC.velocity.X);
							if(sign == 0)
							{
								sign = NPC.direction;
							}
							anim_SpinJump_Spin += 30f * sign*NPC.direction;//22.5f;
							if(anim_SpinJump_Spin > 360f)
							{
								anim_SpinJump_Spin -= 360f;
							}
							if(anim_SpinJump_Spin < 0f)
							{
								anim_SpinJump_Spin += 360f;
							}
							screwAttack = true;
							
							if(!screwBoosted)
							{
								SoundEngine.PlaySound(Sounds.Items.Weapons.ScrewAttackSpeed, BodyPos[0]);
								screwBoostAlpha = 2f;
								screwBoosted = true;
							}
						}
						
						if(NPC.velocity.Y == 0f)
						{
							SoundEngine.PlaySound(Sounds.NPCs.TorizoStep, RLegPos[2]);
							NPC.ai[2] = 3;
							anim_SpinJump_Spin = 0f;
							screwAttack = false;
						}
						
						anim_Walk = 1f;
					}
					if(NPC.ai[2] == 3)
					{
						anim_SpinJump_Spin = 0f;
						screwAttack = false;
						
						NPC.velocity.X = 0f;
						if(anim_SpinJump > 2f)
						{
							anim_SpinJump -= 0.1f;
						}
						anim_SpinJump = Math.Max(anim_SpinJump-0.2f,1f);
						if(anim_SpinJump <= 1f)
						{
							anim_SpinJumpTransition -= 0.1f;
							if(anim_SpinJumpTransition <= 0f)
							{
								anim_SpinJumpTransition = 0f;
								NPC.TargetClosest(true);
								NPC.netUpdate = true;
								if(NPC.ai[1] == 1)
								{
									if(Head != null && Head.active && Main.rand.Next(3) <= 0)//Main.rand.Next(5) > 0)
									{
										NPC.ai[0] = 3;
									}
									else
									{
										NPC.ai[0] = 4;
									}
								}
								else
								{
									NPC.ai[0] = 1;
								}
								NPC.ai[1] = 0;
								NPC.ai[2] = 0;
								NPC.ai[3] = 0;
								anim_SpinJump = 1f;
								anim_Walk = 1f;
							}
						}
					}
					
					SetAnimation("walk", anim_Walk);
					SetAnimation("spin jump", anim_SpinJump, anim_SpinJumpTransition);
					
					SetBodyOffset();
					BodyOffset.Y = MathHelper.Lerp(BodyOffset.Y, Angle.LerpArray(0f,BodyOffset_SpinJump,anim_SpinJump), anim_SpinJumpTransition);
				}
				else
				{
					screwAttack = false;
				}
				// bomb spew
				if(NPC.ai[0] == 3)
				{
					if(NPC.ai[1] == 0)
					{
						if(anim_BombTransition < 1f)
						{
							anim_BombTransition += 0.085f;//0.075f;
							HeadFrame = 4;
						}
						else
						{
							anim_BombTransition = 1f;
							//anim_Bomb = Math.Min(anim_Bomb+0.075f,4f);
							anim_Bomb = Math.Min(anim_Bomb+0.1f,4f);
							
							if(HeadFrame < 8)
							{
								HeadFrameCounter++;
								if(HeadFrameCounter > 7)
								{
									HeadFrame++;
									HeadFrameCounter = 0;
								}
							}
							else
							{
								HeadFrame = 8;
								HeadFrameCounter = 0;
								
								NPC.ai[2]++;
								if(NPC.ai[2] > 5)
								{
									NPC.ai[1] = 1;
									NPC.ai[2] = 0;
								}
							}
						}
					}
					if(NPC.ai[1] == 1)
					{
						bool headFlag = (Head != null && Head.active);
						if((NPC.ai[2] == 10 || NPC.ai[2] == 20 || NPC.ai[2] == 30) && headFlag)
						{
							var entitySource = NPC.GetSource_FromAI();
							for(int i = 0; i < 3; i++)
							{
								Vector2 bombPos = HeadPos[0] + new Vector2(32f*NPC.direction,-6f);
								Vector2 bombVel = new Vector2(3f*NPC.direction,-3f);
								bombVel.X += (Main.rand.Next(50) - 25) * 0.05f;
								bombVel.Y += (Main.rand.Next(50) - 25) * 0.05f;
								
								NPC bomb = Main.npc[NPC.NewNPC(entitySource, (int)bombPos.X,(int)bombPos.Y,ModContent.NPCType<OrbBomb_Golden>(),NPC.whoAmI)];
								bomb.Center = bombPos;
								bomb.velocity = bombVel;
							}
						}
						if(NPC.ai[2] == 10 && headFlag)
						{
							soundInstance = SoundEngine.PlaySound(Sounds.NPCs.TorizoHit, Head.Center);
						}
						
						NPC.ai[2] += 2f;
						if(NPC.ai[2] > 60 || !headFlag)
						{
							NPC.ai[1] = 2;
							NPC.ai[2] = 0;
							HeadFrame = 7;
						}
					}
					if(NPC.ai[1] == 2)
					{
						//anim_Bomb = Math.Max(anim_Bomb-0.075f,1f);
						anim_Bomb = Math.Max(anim_Bomb-0.1f,1f);
						if(HeadFrame > 4)
						{
							HeadFrameCounter++;
							if(HeadFrameCounter > 7)
							{
								HeadFrame--;
								HeadFrameCounter = 0;
							}
						}
						else if(anim_BombTransition > 0f)
						{
							anim_BombTransition -= 0.075f;
							HeadFrame = 3;
						}
						else
						{
							anim_BombTransition = 0f;
							HeadFrame = 3;
							
							NPC.TargetClosest(true);
							NPC.netUpdate = true;
							NPC.ai[0] = 1;
							NPC.ai[1] = 0;
							NPC.ai[2] = 0;
							NPC.ai[3] = 0;
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
					
					SetAnimation("walk", anim_Walk);
					SetAnimation("bomb", anim_Bomb, anim_BombTransition);
					
					SetBodyOffset();
				}
				// ranged claw attack
				if(NPC.ai[0] == 4)
				{
					Player player = Main.player[NPC.target];
					
					if(NPC.ai[1] == 0)
					{
						if(anim_ClawTransition < 1f)
						{
							anim_ClawTransition += 0.1f;
						}
						else
						{
							anim_ClawTransition = 1f;
							
							anim_Claw += 0.15f;//0.05f;
							if((anim_Claw >= 3f && anim_Claw < 4f) || (anim_Claw >= 7f && anim_Claw < 8f))
							{
								anim_Claw += 0.15f;//0.05f;
							}
							if(anim_Claw >= 9f)
							{
								if(Main.rand.Next(6)+5 <= NPC.ai[2])
								{
									NPC.ai[1] = 1;
								}
								else
								{
									NPC.ai[2]++;
								}
								anim_Claw = 1f;
							}
							
							if((anim_Claw > 3.5f && anim_Claw < 4f) || (anim_Claw > 7.5f && anim_Claw < 8f))
							{
								if(NPC.ai[3] == 0)
								{
									NPC.TargetClosest(true);
									/*Vector2 clawPos = BodyPos[0] + new Vector2(32*NPC.direction,-10);
									if(Main.rand.Next(4) == 0 || (player.Center.Y > BodyPos[0].Y && Main.rand.Next(4) > 0))
									{
										clawPos.X = BodyPos[0].X + 26*NPC.direction;
										clawPos.Y = BodyPos[0].Y + 70;
									}
									Vector2 clawVel = new Vector2(8f*NPC.direction,0f);*/
									float targetrot = (float)Math.Atan2(player.Center.Y - (BodyPos[0].Y+30), player.Center.X - BodyPos[0].X);
									
									Vector2 clawPos = BodyPos[0] + targetrot.ToRotationVector2()*32;
									Vector2 clawVel = targetrot.ToRotationVector2() * 12f;
									int slash = Projectile.NewProjectile(NPC.GetSource_FromAI(),clawPos.X,clawPos.Y,clawVel.X,clawVel.Y,ModContent.ProjectileType<Projectiles.Boss.TorizoClawBeam>(),(int)((float)clawDamage/2f),8f);
									Main.projectile[slash].tileCollide = false;
									SoundEngine.PlaySound(Sounds.NPCs.TorizoWave, clawPos);
									NPC.ai[3] = 1;
								}
							}
							else
							{
								NPC.ai[3] = 0;
							}
						}
					}
					if(NPC.ai[1] == 1)
					{
						if(anim_ClawTransition > 0f)
						{
							anim_ClawTransition -= 0.1f;
						}
						else
						{
							anim_ClawTransition = 0f;
							
							NPC.TargetClosest(true);
							NPC.netUpdate = true;
							NPC.ai[0] = 1;
							NPC.ai[1] = 0;
							NPC.ai[2] = 0;
							NPC.ai[3] = 0;
							anim_Claw = 1f;
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
					
					SetAnimation("walk", anim_Walk);
					SetAnimation("claw", anim_Claw, anim_ClawTransition);
					
					SetBodyOffset();
				}
			}
			
			
			
			SetPositions();
			
			state = 0;
			stateAlpha = 0f;
			if(NPC.life < NPC.lifeMax/2)
			{
				state = 1;
				stateAlpha = 1f - (float)NPC.life / (NPC.lifeMax/2);
				spawnAlpha = stateAlpha;
			}
			
			if(screwAttack)
			{
				if(Body != null && Body.active)
				{
					Body.chaseable = false;
				}
				
				if(screwBoostAlpha > 0f)
				{
					screwBoostAlpha -= 1f/30f;
				}
				else
				{
					screwBoostAlpha = 0f;
				}
				
				if(screwSoundCounter <= 0)
				{
					SoundEngine.PlaySound(Sounds.Items.Weapons.ScrewAttack, BodyPos[0]);
					screwSoundCounter = 16;
				}
				else
				{
					screwSoundCounter--;
				}
				
				screwFrameCounter++;
				if(screwFrameCounter > 2)
				{
					screwFrame++;
					screwFrameCounter = 0;
				}
				if(screwFrame >= 4)
				{
					screwFrame = 0;
				}
			}
			else
			{
				if(Body != null && Body.active)
				{
					Body.chaseable = true;
				}
				screwFrame = 0;
				screwFrameCounter = 0;
				screwSoundCounter = 0;
				screwBoosted = false;
				screwBoostAlpha = 0f;
			}
			
			glowAlpha += 0.017f * glowNum;
			if(glowAlpha >= 1f)
			{
				glowAlpha = 1f;
				glowNum = -1;
			}
			if(glowAlpha <= 0f)
			{
				glowAlpha = 0f;
				glowNum = 1;
			}
			
			if(NPC.ai[0] == 2 && NPC.ai[1] == 1 && Body != null && Body.active)
			{
				//if((NPC.direction == 1 && NPC.velocity.X < 0) || (NPC.direction == -1 && NPC.velocity.X > 0))
				//{
					Vector2 velocity = Collision.TileCollision(NPC.position-new Vector2(28,47),NPC.velocity,56,47+numH);
					NPC.velocity.X = velocity.X;
				//}
			}
			
			//if(((NPC.Center.X-100 < Main.player[NPC.target].Center.X && NPC.Center.X+100 > Main.player[NPC.target].Center.X) || (NPC.ai[0] == 0 && NPC.ai[1] == 0)) && NPC.position.Y+numH < Main.player[NPC.target].position.Y+Main.player[NPC.target].height - 16f)
			if(NPC.position.Y+numH < Main.player[NPC.target].position.Y+Main.player[NPC.target].height - 16f && NPC.ai[0] <= 2 && (NPC.ai[0] != 1 || NPC.ai[1] == 0))
			{
				NPC.velocity.Y += 0.5f;
				if(NPC.velocity.Y == 0f)
				{
					NPC.velocity.Y = 0.1f;
				}
			}
			else
			{
				if(Collision.SolidCollision(new Vector2(NPC.position.X,NPC.position.Y+numH-16f), NPC.width, 16) && NPC.position.Y+numH > Main.player[NPC.target].position.Y+Main.player[NPC.target].height)
				{
					if (NPC.velocity.Y > -4f)
					{
						if (NPC.velocity.Y > 0f)
						{
							NPC.velocity.Y = 0f;
						}
						if (NPC.velocity.Y > -0.2f)
						{
							NPC.velocity.Y -= 0.025f;
						}
						else
						{
							NPC.velocity.Y -= 0.2f;
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
				if(NPC.position.Y+numH < Main.player[NPC.target].position.Y && NPC.ai[0] <= 2 && (NPC.ai[0] != 1 || NPC.ai[1] == 0))
				{
					fall = true;
				}
				Vector2 velocity = Collision.TileCollision(NPC.position,new Vector2(0f,Math.Max(NPC.velocity.Y,0f)),NPC.width,numH,fall,fall);
				NPC.velocity.Y = Math.Min(velocity.Y,NPC.velocity.Y);
			}
			if (NPC.velocity.Y > 10f)
			{
				NPC.velocity.Y = 10f;
			}
			
			if(Body != null && Body.active)
			{
				Body.Center = BodyPos[1];
			}
			if(Head != null && Head.active)
			{
				Head.Center = HeadPos[1];
			}
			for(int i = 0; i < 2; i++)
			{
				NPC RArm = GetArm(false, i),
					LArm = GetArm(true, i),
					RLeg = GetLeg(false, i),
					LLeg = GetLeg(true, i);
				if(RArm != null && RArm.active)
				{
					RArm.Center = RArmPos[i+3];
				}
				if(LArm != null && LArm.active)
				{
					LArm.Center = LArmPos[i+3];
				}
				if(RLeg != null && RLeg.active)
				{
					RLeg.Center = RLegPos[i+3];
				}
				if(LLeg != null && LLeg.active)
				{
					LLeg.Center = LLegPos[i+3];
				}
			}
			if(RHand != null && RHand.active)
			{
				RHand.Center = RHandPos[RHandFrame];
			}
			if(LHand != null && LHand.active)
			{
				LHand.Center = LHandPos[LHandFrame];
			}
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

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			NPC.spriteDirection = NPC.direction;
			SpriteEffects effects = SpriteEffects.None;
			if (NPC.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			
			Texture2D texHead = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoHead").Value,
					texHead_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoHead_EyeGlow").Value,
					texHead_Glow2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoHead_EyeGlow2").Value,
					texBody = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoBody").Value,
					texBody_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoBody_Glow").Value,
					texShoulderF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoShoulder_Front").Value,
					texShoulderB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoShoulder_Back").Value,
					texArmF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoArm_Front").Value,
					texArmB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoArm_Back").Value,
					texHandF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoHand_Front").Value,
					texHandB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoHand_Back").Value,
					texThighF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoLegThigh_Front").Value,
					texThighB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoLegThigh_Back").Value,
					texCalfF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoLegCalf_Front").Value,
					texCalfB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoLegCalf_Back").Value,
					texFootF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoLegFoot_Front").Value,
					texFootB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoLegFoot_Back").Value;
			
			Texture2D texSpawnHead = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoHead").Value,
					texSpawnHead_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoHead_EyeGlow").Value,
					texSpawnHead_Glow2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoHead_EyeGlow2").Value,
					texSpawnBody = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoBody").Value,
					texSpawnBody_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoBody_Glow").Value,
					texSpawnShoulderF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoShoulder_Front").Value,
					texSpawnShoulderB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoShoulder_Back").Value,
					texSpawnArmF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoArm_Front").Value,
					texSpawnArmB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoArm_Back").Value,
					texSpawnHandF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoHand_Front").Value,
					texSpawnHandB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoHand_Back").Value,
					texSpawnThighF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoLegThigh_Front").Value,
					texSpawnThighB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoLegThigh_Back").Value,
					texSpawnCalfF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoLegCalf_Front").Value,
					texSpawnCalfB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoLegCalf_Back").Value,
					texSpawnFootF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoLegFoot_Front").Value,
					texSpawnFootB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/Spawn/GoldenTorizoLegFoot_Back").Value;
			if(state == 1)
			{
				texSpawnHead = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoHead").Value;
				texSpawnHead_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoHead_EyeGlow").Value;
				texSpawnHead_Glow2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoHead_EyeGlow2").Value;
				texSpawnBody = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoBody").Value;
				texSpawnBody_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GoldenTorizoBody_Glow").Value;
				texSpawnShoulderF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoShoulder_Front").Value;
				texSpawnShoulderB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoShoulder_Back").Value;
				texSpawnArmF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoArm_Front").Value;
				texSpawnArmB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoArm_Back").Value;
				texSpawnHandF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoHand_Front").Value;
				texSpawnHandB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoHand_Back").Value;
				texSpawnThighF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoLegThigh_Front").Value;
				texSpawnThighB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoLegThigh_Back").Value;
				texSpawnCalfF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoLegCalf_Front").Value;
				texSpawnCalfB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoLegCalf_Back").Value;
				texSpawnFootF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoLegFoot_Front").Value;
				texSpawnFootB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoLegFoot_Back").Value;
			}
			
			Color headColor = NPC.GetAlpha(Lighting.GetColor((int)HeadPos[1].X / 16, (int)HeadPos[1].Y / 16)),
			bodyColor = NPC.GetAlpha(Lighting.GetColor((int)BodyPos[1].X / 16, (int)BodyPos[1].Y / 16)),
			shoulderColorF = NPC.GetAlpha(Lighting.GetColor((int)RArmPos[3].X / 16, (int)RArmPos[3].Y / 16)),
			shoulderColorB = NPC.GetAlpha(Lighting.GetColor((int)LArmPos[3].X / 16, (int)LArmPos[3].Y / 16)),
			armColorF = NPC.GetAlpha(Lighting.GetColor((int)RArmPos[4].X / 16, (int)RArmPos[4].Y / 16)),
			armColorB = NPC.GetAlpha(Lighting.GetColor((int)LArmPos[4].X / 16, (int)LArmPos[4].Y / 16)),
			handColorF = NPC.GetAlpha(Lighting.GetColor((int)RHand.Center.X / 16, (int)RHand.Center.Y / 16)),
			handColorB = NPC.GetAlpha(Lighting.GetColor((int)LHand.Center.X / 16, (int)LHand.Center.Y / 16)),
			thighColorF = NPC.GetAlpha(Lighting.GetColor((int)RLegPos[3].X / 16, (int)RLegPos[3].Y / 16)),
			thighColorB = NPC.GetAlpha(Lighting.GetColor((int)LLegPos[3].X / 16, (int)LLegPos[3].Y / 16)),
			calfColorF = NPC.GetAlpha(Lighting.GetColor((int)RLegPos[4].X / 16, (int)RLegPos[4].Y / 16)),
			calfColorB = NPC.GetAlpha(Lighting.GetColor((int)LLegPos[4].X / 16, (int)LLegPos[4].Y / 16)),
			footColorF = NPC.GetAlpha(Lighting.GetColor((int)RLegPos[2].X / 16, (int)RLegPos[2].Y / 16)),
			footColorB = NPC.GetAlpha(Lighting.GetColor((int)LLegPos[2].X / 16, (int)LLegPos[2].Y / 16));
			
			Color glowColor = Color.White*glowAlpha;
			
			Color eyeGlowColor = Color.White;
			
			int fHandFrame = RHandFrame;
			int bHandFrame = LHandFrame;
			int fFootFrame = RFootFrame;
			int bFootFrame = LFootFrame;
			if(NPC.spriteDirection == -1)
			{
				fHandFrame = LHandFrame;
				bHandFrame = RHandFrame;
				fFootFrame = LFootFrame;
				bFootFrame = RFootFrame;
			}
			
			float handRotF = RArmRot[2],
				handRotB = LArmRot[2];
			handRotF -= -(float)Angle.ConvertToRadians(60 - 30*fHandFrame);
			handRotB -= -(float)Angle.ConvertToRadians(60 - 30*bHandFrame);
			
			float headRot = HeadRot;
			if(HeadFrame >= 4)
			{
				headRot -= -(float)Angle.ConvertToRadians(45);
			}
			
			// back arm
			DrawLimbTexture(NPC, sb, texArmB, LArmPos[1], RArmPos[1], LArmRot[1], RArmRot[1], new Vector2(9,1), armColorB, armColorF, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnArmB, LArmPos[1], RArmPos[1], LArmRot[1], RArmRot[1], new Vector2(9,1), armColorB*spawnAlpha, armColorF*spawnAlpha, fullScale, effects);
			
			// back hand
			DrawLimbTexture(NPC, sb, texHandB, LArmPos[2], RArmPos[2], handRotB, handRotF, new Vector2(13,15), handColorB, handColorF, fullScale, effects, bHandFrame, 3);
			DrawLimbTexture(NPC, sb, texSpawnHandB, LArmPos[2], RArmPos[2], handRotB, handRotF, new Vector2(13,15), handColorB*spawnAlpha, handColorF*spawnAlpha, fullScale, effects, bHandFrame, 3);
			
			// back shoulder
			DrawLimbTexture(NPC, sb, texShoulderB, LArmPos[0], RArmPos[0], LArmRot[0], RArmRot[0], new Vector2(15,15), shoulderColorB, shoulderColorF, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnShoulderB, LArmPos[0], RArmPos[0], LArmRot[0], RArmRot[0], new Vector2(15,15), shoulderColorB*spawnAlpha, shoulderColorF*spawnAlpha, fullScale, effects);
			
			// back calf
			DrawLimbTexture(NPC, sb, texCalfB, LLegPos[1], RLegPos[1], LLegRot[1], RLegRot[1], new Vector2(17,1), calfColorB, calfColorF, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnCalfB, LLegPos[1], RLegPos[1], LLegRot[1], RLegRot[1], new Vector2(17,1), calfColorB*spawnAlpha, calfColorF*spawnAlpha, fullScale, effects);
			
			// back foot
			DrawLimbTexture(NPC, sb, texFootB, LLegPos[2], RLegPos[2], LLegRot[2], RLegRot[2], new Vector2(19,3), footColorB, footColorF, fullScale, effects, bFootFrame, 2);
			DrawLimbTexture(NPC, sb, texSpawnFootB, LLegPos[2], RLegPos[2], LLegRot[2], RLegRot[2], new Vector2(19,3), footColorB*spawnAlpha, footColorF*spawnAlpha, fullScale, effects, bFootFrame, 2);
			
			// back thigh
			DrawLimbTexture(NPC, sb, texThighB, LLegPos[0], RLegPos[0], LLegRot[0], RLegRot[0], new Vector2(13,5), thighColorB, thighColorF, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnThighB, LLegPos[0], RLegPos[0], LLegRot[0], RLegRot[0], new Vector2(13,5), thighColorB*spawnAlpha, thighColorF*spawnAlpha, fullScale, effects);
			
			// body
			DrawLimbTexture(NPC, sb, texBody, BodyPos[0], BodyPos[0], BodyRot, BodyRot, new Vector2(28,49), bodyColor, bodyColor, fullScale, effects, BodyFrame, 2);
			DrawLimbTexture(NPC, sb, texBody_Glow, BodyPos[0], BodyPos[0], BodyRot, BodyRot, new Vector2(28,49), glowColor, glowColor, fullScale, effects, BodyFrame, 2);
			DrawLimbTexture(NPC, sb, texSpawnBody, BodyPos[0], BodyPos[0], BodyRot, BodyRot, new Vector2(28,49), bodyColor*spawnAlpha, bodyColor*spawnAlpha, fullScale, effects, BodyFrame, 2);
			DrawLimbTexture(NPC, sb, texSpawnBody_Glow, BodyPos[0], BodyPos[0], BodyRot, BodyRot, new Vector2(28,49), glowColor*spawnAlpha, glowColor*spawnAlpha, fullScale, effects, BodyFrame, 2);
			
			// head
			if(Head != null && Head.active)
			{
				DrawLimbTexture(NPC, sb, texHead, HeadPos[0], HeadPos[0], headRot, headRot, new Vector2(32,38), headColor, headColor, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(NPC, sb, texHead_Glow, HeadPos[0], HeadPos[0], headRot, headRot, new Vector2(32,38), glowColor, glowColor, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(NPC, sb, texHead_Glow2, HeadPos[0], HeadPos[0], headRot, headRot, new Vector2(32,38), eyeGlowColor, eyeGlowColor, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(NPC, sb, texSpawnHead, HeadPos[0], HeadPos[0], headRot, headRot, new Vector2(32,38), headColor*spawnAlpha, headColor*spawnAlpha, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(NPC, sb, texSpawnHead_Glow, HeadPos[0], HeadPos[0], headRot, headRot, new Vector2(32,38), glowColor*spawnAlpha, glowColor*spawnAlpha, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(NPC, sb, texSpawnHead_Glow2, HeadPos[0], HeadPos[0], headRot, headRot, new Vector2(32,38), eyeGlowColor*spawnAlpha, eyeGlowColor*spawnAlpha, fullScale, effects, HeadFrame, 9);
			}
			
			// front calf
			DrawLimbTexture(NPC, sb, texCalfF, RLegPos[1], LLegPos[1], RLegRot[1], LLegRot[1], new Vector2(17,1), calfColorF, calfColorB, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnCalfF, RLegPos[1], LLegPos[1], RLegRot[1], LLegRot[1], new Vector2(17,1), calfColorF*spawnAlpha, calfColorB*spawnAlpha, fullScale, effects);
			
			// front foot
			DrawLimbTexture(NPC, sb, texFootF, RLegPos[2], LLegPos[2], RLegRot[2], LLegRot[2], new Vector2(19,3), footColorF, footColorB, fullScale, effects, fFootFrame, 2);
			DrawLimbTexture(NPC, sb, texSpawnFootF, RLegPos[2], LLegPos[2], RLegRot[2], LLegRot[2], new Vector2(19,3), footColorF*spawnAlpha, footColorB*spawnAlpha, fullScale, effects, fFootFrame, 2);
			
			// front thigh
			DrawLimbTexture(NPC, sb, texThighF, RLegPos[0], LLegPos[0], RLegRot[0], LLegRot[0], new Vector2(13,5), thighColorF, thighColorB, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnThighF, RLegPos[0], LLegPos[0], RLegRot[0], LLegRot[0], new Vector2(13,5), thighColorF*spawnAlpha, thighColorB*spawnAlpha, fullScale, effects);
			
			// front arm
			DrawLimbTexture(NPC, sb, texArmF, RArmPos[1], LArmPos[1], RArmRot[1], LArmRot[1], new Vector2(9,1), armColorF, armColorB, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnArmF, RArmPos[1], LArmPos[1], RArmRot[1], LArmRot[1], new Vector2(9,1), armColorF*spawnAlpha, armColorB*spawnAlpha, fullScale, effects);
			
			// front hand
			DrawLimbTexture(NPC, sb, texHandF, RArmPos[2], LArmPos[2], handRotF, handRotB, new Vector2(13,15), handColorF, handColorB, fullScale, effects, fHandFrame, 3);
			DrawLimbTexture(NPC, sb, texSpawnHandF, RArmPos[2], LArmPos[2], handRotF, handRotB, new Vector2(13,15), handColorF*spawnAlpha, handColorB*spawnAlpha, fullScale, effects, fHandFrame, 3);
			
			// front shoulder
			DrawLimbTexture(NPC, sb, texShoulderF, RArmPos[0], LArmPos[0], RArmRot[0], LArmRot[0], new Vector2(15,15), shoulderColorF, shoulderColorB, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnShoulderF, RArmPos[0], LArmPos[0], RArmRot[0], LArmRot[0], new Vector2(15,15), shoulderColorF*spawnAlpha, shoulderColorB*spawnAlpha, fullScale, effects);
			
			
			if(screwAttack)
			{
				Texture2D saTex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GT_ScrewAttack").Value;
				int height = saTex.Height / 4;
				sb.Draw(saTex, BodyPos[0] - Main.screenPosition, new Rectangle?(new Rectangle(0,height*screwFrame,saTex.Width,height)),Color.White,-(float)Angle.ConvertToRadians((double)anim_SpinJump_Spin)*NPC.spriteDirection,new Vector2(saTex.Width/2,height/2),1f,effects,0f);
				
				if(screwBoostAlpha > 0f)
				{
					float alpha = Math.Min(screwBoostAlpha,1f);
					Color saColor = Color.White;
					saColor.A = (byte)((float)saColor.A*alpha);
					Texture2D saTex2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GT_ScrewAttack_Yellow").Value,
							saTex3 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/GoldenTorizo/GT_ScrewAttack_YellowGlow").Value;
					height = saTex2.Height / 4;
					sb.Draw(saTex2, BodyPos[0] - Main.screenPosition, new Rectangle?(new Rectangle(0,height*screwFrame,saTex2.Width,height)),saColor*alpha,-(float)Angle.ConvertToRadians((double)anim_SpinJump_Spin)*NPC.spriteDirection,new Vector2(saTex2.Width/2,height/2),1f,effects,0f);
					sb.Draw(saTex3, BodyPos[0] - Main.screenPosition, new Rectangle?(new Rectangle(0,0,saTex3.Width,saTex3.Height)),saColor*alpha*0.5f,(float)Angle.ConvertToRadians((double)anim_SpinJump_Spin)*NPC.spriteDirection,new Vector2(saTex3.Width/2,saTex3.Height/2),1f,effects,0f);
				}
			}
			
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
			writer.Write((byte)_head);
			writer.Write((byte)_rHand);
			writer.Write((byte)_lHand);

			for (int i = 0; i < 2; ++i)
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
			_head = reader.ReadByte();
			_rHand = reader.ReadByte();
			_lHand = reader.ReadByte();

			for (int i = 0; i < 2; ++i)
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
