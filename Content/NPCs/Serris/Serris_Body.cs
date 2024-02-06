using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Serris
{
	public class Serris_Body : Serris
	{
		protected NPC head
		{
			get { return Main.npc[NPC.realLife]; }
		}
		protected NPC start
		{
			get { return Main.npc[(int)NPC.ai[1]]; }
		}
		protected NPC end
		{
			get { return Main.npc[(int)NPC.ai[0]]; }
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Serris");
			Main.npcFrameCount[NPC.type] = 10;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}
		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 40;
			NPC.damage = 20;
			NPC.defense = 28;
			NPC.lifeMax = 500;
			NPC.dontTakeDamage = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
			NPC.knockBackResist = 0;
			NPC.lavaImmune = true;
			NPC.noTileCollide = true;
			NPC.behindTiles = true;
			NPC.frameCounter = 0;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1;
			NPC.boss = true;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			int associatedNPCType = ModContent.NPCType<Serris_Head>();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("An invasive species brought by the Gizzard tribe and released into the seas after the tribe's collapse. The creature moves at extremely high speeds and is hard to keep an eye on. Attacking it will cause it to immediately retaliate and rush into you. Be aware of the creature's speed and strike with a charged attack at the head when it's not moving. Sometimes however... a creature isn't what it appears to be...")
			});
		}

		public override void AI()
		{
			Update_Worm();
			NPC.damage = head.damage;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				if (NPC.life <= 0)
				{
					int gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SerrisGore2").Type, 1f);
					Main.gore[gore].velocity *= 0.4f;
					Main.gore[gore].timeLeft = 60;
				}
			}
		}
		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Texture2D texBody = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Serris/Serris_Body").Value,
				texFins = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Serris/Serris_Fins").Value;
			Serris_Head serris_head = (Serris_Head)head.ModNPC;

			Vector2 bodyOrig = new Vector2(32, 35),
				finsOrig = new Vector2(52, 31);
			int bodyHeight = texBody.Height / 10,
				finsHeight = texFins.Height / 15;

			float bodyRot = NPC.rotation - 1.57f;
			float headRot = head.rotation - 1.57f;
			Color bodyColor = NPC.GetAlpha(Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16));

			SpriteEffects effects = SpriteEffects.None;
			if (head.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipVertically;
				bodyOrig.Y = bodyHeight - bodyOrig.Y;
				finsOrig.Y = finsHeight - finsOrig.Y;
			}
			int frame = serris_head.state - 1;
			if (serris_head.state == 4)
				frame = serris_head.sbFrame + 3;

			// If it's the last body part before the head, draw the fins.
			int yFrame = frame * (bodyHeight * 2);
			if ((int)NPC.ai[1] == head.whoAmI)
			{
				for (int j = 0; j < 3; j++)
				{
					int finFrame = finsHeight * j + frame * (finsHeight * 3);
					Vector2 finPos = new Vector2(4, -16);
					float bodyRot2 = bodyRot - (headRot - bodyRot);
					Vector2 finRotPos = bodyRot.ToRotationVector2();
					if (float.IsNaN(finRotPos.X) || float.IsNaN(finRotPos.Y))
					{
						finRotPos = -Vector2.UnitY;
					}
					if (j == 0)
					{
						finPos = new Vector2(-14, -14);
						finRotPos = Vector2.Normalize(Vector2.Lerp(finRotPos, bodyRot2.ToRotationVector2(), 0.5f));
					}
					if (j == 2)
					{
						finPos = new Vector2(20, -16);
						finRotPos = Vector2.Normalize(Vector2.Lerp(finRotPos, headRot.ToRotationVector2(), 0.5f));
					}
					if (head.spriteDirection == -1)
					{
						finPos.Y *= -1;
					}
					float finRot = finRotPos.ToRotation();
					finRot += (((float)Math.PI / 16) - ((float)Math.PI / 8) * (1f - serris_head.mouthFrame)) * 0.5f * head.spriteDirection;
					float finPosRot = finPos.ToRotation() + bodyRot;
					Vector2 finalFinPos = NPC.Center + finPosRot.ToRotationVector2() * finPos.Length();
					sb.Draw(texFins, finalFinPos - Main.screenPosition, new Rectangle?(new Rectangle(0, finFrame, texFins.Width, finsHeight)),
					bodyColor, finRot, finsOrig, 1f, effects, 0f);
				}
			}
			else
				yFrame += bodyHeight;

			sb.Draw(texBody, NPC.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, yFrame, texBody.Width, bodyHeight)),
			bodyColor, bodyRot, bodyOrig, 1f, effects, 0f);

			return (false);
		}
	}
}
