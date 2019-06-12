using Terraria.ModLoader;
using Terraria;
using MetroidMod.Items;

namespace MetroidMod.Buffs
{
    public class MissileRecharge : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Recharging Missiles");
			Description.SetDefault("Using missile station, cant move");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}
		public override void Update(Player player,ref int buffIndex)
        {
            MPlayer mp = player.GetModPlayer<MPlayer>(mod);
            bool flag = false;
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].type == mod.ItemType("MissileLauncher"))
                {
                    MGlobalItem mi = player.inventory[i].GetGlobalItem<MGlobalItem>(mod);
                    mi.statMissiles++;
                    flag = true;
                    if (mi.statMissiles >= mi.maxMissiles)
                    {
                        Main.PlaySound(SoundLoader.customSoundType, player.Center, mod.GetSoundSlot(SoundType.Custom, "Sounds/MissilesReplenished"));
                        flag = false;
                    }
                }
            }
            if (!flag || player.controlJump || player.controlUseItem)
            {
                player.DelBuff(buffIndex);
                buffIndex--;           }
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
