using MetroidMod.Content.Hatches;
using MetroidMod.Content.Hatches.Variants;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MetroidMod.Common.Systems
{
	internal class HatchSpecialBossSystem : ModSystem
	{
		private bool oldAnyBossAlive;
		private bool anyBossAlive;
		private bool bossWasKilled;

		public override void PreUpdateNPCs()
		{
			anyBossAlive = false;
		}
		public override void PostUpdateNPCs()
		{
			if (anyBossAlive != oldAnyBossAlive)
			{
				oldAnyBossAlive = anyBossAlive;
				foreach (TileEntity tileEntity in TileEntity.ByID.Values)
				{
					if (tileEntity is not HatchTileEntity hatch)
					{
						continue;
					}

					if (hatch.ModHatch is BlueHatch && hatch.State.BlueConversion != HatchBlueConversionStatus.Disabled)
					{
						// We can't turn a blue hatch blue! Activate secret boss functionality.
						DebugAssist.NewTextMP("Triggering boss hatch functionality");

						if (anyBossAlive)
						{
							hatch.State.LockStatus = HatchLockStatus.Locked;
						}
						else if (bossWasKilled)
						{
							hatch.State.LockStatus = HatchLockStatus.UnlockedAndBlinking;
						}
						else
						{
							hatch.State.LockStatus = HatchLockStatus.Unlocked;
						}

						hatch.SyncState();
					}
				}

				if (!anyBossAlive)
				{
					bossWasKilled = false;
				}
			}
		}
		
		private class BossHatchGlobalNPC : GlobalNPC
		{
			public override void AI(NPC npc)
			{
				if(npc.boss)
				{
					ModContent.GetInstance<HatchSpecialBossSystem>().anyBossAlive = true;
				}
			}

			public override void OnKill(NPC npc)
			{
				if (npc.boss)
				{
					ModContent.GetInstance<HatchSpecialBossSystem>().bossWasKilled = true;
				}
			}
		}
	}
}
