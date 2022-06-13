using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace MetroidModPorted.Content.NPCs.Serris
{
	public class Serris_Tail : Serris_Body
	{
		private int tailType
		{
			get { return (int)NPC.ai[2]; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serris");
			Main.npcFrameCount[Type] = 15;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.width = 32;
			NPC.height = 32;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			int associatedNPCType = ModContent.NPCType<Serris_Head>();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
				new FlavorTextBestiaryInfoElement("An invasive species brought by the Gizzard tribe and released into the seas after the tribe's collapse. The creature moves at extremely high speeds and is hard to keep an eye on. Attacking it will cause it to immediately retaliate and rush into you. Be aware of the creature's speed and strike with a charged attack at the head when it's not moving. Sometimes however… a creature isn't what it appears to be…")
			});
		}
		public override bool PreAI()
		{
			if(tailType > 0)
			{
				NPC.width = 20;
				NPC.height = 20;
			}
			return true;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode != NetmodeID.Server)
			{
				if (NPC.life <= 0)
				{
					int gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SerrisGore3").Type, 1f);
					Main.gore[gore].velocity *= 0.4f;
					Main.gore[gore].timeLeft = 60;
				}
			}
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			Texture2D texTail = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Serris/Serris_Tail").Value;
			Serris_Head serris_head = (Serris_Head)head.ModNPC;

			float bRot = NPC.rotation - 1.57f;
			int tailHeight = texTail.Height / 15;
			Vector2 tailOrig = new Vector2(28, 29);
			Color bodyColor = NPC.GetAlpha(Lighting.GetColor((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16));

			SpriteEffects effects = SpriteEffects.None;
			if (head.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipVertically;
				tailOrig.Y = tailHeight - tailOrig.Y;
			}
			int frame = serris_head.state - 1;
			if (serris_head.state == 4)
				frame = serris_head.sbFrame + 3;

			int yFrame = frame * (tailHeight * 3) + (tailHeight * tailType);
			sb.Draw(texTail, NPC.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, yFrame, texTail.Width, tailHeight)),
			bodyColor, bRot, tailOrig, 1f, effects, 0f);
			return (false);
		}
	}
}
