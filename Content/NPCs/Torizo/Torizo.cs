using System;
using System.Collections.Generic;
using System.IO;
using MetroidMod.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Torizo
{
	[AutoloadBossHead]
	public class Torizo : ModNPC
	{
		private bool expert = Main.expertMode;
		private bool master = Main.masterMode;
		private bool legend = Main.getGoodWorld;
		private bool classic = !Main.expertMode && !Main.masterMode && !Main.getGoodWorld;
		public override string BossHeadTexture => Mod.Name + "/Content/NPCs/Torizo/Torizo_Head_Boss";
		public override string Texture => Mod.Name + "/Content/NPCs/Torizo/TorizoBody";
		public string BestTexture => Mod.Name + "/Content/NPCs/Torizo/Torizo_BossLog";

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Torizo");
			Main.npcFrameCount[Type] = 2;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.BossBestiaryPriority.Add(Type);
			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()  //Alright so this here method thingy lets you tweak the bestiary display
			{
				CustomTexturePath = BestTexture, //the sprite the bestiary uses. The method doesn't like the filepath shenanigans so make a variable outside
				Position = new Vector2(-10f, 20f), // these two variables ONLY APPLY TO THE LIST TILES
				Scale = 1f,
				PortraitPositionXOverride = -15f, //these three variables ONLY APPLY TO THE BESTIARY PORTRAIT (the one right above the blurb)
				PortraitPositionYOverride = 5f,
				PortraitScale = 1f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);

			NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.IceFreeze>()] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Buffs.InstantFreeze>()] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 0;//56;
			NPC.height = 0;//94;
			NPC.scale = 1f;
			NPC.damage = 10;
			NPC.defense = 10;
			NPC.lifeMax = 2000;
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
			if (Main.netMode != NetmodeID.Server) { Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Torizo"); }
			//bossBag = mod.ItemType("TorizoBag");
			NPC.chaseable = false;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				new FlavorTextBestiaryInfoElement("Mods.MetroidMod.Bestiary.Torizo")
			});
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.lifeMax = (int)(NPC.lifeMax * balance); //*.7f
			NPC.damage = (int)(NPC.damage * 0.7f);
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Boss.TorizoBag>()));
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Tiles.TorizoRelic>()));
			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.TorizoClaws>(), 3));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.TorizoSpitter>(), 3));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Miscellaneous.EnergyShard>(), 1, 15, 36));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.ChoziteOre>(), 1, 30, 90));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.TorizoMusicBox>(), 6));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.TorizoMask>(), 8));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.TorizoTrophy>(), 11));

			npcLoot.Add(notExpertRule);
		}
		public override void OnKill()
		{
			MSystem.bossesDown |= MetroidBossDown.downedTorizo;
			if (!NPC.AnyNPCs(ModContent.NPCType<Town.ChozoGhost>()))
			{
				NPC.NewNPC(NPC.GetSource_Loot(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Town.ChozoGhost>());
			}
		}
		public override bool? CanBeHitByItem(Player player, Item item) => false;
		public override bool? CanBeHitByProjectile(Projectile projectile) => false;

		ReLogic.Utilities.SlotId soundInstance;
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Head != null && Head.active && (!SoundEngine.TryGetActiveSound(soundInstance, out ActiveSound result) || !result.IsPlaying) && Main.netMode != NetmodeID.Server)
			{
				soundInstance = SoundEngine.PlaySound(Sounds.NPCs.TorizoHit, Head.Center);
			}
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.5f;
			position = new Vector2(NPC.Center.X, NPC.position.Y + 168);
			if (NPC.life < NPC.lifeMax)
			{
				return true;
			}
			return null;
		}

		/*public override void BossHeadSlot(ref int index)
		{
			index = NPCHeadLoader.GetBossHeadSlot(MetroidMod.TorizoHead);
		}*/

		int _body, _head, _rHand, _lHand;
		NPC Body => Main.npc[_body];
		NPC RHand => Main.npc[_rHand];
		NPC LHand => Main.npc[_lHand];

		NPC Head
		{
			get {
				if (Main.npc[_head].type == ModContent.NPCType<Torizo_HitBox>() && Main.npc[_head].ai[1] == 0f)
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
				return Main.npc[_lArm[i]];
			return Main.npc[_rArm[i]];
		}
		NPC GetLeg(bool left, int i)
		{
			if (left)
				return Main.npc[_lLeg[i]];
			return Main.npc[_rLeg[i]];
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

		static Vector2[] DefaultBodyPos = { new Vector2(0, 0), new Vector2(0, 0) },
		DefaultHeadPos = { new Vector2(10, -19), new Vector2(2, -5) },
		DefaultRightArmPos = { new Vector2(1, -20), new Vector2(0, 38), new Vector2(0, 32), new Vector2(0, 28), new Vector2(0, 16) },
		DefaultLeftArmPos = { new Vector2(1, -20), new Vector2(0, 38), new Vector2(0, 32), new Vector2(0, 28), new Vector2(0, 16) },
		DefaultRightHandPos = { new Vector2(9, -7), new Vector2(15, 3), new Vector2(3, 13) },
		DefaultLeftHandPos = { new Vector2(9, -7), new Vector2(15, 3), new Vector2(3, 13) },
		DefaultRightLegPos = { new Vector2(-11, 38), new Vector2(0, 32), new Vector2(0, 34), new Vector2(0, 10), new Vector2(-2, 8) },
		DefaultLeftLegPos = { new Vector2(-11, 38), new Vector2(0, 32), new Vector2(0, 34), new Vector2(0, 10), new Vector2(-2, 8) };

		Vector2[] CurrentBodyPos = new Vector2[2],
		CurrentHeadPos = new Vector2[2],
		CurrentRightArmPos = new Vector2[5],
		CurrentLeftArmPos = new Vector2[5],
		CurrentRightHandPos = new Vector2[3],
		CurrentLeftHandPos = new Vector2[3],
		CurrentRightLegPos = new Vector2[5],
		CurrentLeftLegPos = new Vector2[5];

		Vector2 fullScale = new Vector2(1f, 1f);

		void SetPositions()
		{
			for (int i = 0; i < 5; i++)
			{
				CurrentRightArmPos[i] = DefaultRightArmPos[i];
				CurrentLeftArmPos[i] = DefaultLeftArmPos[i];
				CurrentRightLegPos[i] = DefaultRightLegPos[i];
				CurrentLeftLegPos[i] = DefaultLeftLegPos[i];
				if (i < 3)
				{
					CurrentRightHandPos[i] = DefaultRightHandPos[i];
					CurrentLeftHandPos[i] = DefaultLeftHandPos[i];
				}
				if (i < 2)
				{
					CurrentBodyPos[i] = DefaultBodyPos[i];
					CurrentHeadPos[i] = DefaultHeadPos[i];
				}
			}

			BodyOffsetRot = (float)Math.Atan2(CurrentBodyPos[1].Y, CurrentBodyPos[1].X);
			BodyDist = CurrentBodyPos[1].Length();

			for (int i = 0; i < 5; i++)
			{
				RArmOffsetRot[i] = (float)Math.Atan2(CurrentRightArmPos[i].Y, CurrentRightArmPos[i].X);
				RArmDist[i] = CurrentRightArmPos[i].Length();
				LArmOffsetRot[i] = (float)Math.Atan2(CurrentLeftArmPos[i].Y, CurrentLeftArmPos[i].X);
				LArmDist[i] = CurrentLeftArmPos[i].Length();

				RLegOffsetRot[i] = (float)Math.Atan2(CurrentRightLegPos[i].Y, CurrentRightLegPos[i].X);
				RLegDist[i] = CurrentRightLegPos[i].Length();
				LLegOffsetRot[i] = (float)Math.Atan2(CurrentLeftLegPos[i].Y, CurrentLeftLegPos[i].X);
				LLegDist[i] = CurrentLeftLegPos[i].Length();

				if (i < 3)
				{
					RHandOffsetRot[i] = (float)Math.Atan2(CurrentRightHandPos[i].Y, CurrentRightHandPos[i].X);
					RHandDist[i] = CurrentRightHandPos[i].Length();
					LHandOffsetRot[i] = (float)Math.Atan2(CurrentLeftHandPos[i].Y, CurrentLeftHandPos[i].X);
					LHandDist[i] = CurrentLeftHandPos[i].Length();
				}

				if (i < 2)
				{
					HeadOffsetRot[i] = (float)Math.Atan2(CurrentHeadPos[i].Y, CurrentHeadPos[i].X);
					HeadDist[i] = CurrentHeadPos[i].Length();
				}
			}

			BodyPos[0] = NPC.Center + (CurrentBodyPos[0] + BodyOffset);
			BodyPos[1] = BodyPos[0] + Angle.AngleFlip(BodyOffsetRot + BodyRot, NPC.direction).ToRotationVector2() * BodyDist * fullScale;

			RArmPos[0] = BodyPos[0] + Angle.AngleFlip(RArmOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2() * RArmDist[0] * fullScale;
			RArmPos[1] = RArmPos[0] + Angle.AngleFlip(RArmOffsetRot[1] + RArmRot[0], NPC.direction).ToRotationVector2() * RArmDist[1] * fullScale;
			RArmPos[2] = RArmPos[1] + Angle.AngleFlip(RArmOffsetRot[2] + RArmRot[1], NPC.direction).ToRotationVector2() * RArmDist[2] * fullScale;
			RArmPos[3] = RArmPos[0] + Angle.AngleFlip(RArmOffsetRot[3] + RArmRot[0], NPC.direction).ToRotationVector2() * RArmDist[3] * fullScale;
			RArmPos[4] = RArmPos[1] + Angle.AngleFlip(RArmOffsetRot[4] + RArmRot[1], NPC.direction).ToRotationVector2() * RArmDist[4] * fullScale;

			RHandPos[0] = RArmPos[2] + Angle.AngleFlip(RHandOffsetRot[0] + RArmRot[2] + (float)Angle.ConvertToRadians(60), NPC.direction).ToRotationVector2() * RHandDist[0] * fullScale;
			RHandPos[1] = RArmPos[2] + Angle.AngleFlip(RHandOffsetRot[1] + RArmRot[2] + (float)Angle.ConvertToRadians(30), NPC.direction).ToRotationVector2() * RHandDist[1] * fullScale;
			RHandPos[2] = RArmPos[2] + Angle.AngleFlip(RHandOffsetRot[2] + RArmRot[2], NPC.direction).ToRotationVector2() * RHandDist[2] * fullScale;

			LArmPos[0] = BodyPos[0] + Angle.AngleFlip(LArmOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2() * LArmDist[0] * fullScale;
			LArmPos[1] = LArmPos[0] + Angle.AngleFlip(LArmOffsetRot[1] + LArmRot[0], NPC.direction).ToRotationVector2() * LArmDist[1] * fullScale;
			LArmPos[2] = LArmPos[1] + Angle.AngleFlip(LArmOffsetRot[2] + LArmRot[1], NPC.direction).ToRotationVector2() * LArmDist[2] * fullScale;
			LArmPos[3] = LArmPos[0] + Angle.AngleFlip(LArmOffsetRot[3] + LArmRot[0], NPC.direction).ToRotationVector2() * LArmDist[3] * fullScale;
			LArmPos[4] = LArmPos[1] + Angle.AngleFlip(LArmOffsetRot[4] + LArmRot[1], NPC.direction).ToRotationVector2() * LArmDist[4] * fullScale;

			LHandPos[0] = LArmPos[2] + Angle.AngleFlip(LHandOffsetRot[0] + LArmRot[2] + (float)Angle.ConvertToRadians(60), NPC.direction).ToRotationVector2() * LHandDist[0] * fullScale;
			LHandPos[1] = LArmPos[2] + Angle.AngleFlip(LHandOffsetRot[1] + LArmRot[2] + (float)Angle.ConvertToRadians(30), NPC.direction).ToRotationVector2() * LHandDist[1] * fullScale;
			LHandPos[2] = LArmPos[2] + Angle.AngleFlip(LHandOffsetRot[2] + LArmRot[2], NPC.direction).ToRotationVector2() * LHandDist[2] * fullScale;

			RLegPos[0] = BodyPos[0] + Angle.AngleFlip(RLegOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2() * RLegDist[0] * fullScale;
			RLegPos[1] = RLegPos[0] + Angle.AngleFlip(RLegOffsetRot[1] + RLegRot[0], NPC.direction).ToRotationVector2() * RLegDist[1] * fullScale;
			RLegPos[2] = RLegPos[1] + Angle.AngleFlip(RLegOffsetRot[2] + RLegRot[1], NPC.direction).ToRotationVector2() * RLegDist[2] * fullScale;
			RLegPos[3] = RLegPos[0] + Angle.AngleFlip(RLegOffsetRot[3] + RLegRot[0], NPC.direction).ToRotationVector2() * RLegDist[3] * fullScale;
			RLegPos[4] = RLegPos[1] + Angle.AngleFlip(RLegOffsetRot[4] + RLegRot[1], NPC.direction).ToRotationVector2() * RLegDist[4] * fullScale;

			LLegPos[0] = BodyPos[0] + Angle.AngleFlip(LLegOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2() * LLegDist[0] * fullScale;
			LLegPos[1] = LLegPos[0] + Angle.AngleFlip(LLegOffsetRot[1] + LLegRot[0], NPC.direction).ToRotationVector2() * LLegDist[1] * fullScale;
			LLegPos[2] = LLegPos[1] + Angle.AngleFlip(LLegOffsetRot[2] + LLegRot[1], NPC.direction).ToRotationVector2() * LLegDist[2] * fullScale;
			LLegPos[3] = LLegPos[0] + Angle.AngleFlip(LLegOffsetRot[3] + LLegRot[0], NPC.direction).ToRotationVector2() * LLegDist[3] * fullScale;
			LLegPos[4] = LLegPos[1] + Angle.AngleFlip(LLegOffsetRot[4] + LLegRot[1], NPC.direction).ToRotationVector2() * LLegDist[4] * fullScale;

			HeadPos[0] = BodyPos[0] + Angle.AngleFlip(HeadOffsetRot[0] + BodyRot, NPC.direction).ToRotationVector2() * HeadDist[0] * fullScale;
			HeadPos[1] = HeadPos[0] + Angle.AngleFlip(HeadOffsetRot[1] + HeadRot, NPC.direction).ToRotationVector2() * HeadDist[1] * fullScale;
		}

		bool initialized = false;
		public override bool PreAI()
		{
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			if (!initialized && Main.netMode != NetmodeID.MultiplayerClient)
			{
				NPC.netUpdate = true;
				NPC.TargetClosest(true);

				Player player = Main.player[NPC.target];

				if (NPC.ai[1] == 0)
				{
					NPC.direction = 1;
					if (Main.rand.NextBool(2))
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
				_body = NPC.NewNPC(entitySource, (int)BodyPos[1].X, (int)BodyPos[1].Y, ModContent.NPCType<Torizo_HitBox>(), NPC.whoAmI, NPC.whoAmI, 1f);

				for (int i = 0; i < 2; i++)
				{
					_rArm[i] = NPC.NewNPC(entitySource, (int)RArmPos[i + 3].X, (int)RArmPos[i + 3].Y, ModContent.NPCType<Torizo_HitBox>(), NPC.whoAmI, NPC.whoAmI, 2 + i);
					_lArm[i] = NPC.NewNPC(entitySource, (int)LArmPos[i + 3].X, (int)LArmPos[i + 3].Y, ModContent.NPCType<Torizo_HitBox>(), NPC.whoAmI, NPC.whoAmI, 2 + i);
					_rLeg[i] = NPC.NewNPC(entitySource, (int)RLegPos[i + 3].X, (int)RLegPos[i + 3].Y, ModContent.NPCType<Torizo_HitBox>(), NPC.whoAmI, NPC.whoAmI, 5 + i);
					_lLeg[i] = NPC.NewNPC(entitySource, (int)LLegPos[i + 3].X, (int)LLegPos[i + 3].Y, ModContent.NPCType<Torizo_HitBox>(), NPC.whoAmI, NPC.whoAmI, 5 + i);
				}

				_rHand = NPC.NewNPC(entitySource, (int)RHandPos[0].X, (int)RHandPos[0].Y, ModContent.NPCType<Torizo_HitBox>(), NPC.whoAmI, NPC.whoAmI, 4f);
				_lHand = NPC.NewNPC(entitySource, (int)LHandPos[0].X, (int)LHandPos[0].Y, ModContent.NPCType<Torizo_HitBox>(), NPC.whoAmI, NPC.whoAmI, 4f);

				_head = NPC.NewNPC(entitySource, (int)HeadPos[1].X, (int)HeadPos[1].Y, ModContent.NPCType<Torizo_HitBox>(), NPC.whoAmI, NPC.whoAmI);

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

		int[] HandFrame_Spawn = { 0, 1, 2, 2, 2, 2, 2 };
		int[] LFootFrame_Spawn = { 0, 0, 0, 0, 1, 1, 0 };

		float[] speed_Spawn = { 25f, 8f, 9f, 10f, 23f, 10f, 0f };

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

		int[] RFootFrame_Walk = { 0, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
		int[] LFootFrame_Walk = { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 };

		float[] speed_Walk = { 8f, 12f, 13f, 17f, 16f, 8f, 12f, 13f, 17f, 16f };

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

		int[] FootFrame_Jump = { 0, 1, 1 };

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
		float[][] RArmAnim_Claw = new float[][]{
		new float[] {-22.5f,45f, 90f,-45f,-22.5f,-22.5f,-22.5f,-22.5f, -22.5f},
		new float[] { 22.5f,90f,180f,  0f, 22.5f, 22.5f, 22.5f, 22.5f,  22.5f},
		new float[] {    0f,45f,135f,-45f,    0f,    0f,    0f,    0f,     0f}};

		float[][] LArmAnim_Claw = new float[][]{
		new float[] {-22.5f,-22.5f,-22.5f,-22.5f,-22.5f,45f, 90f,-45f, -22.5f},
		new float[] { 22.5f, 22.5f, 22.5f, 22.5f, 22.5f,90f,180f,  0f,  22.5f},
		new float[] {    0f,    0f,    0f,    0f,    0f,45f,135f,-45f,     0f}};

		float[][] RArmAnim_Claw_Low = new float[][]{
		new float[] {-22.5f,45f, 60f,-45f,-22.5f,-22.5f,-22.5f,-22.5f, -22.5f},
		new float[] { 22.5f,90f,120f,  0f, 22.5f, 22.5f, 22.5f, 22.5f,  22.5f},
		new float[] {    0f,45f, 90f,-45f,    0f,    0f,    0f,    0f,     0f}};

		float[][] LArmAnim_Claw_Low = new float[][]{
		new float[] {-22.5f,-22.5f,-22.5f,-22.5f,-22.5f,45f, 60f,-45f, -22.5f},
		new float[] { 22.5f, 22.5f, 22.5f, 22.5f, 22.5f,90f,120f,  0f,  22.5f},
		new float[] {    0f,    0f,    0f,    0f,    0f,45f, 90f,-45f,     0f}};

		float anim_Claw = 1f;
		float anim_ClawTransition = 0f;

		// Ranged Claw Animation


		void SetAnimation(string type, float anim, float transition = 1f)
		{
			if (type == "spawn")
			{
				BodyRot = MathHelper.Lerp(BodyRot, 0f, transition);
				HeadRot = MathHelper.Lerp(HeadRot, 0f, transition);
				for (int i = 0; i < 3; i++)
				{
					RLegRot[i] = MathHelper.Lerp(RLegRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, RLegAnim_Spawn[i], anim)), transition);
					LLegRot[i] = MathHelper.Lerp(LLegRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, LLegAnim_Spawn[i], anim)), transition);

					RArmRot[i] = MathHelper.Lerp(RArmRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, RArmAnim_Spawn[i], anim)), transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, LArmAnim_Spawn[i], anim)), transition);
				}
				if (transition > 0.5f)
				{
					LFootFrame = LFootFrame_Spawn[(int)Math.Min(anim, LFootFrame_Spawn.Length) - 1];
					RHandFrame = HandFrame_Spawn[(int)Math.Min(anim, HandFrame_Spawn.Length) - 1];
					LHandFrame = HandFrame_Spawn[(int)Math.Min(anim, HandFrame_Spawn.Length) - 1];
				}
			}
			if (type == "walk")
			{
				BodyRot = MathHelper.Lerp(BodyRot, 0f, transition);
				HeadRot = MathHelper.Lerp(HeadRot, 0f, transition);
				for (int i = 0; i < 3; i++)
				{
					RLegRot[i] = MathHelper.Lerp(RLegRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, RLegAnim_Walk[i], anim)), transition);
					LLegRot[i] = MathHelper.Lerp(LLegRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, LLegAnim_Walk[i], anim)), transition);

					RArmRot[i] = MathHelper.Lerp(RArmRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, RArmAnim_Walk[i], anim)), transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, LArmAnim_Walk[i], anim)), transition);
				}
				if (transition > 0.5f)
				{
					RFootFrame = RFootFrame_Walk[(int)Math.Min(anim, RFootFrame_Walk.Length) - 1];
					LFootFrame = LFootFrame_Walk[(int)Math.Min(anim, LFootFrame_Walk.Length) - 1];
					RHandFrame = 2;
					LHandFrame = 2;
				}
			}
			if (type == "jump")
			{
				BodyRot = MathHelper.Lerp(BodyRot, 0f, transition);
				HeadRot = MathHelper.Lerp(HeadRot, 0f, transition);
				for (int i = 0; i < 3; i++)
				{
					RLegRot[i] = MathHelper.Lerp(RLegRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, RLegAnim_Jump[i], anim)), transition);
					LLegRot[i] = MathHelper.Lerp(LLegRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, LLegAnim_Jump[i], anim)), transition);

					RArmRot[i] = MathHelper.Lerp(RArmRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, RArmAnim_Jump[i], anim)), transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, LArmAnim_Jump[i], anim)), transition);
				}
				if (transition >= 0.5f)
				{
					RFootFrame = FootFrame_Jump[(int)Math.Min(anim, FootFrame_Jump.Length) - 1];
					LFootFrame = FootFrame_Jump[(int)Math.Min(anim, FootFrame_Jump.Length) - 1];
					RHandFrame = 2;
					LHandFrame = 2;
				}
			}
			if (type == "bomb")
			{
				BodyRot = MathHelper.Lerp(BodyRot, 0f, transition);
				HeadRot = MathHelper.Lerp(HeadRot, -(float)Angle.ConvertToRadians(45), transition);
				for (int i = 0; i < 3; i++)
				{
					RArmRot[i] = MathHelper.Lerp(RArmRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, RArmAnim_Bomb[i], anim)), transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, LArmAnim_Bomb[i], anim)), transition);
				}
			}
			if (type == "claw")
			{
				BodyRot = MathHelper.Lerp(BodyRot, 0f, transition);
				HeadRot = MathHelper.Lerp(HeadRot, 0f, transition);
				for (int i = 0; i < 3; i++)
				{
					RArmRot[i] = MathHelper.Lerp(RArmRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, RArmAnim_Claw[i], anim)), transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, LArmAnim_Claw[i], anim)), transition);
				}
			}
			if (type == "claw low")
			{
				BodyRot = MathHelper.Lerp(BodyRot, 0f, transition);
				HeadRot = MathHelper.Lerp(HeadRot, 0f, transition);
				for (int i = 0; i < 3; i++)
				{
					RArmRot[i] = MathHelper.Lerp(RArmRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, RArmAnim_Claw_Low[i], anim)), transition);
					LArmRot[i] = MathHelper.Lerp(LArmRot[i], -(float)Angle.ConvertToRadians((double)Angle.LerpArray(0f, LArmAnim_Claw_Low[i], anim)), transition);
				}
			}
		}
		void SetBodyOffset(float hOffset = 0f)
		{
			BodyPos[0] = NPC.Center + (CurrentBodyPos[0] + BodyOffset);
			RLegPos[2] = RLegPos[1] + Angle.AngleFlip(RLegOffsetRot[2] + RLegRot[1], NPC.direction).ToRotationVector2() * RLegDist[2] * fullScale;
			LLegPos[2] = LLegPos[1] + Angle.AngleFlip(LLegOffsetRot[2] + LLegRot[1], NPC.direction).ToRotationVector2() * LLegDist[2] * fullScale;
			if (RLegPos[2].Y >= LLegPos[2].Y)
			{
				BodyOffset.Y = 117f - ((RLegPos[2].Y + (13f * fullScale.Y)) - BodyPos[0].Y) + hOffset;
			}
			else
			{
				BodyOffset.Y = 117f - ((LLegPos[2].Y + (13f * fullScale.Y)) - BodyPos[0].Y) + hOffset;
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
					if (NPC.alpha >= 255)
					{
						NPC.active = false;
					}
				}
			}
			else
			{
				NPC.alpha = Math.Max(NPC.alpha - 10, 0);

				// spawn
				if (NPC.ai[0] == 0)
				{
					NPC.dontTakeDamage = true;

					float speed = 0f;

					// drop from sky
					if (NPC.ai[1] == 0)
					{
						//if(NPC.velocity.Y != 0)
						if (NPC.velocity.Y > 0 && NPC.ai[2] == 0)
						{
							anim_Spawn = 4f;
						}
						else
						{
							NPC.ai[2] = 1;
							if (anim_Spawn > 1f)
							{
								speed = -0.2f;
								anim_Spawn = Math.Max(anim_Spawn + speed, 1f);
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
					if (NPC.ai[1] == 1)
					{
						if (anim_Spawn < 7f)
						{
							NPC.ai[2]++;
							if (NPC.ai[2] > 90)
							{
								if (NPC.ai[2] % 3 == 0)
								{
									if (HeadFrame < 3)
									{
										HeadFrame++;
									}
								}
								if (NPC.ai[2] == 120 || NPC.ai[2] == 134)
								{
									HeadFrame = 0;
								}
							}
							if (NPC.ai[2] > 180)
							{
								speed = 0.1f;
								anim_Spawn = Math.Min(anim_Spawn + speed, 7f);
							}
						}
						else
						{
							anim_Spawn = 7f;
							spawnAlpha -= 0.015f;
							if (spawnAlpha <= 0f)
							{
								NPC.TargetClosest(true);
								NPC.netUpdate = true;
								NPC.ai[0] = 1;
								NPC.ai[1] = 0;
								NPC.ai[2] = 0;
							}
						}
					}

					float moveSpeed = speed_Spawn[(int)Math.Min(anim_Spawn, speed_Spawn.Length) - 1] * Math.Abs(speed) * fullScale.X;

					if (NPC.direction == Math.Sign(speed))
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X *= 0.98f;
						}
						NPC.velocity.X += 0.5f;
					}
					else if (NPC.direction == -Math.Sign(speed))
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
				if (NPC.ai[0] == 1)
				{
					Player player = Main.player[NPC.target];

					float speed = !legend?(expert? 0.20f : master? 0.25f : 0.15f) : 0.30f; //Dr zoooom
					if (Head == null || !Head.active)
					{
						NPC.defense = 10;
						speed *= 1.3f;
					}
					else
					{
						NPC.defense = !legend?( expert ? 17 : master ? 19 : 15) : 21; //DR killing head lowers defense but goes faster
					}

					bool walkFlagR = (anim_Walk > 6f - speed && anim_Walk <= 6f);
					bool walkFlagL = (anim_Walk > 11f - speed && (anim_Walk <= 11f || anim_Walk <= 1f));
					bool walkFlag = (walkFlagR || walkFlagL);
					if (NPC.ai[1] == 0 || NPC.ai[1] > 15 || !walkFlag)
					{
						anim_Walk += speed;
						if (anim_Walk >= 11)
						{
							anim_Walk = 1f;
						}
					}
					else
					{
						speed = 0;
						if (walkFlagR)
						{
							anim_Walk = 6f;
						}
						else if (walkFlagL)
						{
							anim_Walk = 1f;
						}
					}

					if (walkFlagR)
					{
						if (!stepSoundPlayed)
						{
							SoundEngine.PlaySound(Sounds.NPCs.TorizoStep, RLegPos[2]);
							stepSoundPlayed = true;
						}
					}
					else if (walkFlagL)
					{
						if (!stepSoundPlayed)
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
					if (Math.Abs(player.Center.X - NPC.Center.X) < 500 && player.Center.Y < NPC.Center.Y /*&& ((player.velocity.Y == 0f && classic) || NPC.ai[2] >= 1)*/ && NPC.ai[1] == 0f)
					{
						NPC.ai[2]++;
					}
					else
					{
						if (NPC.ai[2] > 0 && NPC.ai[1] == 0f)
						{
							NPC.ai[2]--;
						}
					}

					if (!legend ? NPC.ai[2] > 60 : NPC.ai[2] > 20)
					{
						NPC.TargetClosest(true);
						NPC.netUpdate = true;
						NPC.ai[0] = 2;
						NPC.ai[1] = 0;
						NPC.ai[2] = 0;
						NPC.ai[3] = 0;
					}

					if (player.position.Y + player.height > NPC.position.Y - 50)
					{
						if ((NPC.direction == 1 && player.Center.X < NPC.position.X + 48) || (NPC.direction == -1 && player.Center.X > NPC.position.X - 48))
						{
							if (NPC.ai[1] == 0)
							{
								NPC.netUpdate = true;
								if (Main.rand.NextBool(2))
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
						if ((NPC.direction == 1 && player.Center.X < NPC.position.X - 28) || (NPC.direction == -1 && player.Center.X > NPC.position.X + 28))
						{
							ChangeDir(-NPC.direction);
						}
					}
					if (NPC.ai[1] >= 1)
					{
						if (NPC.ai[1] == 1)
						{
							anim_ClawTransition += 0.3f;
							if (anim_ClawTransition >= 1f)
							{
								anim_ClawTransition = 1f;

								anim_Claw += 0.15f;
								if ((anim_Claw >= 3f && anim_Claw <= 4f) || (anim_Claw >= 7f && anim_Claw <= 8f))
								{
									anim_Claw += 0.05f;

									float dist = 54f;
									Vector2 clawPos = RArmPos[2];
									clawPos += Angle.AngleFlip(RArmRot[0] + MathHelper.PiOver2, NPC.direction).ToRotationVector2() * dist;
									if (anim_Claw >= 7f)
									{
										clawPos = LArmPos[2];
										clawPos += Angle.AngleFlip(LArmRot[0] + MathHelper.PiOver2, NPC.direction).ToRotationVector2() * dist;
									}
									var entitySource = NPC.GetSource_FromAI();
									int slash = Projectile.NewProjectile(entitySource, clawPos.X, clawPos.Y, 0f, 0f, ModContent.ProjectileType<Projectiles.Boss.TorizoSwipe>(), (int)((float)clawDamage / 2f), 8f);
									if (soundCounter <= 0)
									{
										SoundEngine.PlaySound(Sounds.NPCs.TorizoSwipe, clawPos);
										soundCounter = 4;
									}
								}
								if (anim_Claw >= 8f)
								{
									NPC.ai[1] = 2;
									anim_Claw = 8f;
								}
							}
						}
						if (NPC.ai[1] == 2)
						{
							anim_ClawTransition -= 0.1f;
							if (anim_ClawTransition <= 0f)
							{
								NPC.ai[1] = 3;
								anim_ClawTransition = 0f;
								anim_Claw = 1f;
							}
						}
						if (NPC.ai[1] >= 3)
						{
							NPC.ai[1]++;
							if (NPC.ai[1] > 30)
							{
								NPC.TargetClosest(true);
								NPC.netUpdate = true;
								NPC.ai[1] = 0f;
							}
						}
						if (soundCounter > 0)
						{
							soundCounter--;
						}
						//NPC.ai[2] = 0;
						NPC.ai[3] = 0;
					}
					else
					{
						NPC.ai[3]++;
						if (!legend ? NPC.ai[3] >  300 : NPC.ai[3] >100)
						{
							NPC.netUpdate = true;

							if (walkFlag)
							{
								if (walkFlagR)
								{
									anim_Walk = 6f;
								}
								else if (walkFlagL)
								{
									anim_Walk = 1f;
								}

								int num = 0;
								if (Math.Abs(player.Center.X - NPC.Center.X) > 250)
								{
									num = Main.rand.Next((int)Math.Abs(player.Center.X - NPC.Center.X) - 250);
								}

								if (Head != null && Head.active && num < 50 && Math.Abs(player.Center.X - NPC.Center.X) < 500)
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

						if (NPC.ai[1] <= -1)
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

					float moveSpeed = speed_Walk[(int)Math.Min(anim_Walk, speed_Walk.Length) - 1] * speed * fullScale.X;

					if (NPC.direction == 1)
					{
						if (NPC.velocity.X < 0f)
						{
							NPC.velocity.X *= 0.98f;
						}
						NPC.velocity.X += 0.5f;
					}
					else if (NPC.direction == -1)
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
				if (NPC.ai[0] == 2)
				{
					Player player = Main.player[NPC.target];

					if (NPC.ai[2] == 0)
					{
						if (NPC.velocity.X != 0f)
						{
							NPC.velocity.X *= 0.98f;
							NPC.velocity.X -= 0.1f * Math.Sign(NPC.velocity.X);
						}
						if (Math.Abs(NPC.velocity.X) <= 0.1f)
						{
							NPC.velocity.X = 0f;
						}

						anim_JumpTransition += !legend? 0.075f : 0.25f;
						if (anim_JumpTransition >= 1f)
						{
							anim_JumpTransition = 1f;
							NPC.ai[2] = 1;
						}
					}
					if (NPC.ai[2] >= 1 && NPC.ai[2] <= 2)
					{
						if (NPC.ai[2] == 1)
						{
							if (NPC.ai[1] == 1)//jump back
							{
								NPC.velocity.X = -7 * NPC.direction;
								NPC.velocity.Y = -12f;
							}
							else //jump up
							{
								NPC.velocity.X = MathHelper.Clamp((player.Center.X - NPC.Center.X) * 0.015f, -7, 7);
								NPC.velocity.Y = !legend?( expert ? -20f : master ? -24f : -16f): -28f;
							}
							NPC.ai[2] = 2;
						}

						if (anim_Jump < 2f)
						{
							anim_Jump = Math.Min(anim_Jump += !legend? 0.25f : 0.75f, 2f);
						}
						else
						{
							if (NPC.velocity.Y < 0f)
							{
								anim_Jump = Math.Min(anim_Jump += !legend? 0.15f :0.45f, 3f);
							}
							else
							{
								anim_Jump = Math.Max(anim_Jump -= !legend? 0.05f : .15f, 2f);
								anim_JumpTransition = Math.Max(anim_JumpTransition - 0.025f, 0.5f);
							}
						}

						if (NPC.velocity.Y == 0f)
						{
							SoundEngine.PlaySound(Sounds.NPCs.TorizoStep, RLegPos[2]);
							NPC.ai[2] = 3;
						}

						anim_Walk = 1f;
					}
					if (NPC.ai[2] == 3)
					{
						NPC.velocity.X = 0f;
						if (anim_Jump > 2f)
						{
							anim_Jump -= !legend ? 0.1f : 0.3f;
						}
						anim_Jump = Math.Max(anim_Jump - 0.2f, 1f);
						if (anim_Jump <= 1f)
						{
							anim_JumpTransition -= 0.1f;
							if (anim_JumpTransition <= 0f)
							{
								anim_JumpTransition = 0f;
								NPC.TargetClosest(true);
								NPC.netUpdate = true;
								if (NPC.ai[1] == 1)
								{
									if (Head != null && Head.active && Main.rand.Next(5) > 0)
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
				if (NPC.ai[0] == 3)
				{
					if (NPC.ai[1] == 0)
					{
						if (anim_BombTransition < 1f)
						{
							anim_BombTransition += !legend? 0.075f : 0.250f;
							HeadFrame = 4;
							if(anim_BombTransition >= 0.5f)
							{
								HeadFrame = 5;
							}
						}
						else
						{
							anim_BombTransition = 1f;
							anim_Bomb = Math.Min(anim_Bomb += !legend ? 0.075f : 0.250f, 4f);

							if (HeadFrame < 8)
							{
								HeadFrameCounter+= !legend? 1 : 2;
								if (HeadFrameCounter > 14)
								{
									HeadFrame += !legend? 1 : 2;
									HeadFrameCounter = 0;
								}
							}
							else
							{
								HeadFrame = 8;
								HeadFrameCounter = 0;

								NPC.ai[2] += !legend ? 1f :2.5f ;
								if (NPC.ai[2] > 5)
								{
									NPC.ai[1] = 1;
									NPC.ai[2] = 0;
								}
							}
						}
					}
					if (NPC.ai[1] == 1)
					{
						bool headFlag = (Head != null && Head.active);
						if ((NPC.ai[2] == 10 || NPC.ai[2] == 20 || NPC.ai[2] == 30) && headFlag)
						{
							var entitySource = NPC.GetSource_FromAI();
							for (int i = 0; i < (!legend ? (expert ? 4 : master ? 5 : 3) : 6); i++) //DR more bombs whee
							{
								Vector2 bombPos = HeadPos[0] + new Vector2(32f * NPC.direction, -6f);
								Vector2 bombVel = new Vector2(3f * NPC.direction, -3f);
								bombVel.X += (Main.rand.Next(50) - 25) * 0.05f;
								bombVel.Y += (Main.rand.Next(50) - 25) * 0.05f;

								NPC bomb = Main.npc[NPC.NewNPC(entitySource, (int)bombPos.X, (int)bombPos.Y, ModContent.NPCType<OrbBomb>(), NPC.whoAmI)];
								bomb.Center = bombPos;
								bomb.velocity = bombVel;
								bomb.netUpdate = true;
							}
						}
						if (NPC.ai[2] == 10 && headFlag)
						{
							soundInstance = SoundEngine.PlaySound(Sounds.NPCs.TorizoHit, Head.Center);
						}

						NPC.ai[2]++;
						if (!legend ? NPC.ai[2] > 60 : NPC.ai[2]> 20 || !headFlag)
						{
							NPC.ai[1] = 2;
							NPC.ai[2] = 0;
							HeadFrame = 7;
						}
					}
					if (NPC.ai[1] == 2)
					{
						anim_Bomb = Math.Max(anim_Bomb -= !legend ? 0.075f : 0.250f, 1f);
						if (HeadFrame > 4)
						{
							HeadFrameCounter+= !legend ? 1 : 3 ;
							if (HeadFrameCounter > 14)
							{
								HeadFrame--;
								HeadFrameCounter = 0;
							}
						}
						else if (anim_BombTransition > 0f)
						{
							anim_BombTransition -= !legend ? 0.075f : 0.250f;
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

					if (NPC.velocity.X != 0f)
					{
						NPC.velocity.X *= 0.98f;
						NPC.velocity.X -= 0.1f * Math.Sign(NPC.velocity.X);
					}
					if (Math.Abs(NPC.velocity.X) <= 0.1f)
					{
						NPC.velocity.X = 0f;
					}

					SetAnimation("walk", anim_Walk);
					SetAnimation("bomb", anim_Bomb, anim_BombTransition);

					SetBodyOffset();
				}
				// ranged claw attack
				if (NPC.ai[0] == 4)
				{
					Player player = Main.player[NPC.target];

					if (NPC.ai[1] == 0)
					{
						if (anim_ClawTransition < 1f)
						{
							anim_ClawTransition += 0.1f;
						}
						else
						{
							anim_ClawTransition = 1f;

							anim_Claw += !legend? 0.05f : 0.15f;
							if ((anim_Claw >= 3f && anim_Claw < 4f) || (anim_Claw >= 7f && anim_Claw < 8f))
							{
								anim_Claw += !legend ? 0.05f : 0.15f;
							}
							if (anim_Claw >= 9f)
							{
								NPC.netUpdate = true;
								if (Main.rand.Next(6) <= NPC.ai[2])
								{
									NPC.ai[1] = 1;
								}
								else
								{
									NPC.ai[2]++;
								}
								anim_Claw = 1f;
							}

							if ((anim_Claw > 3.5f && anim_Claw < 4f) || (anim_Claw > 7.5f && anim_Claw < 8f))
							{
								if (NPC.ai[3] > 0)
								{
									Vector2 clawPos = BodyPos[0] + new Vector2(32 * NPC.direction, -10);
									if (NPC.ai[3] == 1)
									{
										clawPos.X = BodyPos[0].X + 26 * NPC.direction;
										clawPos.Y = BodyPos[0].Y + 70;
									}
									Vector2 clawVel = new Vector2(8f * NPC.direction, 0f);
									var entitySource = NPC.GetSource_FromAI();
									int slash = Projectile.NewProjectile(entitySource, clawPos.X, clawPos.Y, clawVel.X, clawVel.Y, ModContent.ProjectileType<Projectiles.Boss.TorizoClawBeam>(), (int)((float)clawDamage / 2f), 8f);
									Main.projectile[slash].netUpdate = true;
									SoundEngine.PlaySound(Sounds.NPCs.TorizoWave, clawPos);
									NPC.ai[3] = 0;
									NPC.netUpdate = true;
								}
							}
							else if (anim_Claw <= 2f || (anim_Claw >= 5f && anim_Claw <= 6f))
							{
								if (NPC.ai[3] == 0)
								{
									if (Main.rand.NextBool(4) || (player.Center.Y > BodyPos[0].Y && Main.rand.Next(4) > 0))
									{
										NPC.ai[3] = 1;
									}
									else
									{
										NPC.ai[3] = 2;
									}
									NPC.netUpdate = true;
								}
							}
						}
					}
					if (NPC.ai[1] == 1)
					{
						if (anim_ClawTransition > 0f)
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

					if (NPC.velocity.X != 0f)
					{
						NPC.velocity.X *= 0.98f;
						NPC.velocity.X -= 0.1f * Math.Sign(NPC.velocity.X);
					}
					if (Math.Abs(NPC.velocity.X) <= 0.1f)
					{
						NPC.velocity.X = 0f;
					}

					SetAnimation("walk", anim_Walk);

					string claw = "claw";
					if (NPC.ai[3] == 1)
					{
						claw = "claw low";
					}
					SetAnimation(claw, anim_Claw, anim_ClawTransition);

					SetBodyOffset();
				}
			}



			SetPositions();

			if (NPC.life <= NPC.lifeMax / 2)
			{
				BodyFrame = 1;
				if (!chestExplosion)
				{
					SoundEngine.PlaySound(SoundID.Item14, BodyPos[0]);

					Vector2 dustPos = BodyPos[0] + new Vector2(4, 5);
					if (NPC.direction == -1)
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

				if (Main.netMode != NetmodeID.Server)
				{
					if (dustCounter <= 0)
					{
						Vector2 gorePos = BodyPos[0] + new Vector2(6, 13);
						if (NPC.direction == -1)
						{
							gorePos.X = BodyPos[0].X - 16;
						}
						gorePos.X += Main.rand.Next(10);
						gorePos.Y += Main.rand.Next(10);
						Main.gore[Gore.NewGore(NPC.GetSource_FromAI(), gorePos - new Vector2(8, 0), default(Vector2), Mod.Find<ModGore>("TorizoDroplet").Type, 1f)].velocity *= 0f;

						dustCounter = 30 + Main.rand.Next(20);
					}
					else
					{
						dustCounter--;
					}
				}
			}

			glowAlpha += 0.017f * glowNum;
			if (glowAlpha >= 1f)
			{
				glowAlpha = 1f;
				glowNum = -1;
			}
			if (glowAlpha <= 0f)
			{
				glowAlpha = 0f;
				glowNum = 1;
			}

			if (NPC.ai[0] == 2 && NPC.ai[1] == 1 && Body != null && Body.active)
			{
				//if((NPC.direction == 1 && NPC.velocity.X < 0) || (NPC.direction == -1 && NPC.velocity.X > 0))
				//{
				Vector2 velocity = Collision.TileCollision(NPC.position - new Vector2(28, 47), NPC.velocity, 56, 47 + numH);
				NPC.velocity.X = velocity.X;
				//}
			}

			//if(((NPC.Center.X-100 < Main.player[NPC.target].Center.X && NPC.Center.X+100 > Main.player[NPC.target].Center.X) || (NPC.ai[0] == 0 && NPC.ai[1] == 0)) && NPC.position.Y+numH < Main.player[NPC.target].position.Y+Main.player[NPC.target].height - 16f)
			if (NPC.position.Y + numH < Main.player[NPC.target].position.Y + Main.player[NPC.target].height - 16f && NPC.ai[0] <= 2 && (NPC.ai[0] != 1 || NPC.ai[1] == 0))
			{
				NPC.velocity.Y += 0.5f;
				if (NPC.velocity.Y == 0f)
				{
					NPC.velocity.Y = 0.1f;
				}
			}
			else
			{
				if (Collision.SolidCollision(new Vector2(NPC.position.X, NPC.position.Y + numH - 16f), NPC.width, 16) && NPC.position.Y + numH > Main.player[NPC.target].position.Y + Main.player[NPC.target].height)
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

				if (!Collision.SolidCollision(NPC.position, NPC.width, numH + 1) && NPC.velocity.Y == 0f)
				{
					NPC.velocity.Y = 0.1f;
				}

				bool fall = false;
				if (NPC.position.Y + numH < Main.player[NPC.target].position.Y && NPC.ai[0] <= 2 && (NPC.ai[0] != 1 || NPC.ai[1] == 0))
				{
					fall = true;
				}
				Vector2 velocity = Collision.TileCollision(NPC.position, new Vector2(0f, Math.Max(NPC.velocity.Y, 0f)), NPC.width, numH, fall, fall);
				NPC.velocity.Y = Math.Min(velocity.Y, NPC.velocity.Y);
			}
			if (NPC.velocity.Y > 10f)
			{
				NPC.velocity.Y = 10f;
			}

			if (Body != null && Body.active)
			{
				Body.Center = BodyPos[1];
			}
			if (Head != null && Head.active)
			{
				Head.Center = HeadPos[1];
			}
			for (int i = 0; i < 2; i++)
			{
				NPC RArm = GetArm(false, i),
					LArm = GetArm(true, i),
					RLeg = GetLeg(false, i),
					LLeg = GetLeg(true, i);
				if (RArm != null && RArm.active)
				{
					RArm.Center = RArmPos[i + 3];
				}
				if (LArm != null && LArm.active)
				{
					LArm.Center = LArmPos[i + 3];
				}
				if (RLeg != null && RLeg.active)
				{
					RLeg.Center = RLegPos[i + 3];
				}
				if (LLeg != null && LLeg.active)
				{
					LLeg.Center = LLegPos[i + 3];
				}
			}
			if (RHand != null && RHand.active)
			{
				RHand.Center = RHandPos[RHandFrame];
			}
			if (LHand != null && LHand.active)
			{
				LHand.Center = LHandPos[LHandFrame];
			}
		}

		void ChangeDir(int dir)
		{
			if (NPC.direction == dir)
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

			Texture2D texHead = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoHead").Value,
					texHead_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoHead_EyeGlow").Value,
					texHead_Glow2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoHead_EyeGlow2").Value,
					texBody = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoBody").Value,
					texBody_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoBody_Glow").Value,
					texShoulderF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoShoulder_Front").Value,
					texShoulderB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoShoulder_Back").Value,
					texArmF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoArm_Front").Value,
					texArmB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoArm_Back").Value,
					texHandF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoHand_Front").Value,
					texHandB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoHand_Back").Value,
					texThighF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoLegThigh_Front").Value,
					texThighB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoLegThigh_Back").Value,
					texCalfF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoLegCalf_Front").Value,
					texCalfB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoLegCalf_Back").Value,
					texFootF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoLegFoot_Front").Value,
					texFootB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/TorizoLegFoot_Back").Value;

			Texture2D texSpawnHead = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoHead").Value,
					texSpawnHead_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoHead_EyeGlow").Value,
					texSpawnHead_Glow2 = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoHead_EyeGlow2").Value,
					texSpawnBody = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoBody").Value,
					texSpawnBody_Glow = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoBody_Glow").Value,
					texSpawnShoulderF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoShoulder_Front").Value,
					texSpawnShoulderB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoShoulder_Back").Value,
					texSpawnArmF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoArm_Front").Value,
					texSpawnArmB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoArm_Back").Value,
					texSpawnHandF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoHand_Front").Value,
					texSpawnHandB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoHand_Back").Value,
					texSpawnThighF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoLegThigh_Front").Value,
					texSpawnThighB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoLegThigh_Back").Value,
					texSpawnCalfF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoLegCalf_Front").Value,
					texSpawnCalfB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoLegCalf_Back").Value,
					texSpawnFootF = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoLegFoot_Front").Value,
					texSpawnFootB = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/Spawn/TorizoLegFoot_Back").Value;

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

			Color glowColor = Color.White * glowAlpha;

			Color eyeGlowColor = Color.White;

			int fHandFrame = RHandFrame;
			int bHandFrame = LHandFrame;
			int fFootFrame = RFootFrame;
			int bFootFrame = LFootFrame;
			if (NPC.spriteDirection == -1)
			{
				fHandFrame = LHandFrame;
				bHandFrame = RHandFrame;
				fFootFrame = LFootFrame;
				bFootFrame = RFootFrame;
			}

			float handRotF = RArmRot[2],
				handRotB = LArmRot[2];
			handRotF -= -(float)Angle.ConvertToRadians(60 - 30 * fHandFrame);
			handRotB -= -(float)Angle.ConvertToRadians(60 - 30 * bHandFrame);

			float headRot = HeadRot;
			if (HeadFrame >= 4)
			{
				//headRot -= -(float)Angle.ConvertToRadians(45);
				headRot -= -(float)Angle.ConvertToRadians(25);
				if (HeadFrame >= 5)
				{
					headRot -= -(float)Angle.ConvertToRadians(20);
				}
			}

			// back arm
			DrawLimbTexture(NPC, sb, texArmB, LArmPos[1], RArmPos[1], LArmRot[1], RArmRot[1], new Vector2(9, 1), armColorB, armColorF, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnArmB, LArmPos[1], RArmPos[1], LArmRot[1], RArmRot[1], new Vector2(9, 1), armColorB * spawnAlpha, armColorF * spawnAlpha, fullScale, effects);

			// back hand
			DrawLimbTexture(NPC, sb, texHandB, LArmPos[2], RArmPos[2], handRotB, handRotF, new Vector2(13, 15), handColorB, handColorF, fullScale, effects, bHandFrame, 3);
			DrawLimbTexture(NPC, sb, texSpawnHandB, LArmPos[2], RArmPos[2], handRotB, handRotF, new Vector2(13, 15), handColorB * spawnAlpha, handColorF * spawnAlpha, fullScale, effects, bHandFrame, 3);

			// back shoulder
			DrawLimbTexture(NPC, sb, texShoulderB, LArmPos[0], RArmPos[0], LArmRot[0], RArmRot[0], new Vector2(15, 15), shoulderColorB, shoulderColorF, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnShoulderB, LArmPos[0], RArmPos[0], LArmRot[0], RArmRot[0], new Vector2(15, 15), shoulderColorB * spawnAlpha, shoulderColorF * spawnAlpha, fullScale, effects);

			// back calf
			DrawLimbTexture(NPC, sb, texCalfB, LLegPos[1], RLegPos[1], LLegRot[1], RLegRot[1], new Vector2(17, 1), calfColorB, calfColorF, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnCalfB, LLegPos[1], RLegPos[1], LLegRot[1], RLegRot[1], new Vector2(17, 1), calfColorB * spawnAlpha, calfColorF * spawnAlpha, fullScale, effects);

			// back foot
			DrawLimbTexture(NPC, sb, texFootB, LLegPos[2], RLegPos[2], LLegRot[2], RLegRot[2], new Vector2(19, 3), footColorB, footColorF, fullScale, effects, bFootFrame, 2);
			DrawLimbTexture(NPC, sb, texSpawnFootB, LLegPos[2], RLegPos[2], LLegRot[2], RLegRot[2], new Vector2(19, 3), footColorB * spawnAlpha, footColorF * spawnAlpha, fullScale, effects, bFootFrame, 2);

			// back thigh
			DrawLimbTexture(NPC, sb, texThighB, LLegPos[0], RLegPos[0], LLegRot[0], RLegRot[0], new Vector2(13, 5), thighColorB, thighColorF, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnThighB, LLegPos[0], RLegPos[0], LLegRot[0], RLegRot[0], new Vector2(13, 5), thighColorB * spawnAlpha, thighColorF * spawnAlpha, fullScale, effects);

			// body
			DrawLimbTexture(NPC, sb, texBody, BodyPos[0], BodyPos[0], BodyRot, BodyRot, new Vector2(28, 49), bodyColor, bodyColor, fullScale, effects, BodyFrame, 2);
			DrawLimbTexture(NPC, sb, texBody_Glow, BodyPos[0], BodyPos[0], BodyRot, BodyRot, new Vector2(28, 49), glowColor, glowColor, fullScale, effects, BodyFrame, 2);
			DrawLimbTexture(NPC, sb, texSpawnBody, BodyPos[0], BodyPos[0], BodyRot, BodyRot, new Vector2(28, 49), bodyColor * spawnAlpha, bodyColor * spawnAlpha, fullScale, effects, BodyFrame, 2);
			DrawLimbTexture(NPC, sb, texSpawnBody_Glow, BodyPos[0], BodyPos[0], BodyRot, BodyRot, new Vector2(28, 49), glowColor * spawnAlpha, glowColor * spawnAlpha, fullScale, effects, BodyFrame, 2);

			// head
			if (Head != null && Head.active)
			{
				Vector2 headOrig = new Vector2(34,48); // 32,38
				DrawLimbTexture(NPC, sb, texHead, HeadPos[0], HeadPos[0], headRot, headRot, headOrig, headColor, headColor, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(NPC, sb, texHead_Glow, HeadPos[0], HeadPos[0], headRot, headRot, headOrig, glowColor, glowColor, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(NPC, sb, texHead_Glow2, HeadPos[0], HeadPos[0], headRot, headRot, headOrig, eyeGlowColor, eyeGlowColor, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(NPC, sb, texSpawnHead, HeadPos[0], HeadPos[0], headRot, headRot, headOrig, headColor*spawnAlpha, headColor*spawnAlpha, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(NPC, sb, texSpawnHead_Glow, HeadPos[0], HeadPos[0], headRot, headRot, headOrig, glowColor*spawnAlpha, glowColor*spawnAlpha, fullScale, effects, HeadFrame, 9);
				DrawLimbTexture(NPC, sb, texSpawnHead_Glow2, HeadPos[0], HeadPos[0], headRot, headRot, headOrig, eyeGlowColor*spawnAlpha, eyeGlowColor*spawnAlpha, fullScale, effects, HeadFrame, 9);
			}

			// front calf
			DrawLimbTexture(NPC, sb, texCalfF, RLegPos[1], LLegPos[1], RLegRot[1], LLegRot[1], new Vector2(17, 1), calfColorF, calfColorB, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnCalfF, RLegPos[1], LLegPos[1], RLegRot[1], LLegRot[1], new Vector2(17, 1), calfColorF * spawnAlpha, calfColorB * spawnAlpha, fullScale, effects);

			// front foot
			DrawLimbTexture(NPC, sb, texFootF, RLegPos[2], LLegPos[2], RLegRot[2], LLegRot[2], new Vector2(19, 3), footColorF, footColorB, fullScale, effects, fFootFrame, 2);
			DrawLimbTexture(NPC, sb, texSpawnFootF, RLegPos[2], LLegPos[2], RLegRot[2], LLegRot[2], new Vector2(19, 3), footColorF * spawnAlpha, footColorB * spawnAlpha, fullScale, effects, fFootFrame, 2);

			// front thigh
			DrawLimbTexture(NPC, sb, texThighF, RLegPos[0], LLegPos[0], RLegRot[0], LLegRot[0], new Vector2(13, 5), thighColorF, thighColorB, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnThighF, RLegPos[0], LLegPos[0], RLegRot[0], LLegRot[0], new Vector2(13, 5), thighColorF * spawnAlpha, thighColorB * spawnAlpha, fullScale, effects);

			// front arm
			DrawLimbTexture(NPC, sb, texArmF, RArmPos[1], LArmPos[1], RArmRot[1], LArmRot[1], new Vector2(9, 1), armColorF, armColorB, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnArmF, RArmPos[1], LArmPos[1], RArmRot[1], LArmRot[1], new Vector2(9, 1), armColorF * spawnAlpha, armColorB * spawnAlpha, fullScale, effects);

			// front hand
			DrawLimbTexture(NPC, sb, texHandF, RArmPos[2], LArmPos[2], handRotF, handRotB, new Vector2(13, 15), handColorF, handColorB, fullScale, effects, fHandFrame, 3);
			DrawLimbTexture(NPC, sb, texSpawnHandF, RArmPos[2], LArmPos[2], handRotF, handRotB, new Vector2(13, 15), handColorF * spawnAlpha, handColorB * spawnAlpha, fullScale, effects, fHandFrame, 3);

			// front shoulder
			DrawLimbTexture(NPC, sb, texShoulderF, RArmPos[0], LArmPos[0], RArmRot[0], LArmRot[0], new Vector2(15, 15), shoulderColorF, shoulderColorB, fullScale, effects);
			DrawLimbTexture(NPC, sb, texSpawnShoulderF, RArmPos[0], LArmPos[0], RArmRot[0], LArmRot[0], new Vector2(15, 15), shoulderColorF * spawnAlpha, shoulderColorB * spawnAlpha, fullScale, effects);

			return false;
		}
		static void DrawLimbTexture(NPC npc, SpriteBatch sb, Texture2D tex, Vector2 Pos1, Vector2 Pos2, float Rot1, float Rot2, Vector2 Origin, Color color1, Color color2, Vector2 scale, SpriteEffects effects, int frame = 0, int frameCount = 1)
		{
			float LimbRot = Rot1;
			Vector2 LimbDrawPos = Pos1;
			Vector2 LimbOrigin = Origin;
			Color LimbColor = color1;
			int LimbFrameHeight = tex.Height / frameCount;
			if (npc.spriteDirection == -1)
			{
				LimbRot = Rot2;
				LimbDrawPos = Pos2;
				LimbOrigin.X = tex.Width - LimbOrigin.X;
				LimbColor = color2;
			}
			sb.Draw(tex, LimbDrawPos - Main.screenPosition, new Rectangle?(new Rectangle(0, frame * LimbFrameHeight, tex.Width, LimbFrameHeight)), LimbColor, LimbRot * npc.spriteDirection, LimbOrigin, scale, effects, 0f);
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
