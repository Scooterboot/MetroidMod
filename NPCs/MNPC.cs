using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.NPCs
{
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

