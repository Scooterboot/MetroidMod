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
			if (N.GetGlobalNPC<NPCs.MGlobalNPC>().speedDecrease > 0)
				N.GetGlobalNPC<NPCs.MGlobalNPC>().speedDecrease -= 0.2f;
			else
				N.GetGlobalNPC<NPCs.MGlobalNPC>().speedDecrease = 0;

			int dustID = Dust.NewDust(N.position, N.width, N.height, 59, N.velocity.X * 0.2f, N.velocity.Y * 0.2f, 100, new Color(), 2f);

			return true;
		}

		public override void Update(NPC N, ref int buffIndex)
		{
			N.GetGlobalNPC<NPCs.MGlobalNPC>().froze = true;
		}
    }
}
