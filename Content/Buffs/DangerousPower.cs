using Terraria;
using Terraria.ModLoader;
using MetroidMod.Common.Players;
using Terraria.ID;

namespace MetroidMod.Content.Buffs
{
	public class DangerousPower : ModBuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Electrified}";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Power Surge");
			Description.SetDefault("Extreme power flows through your body, strengthening your attacks but weakening you...");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
		public override bool ReApply(Player player, int time, int buffIndex)
		{
			if (player.TryGetModPlayer(out MPlayer mp))
			{
				mp.dangerousPowerOverheat += 4.0f;
			}
			return false;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.TryGetModPlayer(out MPlayer mp))
			{
				mp.dangerousPowerOverheat += player.ItemAnimationActive ? 1.0f : -0.1f;
			}
		}
	}
}
