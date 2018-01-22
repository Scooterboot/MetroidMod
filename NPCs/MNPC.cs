using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs
{
	public class MNPC : GlobalNPC 
	{
		public override bool InstancePerEntity
		{
		    get
		    {
			return true;
		    }
		}
		public bool froze = false;
		public float speedDecrease = 0.8f;
		public bool oldGravity;
		public bool oldTileCollide;
		int oldDir = 1;
		bool start = false;
		public override void ResetEffects(NPC npc)
		{
		    froze = false;
		}
		public override bool PreAI(NPC npc)
		{
		    if (!start)
		    {
			oldGravity = npc.noGravity;
			oldTileCollide = npc.noTileCollide;
			start = true;
		    }
		    if (froze)
		    {
			if (speedDecrease <= 0 && npc.type != mod.NPCType("LarvalMetroid"))
			{
			    npc.damage = 0;
			    npc.frame.Y = 0;
			    npc.velocity.X = 0;
			    npc.noGravity = false;
			    npc.noTileCollide = false;
			    npc.direction = oldDir;
			    npc.spriteDirection = oldDir;
			    return false;
			}
			else
			{
			    oldDir = npc.direction;
			}
		    }
		    else
		    {
			speedDecrease = 0.8f;
			npc.noGravity = oldGravity;
			npc.noTileCollide = oldTileCollide;
		    }
		    return base.PreAI(npc);
		}
		public override void PostAI(NPC npc)
		{
		    base.PostAI(npc);
		    if (froze && speedDecrease > 0 && npc.type != mod.NPCType("LarvalMetroid"))
		    {
			npc.velocity.X = npc.velocity.X * speedDecrease;
			if (npc.noGravity)
			{
			    npc.velocity.Y = npc.velocity.Y * speedDecrease;
			}

		    }
		}
		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
		    if (froze)
		    {
			drawColor = Lighting.GetColor((int)npc.position.X / 16, (int)npc.position.Y / 16, new Color(0, 144, 255));
		    }
		}
		public override void NPCLoot(NPC npc)
		{
			Player player = Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)];
            		MPlayer mp = player.GetModPlayer<MPlayer>(mod);
			bool flag = false;
            		for(int i = 0; i < player.inventory.Length; i++)
			{
				if(player.inventory[i].type == mod.ItemType("MissileLauncher"))
				{
					flag = true;
				}
			}
			if(flag)
			{
				if (npc.type != 16 && npc.type != 81 && npc.type != 121 && npc.lifeMax > 1 && npc.damage > 0)
				{
					if (Main.rand.Next(5) <= 1)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MissilePickup"), 1 + Main.rand.Next(5));
					}
					else if (Main.rand.Next(10) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MissilePickup"), 10 + Main.rand.Next(16));
					}
				}
			}
        	}
	}
}

