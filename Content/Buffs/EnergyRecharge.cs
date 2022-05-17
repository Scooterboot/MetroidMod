using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;

using MetroidModPorted.Common.Players;

namespace MetroidModPorted.Content.Buffs
{
	public class EnergyRecharge : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Recharging Life and Reserve Energy");
			Description.SetDefault("Using energy station, cant move");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}
		SoundEffectInstance soundInstance;
		bool soundPlayed = false;
		public override void Update(Player player, ref int buffIndex)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if ((player.statLife >= player.statLifeMax2 && mp.reserveHearts >= mp.reserveTanks && mp.Energy >= mp.MaxEnergy) || player.controlJump || player.controlUseItem)
			{
				if(soundInstance != null)
				{
					soundInstance.Stop(true);
				}
				soundPlayed = false;
				SoundEngine.PlaySound(SoundLoader.CustomSoundType, player.Center, SoundLoader.GetSoundSlot(Mod, "Assets/Sounds/MissilesReplenished"));
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
				if (mp.Energy < mp.MaxEnergy)
				{
					mp.Energy++;
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
					soundInstance = SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)player.Center.X, (int)player.Center.Y, SoundLoader.GetSoundSlot(Mod, "Assets/Sounds/ConcentrationLoop"));
					soundPlayed = true;
				}
			}
		}
	}
}
