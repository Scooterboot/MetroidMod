using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroidMod.NPCs.Torizo
{
	[AutoloadBossHead]
    public class Torizo : ModNPC
    {
		private bool spawn = false;
		float armRot = 0;
		float armRot2 = 0;
		float speed = 1.5f;
		int slash = 0;
		bool jumpBack = false;
		int beam = 0;
		Player P = null;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torizo");
			Main.npcFrameCount[npc.type] = 8;
		}
		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 72;
			npc.scale = 1.5f;
			npc.damage = 15;
			npc.defense = 10;
			npc.lifeMax = 1000;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath3;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.value = Item.buyPrice(0, 2, 80, 0);
			npc.knockBackResist = 0;
			npc.lavaImmune = true;
			npc.frameCounter = 0;
			npc.aiStyle = -1;
			npc.npcSlots = 0;
			npc.boss = true;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Ridley");
			bossBag = mod.ItemType("TorizoBag");
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.7f * bossLifeScale + 1);
			npc.damage = (int)(npc.damage * 0.7f);
		}
		public override void NPCLoot()
		{
			MWorld.downedTorizo = true;
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
		public override bool PreAI()
		{
			if (!spawn)
			{
				int TorizoHead = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("TorizoHead"));
				Main.npc[TorizoHead].ai[0] = npc.whoAmI;
				Main.npc[TorizoHead].scale = npc.scale;
				Main.npc[TorizoHead].lifeMax = (int)(npc.lifeMax/2);
				Main.npc[TorizoHead].life = (int)(npc.life/2);
				Main.npc[TorizoHead].width = (int)(Main.npc[TorizoHead].width * npc.scale);
				Main.npc[TorizoHead].height = (int)(Main.npc[TorizoHead].height * npc.scale);
				npc.TargetClosest(true);
				P = Main.player[npc.target];
				spawn = true;
			}
			
			return true;
		}
		public override void AI()
		{
			if (Main.expertMode && NPC.AnyNPCs(mod.NPCType("TorizoHead")))
			{
				npc.dontTakeDamage = true;
			}
			else
			{
				npc.dontTakeDamage = false;
			}
			if (!P.active || P.dead)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead)
				{
					npc.active = false;
				}
			}
			#region Life
				if(npc.life <= (int)(npc.lifeMax) && npc.life >= (int)(npc.lifeMax * 0.8f))
				{
					npc.color = new Color(255, 255, 255);
					speed = 1.5f;
				}
			if(npc.life < (int)(npc.lifeMax * 0.8f) && npc.life >= (int)(npc.lifeMax * 0.6f))
				{
					npc.color = new Color(255, 220, 220);
					speed = 2f;
				}
			if(npc.life < (int)(npc.lifeMax * 0.6f) && npc.life >= (int)(npc.lifeMax * 0.4f))
				{
					npc.color = new Color(255, 190, 190);
					speed = 2.5f;
				}
			if(npc.life < (int)(npc.lifeMax * 0.6f) && npc.life >= (int)(npc.lifeMax * 0.4f))
				{
					npc.color = new Color(255, 150, 150);
					speed = 3f;
				}
			if(npc.life < (int)(npc.lifeMax * 0.4f) && npc.life >= (int)(npc.lifeMax * 0.2f))
				{
					npc.color = new Color(255, 110, 110);
					speed = 3.5f;
				}
				if(npc.life < (int)(npc.lifeMax * 0.2f))
				{
					npc.color = new Color(255, 70, 70);
					speed = 4f;
				}
				#endregion
			if (Main.expertMode)
			{
				speed = speed * 1.3f;
			}
			npc.ai[0]++;

			npc.spriteDirection = npc.direction;

			npc.TargetClosest(true);
			P = Main.player[npc.target];

			#region Movement


			if(!jumpBack && beam <= 0)
			{
				if (npc.velocity.X == 0)
				{
					if (P.position.X+P.width < npc.position.X)
					{
						npc.velocity.X = -speed;
						npc.velocity.Y = -5f;
					}
					if (P.position.X > npc.position.X+npc.width)
					{
						npc.velocity.X = speed;
						npc.velocity.Y = -5f;
					}
				}
				if (P.Center.Y + 40*npc.scale < npc.Center.Y && npc.velocity.Y == 0)
				{
					npc.velocity.Y = -(float)Math.Sqrt(2 * 0.3f * Math.Abs(P.position.Y - (npc.position.Y+npc.height)));
				}
				if (npc.velocity.Y < 0 && !Collision.CanHitLine(new Vector2(npc.Center.X, npc.position.Y), 1, 1, new Vector2(P.Center.X, P.position.Y), 1, 1))
				{
					npc.noTileCollide = true;
				}
				else
				{
					npc.noTileCollide = false;
				}

				if (P.Center.Y - 40*npc.scale > npc.Center.Y)
				{
					npc.position.Y++;
				}
				if(P.Center.X < npc.Center.X - 50*npc.scale)
				{
					npc.velocity.X = -speed;
				}
				if(P.Center.X > npc.Center.X + 50*npc.scale)
				{
					npc.velocity.X = speed;
				}

				if(slash <= 0 && Math.Abs(P.Center.X - npc.Center.X) < 50*npc.scale && Math.Abs(P.Center.Y - npc.Center.Y) < 40*npc.scale)
				{
					if (Main.rand.Next(3) == 0 && npc.velocity.Y == 0)
					{
						jumpBack = true;
						npc.velocity.Y = -9f;
					}
					else
					{
						slash = 1;
					}
				}
			}
			

			#endregion


			#region JumpBack
			if(jumpBack)
			{
				armRot = 36 * npc.direction * 0.0174f;
				armRot2 = 36 * npc.direction * 0.0174f;
				if(P.Center.X < npc.Center.X)
				{
					npc.velocity.X = speed * 2;
				}
				if(P.Center.X > npc.Center.X)
				{
					npc.velocity.X = -speed * 2;
				}
				if (npc.velocity.Y == 0)
				{
					armRot = 0;
					armRot2 = 0;
					jumpBack = false;	
				}
			}
			#endregion

			
			#region Slash

			if (slash > 0)
			{
				slash++;
				if (slash <= 20)
				{
					armRot = slash * 6 * -npc.direction * 0.0174f;			
				}
			}
			if (slash > 20 && slash <= 46)
			{
				int slash2 = 20 - (slash - 20);
				if (slash % 5 == 0)
				{
					Vector2 slashPos = (npc.Center - new Vector2(8,8) + new Vector2(2f*npc.scale*npc.direction, -22f*npc.scale)) + ((slash2+(18*-npc.direction)) * 5 * -npc.direction * 0.0174f).ToRotationVector2()*48*npc.scale;
					Projectile.NewProjectile(slashPos.X,slashPos.Y,0,0,mod.ProjectileType("TorizoSlash"),10,2,0);
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 7);
				}
				armRot = slash2 * 6 * -npc.direction * 0.0174f;
			}
			if (slash > 46 && slash <= 66)
			{
				int slash3 = slash - 46;
				armRot2 = slash3 * 6 * -npc.direction * 0.0174f;
			}
			if(slash > 66 && slash <= 86)
			{
				int slash4 = 20 - (slash - 66);
				if (slash % 5 == 0)
				{
					Vector2 slashPos2 = (npc.Center - new Vector2(8,8) + new Vector2(4f*npc.scale*npc.direction, -22f*npc.scale)) + ((slash4+(18*-npc.direction)) * 5 * -npc.direction * 0.0174f).ToRotationVector2()*48*npc.scale;
					Projectile.NewProjectile(slashPos2.X,slashPos2.Y,0,0,mod.ProjectileType("TorizoSlash"),10,6,0);
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 7);
				}
				armRot2 = slash4 * 6 * -npc.direction * 0.0174f;
			}
			if (slash > 86)
			{
				armRot = 0;
				armRot2 = 0;
				slash = 0;
			}

			#endregion

			#region Beam
			int	beamCool = Main.expertMode ? 1750 : 2500;
			if (beam <= 0)
			{
				npc.ai[1]+= 1 + Main.rand.Next(5);
			}
			if(beam <= 0 && slash <= 0 && npc.ai[1] > beamCool && !jumpBack && npc.velocity.Y == 0)
			{
				beam = 1;
			}
			if (beam > 0)
			{
				npc.velocity.X = 0;
				npc.velocity.Y = 0;
				beam++;
				if (beam <= 16)
				{
					armRot = beam * 7.5f * -npc.direction * 0.0174f;			
				}
				if (beam > 16 && beam <= 32)
				{
					int beam2 = 16 - (beam - 16);
					armRot = beam2 * 7.5f * -npc.direction * 0.0174f;			
				}
				if (beam == 20 || beam == 60)
				{
					float Speed = speed * 2;
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 15);
					float rotation = (float)Math.Atan2(npc.Center.Y - P.Center.Y, npc.Center.X - P.Center.X);
					int num54 = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), mod.ProjectileType("TorizoBeam"), 10, 2f, 0);
				}
				if (beam > 52 && beam <= 68)
				{
					armRot2 = (beam - 52) * 7.5f * -npc.direction * 0.0174f;
				}
				if (beam > 68 && beam <= 84)
				{
					int beam3 = 16 - (beam - 68);
					armRot2 = beam3 * 7.5f * -npc.direction * 0.0174f;
				}
				if (beam > 94)
				{
					if (Main.rand.Next(3) == 0)
					{
						beam = 1;
					}
					else 
					{
						npc.ai[1] = 0;
						beam = 0;
					}
				}
			}
			#endregion

		}
		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			if(npc.velocity.X != 0 && npc.velocity.Y == 0)
			{
				npc.frameCounter++;
			}
			if (npc.velocity.Y != 0)
			{
				npc.frame.Y = 144;
			}
			if (npc.frameCounter >= (int)(16/speed)*npc.scale && npc.velocity.Y == 0)
			{
				npc.frameCounter = 0;	
				npc.frame.Y = (npc.frame.Y + 72);
			}
			if (npc.frame.Y >= 576)
			{
				npc.frame.Y = 0;	
			}
		}

		public override void PostDraw(SpriteBatch sb, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (npc.direction == 1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			else
			{
				effects = SpriteEffects.None;
			}
			Color buffColor = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));
			Color alpha2 = npc.GetAlpha(buffColor);
			Color color = new Color((int)((float)npc.color.R*((float)alpha2.R/255)), (int)((float)npc.color.G*((float)alpha2.G/255)),(int)((float)npc.color.B*((float)alpha2.B/255)));
			Texture2D tex =  mod.GetTexture("NPCs/Torizo/TorizoArm");
			Rectangle rect2 = new Rectangle(0, 0, (tex.Width), (tex.Height));
			Vector2 vector13 = new Vector2((float)tex.Width / 2, (float)tex.Height / 2);
			sb.Draw(tex, npc.Center - Main.screenPosition + new Vector2(4f*npc.scale*npc.direction, -22f*npc.scale), new Rectangle?(rect2), color, armRot, vector13, npc.scale, effects, 0f);
		}
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			SpriteEffects effects = SpriteEffects.None;
			if (npc.direction == 1)
			{
				effects = SpriteEffects.FlipHorizontally;
			}
			else
			{
				effects = SpriteEffects.None;
			}

			Color buffColor = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));
			Color alpha2 = npc.GetAlpha(buffColor);
			Color color = new Color((int)((float)npc.color.R*((float)alpha2.R/255)), (int)((float)npc.color.G*((float)alpha2.G/255)),(int)((float)npc.color.B*((float)alpha2.B/255)));
			Texture2D tex =  mod.GetTexture("NPCs/Torizo/TorizoArm");
			Rectangle rect2 = new Rectangle(0, 0, (tex.Width), (tex.Height));
			Vector2 vector13 = new Vector2((float)tex.Width / 2, (float)tex.Height / 2);
			sb.Draw(tex, npc.Center - Main.screenPosition + new Vector2(4f*npc.scale*npc.direction, -22f*npc.scale), new Rectangle?(rect2), color, armRot2, vector13, npc.scale, effects, 0f);
			return true;
		}
	}
}
