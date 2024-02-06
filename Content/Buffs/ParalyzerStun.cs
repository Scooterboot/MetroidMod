using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Buffs
{
	public class ParalyzerStun : ModBuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.BetsysCurse}";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stunned");
			// Description.SetDefault("How did you get this???");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC N, ref int buffIndex)
		{
			N.MetroidGlobal().stunned = true;
			N.MetroidGlobal().speedDecrease = 0;
		}
	}
}
