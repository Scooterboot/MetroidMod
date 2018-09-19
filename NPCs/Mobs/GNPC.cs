using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AriaMod
{
    public class GNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            // 10% chance to drop the 'Cocoon' item from Mothron.
            if(npc.type == NPCID.Mothron && Main.rand.Next(10) == 0)
            {
                Item.NewItem(npc.position, mod.ItemType("Cocoon"));
            }
        }
    }
}
