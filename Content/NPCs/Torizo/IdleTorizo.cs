using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MetroidModPorted.Common.Systems;

namespace MetroidModPorted.Content.NPCs.Torizo
{
	[AutoloadBossHead]
	public class IdleTorizo : ModNPC
	{
		public override string BossHeadTexture => Mod.Name + "/Content/NPCs/Torizo/IdleTorizo_Head";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("???");
			NPCID.Sets.MPAllowedEnemies[Type] = true;
		}
		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.OldMan);
			
			NPC.width = 96;
			NPC.height = 96;
			NPC.aiStyle = -1;
			NPC.npcSlots = 0;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.dontTakeDamage = true;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			/*for(int i = 0; i < NPC.ai.Length; i++)
			{
				NPC.ai[i] = 0.0f;
			}*/
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
		
		public override bool PreAI()
		{
			return false;
		}
		public override void PostAI()
		{
			Rectangle room = MSystem.TorizoRoomLocation;
			if(room.X > 0 && room.Y > 0)
			{
				Vector2 pos = new Vector2(room.X+8,room.Y+room.Height-4);
				NPC.direction = 1;
				if(room.X > Main.maxTilesX/2)
				{
					pos.X = (room.X+room.Width-8);
					NPC.direction = -1;
				}
				pos *= 16f;
				NPC.spriteDirection = NPC.direction;
				
				NPC.position.X = pos.X-NPC.width/2;
				NPC.position.Y = pos.Y-NPC.height;
				
				for(int i = 0; i < 255; i++)
				{
					Player player = Main.player[i];
					if(player.active && !player.dead && Vector2.Distance(player.Center,NPC.Center) < 200f && 
						Collision.CanHit(NPC.position,NPC.width,NPC.height,player.position,player.width,player.height) && NPC.ai[0] == 0)
					{
						NPC.ai[0] = 1;
						NPC.target = player.whoAmI;
					}
				}
			}
			else
			{
				NPC.active = false;
				return;
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
			
			if(NPC.ai[0] == 1)
			{
				if(NPC.ai[1] <= 0)
				{
					Vector2 ePos = NPC.Center + new Vector2(eTankPos.X*NPC.direction,eTankPos.Y);
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
							Gore gore = Gore.NewGoreDirect(NPC.GetSource_FromAI(), ePos, velocity, Mod.Find<ModGore>("TorizoETankGore" + i).Type);
							gore.velocity.X = velocity.X;
							gore.timeLeft = 60;
						}
						SoundEngine.PlaySound(SoundID.Item,(int)ePos.X,(int)ePos.Y,14);
					}
					drawETank = false;
					
					var entitySource = NPC.GetSource_FromAI();
					for(int i = 0; i < 6; i++)
					{
						Vector2 velocity = new Vector2(Main.rand.Next(12)*NPC.direction,-Main.rand.Next(12));
						if(velocity.Length() < 6f)
						{
							velocity *= (6f/velocity.Length());
						}
						int part = Projectile.NewProjectile(entitySource, ePos,velocity,ModContent.ProjectileType<Projectiles.Boss.Torizo_EnergyParticle>(),0,0f,255,NPC.Center.X,NPC.Center.Y);
						Main.projectile[part].ai[0] = NPC.Center.X;
						Main.projectile[part].ai[1] = NPC.Center.Y;
					}
					
					NPC.ai[1] = 1;
				}
				else
				{
					NPC.ai[1]++;
					if(NPC.ai[1] > 180)
					{
						NPC.ai[0] = 2;
						NPC.ai[1] = 0;
					}
				}
			}
			if(NPC.ai[0] == 2)
			{
				if(Main.netMode != 2)
				{
					var entitySource = NPC.GetSource_FromAI();
					for(int i = 9; i >= 0; i--)
					{
						Vector2 gPos = NPC.Center + gorePos[i];
						byte goreFrame = 0;
						if(NPC.direction == -1)
						{
							gPos.X = NPC.Center.X - gorePos[i].X;
							goreFrame = 1;
						}
						Vector2 velocity = new Vector2(gPos.X-NPC.Center.X,gPos.Y-(NPC.position.Y+NPC.height))*0.02f;
						
						int type = Mod.Find<ModGore>("TorizoStatueGore" + (1+i)).Type;
						gPos.X -= Terraria.GameContent.TextureAssets.Gore[type].Value.Width / 2;
						gPos.Y -= Terraria.GameContent.TextureAssets.Gore[type].Value.Height / 4;
						Gore gore = Gore.NewGorePerfect(entitySource, gPos, velocity, type);
						gore.numFrames = 2;
						gore.frame = goreFrame;
						gore.timeLeft = 60;
						int stype = 0;
						if(i % 2 == 0)
						{
							stype = 21;
						}
						SoundEngine.PlaySound(stype, (int)gPos.X, (int)gPos.Y, 1, 1f, 0f);
					}
					for(int i = 0; i < 35; i++)
					{
						Dust dust = Main.dust[Dust.NewDust(NPC.position-new Vector2(8,8), NPC.width+16, NPC.height+16, 30, 0f, 0f, 100, default(Color), 2.5f)];
						dust.velocity *= 1.4f;
						dust.noGravity = true;
					}
				}
				
				if (!NPC.AnyNPCs(ModContent.NPCType<Torizo>()))
				{
					Vector2 tPos = new Vector2(NPC.Center.X-26*NPC.direction,NPC.position.Y+NPC.height-117);
					NPC.NewNPC(NPC.GetSource_FromAI(), (int)tPos.X,(int)tPos.Y,ModContent.NPCType<Torizo>(),NPC.whoAmI, 0,1,0,0, NPC.target);
				}
				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					Main.NewText(Language.GetTextValue("Announcement.HasAwoken", "Torizo"), new Color(175, 75, 255));
				}
				if (Main.netMode == NetmodeID.Server)
				{
					Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", "The Torizo"), new Color(175, 75, 255), -1);
				}
				NPC.active = false;
			}
		}

		public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
			if(drawETank)
			{
				Texture2D eTex = ModContent.Request<Texture2D>($"{Mod.Name}/Content/NPCs/Torizo/IdleTorizo_ETank").Value;
				Vector2 ePos = NPC.Center + new Vector2(eTankPos.X*NPC.direction,eTankPos.Y);
				int texH = (eTex.Height / 2);
				sb.Draw(eTex,ePos - Main.screenPosition,new Rectangle?(new Rectangle(0,eTankFrame*texH,eTex.Width,texH)),NPC.GetAlpha(drawColor),0f,new Vector2(eTex.Width/2,texH/2),1f,SpriteEffects.None,0f);
			}
			
			Texture2D tex = Terraria.GameContent.TextureAssets.Npc[Type].Value;
			SpriteEffects effects = SpriteEffects.None;
			if(NPC.spriteDirection == -1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			sb.Draw(tex,NPC.Center - Main.screenPosition,new Rectangle?(new Rectangle(0,0,tex.Width,tex.Height)),NPC.GetAlpha(drawColor),0f,new Vector2(tex.Width/2,tex.Height/2),1f,effects,0f);
			
			return false;
		}
	}
}
