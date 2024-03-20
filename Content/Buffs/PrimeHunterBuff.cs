using MetroidMod.Common.Players;
using MetroidMod.Content.DamageClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Buffs
{
	public class PrimeHunterBuff : ModBuff
	{
		private int Stinger = 0;
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndec)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			Stinger++;
			player.statDefense /= 2;
			if (mp.PrimeHunter && mp.ShouldShowArmorUI)
			{
				player.buffTime[buffIndec] = 2;
			}
			else
			{
				player.buffTime[buffIndec] = 0;
			}

			DamageClass damageClass = ModContent.GetInstance<HunterDamageClass>();
			player.GetDamage(damageClass) += 0.30f;
			player.GetCritChance(damageClass) += 15;
			player.GetArmorPenetration(damageClass) += 20;
			player.statDefense -= 20;
			//player.statLifeMax2 -= player.statLifeMax2 / 10;
			player.endurance -= 0.25f;
			//mp.PrimeHunter = true;
			player.aggro += 1000;
			player.jumpSpeedBoost += 2.4f;
			player.maxFallSpeed += 5f;
			player.runAcceleration *= 5f;
			player.runSlowdown *= 5f;
			player.accRunSpeed *= 5f;
			if(player.mount.Active && mp.morphBall)
			{
				player.thorns += 100f;
			}
			/*if (player.lifeRegen > 0)
			{
				player.lifeRegen = 0;
			}*/
			if(Stinger >= 15)
			{
				mp.Energy -= 5;
				Stinger = 0;
				if (mp.Energy <= 0 && (mp.SuitReserves <= 0 || !mp.SuitReservesAuto))
				{
					mp.Energy = 0;
					if (player.lifeRegen > 0)
					{
						player.lifeRegen = 0;
					}
					player.lifeRegenTime = 0;
					player.lifeRegen -= 20;
					if(player.statLife <= 0 && mp.reserveHearts <= 0)
					{
						player.KillMe(PlayerDeathReason.ByCustomReason("The Prime Hunter is dead!"), 0, 0);
					}
					if (!mp.IsPowerSuitBreastplate || !mp.IsPowerSuitGreaves || !mp.IsPowerSuitHelmet)
					{
						player.KillMe(PlayerDeathReason.ByCustomReason($"{player.name} did not find an exploit" ), 0, 0);
					}
				}
			}
			//player.lifeRegenTime = 15;
			//player.lifeRegen -= 5;
			int dustID = Dust.NewDust(player.position, player.width, player.height, DustID.Fireworks, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 100, new Color(), 0.5f);
			Main.dust[dustID].noGravity = true;
		}
	}
}
