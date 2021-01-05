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

using MetroidMod.Common.Worlds;

namespace MetroidMod.NPCs.Torizo
{
	[AutoloadHead]
    public class IdleTorizo : ModNPC
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("???");
		}
		public override void SetDefaults()
		{
			npc.width = 96;
			npc.height = 96;
			npc.aiStyle = -1;
			npc.townNPC = true;
			npc.friendly = true;
			npc.damage = 0;
			npc.defense = 0;
			npc.dontTakeDamage = true;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
		}
		public override bool CanChat()
		{
			return false;
		}
		
		Vector2 eTankPos = new Vector2(32,-32);
		int eTankFrame = 0;
		int eTankFrameCounter = 0;
		bool drawETank = true;
		
		Vector2[] gorePos = {new Vector2(-11,-33),
		new Vector2(1,-13),new Vector2(-13,-1),new Vector2(18,8),
		new Vector2(32,-8),new Vector2(-35,-23),new Vector2(-29,14),
		new Vector2(-19,29),new Vector2(15,27),new Vector2(27,39)};
		
		public override void AI()
		{
			npc.GivenName = "";
			Rectangle room = MWorld.TorizoRoomLocation;
			if(room.X > 0 && room.Y > 0)
			{
				Vector2 pos = new Vector2(room.X+8,room.Y+room.Height-4);
				npc.direction = 1;
				if(room.X > Main.maxTilesX/2)
				{
					pos.X = (room.X+room.Width-8);
					npc.direction = -1;
				}
				pos *= 16f;
				npc.spriteDirection = npc.direction;
				
				npc.position.X = pos.X-npc.width/2;
				npc.position.Y = pos.Y-npc.height;
			}
			
			for(int i = 0; i < 255; i++)
			{
				Player player = Main.player[i];
				if(player.active && !player.dead && Vector2.Distance(player.Center,npc.Center) < 200f && 
					Collision.CanHit(npc.position,npc.width,npc.height,player.position,player.width,player.height) && npc.ai[0] == 0)
				{
					npc.ai[0] = 1;
					npc.target = player.whoAmI;
				}
			}
			
			eTankFrameCounter++;
			if(eTankFrameCounter > 2)
			{
				eTankFrame++;
				eTankFrameCounter = 0;
			}
			if(eTankFrame > 1)
			{
				eTankFrame = 0;
			}
			
			if(npc.ai[0] == 1)
			{
				if(npc.ai[1] <= 0)
				{
					Vector2 ePos = npc.Center + new Vector2(eTankPos.X*npc.direction,eTankPos.Y);
					if(Main.netMode != 2)
					{
						for (int i = 0; i < 10; i++)
						{
							Dust dust = Dust.NewDustDirect(ePos-new Vector2(16,16), 32, 32, 57, 0f, 0f, 100, default(Color), 3f);
							dust.velocity *= 1.4f;
							dust.noGravity = true;
							dust = Dust.NewDustDirect(ePos-new Vector2(16,16), 32, 32, 30, 0f, 0f, 100, default(Color), 3f);
							dust.velocity *= 1.4f;
							dust.noGravity = true;
						}
						for(int i = 1; i <= 4; i++)
						{
							Vector2 velocity = new Vector2(-Main.rand.Next(31),-Main.rand.Next(31)) * 0.2f * 0.4f;
							if(i % 2 == 0)
							{
								velocity.X *= -1;
							}
							Gore gore = Gore.NewGoreDirect(ePos, velocity, mod.GetGoreSlot("Gores/TorizoETankGore" + i));
							gore.velocity.X = velocity.X;
							gore.timeLeft = 60;
						}
						Main.PlaySound(2,(int)ePos.X,(int)ePos.Y,14);
					}
					drawETank = false;
					
					for(int i = 0; i < 6; i++)
					{
						Vector2 velocity = new Vector2(Main.rand.Next(12)*npc.direction,-Main.rand.Next(12));
						if(velocity.Length() < 6f)
						{
							velocity *= (6f/velocity.Length());
						}
						int part = Projectile.NewProjectile(ePos,velocity,mod.ProjectileType("Torizo_EnergyParticle"),0,0f,255,npc.Center.X,npc.Center.Y);
						Main.projectile[part].ai[0] = npc.Center.X;
						Main.projectile[part].ai[1] = npc.Center.Y;
					}
					
					npc.ai[1] = 1;
				}
				else
				{
					npc.ai[1]++;
					if(npc.ai[1] > 180)
					{
						npc.ai[0] = 2;
						npc.ai[1] = 0;
					}
				}
			}
			if(npc.ai[0] == 2)
			{
				if(Main.netMode != 2)
				{
					for(int i = 9; i >= 0; i--)
					{
						Vector2 gPos = npc.Center + gorePos[i];
						byte goreFrame = 0;
						if(npc.direction == -1)
						{
							gPos.X = npc.Center.X - gorePos[i].X;
							goreFrame = 1;
						}
						Vector2 velocity = new Vector2(gPos.X-npc.Center.X,gPos.Y-(npc.position.Y+npc.height))*0.02f;
						
						int type = mod.GetGoreSlot("Gores/TorizoStatueGore" + (1+i));
						gPos.X -= Main.goreTexture[type].Width / 2;
						gPos.Y -= Main.goreTexture[type].Height / 4;
						Gore gore = Gore.NewGorePerfect(gPos, velocity, type);
						gore.numFrames = 2;
						gore.frame = goreFrame;
						gore.timeLeft = 60;
						int stype = 0;
						if(i % 2 == 0)
						{
							stype = 21;
						}
						Main.PlaySound(stype, (int)gPos.X, (int)gPos.Y, 1, 1f, 0f);
					}
					for(int i = 0; i < 35; i++)
					{
						Dust dust = Main.dust[Dust.NewDust(npc.position-new Vector2(8,8), npc.width+16, npc.height+16, 30, 0f, 0f, 100, default(Color), 2.5f)];
						dust.velocity *= 1.4f;
						dust.noGravity = true;
					}
				}
				
				if (!NPC.AnyNPCs(mod.NPCType("Torizo")))
				{
					Vector2 tPos = new Vector2(npc.Center.X-26*npc.direction,npc.position.Y+npc.height-117);
					NPC.NewNPC((int)tPos.X,(int)tPos.Y,mod.NPCType("Torizo"),npc.whoAmI, 0,1,0,0, npc.target);
				}
				if (Main.netMode == 0)
				{
					Main.NewText(Language.GetTextValue("Announcement.HasAwoken", "Torizo"), 175, 75, 255, false);
				}
				if (Main.netMode == 2)
				{
					NetMessage.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", "The Torizo"), new Color(175, 75, 255), -1);
				}
				npc.active = false;
			}
		}
		
		/*public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			//Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];
			//return tile.type == 38 && !NPC.AnyNPCs(mod.NPCType("Torizo")) && !NPC.AnyNPCs(mod.NPCType("TorizoIdle")) && spawnInfo.spawnTileY > Main.spawnTileY + 420 && spawnInfo.spawnTileY < Main.spawnTileY + 550 && spawnInfo.spawnTileX > Main.spawnTileX + 100 && spawnInfo.spawnTileX < Main.spawnTileX + 270 ? 5f : 0f;
			
			Rectangle room = MWorld.TorizoRoomLocation;
			bool rectFlag = (room.X > 0 && room.Y > 0);
			bool rectFlag2 = (spawnInfo.spawnTileX > room.X && spawnInfo.spawnTileX < room.X+room.Width && spawnInfo.spawnTileY > room.Y && spawnInfo.spawnTileY < room.Y+room.Height);
			return (!NPC.AnyNPCs(mod.NPCType("Torizo")) && !NPC.AnyNPCs(mod.NPCType("IdleTorizo")) && rectFlag && rectFlag2) ? 5f : 0f;
		}*/
		
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			if(drawETank)
			{
				Texture2D eTex = mod.GetTexture("NPCs/Torizo/IdleTorizo_ETank");
				Vector2 ePos = npc.Center + new Vector2(eTankPos.X*npc.direction,eTankPos.Y);
				int texH = (eTex.Height / 2);
				sb.Draw(eTex,ePos - Main.screenPosition,new Rectangle?(new Rectangle(0,eTankFrame*texH,eTex.Width,texH)),npc.GetAlpha(drawColor),0f,new Vector2(eTex.Width/2,texH/2),1f,SpriteEffects.None,0f);
			}
			
			Texture2D tex = Main.npcTexture[npc.type];
			SpriteEffects effects = SpriteEffects.None;
			if(npc.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			sb.Draw(tex,npc.Center - Main.screenPosition,new Rectangle?(new Rectangle(0,0,tex.Width,tex.Height)),npc.GetAlpha(drawColor),0f,new Vector2(tex.Width/2,tex.Height/2),1f,effects,0f);
			
			return false;
		}
	}
}