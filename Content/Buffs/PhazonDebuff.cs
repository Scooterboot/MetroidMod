using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Buffs
{
	public class PhazonDebuff : ModBuff
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
			player.lifeRegen -= 60;
			int dustID = Dust.NewDust(player.position, player.width, player.height, DustID.BlueCrystalShard, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 100, new Color(), 2f);
			Main.dust[dustID].noGravity = true;
		}
		public override void Update(NPC N, ref int buffIndec)
		{
			if (N.lifeRegen > 0)
			{
				N.lifeRegen = 0;
			}
			N.lifeRegen -= 20;
			int dustID = Dust.NewDust(N.position, N.width, N.height, DustID.BlueCrystalShard, N.velocity.X * 0.2f, N.velocity.Y * 0.2f, 100, new Color(), 2f);
			Main.dust[dustID].noGravity = true;
		}
	}
}
