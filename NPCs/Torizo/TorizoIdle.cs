using Terraria;
using Terraria.ID;
using Terraria.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Torizo
{
    public class TorizoIdle : ModNPC
    {
		private bool active = false;
		private int ai = 0;
		private int tp = 300;
		private bool spawn = false;
		Point pos = Point.Zero;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Suspicious Chozo Statue");
			Main.npcFrameCount[npc.type] = 16;
		}
		public override void SetDefaults()
		{
			npc.townNPC = true;
			npc.width = 48;
			npc.height = 48;
			npc.aiStyle = 7;
			npc.friendly = true;
			npc.damage = 0;
			npc.defense = 9999;
			npc.dontTakeDamage = true;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
			npc.frameCounter = 0;
		}
		public override string GetChat()
		{
			return "The energy tank in the statue's hands gleams mysteriously...";
		}
		public override string TownNPCName()
		{
			return "Suspicious Chozo Statue";
		}
		public override void AI()
		{
			if(active)
			{
				npc.aiStyle = 0;
				ai++;
				if (ai > 190)
				{
					npc.scale += 0.01f;
				}
				if (ai >= 240)
				{
					if (!NPC.AnyNPCs(mod.NPCType("Torizo")))
					{
						NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Torizo"));
					}
					if (Main.netMode == 0)
					{
						Main.NewText(Language.GetTextValue("Announcement.HasAwoken", "Torizo"), 175, 75, 255, false);
					}
					if (Main.netMode == 2)
					{
						NetMessage.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", "The Torizo"), new Color(175, 75, 255), -1);
					}
					npc.life = -1;
					npc.active = false;
				}
			}
			else
			{
				npc.velocity.X = 0;
				npc.ai[0] = 0;
				npc.ai[1] = 0;
				npc.ai[2] = 0;
				npc.ai[3] = 0;
				npc.direction = -1;
				npc.life = 250;
				npc.homeless = true;
				tp++;
				if (tp > 300 && pos != Point.Zero)
				{
                   			npc.position = pos.ToWorldCoordinates() - new Vector2(8, 8);
					tp = 0;
				}
				if (NPC.AnyNPCs(mod.NPCType("Torizo")) || npc.position.X <= 0 || npc.position.Y <= 0 || npc.position.X >= Main.maxTilesX * 16 || npc.position.Y >= Main.maxTilesY * 16)
				{
					npc.life = -1;
					npc.active = false;
				}
			}
			Vector2 tilepos = new Vector2(npc.position.ToTileCoordinates().X, npc.position.ToTileCoordinates().Y);
			if (!spawn && (Main.tileSolid[Main.tile[(int)tilepos.X, (int)tilepos.Y + 3].type] && Main.tile[(int)tilepos.X, (int)tilepos.Y + 3].active() || Main.tileSolid[Main.tile[(int)tilepos.X+2, (int)tilepos.Y + 3].type] && Main.tile[(int)tilepos.X+2, (int)tilepos.Y + 3].active() || Main.tileSolid[Main.tile[(int)tilepos.X + 1, (int)tilepos.Y + 3].type] && Main.tile[(int)tilepos.X + 1, (int)tilepos.Y + 3].active()))
			{
				pos = npc.position.ToTileCoordinates();
				spawn = true;
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];
			return tile.type == 38 && !NPC.AnyNPCs(mod.NPCType("Torizo")) && !NPC.AnyNPCs(mod.NPCType("TorizoIdle")) && spawnInfo.spawnTileY > Main.spawnTileY + 420 && spawnInfo.spawnTileY < Main.spawnTileY + 550 && spawnInfo.spawnTileX > Main.spawnTileX + 100 && spawnInfo.spawnTileX < Main.spawnTileX + 270 ? 5f : 0f;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			if(active)
			{
				npc.frameCounter++;
			}
			if (npc.frameCounter >= 12 && npc.frame.Y < 1312)
			{
				npc.frameCounter = 0;	
				npc.frame.Y = (npc.frame.Y + 82);
			}
			if (npc.frame.Y >= 1312)
			{
				npc.frame.Y = 1230;	
			}
		}
		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = "Take the energy tank";
		}
		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton && !NPC.AnyNPCs(mod.NPCType("Torizo")))
			{
				active = true;
			}
		}
	}
}
