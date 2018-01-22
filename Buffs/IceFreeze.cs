using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria.ModLoader;
using Terraria;

namespace MetroidMod.Buffs
{
    public class IceFreeze : ModBuff
    {     
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Froze");
			Description.SetDefault("You Got Ice Beam'd!");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
		public override bool ReApply(NPC N, int time, int buffIndex)
		{
			isSkeletronArm = (N.aiStyle == 12 || N.aiStyle == 33 || N.aiStyle == 34 || N.aiStyle == 35 || N.aiStyle == 36);
			canFreeze = (!N.dontTakeDamage && !N.boss && N.lifeMax < 3000 && !isSkeletronArm &&	N.type != 143 && N.type != 144 && N.type != 145 && N.type != 146 && N.aiStyle != 6 && !N.buffImmune[44]);
			if(canFreeze)
			{
				if( N.GetGlobalNPC<NPCs.MNPC>(mod).speedDecrease > 0)
				{
					 N.GetGlobalNPC<NPCs.MNPC>(mod).speedDecrease -= 0.2f;
				}
				else
				{
					 N.GetGlobalNPC<NPCs.MNPC>(mod).speedDecrease = 0;
				}
				int dustID = Dust.NewDust(N.position, N.width, N.height, 59, N.velocity.X * 0.2f, N.velocity.Y * 0.2f, 100, new Color(), 2f);
			}
			if(!canFreeze)
			{
				N.buffImmune[mod.BuffType("MetroidMod:IceFreeze")] = true;
				N.buffImmune[mod.BuffType("MetroidMod:InstantFreeze")] = true;
			}
			return false;
		}
		public override void Update(NPC N,ref int buffIndex)
		{
			isSkeletronArm = (N.aiStyle == 12 || N.aiStyle == 33 || N.aiStyle == 34 || N.aiStyle == 35 || N.aiStyle == 36);
			canFreeze = (!N.dontTakeDamage && !N.boss && N.lifeMax < 3000 && !isSkeletronArm &&	N.type != 143 && N.type != 144 && N.type != 145 && N.type != 146 && N.aiStyle != 6 && !N.buffImmune[44]);
			if(canFreeze)
			{
				N.GetGlobalNPC<NPCs.MNPC>(mod).froze = true;
			}
		}
    }
}
