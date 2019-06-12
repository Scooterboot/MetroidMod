using Terraria.ModLoader;
using Terraria;

namespace MetroidMod.Buffs
{
    public class EnergyRecharge : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Recharging Life");
			Description.SetDefault("Using energy station, cant move");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}
		public override void Update(Player player,ref int buffIndex)
        {
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            bool flag = true;
            player.statLife++;
            if (player.statLife >= player.statLifeMax)
            {
                flag = false;
                Main.PlaySound(SoundLoader.customSoundType, player.Center, mod.GetSoundSlot(SoundType.Custom, "Sounds/MissilesReplenished"));
            }
            if (!flag || player.controlJump || player.controlUseItem)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 2;
                player.controlLeft = false;
                player.controlRight = false;
                player.controlUp = false;
                player.controlDown = false;
                player.controlUseTile = false;
                player.velocity.X *= 0;
                if (player.velocity.Y < 0)
                {
                    player.velocity.Y *= 0;
                }
                player.mount.Dismount(player);
                Main.PlaySound(10, player.Center);
            }
        }
    }
}
