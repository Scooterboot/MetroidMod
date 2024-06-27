using MetroidMod.Common.GlobalItems;
using MetroidMod.Common.Players;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
//using MetroidMod.Content.Items;

namespace MetroidMod.Content.Buffs
{
	public class MissileRecharge : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Recharging Missiles");
			// Description.SetDefault("Using missile station, can't move");
			Main.debuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}
		ReLogic.Utilities.SlotId soundInstance;
		bool soundPlayed = false;
		int num = 0;
		int num3 = 0;
		public override void Update(Player player, ref int buffIndex)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < player.inventory.Length; i++)
			{
				if (player.inventory[i].type == ModContent.ItemType<Items.Weapons.MissileLauncher>() || player.inventory[i].type == ModContent.ItemType<Items.Weapons.PowerBeam>()||player.inventory[i].type == ModContent.ItemType<Items.Weapons.ArmCannon>())
				{
					MGlobalItem mi = player.inventory[i].GetGlobalItem<MGlobalItem>();
					flag = true;
					if (mi.statMissiles < mi.maxMissiles || mi.statUA < mi.maxUA)
					{
						if(mi.statMissiles < mi.maxMissiles)
						{
							mi.statMissiles++;
							num++;
							int num2 = num;
							while (num2 > 50)
							{
								mi.statMissiles++;
								num2 -= 50;
							}
						}
						if(mi.statUA < mi.maxUA)
						{
							mi.statUA++;
							num3++;
							int num4 = num3;
							while (num4 > 50)
							{
								mi.statUA++;
								num4 -= 50;
							}
						}
						break;
					}
					else //if (mi.statMissiles >= mi.maxMissiles)
					{
						//Main.PlaySound(SoundLoader.customSoundType, player.Center, mod.GetSoundSlot(SoundType.Custom, "Sounds/MissilesReplenished"));
						flag = false;
					}
				}
			}
			if (!flag && !flag2 || player.controlJump || player.controlUseItem)
			{
				if (SoundEngine.TryGetActiveSound(soundInstance, out ActiveSound result))
				{
					result.Stop();
				}
				soundPlayed = false;
				if (!flag)
				{
					SoundEngine.PlaySound(Sounds.Suit.MissilesReplenished, player.Center);
				}
				num = 0;
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
