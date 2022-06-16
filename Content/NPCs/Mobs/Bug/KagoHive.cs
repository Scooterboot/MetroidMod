using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.NPCs.Mobs.Bug
{
	public class KagoHive : MNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 5;
		}
		public override void SetDefaults()
		{
			NPC.width = 32; NPC.height = 32;

			/* Temporary NPC values */
			NPC.scale = 2;
			NPC.damage = 15;
			NPC.defense = 5;
			NPC.lifeMax = 150;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement>
			{
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("A hive that the Kago live in, its inhabitants attack when the hive is endangered.")
			});
		}
		public override bool PreAI()
		{
			if (NPC.ai[0] == 0)
				NPC.ai[0] = NPC.lifeMax;

			if (NPC.justHit)
			{
				NPC.TargetClosest(false);

				this.SpawnKago(Main.player[NPC.target], (int)NPC.ai[0] - NPC.life);

				NPC.ai[0] = NPC.life;
			}

			return (true);
		}

		public override void FindFrame(int frameHeight)
		{
			if(NPC.frameCounter++ >= 20)
			{
				NPC.frame.Y = (NPC.frame.Y + frameHeight) % (Main.npcFrameCount[NPC.type] * frameHeight);
				NPC.frameCounter = 0;
			}
		}
		private void SpawnKago(Player player, int damage)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient) return;

			var entitySource = NPC.GetSource_FromAI();

			// Spawn one Kago per hit.
			NPC kago = Main.npc[NPC.NewNPC(entitySource, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Kago>())];

			// Assign a random starting velocity to the newly created Kago to give some sort of 'punch-out' effect.
			kago.velocity = new Vector2(Main.rand.Next(6, 12) * Math.Sign(NPC.Center.X - player.Center.X), -2);
			kago.netUpdate = true;
		}
	}
}
