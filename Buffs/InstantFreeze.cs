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
			bool isSkeletronArm = (N.aiStyle == 12 || N.aiStyle == 33 || N.aiStyle == 34 || N.aiStyle == 35 || N.aiStyle == 36);
			bool canFreeze = (!N.dontTakeDamage && !N.boss && N.lifeMax < 3000 && !isSkeletronArm &&	N.type != 143 && N.type != 144 && N.type != 145 && N.type != 146 && N.aiStyle != 6 && !N.buffImmune[44]);
			if(canFreeze)
			{
				N.GetGlobalNPC<NPCs.MNPC>(mod).froze = true;
				N.GetGlobalNPC<NPCs.MNPC>(mod).speedDecrease = 0;
			}
		}
    }
}
