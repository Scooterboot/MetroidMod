using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MetroidMod;

namespace MetroidMod.NPCs.Torizo
{
    public class Torizo : ModNPC
    {
		public override string Texture => mod.Name + "/NPCs/Torizo/TorizoBody";
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo");
		}
		public override void SetDefaults()
		{
			npc.width = 0;//56;
			npc.height = 0;//94;
			npc.scale = 1f;
			npc.damage = 10;
			npc.defense = 10;
			npc.lifeMax = 2000;
			//npc.dontTakeDamage = true;
			npc.boss = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.value = Item.buyPrice(0, 0, 7, 0);
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.behindTiles = true;
			npc.buffImmune[31] = true;
			npc.buffImmune[mod.BuffType("IceFreeze")] = true;
			npc.buffImmune[mod.BuffType("InstantFreeze")] = true;
			npc.aiStyle = -1;
			npc.npcSlots = 1;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Torizo");
			bossBag = mod.ItemType("TorizoBag");
			npc.chaseable = false;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.7f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.7f);
		}
		public override void NPCLoot()
		{
			MWorld.bossesDown |= MetroidBossDown.downedTorizo;
			if (Main.expertMode)
			{
				npc.DropBossBags();
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EnergyShard"), Main.rand.Next(15, 36));
				if (Main.rand.Next(5) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RidleyMusicBox"));
				}
				if (Main.rand.Next(7) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TorizoMask"));
				}
				if (Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TorizoTrophy"));
				}
			}
		}
		public override bool? CanBeHitByItem(Player player, Item item) => false;
		public override bool? CanBeHitByProjectile(Projectile projectile) => false;
		
		SoundEffectInstance soundInstance;
		public override void HitEffect(int hitDirection, double damage)
		{
			if(Head != null && Head.active && (soundInstance == null || soundInstance.State != SoundState.Playing))
			{
				soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)Head.Center.X, (int)Head.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/TorizoHit"));
			}
		}
		
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.5f;
			position = new Vector2(npc.Center.X,npc.position.Y+168);
			if(npc.life < npc.lifeMax)
			{
				return true;
			}
			return null;
		}
		
		public override void BossHeadSlot(ref int index)
		{
			index = NPCHeadLoader.GetBossHeadSlot(MetroidMod.TorizoHead);
		}
		
		int _body, _head, _rHand, _lHand;
		NPC Body => Main.npc[_body];
		NPC RHand => Main.npc[_rHand];
		NPC LHand => Main.npc[_lHand];
		
		NPC Head
		{
			get
			{
				if(Main.npc[_head].type == mod.NPCType("Torizo_HitBox") && Main.npc[_head].ai[1] == 0f)
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
			
			BodyPos[0] = npc.Center + (CurrentBodyPos[0] + BodyOffset);
			BodyPos[1] = BodyPos[0] + Angle.AngleFlip(BodyOffsetRot + BodyRot, npc.direction).ToRotationVector2()*BodyDist * fullScale;
			
			RArmPos[0] = BodyPos[0] + Angle.AngleFlip(RArmOffsetRot[0] + BodyRot, npc.direction).ToRotationVector2()*RArmDist[0] * fullScale;
			RArmPos[1] = RArmPos[0] + Angle.AngleFlip(RArmOffsetRot[1] + RArmRot[0], npc.direction).ToRotationVector2()*RArmDist[1] * fullScale;
			RArmPos[2] = RArmPos[1] + Angle.AngleFlip(RArmOffsetRot[2] + RArmRot[1], npc.direction).ToRotationVector2()*RArmDist[2] * fullScale;
			RArmPos[3] = RArmPos[0] + Angle.AngleFlip(RArmOffsetRot[3] + RArmRot[0], npc.direction).ToRotationVector2()*RArmDist[3] * fullScale;
			RArmPos[4] = RArmPos[1] + Angle.AngleFlip(RArmOffsetRot[4] + RArmRot[1], npc.direction).ToRotationVector2()*RArmDist[4] * fullScale;
			
			RHandPos[0] = RArmPos[2] + Angle.AngleFlip(RHandOffsetRot[0] + RArmRot[2]+(float)Angle.ConvertToRadians(60), npc.direction).ToRotationVector2()*RHandDist[0] * fullScale;
			RHandPos[1] = RArmPos[2] + Angle.AngleFlip(RHandOffsetRot[1] + RArmRot[2]+(float)Angle.ConvertToRadians(30), npc.direction).ToRotationVector2()*RHandDist[1] * fullScale;
			RHandPos[2] = RArmPos[2] + Angle.AngleFlip(RHandOffsetRot[2] + RArmRot[2], npc.direction).ToRotationVector2()*RHandDist[2] * fullScale;
			
			LArmPos[0] = BodyPos[0] + Angle.AngleFlip(LArmOffsetRot[0] + BodyRot, npc.direction).ToRotationVector2()*LArmDist[0] * fullScale;
			LArmPos[1] = LArmPos[0] + Angle.AngleFlip(LArmOffsetRot[1] + LArmRot[0], npc.direction).ToRotationVector2()*LArmDist[1] * fullScale;
			LArmPos[2] = LArmPos[1] + Angle.AngleFlip(LArmOffsetRot[2] + LArmRot[1], npc.direction).ToRotationVector2()*LArmDist[2] * fullScale;
			LArmPos[3] = LArmPos[0] + Angle.AngleFlip(LArmOffsetRot[3] + LArmRot[0], npc.direction).ToRotationVector2()*LArmDist[3] * fullScale;
			LArmPos[4] = LArmPos[1] + Angle.AngleFlip(LArmOffsetRot[4] + LArmRot[1], npc.direction).ToRotationVector2()*LArmDist[4] * fullScale;
			
			LHandPos[0] = LArmPos[2] + Angle.AngleFlip(LHandOffsetRot[0] + LArmRot[2]+(float)Angle.ConvertToRadians(60), npc.direction).ToRotationVector2()*LHandDist[0] * fullScale;
			LHandPos[1] = LArmPos[2] + Angle.AngleFlip(LHandOffsetRot[1] + LArmRot[2]+(float)Angle.ConvertToRadians(30), npc.direction).ToRotationVector2()*LHandDist[1] * fullScale;
			LHandPos[2] = LArmPos[2] + Angle.AngleFlip(LHandOffsetRot[2] + LArmRot[2], npc.direction).ToRotationVector2()*LHandDist[2] * fullScale;
			
			RLegPos[0] = BodyPos[0] + Angle.AngleFlip(RLegOffsetRot[0] + BodyRot, npc.direction).ToRotationVector2()*RLegDist[0] * fullScale;
			RLegPos[1] = RLegPos[0] + Angle.AngleFlip(RLegOffsetRot[1] + RLegRot[0], npc.direction).ToRotationVector2()*RLegDist[1] * fullScale;
			RLegPos[2] = RLegPos[1] + Angle.AngleFlip(RLegOffsetRot[2] + RLegRot[1], npc.direction).ToRotationVector2()*RLegDist[2] * fullScale;
			RLegPos[3] = RLegPos[0] + Angle.AngleFlip(RLegOffsetRot[3] + RLegRot[0], npc.direction).ToRotationVector2()*RLegDist[3] * fullScale;
			RLegPos[4] = RLegPos[1] + Angle.AngleFlip(RLegOffsetRot[4] + RLegRot[1], npc.direction).ToRotationVector2()*RLegDist[4] * fullScale;
			
			LLegPos[0] = BodyPos[0] + Angle.AngleFlip(LLegOffsetRot[0] + BodyRot, npc.direction).ToRotationVector2()*LLegDist[0] * fullScale;
			LLegPos[1] = LLegPos[0] + Angle.AngleFlip(LLegOffsetRot[1] + LLegRot[0], npc.direction).ToRotationVector2()*LLegDist[1] * fullScale;
			LLegPos[2] = LLegPos[1] + Angle.AngleFlip(LLegOffsetRot[2] + LLegRot[1], npc.direction).ToRotationVector2()*LLegDist[2] * fullScale;
			LLegPos[3] = LLegPos[0] + Angle.AngleFlip(LLegOffsetRot[3] + LLegRot[0], npc.direction).ToRotationVector2()*LLegDist[3] * fullScale;
			LLegPos[4] = LLegPos[1] + Angle.AngleFlip(LLegOffsetRot[4] + LLegRot[1], npc.direction).ToRotationVector2()*LLegDist[4] * fullScale;
			
			HeadPos[0] = BodyPos[0] + Angle.AngleFlip(HeadOffsetRot[0] + BodyRot, npc.direction).ToRotationVector2()*HeadDist[0] * fullScale;
			HeadPos[1] = HeadPos[0] + Angle.AngleFlip(HeadOffsetRot[1] + HeadRot, npc.direction).ToRotationVector2()*HeadDist[1] * fullScale;
		}
		
		bool initialized = false;
		public override bool PreAI()
		{
			npc.noTileCollide = true;
			npc.noGravity = true;
			if(!initialized && Main.netMode != NetmodeID.MultiplayerClient)
			{
				npc.netUpdate = true;
				npc.TargetClosest(true);
				
				Player player = Main.player[npc.target];
				
				if(npc.ai[1] == 0)
				{
					npc.direction = 1;
					if (Main.rand.Next(2) == 0)
						npc.direction = -1;

					npc.velocity.X = 0f;
					npc.velocity.Y = 0.1f;
					npc.Center = new Vector2(player.Center.X - 150 * npc.direction, player.Center.Y - 1500);
				}
				
				SetPositions();
				
				_body = NPC.NewNPC((int)BodyPos[1].X,(int)BodyPos[1].Y,mod.NPCType("Torizo_HitBox"),npc.whoAmI, npc.whoAmI,1f);
				
				for(int i = 0; i < 2; i++)
				{
					_rArm[i] = NPC.NewNPC((int)RArmPos[i+3].X,(int)RArmPos[i+3].Y,mod.NPCType("Torizo_HitBox"),npc.whoAmI, npc.whoAmI,2+i);
					_lArm[i] = NPC.NewNPC((int)LArmPos[i+3].X,(int)LArmPos[i+3].Y,mod.NPCType("Torizo_HitBox"),npc.whoAmI, npc.whoAmI,2+i);
					_rLeg[i] = NPC.NewNPC((int)RLegPos[i+3].X,(int)RLegPos[i+3].Y,mod.NPCType("Torizo_HitBox"),npc.whoAmI, npc.whoAmI,5+i);
					_lLeg[i] = NPC.NewNPC((int)LLegPos[i+3].X,(int)LLegPos[i+3].Y,mod.NPCType("Torizo_HitBox"),npc.whoAmI, npc.whoAmI,5+i);
				}
				
				_rHand = NPC.NewNPC((int)RHandPos[0].X,(int)RHandPos[0].Y,mod.NPCType("Torizo_HitBox"),npc.whoAmI, npc.whoAmI,4f);
				_lHand = NPC.NewNPC((int)LHandPos[0].X,(int)LHandPos[0].Y,mod.NPCType("Torizo_HitBox"),npc.whoAmI, npc.whoAmI,4f);
				
				_head = NPC.NewNPC((int)HeadPos[1].X,(int)HeadPos[1].Y,mod.NPCType("Torizo_HitBox"),npc.whoAmI, npc.whoAmI);
				
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
		/*float[][] RArmAnim_Claw = new float[][]{
		new float[] {-22.5f,45f, 90f, 0f,-22.5f,-22.5f,-22.5f,-22.5f, -22.5f},
		new float[] { 22.5f,90f,180f,45f, 22.5f, 22.5f, 22.5f, 22.5f,  22.5f},
		new float[] {    0f,45f,135f, 0f,    0f,    0f,    0f,    0f,     0f}};
		
		float[][] LArmAnim_Claw = new float[][]{
		new float[] {-22.5f,-22.5f,-22.5f,-22.5f,-22.5f,45f, 90f, 0f, -22.5f},
		new float[] { 22.5f, 22.5f, 22.5f, 22.5f, 22.5f,90f,180f,45f,  22.5f},
		new float[] {    0f,    0f,    0f,    0f,    0f,45f,135f, 0f,     0f}};*/
		
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
			BodyPos[0] = npc.Center + (CurrentBodyPos[0] + BodyOffset);
			RLegPos[2] = RLegPos[1] + Angle.AngleFlip(RLegOffsetRot[2] + RLegRot[1], npc.direction).ToRotationVector2()*RLegDist[2] * fullScale;
			LLegPos[2] = LLegPos[1] + Angle.AngleFlip(LLegOffsetRot[2] + LLegRot[1], npc.direction).ToRotationVector2()*LLegDist[2] * fullScale;
			if(RLegPos[2].Y >= LLegPos[2].Y)
			{
				BodyOffset.Y = 117f - ((RLegPos[2].Y+(13f*fullScale.Y)) - BodyPos[0].Y) + hOffset;
			}
			else
			{
				BodyOffset.Y = 117f - ((LLegPos[2].Y+(13f*fullScale.Y)) - BodyPos[0].Y) + hOffset;
			}
		}
		
		int clawDamage = 30;
		
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
		
		int grounded = 15;
		
		public override void AI()
		{
			int numH = 117;//164;
			
			if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2500f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2500f)
			{
				npc.TargetClosest(true);
				if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2500f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2500f)
				{
					// despawn
					
					npc.alpha = Math.Min(npc.alpha + 10, 255);
					if(npc.alpha >= 255)
					{
						npc.active = false;
					}
				}
			}
			else
			{
				npc.alpha = Math.Max(npc.alpha - 10, 0);
				
				// spawn
				if(npc.ai[0] == 0)
				{
					npc.dontTakeDamage = true;
					
					float speed = 0f;
					
					// drop from sky
					if(npc.ai[1] == 0)
					{
						if(npc.velocity.Y != 0)
						{
							anim_Spawn = 4f;
						}
						else
						{
							if(anim_Spawn > 1f)
							{
								speed = -0.2f;
								anim_Spawn = Math.Max(anim_Spawn+speed,1f);
							}
							else
							{
								anim_Spawn = 1f;
								npc.ai[1] = 1;
							}
						}
					}
					// stand up
					if(npc.ai[1] == 1)
					{
						if(anim_Spawn < 7f)
						{
							npc.ai[2]++;
							if(npc.ai[2] > 90)
							{
								if(npc.ai[2] % 3 == 0)
								{
									if(HeadFrame < 3)
									{
										HeadFrame++;
									}
								}
								if(npc.ai[2] == 120 || npc.ai[2] == 134)
								{
									HeadFrame = 0;
								}
							}
							if(npc.ai[2] > 180)
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
								npc.TargetClosest(true);
								npc.netUpdate = true;
								npc.ai[0] = 1;
								npc.ai[1] = 0;
								npc.ai[2] = 0;
							}
						}
					}
					
					float moveSpeed = speed_Spawn[(int)Math.Min(anim_Spawn,speed_Spawn.Length)-1] * Math.Abs(speed) * fullScale.X;
					
					if(npc.direction == Math.Sign(speed))
					{
						if (npc.velocity.X < 0f)
						{
							npc.velocity.X *= 0.98f;
						}
						npc.velocity.X += 0.5f;
					}
					else if(npc.direction == -Math.Sign(speed))
					{
						if (npc.velocity.X > 0f)
						{
							npc.velocity.X *= 0.98f;
						}
						npc.velocity.X -= 0.5f;
					}
					if (npc.velocity.X > moveSpeed)
					{
						npc.velocity.X = moveSpeed;
					}
					if (npc.velocity.X < -moveSpeed)
					{
						npc.velocity.X = -moveSpeed;
					}
					
					SetAnimation("spawn", anim_Spawn);
					SetBodyOffset();
				}
				else
				{
					npc.dontTakeDamage = false;
				}
				// walk
				if(npc.ai[0] == 1)
				{
					Player player = Main.player[npc.target];
					
					float speed = 0.15f;
					if(Head == null || !Head.active)
					{
						speed *= 1.3f;
					}
					
					bool walkFlagR = (anim_Walk > 6f-speed && anim_Walk <= 6f);
					bool walkFlagL = (anim_Walk > 11f-speed && (anim_Walk <= 11f || anim_Walk <= 1f));
					bool walkFlag = (walkFlagR || walkFlagL);
					if(npc.ai[1] == 0 || npc.ai[1] > 15 || !walkFlag)
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
							Main.PlaySound(SoundLoader.customSoundType, (int)RLegPos[2].X, (int)RLegPos[2].Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/TorizoStep"));
							stepSoundPlayed = true;
						}
					}
					else if(walkFlagL)
					{
						if(!stepSoundPlayed)
						{
							Main.PlaySound(SoundLoader.customSoundType, (int)LLegPos[2].X, (int)LLegPos[2].Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/TorizoStep"));
							stepSoundPlayed = true;
						}
					}
					else
					{
						stepSoundPlayed = false;
					}
					
					// Jump
					if(Math.Abs(player.Center.X-npc.Center.X) < 500 && player.Center.Y < npc.Center.Y && (player.velocity.Y == 0f || npc.ai[2] >= 1) && npc.ai[1] == 0f)
					{
						npc.ai[2]++;
					}
					else
					{
						if(npc.ai[2] > 0)
						{
							npc.ai[2]--;
						}
					}
					
					if(npc.ai[2] > 60)
					{
						npc.TargetClosest(true);
						npc.netUpdate = true;
						npc.ai[0] = 2;
						npc.ai[1] = 0;
						npc.ai[2] = 0;
						npc.ai[3] = 0;
					}
					
					if((npc.direction == 1 && player.Center.X < npc.position.X+48) || (npc.direction == -1 && player.Center.X > npc.position.X-48))
					{
						if(npc.ai[1] == 0)
						{
							if(Main.rand.Next(2) == 0)
							{
								npc.ai[1] = -1;
							}
							else
							{
								npc.ai[1] = 1;
							}
						}
					}
					if(npc.ai[1] >= 1)
					{
						if(npc.ai[1] == 1)
						{
							anim_ClawTransition += 0.3f;
							if(anim_ClawTransition >= 1f)
							{
								anim_ClawTransition = 1f;
								
								anim_Claw += 0.15f;
								if((anim_Claw >= 3f && anim_Claw <= 4f) || (anim_Claw >= 7f && anim_Claw <= 8f))
								{
									anim_Claw += 0.05f;
									
									float dist = 54f;
									Vector2 clawPos = RArmPos[2];
									clawPos += Angle.AngleFlip(RArmRot[0]+1.57f,npc.direction).ToRotationVector2() * dist;
									if(anim_Claw >= 7f)
									{
										clawPos = LArmPos[2];
										clawPos += Angle.AngleFlip(LArmRot[0]+1.57f,npc.direction).ToRotationVector2() * dist;
									}
									int slash = Projectile.NewProjectile(clawPos.X,clawPos.Y,0f,0f,mod.ProjectileType("TorizoSwipe"),(int)((float)clawDamage/2f),8f);
									if(soundCounter <= 0)
									{
										Main.PlaySound(SoundLoader.customSoundType, (int)clawPos.X, (int)clawPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/TorizoSwipe"));
										soundCounter = 4;
									}
								}
								if(anim_Claw >= 8f)
								{
									npc.ai[1] = 2;
									anim_Claw = 8f;
								}
							}
						}
						if(npc.ai[1] == 2)
						{
							anim_ClawTransition -= 0.1f;
							if(anim_ClawTransition <= 0f)
							{
								npc.ai[1] = 3;
								anim_ClawTransition = 0f;
								anim_Claw = 1f;
							}
						}
						if(npc.ai[1] >= 3)
						{
							npc.ai[1]++;
							if(npc.ai[1] > 30)
							{
								npc.TargetClosest(true);
								npc.netUpdate = true;
								npc.ai[1] = 0f;
							}
						}
						if(soundCounter > 0)
						{
							soundCounter--;
						}
						npc.ai[2] = 0;
						npc.ai[3] = 0;
					}
					else
					{
						npc.ai[3]++;
						if(npc.ai[3] > 300)
						{
							npc.netUpdate = true;
							
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
								if(Math.Abs(player.Center.X-npc.Center.X) > 250)
								{
									num = Main.rand.Next((int)Math.Abs(player.Center.X-npc.Center.X)-250);
								}
								
								if(Head != null && Head.active && num < 50 && Math.Abs(player.Center.X-npc.Center.X) < 500)
								{
									npc.ai[0] = 3;
								}
								else
								{
									npc.ai[0] = 4;
								}
								
								npc.ai[1] = 0;
								npc.ai[2] = 0;
								npc.ai[3] = 0;
							}
						}
						
						if(npc.ai[1] <= -1)
						{
							npc.TargetClosest(true);
							npc.netUpdate = true;
							npc.ai[0] = 2;
							npc.ai[1] = 1;
							npc.ai[2] = 0;
							npc.ai[3] = 0;
						}
						soundCounter = 0;
					}
					
					float moveSpeed = speed_Walk[(int)Math.Min(anim_Walk,speed_Walk.Length)-1] * speed * fullScale.X;
					
					if(npc.direction == 1)
					{
						if (npc.velocity.X < 0f)
						{
							npc.velocity.X *= 0.98f;
						}
						npc.velocity.X += 0.5f;
					}
					else if(npc.direction == -1)
					{
						if (npc.velocity.X > 0f)
						{
							npc.velocity.X *= 0.98f;
						}
						npc.velocity.X -= 0.5f;
					}
					if (npc.velocity.X > moveSpeed)
					{
						npc.velocity.X = moveSpeed;
					}
					if (npc.velocity.X < -moveSpeed)
					{
						npc.velocity.X = -moveSpeed;
					}
					
					SetAnimation("walk", anim_Walk);
					SetAnimation("claw", anim_Claw, anim_ClawTransition);
					
					SetBodyOffset();
					
					HeadFrame = 3;
					spawnAlpha = 0f;
				}
				// jump
				if(npc.ai[0] == 2)
				{
					Player player = Main.player[npc.target];
					
					if(npc.ai[2] == 0)
					{
						if(npc.velocity.X != 0f)
						{
							npc.velocity.X *= 0.98f;
							npc.velocity.X -= 0.1f * Math.Sign(npc.velocity.X);
						}
						if(Math.Abs(npc.velocity.X) <= 0.1f)
						{
							npc.velocity.X = 0f;
						}
						
						anim_JumpTransition += 0.075f;
						if(anim_JumpTransition >= 1f)
						{
							anim_JumpTransition = 1f;
							npc.ai[2] = 1;
						}
					}
					if(npc.ai[2] >= 1 && npc.ai[2] <= 2)
					{
						if(npc.ai[2] == 1)
						{
							if(npc.ai[1] == 1)
							{
								npc.velocity.X = -7*npc.direction;
								npc.velocity.Y = -12f;
							}
							else
							{
								npc.velocity.X = MathHelper.Clamp((player.Center.X-npc.Center.X)*0.015f,-7,7);
								npc.velocity.Y = -12f;
							}
							npc.ai[2] = 2;
						}
						
						if(anim_Jump < 2f)
						{
							anim_Jump = Math.Min(anim_Jump+0.25f,2f);
						}
						else
						{
							if(npc.velocity.Y < 0f)
							{
								anim_Jump = Math.Min(anim_Jump+0.15f,3f);
							}
							else
							{
								anim_Jump = Math.Max(anim_Jump-0.05f,2f);
								anim_JumpTransition = Math.Max(anim_JumpTransition-0.025f,0.5f);
							}
						}
						
						if(npc.velocity.Y == 0f)
						{
							Main.PlaySound(SoundLoader.customSoundType, (int)RLegPos[2].X, (int)RLegPos[2].Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/TorizoStep"));
							npc.ai[2] = 3;
						}
						
						anim_Walk = 1f;
					}
					if(npc.ai[2] == 3)
					{
						npc.velocity.X = 0f;
						if(anim_Jump > 2f)
						{
							anim_Jump -= 0.1f;
						}
						anim_Jump = Math.Max(anim_Jump-0.2f,1f);
						if(anim_Jump <= 1f)
						{
							anim_JumpTransition -= 0.1f;
							if(anim_JumpTransition <= 0f)
							{
								anim_JumpTransition = 0f;
								npc.TargetClosest(true);
								npc.netUpdate = true;
								if(npc.ai[1] == 1)
								{
									if(Head != null && Head.active && Main.rand.Next(5) > 0)
									{
										npc.ai[0] = 3;
									}
									else
									{
										npc.ai[0] = 4;
									}
								}
								else
								{
									npc.ai[0] = 1;
								}
								npc.ai[1] = 0;
								npc.ai[2] = 0;
								npc.ai[3] = 0;
								anim_Jump = 1f;
								anim_Walk = 1f;
							}
						}
					}
					
					SetAnimation("walk", anim_Walk);
					SetAnimation("jump", anim_Jump, anim_JumpTransition);
					
					SetBodyOffset();
				}
				// bomb spew
				if(npc.ai[0] == 3)
				{
					if(npc.ai[1] == 0)
					{
						if(anim_BombTransition < 1f)
						{
							anim_BombTransition += 0.075f;
							HeadFrame = 4;
						}
						else
						{
							anim_BombTransition = 1f;
							anim_Bomb = Math.Min(anim_Bomb+0.075f,4f);
							
							if(HeadFrame < 8)
							{
								HeadFrameCounter++;
								if(HeadFrameCounter > 14)
								{
									HeadFrame++;
									HeadFrameCounter = 0;
								}
							}
							else
							{
								HeadFrame = 8;
								HeadFrameCounter = 0;
								
								npc.ai[2]++;
								if(npc.ai[2] > 5)
								{
									npc.ai[1] = 1;
									npc.ai[2] = 0;
								}
							}
						}
					}
					if(npc.ai[1] == 1)
					{
						bool headFlag = (Head != null && Head.active);
						if((npc.ai[2] == 10 || npc.ai[2] == 20 || npc.ai[2] == 30) && headFlag)
						{
							for(int i = 0; i < 3; i++)
							{
								Vector2 bombPos = HeadPos[0] + new Vector2(32f*npc.direction,-6f);
								Vector2 bombVel = new Vector2(3f*npc.direction,-3f);
								bombVel.X += (Main.rand.Next(50) - 25) * 0.05f;
								bombVel.Y += (Main.rand.Next(50) - 25) * 0.05f;
								
								NPC bomb = Main.npc[NPC.NewNPC((int)bombPos.X,(int)bombPos.Y,mod.NPCType("OrbBomb"),npc.whoAmI)];
								bomb.Center = bombPos;
								bomb.velocity = bombVel;
							}
						}
						if(npc.ai[2] == 10 && headFlag)
						{
							soundInstance = Main.PlaySound(SoundLoader.customSoundType, (int)Head.Center.X, (int)Head.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/TorizoHit"));
						}
						
						npc.ai[2]++;
						if(npc.ai[2] > 60 || !headFlag)
						{
							npc.ai[1] = 2;
							npc.ai[2] = 0;
							HeadFrame = 7;
						}
					}
					if(npc.ai[1] == 2)
					{
						anim_Bomb = Math.Max(anim_Bomb-0.075f,1f);
						if(HeadFrame > 4)
						{
							HeadFrameCounter++;
							if(HeadFrameCounter > 14)
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
							
							npc.TargetClosest(true);
							npc.netUpdate = true;
							npc.ai[0] = 1;
							npc.ai[1] = 0;
							npc.ai[2] = 0;
							npc.ai[3] = 0;
						}
					}
					
					if(npc.velocity.X != 0f)
					{
						npc.velocity.X *= 0.98f;
						npc.velocity.X -= 0.1f * Math.Sign(npc.velocity.X);
					}
					if(Math.Abs(npc.velocity.X) <= 0.1f)
					{
						npc.velocity.X = 0f;
					}
					
					SetAnimation("walk", anim_Walk);
					SetAnimation("bomb", anim_Bomb, anim_BombTransition);
					
					SetBodyOffset();
				}
				// ranged claw attack
				if(npc.ai[0] == 4)
				{
					Player player = Main.player[npc.target];
					
					if(npc.ai[1] == 0)
					{
						if(anim_ClawTransition < 1f)
						{
							anim_ClawTransition += 0.1f;
						}
						else
						{
							anim_ClawTransition = 1f;
							
							anim_Claw += 0.05f;
							if((anim_Claw >= 3f && anim_Claw < 4f) || (anim_Claw >= 7f && anim_Claw < 8f))
							{
								anim_Claw += 0.05f;
							}
							if(anim_Claw >= 9f)
							{
								if(Main.rand.Next(6) <= npc.ai[2])
								{
									npc.ai[1] = 1;
								}
								else
								{
									npc.ai[2]++;
								}
								anim_Claw = 1f;
							}
							
							if((anim_Claw > 3.5f && anim_Claw < 4f) || (anim_Claw > 7.5f && anim_Claw < 8f))
							{
								if(npc.ai[3] == 0)
								{
									Vector2 clawPos = BodyPos[0] + new Vector2(32*npc.direction,-10);
									if(Main.rand.Next(4) == 0 || (player.Center.Y > BodyPos[0].Y && Main.rand.Next(4) > 0))
									{
										clawPos.X = BodyPos[0].X + 26*npc.direction;
										clawPos.Y = BodyPos[0].Y + 70;
									}
									Vector2 clawVel = new Vector2(8f*npc.direction,0f);
									int slash = Projectile.NewProjectile(clawPos.X,clawPos.Y,clawVel.X,clawVel.Y,mod.ProjectileType("TorizoClawBeam"),(int)((float)clawDamage/2f),8f);
									Main.PlaySound(SoundLoader.customSoundType, (int)clawPos.X, (int)clawPos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/TorizoWave"));
									npc.ai[3] = 1;
								}
							}
							else
							{
								npc.ai[3] = 0;
							}
						}
					}
					if(npc.ai[1] == 1)
					{
						if(anim_ClawTransition > 0f)
						{
							anim_ClawTransition -= 0.1f;
						}
						else
						{
							anim_ClawTransition = 0f;
							
							npc.TargetClosest(true);
							npc.netUpdate = true;
							npc.ai[0] = 1;
							npc.ai[1] = 0;
							npc.ai[2] = 0;
							npc.ai[3] = 0;
							anim_Claw = 1f;
						}
					}
					
					if(npc.velocity.X != 0f)
					{
						npc.velocity.X *= 0.98f;
						npc.velocity.X -= 0.1f * Math.Sign(npc.velocity.X);
					}
					if(Math.Abs(npc.velocity.X) <= 0.1f)
					{
						npc.velocity.X = 0f;
					}
					
					SetAnimation("walk", anim_Walk);
					SetAnimation("claw", anim_Claw, anim_ClawTransition);
					
					SetBodyOffset();
				}
			}
			
			
			
			SetPositions();
			
			if(npc.life <= npc.lifeMax/2)
			{
				BodyFrame = 1;
				if(!chestExplosion)
				{
					Main.PlaySound(2,(int)BodyPos[0].X,(int)BodyPos[0].Y,14);
					
					Vector2 dustPos = BodyPos[0] + new Vector2(4,5);
					if(npc.direction == -1)
					{
						dustPos.X = BodyPos[0].X - 24;
					}
					for (int num70 = 0; num70 < 15; num70++)
					{
						Dust dust = Main.dust[Dust.NewDust(dustPos, 20, 24, 6, 0f, 0f, 100, default(Color), 5f)];
						dust.velocity *= 1.4f;
						dust.noGravity = true;
						
						dust = Main.dust[Dust.NewDust(dustPos, 20, 24, 30, 0f, 0f, 100, default(Color), 3f)];
						dust.velocity *= 1.4f;
						dust.noGravity = true;
					}
					chestExplosion = true;
				}
				
				if(dustCounter <= 0)
				{
					Vector2 gorePos = BodyPos[0] + new Vector2(6,13);
					if(npc.direction == -1)
					{
						gorePos.X = BodyPos[0].X - 16;
					}
					gorePos.X += Main.rand.Next(10);
					gorePos.Y += Main.rand.Next(10);
					Main.gore[Gore.NewGore(gorePos-new Vector2(8,0), default(Vector2), mod.GetGoreSlot("Gores/TorizoDroplet"), 1f)].velocity *= 0f;
					
					dustCounter = 30+Main.rand.Next(20);
				}
				else
				{
					dustCounter--;
				}
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
			
			if(npc.position.X < Main.player[npc.target].position.X && npc.position.X+npc.width > Main.player[npc.target].position.X+Main.player[npc.target].width && npc.position.Y+numH < Main.player[npc.target].position.Y+Main.player[npc.target].height - 16f)
			{
				grounded = Math.Min(grounded+1,15);
				npc.velocity.Y += 0.5f;
				if(npc.velocity.Y == 0f)
				{
					npc.velocity.Y = 0.1f;
				}
			}
			else
			{
				if(Collision.SolidCollision(new Vector2(npc.position.X,npc.position.Y+numH-16f), npc.width, 16))
				{
					if (npc.velocity.Y > -4f)
					{
						if (npc.velocity.Y > 0f)
						{
							npc.velocity.Y = 0f;
						}
						if (npc.velocity.Y > -0.2f)
						{
							npc.velocity.Y = npc.velocity.Y - 0.025f;
						}
						else
						{
							npc.velocity.Y = npc.velocity.Y - 0.2f;
						}
					}
				}
				else
				{
					npc.velocity.Y += 0.5f;
				}
				
				if(!Collision.SolidCollision(npc.position, npc.width, numH+1) && npc.velocity.Y == 0f)
				{
					npc.velocity.Y = 0.1f;
				}
				
				bool fall = false;
				if(npc.position.Y+numH < Main.player[npc.target].position.Y)// && npc.ai[0] <= 1 && (npc.ai[1] <= 1 || npc.ai[1] == 4))
				{
					fall = true;
				}
				Vector2 velocity = Collision.TileCollision(npc.position,new Vector2(0f,Math.Max(npc.velocity.Y,0f)),npc.width,numH,fall,fall);
				npc.velocity.Y = Math.Min(velocity.Y,npc.velocity.Y);
				if(npc.velocity.Y == 0f)
				{
					grounded = Math.Min(grounded+1,15);
				}
				else
				{
					grounded = Math.Max(grounded-1,0);
				}
			}
			if (npc.velocity.Y > 10f)
			{
				npc.velocity.Y = 10f;
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
			if(npc.direction == dir)
			{
				return;
			}
			else
			{
				npc.direction = dir;
				npc.netUpdate = true;
			}
		}
		
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			npc.spriteDirection = npc.direction;
			SpriteEffects effects = SpriteEffects.None;
			if (npc.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			
			Texture2D texHead = mod.GetTexture("NPCs/Torizo/TorizoHead"),
					texHead_Glow = mod.GetTexture("NPCs/Torizo/TorizoHead_EyeGlow"),
					texHead_Glow2 = mod.GetTexture("NPCs/Torizo/TorizoHead_EyeGlow2"),
					texBody = mod.GetTexture("NPCs/Torizo/TorizoBody"),
					texBody_Glow = mod.GetTexture("NPCs/Torizo/TorizoBody_Glow"),
					texShoulderF = mod.GetTexture("NPCs/Torizo/TorizoShoulder_Front"),
					texShoulderB = mod.GetTexture("NPCs/Torizo/TorizoShoulder_Back"),
					texArmF = mod.GetTexture("NPCs/Torizo/TorizoArm_Front"),
					texArmB = mod.GetTexture("NPCs/Torizo/TorizoArm_Back"),
					texHandF = mod.GetTexture("NPCs/Torizo/TorizoHand_Front"),
					texHandB = mod.GetTexture("NPCs/Torizo/TorizoHand_Back"),
					texThighF = mod.GetTexture("NPCs/Torizo/TorizoLegThigh_Front"),
					texThighB = mod.GetTexture("NPCs/Torizo/TorizoLegThigh_Back"),
					texCalfF = mod.GetTexture("NPCs/Torizo/TorizoLegCalf_Front"),
					texCalfB = mod.GetTexture("NPCs/Torizo/TorizoLegCalf_Back"),
					texFootF = mod.GetTexture("NPCs/Torizo/TorizoLegFoot_Front"),
					texFootB = mod.GetTexture("NPCs/Torizo/TorizoLegFoot_Back");
			
			Texture2D texSpawnHead = mod.GetTexture("NPCs/Torizo/Spawn/TorizoHead"),
					texSpawnHead_Glow = mod.GetTexture("NPCs/Torizo/Spawn/TorizoHead_EyeGlow"),
					texSpawnHead_Glow2 = mod.GetTexture("NPCs/Torizo/Spawn/TorizoHead_EyeGlow2"),
					texSpawnBody = mod.GetTexture("NPCs/Torizo/Spawn/TorizoBody"),
					texSpawnBody_Glow = mod.GetTexture("NPCs/Torizo/Spawn/TorizoBody_Glow"),
					texSpawnShoulderF = mod.GetTexture("NPCs/Torizo/Spawn/TorizoShoulder_Front"),
					texSpawnShoulderB = mod.GetTexture("NPCs/Torizo/Spawn/TorizoShoulder_Back"),
					texSpawnArmF = mod.GetTexture("NPCs/Torizo/Spawn/TorizoArm_Front"),
					texSpawnArmB = mod.GetTexture("NPCs/Torizo/Spawn/TorizoArm_Back"),
					texSpawnHandF = mod.GetTexture("NPCs/Torizo/Spawn/TorizoHand_Front"),
					texSpawnHandB = mod.GetTexture("NPCs/Torizo/Spawn/TorizoHand_Back"),
					texSpawnThighF = mod.GetTexture("NPCs/Torizo/Spawn/TorizoLegThigh_Front"),
					texSpawnThighB = mod.GetTexture("NPCs/Torizo/Spawn/TorizoLegThigh_Back"),
					texSpawnCalfF = mod.GetTexture("NPCs/Torizo/Spawn/TorizoLegCalf_Front"),
					texSpawnCalfB = mod.GetTexture("NPCs/Torizo/Spawn/TorizoLegCalf_Back"),
					texSpawnFootF = mod.GetTexture("NPCs/Torizo/Spawn/TorizoLegFoot_Front"),
					texSpawnFootB = mod.GetTexture("NPCs/Torizo/Spawn/TorizoLegFoot_Back");
			
			Color headColor = npc.GetAlpha(Lighting.GetColor((int)HeadPos[1].X / 16, (int)HeadPos[1].Y / 16)),
			bodyColor = npc.GetAlpha(Lighting.GetColor((int)BodyPos[1].X / 16, (int)BodyPos[1].Y / 16)),
			shoulderColorF = npc.GetAlpha(Lighting.GetColor((int)RArmPos[3].X / 16, (int)RArmPos[3].Y / 16)),
			shoulderColorB = npc.GetAlpha(Lighting.GetColor((int)LArmPos[3].X / 16, (int)LArmPos[3].Y / 16)),
			armColorF = npc.GetAlpha(Lighting.GetColor((int)RArmPos[4].X / 16, (int)RArmPos[4].Y / 16)),
			armColorB = npc.GetAlpha(Lighting.GetColor((int)LArmPos[4].X / 16, (int)LArmPos[4].Y / 16)),
			handColorF = npc.GetAlpha(Lighting.GetColor((int)RHand.Center.X / 16, (int)RHand.Center.Y / 16)),
			handColorB = npc.GetAlpha(Lighting.GetColor((int)LHand.Center.X / 16, (int)LHand.Center.Y / 16)),
			thighColorF = npc.GetAlpha(Lighting.GetColor((int)RLegPos[3].X / 16, (int)RLegPos[3].Y / 16)),
			thighColorB = npc.GetAlpha(Lighting.GetColor((int)LLegPos[3].X / 16, (int)LLegPos[3].Y / 16)),
			calfColorF = npc.GetAlpha(Lighting.GetColor((int)RLegPos[4].X / 16, (int)RLegPos[4].Y / 16)),
			calfColorB = npc.GetAlpha(Lighting.GetColor((int)LLegPos[4].X / 16, (int)LLegPos[4].Y / 16)),
			footColorF = npc.GetAlpha(Lighting.GetColor((int)RLegPos[2].X / 16, (int)RLegPos[2].Y / 16)),
			footColorB = npc.GetAlpha(Lighting.GetColor((int)LLegPos[2].X / 16, (int)LLegPos[2].Y / 16));
			
			Color glowColor = Color.White*glowAlpha;
			
			Color eyeGlowColor = Color.White;
			
			int fHandFrame = RHandFrame;
			int bHandFrame = LHandFrame;
			int fFootFrame = RFootFrame;
			int bFootFrame = LFootFrame;
			if(npc.spriteDirection == -1)
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
			DrawLimbTexture(npc, sb, texArmB, LArmPos[1], RArmPos[1], LArmRot[1], RArmRot[1], new Vector2(9,1), armColorB, armColorF, fullScale, effects);
			DrawLimbTexture(npc, sb, texSpawnArmB, LArmPos[1], RArmPos[1], LArmRot[1], RArmRot[1], new Vector2(9,1), armColorB*spawnAlpha, armColorF*spawnAlpha, fullScale, effects);
			
			// back hand
			DrawLimbTexture(npc, sb, texHandB, LArmPos[2], RArmPos[2], handRotB, handRotF, new Vector2(13,15), handColorB, handColorF, fullScale, effects, bHandFrame, 3);
			DrawLimbTexture(npc, sb, texSpawnHandB, LArmPos[2], RArmPos[2], handRotB, handRotF, new Vector2(13,15), handColorB*spawnAlpha, handColorF*spawnAlpha, fullScale, effects, bHandFrame, 3);
			
			// back shoulder
			DrawLimbTexture(npc, sb, texShoulderB, LArmPos[0], RArmPos[0], LArmRot[0], RArmRot[0], new Vector2(15,15), shoulderColorB, shoulderColorF, fullScale, effects);
			DrawLimbTexture(npc, sb, texSpawnShoulderB, LArmPos[0], RArmPos[0], LArmRot[0], RArmRot[0], new Vector2(15,15), shoulderColorB*spawnAlpha, shoulderColorF*spawnAlpha, fullScale, effects);
			
			// back calf
			DrawLimbTexture(npc, sb, texCalfB, LLegPos[1], RLegPos[1], LLegRot[1], RLegRot[1], new Vector2(17,1), calfColorB, calfColorF, fullScale, effects);
			DrawLimbTexture(npc, sb, texSpawnCalfB, LLegPos[1], RLegPos[1], LLegRot[1], RLegRot[1], new Vector2(17,1), calfColorB*spawnAlpha, calfColorF*spawnAlpha, fullScale, effects);
			
			// back foot
			DrawLimbTexture(npc, sb, texFootB, LLegPos[2], RLegPos[2], LLegRot[2], RLegRot[2], new Vector2(19,3), footColorB, footColorF, fullScale, effects, bFootFrame, 2);
			DrawLimbTexture(npc, sb, texSpawnFootB, LLegPos[2], RLegPos[2], LLegRot[2], RLegRot[2], new Vector2(19,3), footColorB*spawnAlpha, footColorF*spawnAlpha, fullScale, effects, bFootFrame, 2);
			
			// back thigh
			DrawLimbTexture(npc, sb, texThighB, LLegPos[0], RLegPos[0], LLegRot[0], RLegRot[0], new Vector2(13,5), thighColorB, thighColorF, fullScale, effects);
			DrawLimbTexture(npc, sb, texSpawnThighB, LLegPos[0], RLegPos[0], LLegRot[0], RLegRot[0], new Vector2(13,5), thighColorB*spawnAlpha, thighColorF*spawnAlpha, fullScale, effects);
			
			// body
			DrawLimbTexture(npc, sb, texBody, BodyPos[0], BodyPos[0], BodyRot, BodyRot, new Vector2(28,49), bodyColor, bodyColor, fullScale, effects, BodyFrame, 2);
			DrawLimbTexture(npc, sb, texBody_Glow, BodyPos[0], BodyPos[0], BodyRot, BodyRot, new Vector2(28,49), glowColor, glowColor, fullScale, effects, BodyFrame, 2);
			DrawLimbTexture(npc, sb, texSpawnBody, BodyPos[0], BodyPos[0], BodyRot, BodyRot, new Vector2(28,49), bodyColor*spawnAlpha, bodyColor*spawnAlpha, fullScale, effects, BodyFrame, 2);
			DrawLimbTexture(npc, sb, texSpawnBody_Glow, BodyPos[0], BodyPos[0], BodyRot, BodyRot, new Vector2(28,49), glowColor*spawnAlpha, glowColor*spawnAlpha, fullScale, effects, BodyFrame, 2);
			
			// head
			if(Head != null && Head.active)
			{
				DrawLimbTexture(npc, sb, texHead, HeadPos[0], HeadPos[0], headRot, headRot, new Vector2(32,38), headColor, headColor, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(npc, sb, texHead_Glow, HeadPos[0], HeadPos[0], headRot, headRot, new Vector2(32,38), glowColor, glowColor, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(npc, sb, texHead_Glow2, HeadPos[0], HeadPos[0], headRot, headRot, new Vector2(32,38), eyeGlowColor, eyeGlowColor, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(npc, sb, texSpawnHead, HeadPos[0], HeadPos[0], headRot, headRot, new Vector2(32,38), headColor*spawnAlpha, headColor*spawnAlpha, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(npc, sb, texSpawnHead_Glow, HeadPos[0], HeadPos[0], headRot, headRot, new Vector2(32,38), glowColor*spawnAlpha, glowColor*spawnAlpha, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(npc, sb, texSpawnHead_Glow2, HeadPos[0], HeadPos[0], headRot, headRot, new Vector2(32,38), eyeGlowColor*spawnAlpha, eyeGlowColor*spawnAlpha, fullScale, effects, HeadFrame, 9);
			}
			
			// front calf
			DrawLimbTexture(npc, sb, texCalfF, RLegPos[1], LLegPos[1], RLegRot[1], LLegRot[1], new Vector2(17,1), calfColorF, calfColorB, fullScale, effects);
			DrawLimbTexture(npc, sb, texSpawnCalfF, RLegPos[1], LLegPos[1], RLegRot[1], LLegRot[1], new Vector2(17,1), calfColorF*spawnAlpha, calfColorB*spawnAlpha, fullScale, effects);
			
			// front foot
			DrawLimbTexture(npc, sb, texFootF, RLegPos[2], LLegPos[2], RLegRot[2], LLegRot[2], new Vector2(19,3), footColorF, footColorB, fullScale, effects, fFootFrame, 2);
			DrawLimbTexture(npc, sb, texSpawnFootF, RLegPos[2], LLegPos[2], RLegRot[2], LLegRot[2], new Vector2(19,3), footColorF*spawnAlpha, footColorB*spawnAlpha, fullScale, effects, fFootFrame, 2);
			
			// front thigh
			DrawLimbTexture(npc, sb, texThighF, RLegPos[0], LLegPos[0], RLegRot[0], LLegRot[0], new Vector2(13,5), thighColorF, thighColorB, fullScale, effects);
			DrawLimbTexture(npc, sb, texSpawnThighF, RLegPos[0], LLegPos[0], RLegRot[0], LLegRot[0], new Vector2(13,5), thighColorF*spawnAlpha, thighColorB*spawnAlpha, fullScale, effects);
			
			// front arm
			DrawLimbTexture(npc, sb, texArmF, RArmPos[1], LArmPos[1], RArmRot[1], LArmRot[1], new Vector2(9,1), armColorF, armColorB, fullScale, effects);
			DrawLimbTexture(npc, sb, texSpawnArmF, RArmPos[1], LArmPos[1], RArmRot[1], LArmRot[1], new Vector2(9,1), armColorF*spawnAlpha, armColorB*spawnAlpha, fullScale, effects);
			
			// front hand
			DrawLimbTexture(npc, sb, texHandF, RArmPos[2], LArmPos[2], handRotF, handRotB, new Vector2(13,15), handColorF, handColorB, fullScale, effects, fHandFrame, 3);
			DrawLimbTexture(npc, sb, texSpawnHandF, RArmPos[2], LArmPos[2], handRotF, handRotB, new Vector2(13,15), handColorF*spawnAlpha, handColorB*spawnAlpha, fullScale, effects, fHandFrame, 3);
			
			// front shoulder
			DrawLimbTexture(npc, sb, texShoulderF, RArmPos[0], LArmPos[0], RArmRot[0], LArmRot[0], new Vector2(15,15), shoulderColorF, shoulderColorB, fullScale, effects);
			DrawLimbTexture(npc, sb, texSpawnShoulderF, RArmPos[0], LArmPos[0], RArmRot[0], LArmRot[0], new Vector2(15,15), shoulderColorF*spawnAlpha, shoulderColorB*spawnAlpha, fullScale, effects);
			
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