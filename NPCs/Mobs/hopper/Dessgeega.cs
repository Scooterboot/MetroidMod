using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs.Mobs.hopper
{
    public class Dessgeega : MNPC
    {
        private bool spawn = false;
		private float newScale = -1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dessgeega");
			Main.npcFrameCount[npc.type] = 3;
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return (spawnInfo.spawnTileY > WorldGen.lavaLine ? SpawnCondition.Cavern.Chance * 0.05f : 0) + SpawnCondition.Underworld.Chance * 0.1f;
		}
		
		public override void SetDefaults()
		{
			npc.width = 60;
			npc.height = 50;
			npc.aiStyle = -1;
			npc.damage = 30;
			npc.defense = 16;
			npc.lifeMax = 70;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 0, 3, 50);
			npc.knockBackResist = 0.35f;
			//banner = npc.type;
			//bannerItem = mod.ItemType("DessgeegaBanner");
			npc.noGravity = true;
			npc.behindTiles = true;
			npc.lavaImmune = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;

			/* NPC scale networking fix. */
			if (Main.rand != null && Main.netMode != NetmodeID.MultiplayerClient)
				newScale = (Main.rand.Next(9, 12) * 0.1f);
		}
		private void SetStats()
		{
			npc.scale = newScale;
			npc.position.X += (float)(npc.width / 2);
			npc.position.Y += (float)(npc.height);
			npc.width = (int)((float)npc.width * npc.scale);
			npc.height = (int)((float)npc.height * npc.scale);
			npc.position.X -= (float)(npc.width / 2);
			npc.position.Y -= (float)(npc.height);
			npc.defense = (int)((float)npc.defense * npc.scale);
			npc.damage = (int)((float)npc.damage * npc.scale);
			npc.life = (int)((float)npc.life * npc.scale);
			npc.lifeMax = npc.life;
			npc.value = (float)((int)(npc.value * npc.scale));
			npc.npcSlots *= npc.scale;
			npc.knockBackResist *= 2f - npc.scale;
		}
		
		public override bool PreAI()
		{
			if (!spawn && newScale != -1)
			{
				SetStats();
				spawn = true;
				npc.netUpdate = true;
			}
			return true;
		}

		public override void AI()
		{
			mNPC.HopperAI(npc, 5f, 8f);
			
			if(npc.ai[1] == 1f)
			{
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
				npc.frameCounter = 10;
				npc.frame.Y = 2;
			}
		}
		public override bool PreDraw(SpriteBatch sb, Color drawColor)
		{
			mNPC.DrawHopper(npc,sb);
			return false;
		}
		
		Vector2 RandomVel => new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f;
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && Main.netMode != 2)
			{
				for(int i = 0; i < 3; i++)
				{
					Gore gore = Gore.NewGoreDirect(npc.Center, RandomVel, mod.GetGoreSlot("Gores/Mobs/DessgeegaGore"+i), npc.scale);
					gore.position -= new Vector2(Main.goreTexture[gore.type].Width,Main.goreTexture[gore.type].Height) / 2;
					gore.timeLeft = 60;
				}
				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 6, RandomVel.X, RandomVel.Y, 100, default(Color), 1.5f*npc.scale);
					dust.noGravity = false;
				}
			}
		}
    }
	public class Dessgeega_Large : Dessgeega
	{
		private bool spawn = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Large Dessgeega");
			Main.npcFrameCount[npc.type] = 3;
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if(Main.hardMode)
			{
				return SpawnCondition.Cavern.Chance * 0.25f + SpawnCondition.Underworld.Chance * 0.5f;
			}
			return 0f;
		}
		
		public override void SetDefaults()
		{
			npc.width = 96;
			npc.height = 76;
			npc.aiStyle = -1;
			npc.damage = 60;
			npc.defense = 40;
			npc.lifeMax = 600;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 0, 12, 0);
			npc.knockBackResist = 0.1f;
			//banner = npc.type;
			//bannerItem = mod.ItemType("DessgeegaLargeBanner");
			npc.noGravity = true;
			npc.behindTiles = true;
			npc.lavaImmune = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
		}
		
		public override bool PreAI()
		{
			if (!spawn)
			{
				spawn = true;
				npc.netUpdate = true;
			}
			return true;
		}
		
		Vector2 RandomVel => new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f) * .4f;
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && Main.netMode != 2)
			{
				for(int i = 0; i < 3; i++)
				{
					Gore gore = Gore.NewGoreDirect(npc.Center, RandomVel, mod.GetGoreSlot("Gores/Mobs/DessgeegaLargeGore"+i), npc.scale);
					gore.position -= new Vector2(Main.goreTexture[gore.type].Width,Main.goreTexture[gore.type].Height) / 2;
					gore.timeLeft = 60;
				}
				for (int i = 0; i < 15; i++)
				{
					Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 6, RandomVel.X, RandomVel.Y, 100, default(Color), 2f);
					dust.noGravity = false;
				}
			}
		}
	}
}
