using MetroidMod.Common.Players;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace MetroidMod.Content.Buffs
{
	public class EnergyRecharge : ModBuff
	{
		// temporary until an actual resprite is done for this
		public override string Texture => $"{Mod.Name}/Content/Buffs/EnergyRechargeDevtool";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Recharging Suit Energy and Suit Energy reserves");
			// Description.SetDefault("Using energy station, can't move");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}
		ReLogic.Utilities.SlotId soundInstance;
		bool soundPlayed = false;
		public override void Update(Player player, ref int buffIndex)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if ((/*player.statLife >= player.statLifeMax2 && mp.reserveHearts >= mp.reserveTanks && */mp.Energy >= mp.MaxEnergy) || player.controlJump || player.controlUseItem)
			{
				if (SoundEngine.TryGetActiveSound(soundInstance, out ActiveSound result))
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
				/*if (player.statLife < player.statLifeMax2)
				{
					player.statLife++;
				}*/
				if (mp.reserveHearts < mp.reserveTanks)
				{
					mp.reserveHearts++;
				}
				if (mp.Energy < mp.MaxEnergy)
				{
					mp.Energy += 3;
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
				if (!soundPlayed)
				{
					soundInstance = SoundEngine.PlaySound(Sounds.Suit.ConcentrationLoop, player.Center);
					soundPlayed = true;
				}
			}
		}
	}
}
