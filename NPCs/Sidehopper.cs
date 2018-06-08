using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs
{

	public class Sidehopper : ModNPC
    {
		public bool spawn = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sidehopper");
			Main.npcFrameCount[npc.type] = 3;
		}
		public override void SetDefaults()
		{
			npc.width = 100;
			npc.height = 78;
			npc.aiStyle = -1;
			npc.damage = 10;
			npc.defense = 0;
			npc.lifeMax = 100;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 0, 1, 60);
			npc.knockBackResist = 0.75f;
			//banner = npc.type;
			//bannerItem = mod.ItemType("SidehopperBanner");
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0f;//(spawnInfo.player.ZoneCorrupt || spawnInfo.player.ZoneCrimson) && NPC.downedBoss2 ? 0.03f : 0f;
		}
		public override bool PreAI()
		{
			if (!spawn)
			{
				npc.scale = (Main.rand.Next(6, 11) * 0.1f);
				npc.defense = (int)((float)npc.defense * npc.scale);
				npc.damage = (int)((float)npc.damage * npc.scale);
				npc.life = (int)((float)npc.life * npc.scale);
				npc.lifeMax = npc.life;
				npc.value = (float)((int)(npc.value * npc.scale));
				npc.npcSlots *= npc.scale;
				npc.knockBackResist *= 2f - npc.scale;
				spawn = true;
			}
			return true;
		}
		public override void AI()
		{
			if(npc.ai[1] == 1f)
			{
				npc.direction = Math.Sign(Main.player[npc.target].position.X - npc.position.X);
				if(npc.velocity.Y == 0f)
				{
					npc.velocity.X *= 0.1f;
					if(Math.Abs(npc.velocity.X) < 1f)
					{
						npc.velocity.X = 0f;
					}
				}
				npc.ai[0]++;
				if(npc.ai[0] > 40)
				{
					if(Main.rand.Next(2) == 0)
					{
						npc.velocity.Y = -8f;
						npc.velocity.X += (float)(3 * npc.direction);
					}
					else
					{
						npc.velocity.Y = -6f;
						npc.velocity.X += (float)(2 * npc.direction);
					}
					if(npc.type == mod.NPCType("UpsideDownSidehopper"))
					{
						npc.velocity.Y *= -1;
					}
					npc.ai[1] = 0f;
				}
				if(npc.ai[0] < 30)
				{
					if(npc.frameCounter > 0)
					{
						npc.frameCounter--;
					}
				}
				else
				{
					npc.frameCounter = 10;
				}
				if(npc.frameCounter > 0)
				{
					npc.frame.Y = 1;
				}
				else
				{
					npc.frame.Y = 0;
				}
			}
			else
			{
				if(npc.velocity.Y == 0f)
				{
					npc.ai[1] = 1f;
					npc.TargetClosest(true);
				}
				npc.ai[0] = 0f;
				npc.frameCounter = 10;
				npc.frame.Y = 2;
				if(npc.velocity.X != 0f && Collision.SolidCollision(npc.position+new Vector2(npc.velocity.X,0f), npc.width, npc.height) && !Collision.SolidCollision(npc.position, npc.width, npc.height))
				{
					npc.velocity.X *= -1f;
				}
			}
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && Main.netMode != 2)
			{
				/*Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("MetroidGore1"), npc.scale);
				Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("MetroidGore1"), npc.scale);
				Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("MetroidGore2"), npc.scale);
				Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("MetroidGore2"), npc.scale);*/
			}
		}
		
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			Texture2D tex = mod.GetTexture("NPCs/Sidehopper");
			Color color = npc.GetAlpha(Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16));
			int height = tex.Height / Main.npcFrameCount[npc.type];
			
			sb.Draw(tex, new Vector2(npc.Center.X, npc.position.Y+npc.height) - Main.screenPosition, new Rectangle?(new Rectangle(0,npc.frame.Y*height,tex.Width,height)),color,0f,new Vector2(tex.Width/2,height - 2),npc.scale,SpriteEffects.None,0f);
			
			return false;
		}
    }
	public class UpsideDownSidehopper : Sidehopper
	{
		public override string Texture
		{
			get
			{
				return (base.GetType().Namespace + "." + "Sidehopper").Replace('.', '/');
			}
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.noGravity = true;
		}
		public override void PostAI()
		{
			float maxFallSpeed = 10f;
			float gravity = 0.3f;
			
			float num = (float)(Main.maxTilesX / 4200);
			num *= num;
			float num2 = (float)((double)(npc.position.Y / 16f - (60f + 10f * num)) / (Main.worldSurface / 6.0));
			if ((double)num2 < 0.25)
			{
				num2 = 0.25f;
			}
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			gravity *= num2;
			if (npc.wet)
			{
				if (npc.honeyWet)
				{
					gravity = 0.1f;
					maxFallSpeed = 4f;
				}
				gravity = 0.2f;
				maxFallSpeed = 7f;
			}
			
			npc.velocity.Y -= gravity;
			if (npc.velocity.Y < -maxFallSpeed)
			{
				npc.velocity.Y = -maxFallSpeed;
			}
			if(Collision.SolidCollision(npc.position+new Vector2(0f,npc.velocity.Y), npc.width, npc.height))
			{
				if(npc.velocity.Y < 0f)
				{
					npc.velocity.Y = 0f;
				}
				if(npc.velocity.Y > 0f)
				{
					npc.velocity.Y = -0.01f;
				}
			}
		}
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			Texture2D tex = mod.GetTexture("NPCs/Sidehopper");
			Color color = npc.GetAlpha(Lighting.GetColor((int)npc.Center.X / 16, (int)npc.Center.Y / 16));
			int height = tex.Height / Main.npcFrameCount[npc.type];
			
			sb.Draw(tex, new Vector2(npc.Center.X, npc.position.Y) - Main.screenPosition, new Rectangle?(new Rectangle(0,npc.frame.Y*height,tex.Width,height)),color,0f,new Vector2(tex.Width/2,2),npc.scale,SpriteEffects.FlipVertically,0f);
			
			return false;
		}
	}
}
