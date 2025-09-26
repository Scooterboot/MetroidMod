using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Buffs
{
	public class MetroidSucc : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phazon!");
			// Description.SetDefault("Phazon is draining your life away!");
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
			player.lifeRegen -= Main.masterMode ? 160 : Main.expertMode ? 120 : 80;
			player.dazed = true;
			player.velocity *= 0.95f;
		}
		public override void Update(NPC N, ref int buffIndec)
		{
			if (N.lifeRegen > 0)
			{
				N.lifeRegen = 0;
			}
			N.lifeRegen -= 240;
			if (N.knockBackResist > 0)
			{
				N.velocity.X *= 0.5f;
				if (N.noGravity)
				{
					N.velocity.Y *= 0.5f;
				}
				else
				{
					N.velocity.Y *= 0.75f;
				}
			}
		}
	}
}
