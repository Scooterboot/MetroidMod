using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Buffs
{
	public class MochtroidSucc : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
		public override void Update(Player player, ref int buffIndec)
		{
			if (player.lifeRegen > 0)
			{
				player.lifeRegen = 0;
			}
			player.lifeRegenTime = 0;
			player.lifeRegen -= (Main.masterMode ? 80 : Main.expertMode ? 60 : 40) / (player.iceBarrier ? 2 : 1);
			player.dazed = true;
			//player.velocity *= 0.96f;
		}

	}
}
