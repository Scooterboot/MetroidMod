using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace MetroidModPorted.Content.Buffs
{
	public class IceFreeze : ModBuff
	{     
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Froze");
			Description.SetDefault("You Got Ice Beam'd!");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override bool ReApply(NPC N, int time, int buffIndex)
		{
			if (N.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().speedDecrease > 0)
				N.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().speedDecrease -= 0.2f;
			else
				N.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().speedDecrease = 0;

			int dustID = Dust.NewDust(N.position, N.width, N.height, DustID.BlueTorch, N.velocity.X * 0.2f, N.velocity.Y * 0.2f, 100, new Color(), 2f);

			return true;
		}

		public override void Update(NPC N, ref int buffIndex)
		{
			N.GetGlobalNPC<Common.GlobalNPCs.MGlobalNPC>().froze = true;
		}
	}
}
