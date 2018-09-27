using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria.ModLoader;
using Terraria;

namespace MetroidMod.Buffs
{
    public class InstantFreeze : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Froze");
			Description.SetDefault("'Can't move...'");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC N,ref int buffIndex)
		{
			N.GetGlobalNPC<NPCs.MNPC>(mod).froze = true;
			N.GetGlobalNPC<NPCs.MNPC>(mod).speedDecrease = 0;
		}
    }
}
