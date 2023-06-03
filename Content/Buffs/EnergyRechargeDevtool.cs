using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;

using MetroidMod.Common.Players;

namespace MetroidMod.Content.Buffs
{
	public class EnergyRechargeDevtool : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Recharging Life");
			// Description.SetDefault("Using rejuvination station, can't move");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}
		ReLogic.Utilities.SlotId soundInstance;
		bool soundPlayed = false;
		public override void Update(Player player, ref int buffIndex)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			// should reserve hearts (not reserve tanks, those are different)
			// still be recharged by this?
			//yes -Dr
			if ((player.statLife >= player.statLifeMax2 && mp.reserveHearts >= mp.reserveTanks) || player.controlJump || player.controlUseItem)
			{
				if(SoundEngine.TryGetActiveSound(soundInstance, out ActiveSound result))
				{
					result.Stop();
				}
				soundPlayed = false;
				SoundEngine.PlaySound(Sounds.Suit.MissilesReplenished, player.Center);
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				if (player.statLife < player.statLifeMax2)
				{
					player.statLife++;
				}
				if (mp.reserveHearts < mp.reserveTanks)
				{
					mp.reserveHearts++;
				}
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
				//Main.PlaySound(10, player.Center);
				if(!soundPlayed)
				{
					soundInstance = SoundEngine.PlaySound(Sounds.Suit.ConcentrationLoop, player.Center);
					soundPlayed = true;
				}
			}
		}
	}
}
