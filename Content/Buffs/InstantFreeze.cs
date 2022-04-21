using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria.ModLoader;
using Terraria;

namespace MetroidModPorted.Content.Buffs
{
    public class InstantFreeze : ModBuff
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Froze");
			Description.SetDefault("'Can't move...'");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC N,ref int buffIndex)
		{
			N.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().froze = true;
			N.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().speedDecrease = 0;
		}
    }
}
